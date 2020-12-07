module Page

open System.IO
open Prelude

module Template =
    open Giraffe.ViewEngine

    let tracksPage = "tracks.html"

    let generateTracksPage (tracks: Track.T array) =
        html [] [
            head [] [
                meta [ _charset "utf-8" ]
                style [] [
                    str """
                    body {
                      font-size: 14px;
                      font-family: Roboto, Helvetica Neue, Helvetica, Arial, sans-serif;
                    }"""
                ]
            ]
            body [] [
                ol [] [
                    for track in tracks do
                        yield
                            li [] [
                                a [ _href (Link.show track.Link)
                                    _target "_blank" ] [
                                    str track.Title
                                ]
                                str (" by " + track.Artist)
                            ]
                ]
            ]
        ]
        |> RenderView.AsString.htmlDocument
        |> fun output -> File.WriteAllText(tracksPage, output)
        printfn "\nGenerated %s" tracksPage

let main limit =
    let tracks = Track.readTracksFromFile ()
    let tracks = tracks.[0..(limit - 1)]
    Template.generateTracksPage tracks
