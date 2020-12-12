// https://github.com/feihong/swift-quickstart/blob/master/itunes_quickstart.swift
// https://github.com/feihong/music-tools/blob/master/itunes.py
// https://gist.github.com/Maqsim/db8950193761e7c0c2bf60c2455e8ba2
import Foundation
import ScriptingBridge

// Add a function for safe array access
extension Array {
  public func get(_ index: Int) -> Element? {
    guard index >= 0, index < endIndex else {
      return nil
    }
    return self[index]
  }
}

@objc public protocol Track {
  @objc optional var name: String {get}
  @objc optional var artist: String {get}
  @objc optional var genre: String {get}
  @objc optional var lyrics: String {get}
  @objc optional var comment: String {get}
  @objc optional var location: URL { get }
  @objc optional var album: String {get}
}
extension SBObject: Track {}

@objc public protocol Playlist {
  @objc optional func tracks() -> [Track]
  @objc optional var name: String { get }
}
extension SBObject: Playlist {}

@objc protocol iTunesApplication {
  @objc optional func playlists() -> [Playlist]
}
extension SBApplication : iTunesApplication {}

func findPlaylist(name: String, playlists: [Playlist]) -> Playlist? {
  for playlist in playlists {
    if let pname = playlist.name {
      if pname.starts(with: name) {
        return playlist
      }
    }
  }
  return nil
}

guard CommandLine.arguments.count >= 2 else {
  print("Please enter name of playlist you want to search for")
  exit(1)
}

if let app: iTunesApplication = SBApplication(bundleIdentifier: "com.apple.Music"),
   let searchName = CommandLine.arguments.get(1) {
  let playlists: [Playlist] = app.playlists?() ?? []
  guard let playlist = findPlaylist(name: searchName, playlists: playlists) else {
    print("Did not find a playlist whose name starts with '\(searchName)'")
    exit(1)
  }

  let tracks = playlist.tracks?() ?? []
  let dictArray = tracks.map { track in
    [
      "name": track.name ?? "",
      "artist": track.artist ?? "",
      "genre": track.genre ?? "",
      "comment": track.comment ?? "",
      "location": track.location?.path ?? "",
      "lyrics": track.lyrics ?? "",
      "album": track.album ?? "",
    ]
  }

  let result: [String: Any] = [
    "name": searchName,
    "tracks": dictArray,
  ]

  guard let jsonData = try? JSONSerialization.data(withJSONObject: result, options: .prettyPrinted) else {
    print("Failed to serialize tracks to JSON")
    exit(1)
  }
  print(String(decoding: jsonData, as: UTF8.self))
}
