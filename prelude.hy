(require [hy.contrib.walk [let]])
(import [pathlib [Path]])
(import json)

(defn update [dict key func]
  (assoc dict key (func (get dict key)))
  dict)

(defn get-with-default [dict key default]
  (try
    (get dict key)
    (except [[IndexError KeyError]]
      default)))

(defn find-first [coll f]
  (->
    (gfor el coll :if (f el) el)
    (next None)))

(defn spit [location text]
  (let [path (Path location)]
    (.write-text path text)))

(defn get-tracks []
  (->
    (Path "tracks.json")
    .read_text
    json.loads))
