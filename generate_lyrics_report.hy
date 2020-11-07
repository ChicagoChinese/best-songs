(require [hy.contrib.walk [let]])

(import json)
(import [pathlib [Path]])
(import [hanzidentifier :as hi])
(import chinese-converter)
(import [colorama [Fore Style]])
(import [jinja2 [Environment FileSystemLoader]])
(import [prelude [*]])

(setv env (Environment :loader (FileSystemLoader "templates")))
(setv lyrics-report-file "lyrics_report.html")

(defn get-text-chunk [item]
  (->>
    [
      (get item "title")
      (get item "artist")
      ""
      (get item "lyrics")
    ]
    (.join "\n")))

(defn generate-report [items]
  (->>
    (.get-template env "lyrics_report.j2")
    (.render :items items)
    (spit lyrics-report-file)
  (print f"Generated {lyrics-report-file}")))

(defn text-contains-traditional [text]
  (let [choices [hi.TRADITIONAL hi.MIXED hi.BOTH]]
    (print (repr text))
    (in (hi.identify text) choices)))

(setv items
  (as-> (Path "tracks.json") it
    (.read_text it)
    (json.loads it)
    (lfor
      item it
      :do (let
        [chunk (get-text-chunk item)]
        (if (text-contains-traditional chunk)
          (assoc item "simplified" (-> chunk chinese-converter.to-simplified .splitlines))
          (continue)))
      item)))

(when (len items)
  (print f"{Fore.YELLOW}There are {(len items)} tracks that seem to contain traditional characters:\n")
  (for [[i item] (enumerate items 1)]
    ; must use setv when using f-string
    (setv title (get item "title"))
    (print f"{i}. {title}"))
  (print Style.RESET_ALL)
  (generate-report items))
