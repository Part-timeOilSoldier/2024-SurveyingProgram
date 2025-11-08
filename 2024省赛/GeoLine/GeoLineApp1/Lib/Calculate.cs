using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLineApp1.Lib
{
    class Calculate
    {
        public double JzhuanH(double jiao)//角度转弧度
        {
            //12.34566
            double du = (int)jiao;//12
            double fen = (int)((jiao - du) * 100);//34
            double miao = (((jiao - du) * 100) - fen) * 100;//56.6
            double zong = du + fen / 60 + miao / 3600;
            return zong / 180 * Math.PI;
        }

        public double HzhuanJ(double hu)//弧度转角度
        {
            return hu / Math.PI * 180;
        }

        public void baseca(FoundData foundData)//计算椭球基本参数
        {
            foundData.f = 1 / foundData.f1;
            foundData.b = foundData.a * (1 - foundData.f);
            foundData.e2 = (Math.Pow(foundData.a, 2) - Math.Pow(foundData.b, 2)) / (Math.Pow(foundData.a, 2));
            foundData.e12 = foundData.e2 / (1 - foundData.e2);
        }

        public void fuzhuca(LineData lineData,FoundData foundData)//辅助计算
        {
            double B1 = JzhuanH(lineData.B1);double L1 = JzhuanH(lineData.L1);
            double B2 = JzhuanH(lineData.B2);double L2 = JzhuanH(lineData.L2);
            double e2 = foundData.e2;

            double u1 = Math.Atan((Math.Sqrt(1 - e2)) * (Math.Tan(B1)));
            double u2 = Math.Atan((Math.Sqrt(1 - e2)) * (Math.Tan(B2)));

            double l = L2 - L1;

            double a1 = Math.Sin(u1) * Math.Sin(u2);
            double a2 = Math.Cos(u1) * Math.Cos(u2);
            double b1 = Math.Cos(u1) * Math.Sin(u2);
            double b2 = Math.Sin(u1) * Math.Cos(u2);

            lineData.fuzhuData.u1 = u1;
            lineData.fuzhuData.u2 = u2;
            lineData.fuzhuData.l = l;
            lineData.fuzhuData.a1 = a1;
            lineData.fuzhuData.a2 = a2;
            lineData.fuzhuData.b1 = b1;
            lineData.fuzhuData.b2 = b2;
        }

        //计算起点大地方位角
        public void qifang(LineData lineData,FoundData foundData)
        {
            
            double l = lineData.fuzhuData.l;
            double u1 = lineData.fuzhuData.u1;
            double u2 = lineData.fuzhuData.u2;
            double a1 = lineData.fuzhuData.a1;
            double a2 = lineData.fuzhuData.a2;
            double b1 = lineData.fuzhuData.b1;
            double b2 = lineData.fuzhuData.b2;

            double afa, beta, gama, A1, nan, xita, sinA0;


            double deta = 0; 
            double detaqian = 0;
            while(deta==detaqian)
            {
                detaqian = deta;
                nan = l + deta;
                nan = l + deta;
                double p = Math.Cos(u2) * Math.Sin(nan);
                double q = b1 - b2 * Math.Cos(nan);
                A1 = Math.Atan(p / q);
                A1 = HzhuanJ(A1);

                //判断pq符号
                if (p > 0)
                {
                    if (q > 0) A1 = Math.Abs(A1);
                    else A1 = 180 - Math.Abs(A1);
                }
                else
                {
                    if (q < 0) A1 = 180 + Math.Abs(A1);
                    else A1 = 360 - Math.Abs(A1);
                }

                if (A1 < 0) A1 = A1 + 360;
                else if (A1 > 360) A1 = A1 - 360;
                A1 = JzhuanH(A1);

                double sinxita = p * Math.Sin(A1) + q * Math.Cos(A1);
                double cosxita = a1 + a2 * Math.Cos(nan);
                xita = Math.Atan2(sinxita, cosxita);
                xita = HzhuanJ(xita);

                if (cosxita > 0) xita = Math.Abs(xita);
                else xita = 180 - Math.Abs(xita);
                xita = JzhuanH(xita);

                sinA0 = Math.Cos(u1) * Math.Sin(A1);
                double xita_1 = Math.Atan((Math.Tan(u1)) / (Math.Cos(A1)));
                double A0 = Math.Asin(sinA0);
                double cosA0 = Math.Cos(A0);

                afa = caafa(foundData.e2, cosA0);
                beta = cabeta(foundData.e2, cosA0);
                gama = cagama(foundData.e2, cosA0);

                deta =
                    (afa * xita +
                    beta * Math.Cos(2 * xita_1 + xita) * Math.Sin(xita) +
                    gama * Math.Sin(2 * xita) * Math.Cos(4 * xita_1 + 2 * xita)) * sinA0;

                //赋值afa, beta, gama, A1, nan, xita, sinA0
                lineData.fuzhuData.afa = afa;
                lineData.fuzhuData.beta = beta;
                lineData.fuzhuData.gama = gama;
                lineData.fuzhuData.A1 = A1;
                lineData.fuzhuData.nan = nan;
                lineData.fuzhuData.xita = xita;
                lineData.fuzhuData.sinA0 = sinA0;

            }

        }

        //计算大地线长度
        public void caS(LineData lineData,FoundData foundData)
        {
            double sinA0 = lineData.fuzhuData.sinA0;


            double u1 = lineData.fuzhuData.u1;
            double A1 = lineData.fuzhuData.A1;
            double xita = lineData.fuzhuData.xita;
            double xita_1 = Math.Atan((Math.Tan(u1)) / (Math.Cos(A1)));
            double C = caC(sinA0, foundData.e12, foundData.b);
            double xs = C * Math.Sin(2 * xita) * Math.Cos(4 * xita_1 + 2 * xita);
            double B = caB(sinA0, foundData.e12, foundData.b);
            double A = caA(sinA0, foundData.e12, foundData.b);
            double S = (xita - B * Math.Sin(xita) * Math.Cos(2 * xita_1 + xita) - xs) / A1;

            lineData.fuzhuData.A = A;
            lineData.fuzhuData.B = B;
            lineData.fuzhuData.C = C;
            lineData.fuzhuData.xita_1 = xita_1;
            lineData.fuzhuData.xs = xs;
            lineData.S = S;
        }

        //计算afa
        public double caafa(double e2,double cosA0)
        {
            double e4 = Math.Pow(e2, 2);
            double e6 = Math.Pow(e2, 3);
            double cos2A0 = Math.Pow(cosA0, 2);
            double cos4A0 = Math.Pow(cosA0, 4);

            return (e2 / 2 + e4 / 8 + e6 / 16) - ((e4 / 16 + e6 / 16) * cos2A0) + ((3 * e6 / 128) * cos4A0);
        }

        //计算beta
        public double cabeta(double e2, double cosA0)
        {
            double e4 = Math.Pow(e2, 2);
            double e6 = Math.Pow(e2, 3);
            double cos2A0 = Math.Pow(cosA0, 2);
            double cos4A0 = Math.Pow(cosA0, 4);

            return ((e4 / 16 + e6 / 16) * cos2A0) - ((e6 / 32) * cos4A0);
        }

        //计算gama
        public double cagama(double e2, double cosA0)
        {
            double e4 = Math.Pow(e2, 2);
            double e6 = Math.Pow(e2, 3);
            double cos2A0 = Math.Pow(cosA0, 2);
            double cos4A0 = Math.Pow(cosA0, 4);

            return (e6 / 256) * cos4A0;
        }

        //计算A
        public double caA(double sinA0,double e12,double b)
        {
            double sin2A0 = Math.Pow(sinA0, 2);
            double cos2A0 = 1 - sin2A0;
            double k2 = e12 * cos2A0;
            double k4 = Math.Pow(k2, 2);
            double k6 = Math.Pow(k2, 3);

            return (1 - k2 / 4 + 7 * k4 / 64 - 15 * k6 / 256) / b;
        }

        //计算B
        public double caB(double sinA0, double e12, double b)
        {
            double sin2A0 = Math.Pow(sinA0, 2);
            double cos2A0 = 1 - sin2A0;
            double k2 = e12 * cos2A0;
            double k4 = Math.Pow(k2, 2);
            double k6 = Math.Pow(k2, 3);

            return k2 / 4 - k4 / 8 + 37 * k6 / 512;
        }

        //计算C
        public double caC(double sinA0, double e12, double b)
        {
            double sin2A0 = Math.Pow(sinA0, 2);
            double cos2A0 = 1 - sin2A0;
            double k2 = e12 * cos2A0;
            double k4 = Math.Pow(k2, 2);
            double k6 = Math.Pow(k2, 3);

            return k4 / 128 - k6 / 128;
        }

        public void allca(List<LineData> lineDatas,FoundData foundData)
        {
            double test= JzhuanH(30);
            foreach(LineData lineData in lineDatas)
            {
                baseca(foundData);
                fuzhuca(lineData, foundData);
                qifang(lineData, foundData);
                caS(lineData, foundData);
            }
        }

    }
}
