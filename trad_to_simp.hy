; From  https://github.com/tsroten/hanzidentifier/blob/develop/hanzidentifier.py#L9
; UNKNOWN = 0
; TRAD = TRADITIONAL = 1
; SIMP = SIMPLIFIED = 2
; BOTH = 3
; MIXED = 4

(import [hanzidentifier :as hi])
(import [hanziconv [HanziConv]])

(setv text "109台湾原创_客语组【首奖】_望 Mong _邱淑蝉 Chiu shu-chan\n附註：\n平安顺事：歌词创作，意指平安顺序。\n客语歌词校正：王兴宝呀\n===\n\n你这只细妹仔 煞勐读书 下二摆正嫁得着好老公\n你这只细妹仔 爱勤俭 下二摆正寻得着好头路\n你这只细妹仔 命靓靓 像着亻厓 像着亻厓\n你这只细妹仔 爱有样式 下二摆正会得人惜\n你这只细妹仔 生到恁靓 系么人个妹仔啊\n你这只细妹仔 命靓靓 像着亻厓 像着亻厓\n你这只细妹仔 做你走哪 毋使你愁上又愁下\n你这只细妹仔 做你走哪 亻厓会照顾好自家\n你这只细妹仔 净望你 净望你 平安顺事\n你这只细妹仔 莫放核 莫放核 归来个路\n你这只细妹仔 爱记得 爱记得 亻厓等在这等你 等你归来呀\n\n翻译\n\n你这个女孩 认真念书 以后才嫁的到好老公\n你这个女孩 要勤俭 以后才找的到好工作\n你这个女孩 命很美 像到我 像到我\n你这个女孩 要有样子 以后才会得人惜\n你这个女孩 长的真美 是谁家的女孩呀\n你这个女孩 命很美 像到我 像到我\n你这个女孩 尽管走吧 不用你愁上愁下\n你这个女孩 尽管走吧 我会照顾好自己\n你这个女孩 只望你 只望你 平安顺事\n你这个女孩 不要忘记 不要忘记 回家的路\n你这个女孩 要记得 要记得 我们在这等你 等你回来")

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
