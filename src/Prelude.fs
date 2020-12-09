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
    let filename = "tracks.json"

    type T =
        { Title: string
          Artist: string
          Link: Link.T
          Genre: string
          Lyrics: string
          Location: string }

    let readTracksFromFile () =
        File.ReadAllText(filename)
        |> Json.deserialize<T array>

    let writeTracksToFile (tracks: T array) =
        let json = Json.serialize tracks
        File.WriteAllText(filename, json)
