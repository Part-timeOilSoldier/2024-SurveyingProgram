using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateLineApp1.Lib
{
    class DataPoint
    {
        public string Name;
        public double B;
        public double L;

        public DataPoint(string name,string b,string l)
        {
            Name = name;
            B = double.Parse(b);
            L = double.Parse(l);
        }
    }

    class GeoLine
    {
        public double S;
        public DataPoint Start, End;

        public GeoLine(string a,string b,string c,string d,string e,string f)
        {
            Start = new DataPoint(a, b, c);
            End = new DataPoint(d, e, f);
        }
    }

    class CalsulateLines
    {
        public double a, f1;
        public List<GeoLine> geoLines;
        public CalsulateLines()
        {
            geoLines = new List<GeoLine>();
        }
    }
}
