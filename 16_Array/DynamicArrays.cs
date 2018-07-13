using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16_Array
{
    /// <summary>
    /// 动态创建下标不为0的数组
    /// </summary>
    /*
    class DynamicArrays
    {
        static void Main(string[] args)
        {
            Int32[] lowerBounds = { 2005, 1 };
            Int32[] lengths = { 5, 4 };
            Decimal[,] quarterlyRevenue = (Decimal[,])Array.CreateInstance(typeof(Decimal), lengths, lowerBounds);

            Console.WriteLine("{0,4}{1,9}{2,9}{3,9}{4,9}",
                "Year","Q1,","Q2","Q3","Q4");
            Int32 firstYear = quarterlyRevenue.GetLowerBound(0);
            Int32 lastYear = quarterlyRevenue.GetUpperBound(0);
            Int32 firstQuarter = quarterlyRevenue.GetLowerBound(1);
            Int32 lastQuarter = quarterlyRevenue.GetUpperBound(1);

            for(Int32 year = firstYear; year <= lastYear; year++)
            {
                Console.Write(year + " ");
                for(Int32 quarter=firstQuarter; quarter <= lastQuarter; quarter++)
                   Console.Write("{0,9:c}",quarterlyRevenue[year, quarter]);
                Console.WriteLine();
            }
        }
    }
    */

    //演示了访问二维数组的三种方式（安全、交错和不安全）
    public sealed class Program
    {
        private const Int32 c_numElemts = 10000;

        public static void Main()
        {
            //声明二维数组
            Int32[,] a2Dim = new Int32[c_numElemts,c_numElemts];

            //将二维数组声明为交错数组（向量构成的向量）
            Int32[][] aJagged = new Int32[c_numElemts][];
            for(Int32 x = 0; x < c_numElemts; x++)
            {
                aJagged[x] = new Int32[c_numElemts];
            }

            //普通的安全技术
            Safa2DimArrayAccess(a2Dim);

            //交错数组技术访问
            SafeJaggeArrayAccess(aJagged);

            //用unsafe技术访问
            Unsafe2DimArrayAccess(a2Dim);
        }

        private static Int32 Safa2DimArrayAccess(Int32[,] a)
        {
            Int32 sum = 0;
            for(Int32 x = 0; x < c_numElemts; x++)
            {
                for (Int32 y = 0; y < c_numElemts; y++)
                    sum += a[x, y];
            }
            return sum;
        }

        private static Int32 SafeJaggeArrayAccess(Int32[][] a)
        {
            Int32 sum = 0;
            for(Int32 x = 0; x < c_numElemts; x++)
            {
                for(Int32 y = 0; y < c_numElemts; y++)
                {
                    sum += a[x][y];
                }
            }
            return sum;
        }

        private static unsafe Int32 Unsafe2DimArrayAccess(Int32[,] a)
        {
            Int32 sum = 0;
            fixed(Int32* pi = a)
            {
                for(Int32 x = 0; x < c_numElemts; x++)
                {
                    Int32 baseOfDim = x * c_numElemts;
                    for(Int32 y = 0; y < c_numElemts; y++)
                    {
                        sum += pi[baseOfDim + y];
                    }
                }
            }
            return sum;
        }
    }
}
