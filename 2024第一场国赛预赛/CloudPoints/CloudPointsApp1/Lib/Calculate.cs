using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPointsApp1.Lib
{
    class Calculate
    {
        public void chushihuashange(ShanGe[,] shanges)//对所有栅格进行初始化
        {
            for(int i=0; i<10; i++)
            {
                for(int j=0;j<10;j++)
                {
                    shanges[i, j] = new ShanGe();
                }
            }
        }

        public void shangehua(List<DataPoint> dataPoints, ShanGe[,] shanges)//栅格分类
        {
            foreach(DataPoint dataPoint in dataPoints)
            {
                int i, j;//栅格索引
                i = ((int)(Math.Floor(dataPoint.X) / 10));
                j = ((int)(Math.Floor(dataPoint.Y) / 10));
                shanges[i, j].dataPoints.Add(dataPoint);
            }
        }

        public void calculateMH(ShanGe[,] shanges)//计算单个栅格平均高度
        {
            for(int i=0;i<10;i++)
            {
                for(int j=0;j<10;j++)
                {
                    List<DataPoint> dataPoints = shanges[i,j].dataPoints;//读取单个栅格的List
                    double H = 0;
                    foreach (DataPoint dataPoint in dataPoints)
                    {
                        H += dataPoint.Z;
                    }
                    double MH = H / dataPoints.Count();
                    shanges[i, j].meanH = MH;
                }
            }
        }

        public void maxmin(ShanGe[,] shanges)//计算单个栅格的最大最小高度及其差值
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    shanges[i, j].maxH = shanges[i, j].dataPoints.Max(p => p.Z);
                    shanges[i, j].minH = shanges[i, j].dataPoints.Min(p => p.Z);
                    shanges[i, j].maxmin = shanges[i, j].maxH - shanges[i, j].minH;
                }
            }
        }

        public double juli(DataPoint Pi, DataPoint Pj)//计算两点间距离
        {
            double ju = Math.Sqrt(Math.Pow(Pi.X - Pj.X, 2) + Math.Pow(Pi.Y - Pj.Y, 2) + Math.Pow(Pi.Z - Pj.Z, 2));
            return ju;
        }

        public PingMian nihe(DataPoint P1, DataPoint P2, DataPoint P3)//计算三点拟合的平面
        {
            PingMian pingMian = new PingMian();
            pingMian.P1 = P1;pingMian.P2 = P2;pingMian.P3 = P3;
            pingMian.a = juli(P1, P2); pingMian.b = juli(P2, P3); pingMian.c = juli(P3, P1);
            pingMian.p = (pingMian.a + pingMian.b + pingMian.c) / 2;
            pingMian.S = Math.Sqrt(pingMian.p *
                (pingMian.p - pingMian.a) *
                (pingMian.p - pingMian.b) *
                (pingMian.p - pingMian.c));

            pingMian.A = (P2.Y - P1.Y) * (P3.Z - P1.Z) - (P3.Y - P1.Y) * (P2.Z - P1.Z);
            pingMian.B = (P2.Z - P1.Z) * (P3.X - P1.X) - (P3.Z - P1.Z) * (P2.X - P1.X);
            pingMian.C = (P2.X - P1.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P1.Y);
            pingMian.D = -pingMian.A * P1.X - pingMian.B * P1.Y - pingMian.C * P1.Z;
            return pingMian;
        }

        public double mianju(PingMian pingMian, DataPoint dataPoint)//计算点到平面的距离
        {
            double d = 0;
            double need1 = Math.Abs(pingMian.A * dataPoint.X + pingMian.B * dataPoint.Y + pingMian.C * dataPoint.Z + pingMian.D);
            double need2 = Math.Sqrt(Math.Pow(pingMian.A, 2) + Math.Pow(pingMian.B, 2) + Math.Pow(pingMian.C, 2));
            d = need1 / need2;
            return d;
        }

        public void nihequanbu(List<PingMian> pingMians, List<DataPoint> dataPoints)//全部平面拟合
        {
            for(int i=0;i<=299;i++)//三个一组进行遍历
            {
                PingMian pingMian = new PingMian();//创建一个新平面
                DataPoint P1 = dataPoints[i]; DataPoint P2 = dataPoints[i+1]; DataPoint P3 = dataPoints[i+2];
                pingMian = nihe(P1, P2, P3);//计算平面的参数
                if(pingMian.S>=0.1)//如果平面达标
                {
                    List<DataPoint> templist = new List<DataPoint>();
                    foreach(DataPoint dataPoint in dataPoints)
                    {
                        templist.Add(dataPoint);
                    }
                    templist.RemoveAll(q => q.Name == P1.Name);//删除拟合的三个点
                    templist.RemoveAll(q => q.Name == P2.Name);
                    templist.RemoveAll(q => q.Name == P3.Name);
                    for(int index=0;index<994;index++)//遍历删除后的点集
                    {
                        double dm = mianju(pingMian, templist[index]);//计算点到面的距离
                        if(dm<=0.1)//如果距离达标
                        {
                            pingMian.neiPoints.Add(templist[index]);//则将点加入面的内部
                        }
                    }
                    pingMians.Add(pingMian);
                }
            }

        }

        public PingMian panduanJ1(List<PingMian> pingMians)//判断最优平面J1
        {
            PingMian J1 = new PingMian();
            double zuidanei = 0;
            zuidanei = pingMians.Max(p => p.neiPoints.Count());
            J1 = pingMians.Find(p => p.neiPoints.Count() == zuidanei);
            return J1;
        }

        public void neiwai(PingMian J1)//计算内外部点数量
        {
            J1.nei = J1.neiPoints.Count();
            J1.wai = 1000 - 3 - J1.nei;
        }

        public void yijiancalculate(List<DataPoint> dataPoints, ShanGe[,] shanges, List<PingMian> pingMians, PingMian J1, PingMian J2)//一键处理函数
        {
            chushihuashange(shanges);
            shangehua(dataPoints, shanges);
            calculateMH(shanges);
            maxmin(shanges);
            nihequanbu(pingMians,dataPoints);
            J1 = panduanJ1(pingMians);
            neiwai(J1);
            double minx = dataPoints.Min(p => p.X);
            double maxx = dataPoints.Max(p => p.X);
            double miny = dataPoints.Min(p => p.Y);
            double maxy = dataPoints.Max(p => p.Y);
            double minz = dataPoints.Min(p => p.Z);
            double maxz = dataPoints.Max(p => p.Z);
            ShanGe need = shanges[2, 3];
        }
    }
}
