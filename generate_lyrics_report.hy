(require [hy.contrib.walk [let]])

(import json)
(import [pathlib [Path]])
(import hanzidentifier)
(import chinese-converter)
(import [colorama [Fore Style]])
(import [jinja2 [Environment FileSystemLoader]])
(import [prelude [*]])

(setv env (Environment :loader (FileSystemLoader "templates")))
(setv lyrics-report-file "lyrics_report.html")

(defn get-text-chunk [meta]
  (->>
    [
      (get meta "title")
      (get meta "artist")
      ""
      (get meta "lyrics")
    ]
    (.join "\n")))

(defn generate-report [items]
  (->>
    (.get-template env "lyrics_report.j2")
    (.render :items items)
    (spit lyrics-report-file)
  (print f"Generated {lyrics-report-file}")))

(setv items (as-> (Path "tracks.json") it
  (.read_text it)
  (json.loads it)
  (lfor
    m it
    :do (let
      [chunk (get-text-chunk m)]
      (if (hanzidentifier.is-simplified chunk)
        (continue)
        (assoc m "simplified" (-> m get-text-chunk chinese-converter.to-simplified .splitlines))))
    m)))

(when (len items)
  (print f"{Fore.YELLOW}There are {(len items)} tracks that seem to contain traditional characters:\n")
  (for [[i item] (enumerate items 1)]
    ; must use setv when using f-string
    (setv title (get item "title"))
    (print f"{i}. {title}"))
  (print Style.RESET_ALL)
  (generate-report items))
