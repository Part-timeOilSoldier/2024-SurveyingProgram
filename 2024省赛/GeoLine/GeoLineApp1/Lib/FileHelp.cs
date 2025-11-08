using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GeoLineApp1.Lib
{
    class FileHelp
    {
        //读取数据函数
        public void readfile(DataGridView dataGridView,List<LineData> lineDatas,FoundData foundData, string filepath)
        {
            StreamReader sr = new StreamReader(filepath);

            //读取椭球参数
            string line;string[] lines;
            line = sr.ReadLine();
            lines = line.Split(',');
            foundData.a = double.Parse(lines[0]);
            foundData.f1 = double.Parse(lines[1]);
            sr.ReadLine();

            //读取数据
            while((line=sr.ReadLine())!=null)
            {
                lines = line.Split(',');
                LineData lineData = new LineData(lines[0], lines[1], lines[2], lines[3], lines[4], lines[5]);
                lineDatas.Add(lineData);
            }

            //更新视图
            foreach(LineData lineData in lineDatas)
            {
                int index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = lineData.Start;
                dataGridView.Rows[index].Cells[1].Value = lineData.B1;
                dataGridView.Rows[index].Cells[2].Value = lineData.L1;
                dataGridView.Rows[index].Cells[3].Value = lineData.End;
                dataGridView.Rows[index].Cells[4].Value = lineData.B2;
                dataGridView.Rows[index].Cells[5].Value = lineData.L2;
            }

        }

        //保存数据函数
        public void savefile(RichTextBox richTextBox,string filepath)
        {
            richTextBox.SaveFile(filepath, RichTextBoxStreamType.PlainText);
        }

        //更新报告
        public void uprich(List<LineData>  lineDatas,FoundData foundData,RichTextBox richTextBox)
        {
            richTextBox.Clear();
            richTextBox.AppendText("序号，说明，计算结果\n");
            richTextBox.AppendText(string.Format("1,椭球长半轴a，{0}\n", foundData.a));
            richTextBox.AppendText(string.Format("2,扁率倒数1/f，{0}\n", foundData.f1));
            richTextBox.AppendText(string.Format("3,扁率f，{0:F8}\n", foundData.f));
            richTextBox.AppendText(string.Format("4,椭球短半轴b，{0}\n", foundData.b));
            richTextBox.AppendText(string.Format("5,第一偏心率平方，{0:F8}\n", foundData.e2));
            richTextBox.AppendText(string.Format("6,第二偏心率平方，{0:F8}\n", foundData.e12));
            richTextBox.AppendText(string.Format("7,第1条大地线u1，{0}\n", lineDatas[0].fuzhuData.u1));
            richTextBox.AppendText(string.Format("8,第1条大地线u2，{0}\n", lineDatas[0].fuzhuData.u2));
            richTextBox.AppendText(string.Format("9,第1条大地线l(弧度)，{0:F8}\n", lineDatas[0].fuzhuData.l));
            richTextBox.AppendText(string.Format("10,第1条大地线a1，{0:F8}\n", lineDatas[0].fuzhuData.a1));
            richTextBox.AppendText(string.Format("11,第1条大地线a2，{0:F8}\n", lineDatas[0].fuzhuData.a2));
            richTextBox.AppendText(string.Format("12,第1条大地线b1，{0:F8}\n", lineDatas[0].fuzhuData.b1));
            richTextBox.AppendText(string.Format("13,第1条大地线b2，{0:F8}\n", lineDatas[0].fuzhuData.b2));
            richTextBox.AppendText(string.Format("14,第1条大地线afa，{0}\n", lineDatas[0].fuzhuData.afa));
            richTextBox.AppendText(string.Format("15,第1条大地线beta，{0}\n", lineDatas[0].fuzhuData.beta));
            richTextBox.AppendText(string.Format("16,第1条大地线gama，{0}\n", lineDatas[0].fuzhuData.gama));
            richTextBox.AppendText(string.Format("17,第1条大地线A1(弧度)，{0:F8}\n", lineDatas[0].fuzhuData.A1));
            richTextBox.AppendText(string.Format("18,第1条大地线nan，{0:F8}\n", lineDatas[0].fuzhuData.nan));
            richTextBox.AppendText(string.Format("19,第1条大地线xita，{0:F8}\n", lineDatas[0].fuzhuData.xita));
            richTextBox.AppendText(string.Format("20,第1条大地线sinA0，{0:F8}\n", lineDatas[0].fuzhuData.sinA0));
            richTextBox.AppendText(string.Format("21,第1条大地线A，{0}\n", lineDatas[0].fuzhuData.A));
            richTextBox.AppendText(string.Format("22,第1条大地线B，{0:F8}\n", lineDatas[0].fuzhuData.B));
            richTextBox.AppendText(string.Format("23,第1条大地线C，{0:F8}\n", lineDatas[0].fuzhuData.C));
            richTextBox.AppendText(string.Format("24,第1条大地线xita_1，{0:F8}\n", lineDatas[0].fuzhuData.xita_1));
            richTextBox.AppendText(string.Format("25,第1条大地线S1，{0:F3}\n", lineDatas[0].fuzhuData.C));
            richTextBox.AppendText(string.Format("26,第1条大地线S2，{0:F3}\n", lineDatas[1].fuzhuData.C));
            richTextBox.AppendText(string.Format("27,第1条大地线S3，{0:F3}\n", lineDatas[2].fuzhuData.C));
            richTextBox.AppendText(string.Format("28,第1条大地线S4，{0:F3}\n", lineDatas[3].fuzhuData.C));
            richTextBox.AppendText(string.Format("29,第1条大地线S5，{0:F3}\n", lineDatas[4].fuzhuData.C));
        }
    }
}
