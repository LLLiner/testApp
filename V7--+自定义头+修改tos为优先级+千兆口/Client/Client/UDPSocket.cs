using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class UDPSocket
    {
        //不断从数据库获取数据库里时延最大值和最小值
        public int minDelay = 2000;
        public int maxDelay = 10000;

        //用于UDP接收的网络服务类
        static UdpClient udpcRecv = null;

        static IPEndPoint localIpep = null;//表示IP和端口号的类

        //开关：在监听UDP报文阶段为true，否则为false（用一个flag记录是否在监听udp）
        static bool isUdpRecvStart = false;

        //线程：不断监听UDP报文
        static Thread thrRecv;

        public static void startReceive()
        {
            if (!isUdpRecvStart) //未监听的情况，开始监听
            {
                //localIpep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8899); //本机IP和监听端口号
                localIpep = new IPEndPoint(IPAddress.Parse("192.168.1.200"), 8899); //本机IP和监听端口号
                udpcRecv = new UdpClient(localIpep); //用本机ip和端口号实例化一个UdpClient
                thrRecv = new Thread(receiveMessage); //new一个接收数据的线程
                thrRecv.Start();
                isUdpRecvStart = true;
                Console.WriteLine("UDP监听器已成功启动");
            }
        }


        //将byte[]右移n位--暂时未用，有问题
        public static byte[] RightShift(byte[] ba, int n)
        {
            /*
            if (n < 0)
            {
                return ba.LeftShift(Math.Abs(n));
            }
            */
            byte[] ba2 = null;
            ba2 = ba.Clone() as byte[];
            int loop = (int)Math.Ceiling(n / 8.0);
            byte tempByte = 0;
            byte tempByte2 = 0;
            byte Header = 0;

            for (int i = 0; i < loop; i++)
            {
                var tempN = i + 1 == loop ? n % 8 : 8;
                if (tempN == 0 && n != 0)
                {
                    tempN = 8;
                }
                for (int j = 0; j < ba.Length; j++)
                {
                    if (j == 0)
                    {
                        Header = (byte)((ba2.First() & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        tempByte = (byte)((ba2[ba.Length - 1 - j] & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        ba2[ba.Length - 1 - j] >>= tempN;
                    }
                    else
                    {
                        tempByte2 = (byte)((ba2[ba.Length - 1 - j] & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        ba2[ba.Length - 1 - j] >>= tempN;
                        ba2[ba.Length - 1 - j] |= tempByte;
                        tempByte = tempByte2;
                        if (j + 1 == ba.Length)
                        {
                            ba2[j] |= Header;
                        }
                    }
                }
            }
            return ba2;
        }

        // 将byte数组左移n位--有问题
        public static byte[] LeftShift(byte[] ba, int n)
        {
            /*
            if (n < 0)
            {
                return ba.RightShift(Math.Abs(n));
            }
            */
            byte[] ba2 = null;
            ba2 = ba.Clone() as byte[];
            int loop = (int)Math.Ceiling(n / 8.0);
            byte tempByte = 0;
            byte tempByte2 = 0;
            byte Header = 0;

            for (int i = 0; i < loop; i++)
            {
                var tempN = i + 1 == loop ? n % 8 : 8;
                if (tempN == 0 && n != 0)
                {
                    tempN = 8;
                }
                for (int j = 0; j < ba.Length; j++)
                {
                    if (j == 0)
                    {
                        Header = (byte)(ba2.Last() & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        tempByte = (byte)(ba2[j] & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        ba2[j] <<= tempN;
                    }
                    else
                    {
                        tempByte2 = (byte)(ba2[j] & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        ba2[j] <<= tempN;
                        ba2[j] |= (byte)(tempByte >> (8 - tempN));
                        tempByte = tempByte2;
                        if (j + 1 == ba.Length)
                        {
                            ba2[0] |= (byte)(Header >> (8 - tempN));
                        }
                    }
                }
            }
            return ba2;
        }

        

        //接收数据----------直接调用UdpClient的接收接口获取字节数组，然后将字节数组转换成utf8的字符串
        private static void receiveMessage(object obj) //object就是Object
        {
            while (isUdpRecvStart)
            {
                try
                {
                    //这里的receive接口是将远程主机信息放在被引用的形参中，返回用户数据
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep); //使用了ref之后传递的是引用
                    
                    String str1 = localIpep.Address.ToString();
                    
                    if(String.Compare(str1, "192.168.1.120") == 0)
                    {

                        byte priority = (byte)(bytRecv[0] & 0x07);
                        int p = bytRecv[0] & 0x07;
                        byte[] delayBytes = new byte[8];
                        //bytRecv里面是网络字节序，在将数据放在delayBytes里转换成10进制的时候，需要存放顺序，主机一般采用的是小端法
                        delayBytes[0] = bytRecv[6];
                        delayBytes[1] = bytRecv[5];
                        delayBytes[2] = bytRecv[4];
                        delayBytes[3] = bytRecv[3];
                        delayBytes[4] = bytRecv[2];
                        delayBytes[5] = bytRecv[1];
                        delayBytes[6] = 0x00;
                        delayBytes[7] = 0x00;

                        Console.WriteLine(" ");
                        Console.WriteLine(" ");
                        Console.WriteLine("*****************************************************************************");
                        Console.WriteLine("提取的优先级二进制为：" + Convert.ToString(priority, 2));
                        Console.WriteLine("提取的优先级为：" + p);
                        Console.WriteLine("原始接收到的数据：");
                        for (int i = 0; i < 8; i++)
                        {
                            Console.WriteLine(Convert.ToString(bytRecv[i], 2));
                        }
                        Console.WriteLine("提取出的时延数据：");
                        for (int i = 0; i < 8; i++)
                        {
                            Console.WriteLine(Convert.ToString(delayBytes[i], 2));
                        }
                        ulong delayInt = BitConverter.ToUInt64(delayBytes, 0);
                        Console.WriteLine("求取的时延整数:BitConverter.ToUInt64(delayBytes, 0)=" + delayInt);
                        Console.WriteLine("求取的时延delayInt*8(ns):" + delayInt * 8);

                        Console.WriteLine("------------------------");
                        Console.WriteLine("接收到的字节流：");
                        for (int i = 0; i < 7; i++)
                        {
                            //Console.WriteLine(bytRecv[i]+"-");
                            Console.Write(bytRecv[i].ToString("X2") + "-");
                        }
                        Console.WriteLine("------------------------");
                        Console.Read();


                        string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);//这里的接收到的数据
                        Console.WriteLine(string.Format("{0}[{1}]", localIpep, message));
                        

                        //将时延和优先级放入数据库
                        string conStr = "server=localhost;User Id=root;password=1111;Database=delay";
                        using (MySqlConnection connection = new MySqlConnection(conStr))
                        {
                            try
                            {
                                string cmdText = "INSERT INTO time(priority,delay) VALUES (@priority,@delay);";
                                //string cmdText = "INSERT INTO time(priority,delay) VALUES (3,66);";
                                MySqlCommand cmd = new MySqlCommand(cmdText, connection);
                                cmd.Parameters.AddWithValue("@priority", priority);
                                cmd.Parameters.AddWithValue("@delay", delayInt);
                                connection.Open();
                                int result = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }







                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        //关闭接收-------关闭线程，关闭UdpClient，将flag置为关
        public static void stopReceive()
        {
            if (isUdpRecvStart)
            {
                thrRecv.Abort();//必须先关闭这个线程，否则会异常
                udpcRecv.Close();
                isUdpRecvStart = false;
                Console.WriteLine("UDP监听已成功关闭");
            }
        }
    }
}
