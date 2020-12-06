let usage = """
Commands:

dump <playlist name>    Dump track metadata for given playlist
"""

[<EntryPoint>]
let main argv =
  match argv with
  | [|"dump"|] -> Dump.main "Recently Added"
  | [|"dump"; playlistName|] -> Dump.main playlistName
  | _ -> printfn "%s" usage
  0
