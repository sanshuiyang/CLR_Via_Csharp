using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _26_Thread
{
    public static class ThreadStart
    {
        public static void Main_Test()
        {
            Console.WriteLine("Main thread: starting a dedicated thread");
            Thread dedicatedThread = new Thread(ComputeBoundOp);
            dedicatedThread.Start(5);

            Console.WriteLine("Main thread :Doing other work here");
            Thread.Sleep(10000);

            dedicatedThread.Join();//Join方法造成调用线程阻塞（暂停）当前执行的任何代码，
            //直到dedicateThread所代表的那个线程销毁或终止
            Console.WriteLine("Hit <Enter> to end this program");
            Console.ReadLine();
        }

        private static void ComputeBoundOp(Object state)
        {
            //这个方法由一个专用线程执行
            Console.WriteLine("In ComputeBoundOp: state={0}",state);
            Thread.Sleep(1000);
        }
    }
}
