using System;
using System.Drawing;
using System.Windows.Forms;
using ActUtlTypeLib;

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

            button4.Enabled = false;
            button5.Enabled = false;
            textBox1.Enabled = false;
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
                if (returnCode != 0) MessageBox.Show(@"PLC getDevice failed. Please contact the administrator.");
            }
            else
            {
                MessageBox.Show(@"PLC connection failed. Please contact the administrator.");
                plc.Close();
                return;
            }

            #region 判断各个轴是否在原点

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

            #endregion

            returnCode = plc.Close();
            if (returnCode != 0)
                MessageBox.Show(@"PLC Close failed. Please try Again!.");
            else
                button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e) //  CheckIO_Reset
        {
            if (button1.Enabled)
            {
                MessageBox.Show(@"Please select the equipment and connect it and try again!");
                return;
            }

            foreach (var checkStatus in _ioAdress)
                if (checkStatus == 0)
                {
                    MessageBox.Show(@"MCT cannot be remotely recovered .Please check the equipment on site.");
                    plc.Close();
                    button1.Enabled = true;
                    return;
                }

            var returnCode = 0;
            // 连接PLC
            returnCode += plc.Open();
            if (returnCode != 0)
            {
                MessageBox.Show(@"PLC connection failed. Please try again or contact the administrator.");
                plc.Close();
                return;
            }

            returnCode += plc.SetDevice("M759", 1); // 清除报警
            Delay(100);
            returnCode += plc.SetDevice("M759", 0); // 复位

            /* returnCode += plc.SetDevice("M750", 1); // all select
             Delay(100);
 
             returnCode += plc.SetDevice("M756", 1); // manual mode
             Delay(100);
 
             returnCode += plc.SetDevice("M758", 1); // stop
             Delay(300);
             returnCode += plc.SetDevice("M758", 0); // stop复位
 
             Delay(100);
             returnCode += plc.SetDevice("M756", 0); // manual mode
            */
            if (returnCode != 0)
                MessageBox.Show(@"PLC SetDevice failed. Please try again or contact the administrator.");

            returnCode = plc.Close();
            if (returnCode != 0)
                MessageBox.Show(@"PLC Close failed. Please try Again!.");
            else
                button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e) // Auto 按键
        {
            if (button2.Enabled)
            {
                MessageBox.Show(@"Please click CheckIO_Reset and try again!");
                return;
            }

            button3.Enabled = false;
            var returnCode = 0;
            // 连接PLC
            returnCode += plc.Open();
            if (returnCode != 0)
            {
                MessageBox.Show(@"PLC connection failed. Please try again or contact the administrator.");
                return;
            }

            returnCode += plc.SetDevice("M755", 1); // Auto Mode        M755
            Delay(1000);
            returnCode += plc.SetDevice("M755", 0); // Auto Mode复位        M755

            returnCode += plc.SetDevice("M757", 1); // start         M757
            Delay(4000);
            returnCode += plc.SetDevice("M757", 0); // start复位

            if (returnCode != 0)
                MessageBox.Show(@"PLC SetDevice failed. Please try again or contact the administrator.");

            returnCode = plc.Close();
            if (returnCode != 0) MessageBox.Show(@"PLC Close failed. Please try Again!.");

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

            //button3.Enabled = false; 这里需要查询设备状态并给出提示了。可以使用Auto信号进行判定
        }


        // 创建一个Delay函数，在指定的时间内使用application的DoEvents函数
        private void Delay(int ms)
        {
            var stop = DateTime.Now.AddMilliseconds(ms);
            while (DateTime.Now < stop) Application.DoEvents();
        }


        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 读取选中的项
            plc.ActLogicalStationNumber = checkedListBox1.SelectedIndex + 1; // checklist 索引号从0开始的
        }

        private void button4_Click(object sender, EventArgs e) // SET 按钮
        {
            var returnCode = plc.Open();

            // 获取textBox1中的值
            var str = textBox1.Text;
            returnCode += plc.SetDevice(str, 1); // SET ON
            Delay(100);

            returnCode += plc.Close();
            if (returnCode != 0)
                MessageBox.Show(@"PLC SetDevice failed. Please try again or contact the administrator.");
            else
                button4.Enabled = false;
            textBox1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e) // RESET 按钮
        {
            var returnCode = plc.Open();
            // 获取textBox1中的值
            var str = textBox1.Text;
            returnCode += plc.SetDevice(str, 0); // RESET
            Delay(100);
            returnCode += plc.Close();
            if (returnCode != 0)
                MessageBox.Show(@"PLC ReSetDevice failed. Please try again or contact the administrator.");
            else
                button4.Enabled = true;
            textBox1.Enabled = true;
        }


        private void iOListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newForm = new Form();

            newForm.Text = "I/O List";

            // 创建并添加文本标签
            var label1 = new Label();
            label1.Text = @"清除报警: M759";
            label1.Location = new Point(50, 30);
            newForm.Controls.Add(label1);

            var label2 = new Label();
            label2.Text = @"全部选择: M750";
            label2.Location = new Point(50, 60); // 设置第二行文本标签的位置
            newForm.Controls.Add(label2); // 将第二行文本标签添加到新窗体中

            var label3 = new Label();
            label3.Text = @"自动模式: M755";
            label3.Location = new Point(50, 90);
            newForm.Controls.Add(label3);

            var label4 = new Label();
            label4.Text = @"手动模式: M756";
            label4.Location = new Point(50, 120);
            newForm.Controls.Add(label4);

            var label5 = new Label();
            label5.Text = @"停止: M758";
            label5.Location = new Point(50, 150);
            newForm.Controls.Add(label5);

            var label6 = new Label();
            label6.Text = @"启动: M757";
            label6.Location = new Point(50, 180);
            newForm.Controls.Add(label6);

            var label7 = new Label();
            label7.Text = @"初始化: M754";
            label7.Location = new Point(50, 210);
            newForm.Controls.Add(label7);

            // 设置新窗体的大小
            newForm.Size = new Size(240, 300);

            // 显示新窗体
            newForm.Show(this);
        }


        private TextBox textBox;
        private Form newWindow;

        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新的窗体对象
            var newWindow = new Form();

            newWindow.Text = @"请输入密码";

            // 创建文本框控件
            textBox = new TextBox();
            textBox.Location = new Point(30, 30); // 设置文本框的位置
            newWindow.Controls.Add(textBox); // 将文本框添加到新窗体中
            textBox.PasswordChar = '*'; // 将文本框输入内容替换为*

            // 创建登录按钮控件
            var loginButton = new Button();
            loginButton.Text = @"登录";
            loginButton.Location = new Point(150, 30); // 设置登录按钮的位置
            loginButton.Click += LoginButton_Click; // 为登录按钮添加点击事件处理程序
            newWindow.Controls.Add(loginButton); // 将登录按钮添加到新窗体中


            // 设置新窗体的大小
            newWindow.Size = new Size(270, 130);

            // 显示新窗体
            newWindow.ShowDialog();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // 在这里编写登录逻辑
            // 获取文本框中的输入，进行验证操作
            var strPwd = textBox.Text;
            // MessageBox.Show(strPwd);
            if (strPwd != "723181")
            {
                MessageBox.Show(@"密码错误，重新输入！");
            }
            else
            {
                button4.Enabled = true;
                button5.Enabled = true;
                textBox1.Enabled = true;
            }
        }
    }
}