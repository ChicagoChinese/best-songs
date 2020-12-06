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
