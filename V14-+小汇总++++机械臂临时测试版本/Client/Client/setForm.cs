using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class setForm : Form
    {
        //用于主从时钟flag
        int master = 0;
        String masterIP;
        String slaveIP;

        public setForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        //通过unsafe将几个byte数组连接起来
        public unsafe static byte[] byUnsafe(byte[] gateBytes, byte slot, byte[] master)
        {
            MemoryStream ms = new MemoryStream();
            byte[] flag = {0xaf,0xaf};
            int i = 0;
            ms.WriteByte(flag[0]);
            ms.WriteByte(flag[1]);
            for (i = 0; i < gateBytes.Length; i++)
            {
                ms.WriteByte(gateBytes[i]);
            }
            ms.WriteByte(slot);
            for (i = 0; i < master.Length; i++)
            {
                ms.WriteByte(master[i]);
            }
            byte[] bytes = ms.ToArray();

            Console.WriteLine("flag+gate+slot+master转成byte数组：" + ByteArrayToHexString(bytes));

            return bytes;
        }


        private void setBtn_Click(object sender, EventArgs e)
        {
            //String gateTxt = gateTxtBox.Text;
            string[] hexValuesSplit = gateTxtBox.Text.Split(',');
            int i = 0;
            byte[] gateBytes = new byte[8];
            foreach (String hex in hexValuesSplit)
            {
                gateBytes[i] = Convert.ToByte(hex, 16);
                
                Console.WriteLine("hexadecimal value = {0}, byte value = {1}",hex, gateBytes[i]);
                i++;
            }

            byte slot = Convert.ToByte(slotTxtBox.Text);
            Console.WriteLine("Convert.ToByte(slotTxt):" + Convert.ToByte(slotTxtBox.Text));

            byte[] sendBytes = new byte[30]; //在创建时会自动初始化为0
            byte[] sendBytes2 = new byte[30];
            sendBytes[0] = 0xaf;
            sendBytes[1] = 0xaf;
            for(i = 0;i < gateBytes.Length;i++)
            {
                sendBytes[i + 2] = gateBytes[i];
            }
            sendBytes[i + 2] = slot;
            int tmp = i + 3;//tmp为下一个待赋值的数组下标
            byte[] masterByte = { 0x00,0x00,0x00,0x00};
            for (i = 0;i <masterByte.Length;i++)
            {
                sendBytes[tmp + i] = masterByte[i];
            }

            for(i = 0;i < sendBytes.Length;i++)
            {
                sendBytes2[i] = sendBytes[i];
            }
            sendBytes2[tmp + 3] = 0x06;

            //发送设置指令
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("192.168.1.200"), 10000); // 本机IP，指定的端口号
            UdpClient udpcSend = new UdpClient(localIpep);
            /*
            byte[] sendbytes = { 0xaf,0xaf, //16位标志位
                                 0xfc,0xfa,0xf9,0xf8,0xfc,0xfa,0xf9,0xf8,//64位门控
                                 0x07,     //8位时间槽
                                 0x00,0x00,0x00,0x00, //32位主从，0为主，6为从
                                 0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11,0x11
                                };
                                */
            IPEndPoint masterIpep = new IPEndPoint(IPAddress.Parse(masterIP), 10000); // 发送到的IP地址和端口号
            
            IPEndPoint slaveIpep = new IPEndPoint(IPAddress.Parse(slaveIP), 10000);

            try
            {
                udpcSend.Send(sendBytes, sendBytes.Length, masterIpep);
                udpcSend.Send(sendBytes2, sendBytes.Length, slaveIpep);

                Console.WriteLine("--->" + masterIpep.ToString());
                Console.WriteLine("发送的数据1为：" + BitConverter.ToString(sendBytes));
                Console.WriteLine("--->" + slaveIpep.ToString());
                Console.WriteLine("发送的数据2为：" + BitConverter.ToString(sendBytes2));
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送时产生异常：" + ex.Message);
                throw;
            }
            udpcSend.Close();
            MessageBox.Show("已成功设置！");
        }

        private void select_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);

            int choose = Convert.ToInt32(btn.Tag);//获取是谁被选中当主时钟
            if (choose != master)
            {
                master = choose;
                if (master == 1)
                {
                    //修改界面显示状态
                    cardB1.ImageIndex = 1;
                    cardB2.ImageIndex = 0;
                    masterIP = "192.168.1.232";
                    slaveIP = "192.168.1.233";
                }
                if (master == 2)
                {
                    cardB1.ImageIndex = 0;
                    cardB2.ImageIndex = 1;
                    masterIP = "192.168.1.233";
                    slaveIP = "192.168.1.232";
                }
            }
        }
    }
}
