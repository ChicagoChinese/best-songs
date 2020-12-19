module Site

open Prelude

let main () =
    for playlist in Playlist.getPlaylists () do
        printfn $"{playlist.Name}"
