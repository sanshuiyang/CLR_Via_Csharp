using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _21_GC
{
    public static class M_HandleCoolector
    {
        //MemmoryPressureDemo 可看出代的思想
        public static void Main_test()
        {
            MemmoryPressureDemo(0);//0 不导致频繁的GC
            MemmoryPressureDemo(10 * 1024 * 1024); //10M 导致频繁的GC

            HandleCollectDemo();
        }

        private static void MemmoryPressureDemo(Int32 size)
        {
            Console.WriteLine();
            Console.WriteLine("MemoryPressureDemo,size={0}", size);

            //创建一组对象，并制定它们的逻辑大小
            for (Int32 count = 0; count < 15; count++)
            {
                new BigNativeResource(size);
            }

            //演示
            GC.Collect();
        }

        private sealed class BigNativeResource
        {
            private Int32 m_size;

            public BigNativeResource(Int32 size)
            {
                m_size = size;
                //使垃圾回收器认为对象在物理上比较大
                if (m_size > 0) GC.AddMemoryPressure(m_size);
                Console.WriteLine("BigNativeResource create.");
            }

            ~BigNativeResource()
            {
                //使用垃圾回收器认为对象释放了更多的内存
                if (m_size > 0) GC.RemoveMemoryPressure(m_size);
                Console.WriteLine("BigNativeResource destroy.");
            }
        }
        
        private static void HandleCollectDemo()
        {
            Console.WriteLine();
            Console.WriteLine("HandleCollectDemo");
            for(Int32 count = 0; count < 10; count++)
            {
                new LimitedResource();
            }
            GC.Collect();
        }

        private sealed class LimitedResource
        {
            //创建一个HandleCollector，告诉它当两个或者更多这样的对象
            //存在于堆中时，就执行回收
            private static readonly HandleCollector s_hc = new HandleCollector("LimitedResource", 2);

            public LimitedResource()
            {
                //告诉HandleCollector堆中增加一个LimitedResource对象
                s_hc.Add();
                Console.WriteLine("LimitedResource creat,count = {0}", s_hc.Count);
            }

            ~LimitedResource()
            {
                s_hc.Remove();
                Console.WriteLine("LimitedResource destroy . Count = {0}", s_hc.Count);
            }
        }
    }
}
