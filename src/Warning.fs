module Warning

open System.IO
open System.Drawing
open Prelude
open Colorful

module H = Hanzidentifier

module Template =
    open Giraffe.ViewEngine

    let lyricsReport = "lyrics_report.html"

    let generateLyricsReport (tracks: Track.T array) =
        let getSimplifiedLines (track: Track.T) =
            let text =
                sprintf "%s\n%s\n%s" track.Title track.Artist track.Lyrics

            let simplified = HanziConv.toSimplified text
            let lines = simplified.Split("\n")
            (lines.[0], lines.[1], lines.[2..])

        html [] [
            head [] [ meta [ _charset "utf-8" ] ]
            body [] [
                ol [] [
                    for track in tracks do
                        let (title, artist, lines) = getSimplifiedLines track
                        yield h1 [] [ str title ]
                        yield h2 [] [ str artist ]
                        yield!
                            [ for line in lines do
                                yield str line
                                yield br [] ]
                        yield hr []
                ]
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
            sprintf "\nThere are %d tracks with traditional lyrics:" n

        Console.WriteLine(mesg, Color.Yellow)
        for track in tracks do
            printfn "- %s  %s" track.Title track.Artist
        Template.generateLyricsReport tracks

let main () =
    let tracks = Track.readTracksFromFile ()

    checkCarriageReturns tracks
    checkBadLinks tracks
    checkTraditionalTracks tracks
