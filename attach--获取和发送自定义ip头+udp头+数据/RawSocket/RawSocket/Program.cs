using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RawSocket
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */


            /*
            RawSocket socket = new RawSocket();
            socket.Run();
            */

            //数据包没有发出去呀！！！！！！-----发出去了，现在问题在于byte数组和结构体的转换
            // IpHeader* ip_header = (IpHeader*)pbuffer; byte[]--->struct可以使用指针强转，，，but反一面好像不可以
            
            udpIpHeader udpIPH = new udpIpHeader();
            //udpIPH.send();
            udpIPH.send2();
            





        }
    }
}
