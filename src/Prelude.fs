module Prelude

open System.IO
open Fake.Core
open FSharp.Json

let getCommandOutput command args =
    let result =
        CreateProcess.fromRawCommand command args
        |> CreateProcess.redirectOutput
        |> CreateProcess.ensureExitCode
        |> Proc.run

    result.Result.Output.Trim()

module Track =
    let filename = "tracks.json"

    type Link =
        | YouTube of string
        | YouTubeLong of string
        | Other of string
        | None

    type T =
        { title: string
          artist: string
          link: Link
          genre: string
          lyrics: string
          location: string }

    let readTracksFromFile json = Json.deserialize<T array> json

    let writeTracksToFile (tracks: T array) =
        let json = Json.serialize tracks
        File.WriteAllText(filename, json)
