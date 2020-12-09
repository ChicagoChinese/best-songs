module Video

open Prelude

let main limit =
    let tracks =
        Track.readTracksFromFile ()
        |> Warning.getBadLinkTracks

    let tracks = tracks.[0..(limit - 1)]
    for track in tracks do
        printfn "%s" track.Title
