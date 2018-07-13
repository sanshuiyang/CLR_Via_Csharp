using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13_Interface
{
    //public interface ICompareble<in T>
    //{
    //    Int32 CompareTo(T other);
    //}
    
    //public sealed class Point: _13_Interface.ICompareble<Point>
    //{
    //    private Int32 m_x, m_y;
    //    public Point(Int32 x, Int32 y)
    //    {
    //        m_x = x;
    //        m_y = y;
    //    }

    //    public Int32 CompareTo(Point other)
    //    {
    //        return Math.Sign(Math.Sqrt(m_x*m_x +m_y*m_y))-
    //            Math.Sign(other.m_x*other.m_x+other.m_y*other.m_y));
    //    }

    //    public override string ToString()
    //    {
    //        return String.Format("({0},{1})", m_x, m_y);
    //    }
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Point[] points = new Point[]
    //        {
    //            new Point(3,3),
    //            new Point(1,2)
    //        };

    //        if (points[0].CompareTo(points[1]) > 0) {
    //            points[0] = points[1];
    //        }
    //    }
    //}

    public sealed class SimpleType : IDisposable
    {
        public void Dispose() { Console.WriteLine("public Dispose"); }
        void IDisposable.Dispose() { Console.WriteLine("Idisposeble Dispose"); }
    }

    public sealed class Program
    {
        public static void Main()
        {
            SimpleType st = new SimpleType();

            st.Dispose();

            IDisposable d = st;

            d.Dispose();
        }
    }
}
