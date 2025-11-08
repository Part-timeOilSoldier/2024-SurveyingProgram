using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 激光点云数据的平面分割
{
   public class Algorithm
    {
        DataCenter dataCenter;

        public Algorithm(DataCenter dataCenter)
        {
            this.dataCenter = dataCenter;
        }
        /// <summary>
        /// 求出点的归属
        /// </summary>
        public void Calij()
        {
            for (int i = 0; i < dataCenter.pointNum; i++)
            {
                var point = dataCenter.pointInfos[i];
                point.i = (int)Math.Floor(point.Y / 10.0);
                point.j = (int)Math.Floor(point.X / 10.0);
                if(dataCenter.Matrix[point.i, point.j] == null)
                    dataCenter.Matrix[point.i, point.j] = new List<PointInfo>();
                dataCenter.Matrix[point.i, point.j].Add(point);
            }
        }
        /// <summary>
        /// 求平均高度
        /// </summary>
        public double[,] CalEvaZ()
        {
            double[, ] Z = new double[10, 10];
            for (int i = 0; i < dataCenter.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < dataCenter.Matrix.GetLength(1); j++)
                {
                    double temp = 0;
                    for (int k = 0; k < dataCenter.Matrix[i, j].Count; k++)
                    {
                        temp += dataCenter.Matrix[i, j][k].Z;
                    }
                    Z[i, j] = temp / dataCenter.Matrix[i, j].Count;
                }
            }
            return Z;
        }
        /// <summary>
        /// 计算高差
        /// </summary>
        /// <returns></returns>
        public double[,] CalDisH()
        {
            double[,] DisH = new double[10, 10];
            for (int i = 0; i < dataCenter.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < dataCenter.Matrix.GetLength(1); j++)
                {
                    var data = dataCenter.Matrix[i, j];
                    data.Sort((x, y) => x.Z.CompareTo(y.Z));
                    DisH[i, j] = data[data.Count - 1].Z - data[0].Z;
                }
            }
            return DisH;
        }
        /// <summary>
        /// 求方差
        /// </summary>
        /// <returns></returns>
        public double[,] Calsigma2()
        {
            double[,] sigma2 = new double[10, 10];
            var Z = CalEvaZ();
            for (int i = 0; i < dataCenter.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < dataCenter.Matrix.GetLength(1); j++)
                {
                    double temp = 0;
                    var data = dataCenter.Matrix[i, j];
                    for (int k = 0; k < dataCenter.Matrix[i, j].Count; k++)
                    {
                        temp += Math.Pow(data[k].Z - Z[i, j], 2);
                    }
                    sigma2[i, j] = temp / dataCenter.Matrix[i, j].Count;
                }
            }
            return sigma2;
        }

    }
}
