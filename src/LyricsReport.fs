module LyricsReport

open System.IO
open Giraffe.ViewEngine
open Prelude

module H = Hanzidentifier

let lyricsReport = "lyrics_report.html"

let disclaimer =
    "【影片使用的照片及文字和音乐版权归唱片公司和歌手所有，如侵犯您的权益，请通知我，我将马上删除。】"


// Return attributes and body for simplified text
let makeHighlightedElement tag text =
    let newText = HanziConv.toSimplified text

    let isMixed =
        match H.identify newText with
        | H.Mixed
        | H.Traditional -> true
        | _ -> false

    let attributes =
        if isMixed then [ _class "highlighted" ] else []

    tag attributes [ str newText ]

let generate (tracks: Track.T array) includeDisclaimer =
    html [] [
        head [] [
            meta [ _charset "utf-8" ]
            style [] [
                str
                    """
                    .highlighted {
                      background-color: palegoldenrod;
                    }
                    """
            ]
        ]
        body [] [
            for track in tracks do
                yield makeHighlightedElement h1 track.Title
                yield makeHighlightedElement h1 track.Artist

                yield!
                    [ for line in track.Lyrics.Split("\n") do
                        yield makeHighlightedElement span line
                        yield br [] ]

                yield! if includeDisclaimer then [ br []; str disclaimer ] else []
                yield hr []
        ]
    ]
    |> RenderView.AsString.htmlDocument
    |> fun output -> File.WriteAllText(lyricsReport, output)

    printfn $"\nGenerated {lyricsReport}"
