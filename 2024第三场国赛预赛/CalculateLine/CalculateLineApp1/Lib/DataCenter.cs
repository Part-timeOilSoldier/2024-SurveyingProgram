using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateLineApp1.Lib
{
    class GeoPoint//大地点
    {
        public string Name;
        public double B, L;
        public GeoPoint(string name,string b,string l)
        {
            Name = name;
            B = double.Parse(b);
            L = double.Parse(l);
        }
    }

    class GeoLine//大地线
    {
        public GeoPoint Start, End;
        public double S;
        public GeoLine(string a,string b,string c,string d,string e,string f)
        {
            Start = new GeoPoint(a, b, c);
            End = new GeoPoint(d, e, f);
        }
    }

    class CalculateGeoLine
    {
        public double a, f1;
        public List<GeoLine> geoLines = new List<GeoLine>();
        
    }
}
