using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeaceAnalyseApp1.Lib
{
    class SAPoint//单个空间点
    {
        public string ID;//点ID
        public double X, Y, AreaCode;//xy和区域编号
        public double a, b;//事件点的标准差

        public SAPoint(string a,string b,string c,string d)//读取时初始化方法
        {
            ID = a;
            X = double.Parse(b);
            Y = double.Parse(c);
            AreaCode = double.Parse(d);
        }

        public SAPoint()//创建空单位初始化方法
        {

        }
    }

    class AreaSA//单个区域
    {
        public int areacode;//区域编号
        public List<SAPoint> sAPoints;//区域内点集合
        public double number = 0;//区域内点数量
        public double avgax, avgay;//区域平均中心
        public double Ii,Zi;//局部莫兰指数和Z得分
        public double xi;//区域内的犯罪数量
        public AreaSA(int areaid)
        {
            areacode = areaid;
            sAPoints = new List<SAPoint>();
        }
    }

    class Ell//椭圆参数
    {
        public double A, B, C;
        public double xita, SDEx, SDEy;
    }

    class DataCenter
    {
        public List<SAPoint> sAPoints;//点列表
        public List<AreaSA> areaSAs;//区域集合
        public double avg_x, avg_y;//x和y的平均中心
        public Ell ell;//椭圆参数
        public double[,] quan;//各区域之间的权重矩阵
        public double I, S0;//全局莫兰指数
        public double X;//研究区平均犯罪数量
        public double miu, cta;//局部莫兰指数的平均值和标准差

        public DataCenter()
        {
            sAPoints = new List<SAPoint>();
            areaSAs = new List<AreaSA>();
            ell = new Ell();
            int i = 1;
            while(i<8)//创建七个区域
            {
                AreaSA areaSA = new AreaSA(i);
                areaSAs.Add(areaSA);
                i++;
            }
        }
    }
}
