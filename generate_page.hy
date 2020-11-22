(import [jinja2 [Environment FileSystemLoader]])
(import [prelude [*]])

(setv env (Environment :loader (FileSystemLoader "templates")))
(setv tracks-page-file "tracks.html")

(defn get-top-tracks []
  (setv get-youtube-line (fn [text]
    (->
      text
      .splitlines
      (find-first (fn [line] (or (.startswith line "https://youtu.be") (.startswith line "https://www.youtube.com")))))))
  (as-> (get-tracks) it
    (cut it 0 50)
    (lfor
      track
      it
      (update track "link" get-youtube-line))))

(defn main []
  (setv tracks (get-top-tracks))
  (for [track tracks]
    (print (get track "title") (get track "link") "\n"))
  (->>
    (.get-template env "tracks_page.j2")
    (.render :items tracks)
    (spit tracks-page-file))
  (print f"Generated {tracks-page-file}"))
