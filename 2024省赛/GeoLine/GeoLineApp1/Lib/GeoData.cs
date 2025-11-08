using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLineApp1.Lib
{
    class LineData
    {
        //一条大地线的基本参数
        public string Start, End;
        public double B1, L1, B2, L2, S;
        public FuzhuData fuzhuData;

        //初始化函数
        public LineData(string start,string b1,string l1,string end, string b2,string l2)
        {
            fuzhuData = new FuzhuData();
            Start = start; End = end;
            B1 = double.Parse(b1);
            L1 = double.Parse(l1);
            B2 = double.Parse(b2);
            L2 = double.Parse(l2);
        }
    }

    class FoundData//椭球基本参数
    {
        public double a, b, e2, e12, f, f1;

    }

    class FuzhuData//辅助参数
    {
        //辅助计算
        public double u1, u2, l, a1, a2, b1, b2;

        //大地方位角参数
        public double A1,nan;
        public double deta = 0;
        public double p, q, xita;
        public double sinA0, xita_1;
        public double afa, beta, gama;

        public double xs, A, B, C;

    }
}
