using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ZHDuanMianApp1.Lib
{
    class FileHelper
    {
        public void readfile(ZHData zHData,DataGridView dataGridView,string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine();
            string[] lines;
            while(!sr.EndOfStream)
            {
                line = sr.ReadLine();
                lines = line.Split(',');
                ZHPoint zHPoint = new ZHPoint(lines[0], lines[1], lines[2], lines[3]);
                zHData.zHPoints.Add(zHPoint);
            }
            foreach(ZHPoint zHPoint in zHData.zHPoints)
            {
                int index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = zHPoint.Name;
                dataGridView.Rows[index].Cells[1].Value = zHPoint.X;
                dataGridView.Rows[index].Cells[2].Value = zHPoint.Y;
                dataGridView.Rows[index].Cells[3].Value = zHPoint.H;
            }
        }
    }
}
