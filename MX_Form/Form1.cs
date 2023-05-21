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
            //this.ControlBox = false;
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
        private int z1Pos, z2Pos, z3Pos,z4Pos, t1Pos, y1Pos,handStatus,bufferStatus;
        // 创建PLC对象
        ActUtlTypeLib.ActUtlType plc = new ActUtlTypeLib.ActUtlType();

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
            int returnCode =  plc.Open();
            if ( returnCode == 0 )
            {
                MessageBox.Show("Connection success");
            }
            else
            {
                MessageBox.Show("Connection fail");
                return;
            }
            // 现在，读取各个轴的位置
            plc.GetDevice("M240", out z1Pos);
            plc.GetDevice("M241", out z2Pos);
            plc.GetDevice("M242", out z3Pos);
            plc.GetDevice("M243", out z4Pos);
            plc.GetDevice("M244", out t1Pos);
            plc.GetDevice("M245", out y1Pos);
            plc.GetDevice("M246", out handStatus);
            plc.GetDevice("M247", out bufferStatus);
            // 判断各个轴是否在原点
            //label5.Text = z1Pos != 0 ? "NG" : "OK";
            //label6.Text = z2Pos != 0 ? "NG" : "OK";
            //label7.Text = y1Pos != 0 ? "NG" : "OK";
            //label8.Text = t1Pos != 0 ? "NG" : "OK";

            if (z1Pos == 0)
            {
                label5.Text = "NG";
                label5.BackColor = Color.Red;
            }
            else { label5.Text = "OK"; }

            if (z2Pos == 0)
            {
                label6.Text = "NG";
                label6.BackColor = Color.Red;
            }
            else { label6.Text = "OK"; }
            
            if (z3Pos == 0)
            {
                label7.Text = "NG";
                label7.BackColor = Color.Red;
            }
            else { label7.Text = "OK"; }

            if (z4Pos == 0)
            {
                label8.Text = "NG";
                label8.BackColor = Color.Red;
            }
            else { label8.Text = "OK"; }

            if (t1Pos == 0)
            {
                label12.Text = "NG";
                label12.BackColor = Color.Red;
            }
            else { label12.Text = "OK"; }

            if (y1Pos == 0)
            {
                label13.Text = "NG";
                label13.BackColor = Color.Red;
            }
            else { label13.Text = "OK"; }

            if (bufferStatus == 0)
            {
                label14.Text = "NG";
                label14.BackColor = Color.Red;
            }
            else { label14.Text = "OK"; }

            if (handStatus == 0)
            {
                label16.Text = "NG";
                label16.BackColor = Color.Red;
            }
            else { label16.Text = "OK"; }

        }

        private void button2_Click(object sender, EventArgs e) //  CheckIO_Reset
        {
            if (z1Pos == 1 && z2Pos == 1 && z3Pos == 1 && z4Pos == 1 && t1Pos == 1 && y1Pos == 1 && handStatus == 1 && bufferStatus == 1)
            {
                plc.SetDevice("M759", 1); // 清除报警
                Thread.Sleep(500);
                plc.SetDevice("M759", 0); // 
            }
            else
            {
                MessageBox.Show("Please check the equipment on site");
                plc.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e) // Auto 按键
        {
            if (z1Pos == 1 && z2Pos == 1 && z3Pos == 1 && z4Pos == 1 && t1Pos == 1 && y1Pos == 1 && handStatus == 1 && bufferStatus == 1)
            {
                plc.SetDevice("M754", 1); //initial

               // 全部选择
               plc.SetDevice("M750", 1);
               // 自动模式
               Thread.Sleep(500);
               plc.SetDevice("M755", 1);
               // Auto
               Thread.Sleep(500);
               plc.SetDevice("M757", 1);

                //plc.SetDevice()


                //MessageBox.Show("已经initial,请查看设备状态");
                // 关闭连接
                plc.Close();
            }
            else
            {
                MessageBox.Show("Please check the equipment on site");
                plc.Close();
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


