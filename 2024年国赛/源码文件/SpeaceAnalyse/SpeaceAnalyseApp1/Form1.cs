using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeaceAnalyseApp1.Lib;

namespace SpeaceAnalyseApp1
{
    public partial class Form1 : Form
    {
        DataCenter dataCenter = new DataCenter();
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
                fileHelp.readfile(dataCenter, dataGridView1, openFileDialog.FileName);
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
                fileHelp.savefile(richTextBox1, saveFileDialog.FileName);
            }
            tabControl1.SelectedTab = tabPage2;
        }

        private void 一键处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculate calculate = new Calculate(dataCenter);
            FileHelp fileHelp = new FileHelp();
            fileHelp.updaterich(richTextBox1, dataCenter);
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

        private void 问题解决ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "如果程序遇到问题请检查数据格式是否正确";
            string title = "问题解决";
            MessageBox.Show(text, title);
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
