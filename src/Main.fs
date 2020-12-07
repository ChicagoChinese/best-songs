let usage = """
Commands:

dump <playlist name>    Dump track metadata for given playlist
warnings                Generate warnings report
page                    Generate tracks page
"""

[<EntryPoint>]
let main argv =
  match argv with
  | [|"dump"|] -> Dump.main "Recently Added"
  | [|"dump"; playlistName|] -> Dump.main playlistName
  | [|"warnings"|] -> Warning.main ()
  | [|"page"|] -> Page.main ()
  | _ -> printfn "%s" usage
  0
