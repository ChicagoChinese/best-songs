let usage = """
Commands:

dump <playlist name>    Dump track metadata for given playlist
warnings                Generate warnings report
page                    Generate tracks page
zip                     Generate zip file containing track files
"""

[<EntryPoint>]
let main argv =
  match argv with
  | [|"dump"|] -> Dump.main "Recently Added"
  | [|"dump"; playlistName|] -> Dump.main playlistName
  | [|"warnings"|] -> Warning.main ()
  | [|"page"|] -> Page.main 50
  | [|"page"; limit|] -> Page.main (int limit)
  | [|"zip"|] -> Zip.main ()
  | _ -> printfn "%s" usage
  0
