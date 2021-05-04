using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProgramUpdates();
        }

        public void ProgramUpdates()
        {
            string NowPath = System.IO.Directory.GetCurrentDirectory();
            string iniFilePath = NowPath + "\\" + "App.ini";
            // 抓INI路徑
            IniFile iniFile = new IniFile(iniFilePath);
            string Path = iniFile.ReadIni("upgrade", "path");
            // 當前程式
            var Current = Process.GetCurrentProcess().MainModule.FileName;
            string OldPathVersion = FileVersionInfo.GetVersionInfo(Current).FileVersion.ToString();
            string NewPathVersion = FileVersionInfo.GetVersionInfo(Path + "\\" + "Master.exe").FileVersion;
            if (OldPathVersion != NewPathVersion)
            {
                DialogResult dialog = MessageBox.Show("发现新程式，是否更新程式？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (DialogResult.OK == dialog)
                    Process.Start(NowPath + "\\" + "Upgrade.exe");//啓動升級程式
            }
            else
            {
                Process[] process = Process.GetProcessesByName("Upgrade");
                foreach (Process p in process)
                {
                    p.Kill();
                }
            }
        }
    }
}
