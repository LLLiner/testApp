using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


//直接发送抓获的ip包
namespace RawSocket
{
    class udpIpHeader
    {
        //Socket socket;
        //Byte sendBuffer; Byte是类型，byte是关键字，无区别，指向的都是同一样东西。
        int SENDBUF_SIZE = 50; //定义发送缓存大小



        public struct IpHeader
        {
            public byte ip_verlen; // IP version and IP Header length
            public byte ip_tos; // Type of service
            public ushort ip_totallength; // total length of the packet
            public ushort ip_id; // unique identifier
            public ushort ip_offset; // flags and offset
            public byte ip_ttl; // Time To Live
            public byte ip_protocol; // protocol (TCP, UDP etc)
            public ushort ip_HChecksum; //IP Header checksum
            public uint ip_srcaddr; //Source address
            public uint ip_destaddr;//Destination Address
        }
        public struct UdpHeader
        {
            public ushort srcPort;//源端口
            public ushort dstPort;//目的端口
            public ushort totallength;//udp总长度
            public ushort udpCheckSum;//udp校验和
        }


        /// <summary>
        /// 将一个byte数组转换成16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString();
        }


        //通过marshal将结构体转换成byte数组
        public static void ByMarshal(object p_StructObj, Stream stream)
        {
            int stuctSize = Marshal.SizeOf(p_StructObj);
            byte[] stuctBytes = new byte[stuctSize];
            IntPtr structPtr = Marshal.AllocHGlobal(stuctSize);
            Marshal.StructureToPtr(p_StructObj, structPtr, false);
            Marshal.Copy(structPtr, stuctBytes, 0, stuctSize);
            Marshal.FreeHGlobal(structPtr);
            stream.Write(stuctBytes, 0, stuctSize);
        }

        //改动过后的版本，直接返回结构体转换的byte[]数组
        public byte[] ByMarshal2(object p_StructObj)
        {
            int stuctSize = Marshal.SizeOf(p_StructObj);
            byte[] structBytes = new byte[stuctSize];
            IntPtr structPtr = Marshal.AllocHGlobal(stuctSize);
            Marshal.StructureToPtr(p_StructObj, structPtr, false);
            Marshal.Copy(structPtr, structBytes, 0, stuctSize);
            Marshal.FreeHGlobal(structPtr);
            //stream.Write(stuctBytes, 0, stuctSize);
            return structBytes;
        }


        /*
        //通过unsafe实现结构向byte[]的转换
        public unsafe static void ByUnsafe(MyStruct s, Stream stream)
        {
            MyStruct* ptr = &s;
            for (int i = 0; i < sizeof(MyStruct); i++)
                stream.WriteByte(*((byte*)ptr + i));
        }
        */

        //通过unsafe实现结构向byte[]的转换
        public unsafe static byte[] byUnsafe(IpHeader s1,UdpHeader s2)
        {
            MemoryStream ms = new MemoryStream();
            IpHeader* ptr1 = &s1;
            UdpHeader* ptr2 = &s2;
            for (int i = 0; i < sizeof(IpHeader); i++)
            {
                ms.WriteByte(*((byte*)ptr1 + i));
            }
            for(int i = 0;i < sizeof(UdpHeader);i++)
            {
                ms.WriteByte(*((byte*)ptr2 + i));
            }
            ms.WriteByte(0x31);
            ms.WriteByte(0x32);
            ms.WriteByte(0x33);
            ms.WriteByte(0x33);
            ms.WriteByte(0x34);
            ms.WriteByte(0x35);
            ms.WriteByte(0x36);
            ms.ToArray();

            byte[] bytes = ms.ToArray();
            
            
            Console.WriteLine("ip_hd+udp_hd+data转成byte数组：" + ByteArrayToHexString(bytes));

            return bytes;
        }
        

        public void send2()
        {
            byte[] sendBuf2 = new byte[SENDBUF_SIZE];

            IpHeader ip_hd = new IpHeader();
            UdpHeader udp_hd = new UdpHeader();
            ip_hd.ip_verlen = (0x45); //version and HLength
            ip_hd.ip_tos = (0x00);//TOS
            ////在将16进制的数据（只要大于一个字节的数据）存放在变量里时，就需要考虑主机字节序（和一般的网络字节序是相反的）。
            ///如果没有正确考虑字节序进行存放，会导致发包失败
            //ip_hd.ip_totallength = (0x0024); 
            ip_hd.ip_totallength = (0x2400);//IP数据包总长度-------后面还会被自动修改
            ip_hd.ip_id = (0x0000);//identification
            ip_hd.ip_offset = (0x0000);//3位flags+13位片偏移
            ip_hd.ip_ttl = (0x40);//TTL
            ip_hd.ip_protocol = (0x11);//传输层协议
            ip_hd.ip_HChecksum = (0x7b0c);//头部校验和---------后面会被自动修改
            ip_hd.ip_srcaddr = (0xc801a8c0);//源ip
            ip_hd.ip_destaddr = (0x7701a8c0);//目的ip
            //udp头
            udp_hd.srcPort = (0xc322);//源端口
            udp_hd.dstPort = (0x901f);//目的端口
            udp_hd.totallength = (0x0e00);//总长度-----------------不知道为啥，这个地方没有被自动修改--------------如果总长度字段和实际数据长度字段对应不上，wireshark会爆红
            udp_hd.udpCheckSum = (0x4492);//验和----------------自动被修改


            sendBuf2 = byUnsafe(ip_hd,udp_hd);

           
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);  //指示应用程序为输出输出数据报提供ip头---应该是发送的时候
            IPEndPoint IPEPoint = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080); //

            while (true)
            {
                System.Threading.Thread.Sleep(2000);
                try
                {
                    int sendSize = 0;
                    EndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080);
                    //sendTo只是将数据拷贝到内存，交给系统的网络模块，就算把网线拔了返回值依然是正常的
                    sendSize = socket.SendTo(sendBuf2, ep); //不知道IPEndPoint会不会强制转换成EndPoint
                    Console.WriteLine(" ");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("已成功发送" + sendSize + "字节数据");
                    Console.WriteLine("sendBuf:" + ByteArrayToHexString(sendBuf2));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("发送错误!" + ex.Message);
                }
            }











            /*
             * 试图通过marshal将多个结构体放入一个byte数组，但是失败了。
            int iphSize = sizeof(IpHeader);
            byte[] iphBytes = new byte[];
            ByMarshal2(ip_hd);

            int structSize2 = Marshal.SizeOf(udp_hd);//获取结构体大小
            IntPtr structPtr2 = Marshal.AllocHGlobal(structSize2);//分配结构体大小相同的内存(相当于指针)
            Marshal.StructureToPtr(udp_hd, structPtr2, false);


            //Marshal.Copy(structPtr1, sendBuf2, 0, structSize1);
            */
            //Marshal.Copy(structPtr2, sendBuf2, 0, structSize2);//虽然空间足够大，但是直接重复copy是会把前面一次copy的内容覆盖掉。

            /*
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("sendBuf2：" + ByteArrayToHexString(sendBuf2));
            */
        }


        //直接抓取一个能正常发送的数据包自己再用原始套接字发送一次
        //119:8080-->200:8899抓取的ip数据包
        public void send()
        {
            byte[] sendBuf = new byte[SENDBUF_SIZE];
            
            //sendBuf.SetValue(0x45, 0);
            sendBuf[0] = (0x45); //version and HLength
            sendBuf[1] = (0x00);//TOS

            sendBuf[2] = (0x00);//IP数据包总长度-------后面还会被自动修改
            sendBuf[3] = (0x24);

            sendBuf[4] = (0x00);//identification
            sendBuf[5] = (0x00);

            sendBuf[6] = (0x00);//3位flags+13位片偏移
            sendBuf[7] = (0x00);
            sendBuf[8] = (0x40);//TTL
            sendBuf[9] = (0x11);//传输层协议

            sendBuf[10] = (0x0c);//头部校验和---------后面会被自动修改
            sendBuf[11] = (0x7b);

            sendBuf[12] = (0xc0);//源ip
            sendBuf[13] = (0xa8);
            sendBuf[14] = (0x01);
            sendBuf[15] = (0xc8);
            sendBuf[16] = (0xc0);//目的ip
            sendBuf[17] = (0xa8);
            sendBuf[18] = (0x01);
            sendBuf[19] = (0x77);

            //udp头
            sendBuf[20] = (0x22);//源端口
            sendBuf[21] = (0xc3);
            sendBuf[22] = (0x1f);//目的端口
            sendBuf[23] = (0x90);
            sendBuf[24] = (0x00);//总长度-----------------不知道为啥，这个地方没有被自动修改--------------如果总长度字段和实际数据长度字段对应不上，wireshark会爆红
            sendBuf[25] = (0x0e);
            sendBuf[26] = (0x92);//校验和----------------自动被修改
            sendBuf[27] = (0x44);

            sendBuf[28] = (0x31);//用户数据
            sendBuf[29] = (0x32);
            sendBuf[30] = (0x33);
            sendBuf[31] = (0x34);
            sendBuf[32] = (0x35);
            sendBuf[33] = (0x36);
            //sendBuf[34] = (0x0d);
            //sendBuf[35] = (0x0a);


            //   对于最后一个协议字段，到底应该设置网络层的ip协议还是传输层的udp协议
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);  //指示应用程序为输出输出数据报提供ip头---应该是发送的时候
            IPEndPoint IPEPoint = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080); //

            //while (true)
            //{
                //System.Threading.Thread.Sleep(2000);
                try
                {

                    // 可考虑换种EndPoint方式
                    ////Socket s = new Socket(AddressFamily.Unspecified, SocketType.Raw, ProtocolType.Raw);
                    //EndPoint ep = new IPEndPoint(IPAddress.Parse("205.188.100.58"), 80);
                    //s.SendTo(GetBytes(""), ep); //im sending nothing, so i expect the frame to just have ethernet stuff
                    ////s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, false);


                    int sendSize = 0;
                    EndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080);
                    //sendTo只是将数据拷贝到内存，交给系统的网络模块，就算把网线拔了返回值依然是正常的
                    sendSize = socket.SendTo(sendBuf, ep); //不知道IPEndPoint会不会强制转换成EndPoint
                    Console.WriteLine(" ");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("已成功发送" + sendSize + "字节数据");
                    Console.WriteLine("sendBuf:" + ByteArrayToHexString(sendBuf));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("发送错误!" + ex.Message);
                }
            //}

        }

    }
}
