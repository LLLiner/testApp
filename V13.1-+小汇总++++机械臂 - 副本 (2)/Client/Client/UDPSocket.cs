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

using System.Net.NetworkInformation; //ping测量

namespace Client
{
    class UDPSocket
    {
        //用于输出调试信息
        static int debug = 0;

        //不断从数据库获取数据库里时延最大值和最小值
        public int minDelay = 2000;
        public int maxDelay = 10000;

        //用于UDP接收的网络服务类
        static UdpClient udpcRecv = null;
        static UdpClient udpcRecvOffset = null;

        static IPEndPoint localIpep = null;//表示IP和端口号的类
        static IPEndPoint localIpepOffset = null;

        //开关：在监听UDP报文阶段为true，否则为false（用一个flag记录是否在监听udp）
        static bool isUdpRecvStart = false;

        //线程：不断监听UDP报文
        static Thread thrRecv;
        static Thread thrRecvOffset;

        //开启监听线程
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

                //接收offset线程
                /*
                localIpepOffset = new IPEndPoint(IPAddress.Parse("192.168.1.200"), 10000);
                udpcRecvOffset = new UdpClient(localIpepOffset);
                thrRecvOffset = new Thread(receiveOffsetValue); //new一个接收数据的线程
                thrRecvOffset.Start();
                Console.WriteLine("UDP监听器已成功启动");
                */
            }
        }

        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString();
        }







        private static void receiveOffsetValue()
        {
            byte[] bytRecv = udpcRecv.Receive(ref localIpepOffset);
            String addressStr = localIpep.Address.ToString();

            byte[] flags = new byte[4];
            flags[0] = bytRecv[0];
            flags[1] = bytRecv[1];
            flags[2] = bytRecv[2];
            flags[3] = bytRecv[3];
            byte slot = bytRecv[4];
            byte[] master = new byte[4];
            /*
            master[0] = bytRecv[5];
            master[1] = bytRecv[6];
            master[2] = bytRecv[7];
            master[3] = bytRecv[8];
            */
            master[0] = bytRecv[8];
            master[1] = bytRecv[7];
            master[2] = bytRecv[6];
            master[3] = bytRecv[5];
            if (String.Compare(addressStr, "192.168.1.201") == 0 )
            {

            }

        }












      

        //接收数据----------直接调用UdpClient的接收接口获取字节数组，然后将字节数组转换成utf8的字符串
        private static void receiveMessage(object obj) //object就是Object
        {
            byte[] delayBytes = new byte[8];
            while (isUdpRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep); //使用了ref之后传递的是引用
                    String str1 = localIpep.Address.ToString();

                    //如果是时延数据。以下是对返回的时延数据进行处理
                    if (String.Compare(str1, "192.168.1.120") == 0)
                    {
                        if(debug == 1)
                        {
                            //test
                            Console.WriteLine("-------------------------------------------------------------------");
                            Console.WriteLine("收到的测试数据（16进制）：" + BitConverter.ToString(bytRecv));
                            Console.WriteLine("--------------------------------------------------------------------");
                            Console.WriteLine(" ");
                        }
                        



                        //bytRecv里面是网络字节序，在将数据放在delayBytes里转换成10进制的时候，需要存放顺序，主机一般采用的是小端法
                        delayBytes[0] = bytRecv[6];
                        delayBytes[1] = bytRecv[5];
                        delayBytes[2] = bytRecv[4];
                        delayBytes[3] = bytRecv[3];
                        delayBytes[4] = bytRecv[2];
                        delayBytes[5] = bytRecv[1];
                        delayBytes[6] = 0x00; //这是为了方便后面直接转换成int64而填充的--高位补0，不影响值
                        delayBytes[7] = 0x00;
                        
                        ulong delayInt = BitConverter.ToUInt64(delayBytes, 0);
                        uint offsetInt = (uint)(bytRecv[12]);//虽然传过来的offset有48位但是数值不可能这么大，8位足够了

                        if(debug == 1)
                        {
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("提取的offset：" + offsetInt);
                        }
                        

                        if (offsetInt != 255)
                        {
                            offsetInt = offsetInt * 8;
                            string conStr2 = "server=localhost;User Id=root;password=1111;Database=delay;";
                            using (MySqlConnection connection2 = new MySqlConnection(conStr2))
                            {
                                try
                                {
                                    string cmdText2 = "INSERT INTO offset(offsetValue) VALUES (@offsetValue);";
                                    MySqlCommand cmd2 = new MySqlCommand(cmdText2, connection2);
                                    cmd2.Parameters.AddWithValue("@offsetValue", offsetInt);
                                    connection2.Open();
                                    int result2 = cmd2.ExecuteNonQuery();
                                    connection2.Close();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("插入offsetValue出错啦！"+ex.Message);
                                    throw;
                                }
                            }
                                    
                        }


                        // 时延处理部分
                        if (delayInt < 100000000)
                        {
                            byte priority = (byte)(bytRecv[0] & 0x07);
                            int upFlag = bytRecv[0] & 0xf8; //判断是上行时延还是下行时延
                                                            //Console.WriteLine("bytRecv[0]:" + bytRecv[0].ToString());

                            if (upFlag == 248) //如果是前五位是11111就是下行数据时延
                            {
                                upFlag = 0;
                            }
                            if (upFlag == 168) //如果是前五位是10101就是上行数据时延
                            {
                                upFlag = 1;
                            }
                            int p = bytRecv[0] & 0x07;

                            /*
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
                            */
                            ///*
                            if(debug == 1)
                            {
                                Console.WriteLine("求取的时延整数:BitConverter.ToUInt64(delayBytes, 0)=" + delayInt);
                                Console.WriteLine("求取的时延delayInt*8(ns):" + delayInt * 8);
                                Console.WriteLine("");
                            }
                            
                            //Console.WriteLine("------------------------");
                            //Console.WriteLine("接收到的字节流：");
                            //for (int i = 0; i < 7; i++)
                            //{
                            //    Console.Write(bytRecv[i].ToString("X2") + "-");
                            //}
                            //Console.WriteLine("------------------------");
                            //Console.Read();
                            //*/

                            string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);//这里的接收到的数据
                            //Console.WriteLine(string.Format("{0}[{1}]", localIpep, message));
                            int owner = -1;
                            //将时延和优先级放入数据库
                            //string conStr = "server=localhost;User Id=root;password=1111;Database=delay;SslMode=none";
                            string conStr = "server=localhost;User Id=root;password=1111;Database=delay;";
                            using (MySqlConnection connection = new MySqlConnection(conStr))
                            {
                                try
                                {
                                    if(priority == MainWindows.lightP)
                                    {
                                        owner = 0;
                                    }
                                    if(priority == MainWindows.motorP)
                                    {
                                        owner = 1;
                                    }
                                    if (priority == MainWindows.armP)
                                    {
                                        owner = 2;
                                    }
                                    string cmdText = "INSERT INTO time(priority,delay,upFlag,owner) VALUES (@priority,@delay,@upFlag,@owner);";
                                    //string cmdText = "INSERT INTO time(priority,delay) VALUES (3,66);";
                                    MySqlCommand cmd = new MySqlCommand(cmdText, connection);
                                    cmd.Parameters.AddWithValue("@priority", priority);
                                    cmd.Parameters.AddWithValue("@delay", delayInt * 8);
                                    cmd.Parameters.AddWithValue("@upFlag", upFlag);
                                    cmd.Parameters.AddWithValue("@owner", owner);
                                    connection.Open();
                                    
                                    int result = cmd.ExecuteNonQuery();
                                    connection.Close();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("插入时延出错啦！"+ex.Message);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("delayInt太大了(>100000000)，已被丢弃："+ delayInt);
                        }
                        continue;
                    }
                    
                    //显示控制器返回的状态数据
                    if(String.Compare(str1, "192.168.1.232") == 0 && bytRecv.Length > 8)
                    {
                        /*
                        Console.WriteLine("bytRecv:"+ BitConverter.ToString(bytRecv));
                        Console.WriteLine(bytRecv[7]);
                        */

                        switch(bytRecv[7])
                        {
                            case 0:   //关 关
                                MainWindows.mainForm.btnDO1.ImageIndex = 0;
                                MainWindows.mainForm.btnDO2.ImageIndex = 0;
                                break;
                            case 1:   //开 关
                                MainWindows.mainForm.btnDO1.ImageIndex = 1;
                                MainWindows.mainForm.btnDO2.ImageIndex = 0;
                                break;
                            case 2:   //关 开
                                MainWindows.mainForm.btnDO1.ImageIndex = 0;
                                MainWindows.mainForm.btnDO2.ImageIndex = 1;
                                break;
                            case 3:   //开 开
                                MainWindows.mainForm.btnDO1.ImageIndex = 1;
                                MainWindows.mainForm.btnDO2.ImageIndex = 1;
                                break;
                        }
                        continue;
                    }

                    if (String.Compare(str1, "192.168.1.233") == 0 && bytRecv.Length > 8)
                    {
                        switch (bytRecv[7])
                        {
                            case 0:   //关 关
                                MainWindows.mainForm.btnDO3.ImageIndex = 0;
                                MainWindows.mainForm.btnDO4.ImageIndex = 0;
                                break;
                            case 1:   //开 关
                                MainWindows.mainForm.btnDO3.ImageIndex = 1;
                                MainWindows.mainForm.btnDO4.ImageIndex = 0;
                                break;
                            case 2:   //关 开
                                MainWindows.mainForm.btnDO3.ImageIndex = 0;
                                MainWindows.mainForm.btnDO4.ImageIndex = 1;
                                break;
                            case 3:   //开 开
                                MainWindows.mainForm.btnDO3.ImageIndex = 1;
                                MainWindows.mainForm.btnDO4.ImageIndex = 1;
                                break;
                        }
                        continue;
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
