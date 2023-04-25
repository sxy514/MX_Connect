using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
            label3.Text = "T-Axis";
            label4.Text = "Y-Axis";
            label5.Text = " ";
            label6.Text = " ";
            label7.Text = " ";
            label8.Text = " ";

        }

        private int logicalNumber;
        // 为各个轴创建变量
        private int z1Pos, z2Pos, t1Pos, y1Pos;
        // 创建PLC对象
        ActUtlTypeLib.ActUtlType plc = new ActUtlTypeLib.ActUtlType();

        private void button1_Click(object sender, EventArgs e) // 连接并判断各轴是否在原点
        {
            // 清除label 的背景颜色
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
         
            plc.ActLogicalStationNumber = logicalNumber;
            int returnCode =  plc.Open();
            if ( returnCode == 0 )
            {
                MessageBox.Show("连接成功");
            }
            else
            {
                MessageBox.Show("连接失败");
                return;
            }
            // 现在，读取各个轴的位置Z1: D4420 Z2:D4620 T1:D4820 Y1:D5020
            plc.GetDevice("D4420", out z1Pos);
            plc.GetDevice("D4620", out z2Pos);
            plc.GetDevice("D4820", out t1Pos);
            plc.GetDevice("D4820", out y1Pos);
            // 判断各个轴是否在原点
            //label5.Text = z1Pos != 0 ? "NG" : "OK";
            //label6.Text = z2Pos != 0 ? "NG" : "OK";
            //label7.Text = y1Pos != 0 ? "NG" : "OK";
            //label8.Text = t1Pos != 0 ? "NG" : "OK";

            if (z1Pos != 0)
            {
                label5.Text = "NG";
                label5.BackColor = Color.Red;
            }
            else { label5.Text = "OK"; }

            if (z2Pos != 0)
            {
                label6.Text = "NG";
                label6.BackColor = Color.Red;
            }
            else { label6.Text = "OK"; }
            
            if (y1Pos != 0)
            {
                label7.Text = "NG";
                label7.BackColor = Color.Red;
            }
            else { label7.Text = "OK"; }

            if (t1Pos != 0)
            {
                label8.Text = "NG";
                label8.BackColor = Color.Red;
            }
            else { label8.Text = "OK"; }
        }

        private void button2_Click(object sender, EventArgs e) //  Reset
        {
            if (z1Pos == 0 && z2Pos == 0 && y1Pos == 0 && t1Pos == 0)
            {
                plc.SetDevice("M37", 1); // 清除报警
                Thread.Sleep(500);
                plc.SetDevice("M37", 0); // M37复位
            }
            else
            {
                MessageBox.Show("轴不在原点,请到现场查看设备");
                plc.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e) // Auto 
        {
            if (z1Pos == 0 && z2Pos == 0 && y1Pos == 0 && t1Pos == 0)
            {
                plc.SetDevice("M7790", 1); //initial

                // 现在不知道恢复之后M7790的状态是否为1...
                Thread.Sleep(500);
                plc.SetDevice("M7790", 0); // initial复位

                // 完成初始化后需要切换自动并Auto start，查看Por-face找到指定的软元件
                // 

                //plc.SetDevice()


                //MessageBox.Show("已经initial,请查看设备状态");
                // 关闭连接
                plc.Close();
            }
            else
            {
                MessageBox.Show("轴不在原点,请到现场查看设备");
                plc.Close();
            }
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选中的项发生改变时，触发该事件
            // 读取选中的项
            logicalNumber = checkedListBox1.SelectedIndex + 1; // checklist 索引号从0开始
            //string a = Convert.ToString(logicalNumber);
           // MessageBox.Show(a);


        }
    }
}


