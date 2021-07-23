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
        //用于曲线图
        //private static ChartArea chart;

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





            /*
            MyThreadClass mt = new MyThreadClass(i);
            ThreadStart threadStart = new ThreadStart(mt.startThread);
            Thread thread = new Thread(threadStart);
            thread.Start();
            */

            //ThreadStart ts


            //使用多线程---测试中
            /*
            UDPSocket udpDelay = new UDPSocket();
            ThreadStart threadStart = new ThreadStart(UDPSocket.startReceive());
            Thread thread = new Thread(threadStart);
            thread.Start();
            */
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (new ICPSetting().ShowDialog() == DialogResult.OK)
            {
            }
            this.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (new ICPSetting().ShowDialog() == DialogResult.OK)
            {
            }
            this.Show();
        }

        private void RealPlayWnd_Click(object sender, EventArgs e)
        {

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
                //labelLogin.BeginInvoke(new UpdateTextStatusCallback(UpdateClientList), paras);
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
                //MessageBox.Show("Please login the device firstly");
                //return;
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

                //登录设备 Login the device
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
                    MessageBox.Show("Login Success!");
                    //btnLogin.Text = "Logout";
                }

            }

            if (m_lRealHandle < 0)//如果没有实时现在需要实时
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口
                //lpPreviewInfo.lChannel = Int16.Parse(textBoxChannel.Text);//预te览的设备通道
                lpPreviewInfo.lChannel = Int16.Parse("1");//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库播放缓冲区最大缓冲帧数
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                /*if (textBoxID.Text != "")
                {
                    lpPreviewInfo.lChannel = -1;
                    byte[] byStreamID = System.Text.Encoding.Default.GetBytes(textBoxID.Text);
                    lpPreviewInfo.byStreamID = new byte[32];
                    byStreamID.CopyTo(lpPreviewInfo.byStreamID, 0);
                }*/


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
                    //MessageBox.Show("Please stop live view firstly");
                    //return;
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
        private void btnDO_Click(object sender, EventArgs e) //谁触发的这个事件，sender就是谁
        {
            //将整数优先级转换为前3位为优先级的byte
            int p = 1;

            Button btn = (sender as Button);
            bool isopen = (btn.ImageIndex == 0) ? false : true;

            int io = Convert.ToInt32(btn.Tag);//btn.tag有什么作用呢？代表什么呢

            btn.ImageIndex = (isopen) ? 0 : 1;//点击之后切换灯的状态从而更新图片

            //byte[] info = CModbusDll.WriteDO(Convert.ToInt16(txtaddr.Text), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            byte[] info = CModbusDll.WriteDO(Convert.ToInt16("254"), io - 1, !isopen); //获取产生的指令，指令长度是变化的
                                                                                       // byte[] rst = sendinfo(info); //将指令作为数据通过tcp发送

            
            //通过自定义ip数据包发送出去
            String srcIp = "192.168.1.200";
            //String dstIp = "192.168.1.232";
            String dstIp = "192.168.1.255";
            String srcPort = "8899";
            String dstPort = "10000";
            
            /*
            //通过自定义ip数据包发送出去
            IPAddress temp1 = IPAddress.Parse("192.168.1.200");
            UInt32 srcIp = BitConverter.ToUInt32(temp1.GetAddressBytes(), 0);
            IPAddress temp2 = IPAddress.Parse("192.168.1.232");
            UInt32 dstIp = BitConverter.ToUInt32(temp2.GetAddressBytes(), 0);

            ushort srcPort = Convert.ToUInt16("8899");
            //Console.WriteLine("Convert.ToUInt16(8899)" + Convert.ToString(srcPort, 16)); //22c3
            ushort dstPort = Convert.ToUInt16("10000");
            //Console.WriteLine("Convert.ToUInt16(10000)" + Convert.ToString(dstPort, 16));//2710
            */
            
            udpIpHeader UIHSocket = new udpIpHeader();
            int rst = UIHSocket.send2(srcIp, srcPort, dstIp, dstPort, info, p);

        }


        private void btnDO_Click2(object sender, EventArgs e) //谁触发的这个事件，sender就是谁
        {
            //将整数优先级转换为前3位为优先级的byte
            int p = 2;

            Button btn = (sender as Button);
            bool isopen = (btn.ImageIndex == 0) ? false : true;

            int io = Convert.ToInt32(btn.Tag);//btn.tag有什么作用呢？代表什么呢

            btn.ImageIndex = (isopen) ? 0 : 1;//点击之后切换灯的状态从而更新图片

            //byte[] info = CModbusDll.WriteDO(Convert.ToInt16(txtaddr.Text), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            byte[] info = CModbusDll.WriteDO(Convert.ToInt16("254"), io - 1, !isopen); //获取产生的指令，指令长度是变化的
                                                                                       // byte[] rst = sendinfo(info); //将指令作为数据通过tcp发送

            String srcIp = "192.168.1.200";
            //String dstIp = "192.168.1.233";
            String dstIp = "192.168.1.255";
            String srcPort = "8899";
            String dstPort = "10000";


            udpIpHeader UIHSocket = new udpIpHeader();
            int rst = UIHSocket.send2(srcIp, srcPort, dstIp, dstPort, info, p);

        }

        /*
        private void btnopen_Click(object sender, EventArgs e)
        {
            try
            {
                if (tclient.Connected == false)
                {
                    //tclient.Connect(ip1.Text, Convert.ToInt32(txtport.Text));
                    tclient.Connect("192.168.1.232", Convert.ToInt32("10000"));
                    if (tclient.Connected == true)
                        btnopen.Text = "关闭连接";

                    ns1 = tclient.GetStream();
                    ns2 = tclient.GetStream();
                }
                else
                {
                    tclient.Close();
                    tclient = new TcpClient();
                    btnopen.Text = "打开连接";
                }
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
                MessageBox.Show(ex.Message);
            }
        }
        */




        /// <summary>
        /// 以下为画曲线图
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        //通过DataAdaptor存放数据
        private DataTable getDBData(int p)
        {
            string constructorString = "server=localhost;User Id=root;password=1111;Database=delay";
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT * from time WHERE priority=?i;", myConnect);
            myCmd.Parameters.AddWithValue("@i", p);
            //Console.WriteLine(myCmd.CommandText);

            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd);
            adapter.Fill(ds, "priorityT1");
            dt1 = ds.Tables["priorityT1"];

            /*
             * 显示获取到的数据库数据
            Console.WriteLine("共获取到" + dt1.Rows.Count + "条数据");

            foreach (DataRow dr in dt1.Rows)
            {
                Console.WriteLine("id: " + dr["id"] + " \t 优先级：" + dr["priority"] + "\t  时延：" + dr["delay"]+"\t 时间戳："+dr["time"]);
            }
            */

            myConnect.Close();//有close，数据才会被真正写入保存在数据库
            return dt1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt0 = getDBData(0);
            DataTable dt1 = getDBData(1);
            DataTable dt2 = getDBData(2);

            ChartArea chart = chart1.ChartAreas[0];
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "";
            chart1.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = true;

            chart.AxisY.Minimum = 2000;
            chart.AxisY.Maximum = 6000;
            chart.AxisY.Interval = 500;
            
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
    
            chart1.Series[1].MarkerBorderColor = Color.Black;
            chart1.Series[1].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[1].MarkerColor = Color.White;//AxisColor
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
            
            //使用datatable绑定
            chart1.Series[0].Points.DataBind(dt0.AsEnumerable(), "time", "delay", "");
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //毫秒格式： hh:mm:ss.fff ，后面几个f则保留几位毫秒小数，此时要注意轴的最大值和最小值不要差太大


            //以下两行出现矛盾，会都会时间数据显示不出来
            //chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Milliseconds;
            //chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 100;
            
            chart1.Series[1].Points.DataBind(dt1.AsEnumerable(), "time", "delay", "");
            chart1.Series[1].IsVisibleInLegend = true;//???
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ChartArea chart = chart1.ChartAreas[0];
            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "";
            chart1.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = true;

            chart.AxisY.Minimum = 2000;
            chart.AxisY.Maximum = 6000;
            chart.AxisY.Interval = 500;

           
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

            chart1.Series[1].MarkerBorderColor = Color.Black;
            chart1.Series[1].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[1].MarkerColor = Color.White;//AxisColor
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;

            chart1.Series[2].MarkerBorderColor = Color.Gray;
            chart1.Series[2].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[2].MarkerColor = Color.White;//AxisColor
            chart1.Series[2].MarkerSize = 5;
            chart1.Series[2].MarkerStyle = MarkerStyle.Circle;
            
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            
            //刷新X轴--时间轴
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;

            //没有直接写成的减函数，用在加函数里面加一个负数实现减法
            chart1.ChartAreas[0].AxisX.Maximum= DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-600).ToOADate();

            



            //获取最大最小值更新纵坐标范围
            /*
            string constructorString = "server=localhost;User Id=root;password=1111;Database=delay";
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT MAX(delay) as maxDelay,MIN(delay) as minDelay FROM time;", myConnect);
            */
            


            

            //重新获取数据，绑定数据
            DataTable dt0 = getDBData(0);
            DataTable dt1 = getDBData(1);
            DataTable dt2 = getDBData(2);

            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();

            chart1.Series[0].Points.DataBind(dt0.AsEnumerable(), "time", "delay", "");
            chart1.Series[1].Points.DataBind(dt1.AsEnumerable(), "time", "delay", "");
            chart1.Series[2].Points.DataBind(dt2.AsEnumerable(), "time", "delay", "");
        }

        private void MainWindows_Load(object sender, EventArgs e)
        {
            

        }
        

        private void MainWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MainWindows_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
