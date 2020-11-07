(import [hanzidentifier :as hi])
(import [hanziconv [HanziConv]])

(setv text "词曲：持修\n编曲：持修\n\n到底为什么 你会闯入我的世界\n你抱着我 看着我 对我说\n我们之间 有好多 都相同\n\n其实我跟大家一样虚伪\n宁可欺骗你 也不让 自己受伤\n其实我跟大家一样恶劣\n只想快乐却 又不敢 承担悲伤\n是个废物\n是个废物\n你跟我才不一样呢\n\n你抱着我 看着我 对我说\n我们之间 有好多 都相同\n你是如此完美(⁎⁍̴̛ᴗ⁍̴̛⁎)\n\n其实我跟大家一样虚伪\n宁可欺骗你 也不让 自己受伤\n其实我跟大家一样恶劣\n只想快乐却 又不敢 承担悲伤\n是个废物\n是个废物\n你跟我才不一样呢")

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
