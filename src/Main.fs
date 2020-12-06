let usage = """
Commands:

dump <playlist name>    Dump track metadata for given playlist
warnings                Generate warnings report
"""

[<EntryPoint>]
let main argv =
  match argv with
  | [|"dump"|] -> Dump.main "Recently Added"
  | [|"dump"; playlistName|] -> Dump.main playlistName
  | [|"warnings"|] -> Warning.main ()
  | _ -> printfn "%s" usage
  0
