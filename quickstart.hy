(import subprocess)
(import json)
(import [pathlib [Path]])

(require [hy.contrib.walk [let]])

(defn get-track-paths [playlist-name]
  (->
    ["swift" "track_paths.swift" playlist-name]
    (subprocess.check_output)
    (.decode "utf-8")
    (.strip)
    (.splitlines)))

(defn get-track-meta [path]
  (->
    ["ffprobe" path "-print_format" "json" "-show_format"]
    (subprocess.check_output)
    json.loads))

(for [path (get-track-paths "Best of 2020")]
  (print (get-track-meta path)))
