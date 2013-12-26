重装机兵2R

编码：unicode  utf-16le

类型1：
at ./pack_data.pak
little-endian

4 byte header 53 45 54 55 "SETU"
1 int32 index count
(
1 int32 offset to text, could be 00 00 00 00
)
* string, end with 00 00

139 byte string at end
"/*
HasWrite      :True
HasBinary     :False
HasBinaryIndex:False
HasDefine     :False
HasIndex      :False
HasName       :False
*/
"

类型2：
at ./data/pack_data.pak

2 byte header 00 00
* string, end with 00 00

类型3：
at ./npc/pack_data.pak
little-endian

4 byte header 54 43 52 43 "TCRC"
1 int32 offset to text block from current position(+ 8 byte)
(index block
1 int32 text id 13 38 CE 01 
1 int32 text offset from text block start
)
4 byte fix data 54 45 58 54 "TEXT"
1 int32 text block length
(
* string,end with 00 00 00 00
)
