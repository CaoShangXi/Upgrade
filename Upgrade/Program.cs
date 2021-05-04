using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Upgrade
{
    class Program
    {
        public static string programName;
        static void Main(string[] args)
        {
            programName = Process.GetCurrentProcess().ProcessName;
            Process[] process = Process.GetProcessesByName("Master");
            foreach (Process p in process)
            {
                p.Kill();
            }
            string NowPath = System.IO.Directory.GetCurrentDirectory();
            string iniFilePath = NowPath + "\\" + "App.ini";
            // 抓INI路徑
            IniFile iniFile = new IniFile(iniFilePath);
            string Path = iniFile.ReadIni("upgrade", "path");
            bool result = CopyFile(Path, NowPath);
            if (result)
            {
                Console.WriteLine("程式更新成功！");
                Thread.Sleep(2000);
                Process.Start("Master.exe");//啓動主程式
                return;
            }
            Console.WriteLine("程式更新失敗！");
            Console.ReadKey();
        }

        /// <summary>
        /// copy 路徑上所有資料
        /// </summary>
        /// <param name="Path">源路徑</param>
        /// <param name="CreatePath">目標路徑</param>
        /// <returns></returns>
        public static bool CopyFile(string Path, string CreatePath)
        {
            string[] tempFile;
            FileInfo info;
            try
            {
                if (Directory.Exists(Path))
                {

                   Directory.CreateDirectory(CreatePath);
                    var FolderFile = Directory.GetDirectories(Path);

                    foreach (var item in FolderFile)
                    {

                        info = new FileInfo(item);
                        var CreateFolder = CreatePath + "\\" + info.Name;
                        Directory.CreateDirectory(CreateFolder);

                        var tempFile2 = Directory.GetFiles(item);
                        foreach (var item2 in tempFile2)
                        {
                            var info2 = new FileInfo(item2);
                            var CreateFilePath = CreatePath + "\\" + info.Name + "\\" + info2.Name;
                            // Copey資料過去
                            File.Copy(info2.FullName, CreateFilePath, true);
                        }
                    }


                    tempFile = Directory.GetFiles(Path);
                    foreach (string item in tempFile)
                    {
                        info = new FileInfo(item);
                        if (!info.Name.StartsWith(programName))
                        {
                            var CreateFilePath = CreatePath + "\\" + info.Name;
                            // Copey資料過去
                            File.Copy(info.FullName, CreateFilePath, true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
