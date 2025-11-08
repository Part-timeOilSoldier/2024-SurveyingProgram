using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 激光点云数据的平面分割
{
    public partial class Form1 : Form
    {
        DataCenter dataCenter;
        string report = "";
        public Form1()
        {
            InitializeComponent();
        }

        #region 写表格
        private void WriteDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("点名", typeof(string));
            dataTable.Columns.Add("X", typeof(string));
            dataTable.Columns.Add("Y", typeof(string));
            dataTable.Columns.Add("Z", typeof(string));

            DataRow dr;
            for (int i = 0; i < dataCenter.pointNum; i++)
            {
                var point = dataCenter.pointInfos[i];
                dr = dataTable.NewRow();
                dr[0] = point.Name;
                dr[1] = point.X.ToString();
                dr[2] = point.Y.ToString();
                dr[3] = point.Z.ToString();
                dataTable.Rows.Add(dr);
            }
            dataGridView1.DataSource = dataTable;
        }
        #endregion
        private void ToolOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog() { Filter = "txt文件|*.txt" };
                if (op.ShowDialog() == DialogResult.OK)
                {
                    dataCenter = FileHelper.ReadFile(op.FileName);
                    WriteDataTable();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("打开文件失败！");
            }
            TText.Text = "成功打开文件";
        }

        private void ToolCal_Click(object sender, EventArgs e)
        {
            try
            {
                Algorithm alo = new Algorithm(dataCenter);
                alo.Calij();
                var eva = alo.CalEvaZ();
                var cha = alo.CalDisH();
                dataCenter.Matrix[2, 3].Sort((x, y) => x.Z.CompareTo(y.Z));
                var sigma2 = alo.Calsigma2();
                RANSAC ran = new RANSAC(dataCenter);
                double S;
                bool a = ran.CalTRF(dataCenter.pointInfos[0], dataCenter.pointInfos[1], dataCenter.pointInfos[2], out S);
                double A, B, C, D;
                ran.CalABCD(dataCenter.pointInfos[0], dataCenter.pointInfos[1], dataCenter.pointInfos[2], out A, out B, out C, out D);
                var dis1000 = ran.CalDisP(dataCenter.pointInfos[999], A, B, C, D);
                var dis5 = ran.CalDisP(dataCenter.pointInfos[4], A, B, C, D);
                var inner = ran.CalInnerPoint(A, B, C, D, dataCenter.pointInfos[0], dataCenter.pointInfos[1], dataCenter.pointInfos[2]);
                double A1, B1, C1, D1;
                List<PointInfo> Minll;
                List<PointInfo> rees = ran.CalMaxInner(out A1, out B1, out C1, out D1, out Minll);
                double A2, B2, C2, D2;
                var ress2 = ran.CalJ2(Minll, out A2, out B2, out C2, out D2);
                double xi, yi, zi, x1, y1, z1;
                ran.CalShodow(dataCenter.pointInfos[4], A1, B1, C1, D1, out xi, out yi, out zi);
                ran.CalShodow(dataCenter.pointInfos[799], A2, B2, C2, D2, out x1, out y1, out z1);
                report = FileHelper.WriteReport(dataCenter);
                richTextBox1.Text = report;
            }
            catch (Exception)
            {

                MessageBox.Show("计算失败");
            }
            TText.Text = "计算成功！请查看报告";
        }
        private void ToolSaveReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sa = new SaveFileDialog() { Filter = "txt文件|*.txt" };
            if(sa.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sa.FileName);
                sw.Write(report);
                sw.Flush();
                sw.Close();
            }
        }

        private void ToolExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolViewData_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void ToolViewReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void ToolHelper_Click(object sender, EventArgs e)
        {
            MessageBox.Show("使用说明：\n(1)导入数据\n(2)计算数据\n(3)生成并查看报告\n(4)保存报告");
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            ToolOpenFile_Click(sender, e);
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            ToolSaveReport_Click(sender, e);
        }

        private void ViewData_Click(object sender, EventArgs e)
        {
            ToolViewData_Click(sender, e);
        }

        private void ViewReport_Click(object sender, EventArgs e)
        {
            ToolViewReport_Click(sender, e);
        }

        private void Helper_Click(object sender, EventArgs e)
        {
            ToolHelper_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("确定要退出吗？","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(res != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            ToolCal_Click(sender, e);
        }
    }
}
