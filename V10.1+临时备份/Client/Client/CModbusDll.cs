using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public static class CModbusDll
    {
        public static byte[] WriteDO(int addr,int io,bool openclose)
        {
            byte[] src = new byte[8];
            //FE 设备地址，这里为广播地址
            src[0] = (byte)addr;//设备地址后8位----4byte字节的int转byte，截取int后8位--推测这就是为什么PC和设备需要在同一网段
            src[1] = 0x05;
            src[2] = 0x00;
            src[3] = (byte)io;
            src[4] = (byte)((openclose) ? 0xff : 0x00);   //操作------需要进行开还是关操作
            src[5] = 0x00;
            ushort crc = CMBRTU.CalculateCrc(src,6);
            src[6] = (byte)(crc & 0xff);   //前6字节数据的CRC16校验码
            src[7] = (byte)(crc>>8);
            return src;
        }
        public static byte[] WriteAllDO(int addr, int ionum, bool openclose)
        {
            byte[] src = new byte[10];
            src[0] = (byte)addr;
            src[1] = 0x0f;
            src[2] = 0x00;
            src[3] = 0x00;
            src[4] = 0x00;
            src[5] = (byte)ionum;
            src[6] = 0x01;
            src[7] = (byte)((openclose)?0xff:0x00);
            ushort crc = CMBRTU.CalculateCrc(src, 8);
            src[8] = (byte)(crc & 0xff);
            src[9] = (byte)(crc >> 8);
            return src;
        }
        public static byte[] ReadDO(int addr, int donum)
        {
            byte[] src = new byte[8];
            src[0] = (byte)addr;
            src[1] = 0x01;
            src[2] = 0x00;
            src[3] = 0x00;
            src[4] = 0x00;
            src[5] = (byte)donum;
            ushort crc = CMBRTU.CalculateCrc(src, 6);
            src[6] = (byte)(crc & 0xff);
            src[7] = (byte)(crc >> 8);
            return src;
        }
        public static byte[] ReadDI(int addr,int dinum )
        {
            byte[] src = new byte[8];
            src[0] = (byte)addr;
            src[1] = 0x02;
            src[2] = 0x00;
            src[3] = 0x00;
            src[4] = 0x00;
            src[5] = (byte)dinum;
            ushort crc = CMBRTU.CalculateCrc(src, 6);
            src[6] = (byte)(crc & 0xff);
            src[7] = (byte)(crc >> 8);
            return src;
        }
    }
}
