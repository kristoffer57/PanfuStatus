using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanfuStatus
{   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            updateAll();
            InitTimer();
        }

        private System.Windows.Forms.Timer timer1;
        public void InitTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 15000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateAll();
        }

        private void serverUpdate()
        {
            try
            {
                TcpClient client = new TcpClient();
                IAsyncResult result = client.BeginConnect("info.panfu.me", 5555, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success)
                    {
                        pictureBox2.Image = Properties.Resources.offline;
                    }
                else
                    {
                        pictureBox2.Image = Properties.Resources.online;
                    }
                client.EndConnect(result);
            }
            catch (Exception)
            {
                pictureBox2.Image = Properties.Resources.offline;
            }


            try
            {
                TcpClient client = new TcpClient();
                IAsyncResult result = client.BeginConnect("info.panfu.me", 5556, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success)
                {
                    pictureBox3.Image = Properties.Resources.offline;
                }
                else
                {
                    pictureBox3.Image = Properties.Resources.online;
                }
                client.EndConnect(result);
            }
            catch (Exception)
            {
                pictureBox3.Image = Properties.Resources.offline;
            }

            try
            {
                TcpClient client = new TcpClient();
                IAsyncResult result = client.BeginConnect("info.panfu.me", 5557, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success)
                {
                    pictureBox4.Image = Properties.Resources.offline;
                }
                else
                {
                    pictureBox4.Image = Properties.Resources.online;
                }
                client.EndConnect(result);
            }
            catch (Exception)
            {
                pictureBox4.Image = Properties.Resources.offline;
            }
        }

        private void websiteUpdate()
        {
            try
            {
                HttpWebRequest websiteRequest = (HttpWebRequest)WebRequest.Create("http://panfu.me");
                HttpWebResponse websiteResponse = (HttpWebResponse)websiteRequest.GetResponse();
                websiteRequest.Timeout = 5000;
                websiteRequest.Method = "GET";
                if (websiteResponse == null || websiteResponse.StatusCode != HttpStatusCode.OK)
                {
                    pictureBox1.Image = Properties.Resources.offline;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.online;
                    if (websiteResponse.ResponseUri == new Uri("panfu.me"))
                    {
                        pictureBox1.Image = Properties.Resources.online;
                    }
                    else
                    {
                        pictureBox1.Image = Properties.Resources.offline;
                    }
                }

                websiteResponse.Close();
            }
            catch (Exception)
            {
                pictureBox1.Image = Properties.Resources.offline;
            }
        }

        private void updateAll()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                websiteUpdate();
                serverUpdate();
            }).Start();
        }
    }
}
