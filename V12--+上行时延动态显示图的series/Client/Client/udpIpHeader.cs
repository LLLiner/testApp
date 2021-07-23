using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


//发送自定义ip数据包
namespace Client
{
    class udpIpHeader
    {
        //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
        //Byte sendBuffer; Byte是类型，byte是关键字，无区别，指向的都是同一样东西。
        int SENDBUF_SIZE = 60; //定义发送缓存大小

        public udpIpHeader()
        {
            //socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1); 
        }

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

        //通过unsafe实现结构体向byte[]的转换
        public unsafe static byte[] byUnsafe(IpHeader s1, UdpHeader s2,byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            IpHeader* ptr1 = &s1;
            UdpHeader* ptr2 = &s2;
            for (int i = 0; i < sizeof(IpHeader); i++)
            {
                ms.WriteByte(*((byte*)ptr1 + i));
            }
            for (int i = 0; i < sizeof(UdpHeader); i++)
            {
                ms.WriteByte(*((byte*)ptr2 + i));
            }
            for (int i = 0; i < data.Length; i++)
            {
                ms.WriteByte(data[i]);
            }

            ms.ToArray();

            byte[] bytes = ms.ToArray();

            //Console.WriteLine("ip_hd+udp_hd+data转成byte数组：" + ByteArrayToHexString(bytes));

            return bytes;
        }

        
        public int send2(UInt32 srcIp, ushort srcPort, UInt32 dstIp, ushort dstPort, byte[] cmd, int p)
        {
            Console.WriteLine("这边接收到的数据srcPort:" + Convert.ToString(srcPort, 16));
            Console.WriteLine("dstPort:" + Convert.ToString(dstPort, 16));
            byte[] sendBuf2 = new byte[SENDBUF_SIZE];

            IpHeader ip_hd = new IpHeader();
            UdpHeader udp_hd = new UdpHeader();

            byte priority = Convert.ToByte(p);
            Console.WriteLine("移位前byte输出：" + Convert.ToString(priority, 2));
            priority = (byte)((priority & 0x07) << 5);
            Console.WriteLine("移位后byte输出：" + Convert.ToString(priority, 2));

            ip_hd.ip_verlen = (0x45); //version and HLength
            ip_hd.ip_tos = priority;//TOS
            ip_hd.ip_totallength = (0x2400);//IP数据包总长度-------后面还会被自动修改
            ip_hd.ip_id = (0x0000);//identification
            ip_hd.ip_offset = (0x0000);//3位flags+13位片偏移
            ip_hd.ip_ttl = (0x40);//TTL
            ip_hd.ip_protocol = (0x11);//传输层协议
            ip_hd.ip_HChecksum = (0x7b0c);//头部校验和---------后面会被自动修改
            ip_hd.ip_srcaddr = srcIp;//源ip
            ip_hd.ip_destaddr = dstIp;//目的ip
            //udp头
            //udp_hd.srcPort = srcPort;//源端口
            //udp_hd.dstPort = dstPort;//目的端口
            udp_hd.srcPort = (0xc322);
            udp_hd.dstPort = (0x1027);
            udp_hd.totallength = (0x1000);//总长度-----------------不知道为啥，这个地方没有被自动修改--------------如果总长度字段和实际数据长度字段对应不上，wireshark会爆红
            udp_hd.udpCheckSum = (0x4492);//验和----------------并没有被自动修改！！！！！！！！

            Console.WriteLine("udp_hd.srcPort:" + Convert.ToString(udp_hd.srcPort, 16));
            Console.WriteLine("udp_hd.dstPort:" + Convert.ToString(udp_hd.dstPort, 16));

            sendBuf2 = byUnsafe(ip_hd, udp_hd, cmd);//获取自定义的ip包

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);  //指示应用程序为输出输出数据报提供ip头---应该是发送的时候
                                                                                               //IPEndPoint IPEPoint = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080); //

            //while (true)
            //{
            //System.Threading.Thread.Sleep(2000);
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
                socket.Close();
                return sendSize;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("发送错误!" + ex.Message);
                socket.Close();
                return 0;
            }
            //}


        }

        /// <summary>
        /// 将两端信息和产生的命令作为参数传入进行udp发送
        /// （传送过来的ip、端口虽然是正确的，但是将最后的发送流显示出来的时候只有端口字节序变化了）</summary>
        /// <param name="srcIp_str"></param>
        /// <param name="srcPort_str"></param>
        /// <param name="dstIp_str"></param>
        /// <param name="dstPort_str"></param>
        /// <param name="cmd"></param>
        /// <param name="p"></param>
        /// <returns></returns>执行成功就返回发送的字节数，失败返回-1
        public int send2(String srcIp_str,String srcPort_str,String dstIp_str,String dstPort_str, byte[] cmd,int p)
        {
            //通过自定义ip数据包发送出去
            IPAddress temp1 = IPAddress.Parse(srcIp_str);
            UInt32 srcIp = BitConverter.ToUInt32(temp1.GetAddressBytes(), 0);
            IPAddress temp2 = IPAddress.Parse(dstIp_str);
            UInt32 dstIp = BitConverter.ToUInt32(temp2.GetAddressBytes(), 0);

            ushort srcPort = Convert.ToUInt16(srcPort_str);
            //Console.WriteLine("Convert.ToUInt16(8899)" + Convert.ToString(srcPort, 16)); //22c3
            ushort dstPort = Convert.ToUInt16(dstPort_str);
            //Console.WriteLine("Convert.ToUInt16(10000)" + Convert.ToString(dstPort, 16));//2710

            /*
            Console.WriteLine("这边接收到的数据srcPort:"+ Convert.ToString(srcPort, 16));
            Console.WriteLine("dstPort:" + Convert.ToString(dstPort, 16));
            */
            byte[] sendBuf2 = new byte[SENDBUF_SIZE];

            IpHeader ip_hd = new IpHeader();
            UdpHeader udp_hd = new UdpHeader();

            byte priority = Convert.ToByte(p);
            //Console.WriteLine("移位前byte输出：" + Convert.ToString(priority, 2));
            priority = (byte)((priority & 0x07) << 5);
            //Console.WriteLine("移位后byte输出：" + Convert.ToString(priority, 2));

            ip_hd.ip_verlen = (0x45); //version and HLength
            ip_hd.ip_tos = priority;//TOS
            ip_hd.ip_totallength = (0x2400);//IP数据包总长度-------后面还会被自动修改
            ip_hd.ip_id = (0x0000);//identification

            ip_hd.ip_offset = (0x0000);//3位flags+13位片偏移，把这里改成0040还是不行
            
            ip_hd.ip_ttl = (0x40);//TTL
            ip_hd.ip_protocol = (0x11);//传输层协议
            ip_hd.ip_HChecksum = (0x7b0c);//头部校验和---------后面会被自动修改
            ip_hd.ip_srcaddr = srcIp;//源ip
            ip_hd.ip_destaddr = dstIp;//目的ip
            //udp头--直接这样会出现字节序问题，但是ip这样又不会
            //udp_hd.srcPort = srcPort;//源端口
            //udp_hd.dstPort = dstPort;//目的端口
            udp_hd.srcPort = (0xc322);
            udp_hd.dstPort = (0x1027);
            udp_hd.totallength = (0x1000);//总长度-----------------不知道为啥，这个地方没有被自动修改--------------如果总长度字段和实际数据长度字段对应不上，wireshark会爆红
            //udp_hd.udpCheckSum = (0x4492);//验和----------------自动被修改
            udp_hd.udpCheckSum = (0x0000);//检验和

            //Console.WriteLine("udp_hd.srcPort:" + Convert.ToString(udp_hd.srcPort, 16));
            //Console.WriteLine("udp_hd.dstPort:" + Convert.ToString(udp_hd.dstPort, 16));

            sendBuf2 = byUnsafe(ip_hd, udp_hd,cmd);//获取自定义的ip包
            
            //移到全局
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);  //指示应用程序为输出输出数据报提供ip头---应该是发送的时候

            //IPEndPoint IPEPoint = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080); //

            //while (true)
            //{
                //System.Threading.Thread.Sleep(2000);
                try
                {
                    int sendSize = 0;
                    EndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.1.119"), 8080);
                    //sendTo只是将数据拷贝到内存，交给系统的网络模块，就算把网线拔了返回值依然是正常的
                    sendSize = socket.SendTo(sendBuf2, ep); //不知道IPEndPoint会不会强制转换成EndPoint

                    /*
                    Console.WriteLine(" ");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("已成功发送" + sendSize + "字节数据");
                    Console.WriteLine("sendBuf:" + ByteArrayToHexString(sendBuf2));
                    */
                    socket.Close();
                    return sendSize;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("发送错误!" + ex.Message);
                    socket.Close();
                    return -1;
                }
            //}


        }
    }
}

