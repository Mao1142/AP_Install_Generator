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
            using (var folderBrowser = new FolderBrowserDialog())
            {
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
            }

            Data data = new Data(txb_IP.Text, txb_ssid.Text, txb_pw.Text);
            if (System.IO.File.Exists($"{path}install.sh"))
            {
                if(MessageBox.Show("檔案已經存在於目標位置,是否要覆蓋?","覆蓋檔案",MessageBoxButtons.YesNo)
                    == DialogResult.No)
                {
                    MessageBox.Show("取消生成");
                    return;
                }
            }
            
            using (StreamWriter writer = new StreamWriter($"{path}install.sh"))
            {
                writer.Write($"{data.script_generate()}");
                writer.Close();
            }
            MessageBox.Show("complete");
        }
    }
}
