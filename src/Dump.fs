module Dump

open FSharp.Data
open Prelude

type FfprobeMeta = JsonProvider<"""
{
  "format": {
    "tags": {
      "title": "",
      "artist": "",
      "genre": "",
      "lyrics": "",
      "comment": "https://example.com/hello/"
    }
  }
}""">

let getTrackLocations playlistName =
  let output = getCommandOutput "swift" ["track_paths.swift"; playlistName]
  output.Split "\n"

let getTrackMetaJson location =
  getCommandOutput "ffprobe" [location; "-print_format"; "json"; "-show_format"]
  |> FfprobeMeta.Parse
  |> fun obj -> obj.Format.Tags

let main playlistName =
  printfn "Dumping track metadata for playlist '%s'\n" playlistName
  // printfn "%A" (getTrackLocations playlistName)
  let tracks =
    getTrackLocations playlistName
    |> Array.map getTrackMetaJson
  for track in tracks do
    printfn "%A" track
