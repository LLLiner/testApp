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
                Console.WriteLine("UDP监听器已成功启动");
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
                    //这里的receive接口是将远程主机信息放在被引用的形参中，返回用户数据
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep); //使用了ref之后传递的是引用
                    
                    String str1 = localIpep.Address.ToString();
                    
                    //如果是时延数据。以下是对返回的时延数据进行处理
                    if (String.Compare(str1, "192.168.1.120") == 0)
                    {
                        //bytRecv里面是网络字节序，在将数据放在delayBytes里转换成10进制的时候，需要存放顺序，主机一般采用的是小端法
                        delayBytes[0] = bytRecv[6];
                        delayBytes[1] = bytRecv[5];
                        delayBytes[2] = bytRecv[4];
                        delayBytes[3] = bytRecv[3];
                        delayBytes[4] = bytRecv[2];
                        delayBytes[5] = bytRecv[1];
                        delayBytes[6] = 0x00;
                        delayBytes[7] = 0x00;

                        ulong delayInt = BitConverter.ToUInt64(delayBytes, 0);

                        if(delayInt < 1000000)
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
                            /*
                            Console.WriteLine("求取的时延整数:BitConverter.ToUInt64(delayBytes, 0)=" + delayInt);
                            Console.WriteLine("求取的时延delayInt*8(ns):" + delayInt * 8);
                            Console.WriteLine("------------------------");
                            Console.WriteLine("接收到的字节流：");
                            for (int i = 0; i < 7; i++)
                            {
                                Console.Write(bytRecv[i].ToString("X2") + "-");
                            }
                            Console.WriteLine("------------------------");
                            Console.Read();
                            */

                            string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);//这里的接收到的数据
                            //Console.WriteLine(string.Format("{0}[{1}]", localIpep, message));

                            //将时延和优先级放入数据库
                            string conStr = "server=localhost;User Id=root;password=1111;Database=delay";
                            using (MySqlConnection connection = new MySqlConnection(conStr))
                            {
                                try
                                {
                                    string cmdText = "INSERT INTO time(priority,delay,upFlag) VALUES (@priority,@delay,@upFlag);";
                                    //string cmdText = "INSERT INTO time(priority,delay) VALUES (3,66);";
                                    MySqlCommand cmd = new MySqlCommand(cmdText, connection);
                                    cmd.Parameters.AddWithValue("@priority", priority);
                                    cmd.Parameters.AddWithValue("@delay", delayInt * 8);
                                    cmd.Parameters.AddWithValue("@upFlag", upFlag);
                                    connection.Open();
                                    int result = cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        continue;
                    }
                    
                    
                    
                    //显示控制器返回的状态数据
                    if(String.Compare(str1, "192.168.1.232") == 0 && bytRecv.Length > 8)
                    {
                        /*
                        Console.WriteLine("bytRecv:"+ BitConverter.ToString(bytRecv));
                        Console.WriteLine(bytRecv[7].Equals(0x00));
                        Console.WriteLine(bytRecv[7].Equals(0x01));
                        Console.WriteLine(bytRecv[7].Equals(0x02));
                        Console.WriteLine(bytRecv[7].Equals(0x03));

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
                        /*
                        Console.WriteLine("bytRecv:" + BitConverter.ToString(bytRecv));
                        Console.WriteLine(bytRecv[7].Equals(0x00));
                        Console.WriteLine(bytRecv[7].Equals(0x01));
                        Console.WriteLine(bytRecv[7].Equals(0x02));
                        Console.WriteLine(bytRecv[7].Equals(0x03));

                        Console.WriteLine(bytRecv[7]);
                        */

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
