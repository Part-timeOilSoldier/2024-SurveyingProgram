using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPointsApp1.Lib
{
    class DataPoint//点数据
    {
        public string Name;
        public double X, Y, Z;
        public DataPoint(string name, string x, string y, string z)
        {
            Name = name;
            X = double.Parse(x);
            Y = double.Parse(y);
            Z = double.Parse(z);
        }
    }

    class ShanGe//栅格数据
    {
        public List<DataPoint> dataPoints;
        public double meanH;//平均高度
        public double maxH, minH, maxmin;//高度最大值和最小值及其差值
        public ShanGe()
        {
            dataPoints = new List<DataPoint>();
        }
    }

    class PingMian//三点平面
    {
        public DataPoint P1, P2, P3;//三个点
        public double p, A, B, C, D;//平面参数
        public double a, b, c;//三点间距离
        public double S;//平面面积
        public List<DataPoint> neiPoints = new List<DataPoint>();//内部点
        public double nei, wai;
        public PingMian()
        {
            neiPoints = new List<DataPoint>();
        }
    }
}
