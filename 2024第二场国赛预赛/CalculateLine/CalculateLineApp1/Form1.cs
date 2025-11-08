using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculateLineApp1.Lib;

namespace CalculateLineApp1
{
    public partial class Form1 : Form
    {
        CalsulateLines calsulateLines = new CalsulateLines();
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
                FileHelp fileHelp = new FileHelp();
                fileHelp.readfile(openFileDialog.FileName, dataGridView1, calsulateLines);
            }
            tabControl1.SelectedTab = tabPage1;
        }

        private void 保存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 一键处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 查看数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 查看报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
