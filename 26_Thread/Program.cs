using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _26_Thread
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //ThreadStart.Main_Test();
            //Asychronous.Main_Test();
            //M_ExecutionContext.Main_Test();
            //await Go();
        }



        public static async Task Go()
        {

#if DEBUG
            TaskLogger.LogLevel = TaskLogger.TaskLogLevel.Pending;
#endif
            var tasks = new List<Task>
            {
                Task.Delay(2000).Log("2s op"),
                Task.Delay(5000).Log("5s op"),
                Task.Delay(6000).Log("6s op"),
            };

            //try
            //{
            await Task.WhenAll(tasks);
            //}

            //foreach(var op in TaskLogger.GetLogEntry().OrderBy<tle => tle.LogTime))

        }
    }
}
