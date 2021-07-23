using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

/// <summary>
/// 读
/// 能获取socket缓存里的带ip头+传输层头+数据（含填充数据）的数据包，
/// 将数据流转换成16进制后和wireshark抓住的ip包一样
/// </summary>
namespace RawSocket
{
    /// <summary>
            /// @author segment
            /// </summary>

    //ps：网络层ip数据报格式如下：ip header＋××header(如TCP header) + Data
    //默认的结构体布局排序是顺序的，通过设置为explicit精确布局，可以通过设置相同的[FieldOffset(0)]、[FieldOffset(0)]达到C中共用体的效果
    [StructLayout(LayoutKind.Explicit)]
    public struct IpHeader
    {
        [FieldOffset(0)]
        public byte ip_verlen; // IP version and IP Header length
        [FieldOffset(1)]
        public byte ip_tos; // Type of service
        [FieldOffset(2)]
        public ushort ip_totallength; // total length of the packet
        [FieldOffset(4)]
        public ushort ip_id; // unique identifier
        [FieldOffset(6)]
        public ushort ip_offset; // flags and offset
        [FieldOffset(8)]
        public byte ip_ttl; // Time To Live
        [FieldOffset(9)]
        public byte ip_protocol; // protocol (TCP, UDP etc)
        [FieldOffset(10)]
        public ushort ip_checksum; //IP Header checksum
        [FieldOffset(12)]
        public uint ip_srcaddr; //Source address
        [FieldOffset(16)]
        public uint ip_destaddr;//Destination Address
    }

    class RawSocket
    {
        Socket socket;
        //String IP = "10.16.63.100";
        String IP = "192.168.1.200";

        //将ip地址存放在长度为4的对应的字节数组
        private String StandardIP(uint ip)
        {
            byte[] b_ip = new byte[4];
            b_ip[0] = (byte)(ip & 0x000000ff);
            b_ip[1] = (byte)((ip & 0x0000ff00) >> 8);
            b_ip[2] = (byte)((ip & 0x00ff0000) >> 16);
            b_ip[3] = (byte)((ip & 0xff000000) >> 24);
            return b_ip[0].ToString() + "." + b_ip[1].ToString() + "." + b_ip[2].ToString() + "." + b_ip[3].ToString();
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
            return sb.ToString().ToUpper();
        }



        //使用unsafe的话，需要设置项目生成，允许生成不安全代码
        unsafe private void ParseReceive(byte[] buffer, int size)
        {
            if (buffer == null) return;
            //使用fixed是为了兼容只存放地址的C指针---？？？没懂为啥要使用地址不变的指针--可能是方便将buffer进行对应的结构体地址强制转换
            fixed (byte* pbuffer = buffer)
            {
                IpHeader* ip_header = (IpHeader*)pbuffer;//将buffer强制类型转换成已经定义的ip头结构
                int protocol = ip_header->ip_protocol;
                uint ip_srcaddr = ip_header->ip_srcaddr, ip_destaddr = ip_header->ip_destaddr, header_len = 0;
                string out_string = "";
                short src_port = 8899, dst_port = 8899;
                IPAddress tmp_ip;
                string from_ip = "", to_ip = "";
                from_ip = ip_header->ip_srcaddr.ToString();
                switch (protocol)
                {
                    case 1: out_string = "ICMP:"; break;
                    case 2: out_string = "IGMP:"; break;
                    case 6:
                        out_string = "TCP:";
                        break;
                    case 17:
                        out_string = "UDP:";
                        break;
                    default: out_string = "UNKNOWN"; break;
                }
                //                System.Console.WriteLine(out_string + "from ip:" + IPAddress.Parse(from_ip).ToString() + " to ip:" + IPAddress.Parse(to_ip).ToString());
                //System.Console.WriteLine(out_string + "src:" + StandardIP(ip_srcaddr) + "dest:" + StandardIP(ip_destaddr));


                //判断只显示目的地址是自己的信息
                //Console.WriteLine("ip_destaddr=" + ip_destaddr); //会将32位的ip当做uint输出
                //Console.WriteLine("StandardIP(ip_destaddr)=" + StandardIP(ip_destaddr));

                int result = StandardIP(ip_destaddr).CompareTo("192.168.1.200");
                if(result == 0) //设置成只显示目的地址是自己的包信息
                {
                    Console.WriteLine(" ");
                    System.Console.WriteLine("---------------------------------------------------");
                    System.Console.WriteLine(out_string + "src:" + StandardIP(ip_srcaddr) +"-->"+ "dest:" + StandardIP(ip_destaddr));
                    System.Threading.Thread.Sleep(500);
                    //Console.WriteLine("socket.Receive(buffer)=" + Encoding.ASCII.GetString(buffer, 0, buffer.Length));//转成ascii码字符和转成16进制不一样
                    //将buffer里数据转换出来成16进制和wireshark抓的包比较发现是
                    //buffer = ip头+udp头+用户数据+填充数据
                    //socket.Receive(buffer)=45000024A25C0000801113DDC0A80177C0A801C81F9022C3001092443132333435360D0A000000000
                    Console.WriteLine("socket.Receive(buffer)=" + ByteArrayToHexString(buffer));
                    Console.WriteLine("接收到buffer.Length:" + buffer.Length);
                    Console.WriteLine("-------------------------------------------------------");
                }


            }

        }
        public void ShutDown()
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception)
                {
                    System.Console.WriteLine("关闭socket错误!");
                }

            }
        }
        public void Run()
        {
            System.Console.WriteLine("Raw Socket running...");

            //获取到的数据包只有ip头，不知道为啥有21个字节。
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);//1、创建raw socket

            //socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);   //指出发送到套接字的数据将包含IP头
            //socket.sendto

            //Linux下只是一些设置名不同 socket = new Socket(AF_INETI);//1、创建raw socket


            byte[] buffer = new byte[4096];
            int rcv_size = 0;
            // socket.Blocking = false;
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP), 0));//2、绑定ip和端口号
            while (true)
            {
                try
                {
                    //socket.BeginReceive(buffer, 0, 10, SocketFlags.None, Callback, null);
                    rcv_size = socket.Receive(buffer);//3、接收数据到buffer
                    ParseReceive(buffer, rcv_size);// 对收到的缓存剥离出ip头部信息。数据部分呢？？？？--数据部分在ip头后面的
                    

                }
                catch (Exception e)
                {
                    System.Console.WriteLine("异常：" + e.Message);
                    return;
                }
                //System.Threading.Thread.Sleep(500);
                //System.Console.WriteLine("接收到:" + rcv_size.ToString());
            }
        }
        ~RawSocket()
        {
            ShutDown();
        }
    }
}
