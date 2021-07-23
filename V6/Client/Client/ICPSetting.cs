using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ICPSetting : Form
    {

        //用于网络摄像头
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private bool m_bInitSDK = false;
        private bool m_bRecord = false;
        private bool m_bTalk = false;
        private Int32 m_lRealHandle = -1;
        private int lVoiceComHandle = -1;
        private string str;

        CHCNetSDK.REALDATACALLBACK RealData = null;
        CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack = null;
        public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;
        public CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;

        public ICPSetting()
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
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if (m_lUserID < 0)
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
                    btnLogin.Text = "Logout";
                }

            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    MessageBox.Show("Please stop live view firstly");
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lUserID = -1;
                btnLogin.Text = "Login";
            }
            return;
        }

        
    }
}
