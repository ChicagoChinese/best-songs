module Zip

open Fake.IO
open Prelude

let tracksZipFile = "tracks.zip"

let main () =
    let files =
        [ for track in Track.readTracksFromFile () -> track.Location ]

    let files = files.[0..49]

    Zip.createZip
        "."  // working directory
        tracksZipFile  // zip file name
        ""  // ignored comment
        0  // compression level
        true  // all files are put at root level
        files // sequence of file paths

    printfn "Generated %s" tracksZipFile
