using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace 激光点云数据的平面分割
{
    public class FileHelper
    {
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataCenter ReadFile(string filePath)
        {
            DataCenter dataCenter = new DataCenter();
            StreamReader sr = new StreamReader(filePath);
            string str;
            string[] strs;

            str = sr.ReadLine();
            strs = str.Split(',');
            dataCenter.pointNum = int.Parse(strs[0]);

            PointInfo point;
            while((str = sr.ReadLine()) != null)
            {
                strs = str.Split(',');
                point = new PointInfo(strs[0], double.Parse(strs[1]), double.Parse(strs[2]), double.Parse(strs[3]));
                if (dataCenter.xma < point.X) dataCenter.xma = point.X;
                if (dataCenter.yma < point.Y) dataCenter.yma = point.Y;
                if (dataCenter.zma < point.Z) dataCenter.zma = point.Z;

                if (dataCenter.xmi > point.X) dataCenter.xmi = point.X;
                if (dataCenter.ymi > point.Y) dataCenter.ymi = point.Y;
                if (dataCenter.zmi > point.Z) dataCenter.zmi = point.Z;
                dataCenter.pointInfos.Add(point);
            }
            return dataCenter;
        }
        public static string WriteReport(DataCenter dataCenter)
        {
            string line = "\t点名，\tX，\tY，\tZ，\t标识\n";
            for (int i = 0; i < dataCenter.pointInfos.Count; i++)
            {
                line += dataCenter.pointInfos[i].ToString();
            }
            return line;
        }
    }
}
