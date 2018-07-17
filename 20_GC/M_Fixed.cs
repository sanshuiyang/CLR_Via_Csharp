using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20_GC
{
    class M_Fixed
    {
        public static void Main_test()
        {
            Go();
        }

        unsafe public static void Go()
        {
            //分配一系列立即变成垃圾的对象
            for (Int32 x = 0; x < 10000; x++) new Object();
            
            IntPtr originalMemoryAddress;
            Byte[] bytes = new Byte[1000];//在垃圾对象后分配这个数组

            //获取byte[]在内存中的地址
            fixed(Byte* pbytes = bytes)
            {
                originalMemoryAddress = (IntPtr)pbytes;
            }

            //强行进行一次垃圾回收，垃圾对象会被回收，byte[]可能被压缩
            GC.Collect();

            //获取byte[]当前在内存中的地址，把它通第一个地址比较
            fixed(Byte* pbytes = bytes)
            {
                Console.WriteLine("the byte[] did{0} move during the GC",
                    (originalMemoryAddress==(IntPtr)pbytes?" not ":null));
            }
        }
    }
}
