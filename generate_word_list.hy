(import json)
(import [pathlib [Path]])
(import [collections [Counter]])
(import jieba)

(setv lyrics-iter
  (as-> (Path "tracks.json") it
    (.read_text it)
    (json.loads it)
    (gfor item it (get item "lyrics"))))

(print (list lyrics-iter))
; (setv seg-list (list (jieba.cut "我来到北京清华大学" :cut-all True)))
; (print (list seg-list))
