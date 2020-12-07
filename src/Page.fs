module Page

open System.IO
open Prelude

module Template =
    open Giraffe.ViewEngine

    let tracksPage = "tracks.html"

    let generateTracksPage (tracks: Track.T array) =
        html [] [
            head [] [ meta [ _charset "utf-8" ] ]
            body [] [
                ol [] [
                    for track in tracks do
                        yield li [] [ str track.Title ]
                ]
            ]
        ]
        |> RenderView.AsString.htmlDocument
        |> fun output -> File.WriteAllText(tracksPage, output)
        printfn "\nGenerated %s" tracksPage

let main () =
    let tracks = Track.readTracksFromFile ()
    Template.generateTracksPage tracks
