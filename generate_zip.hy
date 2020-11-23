(import subprocess)
(import json)
(import [zipfile [ZipFile ZIP_STORED]])
(import [pathlib [Path]])
(import [prelude [*]])

(setv tracks-zip-name "Best of 2020")
(setv tracks-zip-file (+ tracks-zip-name ".zip"))

(defn get-top-tracks []
  (->
    (get-tracks)
    (cut 0 50)))

; Use zipfile library instead of zip command because this way you can set arcname
(defn generate-zip-file [files]
  (with [zf (ZipFile tracks-zip-file "w")]
    (for [file_ files]
      (setv name (. (Path file_) name))
      (.write zf file_ :arcname name :compress-type ZIP_STORED))))

(defn main []
  (as->
    (get-top-tracks) it
    (lfor track it (get track "location"))
    (generate-zip-file it))
  (print f"Generated {tracks-zip-file}"))
