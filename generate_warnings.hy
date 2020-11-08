(require [hy.contrib.walk [let]])

(import json)
(import [pathlib [Path]])
(import [hanzidentifier :as hi])
(import [hanziconv [HanziConv]])
(import [colorama [Fore Style]])
(import [jinja2 [Environment FileSystemLoader]])
(import [prelude [*]])

(setv env (Environment :loader (FileSystemLoader "templates")))
(setv lyrics-report-file "lyrics_report.html")

(defn get-text-chunk [track]
  (->>
    [
      (get track "title")
      (get track "artist")
      ""
      (get track "lyrics")
    ]
    (.join "\n")))

(defn generate-lyrics-report [tracks]
  (->>
    (.get-template env "lyrics_report.j2")
    (.render :items tracks)
    (spit lyrics-report-file)
  (print f"\nPlease see {lyrics-report-file} to correct the metadata\n")))

(defn text-contains-traditional [text]
  (let [choices [hi.TRADITIONAL hi.MIXED hi.BOTH]]
    (in (hi.identify text) choices)))

(setv tracks
  (->
    (Path "tracks.json")
    .read_text
    json.loads))

(defn check-for-simplified [tracks]
  ; must use setv for variables that appear inside f-string
  (setv tracks
    (lfor
      track
      tracks
      :do (let [chunk (get-text-chunk track)]
        (if (text-contains-traditional chunk)
          (assoc track "simplified" (-> chunk HanziConv.toSimplified .splitlines))
          (continue)))
      track))
  (if (= (len tracks) 0)
    (print f"{Fore.GREEN}All tracks use simplified characters{Style.RESET_ALL}\n")
    (do
      (print f"{Fore.YELLOW}There are {(len tracks)} tracks that seem to contain traditional characters:{Style.RESET_ALL}\n")
      (for [[i track] (enumerate tracks 1)]
        (setv title (get track "title"))
        (print f"{i}. {title}"))
      (generate-lyrics-report tracks))))

(defn check-for-youtube-links [tracks]
  (setv tracks
    (let [has-youtube-link (fn [link] (or (in "youtube.com" link) (in "youtu.be" link)))]
      (lfor
        track
        tracks
        :if (not (has-youtube-link (get track "link")))
        track)))
  (if (= (len tracks) 0)
    (print f"{Fore.GREEN}All tracks have youtube links{Style.RESET_ALL}\n")
    (do
      (print f"{Fore.YELLOW}There are {(len tracks)} tracks that don't have youtube links:{Style.RESET_ALL}\n")
      (for [[i track] (enumerate tracks 1)]
        (setv
          title (get track "title")
          artist (get track "artist"))
        (print f"{i}. {artist}  {title}")))))

(defmain []
  (check-for-simplified tracks)
  (check-for-youtube-links tracks))
