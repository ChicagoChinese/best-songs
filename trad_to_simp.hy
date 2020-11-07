(import [hanzidentifier :as hi])
(import chinese-converter)

(setv text "作曲：嘟嘟\n作词：嘟嘟\n编曲：嘟嘟\n===\n\n摇晃水平景色骤变 风华瞬间狼狈\n采集信任让谎破灭\n保持平衡视线准确 一切认知崩溃\n重塑我的先天条件\n\n请说吧 我忏悔\nI'll tell you I'm in paradise\n无所谓 我无所谓\nI'll lead you to my paradise\n一瞬间所有的净水 都变成我干涸的血\nNow we gonna call that ''home''\n\n就往身上貼满标签 你送我的标签\n和最自豪的那双眼\n就往身上喷上香味 藏好自己的低贱\n你说不对那就不对\n\n请说吧 我忏悔\nI'll tell you I'm in paradise\n无所谓 都无所谓\nI'll lead you to my paradise\n一瞬间所有的净水 都变成我干涸的血\nNow we gonna call that ''home''\n\n请说吧 我忏悔\nI'll tell you I'm in paradise\n无所谓 都无所谓\nI'll lead you to my paradise\n就投下吧 你的自以为\n在饥饿以前让我再说 最后一次说\nNow we gonna call that “home”")

; (print (.strip text))
(setv id-code (hi.identify text))
(print "Identification code:" id-code)

(if (in id-code [hi.TRADITIONAL hi.MIXED hi.BOTH])
  (do
    (print "Detected non-simplified text, converting...")
    (for [line (.splitlines text)]
      (setv new-line (chinese-converter.to-simplified line))
      (print (hi.identify new-line) new-line)))
    ; (print (chinese-converter.to-simplified text)))
  (print "Text doesn't contain traditional characters"))
