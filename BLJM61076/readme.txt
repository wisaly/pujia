文件结构
8 bytes string "Filename"
4 bytes int32 "Pack"的地址索引

212条4bytes地址索引，数值+0xC指向文件名字符串（每个文本段的文件名）

212条文本，以00结束

8 bytes string "Pack"+四个空格(0x20202020)
4 bytes 00 00 06 B0 "Pack"到下面索引结束的长度（0x1800-0x1150）
4 bytes 00 00 00 D4 下面索引（文本段个数）

212（0xD4）条索引
每个8bytes，组成：
4bytes int32 文本段开始地址
4bytes int32 文本段长度