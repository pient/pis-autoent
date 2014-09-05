using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace PIS.AutoEnt.ConfigTool
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class EncryptWindow : Window
    {
        public EncryptWindow()
        {
            InitializeComponent();

            btnEncrypt.IsEnabled = false;
            btnDecrypt.IsEnabled = false;
            btnSaveAs.IsEnabled = false;

            // 默认获取本机MAC地址
            // txtMAC.Text = PIC.SystemHelper.GetMACAddress();
            txtMAC.Text = SystemHelper.GetMACCode();
        }

        private void RealodConfig(bool? encrypted)
        {
            try
            {
                if (String.IsNullOrEmpty(txtMAC.Text))
                {
                    MessageBox.Show("请提供正确的机器码信息.", "消息");
                    return;
                }

                if (String.IsNullOrEmpty(txtConfig.Text))
                {
                    MessageBox.Show("请提供需要加密的内容.", "消息");
                    return;
                }

                Config config = new Config(txtConfig.Text, txtMAC.Text);
                bool txtCfgProtected = ((encrypted == null) ? config.IsProtected : encrypted.Value);

                if (encrypted != null)
                {
                    if (txtCfgProtected)
                    {
                        txtConfig.Text = config.EncryptedContent;
                    }
                    else
                    {
                        txtConfig.Text = config.Content;
                    }
                }

                btnEncrypt.IsEnabled = !txtCfgProtected;
                btnDecrypt.IsEnabled = txtCfgProtected;
                btnSaveAs.IsEnabled = (txtConfig.Text.Length > 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "消息");
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "配置文件(*.xml;*.config)|*.xml;*.config|所有文件 (*.*)|*.*";
            dlg.Multiselect = false;

            if (!String.IsNullOrEmpty(txtFile.Text))
            {
                dlg.FileName = txtFile.Text;
            }

            if ((bool)dlg.ShowDialog())
            {
                FileInfo file = new FileInfo(dlg.FileName);

                if (file.Exists)
                {
                    txtFile.Text = file.FullName;

                    StreamReader sr = file.OpenText();

                    txtConfig.Text = sr.ReadToEnd();

                    RealodConfig(null);
                }
            }
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            RealodConfig(true);
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            RealodConfig(false);
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.DefaultExt = ".config"; // Default file extension
            dlg.Filter = "配置文件(*.xml;*.config)|*.xml;*.config"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    writer.Write(txtConfig.Text);
                }

                MessageBox.Show("保存成功！", "消息");
            }
         }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
