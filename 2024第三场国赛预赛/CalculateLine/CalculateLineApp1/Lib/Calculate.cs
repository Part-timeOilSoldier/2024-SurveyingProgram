using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculateLineApp1.Lib
{
    class Calculate
    {
        double B1, L1, B2, L2;//起始点和终点参数.均为弧度
        double a, f, b, e2, e12;//大地基础参数
        double u1, u2, l, a1, a2, b1, b2;//辅助计算参数
        double p, q, A1, nan, deta, jiaoA1, sinxita, cosxita, xita, jiaoxita, sinA0, xita1;//计算起点大地方位角参数
        double afa, beta, gama;//计算起点大地方位角的参数
        double xs;//计算大地线长度参数
        double A, B, C;//计算大地线长度所用到的参数

        public double JzhuanH(double jiao)//角度dd.mmsss转弧度
        {
            double du = (int)jiao;//dd
            double fen = (int)((jiao - du) * 100);//mm
            double miao = (((jiao - du) * 100) - fen) * 100;//ss.s
            double zong = du + fen / 60 + miao / 3600;
            double hudu = zong / 180 * Math.PI;
            return hudu;
        }

        public double JzhuanHbase(double jiao)//角度转弧度
        {
            double hu = jiao / 180 * Math.PI;
            return hu;
        }

        public double HzhuanJ(double hu)//弧度转角度
        {
            double jiao = hu / Math.PI * 180;
            return jiao;
        }

        public void cabasedata(CalculateGeoLine calculateGeoLine, GeoLine geoLine)//椭球基本参数
        {
            B1 = geoLine.Start.B;L1 = geoLine.Start.L;
            B2 = geoLine.End.B; L2 = geoLine.End.L;
            B1 = JzhuanH(B1); L1 = JzhuanH(L1); B2 = JzhuanH(B2); L2 = JzhuanH(L2);
            a = calculateGeoLine.a;
            f = 1 / calculateGeoLine.f1;
            b = a * (1 - f);
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / (Math.Pow(a, 2));
            e12 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / (Math.Pow(b, 2));
        }

        public void cafuzhu()//辅助计算
        {
            u1 = Math.Atan(Math.Sqrt(1 - e2) * Math.Tan(B1));
            u2 = Math.Atan(Math.Sqrt(1 - e2) * Math.Tan(B2));

            l = L2 - L1;

            a1 = Math.Sin(u1) * Math.Sin(u2);
            a2 = Math.Cos(u1) * Math.Cos(u2);
            b1 = Math.Cos(u1) * Math.Sin(u2);
            b2 = Math.Sin(u1) * Math.Cos(u2);
        }

        public void caabg()//计算afa,beta,gama
        {
            double e4 = Math.Pow(e2, 2); double e6 = Math.Pow(e2, 3);
            double A0 = Math.Asin(sinA0);double cosA0 = Math.Cos(A0);
            double cos2A0 = Math.Pow(cosA0, 2);double cos4A0 = Math.Pow(cosA0, 4);
            afa = (e2 / 2 + e4 / 8 + e6 / 16) - (e4 / 16 + e6 / 16) * cos2A0 + (3 * e6 / 128) * cos4A0;
            beta= (e4 / 16 + e6 / 16) * cos2A0 - (e6 / 32) * cos4A0;
            gama = (e6 / 256) * cos4A0;
        }

        public void cafangweijiao()//计算起点大地方位角
        {
            double temp = 1;
            deta = 0;
            nan = l + deta;
            while((Math.Abs(deta - temp))>=0.0000000001)//迭代
            {
                temp = deta;
                p = Math.Cos(u2) * Math.Sin(nan);
                q = b1 - b2 * Math.Cos(nan);
                A1 = Math.Atan(p / q);//弧度
                jiaoA1 = HzhuanJ(A1);//角度

                if (p > 0)//判断
                {
                    if (q > 0) jiaoA1 = Math.Abs(jiaoA1);
                    else if (q < 0) jiaoA1 = 180 - Math.Abs(jiaoA1);
                }
                else if (p < 0)
                {
                    if (q < 0) jiaoA1 = 180 + Math.Abs(jiaoA1);
                    else if (q > 0) jiaoA1 = 360 - Math.Abs(jiaoA1);
                }
                if (jiaoA1 < 0) jiaoA1 = jiaoA1 + 360;
                else if (jiaoA1 > 360) jiaoA1 = jiaoA1 - 360;
                A1 = JzhuanHbase(jiaoA1);//判断后的A1角度转为弧度

                sinxita = p * Math.Sin(A1) + q * Math.Cos(A1);
                cosxita = a1 + a2 * Math.Cos(nan);
                xita = Math.Atan2(sinxita, cosxita);//弧度
                jiaoxita = HzhuanJ(xita);//角度
                if (cosxita > 0) jiaoxita = Math.Abs(jiaoxita);
                else if (cosxita < 0) jiaoxita = 180 - Math.Abs(jiaoxita);
                xita = JzhuanHbase(jiaoxita);//判断后的角西塔转为弧度

                sinA0 = Math.Cos(u1) * Math.Sin(A1);
                xita1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                caabg();
                deta = (afa * xita +
                    beta * Math.Cos(2 * xita1 + xita) * Math.Sin(xita) +
                    gama * Math.Sin(2 * xita) * Math.Cos(4 * xita1 + 2 * xita)) * sinA0;
                nan = l + deta;
            }
        }

        public void caABC()//计算ABC
        {
            double cos2A0 = 1 - Math.Pow(sinA0, 2);
            double k2 = e12 * cos2A0;
            double k4 = Math.Pow(k2, 2);
            double k6 = Math.Pow(k2, 3);
            A = (1 - k2 / 4 + 7 * k4 / 64 - 15 * k6 / 256) / b;
            B = k2 / 4 - k4 / 8 + 37 * k6 / 512;
            C = k4 / 128 - k6 / 128;
        }

        public void cas(GeoLine geoLine)
        {
            xita1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
            caABC();
            xs = C * Math.Sin(2 * xita) * Math.Cos(4 * xita1 + 2 * xita);
            geoLine.S = (xita - B * Math.Sin(xita) * Math.Cos(2 * xita1 + xita) - xs) / A;
        }

        public Calculate(CalculateGeoLine calculateGeoLine)
        {
            int index = 0;
            foreach (GeoLine geoLine in calculateGeoLine.geoLines)
            {
                cabasedata(calculateGeoLine, geoLine);
                cafuzhu();
                cafangweijiao();
                cas(geoLine);
                calculateGeoLine.geoLines[index].S = geoLine.S;
                string text = string.Format("bata{0:N8},gama{1:N8},A{2:N8},C{3:N8}", this.beta, this, gama, this.A, this.C);
                Console.WriteLine(text);
                index++;
            }
            
        }
    }
}
