using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AP_install_generate.Properties;

namespace AP_install_generate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            string path = "";
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            if (folderBrowser.SelectedPath != null &&
                folderBrowser.SelectedPath.Length > 0)
            {
                path = folderBrowser.SelectedPath;
                if (path[path.Length - 1] != 92)
                {
                    path += "\\";
                }
            }
            Data data = new Data(txb_IP.Text, txb_ssid.Text, txb_pw.Text);

            using (StreamWriter writer = new StreamWriter($"{path}install.sh"))
            {
                writer.Write($"{data.script_generate()}");
                writer.Close();
            }
            MessageBox.Show("complete");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
