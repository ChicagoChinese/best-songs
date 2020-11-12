import jieba
from hanziconv import HanziConv

text = "多年生草本植物，分布於海拔500－1100米的地區，一般生長於晝夜溫差小的山地緩坡或斜坡地的針闊混交林或雜木林中，喜陰涼、濕潤的氣候。由於其根部肥大，形若紡錘，常有分叉，整體形似人的頭、手、足和四肢，故稱其為人參。風茄外型亦類似人。古代人參的雅稱為黃精、地精、神草。人參被人們稱為「百草之王」，是聞名遐邇的「東北三寶」（人參、貂皮、鹿茸）之一，是馳名中外、老幼皆知的名貴藥材。 "

# text = HanziConv.toSimplified(text)
print(text)

print("; ".join(jieba.cut(text, cut_all=True)))
