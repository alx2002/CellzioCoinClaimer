using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellzIoCoinClaimer
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            this.Shown += new System.EventHandler(this.Form1_Shown);
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.DeepPurple400, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory + "\\paste_token_here.txt";



            if (!File.Exists(path))
            {
                MessageBox.Show("paste_token_here.txt    does not exist, create it and paste your Token.");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = false;
            base.OnFormClosing(e);
        }
        

        string html;

        private void Form1_Shown(object sender, EventArgs e)
        {
            CreateRequests();

        }

        private void CreateRequests()
        {
            string TokenDir = File.ReadAllText(Environment.CurrentDirectory + "\\paste_token_here.txt");
            string url = $@"https://api.cellz.io/freeCoins?jwt={TokenDir}";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Thread.Sleep(1000);
                    html = CreateRequest(url);

                }
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Thread.Sleep(1000);
                    Invoke(new Action(() => { this.materialLabel1.Text = html; }));
                }
            }).Start();
        }

        private static string CreateRequest(string url)
        {
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
           
            return html;
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            string TokenDir = File.ReadAllText(Environment.CurrentDirectory + "\\paste_token_here.txt");
            CreateRequests();
            this.materialLabel3.ForeColor = Color.Red;
            this.materialLabel3.Text = "Paste your token in the txt and update for each act! You can do this for multiple accounts as a way to farm coins fast";
            this.materialLabel2.Text = "Claiming coins for: "+ TokenDir;

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(materialLabel2.Text);
            MessageBox.Show("Token Copied to Clipboard!");
        }

        private void materialLabel3_Click(object sender, EventArgs e)
        {

        }


        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/Qp6VyBd");
        }
    }
}
