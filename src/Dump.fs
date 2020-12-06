module Dump

open Fake.Core

let getTrackLocations playlistName =
  let result =
    CreateProcess.fromRawCommand "swift" ["track_paths.swift"; playlistName]
    |> CreateProcess.redirectOutput
    |> CreateProcess.ensureExitCode
    |> Proc.run
  result.Result.Output.Split "\n"

let main playlistName =
  printfn "Dumping track metadata for playlist '%s'\n" playlistName
  printfn "%A" (getTrackLocations playlistName)
