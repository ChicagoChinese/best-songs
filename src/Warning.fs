module Warning

open System.Drawing
open Prelude
open Colorful

module H = Hanzidentifier

let filename = "lyrics_report.html"

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

let main () =
    let tracks = Track.readTracksFromFile ()

    checkCarriageReturns tracks
    checkBadLinks tracks
    checkTraditionalTracks tracks
