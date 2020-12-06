module Dump

open System.IO
open FSharp.Data
open Prelude

type FfprobeMeta =
    JsonProvider<"""
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
    | 0 -> Track.None
    | _ ->
        match lines
              |> Array.tryFind (fun line -> line.StartsWith "https://youtu.be") with
        | Some link -> Track.YouTube link
        | None ->
            match lines
                  |> Array.tryFind (fun line -> line.StartsWith "https://www.youtube.com") with
            | Some link -> Track.YouTubeLong link
            | None -> Track.Other(lines |> Array.tryHead |> Option.defaultValue "")

let getTrack location =
    let meta = getTrackMeta location
    let tags = meta.Format.Tags
    { location = location
      title = tags.Title.ToString()
      artist = tags.Artist.ToString()
      genre = tags.Genre.ToString()
      lyrics = tags.Lyrics.ToString()
      link = parseLink (tags.Comment.ToString()) }: Track.T

let main playlistName =
    printfn "Dumping track metadata for playlist '%s'\n" playlistName
    getTrackLocations playlistName
    |> Array.map getTrack
    |> Track.writeTracksToFile

    printfn "Generated %s" Track.filename
