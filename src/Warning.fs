module Warning

open System.IO
open System.Drawing
open Prelude
open Colorful

module H = Hanzidentifier

let checkExcessivNewlines (tracks: Track.T array) =
    let tracks =
        [| for track in tracks do
            if track.Lyrics.Contains("\n\n\n") then track |]

    match tracks.Length with
    | 0 -> Console.WriteLine("No excessive newlines detected!", Color.Green)
    | n ->
        Console.WriteLine($"\nThere are {n} tracks whose lyrics have excessive newlines:", Color.Yellow)

        for track in tracks do
            printfn $"- {track.Title}  {track.Artist}"

        printfn ""

let getBadLinkTracks (tracks: Track.T array) =
    [| for track in tracks do
        match track.Link with
        | Link.YouTube _ -> ()
        | _ -> yield track |]

let checkBadLinks (tracks: Track.T array) =
    let tracks = getBadLinkTracks tracks

    match tracks.Length with
    | 0 -> Console.WriteLine("All tracks have good youtube links!")
    | n ->
        Console.WriteLine($"\nThere are {n} tracks with bad links:", Color.Yellow)

        for track in tracks do
            printfn $"- {track.Title}  {track.Artist} -> {Link.show track.Link}"

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
        Console.WriteLine($"There are {n} tracks with traditional lyrics:", Color.Yellow)

        for track in tracks do
            printfn $"- {track.Title}  {track.Artist}"

        LyricsReport.generate tracks false

let main () =
    let tracks = (Playlist.readFromFile ()).Tracks

    checkExcessivNewlines tracks
    checkBadLinks tracks
    checkTraditionalTracks tracks
