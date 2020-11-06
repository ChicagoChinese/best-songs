(require [hy.contrib.walk [let]])
(import [pathlib [Path]])

(defn spit [location text]
  (let [path (Path location)]
    (.write-text path text)))
