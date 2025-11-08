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
        CalculateGeoLine calculateGeoLine=new CalculateGeoLine();
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
                fileHelp.readfile(openFileDialog.FileName, calculateGeoLine, dataGridView1);
            }
            tabControl1.SelectedTab = tabPage1;
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文件|*.txt";
            if(saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                FileHelp fileHelp = new FileHelp();
                fileHelp.savefile(saveFileDialog.FileName, richTextBox1);
            }
            tabControl1.SelectedTab = tabPage2;
        }

        private void 一键处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculate calculate = new Calculate(calculateGeoLine);

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
            string text = "遇到问题请检查数据是否正确";
            string title = "关于";
            MessageBox.Show(text, title);
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
