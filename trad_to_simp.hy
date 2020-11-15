; From  https://github.com/tsroten/hanzidentifier/blob/develop/hanzidentifier.py#L9
; UNKNOWN = 0
; TRAD = TRADITIONAL = 1
; SIMP = SIMPLIFIED = 2
; BOTH = 3
; MIXED = 4

(import [hanzidentifier :as hi])
(import [hanziconv [HanziConv]])

(setv text "词曲Lyricist & Composer：Yider（伊德尔）\n制作人Producer: Yider（伊德尔）\n编曲Arrangement: Yider（伊德尔）\n附加制作Additional Production: Radiax杨博文\n人声採样Vocal sampling: AK郭采洁\n马头琴Moriinhuur: Yider（伊德尔）\n专辑录音室Recording Studio: 录顶技Studio\n录音工程师Recording Studio Engineers: 一丁Yiding/潇意\n录音师助理Recording Assistant: 孙少天\n混音工程师Engineer of Mixing: Radiax杨博文\n母带后期处理录音室Mastering Studio: Metropolis studio（UK）\n母带后期处理工程师Mastering Engineer: John Davis（UK）\n===\n\nYellow wall and empty eyes\n虚迷的阻碍 空洞的眼\nI loose my mind\n我迷失我自己\nIn the world and empty eyes\n在这世界 和空洞的眼\nI’ve put my past behind\n我丢失了过去\n（蒙语）nig​ ​hoyor​ ​gorov​ ​taav​ ​dolloo​ 24 hours\n(1 2 3 5 7 24hours)\n12357 24小时\n（蒙语）mini hosher nigen uuder​ 48 hours\n(I hope to have one day 48hours)\n我希望一天有48小时")

; (print (.strip text))
(setv id-code (hi.identify text))
(print "Identification code:" id-code)

(if (in id-code [hi.TRADITIONAL hi.MIXED hi.BOTH])
  (do
    (print "Detected non-simplified text, converting...")
    (for [line (.splitlines text)]
      (setv new-line (HanziConv.toSimplified line))
      (print (hi.identify new-line) new-line)))
    ; (print (chinese-converter.to-simplified text)))
  (print "Text doesn't contain traditional characters"))
