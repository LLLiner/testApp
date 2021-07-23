using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using Client.Properties;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace Client
{
    public partial class Frmmain : Form
    {
        private TcpClient tclient = new TcpClient();
        private NetworkStream ns1;
        private NetworkStream ns2;
        

        public Frmmain()
        {
            InitializeComponent();
        }

        private void Frmmain_Load(object sender, EventArgs e)
        {
            // zxl 
            // 从实例化的Setting对象中读取值，设置界面默认显示数据
            Settings.Default.Reload();
            ip1.Text = Settings.Default["ip"].ToString();
            txtport.Text=Settings.Default["port"].ToString();
            txtaddr.Text = Settings.Default["addr"].ToString();
            txtDONum.Text = Settings.Default["donum"].ToString();
            txtDINum.Text = Settings.Default["dinum"].ToString();

            
        }

        //开关一个连接
        private void btnopen_Click(object sender, EventArgs e)
        {
            try
            {
                if (tclient.Connected == false)
                {               
                    tclient.Connect(ip1.Text, Convert.ToInt32(txtport.Text));
                    if (tclient.Connected == true)
                        btnopen.Text = "关闭";

                    ns1 = tclient.GetStream();//。  ？？？？？？？接收数据，没懂，刚连接好怎么接收数据--------获取绑定本套接字的网络流，方便后续直接对网络流操作
                    ns2 = tclient.GetStream();
                }
                else
                {
                    tclient.Close();
                    tclient = new TcpClient();//。  如果关闭上一个连接之后需要重新实例化一个TcpClient
                    btnopen.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
                MessageBox.Show(ex.Message);
            }
        }

        /*private void btnClear_Click(object sender, EventArgs e)
        {
            txtBoxRcv.Clear();
        }*/

    

        private void Frmmain_FormClosed(object sender, FormClosedEventArgs e)
        {
            tclient.Close();

            Settings.Default["ip"] = ip1.Text;
            Settings.Default["port"] = txtport.Text;
            Settings.Default["addr"] = txtaddr.Text;
            Settings.Default["donum"] = txtDONum.Text;
            Settings.Default["dinum"] = txtDINum.Text;
            
            Settings.Default.Save();
        }

        //点击单个灯开关操作
        private void btnDO_Click(object sender, EventArgs e)  //谁触发的这个事件，sender就是谁
        {
            Button btn = (sender as Button);
            bool isopen = (btn.ImageIndex == 0) ? false : true;

            int io = Convert.ToInt32(btn.Tag);//btn.tag有什么作用呢？代表什么呢

            btn.ImageIndex = (isopen) ? 0 : 1;//点击之后切换灯的状态从而更新图片

            byte[] info = CModbusDll.WriteDO(Convert.ToInt16(txtaddr.Text), io - 1, !isopen); //获取产生的指令，指令长度是变化的
            byte[] rst = sendinfo(info); //将指令作为数据发送出去
            if (rst!=null)
            {
                btn.ImageIndex = (rst[0] == 0) ? 0 : 1;
            }
            
        }
        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            byte[] info = CModbusDll.WriteAllDO(Convert.ToInt16(txtaddr.Text), Convert.ToInt16(txtDONum.Text), false);
            byte[] rst = sendinfo(info);
            if (rst != null)
            {
                btnDO1.ImageIndex = 0;
                btnDO2.ImageIndex = 0;
                btnDO3.ImageIndex = 0;
                btnDO4.ImageIndex = 0;
                btnDO5.ImageIndex = 0;
                btnDO6.ImageIndex = 0;
                btnDO7.ImageIndex = 0;
                btnDO8.ImageIndex = 0;
            }
        }

        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            byte[] info = CModbusDll.WriteAllDO(Convert.ToInt16(txtaddr.Text), Convert.ToInt16(txtDONum.Text), true);
            byte[] rst = sendinfo(info);
            if (rst != null)
            {
                btnDO1.ImageIndex = 1;
                btnDO2.ImageIndex = 1;
                btnDO3.ImageIndex = 1;
                btnDO4.ImageIndex = 1;
                btnDO5.ImageIndex = 1;
                btnDO6.ImageIndex = 1;
                btnDO7.ImageIndex = 1;
                btnDO8.ImageIndex = 1;
            }
        }

        private void btnReadDO_Click(object sender, EventArgs e)
        {
            byte[] info = CModbusDll.ReadDO(Convert.ToInt16(txtaddr.Text), Convert.ToInt16(txtDONum.Text));
            byte[] rst = sendinfo(info);
            if (rst != null)
            {
                btnDO1.ImageIndex = ((rst[0] & 0x01) == 0x00) ? 0 : 1;
                btnDO2.ImageIndex = ((rst[0] & 0x02) == 0x00) ? 0 : 1;
                btnDO3.ImageIndex = ((rst[0] & 0x04) == 0x00) ? 0 : 1;
                btnDO4.ImageIndex = ((rst[0] & 0x08) == 0x00) ? 0 : 1;
                btnDO5.ImageIndex = ((rst[0] & 0x10) == 0x00) ? 0 : 1;
                btnDO6.ImageIndex = ((rst[0] & 0x20) == 0x00) ? 0 : 1;
                btnDO7.ImageIndex = ((rst[0] & 0x40) == 0x00) ? 0 : 1;
                btnDO8.ImageIndex = ((rst[0] & 0x80) == 0x00) ? 0 : 1;
            }
        }

        /*private void btnReadDI_Click(object sender, EventArgs e)
        {
            byte[] info = CModbusDll.ReadDI(Convert.ToInt16(txtaddr.Text), Convert.ToInt16(txtDINum.Text));
            byte[] rst = sendinfo(info);
            if (rst != null)
            {
                btnDI1.ImageIndex = ((rst[0] & 0x01) == 0x00) ? 0 : 1;
                btnDI2.ImageIndex = ((rst[0] & 0x02) == 0x00) ? 0 : 1;
                btnDI3.ImageIndex = ((rst[0] & 0x04) == 0x00) ? 0 : 1;
                btnDI4.ImageIndex = ((rst[0] & 0x08) == 0x00) ? 0 : 1;
                btnDI5.ImageIndex = ((rst[0] & 0x10) == 0x00) ? 0 : 1;
                btnDI6.ImageIndex = ((rst[0] & 0x20) == 0x00) ? 0 : 1;
                btnDI7.ImageIndex = ((rst[0] & 0x40) == 0x00) ? 0 : 1;
                btnDI8.ImageIndex = ((rst[0] & 0x80) == 0x00) ? 0 : 1;

                if(rst.Length>1)
                {
                    btnDI9.ImageIndex = ((rst[1] & 0x01) == 0x00) ? 0 : 1;
                    btnDI10.ImageIndex = ((rst[1] & 0x02) == 0x00) ? 0 : 1;
                    btnDI11.ImageIndex = ((rst[1] & 0x04) == 0x00) ? 0 : 1;
                    btnDI12.ImageIndex = ((rst[1] & 0x08) == 0x00) ? 0 : 1;
                    btnDI13.ImageIndex = ((rst[1] & 0x10) == 0x00) ? 0 : 1;
                    btnDI14.ImageIndex = ((rst[1] & 0x20) == 0x00) ? 0 : 1;
                    btnDI15.ImageIndex = ((rst[1] & 0x40) == 0x00) ? 0 : 1;
                    btnDI16.ImageIndex = ((rst[1] & 0x80) == 0x00) ? 0 : 1;
                }

                if (rst.Length > 2)
                {
                    btnDI17.ImageIndex = ((rst[2] & 0x01) == 0x00) ? 0 : 1;
                    btnDI18.ImageIndex = ((rst[2] & 0x02) == 0x00) ? 0 : 1;
                    btnDI19.ImageIndex = ((rst[2] & 0x04) == 0x00) ? 0 : 1;
                    btnDI20.ImageIndex = ((rst[2] & 0x08) == 0x00) ? 0 : 1;
                }                
            }
        }*/

        private byte[] analysisRcv(byte[] src, int len)
        {
            if (len < 6) return null;
            if (src[0] != Convert.ToInt16(txtaddr.Text)) return null;
            
            switch(src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src,6)==0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[3];
                        return dst;
                    }
                    break;
                case 0x02:
                    if (CMBRTU.CalculateCrc(src, src[2]+5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2];i++ )
                            dst[i] = src[3+i];
                        return dst;
                    }
                    break;
                case 0x05:
                    if (CMBRTU.CalculateCrc(src,8)==0x00)
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
        private byte[] sendinfo(byte[] info)
        {
            if (tclient.Connected == false) return null;
            tclient.SendTimeout = 1000;
            try
            {
                ns1.Write(info, 0, info.Length);//zxl 将数据写入本套接字的NetworkStream
                DebugInfo("发送",info);

                byte[] data = new byte[2048];
                ns2.ReadTimeout = 2000;
                int len = ns2.Read(data, 0, 2048);

                DebugInfo("接收", data, len);

                return analysisRcv(data,len);
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
                btnopen.Text = "打开";
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private void DebugInfo(string infotxt,byte[] info,int len=0)
        {
            string debuginfo;
            StringBuilder builder = new StringBuilder();
            if(info!=null)
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
        private void tmrrecv_Tick(object sender, EventArgs e)
        {
            if (tclient.Connected == false) return;
            if (tclient.Available <= 0) return;

            byte[] data = new byte[2048];
            int len = ns2.Read(data, 0, 2048);

        }

        /*private void btnDI19_Click(object sender, EventArgs e)
        {

        }*/

        private void RealPlayWnd_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        
        

       
        

        private void btnPreview_Click(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
        }

        private void ip1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
