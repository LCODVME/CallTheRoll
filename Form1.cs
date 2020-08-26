using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace CallTheRoll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int callState = 0;
        static int callNum = 0;
        string[] nameList = new string[4096];
        const string callDir = "D:\\CallTheRoll";
        string callAddr = callDir + "\\名单.txt";
        Random ra = new Random();
        System.Timers.Timer rollTimer = new System.Timers.Timer(20);

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            /* create heart beat timer */
            rollTimer.Elapsed += new System.Timers.ElapsedEventHandler(nameRollTimerCB);
            rollTimer.AutoReset = true;
            rollTimer.Enabled = true;
            rollTimer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(callState == 0)
            {
                int tempNun = 0;

                try
                {
                    foreach (string str in System.IO.File.ReadAllLines(callAddr, Encoding.Default))
                    {
                        nameList[tempNun++] = str;
                    }
                    callNum = tempNun;
                    if(callNum <= 0)
                    {
                        MessageBox.Show("名单内无学生，请点击下方修改名单，添加学生！");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("名单不存在，请点击下方修改名单，生成名单！");
                    return;
                }
                button1.Text = "停止";
                callState = 1;
                rollTimer.Start();
            }
            else
            {
                button1.Text = "开始点名";
                callState = 0;
                rollTimer.Stop();
            }
        }

        private void nameRollTimerCB(object source, System.Timers.ElapsedEventArgs e)
        {
            if(callState == 1)
            {
                label1.Text = nameList[ra.Next(0, callNum)];
            }
            else
            {
                rollTimer.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (callState == 1) return;
            if (!Directory.Exists(callDir))
            {
                Directory.CreateDirectory(callDir);
                File.SetAttributes(callDir, FileAttributes.Hidden); //隐藏
            }
            if (File.Exists(callAddr))
            {
                System.Diagnostics.Process.Start(callAddr);
            }
            else
            {
                System.IO.FileStream nameFile = new System.IO.FileStream(callAddr, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                nameFile.Close();
                System.Diagnostics.Process.Start(callAddr);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
