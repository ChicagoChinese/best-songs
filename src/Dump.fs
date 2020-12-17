module Dump

open System.IO
open FSharp.Data
open Prelude

type SwiftMeta =
    JsonProvider<"""
{
  "name": "STRING",
  "tracks": [
     {
      "name": "STRING",
      "artist": "STRING",
      "genre": "STRING",
      "lyrics": "STRING",
      "comment": "STRING",
      "location": "STRING"
    }
  ]
}""">

let getRawPlaylist playlistName =
    getCommandOutput "swift" [ "dump_playlist.swift"; playlistName ]
    |> SwiftMeta.Parse

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

let getPlaylist playlistName =
    let p = getRawPlaylist playlistName
    { Name = p.Name
      Tracks =
          p.Tracks
          |> Array.map (fun t ->
              { Location = t.Location
                Title = t.Name
                Artist = t.Artist
                Genre = t.Genre
                Lyrics = t.Lyrics.Replace('\r', '\n')
                Link = parseLink t.Comment }: Track.T) }: Playlist.T

let main playlistName =
    printfn $"Dumping track metadata for playlist '{playlistName}'\n"

    getPlaylist playlistName |> Playlist.writeToFile

    printfn $"Generated {Playlist.defaultFilename}"
