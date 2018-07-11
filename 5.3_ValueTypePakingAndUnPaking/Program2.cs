using System;
using System.Collections.Generic;
using System.Text;

namespace _5._3_ValueTypePakingAndUnPaking
{
    internal struct Point : IComparable
    {
        private Int32 m_x, m_y;
        public Point(Int32 x,Int32 y)
        {
            m_x = x;
            m_y = y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", m_x.ToString(), m_y.ToString());
        }

        public Int32 CompareTo(Point other)
        {
            //利用勾股定理计算那个Point距离原点更远
            return Math.Sign(Math.Sqrt(m_x * m_x + m_y * m_y)
                - Math.Sqrt(other.m_x * other.m_x + other.m_y * other.m_y));
        }

        public Int32 CompareTo(Object o)
        {
            if (GetType() != o.GetType())
            {
                throw new ArgumentException("o is not a point");
            }
            //调用类型安全的CompareTo方法
            return CompareTo((Point)o);
        }
    }


    public static class Program2
    {
        //public static void Main()
        //{
        //    //在栈上创建两个Point实例
        //    Point p1 = new Point(10, 10);
        //    Point p2 = new Point(20, 20);

        //    //调用Tostring方法，不装箱
        //    Console.WriteLine(p1.ToString());
        //}
    }
}
