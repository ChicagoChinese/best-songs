module Page

open System.IO
open Prelude

module Template =
    open Giraffe.ViewEngine

    let tracksPage = "tracks.html"

    let generateTracksPage (playlist: Playlist.T) =
        html [] [
            head [] [
                meta [ _charset "utf-8" ]
                link [ _href "https://fonts.googleapis.com/css?family=Roboto:300,300i,400,400i,700,700i"
                       _rel "stylesheet" ]
                style [] [
                    rawText
                        """
                    body {
                      font-size: 14px;
                      font-family: "Roboto", "Helvetica Neue", Helvetica, Arial, sans-serif;
                    }"""
                ]
            ]
            body [] [
                h1 [] [ str playlist.Name ]
                ol [] [
                    for track in playlist.Tracks do
                        yield
                            li [] [
                                a [ _href (Link.show track.Link)
                                    _target "_blank"
                                    _style "font-size: 1.2rem" ] [
                                    str track.Title
                                ]
                                span [ _style "color: #888" ] [
                                    str $" ✪ {track.Artist} ♫ {track.Genre}"
                                ]
                            ]
                ]
            ]
        ]
        |> RenderView.AsString.htmlDocument
        |> fun output -> File.WriteAllText(tracksPage, output)

        printfn $"\nGenerated {tracksPage}"

let main limit =
    let playlist = Playlist.readFromDefaultFile ()

    let playlist =
        { playlist with
              Tracks = playlist.Tracks.[0..(limit - 1)] }

    Template.generateTracksPage playlist
