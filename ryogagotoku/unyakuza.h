/*
Simple LZSS used in SEGA 0.1
by Luigi Auriemma
e-mail: aluigi@autistici.org
web:    aluigi.org

Used in Yakuza 3 and Binary Domain.

    bytes   description
    4       SLLZ
    1       0 for little endian, 1 for big
    1       ???
    2       0x10
    4       uncompressed size
    4       compressed size
    x       compressed data
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>



#define unyakuza_bswap32(n) \
        (((n & 0xff000000) >> 24) | \
         ((n & 0x00ff0000) >>  8) | \
         ((n & 0x0000ff00) <<  8) | \
         ((n & 0x000000ff) << 24))



int unyakuza(unsigned char *in, int insz, unsigned char *out, int outsz, int check_head) {
    typedef struct {    
        unsigned char   sign[4];
        unsigned char   endian;
        unsigned char   dummy;
        unsigned short  zoff;   // type or data offset?
        unsigned int    size;
        unsigned int    zsize;
    } yakuza_t;
    yakuza_t        *yakuza;
    int             i,
                    d,
                    a,
                    b,
                    op;
    unsigned char   *p  = in,
                    *o  = out,
                    *il = in + insz,    
                    *ol = out + outsz;  

    if(!in || (insz < 0) || !out || (outsz < 0)) return(-1);  //容错处理

    if(check_head) {   //如果check_head为true;
        if(insz < sizeof(yakuza_t)) return(-2);
        yakuza = (yakuza_t *)p;
        if(memcmp(yakuza->sign, "SLLZ", 4)) return(-3);         //应该是判断标识为“SLLZ”退出;
        if(yakuza->endian) {  //如果endian为true;
            yakuza->zoff  = (yakuza->zoff >> 8) | (yakuza->zoff << 8);  //交换大小端
            yakuza->size  = unyakuza_bswap32(yakuza->size);   //交换大小端
            yakuza->zsize = unyakuza_bswap32(yakuza->zsize);  //交换大小端
        }
        if(yakuza->zoff != 0x10) return(-4);   //容错处理
        if(yakuza->size > outsz) return(-5);   //容错处理
        if(yakuza->zsize > insz) return(-6);   //容错处理
        p += 0x10; //处理完头部，指针移到头部后面
    }

    b = *p++;  //0x10处的数据给b p后移
    a = 8;     //计数器一次处理8字节数据
    while(o < ol) {
        if(p >= il) return(-7);  //如果已经到结束位置退出

        if(b & 0x80) op = 1;     //b与0x80做and运算判断是否大于0
        else         op = 0;

        b <<= 1;       //b向左位移一位
        a--;
        if(!a) {       //判断a是否小于等于0，开始处理下一批数据
            b = *p++;  
            a = 8;
        }

        if(op) {
            d = ((p[0] >> 4) | (p[1] << 4)) + 1;   //第一个字节向右位移4位，第二个字节向左位移4位拼起来再+1；
            for(i = (p[0] & 15) + 3; i > 0; i--) {
                if(o >= ol) break;
                *o = *(o - d);   //把地址为o-d的地址的内容放到输出中
                o++;
            }
            p += 2;  //指针向后移两字节
        } else {
            if(o >= ol) break;
            *o++ = *p++; //把p指向的数据放入输出中 指针自增
        }
    }
    return(o - out); //返回输出数据指针
}


//前 0x10 为数据头 0x4决定大小端


