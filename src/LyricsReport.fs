module LyricsReport

open System.IO
open Giraffe.ViewEngine
open Prelude

module H = Hanzidentifier

let lyricsReport = "lyrics_report.html"

let disclaimer =
    "【影片使用的照片及文字和音乐版权归唱片公司和歌手所有，如侵犯您的权益，请通知我，我将马上删除。】"

let convert = HanziConv.toSimplified

let generate (tracks: Track.T array) includeDisclaimer =
    let getSimplifiedLines text =
        let simplified = convert text
        [| for line in simplified.Split("\n") -> (H.identify line = H.Mixed, line) |]

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
                yield h1 [] [ str (convert track.Title) ]
                yield h2 [] [ str (convert track.Artist) ]

                yield!
                    [ for (isMixed, line) in (getSimplifiedLines track.Lyrics) do
                        yield span (if isMixed then [ _class "highlighted" ] else []) [ str line ]

                        yield br [] ]

                yield! if includeDisclaimer then [ br []; str disclaimer ] else []
                yield hr []
        ]
    ]
    |> RenderView.AsString.htmlDocument
    |> fun output -> File.WriteAllText(lyricsReport, output)

    printfn $"\nGenerated {lyricsReport}"
