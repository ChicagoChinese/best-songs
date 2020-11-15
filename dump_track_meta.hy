(require [hy.contrib.walk [let]])

(import subprocess)
(import json)
(import [prelude [*]])

(defn get-track-locations [playlist-name]
  (->
    ["swift" "track_paths.swift" playlist-name]
    (subprocess.check-output)
    (.decode "utf-8")
    .strip
    .splitlines))

(defn ffprobe-output-to-meta [d location]
  (let [
    tags (get d "format" "tags")
    gett (fn [key] (get tags key))
    lyrics (-> (get-with-default tags "lyrics" "")
               (.replace "\r\n" "\n")
               (.replace "\r" "\n"))]
    {
      "title" (gett "title")
      "artist" (gett "artist")
      "link" (gett "comment")
      "genre" (gett "genre")
      "lyrics" lyrics
      "location" location
    }))

(defn get-track-meta [location]
  (->
    ["ffprobe" location "-print_format" "json" "-show_format"]
    subprocess.check-output
    json.loads
    (ffprobe-output-to-meta location)))

(defn main [playlist-name]
  (->>
    (map get-track-meta (get-track-locations playlist-name))
    list
    (json.dumps :indent 2 :ensure-ascii False)
    (spit "tracks.json")))
