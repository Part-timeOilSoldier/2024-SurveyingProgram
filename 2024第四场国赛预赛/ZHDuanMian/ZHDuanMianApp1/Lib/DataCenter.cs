using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDuanMianApp1.Lib
{
    class ZHPoint//普通点
    {
        public string Name;
        public double X, Y, H;
        public ZHPoint(string a,string b,string c,string d)
        {
            Name = a;
            X = double.Parse(b);
            Y = double.Parse(c);
            H = double.Parse(d);
        }
        public ZHPoint()
        {

        }
    }

    class KeyPoint//关键点集合
    {
        List<ZHPoint> keyPoints;
        public KeyPoint(List<ZHPoint> zHPoints)//初始化关键点时直接寻找到包含K的点并从原点集中删除
        {
            keyPoints = new List<ZHPoint>();
            keyPoints.Add(zHPoints.Find(p => p.Name.Contains("K0")));
            keyPoints.Add(zHPoints.Find(p => p.Name.Contains("K1")));
            keyPoints.Add(zHPoints.Find(p => p.Name.Contains("K2")));
            zHPoints.Remove(zHPoints.Find(p => p.Name.Contains("K0")));
            zHPoints.Remove(zHPoints.Find(p => p.Name.Contains("K1")));
            zHPoints.Remove(zHPoints.Find(p => p.Name.Contains("K2")));
        }
    }

    class ZHData//包含普通点和关键点的集合
    {
        public double basepoint = 100.000;
        public List<ZHPoint> zHPoints;//所有点
        public List<ZHPoint> keyPoints;//关键点
        public List<ZHPoint> ptPoints;//普通点
        public List<ZHPoint> neiPoints;//纵断面内插点
        public List<ZHPoint> xinPoints;//中心点
        public List<ZHPoint> hengneiPoints;//横断面内插点
        public ZHData()
        {
            zHPoints = new List<ZHPoint>();
            keyPoints = new List<ZHPoint>();
            ptPoints = new List<ZHPoint>();
            neiPoints = new List<ZHPoint>();
            xinPoints = new List<ZHPoint>();
            hengneiPoints = new List<ZHPoint>();
        }
    }
}
