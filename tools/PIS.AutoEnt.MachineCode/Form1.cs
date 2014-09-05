using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIS.AutoEnt.MachineCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMAC.Text = SystemHelper.GetMACCode();

            string macInfo = String.Format("CPU 号：{0}\r\n硬盘号：{1}\r\nMAC地址：{2}\r\n",
                SystemHelper.GetCPUID(), SystemHelper.GetHardDiskID(), SystemHelper.GetMACAddress());
            txtMacInfo.Text = macInfo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtMAC.Text);
        }
    }
}
