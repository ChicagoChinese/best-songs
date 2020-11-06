(import json)
(import [pathlib [Path]])
(import hanzidentifier)
(import chinese-converter)

(require [hy.contrib.walk [let]])


(defn get-text-chunk [meta]
  (->>
    [
      (get meta "title")
      (get meta "artist")
      ""
      (get meta "lyrics")
    ]
    (.join "\n")))

(as-> (Path "tracks.json") it
  (.read_text it)
  (json.loads it)
  (lfor
    m it
    :do (let
      [chunk (get-text-chunk m)]
      (if (hanzidentifier.is-simplified chunk)
        (continue)
        (do
          (assoc m "chunk" (-> m get-text-chunk chinese-converter.to-simplified))
          m)))
    m)
  (print it))
