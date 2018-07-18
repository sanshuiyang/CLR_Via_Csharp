using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _26_Thread
{
    public static class M_ExecutionContext
    {
        public static void Main_Test()
        {
            //将一些数据方法放到Main线程的逻辑调用上下文中
            CallContext.LogicalSetData("Name", "Jeffrey");

            //初始化要有一个线程池线程做的一些工作
            //线程池线程能访问逻辑调用上下文数据
            ThreadPool.QueueUserWorkItem(
                state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));

            //阻止main线程的执行上下文的流动
            ExecutionContext.SuppressFlow();

            //线程池不能访问逻辑调用上下文数据
            ThreadPool.QueueUserWorkItem(
                state => Console.WriteLine("Name = {0}", CallContext.LogicalGetData("Name")));

            //恢复main线程的执行上下文的流动
            //以免将来使用更多的线程池线程
            ExecutionContext.RestoreFlow();

            Console.ReadKey(true);
        }
    }
}
