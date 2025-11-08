using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDuanMianApp1.Lib
{
    class Calculate
    {
        double D;//纵断面总长度
        double afa01, afa12;//两个方位角
        double juK1K2, juK0K1;//关键点之间的距离
        double S, S1, S2;//纵断面梯形的面积
        double afam1,afam2;//横断面两个方位角
        double H1, H2, H;

        public void ceshidian(ZHData zHData)//测试点
        {
            ZHPoint A = new ZHPoint(); ZHPoint B = new ZHPoint();//初始化测试点
            A.X = 129.676;A.Y = 538.599;
            B.X = 124.471;B.Y = 526.233;
            double abfang = cafangwei(A, B);
            caneigao(zHData, A);//计算AB点的内插点高程
            caneigao(zHData, B);
            double abmian = catimian(A, B);
        }

        public double cafangwei(ZHPoint A,ZHPoint B)//计算两点之间的方位角并返回方位角的值
        {
            double fangweijiao;
            double x, y;
            x = B.X - A.X;
            y = B.Y - A.Y;
            fangweijiao = Math.Atan(y / x);
            fangweijiao = HzhuanJ(fangweijiao);//转成角度进行判断
            double endneed = 0;
            if(y>0)
            {
                if (x > 0) endneed = fangweijiao;
                else if (x < 0) endneed = 180 + fangweijiao;
                else if (x == 0) endneed = 90;
            }
            else if(y<0)
            {
                if (x < 0) endneed = 180 + fangweijiao;
                else if (x > 0) endneed = 360 + fangweijiao;
                else if (x == 0) endneed = 270;
            }
            endneed = JzhuanH(endneed);
            return endneed;
        }

        public double HzhuanJ(double hudu)//弧度转角度函数
        {
            return hudu / Math.PI * 180;
        }

        public double JzhuanH(double jiao)//角度转弧度函数
        {
            return jiao / 180 * Math.PI;
        }

        public void FindPoint(ZHData zHData)//找出关键点和普通点
        {
            foreach(ZHPoint zHPoint in zHData.zHPoints)
            {
                if(zHPoint.Name.Contains("K"))
                {
                    zHData.keyPoints.Add(zHPoint);
                }
                else
                {
                    zHData.ptPoints.Add(zHPoint);
                }
            }
        }

        public void caneigao(ZHData zHData,ZHPoint zHPoint)//计算内插点高程
        {
            List<ZHPoint> Q = new List<ZHPoint>();//初始化
            Q = zHData.zHPoints.OrderBy(p => cajuli(zHPoint, p)).Take(5).ToList();//计算距离最近的五个点
            double zong1=0, zong2=0;
            foreach(ZHPoint I in Q)
            {
                double d = cajuli(zHPoint, I);//计算每个点到点的距离
                zong1 += (I.H / d);zong2 += (1 / d);
            }
            zHPoint.H = zong1 / zong2;
        }

        public double cajuli(ZHPoint zHPoint1,ZHPoint zHPoint2)//计算两点间的距离
        {
            double d = 0;
            double x = zHPoint1.X - zHPoint2.X;
            double y = zHPoint1.Y - zHPoint2.Y;
            d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return d;
        }

        public double catimian(ZHPoint A,ZHPoint B)//计算AB两点间的梯形面积
        {
            double L = cajuli(A, B);
            double S = (A.H + B.H - 2 * 100) / 2 * L;
            return S;
        }

        public void zong(ZHData zHData)//道路纵断面计算
        {
            cazongju(zHData);//计算纵断面的平面距离
            findnei(zHData);//计算方位角，找到内插点
            cazongS(zHData);//计算所有纵断面面积
        }

        public void cazongju(ZHData zHData)//计算纵断面的平面距离
        {
            juK0K1 = cajuli(zHData.keyPoints[0], zHData.keyPoints[1]);
            juK1K2 = cajuli(zHData.keyPoints[1], zHData.keyPoints[2]);
            D = juK0K1 + juK1K2;
        }

        public void findnei(ZHData zHData)//计算方位角，找到纵断面的内插点
        {
            //先算方位角
            afa01 = cafangwei(zHData.keyPoints[0], zHData.keyPoints[1]);
            afa12 = cafangwei(zHData.keyPoints[1], zHData.keyPoints[2]);
            double deta = 10;
            int i = 1;//索引
            while(deta <= juK0K1)//内插点在K0K1上
            {
                ZHPoint zHPoint = new ZHPoint();
                double X0 = zHData.keyPoints[0].X;
                double Y0 = zHData.keyPoints[0].Y;
                double L = deta;
                zHPoint.X = X0 + L * afa01;
                zHPoint.Y = Y0 + L * afa01;
                string name = "Z" + i.ToString();
                zHPoint.Name = name;
                caneigao(zHData, zHPoint);
                zHData.neiPoints.Add(zHPoint);
                deta += 10;i++;
            }i = 1;//重置索引
            while(deta > juK0K1 && deta <= D)//内插点在K1K2上
            {
                ZHPoint zHPoint = new ZHPoint();
                double X0 = zHData.keyPoints[0].X;
                double Y0 = zHData.keyPoints[0].Y;
                double L = deta;
                zHPoint.X = X0 + (L - juK0K1) * afa12;
                zHPoint.Y = Y0 + (L - juK0K1) * afa12;
                string name = "Y" + i.ToString();
                zHPoint.Name = name;
                caneigao(zHData, zHPoint);
                zHData.neiPoints.Add(zHPoint);
                deta += 10;i++;
            }
        }

        public void cazongS(ZHData zHData)//计算所有纵断面面积
        {
            List<ZHPoint> list1 = new List<ZHPoint>();
            list1.Add(zHData.keyPoints[0]);
            foreach(ZHPoint zHPoint in zHData.neiPoints)
            {
                if(zHPoint.Name.Contains("Z"))
                {
                    list1.Add(zHPoint);
                }
            }
            list1.Add(zHData.keyPoints[1]);

            List<ZHPoint> list2 = new List<ZHPoint>();
            list2.Add(zHData.keyPoints[1]);
            foreach (ZHPoint zHPoint in zHData.neiPoints)
            {
                if (zHPoint.Name.Contains("Y"))
                {
                    list2.Add(zHPoint);
                }
            }
            list2.Add(zHData.keyPoints[2]);

            int i = 0;
            while(i<list1.Count()-1)
            {
                S1 += catimian(list1[i], list1[i + 1]);
                i++;
            }
            i = 0;
            while (i < list2.Count() - 1)
            {
                S2 += catimian(list2[i], list2[i + 1]);
                i++;
            }
            S = S1 + S2;
        }

        public void heng(ZHData zHData)//道路横断面计算
        {
            findxin(zHData);//找到中心点
            cahengfang(zHData);//计算横断面方位角
            findhengnei(zHData);//找到内插点
            cahengmian(zHData);
        }

        public void findxin(ZHData zHData)//找到中心点
        {
            ZHPoint zHPoint = new ZHPoint();
            string name = "M1";
            double x = (zHData.keyPoints[0].X + zHData.keyPoints[1].X) / 2;
            double y = (zHData.keyPoints[0].Y + zHData.keyPoints[1].Y) / 2;
            ZHPoint xin = new ZHPoint();
            xin.Name = name;
            xin.X = x; xin.Y = y;
            caneigao(zHData, xin);
            zHData.xinPoints.Add(xin);

            name = "M2";
            x = (zHData.keyPoints[1].X + zHData.keyPoints[2].X) / 2;
            y = (zHData.keyPoints[1].Y + zHData.keyPoints[2].Y) / 2;
            ZHPoint xin2 = new ZHPoint();
            xin2.Name = name;
            xin2.X = x; xin2.Y = y;
            caneigao(zHData, xin2);
            zHData.xinPoints.Add(xin2);
            
        }

        public void cahengfang(ZHData zHData)//计算横断面方位角
        {
            afa01 = HzhuanJ(afa01);afa12 = HzhuanJ(afa12);
            afam1 = afa01 + 90;afam1 = JzhuanH(afam1);
            afam2 = afa12 + 90;afam2 = JzhuanH(afam2);
        }

        public void findhengnei(ZHData zHData)//找到横断面的内插点
        {
            double[] need = new double[10] { -5, -4, -3, -2, -1, 1, 2, 3, 4, 5 };

            int i = 1;//名字
            foreach (double j in need)
            {
                ZHPoint zH = new ZHPoint();
                double x = zHData.xinPoints[0].X + j * 5 * Math.Cos(afam1);
                double y = zHData.xinPoints[0].Y + j * 5 * Math.Sin(afam1);
                string name = "Q" + i.ToString();
                zH.Name = name;zH.X = x;zH.Y = y;
                caneigao(zHData, zH);
                zHData.hengneiPoints.Add(zH);
                i++;
            }

            i = 1;
            foreach (double j in need)
            {
                ZHPoint zH = new ZHPoint();
                double x = zHData.xinPoints[1].X + j * 5 * Math.Cos(afam1);
                double y = zHData.xinPoints[1].Y + j * 5 * Math.Sin(afam1);
                string name = "W" + i.ToString();
                zH.Name = name; zH.X = x; zH.Y = y;
                caneigao(zHData, zH);
                zHData.hengneiPoints.Add(zH);
                i++;
            }

        }

        public void cahengmian(ZHData zHData)//计算横断面面积
        {
            List<ZHPoint> list1 = new List<ZHPoint>();
            foreach (ZHPoint zHPoint in zHData.hengneiPoints)
            {
                if (zHPoint.Name.Contains("Q"))
                {
                    if (zHPoint.Name.Contains("Q6")) list1.Add(zHData.xinPoints[0]);
                    list1.Add(zHPoint);
                }
            }

            List<ZHPoint> list2 = new List<ZHPoint>();
            foreach (ZHPoint zHPoint in zHData.hengneiPoints)
            {
                if (zHPoint.Name.Contains("W"))
                {
                    if (zHPoint.Name.Contains("W6")) list2.Add(zHData.xinPoints[0]);
                    list2.Add(zHPoint);
                }
            }

            int i = 0;
            while (i < list1.Count() - 1)
            {
                H1 += catimian(list1[i], list1[i + 1]);
                i++;
            }
            i = 0;
            while (i < list2.Count() - 1)
            {
                H2 += catimian(list2[i], list2[i + 1]);
                i++;
            }
            H = H1 + H2;
        }

        public Calculate(ZHData zHData)
        {
            FindPoint(zHData);//对关键点和普通点进行分类
            ceshidian(zHData);//测试点AB
            zong(zHData);//纵断面计算
            heng(zHData);//横断面计算
        }

    }
}
