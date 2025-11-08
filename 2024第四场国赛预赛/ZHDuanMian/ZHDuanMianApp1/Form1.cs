using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZHDuanMianApp1.Lib;

namespace ZHDuanMianApp1
{
    public partial class Form1 : Form
    {
        ZHData zHData = new ZHData();
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.txt";
            if(openFileDialog.ShowDialog()==DialogResult.OK)
            {
                FileHelper fileHelper = new FileHelper();
                fileHelper.readfile(zHData, dataGridView1, openFileDialog.FileName);
            }
            tabControl1.SelectedTab = tabPage1;
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void 一键处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculate calculate = new Calculate(zHData);
            tabControl1.SelectedTab = tabPage2;
        }

        private void 查看数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void 查看报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "报错请检查数据是否合规";
            string title = "关于";
            MessageBox.Show(text, title);
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
