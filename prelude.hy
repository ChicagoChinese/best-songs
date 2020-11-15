(require [hy.contrib.walk [let]])
(import [pathlib [Path]])

(defn update [dict key func]
  (assoc dict key (func (get dict key))))

(defn get-with-default [dict key default]
  (try
    (get dict key)
    (except [[IndexError KeyError]]
      default)))

(defn spit [location text]
  (let [path (Path location)]
    (.write-text path text)))
