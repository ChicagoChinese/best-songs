module Prelude

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
  type Link =
    | YouTube of string
    | YouTubeLong of string
    | Other of string
    | None

  type T = {
    title: string
    artist: string
    link: Link
    genre: string
    lyrics: string
    location: string
  }

  let parse json =
    Json.deserialize<T> json

  let serializeTracks (tracks: T array) =
    Json.serialize tracks
