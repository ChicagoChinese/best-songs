module Prelude

open System.IO
open Fake.Core
open FSharp.Json

let trackLimit = 50

let getCommandOutput command args =
    let result =
        CreateProcess.fromRawCommand command args
        |> CreateProcess.redirectOutput
        |> CreateProcess.ensureExitCode
        |> Proc.run

    result.Result.Output.Trim()

module Link =
    type T =
        | YouTube of string
        | YouTubeLong of string
        | Other of string
        | None

    let show t =
        match t with
        | YouTube s
        | YouTubeLong s
        | Other s -> s
        | None -> "no link"

module Track =
    type T =
        { Title: string
          Artist: string
          Link: Link.T
          Genre: string
          Lyrics: string
          Location: string }

module Playlist =
    let defaultFilename = "playlist.json"
    let playlistDirectory = "playlists"

    type T = { Name: string; Tracks: Track.T array }

    let config =
        JsonConfig.create (jsonFieldNaming = Json.lowerCamelCase)

    let readFromFile filename =
        File.ReadAllText(filename)
        |> Json.deserializeEx<T> config

    let readFromDefaultFile () = readFromFile defaultFilename

    let getTracksFromDefaultFile () = (readFromDefaultFile ()).Tracks

    let getPlaylists () =
        Directory.GetFiles(playlistDirectory)
        |> Array.sort
        |> Array.map readFromFile

    let writeToFile (playlist: T) =
        let json = Json.serializeEx config playlist
        File.WriteAllText(defaultFilename, json)
