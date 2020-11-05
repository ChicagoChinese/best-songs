(import hanzidentifier)
(import chinese_converter)

(setv text "\
作詞/曲：福祿壽FloruitShow

風吹亂了她的頭髮
也把她影子拉扯散啦
多想也把我帶著走啊
就算虛度也不怕
她說我心裡有個寶啊
千萬別把它給毀啦
所以和風一起飛的我啊
不需要把腳落下

起風啦
該回去啦
你看 你看
所有過往都在這兒呢
大世界 它耀眼嗎
只是 只是我
再也找不到你了

風吹皺了她的手啊
還想她拉著我慢慢走啊
走到溫暖金黃色的舊日子裡啊
那時我天不怕地也不怕

起風啦
該回去啦
你看 你看
所有過往都在這兒呢

大世界 我也會去呀
等著 等著
我走完這段路就來了

她的茉莉花我還在喝著
她聽的歌我還在唱著呢
直到她的苦衷變成了我的
她的仁慈也變成我的了

起風啦（Woo～）
起風啦（Woo～）

你會不會披星戴月
乘風破浪來我夢裡

風吹亂了我的頭髮
我知道你在這兒呢
下次在春光中見到你啊
千萬別再 拋下我啦

你會不會披星戴月
乘風破浪來我夢裡

---轉自福祿壽FloruitShow樂團 無商業用途  純好歌分享---
")

; (print (.strip text))
(unless (hanzidentifier.is_simplified text)
  (print "Detected non-simplified text, converting...")
  (print (chinese_converter.to_simplified text))
)
