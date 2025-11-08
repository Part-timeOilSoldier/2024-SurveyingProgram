using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDMApp1.Lib
{
    class Calculate
    {
        double D0, D1, D;//纵断面长度
        double afa01, afa12;//方位角01和12
        double zongS1, zongS2, zongS;//纵断面面积
        double afam0, afam1;//横断面方位角
        double srow1, srow2, srow;//横断面面积

        public void splitdata(ZHData zHData)//对读取的数据进行分组，分出关键点和普通点
        {
            foreach(ZHPoint zHPoint in zHData.AllPoints)
            {
                if (zHPoint.Name.Contains("K"))
                {
                    zHData.KeyPoints.Add(zHPoint);
                }
                else zHData.NormalPoints.Add(zHPoint);
            }
        }

        public double HzhuanJ(double hu)//弧度转角度
        {
            return hu / Math.PI * 180;
        }

        public double JzhuanH(double jiao)//角度转弧度
        {
            return jiao / 180 * Math.PI;
        }

        public double cafangweijiao(ZHPoint A,ZHPoint B)//计算AB两点间的方位角
        {
            double detax = B.X - A.X;
            double detay = B.Y - A.Y;
            double afaab = Math.Atan(detay / detax);//算出来弧度
            afaab = HzhuanJ(afaab);//转为角度进行判断变换
            //下面进行判断并再次转为弧度
            if(detay>0)
            {
                if (detax > 0) afaab = JzhuanH(afaab);
                else if (detax < 0) afaab = JzhuanH(180 + afaab);
                else if (detax == 0) afaab = JzhuanH(90);
            }
            else if(detay<0)
            {
                if (detax < 0) afaab = JzhuanH(180 + afaab);
                else if (detax > 0) afaab = JzhuanH(360 + afaab);
                else if (detax == 0) afaab = JzhuanH(270);
            }
            return afaab;//返回计算得到的AB方位角
        }

        public double cajuli(ZHPoint A, ZHPoint B)//计算AB两点间的距离
        {
            double deta1 = A.X - B.X;
            double deta2 = A.Y - B.Y;
            double a = Math.Sqrt(Math.Pow(deta1, 2) + Math.Pow(deta2, 2));
            return a;
        }

        public void caneigao(ZHData zHData, ZHPoint zHPoint)//计算内插点高程值
        {
            List<ZHPoint> zHPoints = new List<ZHPoint>();//存储最近的点
            zHPoints = zHData.AllPoints.OrderBy(p => cajuli(zHPoint, p)).Take(5).ToList();//取距离最近的五个点

            double zong1 = 0;
            double zong2 = 0;
            foreach(ZHPoint temp in zHPoints)
            {
                zong1 += temp.H / (cajuli(zHPoint, temp));
                zong2 += 1 / (cajuli(zHPoint, temp));
            }
            zHPoint.H = zong1 / zong2;

        }

        public double catiS(ZHPoint A, ZHPoint B)//计算两点间断面的面积
        {
            double S = (A.H + B.H - 2 * 100) / 2 * cajuli(A, B);
            return S;
        }

        public void ceshi(ZHData zHData)//创建AB点并进行测试
        {
            ZHPoint A = new ZHPoint();ZHPoint B = new ZHPoint();
            A.X = 129.676;A.Y = 538.599;
            B.X = 124.471;B.Y = 526.233;
            double jiao = cafangweijiao(A, B);
            caneigao(zHData, A);caneigao(zHData, B);
            double tis = catiS(A, B);
        }

        public void cazongju(ZHData zHData)//计算纵断面的长度
        {
            D0 = cajuli(zHData.KeyPoints[0], zHData.KeyPoints[1]);
            D1 = cajuli(zHData.KeyPoints[1], zHData.KeyPoints[2]);
            D = D0 + D1;
        }

        public void findzongnei(ZHData zHData)//找到纵断面内插点(包含计算方位角afa01,afa12)
        {
            //先计算方位角
            afa01 = cafangweijiao(zHData.KeyPoints[0], zHData.KeyPoints[1]);
            afa12 = cafangweijiao(zHData.KeyPoints[1], zHData.KeyPoints[2]);

            //判断点在哪
            zHData.ZongneiPoints.Add(zHData.KeyPoints[0]);
            double deta = 10;
            int i = 1;
            while(deta<=D0)//点在K0K1上
            {
                ZHPoint zHPoint = new ZHPoint();
                zHPoint.X = zHData.KeyPoints[0].X + deta * Math.Cos(afa01);
                zHPoint.Y = zHData.KeyPoints[0].Y + deta * Math.Sin(afa01);
                caneigao(zHData, zHPoint);
                zHPoint.Name = "Z" + i.ToString();
                zHData.ZongneiPoints.Add(zHPoint);
                deta += 10;i++;
            }i = 1; zHData.ZongneiPoints.Add(zHData.KeyPoints[1]);
            while (deta<=D)//点在K1K2上
            {
                ZHPoint zHPoint = new ZHPoint();
                zHPoint.X = zHData.KeyPoints[1].X + (deta - D0) * Math.Cos(afa12);
                zHPoint.Y = zHData.KeyPoints[1].Y + (deta - D0) * Math.Sin(afa12);
                caneigao(zHData, zHPoint);
                zHPoint.Name = "Y" + i.ToString();
                zHData.ZongneiPoints.Add(zHPoint);
                deta += 10;i++;
            }zHData.ZongneiPoints.Add(zHData.KeyPoints[2]);        
        }

        public void cazongS(ZHData zHData)//计算纵断面面积
        {
            int i = 0;
            while(i<13)
            {
                zongS1 += catiS(zHData.ZongneiPoints[i], zHData.ZongneiPoints[i + 1]);
                i++;
            }
            while(i<21)
            {
                zongS2 += catiS(zHData.ZongneiPoints[i], zHData.ZongneiPoints[i + 1]);
                i++;
            }
            zongS = zongS1 + zongS2;
        }

        public void findxin(ZHData zHData)//计算横断面中心点
        {
            int i = 0;
            while(i<zHData.KeyPoints.Count()-1)
            {
                ZHPoint zHPoint = new ZHPoint();
                zHPoint.X = (zHData.KeyPoints[i].X + zHData.KeyPoints[i + 1].X) / 2;
                zHPoint.Y = (zHData.KeyPoints[i].Y + zHData.KeyPoints[i + 1].Y) / 2;
                caneigao(zHData, zHPoint);
                zHPoint.Name = "M" + i.ToString();
                zHData.Xin.Add(zHPoint);
                i++;
            }
        }

        public void findhengnei(ZHData zHData)//找到横断面内插点(包含计算横断面方位角afam0,afam1)
        {
            //计算横断面方位角
            afam0 = JzhuanH(HzhuanJ(afa01) + 90);
            afam1 = JzhuanH(HzhuanJ(afa12) + 90);

            //找到横断面内插点
            double[] temparry = new double[10] { -5, -4, -3, -2, -1, 1, 2, 3, 4, 5 };
            double deta = 5;
            int i = 1;
            foreach(double j in temparry)
            {
                if(j==1)
                {
                    zHData.HengneiPoints.Add(zHData.Xin[0]);
                }
                ZHPoint zHPoint = new ZHPoint();
                zHPoint.X = zHData.Xin[0].X + j * deta * Math.Cos(afam0);
                zHPoint.Y = zHData.Xin[0].Y + j * deta * Math.Sin(afam0);
                zHPoint.Name = "Q" + i.ToString(); i++;
                caneigao(zHData, zHPoint);
                zHData.HengneiPoints.Add(zHPoint);
            }i = 1;
            foreach (double j in temparry)
            {
                if (j == 1)
                {
                    zHData.HengneiPoints.Add(zHData.Xin[1]);
                }
                ZHPoint zHPoint = new ZHPoint();
                zHPoint.X = zHData.Xin[1].X + j * deta * Math.Cos(afam1);
                zHPoint.Y = zHData.Xin[1].Y + j * deta * Math.Sin(afam1);
                zHPoint.Name = "W" + i.ToString(); i++;
                caneigao(zHData, zHPoint);
                zHData.HengneiPoints.Add(zHPoint);
            }
        }

        public void cahengS(ZHData zHData)//计算横断面面积
        {
            int i = 0;
            while(i<10)
            {
                srow1 += catiS(zHData.HengneiPoints[i], zHData.HengneiPoints[i + 1]);
                i++;
            }i++;
            while(i<21)
            {
                srow2 += catiS(zHData.HengneiPoints[i], zHData.HengneiPoints[i + 1]);
                i++;
            }
            srow = srow1 + srow2;
        }

        public Calculate(ZHData zHData)
        {
            ceshi(zHData);
            splitdata(zHData);
            cazongju(zHData);
            findzongnei(zHData);
            cazongS(zHData);
            findxin(zHData);
            findhengnei(zHData);
            cahengS(zHData);
        }
    }
}
