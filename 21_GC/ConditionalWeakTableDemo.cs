using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _21_GC
{
    class ConditionalWeakTableDemo
    {
        public static void Main()
        {
            Object o = new Object().GCWatch("My Object created at " + DateTime.Now);
            GC.Collect();//此时看不到GC
            GC.KeepAlive(o);//确定o引用的对象现在还活着
            o = null;

            GC.Collect();
            Console.ReadLine();
        }
    }

    internal static class GCWatcher
    {
        private readonly static ConditionalWeakTable<Object, NotifyWhenGCd<string>> s_cwt =
            new ConditionalWeakTable<object, NotifyWhenGCd<string>>();

        private sealed class NotifyWhenGCd<T>
        {
            private readonly T m_value;

            internal NotifyWhenGCd(T value) { m_value = value; }
            public override string ToString()
            {
                return m_value.ToString();
            }

            ~NotifyWhenGCd() { Console.WriteLine("GC'd: " + m_value); }
        }

        public static T GCWatch<T>(this T @object,string tag)where T : class
        {
            s_cwt.Add(@object, new NotifyWhenGCd<string>(tag));
            return @object;
        }
    }
}
