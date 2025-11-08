using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoLineApp1.Lib;

namespace GeoLineApp1
{
    public partial class Form1 : Form
    {
        List<LineData> lineDatas = new List<LineData>();
        FoundData foundData = new FoundData();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.txt";
            if(openFileDialog.ShowDialog()==DialogResult.OK)
            {
                FileHelp fileHelp = new FileHelp();
                fileHelp.readfile(dataGridView1, lineDatas, foundData, openFileDialog.FileName);
            }
            tabControl1.SelectedTab = tabPage1;
        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 一键计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculate calculate = new Calculate();
            calculate.allca(lineDatas, foundData);
            FileHelp fileHelp = new FileHelp();
            fileHelp.uprich(lineDatas, foundData, richTextBox1);
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
            string text = "如果有任何问题请联系作者";
            string title = "关于";
            MessageBox.Show(text, title);
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
