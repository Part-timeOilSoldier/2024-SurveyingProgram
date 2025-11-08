using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace SpeaceAnalyseApp1.Lib
{
    class Calculate
    {
        double avg_x, avg_y;//存储x和y的平均中心
        double A, B, C;//辅助量BAC
        double[] a, b;double ai, bi, aibi;
        double xita;
        double[,] quan = new double[7, 7];

        public void getarea(DataCenter dataCenter)//区域分类
        {
            foreach(SAPoint sAPoint in dataCenter.sAPoints)
            {
                if (sAPoint.AreaCode == 1) dataCenter.areaSAs[0].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 2) dataCenter.areaSAs[1].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 3) dataCenter.areaSAs[2].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 4) dataCenter.areaSAs[3].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 5) dataCenter.areaSAs[4].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 6) dataCenter.areaSAs[5].sAPoints.Add(sAPoint);
                else if (sAPoint.AreaCode == 7) dataCenter.areaSAs[6].sAPoints.Add(sAPoint);
                else MessageBox.Show("不存在该区域");
            }
            foreach(AreaSA areaSA in dataCenter.areaSAs)
            {
                areaSA.number = areaSA.sAPoints.Count;
            }
        }

        public void caavgcenter(DataCenter dataCenter)//计算平均中心
        {
            dataCenter.avg_x = dataCenter.sAPoints.Average(p => p.X);
            dataCenter.avg_y = dataCenter.sAPoints.Average(p => p.Y);
            avg_x = dataCenter.avg_x;
            avg_y = dataCenter.avg_y;
        }

        public void cabiaozhuncha(DataCenter dataCenter)//计算所有事件点的平均中心
        {
            foreach (SAPoint sAPoint in dataCenter.sAPoints)
            {
                sAPoint.a = sAPoint.X - avg_x;
                sAPoint.b = sAPoint.Y - avg_y;
            }
        }

        public void cafuzhu(DataCenter dataCenter)//计算辅助量ABC,ai和bi
        {
            a = new double[dataCenter.sAPoints.Count];
            b = new double[dataCenter.sAPoints.Count];
            int i = 0;
            foreach (SAPoint sAPoint in dataCenter.sAPoints)
            {
                a[i] = sAPoint.a;
                b[i] = sAPoint.b;
                aibi += a[i] * b[i];
                i++;
            }
            ai = a.Sum();bi = b.Sum();

            //A
            A = ai - bi; dataCenter.ell.A = A;
            //B
            double need1, need2;
            need1 = Pow((ai - bi), 2);
            need2 = 4 * Pow(aibi, 2);
            B = Sqrt(need1 + need2);dataCenter.ell.B = B;
            //C
            C = 2 * aibi;dataCenter.ell.C = C;
        }

        public void cacanshu(DataCenter dataCenter)//计算椭圆参数
        {
            dataCenter.ell.xita = Atan((A + B) / C);
            xita = dataCenter.ell.xita;
            double up1 = 0;double up2 = 0;
            int i = 0;
            while(i<a.Count())
            {
                up1 += Pow((ai * Cos(xita) + bi * Sin(xita)), 2);
                up2 += Pow((ai * Sin(xita) - bi * Cos(xita)), 2);
                i++;
            }
            dataCenter.ell.SDEx = Sqrt(2) * Sqrt(up1 / a.Count());
            dataCenter.ell.SDEy = Sqrt(2) * Sqrt(up2 / a.Count());
        }

        public void caareacenter(DataCenter dataCenter)//计算各区域的平均中心
        {
            foreach(AreaSA areaSA in dataCenter.areaSAs)
            {
                double sumx = 0;double sumy = 0;
                foreach(SAPoint sAPoint in areaSA.sAPoints)
                {
                    sumx += sAPoint.X;
                    sumy += sAPoint.Y;
                }
                areaSA.avgax = sumx / areaSA.number;
                areaSA.avgay = sumy / areaSA.number;
            }
        }

        public double cajuli(AreaSA I,AreaSA J)//计算两区域之间的距离
        {
            double d = 0;
            double need1 = Pow((I.avgax - J.avgax), 2);
            double need2 = Pow((I.avgay - J.avgay), 2);
            d = Sqrt(need1 + need2);
            return d;
        }

        public void caquan(DataCenter dataCenter)//计算各个区域之间的权重矩阵
        {
            dataCenter.quan = new double[7, 7];
            for(int i=0; i<7; i++)
            {
                for(int j=0;j<7;j++)
                {
                    if (i == j) quan[i, j] = 0.0;
                    else quan[i, j] = 1000 / cajuli(dataCenter.areaSAs[i], dataCenter.areaSAs[j]);
                }
            }
            dataCenter.quan = quan;
        }

        public void zhengli(DataCenter dataCenter)//数据整理
        {
            double n = 0;
            foreach(AreaSA areaSA in dataCenter.areaSAs)
            {
                areaSA.xi = areaSA.sAPoints.Count;
                n += areaSA.xi;
            }
            dataCenter.X = n / 7;
        }

        public void caquanmo(DataCenter dataCenter)//计算全局莫兰指数
        {
            double N = 7;
            for(int i=0;i<7; i++)
            {
                for(int j=0;j<7;j++)
                {
                    dataCenter.S0 += quan[i, j];
                }
            }
            double need1 = 0;double need2 = 0;
            for(int i=0;i<7;i++)
            {
                need2 += Pow((dataCenter.areaSAs[i].xi - dataCenter.X),2);
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    need1 += (quan[i, j] * 
                        (dataCenter.areaSAs[i].xi - dataCenter.X) * 
                        (dataCenter.areaSAs[j].xi - dataCenter.X));
                }
            }
            dataCenter.I = (N / dataCenter.S0) * (need1 / need2);
        }

        public void cajubumo(DataCenter dataCenter)//计算局部莫兰指数
        {
            for (int i = 0; i < 7; i++)//7个区域
            {
                double sum1 = 0; double sum2 = 0;
                for (int j = 0; j < 7; j++)//求两种分子
                {
                    if(i!=j)
                    {
                        sum1 += quan[i, j] * (dataCenter.areaSAs[j].xi - dataCenter.X);
                        sum2 += Pow((dataCenter.areaSAs[j].xi - dataCenter.X),2);
                    }
                }
                double Si2 = sum2 / 6;
                dataCenter.areaSAs[i].Ii = (dataCenter.areaSAs[i].xi - dataCenter.X) / Si2 * sum1;
            }
        }

        public void caZ(DataCenter dataCenter)//计算莫兰指数的Z得分
        {
            double i = 0;
            foreach(AreaSA areaSA in dataCenter.areaSAs)
            {
                i += areaSA.Ii;
            }
            dataCenter.miu = i / 7;

            double need1 = 0;
            foreach (AreaSA areaSA in dataCenter.areaSAs)
            {
                need1 += Pow((areaSA.Ii-dataCenter.miu),2);
            }
            dataCenter.cta = Sqrt(need1 / 6);

            foreach (AreaSA areaSA in dataCenter.areaSAs)
            {
                areaSA.Zi = (areaSA.Ii - dataCenter.miu) / dataCenter.cta;
            }
        }

        public Calculate(DataCenter dataCenter)
        {
            getarea(dataCenter);
            caavgcenter(dataCenter);
            cabiaozhuncha(dataCenter);
            cafuzhu(dataCenter);
            cacanshu(dataCenter);
            caareacenter(dataCenter);
            caquan(dataCenter);
            zhengli(dataCenter);
            caquanmo(dataCenter);
            cajubumo(dataCenter);
            caZ(dataCenter);
        }
    }
}
