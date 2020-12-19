module Site

open JiebaNet.Segmenter
open ToolGood.Words.Pinyin
open Prelude

let main () =
    let segmenter = JiebaSegmenter()

    for playlist in Playlist.getPlaylists () do
        printfn $"{playlist.Name}"

        for track in playlist.Tracks do
            let segments =
                segmenter.Cut(track.Title, cutAll = true)

            printfn
                "%s"
                (segments
                 |> Seq.filter (fun s -> s <> "")
                 |> Seq.map (fun s -> WordsHelper.GetPinyin(s).ToLower())
                 |> String.concat "-")
