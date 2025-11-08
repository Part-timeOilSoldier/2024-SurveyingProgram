using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 激光点云数据的平面分割
{
    public class RANSAC
    {
        DataCenter dataCenter;

        public RANSAC(DataCenter dataCenter)
        {
            this.dataCenter = dataCenter;
        }
        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public double CalDis(PointInfo P1, PointInfo P2)
        {
            return Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));

        }
        /// <summary>
        /// 判断是否共线
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="P3"></param>
        /// <returns></returns>
        public bool CalTRF(PointInfo P1, PointInfo P2, PointInfo P3, out double S)
        {
            double a = CalDis(P1, P2), b = CalDis(P2, P3), c = CalDis(P3, P1);
            double p = (a + b + c) / 2.0;
             S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            if (S > 0.1)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 计算ABCD的值
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="P3"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        public void CalABCD(PointInfo P1, PointInfo P2, PointInfo P3, out double A, out double B, out double C, out double D)
        {
            A = (P2.Y - P1.Y) * (P3.Z - P1.Z) - (P3.Y - P1.Y) * (P2.Z - P1.Z);
            B = (P2.Z - P1.Z) * (P3.X - P1.X) - (P3.Z - P1.Z) * (P2.X - P1.X);
            C = (P2.X - P1.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P1.Y);
            D = -A * P1.X - B * P1.Y - C * P1.Z;
        }
        /// <summary>
        /// 计算点到平面的距离
        /// </summary>
        /// <param name="P0"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public double CalDisP(PointInfo P0, double A, double B, double C, double D)
        {
            return Math.Abs(A * P0.X + B * P0.Y + C * P0.Z + D) / Math.Sqrt(A * A + B * B + C * C);
        }

        /// <summary>
        /// 计算内部点
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public List<PointInfo> CalInnerPoint(double A, double B, double C, double D, PointInfo P1, PointInfo P2, PointInfo P3)
        {
            List<PointInfo> InnerPoints = new List<PointInfo>();
            for (int i = 0; i < dataCenter.pointNum; i++)
            {
                if (dataCenter.pointInfos[i].Name == P1.Name) continue;
                if (dataCenter.pointInfos[i].Name == P2.Name) continue;
                if (dataCenter.pointInfos[i].Name == P3.Name) continue;
                if (CalDisP(dataCenter.pointInfos[i], A, B, C, D) < 0.1)
                {
                    InnerPoints.Add(dataCenter.pointInfos[i]);
                    dataCenter.pointInfos[i].N = "J1";
                }
            }
            return InnerPoints;
        }
        /// <summary>
        /// 迭代找最佳分割平面
        /// </summary>
        /// <returns></returns>
        public List<PointInfo> CalMaxInner(out double A1, out double B1, out double C1, out double D1, out List<PointInfo> Minll)
        {
            List<PointInfo> Maxll = new List<PointInfo>();
             Minll = new List<PointInfo>();
            A1 = B1 = C1 = D1 = 0;

            var data = dataCenter.pointInfos;
            int num = 0;
            for (int i = 0; i < dataCenter.pointNum; i++)
            {
                if (num == 300) break;
                double S, A = 0,B = 0,C = 0, D = 0;
                List<PointInfo> Points = new List<PointInfo>();
                List<PointInfo> MPoints = new List<PointInfo>();
                var trf = CalTRF(data[3 * i], data[3 * i + 1], data[3 * i + 2], out S);
                if(trf == true)
                {
                    CalABCD(data[3 * i], data[3 * i + 1], data[3 * i + 2], out A, out B, out C, out D);
                    Points = CalInnerPoint(A, B, C, D, data[3 * i], data[3 * i + 1], data[3 * i + 2]);
                    for (int q = 0; q < dataCenter.pointInfos.Count; q++)
                    {
                        PointInfo P = Points.Find(x => x.Name.Equals(dataCenter.pointInfos[q].Name));
                        if (dataCenter.pointInfos[q].Name == data[3 * i].Name || dataCenter.pointInfos[q].Name == data[3 * i + 1].Name || dataCenter.pointInfos[q].Name == data[3 * i + 2].Name) continue;
                            if (P == null)
                        {
                                MPoints.Add(dataCenter.pointInfos[q]);
                        }
                    }
                    if (num == 0) Maxll = Points;
                }
                if (Maxll.Count < Points.Count) { Maxll = Points; A1 = A; B1 = B; C1 = C; D1 = D; Minll = MPoints; }
                num++;
            }
            return Maxll;
        }
        /// <summary>
        /// 二次计算
        /// </summary>
        /// <param name="Minaa"></param>
        /// <param name="A1"></param>
        /// <param name="B1"></param>
        /// <param name="C1"></param>
        /// <param name="D1"></param>
        /// <returns></returns>
        public List<PointInfo> CalJ2(List<PointInfo> Minaa, out double A1, out double B1, out double C1, out double D1)
        {
            List<PointInfo> Maxll = new List<PointInfo>();

            A1 = B1 = C1 = D1 = 0;

            var data = Minaa;
            int num = 0;
            for (int i = 0; i < Minaa.Count; i++)
            {
                if (num == 80) break;
                double S, A = 0, B = 0, C = 0, D = 0;
                List<PointInfo> Points = new List<PointInfo>();

                var trf = CalTRF(data[3 * i], data[3 * i + 1], data[3 * i + 2], out S);
                if (trf == true)
                {
                    CalABCD(data[3 * i], data[3 * i + 1], data[3 * i + 2], out A, out B, out C, out D);
                    Points = CalInnerPointJ2(A, B, C, D, data[3 * i], data[3 * i + 1], data[3 * i + 2], Minaa);
                }
                if (Maxll.Count < Points.Count) { Maxll = Points; A1 = A; B1 = B; C1 = C; D1 = D; }
                num++;
            }
            return Maxll;
        }
        /// <summary>
        /// 二次计算内插点
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="P3"></param>
        /// <param name="Minall"></param>
        /// <returns></returns>
        public List<PointInfo> CalInnerPointJ2(double A, double B, double C, double D, PointInfo P1, PointInfo P2, PointInfo P3, List<PointInfo> Minall)
        {
            List<PointInfo> InnerPoints = new List<PointInfo>();
            for (int i = 0; i < Minall.Count; i++)
            {
                if (Minall[i].Name == P1.Name) continue;
                if (Minall[i].Name == P2.Name) continue;
                if (Minall[i].Name == P3.Name) continue;
                if (CalDisP(Minall[i], A, B, C, D) < 0.1)
                {
                    InnerPoints.Add(Minall[i]);
                    for (int p = 0; p < dataCenter.pointNum; p++)
                    {
                        if(dataCenter.pointInfos[p].Name == Minall[i].Name)
                        {
                            dataCenter.pointInfos[p].N = "J2";
                        }
                    }
                }
            }
            return InnerPoints;
        }

        public void CalShodow(PointInfo P, double A, double B, double C, double D, out double x1, out double y1, out double z1)
        {
            x1 = ((B * B + C * C) * P.X - A * (B * P.Y + C * P.Z + D)) / (A * A + B * B + C * C);
            y1 = ((A * A + C * C) * P.Y - B * (A * P.X + C * P.Z + D)) / (A * A + B * B + C * C);
            z1 = ((B * B + A * A) * P.Z - C * (A * P.X + B * P.Y + D)) / (A * A + B * B + C * C);
        }
    }
}
