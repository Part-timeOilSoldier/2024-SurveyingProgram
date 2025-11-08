using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SpeaceAnalyseApp1.Lib
{
    class FileHelp
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataCenter">数据集合</param>
        /// <param name="dataGridView">表格</param>
        /// <param name="filepath">数据路径</param>
        public void readfile(DataCenter dataCenter,DataGridView dataGridView,string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine();
            string[] lines;
            while (!sr.EndOfStream)//读取数据
            {
                line = sr.ReadLine();
                lines = line.Split(',');
                SAPoint sAPoint = new SAPoint(lines[0], lines[1], lines[2], lines[3]);
                dataCenter.sAPoints.Add(sAPoint);
            }
            dataGridView.Rows.Clear();//清空表格
            foreach (SAPoint sAPoint in dataCenter.sAPoints)//遍历数据添加进表格
            {
                int index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = sAPoint.ID;
                dataGridView.Rows[index].Cells[1].Value = sAPoint.X;
                dataGridView.Rows[index].Cells[2].Value = sAPoint.Y;
                dataGridView.Rows[index].Cells[3].Value = sAPoint.AreaCode;
            }
        }

        /// <summary>
        /// 保存报告
        /// </summary>
        /// <param name="richTextBox">计算报告</param>
        /// <param name="filepath">保存路径</param>
        public void savefile(RichTextBox richTextBox,string filepath)
        {
            richTextBox.SaveFile(filepath, RichTextBoxStreamType.PlainText);
        }

        /// <summary>
        /// 更新计算报告
        /// </summary>
        /// <param name="richTextBox">计算报告</param>
        /// <param name="dataCenter">数据集合</param>
        public void updaterich(RichTextBox richTextBox,DataCenter dataCenter)
        {
            string text = "序号，说明，计算结果\n";
            richTextBox.AppendText(text);

            text = string.Format("P6 的坐标 x：{0}\n", dataCenter.sAPoints[5].X);
            richTextBox.AppendText(text);
            text = string.Format("P6 的坐标 y：{0}\n", dataCenter.sAPoints[5].Y);
            richTextBox.AppendText(text);
            text = string.Format("P6 的区号：{0}\n", dataCenter.sAPoints[5].AreaCode);
            richTextBox.AppendText(text);

            text = string.Format("1 区（区号为 1）的事件数量 n1：{0}\n", dataCenter.areaSAs[0].number);
            richTextBox.AppendText(text);
            text = string.Format("4 区（区号为 4）的事件数量 n4：{0}\n", dataCenter.areaSAs[3].number);
            richTextBox.AppendText(text);
            text = string.Format("6 区（区号为 6）的事件数量 n6：{0}\n", dataCenter.areaSAs[5].number);
            richTextBox.AppendText(text);

            text = string.Format("事件总数 n：{0}\n", dataCenter.sAPoints.Count);
            richTextBox.AppendText(text);
            text = string.Format("坐标分量 x 的平均值：{0}\n", dataCenter.avg_x);
            richTextBox.AppendText(text);
            text = string.Format("坐标分量 y 的平均值：{0}\n", dataCenter.avg_y);
            richTextBox.AppendText(text);

            text = string.Format("P6 坐标分量与平均中心之间的偏移量 a6：{0}\n", dataCenter.sAPoints[5].a);
            richTextBox.AppendText(text);
            text = string.Format("P6 坐标分量与平均中心之间的偏移量 b6：{0}\n", dataCenter.sAPoints[5].b);
            richTextBox.AppendText(text);

            text = string.Format("辅助量 A：{0}\n", dataCenter.ell.A);
            richTextBox.AppendText(text);
            text = string.Format("辅助量 B：{0}\n", dataCenter.ell.B);
            richTextBox.AppendText(text);
            text = string.Format("辅助量 C：{0}\n", dataCenter.ell.C);
            richTextBox.AppendText(text);

            text = string.Format("标准差椭圆长轴与竖直方向的夹：{0}\n", dataCenter.ell.xita);
            richTextBox.AppendText(text);
            text = string.Format("标准差椭圆的长半轴SDEx：{0}\n", dataCenter.ell.SDEx);
            richTextBox.AppendText(text);
            text = string.Format("标准差椭圆的长半轴SDEy：{0}\n", dataCenter.ell.SDEy);
            richTextBox.AppendText(text);

            text = string.Format("1 区平均中心的坐标分量 X：{0}\n", dataCenter.areaSAs[0].avgax);
            richTextBox.AppendText(text);
            text = string.Format("1 区平均中心的坐标分量 Y：{0}\n", dataCenter.areaSAs[0].avgay);
            richTextBox.AppendText(text);
            text = string.Format("4 区平均中心的坐标分量 X：{0}\n", dataCenter.areaSAs[3].avgax);
            richTextBox.AppendText(text);
            text = string.Format("4 区平均中心的坐标分量 Y：{0}\n", dataCenter.areaSAs[3].avgay);
            richTextBox.AppendText(text);
            text = string.Format("1 区和 4 区的空间权重𝑤1,4：{0}\n", dataCenter.quan[0, 3]);
            richTextBox.AppendText(text);
            text = string.Format("6 区和 7 区的空间权重𝑤6,7：{0}\n", dataCenter.quan[5, 6]);
            richTextBox.AppendText(text);

            text = string.Format("研究区域犯罪事件的平均值：{0}\n", dataCenter.X);
            richTextBox.AppendText(text);
            text = string.Format("全局莫兰指数辅助量 S0：{0}\n", dataCenter.S0);
            richTextBox.AppendText(text);
            text = string.Format("全局莫兰指数 I：{0}\n", dataCenter.I);
            richTextBox.AppendText(text);
            text = string.Format("1 区的局部莫兰指数𝐼1：{0}\n", dataCenter.areaSAs[0].Ii);
            richTextBox.AppendText(text);
            text = string.Format("3 区的局部莫兰指数𝐼3：{0}\n", dataCenter.areaSAs[2].Ii);
            richTextBox.AppendText(text);
            text = string.Format("5 区的局部莫兰指数𝐼5：{0}\n", dataCenter.areaSAs[4].Ii);
            richTextBox.AppendText(text);
            text = string.Format("7 区的局部莫兰指数𝐼7：{0}\n", dataCenter.areaSAs[6].Ii);
            richTextBox.AppendText(text);

            text = string.Format("局部莫兰指数的平均数μ：{0}\n", dataCenter.miu);
            richTextBox.AppendText(text);
            text = string.Format("局部莫兰指数的标准差σ：{0}\n", dataCenter.cta);
            richTextBox.AppendText(text);

            text = string.Format("1 区局部莫兰指数的 Z 得分𝑍1：{0}\n", dataCenter.areaSAs[0].Zi);
            richTextBox.AppendText(text);
            text = string.Format("3 区局部莫兰指数的 Z 得分𝑍3：{0}\n", dataCenter.areaSAs[2].Zi);
            richTextBox.AppendText(text);
            text = string.Format("5 区局部莫兰指数的 Z 得分𝑍5：{0}\n", dataCenter.areaSAs[4].Zi);
            richTextBox.AppendText(text);
            text = string.Format("7 区局部莫兰指数的 Z 得分𝑍7：{0}\n", dataCenter.areaSAs[6].Zi);
            richTextBox.AppendText(text);
        }
    }
}
