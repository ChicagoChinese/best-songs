module Warning

open System.IO
open System.Drawing
open Prelude
open Colorful

module H = Hanzidentifier

module Template =
    open Giraffe.ViewEngine

    let lyricsReport = "lyrics_report.html"

    let convert = HanziConv.toSimplified

    let generateLyricsReport (tracks: Track.T array) =
        let getSimplifiedLines text =
            let simplified = convert text
            [| for line in simplified.Split("\r") -> (H.identify line = H.Mixed, line) |]

        html [] [
            head [] [
                meta [ _charset "utf-8" ]
                style [] [
                    str """
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
                    yield hr []
            ]
        ]
        |> RenderView.AsString.htmlDocument
        |> fun output -> File.WriteAllText(lyricsReport, output)
        printfn "\nGenerated %s" lyricsReport

let checkCarriageReturns (tracks: Track.T array) =
    let tracks =
        [| for track in tracks do
            if track.Lyrics.Contains("\r\n") then track |]

    match tracks.Length with
    | 0 -> Console.WriteLine("No carriage returns detected!", Color.Green)
    | n ->
        let mesg =
            sprintf "\nThere are %d tracks whose lyrics have carriage returns:" n

        Console.WriteLine(mesg, Color.Yellow)
        for track in tracks do
            printfn "- %s  %s" track.Title track.Artist
        printfn ""

let checkBadLinks (tracks: Track.T array) =
    let tracks =
        [| for track in tracks do
            match track.Link with
            | Link.YouTube _ -> ()
            | _ -> yield track |]

    match tracks.Length with
    | 0 -> Console.WriteLine("All tracks have good youtube links!")
    | n ->
        let mesg =
            sprintf "\nThere are %d tracks with bad links:" n

        Console.WriteLine(mesg, Color.Yellow)
        for track in tracks do
            printfn "- %s  %s -> %s" track.Title track.Artist (Link.show track.Link)
        printfn ""

let checkTraditionalTracks (tracks: Track.T array) =
    let tracks =
        [| for track in tracks do
            match H.identify track.Lyrics with
            | H.Traditional
            | H.Mixed -> yield track
            | _ -> () |]

    match tracks.Length with
    | 0 -> Console.WriteLine("All tracks have simplified lyrics!", Color.Green)
    | n ->
        let mesg =
            sprintf "There are %d tracks with traditional lyrics:" n

        Console.WriteLine(mesg, Color.Yellow)
        for track in tracks do
            printfn "- %s  %s" track.Title track.Artist
        Template.generateLyricsReport tracks

let main () =
    let tracks = Track.readTracksFromFile ()

    checkCarriageReturns tracks
    checkBadLinks tracks
    checkTraditionalTracks tracks
