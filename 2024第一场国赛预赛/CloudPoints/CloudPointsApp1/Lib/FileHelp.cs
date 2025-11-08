using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CloudPointsApp1.Lib
{
    class FileHelp
    {
        //打开数据文件夹
        public void readfile(List<DataPoint> dataPoints,DataGridView dataGridView,string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine();
            string[] lines;

            while((line=sr.ReadLine())!=null)
            {
                lines = line.Split(',');
                DataPoint dataPoint = new DataPoint(lines[0], lines[1], lines[2], lines[3]);
                dataPoints.Add(dataPoint);
            }

            int index = 0;
            foreach(DataPoint dataPoint in dataPoints)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = dataPoint.Name;
                dataGridView.Rows[index].Cells[1].Value = dataPoint.X;
                dataGridView.Rows[index].Cells[2].Value = dataPoint.Y;
                dataGridView.Rows[index].Cells[3].Value = dataPoint.Z;
                index++;
            }
        }

        //保存数据文件
        public void savefile(RichTextBox richTextBox, string filepath)
        {
            richTextBox.SaveFile(filepath, RichTextBoxStreamType.PlainText);
        }

        //更新报告
        public void uprich()
        {

        }
    }
}
