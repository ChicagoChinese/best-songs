module Dump

open System.IO
open FSharp.Data
open Prelude

type FfprobeMeta =
    JsonProvider<"""
{
  "format": {
    "tags": {
      "title": "STRING",
      "artist": "STRING",
      "genre": "STRING",
      "lyrics": "STRING",
      "comment": "https://example.com/hello/"
    }
  }
}""">

let getTrackLocations playlistName =
    let output =
        getCommandOutput "swift" [ "track_paths.swift"; playlistName ]

    output.Split "\n"

let getTrackMetaJson location =
    getCommandOutput
        "ffprobe"
        [ location
          "-print_format"
          "json"
          "-show_format" ]

let getTrackMeta location =
    getTrackMetaJson location |> FfprobeMeta.Parse

let parseLink (text: string) =
    let lines = text.Split "\n"
    match lines.Length with
    | 0 -> Link.None
    | _ ->
        match lines
              |> Array.tryFind (fun line -> line.StartsWith "https://youtu.be") with
        | Some link -> Link.YouTube link
        | None ->
            match lines
                  |> Array.tryFind (fun line -> line.StartsWith "https://www.youtube.com") with
            | Some link -> Link.YouTubeLong link
            | None -> Link.Other(lines |> Array.tryHead |> Option.defaultValue "")

let getTrack location =
    let meta = getTrackMeta location
    let tags = meta.Format.Tags
    { Location = location
      Title = tags.Title
      Artist = tags.Artist
      Genre = tags.Genre
      Lyrics = tags.Lyrics
      Link = parseLink tags.Comment }: Track.T

let main playlistName =
    printfn "Dumping track metadata for playlist '%s'\n" playlistName
    getTrackLocations playlistName
    |> Array.map getTrack
    |> Track.writeTracksToFile

    printfn "Generated %s" Track.filename
