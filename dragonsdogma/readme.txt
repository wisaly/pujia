龙之信条

文本导入导出

结构：
4 byte : 00 44 4D 47  "DMG"
4 byte : 00 01 02 01 
12 byte : 00
1 int32 : text tag count
1 int32 : text count
1 int32 : text tag  length
1 int32 : text length
1 int32 : 00 00 00 07
8 byte : 54 65 78 74 57 65 62 00 "TextWeb"
( text tag index
1 int32 : index
1 int32 : offset base on a big address
)
( text tags
string : end with 0
)
( texts
string : end with 0 utf-8 encode
)