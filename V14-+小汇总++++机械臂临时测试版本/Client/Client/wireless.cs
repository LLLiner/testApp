using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Client
{
    public partial class wireless : Form
    {
        public wireless()
        {
            InitializeComponent();
        }

        int inter = 0;

        //用于数据库中最大最小时延值
        public string constructorString = "server=localhost;User Id=root;password=1111;Database=delay";
        public double maxDelay = 2.5;
        public double minDelay = 0;

        //根据显示时延数据
        private DataTable getDBDataByFlag(int flag,int interp)
        {
            int interP1 = interp;
            int interP2 = interp + 10;

            MySqlConnection myConnect = new MySqlConnection(constructorString);
            myConnect.Open();
            MySqlCommand myCmd = new MySqlCommand("SELECT * from wireless WHERE upFlag=?flag and id > ?interP1 AND id < ?interP2 ;", myConnect);
            myCmd.Parameters.AddWithValue("@Flag", flag);
            myCmd.Parameters.AddWithValue("@interP1", interP1);
            myCmd.Parameters.AddWithValue("@interP2", interP2);

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

        private void chartTimer_Tick(object sender, EventArgs e)
        {
            inter = inter + 10; //这是id间隔
            
            //重新获取下行时延数据，绑定数据
            DataTable dtUp = getDBDataByFlag(0,inter);
            DataTable dtDown = getDBDataByFlag(1,inter);
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();

            chart1.Series[0].Points.DataBind(dtUp.AsEnumerable(), "time", "delay", "");
            chart1.Series[1].Points.DataBind(dtDown.AsEnumerable(), "time", "delay", "");

            //----------------实时改变曲线图的图标
            /*
            chart1.Series[0].Name = "p"+lightP.ToString()+ " 灯";
            chart1.Series[1].Name = "p" + motorP.ToString()+ " 马达";
            chart1.Series[2].Name = "p" + armP.ToString()+ " 机械臂";
            */
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void wireless_Load(object sender, EventArgs e)
        {

            //offset图展示
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 1;
            this.chart1.ChartAreas[0].AxisY.Interval = 0.1;

            chart1.Series[0].MarkerBorderColor = Color.MediumVioletRed;
            chart1.Series[0].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[0].MarkerColor = Color.White;//AxisColor
            chart1.Series[0].MarkerSize = 5;
            chart1.Series[0].MarkerStyle = MarkerStyle.Circle;

            chart1.Series[1].MarkerBorderColor = Color.Black;
            chart1.Series[1].MarkerBorderWidth = 2; //chart1.;// Xaxis 
            chart1.Series[1].MarkerColor = Color.White;//AxisColor
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;

            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisY.Title = "时延（ms）";



            //重新获取下行时延数据，绑定数据
            DataTable dtUp = getDBDataByFlag(0, inter);
            DataTable dtDown = getDBDataByFlag(1, inter);
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();

            chart1.Series[0].Points.DataBind(dtUp.AsEnumerable(), "time", "delay", "");
            chart1.Series[1].Points.DataBind(dtDown.AsEnumerable(), "time", "delay", "");

            //----------------实时改变曲线图的图标
            /*
            chart1.Series[0].Name = "p"+lightP.ToString()+ " 灯";
            chart1.Series[1].Name = "p" + motorP.ToString()+ " 马达";
            chart1.Series[2].Name = "p" + armP.ToString()+ " 机械臂";
            */

        }
    }
}
