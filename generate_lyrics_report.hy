(require [hy.contrib.walk [let]])

(import json)
(import [pathlib [Path]])
(import hanzidentifier)
(import chinese-converter)
(import [colorama [Fore Style]])

(defn get-text-chunk [meta]
  (->>
    [
      (get meta "title")
      (get meta "artist")
      ""
      (get meta "lyrics")
    ]
    (.join "\n")))

(setv items (as-> (Path "tracks.json") it
  (.read_text it)
  (json.loads it)
  (lfor
    m it
    :do (let
      [chunk (get-text-chunk m)]
      (if (hanzidentifier.is-simplified chunk)
        (continue)
        (assoc m "chunk" (-> m get-text-chunk chinese-converter.to-simplified))))
    m)))

(when (len items)
  (print f"{Fore.YELLOW}There are {(len items)} tracks that seem to contain traditional characters:")
  (for [[i item] (enumerate items 1)]
    ; can't use let macro here
    (setv title (get item "title"))
    (print f"{i}. {title}")))

