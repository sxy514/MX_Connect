using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ActUtlTypeLib;
using MySql.Data.MySqlClient;

namespace MX_Form
{
    public partial class Form1 : Form
    {
        // 创建一个整型数组，用于存储8个轴变量的地址
        private readonly int[] _ioAdress = new int[8];

        // 创建PLC对象
        private readonly ActUtlType plc = new ActUtlType();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // 加载窗口
        {
            // 初始化checkListBox
            checkedListBox1.Items.Add("MCT112");
            checkedListBox1.Items.Add("MCT113");
            checkedListBox1.Items.Add("MCT114");
            checkedListBox1.Items.Add("MCT115");

            // 初始化label
            label5.Text = " ";
            label6.Text = " ";
            label7.Text = " ";
            label8.Text = " ";
            label12.Text = " ";
            label13.Text = " ";
            label14.Text = " ";
            label16.Text = " ";
        }

        private void button1_Click(object sender, EventArgs e) // 连接并判断各轴是否在原点
        {
            // 清除label 的背景颜色
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label12.BackColor = Color.Transparent;
            label13.BackColor = Color.Transparent;
            label14.BackColor = Color.Transparent;
            label16.BackColor = Color.Transparent;

            var returnCode = plc.Open();
            if (returnCode == 0)
            {
                // 现在，读取各个轴的位置
                returnCode += plc.GetDevice("M240", out _ioAdress[0]);
                returnCode += plc.GetDevice("M241", out _ioAdress[1]);
                returnCode += plc.GetDevice("M242", out _ioAdress[2]);
                returnCode += plc.GetDevice("M243", out _ioAdress[3]);
                returnCode += plc.GetDevice("M244", out _ioAdress[4]);
                returnCode += plc.GetDevice("M245", out _ioAdress[5]);
                returnCode += plc.GetDevice("M246", out _ioAdress[6]);
                returnCode += plc.GetDevice("M247", out _ioAdress[7]);
                if (returnCode != 0) MessageBox.Show("PLC getDevice failed. Please contact the administrator.");
            }
            else
            {
                MessageBox.Show("PLC connection failed. Please contact the administrator.");
                plc.Close();
                return;
            }

            // 判断各个轴是否在原点
            if (_ioAdress[0] == 0)
            {
                label5.Text = "NG";
                label5.BackColor = Color.Red;
            }
            else
            {
                label5.Text = "OK";
            }

            if (_ioAdress[1] == 0)
            {
                label6.Text = "NG";
                label6.BackColor = Color.Red;
            }
            else
            {
                label6.Text = "OK";
            }

            if (_ioAdress[2] == 0)
            {
                label7.Text = "NG";
                label7.BackColor = Color.Red;
            }
            else
            {
                label7.Text = "OK";
            }

            if (_ioAdress[3] == 0)
            {
                label8.Text = "NG";
                label8.BackColor = Color.Red;
            }
            else
            {
                label8.Text = "OK";
            }

            if (_ioAdress[4] == 0)
            {
                label12.Text = "NG";
                label12.BackColor = Color.Red;
            }
            else
            {
                label12.Text = "OK";
            }

            if (_ioAdress[5] == 0)
            {
                label13.Text = "NG";
                label13.BackColor = Color.Red;
            }
            else
            {
                label13.Text = "OK";
            }

            if (_ioAdress[6] == 0)
            {
                label14.Text = "NG";
                label14.BackColor = Color.Red;
            }
            else
            {
                label14.Text = "OK";
            }

            if (_ioAdress[7] == 0)
            {
                label16.Text = "NG";
                label16.BackColor = Color.Red;
            }
            else
            {
                label16.Text = "OK";
            }

            returnCode = plc.Close();
            if (returnCode != 0)
                MessageBox.Show("PLC Close failed. Please try Again!.");
            else
                button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e) //  CheckIO_Reset
        {
            if (button1.Enabled)
            {
                MessageBox.Show("Please select the equipment and connect it and try again!");
                return;
            }

            foreach (var checkStatus in _ioAdress)
                if (checkStatus == 0)
                {
                    MessageBox.Show("MCT cannot be remotely recovered .Please check the equipment on site.");
                    plc.Close();
                    button1.Enabled = true;
                    return;
                }


            var returnCode = 0;
            // 连接PLC
            returnCode += plc.Open();
            if (returnCode != 0)
            {
                MessageBox.Show("PLC connection failed. Please try again or contact the administrator.");
                plc.Close();
                return;
            }

            /*  从数据库判断单元状态 */
            if (IsUnitActive())
            {
                MessageBox.Show(
                    "One unit Of this MCT is Active,Please wait and Operation it until all of this MCT unit is Idle!");
                plc.Close();
                return;
            }

            returnCode += plc.SetDevice("M759", 1); // 清除报警
            Delay(100);
            returnCode += plc.SetDevice("M759", 0); // 复位

            returnCode += plc.SetDevice("M750", 1); // all select
            Delay(100);

            returnCode += plc.SetDevice("M755", 1); // manual mode
            Delay(100);

            returnCode += plc.SetDevice("M758", 1); // stop
            Delay(300);
            returnCode += plc.SetDevice("M758", 0); // stop复位

            if (returnCode != 0)
                MessageBox.Show("PLC SetDevice failed. Please try again or contact the administrator.");

            returnCode = plc.Close();
            if (returnCode != 0)
                MessageBox.Show("PLC Close failed. Please try Again!.");
            else
                button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e) // Auto 按键
        {
            if (button1.Enabled)
            {
                MessageBox.Show("Please select the equipment and connect it and try again!");
                return;
            }

            var returnCode = 0;
            // 连接PLC
            returnCode += plc.Open();
            if (returnCode != 0)
            {
                MessageBox.Show("PLC connection failed. Please try again or contact the administrator.");
                return;
            }

            returnCode += plc.SetDevice("M754", 1); //initial ？
            Delay(8000);

            returnCode += plc.SetDevice("M750", 1); // All selection
            Delay(100);

            returnCode += plc.SetDevice("M755", 1); // Auto Mode
            Delay(100);

            returnCode += plc.SetDevice("M757", 1); // start
            Delay(5000);
            returnCode += plc.SetDevice("M757", 0); // start复位

            if (returnCode != 0)
                MessageBox.Show("PLC SetDevice failed. Please try again or contact the administrator.");
            returnCode = plc.Close();
            if (returnCode != 0)
            {
                MessageBox.Show("PLC Close failed. Please try Again!.");
            }
            else
            {
                if (IsUnitRun())
                    MessageBox.Show("MCT state is running,Alarm reset success");
                else
                    MessageBox.Show(MessageFormat() + " , " + " Please go to the site and Confirm!");
                plc.Close();
            }

            button1.Enabled = true;
            button2.Enabled = true;


            //button3.Enabled = false; 这里需要查询设备状态并给出提示了。
        }


        // 创建一个Delay函数，在指定的时间内使用application的DoEvents函数
        private void Delay(int ms)
        {
            var stop = DateTime.Now.AddMilliseconds(ms);
            while (DateTime.Now < stop) Application.DoEvents();
        }

        // GetUnitState() 函数用于获取设备状态
        private Dictionary<string, string> GetUnitState()
        {
            var nUnitState = new Dictionary<string, string>();
            var connstr =
                "data source=localhost;database=tcsv2;user id=root;password=808999;pooling=false;charset=utf8";
            using (var conn = new MySqlConnection(connstr))
            {
                var Station = "MCT" + (111 + plc.ActLogicalStationNumber);
                var sql = string.Format("select ID,State from tcsunit where ID like '%{0}%'", Station);
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    nUnitState.Add(reader.GetString("ID"), reader.GetString("State"));
                conn.Close();
            }

            return nUnitState;
        }

        private Dictionary<string, string> GetSubUnitState()
        {
            var nSubUnitState = new Dictionary<string, string>();
            var connstr =
                "data source=localhost;database=tcsv2;user id=root;password=808999;pooling=false;charset=utf8";
            using (var conn = new MySqlConnection(connstr))
            {
                var Station = "MCT" + (111 + plc.ActLogicalStationNumber) + "_V";
                var sql = $"select ID,Substate from tcsunit where ID like '%{Station}%'";
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    nSubUnitState.Add(reader.GetString("ID"), reader.GetString("SubState"));
                conn.Close();
            }

            return nSubUnitState;
        }

        private bool IsUnitActive()
        {
            var SubunitState = new Dictionary<string, string>();
            var Result = false;
            SubunitState = GetSubUnitState();
            foreach (var i in SubunitState.Keys)
                if (SubunitState[i] != "NOTASSIGN")
                {
                    Result = true;
                    return Result;
                }

            return Result;
        }

        private bool IsUnitRun()
        {
            var unitState = new Dictionary<string, string>();
            var Result = true;
            unitState = GetUnitState();
            foreach (var i in unitState.Keys)
                if (!(unitState[i] == "RUN") || unitState[i] == "RESERVED") // 有一个不是RUN或RESERVED就返回false
                {
                    Result = false;
                    return Result;
                }

            return Result;
        }

        private string MessageFormat()
        {
            var unitState = new Dictionary<string, string>();
            var Message = string.Empty;
            unitState = GetUnitState();
            foreach (var key in unitState.Keys)
                if (!(unitState[key] == "RUN") || unitState[key] == "RESERVED")
                    Message += string.Format("The State Of {0} is {1}!", key, unitState[key]);
            return Message;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 读取选中的项
            plc.ActLogicalStationNumber = checkedListBox1.SelectedIndex + 1; // checklist 索引号从0开始的
        }
    }
}