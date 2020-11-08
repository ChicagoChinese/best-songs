(require [hy.contrib.walk [let]])
(import [pathlib [Path]])

(defn update [dict key func]
  (assoc dict key (func (get dict key))))

(defn spit [location text]
  (let [path (Path location)]
    (.write-text path text)))
