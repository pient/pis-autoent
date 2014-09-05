using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;

namespace PIS.AutoEnt.ConfigTool
{
    internal class AppStarter
    {
        /// <summary>
        /// 启动 GUI 界面 
        /// </summary>
        public static void StartGUI()
        {
            EncryptWindow encWin = new EncryptWindow();
            encWin.Show();
        }

        /// <summary>
        /// 启动命令行界面
        /// </summary>
        public static void StartConsole()
        {
            string lstr = "";
            string lfile = "license.config";
            string maccode = ConfigurationManager.AppSettings["MacCode"];

            if (String.IsNullOrEmpty(maccode))
            {
                maccode = SystemHelper.GetMACCode();
            }

            try
            {
                Console.WriteLine("Generating license file...");

                FileInfo file = new FileInfo("license.dec.config");

                if (file.Exists)
                {
                    StreamReader sr = file.OpenText();
                    lstr = sr.ReadToEnd();
                }

                FileInfo lfileinfo = new FileInfo(lfile);

                if (file.Exists)
                {
                    file.Delete();
                }

                Config config = new Config(lstr, maccode);

                using (StreamWriter writer = new StreamWriter(lfile, false, Encoding.UTF8))
                {
                    writer.Write(config.EncryptedContent);
                }

                Console.WriteLine("License has been generated. Please any key to close...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();

            App.Current.Shutdown();
        }
    }
}
