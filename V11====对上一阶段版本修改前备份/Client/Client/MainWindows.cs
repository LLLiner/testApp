using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Client
{
    public partial class MainWindows : Form
    {
        //用于自定义类操作主窗口
        public static MainWindows mainForm;

        //用于控制器设置优先级
        public static int lightP = 2;
        public static int motorP = 7;
        public static int lpP = 0;
        udpIpHeader UIHSocket = new udpIpHeader();

        //用于6lowpan
        private static IPEndPoint localIpep;
        private static UdpClient udpReceive;
        Thread thrRecv;
        private bool isStart = false;


        //用于数据库中最大最小时延值
        public string constructorString = "server=localhost;User Id=root;password=1111;Database=delay";
        public int maxDelay = 10000;
        public int minDelay = 2000;
        public int maxUpDelay = 10000;
        public int minUpDelay = 2000;

        //用于曲线图
        private ChartArea chart;//显示下行时延
        private ChartArea chartA2;//显示上行时延

        /// <summary>
        /// 用于网络摄像头
        /// </summary>
        public static string ip;//这两个属性有待扩展实现
        public static string port;

        public static uint iLastErr = 0;
        public static Int32 m_lUserID = -1;
        public static bool m_bInitSDK = false;
        public static bool m_bRecord = false;
        public static bool m_bTalk = false;
        public static Int32 m_lRealHandle = -1;
        public static int lVoiceComHandle = -1;
        public static string str;

        CHCNetSDK.REALDATACALLBACK RealData = null;
        CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack = null;
        public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;
        public CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;



        /// <summary>
        /// 用于灯创建TCP Socket和数据流
        /// </summary>
        private TcpClient tclient = new TcpClient();
        private NetworkStream ns1;
        private NetworkStream ns2;

        public MainWindows()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            mainForm = this;

            //1、网络摄像头的初始化
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }

        }

        public void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            string strLoginCallBack = "登录设备，lUserID：" + lUserID + "，dwResult：" + dwResult;

            if (dwResult == 0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                strLoginCallBack = strLoginCallBack + "，错误号:" + iErrCode;
            }

            //下面代码注释掉也会崩溃
            if (InvokeRequired)
            {
                object[] paras = new object[2];
                paras[0] = strLoginCallBack;
                paras[1] = lpDeviceInfo;
            }
            else
            {
                //创建该控件的主线程直接更新信息列表 
                //UpdateClientList(strLoginCallBack, lpDeviceInfo);
            }

        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            if (dwBufSize > 0)
            {
                byte[] sData = new byte[dwBufSize];
                Marshal.Copy(pBuffer, sData, 0, (Int32)dwBufSize);

                string str = "实时流数据.ps";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sData, 0, iLen);
                fs.Close();
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //实时显示
            if (m_lUserID < 0)//如果没登录就登录
            {
                struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

                //设备IP地址或者域名
                byte[] byIP = System.Text.Encoding.Default.GetBytes("192.168.1.64");
                struLogInfo.sDeviceAddress = new byte[129];
                byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

                //设备用户名
                byte[] byUserName = System.Text.Encoding.Default.GetBytes("admin");
                struLogInfo.sUserName = new byte[64];
                byUserName.CopyTo(struLogInfo.sUserName, 0);

                //设备密码
                byte[] byPassword = System.Text.Encoding.Default.GetBytes("WLWL123456");
                struLogInfo.sPassword = new byte[64];
                byPassword.CopyTo(struLogInfo.sPassword, 0);

                struLogInfo.wPort = ushort.Parse("8000");//设备服务端口号

                if (LoginCallBack == null)
                {
                    LoginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack);//注册回调函数                    
                }
                struLogInfo.cbLoginResult = LoginCallBack;
                struLogInfo.bUseAsynLogin = false; //是否异步登录：0- 否，1- 是 

                DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

                //2、登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V40 failed, error code= " + iLastErr; //登录失败，输出错误号
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //登录成功
                    //MessageBox.Show("Login Success!");
                }

            }

            if (m_lRealHandle < 0)//如果没有实时现在需要实时
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();//预览配置信息
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口
                //lpPreviewInfo.lChannel = Int16.Parse(textBoxChannel.Text);//预te览的设备通道
                lpPreviewInfo.lChannel = Int16.Parse("1");//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 1;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库播放缓冲区最大缓冲帧数
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;


                if (RealData == null)
                {
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                }

                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //预览成功
                    btnPreview.Text = "Stop Live View";
                }
            }
            else
            {
                //停止预览 Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lRealHandle = -1;
                btnPreview.Text = "Live View";

                //退出登录
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    //停止预览 Stop live view 
                    if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                        MessageBox.Show(str);
                        return;
                    }
                    m_lRealHandle = -1;
                    btnPreview.Text = "Live View";
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lUserID = -1;

            }
            return;

        }





        /// <summary>
        /// 以下为控制灯
        /// </summary>
        /*
        private byte[] analysisRcv(byte[] src, int len)
        {
            if (len < 6) return null;
            //if (src[0] != Convert.ToInt16(txtaddr.Text)) return null;
            if (src[0] != Convert.ToInt16("254")) return null;

            switch (src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src, 6) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[3];
                        return dst;
                    }
                    break;
                case 0x02:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x05:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[4];
                        return dst;
                    }
                    break;
                case 0x0f:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = 1;
                        return dst;
                    }
                    break;
            }
            return null;
        }
        private void DebugInfo(string infotxt, byte[] info, int len = 0)
        {
            string debuginfo;
            StringBuilder builder = new StringBuilder();
            if (info != null)
            {
                if (len == 0) len = info.Length;
                //判断是否是显示为16禁止
                //依次的拼接出16进制字符串
                for (int i = 0; i < len; i++)
                {
                    builder.Append(info[i].ToString("X2") + " ");
                }
            }
            debuginfo = string.Format("{0}:{1}\r\n", infotxt, builder.ToString());
            builder.Clear();
            //因为要访问ui资源，所以需要使用invoke方式同步ui。
            this.Invoke((EventHandler)(delegate
            {
                //追加的形式添加到文本框末端，并滚动到最后。
                //txtBoxRcv.AppendText(debuginfo);
            }));
        }
        */

        //原有的直接将指令放入stream进行发送（没有构造头）
        /*
        private byte[] sendinfo(byte[] info)
        {
            if (tclient.Connected == false) return null;
            tclient.SendTimeout = 1000;
            try
            {
                ns1.Write(info, 0, info.Length);//zxl 将数据写入本套接字的NetworkStream
                DebugInfo("发送", info);

                byte[] data = new byte[2048];
                ns2.ReadTimeout = 2000;
                int len = ns2.Read(data, 0, 2048);

                DebugInfo("接收", data, len);

                return analysisRcv(data, len);
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
                //btnopen.Text = "打开";
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        */
 
        //发送查询控制器状态指令
        private void sendStatus(String srcIp_str, String srcPort_str, String dstIp_str, String dstPort_str, int p)
        {
            // byte[] cmd = CModbusDll.WriteDO(Convert.ToInt16("254"), io - 1, !isopen);
            byte[] cmd = { 0xFE, 0x04, 0x03, 0xE8, 0x00, 0x14, 0x64, 0x7A };//查询状态指令，两个控制器是一样的
            int rst = UIHSocket.send2(srcIp_str, srcPort_str, dstIp_str, dstPort_str, cmd, p);
        }

        //如果是广播（ip为255）两个子设备均会受到控制，也就是说对于两个子设备的控制指令没有差别
        private void btnDO_Click(object sender, EventArgs e) //谁触发的这个事件，sender就是谁
        {
            //将整数优先级转换为前3位为优先级的byte

            Button btn = (sender as Button);
            bool isopen = (btn.ImageIndex == 0) ? false : true;

            int io = Convert.ToInt32(btn.Tag);//btn.tag有什么作用呢？代表什么呢

            btn.ImageIndex = (isopen) ? 0 : 1;//点击之后切换灯的状态从而更新图片----修改为根据返回的状态信息显示

            //byte[] info = CModbusDll.WriteDO(Convert.ToInt16(txtaddr.Text), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            byte[] infoCmd = CModbusDll.WriteDO(Convert.ToInt16("254"), io - 1, !isopen); //获取产生的指令，指令长度是变化的

            //通过自定义ip数据包发送出去
            String srcIp = "192.168.1.200"; //直接设置成127.0.0.1控制不了灯
            String dstIp = "192.168.1.232";
            String srcPort = "8899";
            String dstPort = "10000";

            //UIHSocket = new udpIpHeader();
            int rst = UIHSocket.send2(srcIp, srcPort, dstIp, dstPort, infoCmd, lightP);

            //sendStatus(srcIp, srcPort, dstIp, dstPort, lightP);

            /*
            if (statusTimer1.Enabled == false)
            {
                statusTimer1.Enabled = true;
            }
            */
        }
        
        //和btnDO_Click相同的控制指令只是优先级、目的ip不同
        private void btnDO_Click2(object sender, EventArgs e) //谁触发的这个事件，sender就是谁
        {
            //将整数优先级转换为前3位为优先级的byte

            Button btn = (sender as Button);
            bool isopen = (btn.ImageIndex == 0) ? false : true;

            int io = Convert.ToInt32(btn.Tag);//btn.tag有什么作用呢？代表什么呢

            btn.ImageIndex = (isopen) ? 0 : 1;//点击之后切换灯的状态从而更新图片

            //byte[] info = CModbusDll.WriteDO(Convert.ToInt16(txtaddr.Text), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            byte[] info = CModbusDll.WriteDO(Convert.ToInt16("254"), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            // byte[] rst = sendinfo(info); //将指令作为数据通过tcp发送

            String srcIp = "192.168.1.200";
            String dstIp = "192.168.1.233";
            String srcPort = "8899";
            String dstPort = "10000";

            udpIpHeader UIHSocket = new udpIpHeader();
            int rst = UIHSocket.send2(srcIp, srcPort, dstIp, dstPort, info, motorP);

            //sendStatus(srcIp, srcPort, dstIp, dstPort, motorP);

            /*
            if (statusTimer2.Enabled == false)
            {
                statusTimer2.Enabled = true;
            }
            */
        }




        /// <summary>
        /// 以下为画曲线图
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        //获取数据库时延最大最小值
        private void setMData()
        {
            int min = 0;
            int max = 0;
            int interval = 0;
            
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT MAX(delay) as maxDelay,MIN(delay) as minDelay FROM time where upFlag=0 AND date_sub(now(), interval 1 minute) < time AND priority in (@lightP,@motorP);", myConnect);
            MySqlCommand myCmdUp = new MySqlCommand("SELECT MAX(delay) as maxDelay,MIN(delay) as minDelay FROM time where upFlag=1 AND date_sub(now(), interval 1 minute) < time;", myConnect);
            myCmd.Parameters.AddWithValue("@lightP", lightP);
            myCmd.Parameters.AddWithValue("@motorP", motorP);

            DataTable dt = new DataTable();
            try
            {
                //1、以ExecuteReader来执行sql--解决了判断是否为空的问题，但是不方便数据类型转换
                MySqlDataReader reader = myCmd.ExecuteReader();
                //Console.WriteLine("\n\n--------------------------");
                //Console.WriteLine("reader.HasRows:" + reader.HasRows);
                while (reader.Read())
                {
                    //当有最值数据时才替换设置范围
                    if (reader[0].ToString() != "" && reader[1].ToString() != "")
                    {
                        /*
                        Console.WriteLine(" ");
                        Console.WriteLine("从数据库读取的minDelay:" + reader["minDelay"]);
                        Console.WriteLine("从数据库读取的maxDelay:" + reader["maxDelay"]);
                        Console.WriteLine("stringMin:"+reader[1].ToString());
                        Console.WriteLine("stringMax:" + reader[0].ToString());
                        */

                        max = Convert.ToInt32(reader["maxDelay"].ToString());
                        min = Convert.ToInt32(reader["minDelay"].ToString());
                        //Console.WriteLine("min:" + min);
                        //Console.WriteLine("max:" + max);
                        max = ((max / 10) + 1) * 10;
                        min = (min / 10) * 10;
                        interval = (max - min) / 20;
                        minDelay = min;
                        if (interval > 0)
                        {
                            maxDelay = max;
                        }
                        else
                        {
                            maxDelay = minDelay + 20;
                        }
                    }
                    reader.NextResult();
                }
                reader.Close();

                chart.AxisY.Minimum = minDelay;
                chart.AxisY.Maximum = maxDelay;
                chart.AxisY.Interval = (maxDelay - minDelay) / 20;

                /*
                 * //2、这种直接执行方式，无法进行判断空值，当查询结果为空时，会报错
                MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd); 
                if (adapter != null) //当查询结果不为空时才进行操作
                {
                    adapter.Fill(dt); //因为返回的数据较简单，所以直接把数据结果放在dataTable中
                    maxDelay = Convert.ToInt32(dt.Rows[0][0].ToString());
                    minDelay = Convert.ToInt32(dt.Rows[0][1].ToString());
                    Console.WriteLine("dt.Rows[0][0].ToString()=" + dt.Rows[0][0].ToString());
                    Console.WriteLine("提取成整数后maxDelay=" + maxDelay);
                    Console.WriteLine("提取成整数后minDelay=" + minDelay);
                    
                    minDelay = ((minDelay / 1000) + 1) * 1000;
                    maxDelay = (maxDelay / 1000) * 1000;
                    chart.AxisY.Minimum = minDelay;
                    chart.AxisY.Maximum = maxDelay;
                    chart.AxisY.Interval = (maxDelay - minDelay) / 20;
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }




            //设置上行数据时延
            try
            {
                MySqlDataReader reader = myCmdUp.ExecuteReader();
                //Console.WriteLine("\n\n--------------------------");
                //Console.WriteLine("reader.HasRows:" + reader.HasRows);
                while (reader.Read())
                {
                    //当有最值数据时才替换设置范围
                    if (reader[0].ToString() != "" && reader[1].ToString() != "")
                    {
                        //Console.WriteLine("minDelay:" + reader["minDelay"]);
                        //Console.WriteLine("maxDelay:" + reader["maxDelay"]);
                        //Console.WriteLine("string" + reader[0].ToString());
                        //Console.WriteLine("string" + reader[1].ToString());

                        max = Convert.ToInt32(reader["maxDelay"].ToString());
                        min = Convert.ToInt32(reader["minDelay"].ToString());
                        max = ((max / 10) + 1) * 10;
                        min = (min / 10) * 10;
                        interval = (max - min) / 20;
                        if (interval > 10)
                        {
                            maxUpDelay = max;
                            minUpDelay = min;
                        }
                    }
                    reader.NextResult();
                }
                reader.Close();

                chartA2.AxisY.Minimum = minUpDelay;
                chartA2.AxisY.Maximum = maxUpDelay;
                chartA2.AxisY.Interval = (maxUpDelay - minUpDelay) / 20;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }

        //通过DataAdaptor存放数据（根据优先级获取下行时延数据）
        private DataTable getDBData(int p)
        {
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT * from time WHERE priority=?i AND upFlag=0 AND date_sub(now(),interval 2 minute) < time;", myConnect);
            myCmd.Parameters.AddWithValue("@i", p);

            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd);
            adapter.Fill(ds, "priorityT1");
            dt1 = ds.Tables["priorityT1"];

            
             // 显示获取到的数据库数据
            /*Console.WriteLine("共获取到" + dt1.Rows.Count + "条数据");

            foreach (DataRow dr in dt1.Rows)
            {
                Console.WriteLine("id: " + dr["id"] + " \t 优先级：" + dr["priority"] + "\t  时延：" + dr["delay"]+"\t 时间戳："+dr["time"]+ "\t upFlag:"+dr["upFlag"]);
            }
            */

            myConnect.Close();//有close，数据才会被真正写入保存在数据库
            return dt1;
        }

        //根据owner显示时延数据
        private DataTable getDBDataByOwner(int owner)
        {
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT * from time WHERE owner=?owner AND upFlag=0 AND date_sub(now(),interval 2 minute) < time;", myConnect);
            myCmd.Parameters.AddWithValue("@owner", owner);

            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd);
            adapter.Fill(ds, "priorityT1");
            dt1 = ds.Tables["priorityT1"];


            // 显示获取到的数据库数据
            /*Console.WriteLine("共获取到" + dt1.Rows.Count + "条数据");

            foreach (DataRow dr in dt1.Rows)
            {
                Console.WriteLine("id: " + dr["id"] + " \t 优先级：" + dr["priority"] + "\t  时延：" + dr["delay"]+"\t 时间戳："+dr["time"]+ "\t upFlag:"+dr["upFlag"]);
            }
            */

            myConnect.Close();//有close，数据才会被真正写入保存在数据库
            return dt1;
        }

        //获取上行所有优先级的数据
        private DataSet getUpData()
        {
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            DataSet ds = new DataSet();

            int i = 0;
            for (i = 0; i < 8; i++)
            {
                MySqlCommand myCmd = new MySqlCommand("SELECT * from time WHERE priority=?i and upFlag=1 AND date_sub(now(),interval 2 minute) < time;", myConnect);
                myCmd.Parameters.AddWithValue("@i", i);
                MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ds.Tables.Add(dt);//将获取到的表加入ds中
            }
            /*
             //* 显示获取到的数据库数据
            //Console.WriteLine("共获取到" + dt1.Rows.Count + "条数据");
            Console.WriteLine("共获取到" + ds.Tables.Count + "个数据库表");
            DataTable dt1 = new DataTable();
            for (i = 0;i < 8;i++)
            {
                dt1 = ds.Tables[i];
                Console.WriteLine(" ");
                Console.WriteLine("ds.Tables[" + i+"]:");
                foreach (DataRow dr in dt1.Rows)
                {
                    //Console.WriteLine("id: " + dr["id"] + " \t 优先级：" + dr["priority"] + "\t  时延：" + dr["delay"] + "\t 时间戳：" + dr["time"] + "\t upFlag:" + dr["upFlag"]);
                    Console.WriteLine("id: " + dr["id"] + "\t 时间戳：" + dr["time"]);
                }
            }
            */

            myConnect.Close();//有close，数据才会被真正写入保存在数据库
            return ds;
        }




        //测试输出秒的小数位
        private void test_time()
        {
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();

            MySqlCommand myCmd = new MySqlCommand("SELECT * from time WHERE priority=0 and upFlag=1;", myConnect);
            MySqlDataReader reader = myCmd.ExecuteReader();
            while (reader.Read())
            {
                //Console.WriteLine("minDelay:" + reader["minDelay"]);
                //Console.WriteLine("maxDelay:" + reader["maxDelay"]);
                Console.WriteLine(reader["id"]+ "\t " + reader["time"]+ "\t ");
                
                reader.NextResult();
            }
            reader.Close();
            
            myConnect.Close();
        }

        //计时器定时刷新曲线，达到模拟实时效果
        private void timer1_Tick(object sender, EventArgs e)
        {
            //刷新时延数据
            DataSet upDS = this.getUpData();
            int i = 0;
            for (i = 0; i < 8; i++)
            {
                this.chart2.Series[i].Points.Clear();
                chart2.Series[i].Points.DataBind(upDS.Tables[i].AsEnumerable(), "time", "delay", "");
            }
            chart2.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();
            chart2.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-60).ToOADate();


            //刷新X轴--时间轴
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-60).ToOADate();

            this.setMData(); //动态设置纵坐标范围

            //重新获取数据，绑定数据
            DataTable dtLight = getDBDataByOwner(0);//0代表灯，1代表马达
            DataTable dtMotor = getDBDataByOwner(1);
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            chart1.Series[0].Points.DataBind(dtLight.AsEnumerable(), "time", "delay", "");
            chart1.Series[1].Points.DataBind(dtMotor.AsEnumerable(), "time", "delay", "");
        }
        

        private void MainWindows_Load(object sender, EventArgs e)
        {
            //设置默认优先级
            TBPlight.Text = lightP.ToString();
            TBPmotor.Text = motorP.ToString();
            
            chart = chart1.ChartAreas[0];
            chartA2 = chart2.ChartAreas[0];
            chart.AxisY.Minimum = minDelay;
            chart.AxisY.Maximum = maxDelay;
            chart.AxisY.Interval = (maxDelay - minDelay) / 20;

            chartA2.AxisY.Minimum = minUpDelay;
            chartA2.AxisY.Maximum = maxUpDelay;
            chartA2.AxisY.Interval = (maxUpDelay - minUpDelay) / 20;

            chart1.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = true;
            chartA2.AxisY.LabelStyle.IsEndLabelVisible = true;

            //设置上行时延图格式
            chart2.Series[0].MarkerBorderColor = Color.MediumVioletRed;
            chart2.Series[0].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[0].MarkerColor = Color.White;//AxisColor
            chart2.Series[0].MarkerSize = 5;
            chart2.Series[0].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[1].MarkerBorderColor = Color.FromArgb(255, 128, 0);
            chart2.Series[1].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[1].MarkerColor = Color.White;//AxisColor
            chart2.Series[1].MarkerSize = 5;
            chart2.Series[1].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[2].MarkerBorderColor = Color.FromArgb(192, 192, 0);
            chart2.Series[2].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[2].MarkerColor = Color.White;//AxisColor
            chart2.Series[2].MarkerSize = 5;
            chart2.Series[2].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[3].MarkerBorderColor = Color.Green;
            chart2.Series[3].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[3].MarkerColor = Color.White;//AxisColor
            chart2.Series[3].MarkerSize = 5;
            chart2.Series[3].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[4].MarkerBorderColor = Color.Blue;
            chart2.Series[4].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[4].MarkerColor = Color.White;//AxisColor
            chart2.Series[4].MarkerSize = 5;
            chart2.Series[4].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[5].MarkerBorderColor = Color.FromArgb(0, 192, 192);
            chart2.Series[5].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[5].MarkerColor = Color.White;//AxisColor
            chart2.Series[5].MarkerSize = 5;
            chart2.Series[5].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[6].MarkerBorderColor = Color.Purple;
            chart2.Series[6].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[6].MarkerColor = Color.White;//AxisColor
            chart2.Series[6].MarkerSize = 5;
            chart2.Series[6].MarkerStyle = MarkerStyle.Circle;

            chart2.Series[7].MarkerBorderColor = Color.Black;
            chart2.Series[7].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart2.Series[7].MarkerColor = Color.White;//AxisColor
            chart2.Series[7].MarkerSize = 5;
            chart2.Series[7].MarkerStyle = MarkerStyle.Circle;


            //设置坐标格式
            chart2.Series[0].XValueType = ChartValueType.DateTime;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart2.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas[0].AxisY.Title = "时延（ns）";

            //------------------------------------------------------------------------------

            //标记点边框颜色      
            chart1.Series[0].MarkerBorderColor = Color.Blue;
            //标记点边框大小
            chart1.Series[0].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            //标记点中心颜色
            chart1.Series[0].MarkerColor = Color.White;//AxisColor
            //标记点大小
            chart1.Series[0].MarkerSize = 5;
            //标记点类型     
            chart1.Series[0].MarkerStyle = MarkerStyle.Circle;

            //chart1.Series[1].MarkerBorderColor = Color.FromArgb(255, 128, 255);
            chart1.Series[1].MarkerBorderColor = Color.Orange;
            chart1.Series[1].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[1].MarkerColor = Color.White;//AxisColor
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;

            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //chart1.ChartAreas[0].AxisX.LabelStyle.Format = "ss:ffffff";
            chart1.ChartAreas[0].AxisY.Title = "时延（ns）";
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;


            //数据库里面有秒的小数，横坐标也能显示显示，but。。。中间输出sql执行结果的时候输出的没有小数
            //this.test_time();


            //在窗体加载时手动查询一次控制器状态
            String srcIp = "192.168.1.200";
            String dstIp = "192.168.1.232";
            String srcPort = "8899";
            String dstPort = "10000";
            sendStatus(srcIp, srcPort, dstIp, dstPort, lightP);

            String srcIp2 = "192.168.1.200";
            String dstIp2 = "192.168.1.233";
            String srcPort2 = "8899";
            String dstPort2 = "10000";
            sendStatus(srcIp2, srcPort2, dstIp2, dstPort2, motorP);


        }
        private void MainWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0); //关闭窗口后，关闭所有相关进程
        }
        





        //以下为6lowpan控制

        //以下函数可以就写成下面一句话
        //String numStr = BitConverter.ToString(number);//输出16进制，32-34

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
        //获取温度湿度udp数据
        public void receiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpReceive.Receive(ref localIpep);
                    byte[] number = { bytRecv[4], bytRecv[5] };//这里不改字节序可以按照正常输出吗？
                    String str1 = localIpep.Address.ToString();
                    /*
                    Console.WriteLine("");
                    Console.WriteLine("6lowpan：");
                    Console.WriteLine(str1 + "--------------------------->");
                    Console.WriteLine("srcIp:" + str1);
                    Console.WriteLine("收到的数据:" + ByteArrayToHexString(bytRecv));
                    */

                    String numStr = Encoding.ASCII.GetString(number);

                    //Console.WriteLine("提取出的数值:" + numStr);

                    if (String.Compare(str1, "192.168.1.2") == 0) // 这里的ip需注意
                    {
                        if (numStr.CompareTo("00") != 0)
                        {
                            //先判断是湿度还是温度---根据第一个字母是H还是T来判断
                            if ((bytRecv[0].CompareTo(0x48) == 0))//如果是“H”
                            {
                                this.timeLabel.Text = DateTime.Now.ToString();
                                this.humLabel.Text = numStr + " %";//设置值直接赋值，不是调用函数
                            }
                            else
                            {
                                this.timeLabel.Text = DateTime.Now.ToString();
                                this.temlabel.Text = numStr + " ℃";
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            isStart = !isStart;
            if(isStart == true) //想要开始接收时
            {
                try
                {
                    localIpep = new IPEndPoint(IPAddress.Parse("192.168.1.200"), 12345); //本机IP和监听端口号
                    udpReceive = new UdpClient(localIpep); //用本机ip和端口号实例化一个UdpClient
                    thrRecv = new Thread(receiveMessage); //new一个接收数据的线程,单独创建一个线程接收数据的病修改label的话会报错
                    thrRecv.Start();
                    Console.WriteLine("UDP监听器已成功启动");
                    isStartButton.Text = "终止接收";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("6lowpan异常："+ex.Message);
                    throw;
                }
                
            }
            else
            {
                try
                {
                    udpReceive.Close();
                    thrRecv.Abort();
                    isStartButton.Text = "开始接收";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                
            }

        }


        



        private void button1_Click_1(object sender, EventArgs e)
        {
            String str = TBPlight.Text;
            if (str != null && str != "")
            {
                //Console.WriteLine("获取的优先级字符为：" + str);
                lightP = Convert.ToInt32(str);
                //Console.WriteLine("转换出来的int：" + lightP);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String str = TBPmotor.Text;
            if (str != null && str != "")
            {
                //Console.WriteLine("获取的优先级字符为：" + str);
                motorP = Convert.ToInt32(str);
                //Console.WriteLine("转换出来的int：" + motorP);
            }
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (new structureForm().ShowDialog() == DialogResult.OK)
            {
            }
            this.Show();
        }

        // 循环广播状态查询指令---修改为从本机不同的端口发出去
        private void statusTimer_Tick(object sender, EventArgs e)
        {
            String srcIp = "192.168.1.200";
            String dstIp = "192.168.1.232";
            String srcPort = "8899";
            String dstPort = "10000";
            sendStatus(srcIp, srcPort, dstIp, dstPort, lightP);
            
            ///*
            String srcIp2 = "192.168.1.200";
            String dstIp2 = "192.168.1.233";
            String srcPort2 = "8899";
            String dstPort2 = "10000";
            sendStatus(srcIp2, srcPort2, dstIp2, dstPort2, motorP);
            //*/
        }

        private void statuesTimer2_Tick(object sender, EventArgs e)
        {
            String srcIp2 = "192.168.1.200";
            String dstIp2 = "192.168.1.233";
            String srcPort2 = "8899";
            String dstPort2 = "10000";
            sendStatus(srcIp2, srcPort2, dstIp2, dstPort2, motorP);
        }
    }
}
