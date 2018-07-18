using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _26_Thread
{
    public static class Asychronous
    {
        public static void Main_Test()
        {
            Console.WriteLine("Main thread:queuing an asynchronous operation");
            ThreadPool.QueueUserWorkItem(ComputeBoundOp,5);
            Console.WriteLine("Do other sth");
            Thread.Sleep(10000);
            Console.WriteLine("Hit <Enter> to end program");
            Console.ReadLine();
        }

        private static void ComputeBoundOp(Object state)
        {
            Console.WriteLine("In ComputeBoundOp: state={0}",state);
            Thread.Sleep(1000);
        }
    }
}
