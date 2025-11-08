using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CalculateLineApp1.Lib
{
    class FileHelp
    {
        public void readfile(string filepath, CalculateGeoLine calculateGeoLine, DataGridView dataGridView)//读取数据
        {
            StreamReader sr = new StreamReader(filepath);

            string line = sr.ReadLine();
            string[] lines = line.Split(',');
            calculateGeoLine.a = double.Parse(lines[0]);
            calculateGeoLine.f1 = double.Parse(lines[1]);
            line = sr.ReadLine();

            while(!sr.EndOfStream)
            {
                line = sr.ReadLine();
                lines = line.Split(',');
                GeoLine geoline = new GeoLine(lines[0], lines[1], lines[2], lines[3], lines[4], lines[5]);
                calculateGeoLine.geoLines.Add(geoline);
            }

            int index = 0;//索引
            foreach(GeoLine geoline in calculateGeoLine.geoLines)
            {
                index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = geoline.Start.Name;
                dataGridView.Rows[index].Cells[1].Value = geoline.Start.B;
                dataGridView.Rows[index].Cells[2].Value = geoline.Start.L;
                dataGridView.Rows[index].Cells[3].Value = geoline.End.Name;
                dataGridView.Rows[index].Cells[4].Value = geoline.End.B;
                dataGridView.Rows[index].Cells[5].Value = geoline.End.L;
            }
        }

        public void savefile(string filepath,RichTextBox richTextBox)//保存报告
        {
            richTextBox.SaveFile(filepath, RichTextBoxStreamType.PlainText);
        }

        public void uprich(RichTextBox richTextBox,CalculateGeoLine calculateGeoLine)//更新报告
        {
            richTextBox.Clear();

        }
    }
}
