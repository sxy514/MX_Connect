using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ActUtlTypeLib;

namespace MX_Form
{
    public partial class Form1 : Form
    {
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
            label1.Text = "Z1-Axis";
            label2.Text = "Z2-Axis";
            label3.Text = "Z3-Axis";
            label4.Text = "Z4-Axis";
            label5.Text = " ";
            label6.Text = " ";
            label7.Text = " ";
            label8.Text = " ";
            label9.Text = "T-Axis";
            label10.Text = "Y-Axis";
            label11.Text = "Buffer";
            label12.Text = " ";
            label13.Text = " ";
            label14.Text = " ";
            label15.Text = " MCT_Hand";
            label16.Text = " ";
        }

        private int logicalNumber;

        // 为各个轴创建变量
        private int _z1Pos, _z2Pos, _z3Pos, _z4Pos, _t1Pos, _y1Pos, _handStatus, _bufferStatus;

        // 创建PLC对象
        private readonly ActUtlType plc = new ActUtlType();

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


            plc.ActLogicalStationNumber = logicalNumber;
            var returnCode = plc.Open();
            if (returnCode == 0)
            {
                // 现在，读取各个轴的位置
                returnCode += plc.GetDevice("M240", out _z1Pos);
                returnCode += plc.GetDevice("M241", out _z2Pos);
                returnCode += plc.GetDevice("M242", out _z3Pos);
                returnCode += plc.GetDevice("M243", out _z4Pos);
                returnCode += plc.GetDevice("M244", out _t1Pos);
                returnCode += plc.GetDevice("M245", out _y1Pos);
                returnCode += plc.GetDevice("M246", out _handStatus);
                returnCode += plc.GetDevice("M247", out _bufferStatus);
                if (returnCode != 0)
                {
                    MessageBox.Show("PLC getDevice failed. Please contact the administrator.");
                    plc.Close(); // try Close
                }
            }
            else
            {
                MessageBox.Show("PLC connection failed. Please contact the administrator.");
                return;
            }

            // 判断各个轴是否在原点
            if (_z1Pos == 0)
            {
                label5.Text = "NG";
                label5.BackColor = Color.Red;
            }
            else
            {
                label5.Text = "OK";
            }

            if (_z2Pos == 0)
            {
                label6.Text = "NG";
                label6.BackColor = Color.Red;
            }
            else
            {
                label6.Text = "OK";
            }

            if (_z3Pos == 0)
            {
                label7.Text = "NG";
                label7.BackColor = Color.Red;
            }
            else
            {
                label7.Text = "OK";
            }

            if (_z4Pos == 0)
            {
                label8.Text = "NG";
                label8.BackColor = Color.Red;
            }
            else
            {
                label8.Text = "OK";
            }

            if (_t1Pos == 0)
            {
                label12.Text = "NG";
                label12.BackColor = Color.Red;
            }
            else
            {
                label12.Text = "OK";
            }

            if (_y1Pos == 0)
            {
                label13.Text = "NG";
                label13.BackColor = Color.Red;
            }
            else
            {
                label13.Text = "OK";
            }

            if (_bufferStatus == 0)
            {
                label14.Text = "NG";
                label14.BackColor = Color.Red;
            }
            else
            {
                label14.Text = "OK";
            }

            if (_handStatus == 0)
            {
                label16.Text = "NG";
                label16.BackColor = Color.Red;
            }
            else
            {
                label16.Text = "OK";
            }

            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e) //  CheckIO_Reset
        {
            if (_z1Pos == 1 && _z2Pos == 1 && _z3Pos == 1 && _z4Pos == 1 && _t1Pos == 1 && _y1Pos == 1 &&
                _handStatus == 1 &&
                _bufferStatus == 1)
            {
                plc.SetDevice("M759", 1); // 清除报警
                Thread.Sleep(500);
                plc.SetDevice("M759", 0); // 
                button2.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please check the equipment on site");
                plc.Close();
                button1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Auto 按键
        {
            var returnCode1 = 0;
            if (_z1Pos == 1 && _z2Pos == 1 && _z3Pos == 1 && _z4Pos == 1 && _t1Pos == 1 && _y1Pos == 1 &&
                _handStatus == 1 &&
                _bufferStatus == 1)
            {
                returnCode1 += plc.SetDevice("M754", 1); //initial ？
                Thread.Sleep(500);

                returnCode1 += plc.SetDevice("M750", 1); // All selection
                Thread.Sleep(500);

                returnCode1 += plc.SetDevice("M755", 1); // Auto Mode
                Thread.Sleep(500);

                returnCode1 += plc.SetDevice("M757", 1); // start

                returnCode1 += plc.Close();
                if (returnCode1 != 0) MessageBox.Show("PLC Close failed. Please contact the administrator.");
            }
            else
            {
                MessageBox.Show("Please check the equipment on site");
                plc.Close();
                button1.Enabled = true;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选中的项发生改变时，触发该事件
            // 读取选中的项
            logicalNumber = checkedListBox1.SelectedIndex + 1; // checklist 索引号从0开始的
            //string a = Convert.ToString(logicalNumber);
            // MessageBox.Show(a);
        }
    }
}
