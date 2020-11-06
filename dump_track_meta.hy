(require [hy.contrib.walk [let]])

(import subprocess)
(import json)
(import [pathlib [Path]])

(defn get-track-locations [playlist-name]
  (->
    ["swift" "track_paths.swift" playlist-name]
    (subprocess.check-output)
    (.decode "utf-8")
    (.strip)
    (.splitlines)))

(defn ffprobe-output-to-meta [d location]
  (let [
    tags (get d "format" "tags")
    gett (fn [key] (get tags key))]
    {
      "title" (gett "title")
      "artist" (gett "artist")
      "link" (gett "comment")
      "genre" (gett "genre")
      "lyrics" (.replace (gett "lyrics") "\r" "\n")
      "location" location
    }))

(defn get-track-meta [location]
  (->
    ["ffprobe" location "-print_format" "json" "-show_format"]
    (subprocess.check-output)
    (json.loads)
    (ffprobe-output-to-meta location)))

(defn spit [location text]
  (let [path (Path location)]
    (.write-text path text)))

(->>
  (map get-track-meta (get-track-locations "Best of 2020"))
  (list)
  (json.dumps :indent 2 :ensure-ascii False)
  (spit "tracks.json"))
