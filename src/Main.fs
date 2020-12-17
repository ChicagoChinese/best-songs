let usage = """
Commands:

dump [playlist name]    Dump track metadata for given playlist
warnings                Generate warnings report
page [n]                Generate tracks page for first n tracks (n defaults to 50)
zip                     Generate zip file containing first 50 track files
video [n]               Convert tracks without youtube links to MP4 video format
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
  | [|"video"; limit|] -> Video.main (int limit)
  | _ -> printfn $"{usage}"
  0
