using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _26_Thread
{
    /// <summary>
    /// 显示尚未完成的异步操作
    /// </summary>
    public static class TaskLogger
    {
        public enum TaskLogLevel { None,Pending}
        public static TaskLogLevel LogLevel { get; set; }

        public sealed class TaskLogEntry
        {
            public Task M_Task { get; internal set; }
            public String Tag { get; internal set; }
            public DateTime LogTime { get; internal set; }
            public String CallerMemberName { get; internal set; }
            public String CallerFilePath { get; internal set; }
            public Int32 CallerLineNumber { get; internal set; }
            public override String ToString()
            {
                return String.Format("LogTime={0},Tag={1},Member={2},File={3}{4}",
                    LogTime, Tag ?? "(None)", CallerMemberName, CallerFilePath, CallerLineNumber);
            }
        }

        private static readonly Dictionary<Task, TaskLogEntry> s_log = new Dictionary<Task, TaskLogEntry>();
        public static IEnumerable<TaskLogEntry> GetLogEntry() { return s_log.Values; }

        public static Task<TResult> Log<TResult>(this Task<TResult> task,String tag =null,
            [CallerMemberName]String callerMemberName =null,
            [CallerFilePath]String callerFilePath=null,
            [CallerLineNumber]Int32 callerLineNumber = -1)
        {
            return (Task<TResult>)Log((Task)task, tag, callerMemberName, callerFilePath, callerLineNumber);
        }

        public static Task Log(this Task task, String tag = null,
            [CallerMemberName]String callerMemberName = null,
            [CallerFilePath]String callerFilePath = null,
            [CallerLineNumber]Int32 callerLineNumber = -1)
        {
            if (LogLevel == TaskLogLevel.None) return task;
            var logEntry = new TaskLogEntry
            {
                M_Task = task,
                LogTime = DateTime.Now,
                Tag = tag,
                CallerMemberName = callerMemberName,
                CallerFilePath = callerFilePath,
                CallerLineNumber = callerLineNumber
            };
            s_log[task] = logEntry;
            task.ContinueWith(t => { s_log.Remove(t); },
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }
    }
}
