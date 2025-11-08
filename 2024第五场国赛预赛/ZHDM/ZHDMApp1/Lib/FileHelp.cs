using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ZHDMApp1.Lib
{
    class FileHelp
    {
        public void readfile(ZHData zHData,DataGridView dataGridView,string filepath)//数据读取并显示
        {
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine();
            line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine();
            string[] lines;
            while((line = sr.ReadLine()) != null)
            {
                lines = line.Split(',');
                ZHPoint zHPoint = new ZHPoint(lines[0], lines[1], lines[2], lines[3]);
                zHData.AllPoints.Add(zHPoint);
            }

            dataGridView.Rows.Clear();
            foreach(ZHPoint zHPoint in zHData.AllPoints)
            {
                int index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = zHPoint.Name;
                dataGridView.Rows[index].Cells[1].Value = zHPoint.X;
                dataGridView.Rows[index].Cells[2].Value = zHPoint.Y;
                dataGridView.Rows[index].Cells[3].Value = zHPoint.H;
            }
        }

        public void savefile(RichTextBox richTextBox,string filepath)//保存计算报告
        {
            richTextBox.SaveFile(filepath,RichTextBoxStreamType.PlainText);
        }

        public void updaterich(RichTextBox richTextBox,ZHData zHData)//计算后更新报告
        {
            richTextBox.Clear();
            richTextBox.AppendText("纵断面信息如下:\n");
            foreach (ZHPoint zHPoint in zHData.ZongneiPoints)
            {
                string need = string.Format("Name:{0}, X:{1:N3}, Y:{2:N3}, H:{3:N3}\n", zHPoint.Name, zHPoint.X, zHPoint.Y, zHPoint.H);
                richTextBox.AppendText(need);
            }
            richTextBox.AppendText("横断面信息如下:\n");
            foreach (ZHPoint zHPoint in zHData.HengneiPoints)
            {
                string need = string.Format("Name:{0}, X:{1:N3}, Y:{2:N3}, H:{3:N3}\n", zHPoint.Name, zHPoint.X, zHPoint.Y, zHPoint.H);
                richTextBox.AppendText(need);
            }
        }
    }
}
