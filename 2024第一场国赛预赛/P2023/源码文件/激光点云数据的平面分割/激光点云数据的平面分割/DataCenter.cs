using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 激光点云数据的平面分割
{
    /// <summary>
    /// 数据中心
    /// </summary>
    public class DataCenter
    {
        public int pointNum;
        public List<PointInfo> pointInfos = new List<PointInfo>();
        public List<PointInfo>[,] Matrix = new List<PointInfo>[10, 10];
        public double xma = -1000000, yma=  -1000000, zma = -1000000, xmi = 1000000, ymi = 1000000, zmi = 1000000;
    }
    /// <summary>
    /// 点的基本信息
    /// </summary>
    public class PointInfo
    {
        public string Name;
        public double X, Y, Z;
        public int i, j;
        public string N;
        public PointInfo()
        {

        }
        public PointInfo(string name, double x, double y, double z)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }
        public override string ToString()
        {
            string line = string.Format("\t{0, -8}{1, -10:f3}{2, -10:f3}{3, -10:f3}{4, -5}\n", Name, X, Y, Z, N);
            return line;
        }
    }
}
