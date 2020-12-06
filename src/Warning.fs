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
            sprintf "There are %d tracks whose lyrics have carriage returns"

        Console.WriteLine(mesg, Color.Yellow)

// for track in tracks do
//     printfn "%s" track.Title
