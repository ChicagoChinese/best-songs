module Video

open System.IO
open Fake.Core
open Prelude

// https://github.com/feihong/music-tools/blob/master/tasks/convert.py#L47

let assetsDir = "assets"

// Don't redirect output so we can see progress of conversion
let runCommand command args =
    CreateProcess.fromRawCommand command args
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

let extractImage (trackFile: string) =
    let getImageFile ext =
        let path =
            Path.Combine
                (assetsDir,
                 Path.GetFileNameWithoutExtension(trackFile)
                 + "_artwork_1"
                 + ext)

        if File.Exists(path) then Some path else None

    getCommandOutput "AtomicParsley" [ trackFile; "--extractPix" ]
    |> ignore

    match ([ getImageFile ".jpg"
             getImageFile ".png" ]
           |> List.filter Option.isSome) with
    | [] -> None
    | hd :: _ -> hd

let convertToMp4 audioFile imageFile =
    let videoFile = Path.ChangeExtension(audioFile, ".mp4")
    runCommand
        "ffmpeg"
        [ // overwrite existing file
          "-y"
          // loop image infinitely
          "-loop"
          "1"
          // image file
          "-i"
          imageFile
          // audio file
          "-i"
          audioFile
          // don't re-encode audio
          "-c:a"
          "copy"
          // use x264 to encode video
          "-c:v"
          "libx264"
          // set constant rate factor to medium (0-51, 0 is lossless)
          "-crf"
          "20"
          // if not provided, will use yuv444p, which is not as widely supported
          "-pix_fmt"
          "yuv420p"
          // needed to finish encoding after the audio stream finishes
          "-shortest"
          videoFile ]
    |> ignore
    printfn "Generated %s" videoFile

let main limit =
    let tracks =
        Track.readTracksFromFile ()
        |> Warning.getBadLinkTracks

    let tracks = tracks.[0..(limit - 1)]
    for track in tracks do
        printfn "%s" track.Title

        let audioFile =
            Path.Combine(assetsDir, Path.GetFileName(track.Location))

        File.Copy(track.Location, audioFile, true)

        match extractImage audioFile with
        | None -> printfn "Failed to extract image from %s" audioFile
        | Some imageFile -> convertToMp4 audioFile imageFile
