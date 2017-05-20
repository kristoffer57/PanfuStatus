using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
