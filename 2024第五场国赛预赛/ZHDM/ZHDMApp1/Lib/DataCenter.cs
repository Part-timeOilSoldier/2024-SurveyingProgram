using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHDMApp1.Lib
{
    class ZHPoint//点数据
    {
        public string Name;
        public double X, Y, H;
        public ZHPoint(string a,string b,string c,string d)//初始化函数
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

    class ZHData//纵横断面数据
    {
        public List<ZHPoint> AllPoints;//读取的全部数据
        public List<ZHPoint> KeyPoints;//关键点
        public List<ZHPoint> NormalPoints;//普通点
        public List<ZHPoint> ZongneiPoints;//纵断面内插点
        public List<ZHPoint> HengneiPoints;//横断面内插点
        public List<ZHPoint> Xin;//横断面中心点
        public ZHData()
        {
            AllPoints = new List<ZHPoint>();
            KeyPoints = new List<ZHPoint>();
            NormalPoints = new List<ZHPoint>();
            ZongneiPoints = new List<ZHPoint>();
            HengneiPoints = new List<ZHPoint>();
            Xin = new List<ZHPoint>();
        }
    }
}
