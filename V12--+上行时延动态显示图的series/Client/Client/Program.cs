using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //打开一个线程进行udp时延数据的接收
            UDPSocket.startReceive();
            //Console.Read(); //等待键盘输入，退出程序。使调试时能看到输出结果。如果没有此句，命令窗口会一闪而过。

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindows());
            

        }
        
    }

    

}
