(import json)
(import [pathlib [Path]])
(import [collections [Counter]])
(import [prelude [*]])
(import jieba)

(setv lyrics-iter
  (as-> (Path "tracks.json") it
    (.read_text it)
    (json.loads it)
    (gfor item it (get item "lyrics"))))

(setv counter
  (do
    (setv counter (Counter))
    (for [text lyrics-iter]
      (for [word (jieba.cut text :cut-all True)]
        (update counter word inc)))
    counter))

(for [[word count] (.most_common counter 100)]
  (print word count))
