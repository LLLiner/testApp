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

            // 测试通过引用在Mysql官网下载的.dll连接数据库
            /*string constructorString = "server=localhost;User Id=root;password=1111;Database=delay";
            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("insert into time(priority,delay) values(5,567)", myConnect);
            Console.WriteLine(myCmd.CommandText);
            if (myCmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("数据插入成功！");
                Console.Read();//防止黑框闪退，可以看到结果。
            }
            myConnect.Close();//有close，数据才会被真正写入保存在数据库
            */






            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frmmain());


            
        }
        
    }
}
