module Site

open JiebaNet.Segmenter
open ToolGood.Words.Pinyin
open Prelude

let getSlug =
    let segmenter = JiebaSegmenter()

    fun s ->
        let segments = segmenter.Cut(s, cutAll = true)

        segments
        |> Seq.filter (fun s -> s <> "")
        |> Seq.map (fun s -> WordsHelper.GetPinyin(s).ToLower())
        |> String.concat "-"

let main () =
    for playlist in Playlist.getPlaylists () do
        printfn $"{playlist.Name}"

        for track in playlist.Tracks do
            printfn "%s" (getSlug track.Title)
