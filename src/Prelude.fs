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
        { Title: string
          Artist: string
          Link: Link
          Genre: string
          Lyrics: string
          Location: string }

    let readTracksFromFile () =
        File.ReadAllText(filename)
        |> Json.deserialize<T array>

    let writeTracksToFile (tracks: T array) =
        let json = Json.serialize tracks
        File.WriteAllText(filename, json)
