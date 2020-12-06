module Warning

open System.Drawing
open Prelude
open Colorful

let filename = "lyrics_report.html"

let main () =
    let tracks = Track.readTracksFromFile ()

    let crTracks =
        [| for track in tracks do
            if track.Lyrics.Contains('\r') then track |]

    match crTracks.Length with
    | 0 -> Console.WriteLine("No carriage returns detected!", Color.Green)
    | n ->
        let mesg =
            sprintf "\nThere are %d tracks whose lyrics have carriage returns"

        Console.WriteLine(mesg, Color.Yellow)

    let badLinkTracks =
        [| for track in tracks do
            match track.Link with
            | Link.YouTube _ -> ()
            | _ -> yield track |]

    match badLinkTracks.Length with
    | 0 -> Console.WriteLine("All tracks have good youtube links!")
    | n ->
        let mesg =
            sprintf "\nThere are %d tracks with bad links:" n

        Console.WriteLine(mesg, Color.Yellow)
        for track in badLinkTracks do
            printfn "- %s by %s -> %s" track.Title track.Artist (Link.show track.Link)
