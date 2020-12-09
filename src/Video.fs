module Video

open System.IO
open Fake.Core
open Prelude

// https://github.com/feihong/music-tools/blob/master/tasks/convert.py#L47

let assetsDir = "assets"

let extractImage trackFile =
    CreateProcess.fromRawCommand "AtomicParsley" [ trackFile; "--extractPix" ]
    |> CreateProcess.redirectOutput
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

    let imageFile =
        Path.Combine
            (assetsDir,
             Path.GetFileNameWithoutExtension(trackFile)
             + "_artwork_1.jpg")

    if File.Exists(imageFile) then Some imageFile else None

let main limit =
    let tracks =
        Track.readTracksFromFile ()
        |> Warning.getBadLinkTracks

    let tracks = tracks.[0..(limit - 1)]
    for track in tracks do
        printfn "%s" track.Title

        let destFile =
            Path.Combine(assetsDir, Path.GetFileName(track.Location))

        File.Copy(track.Location, destFile, true)

        printfn "%A" (extractImage destFile)
