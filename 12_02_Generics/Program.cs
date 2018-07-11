using System;
using System.Collections.Generic;
using System.Text;

namespace _12_02_Generics
{
    //部分指定的开放类型
    internal sealed class DictionaryStringkey<TValue> : Dictionary<string, TValue>
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            Object o = null;

            //Dictionary<,>是开放类型，有2个类型参数
            Type t = typeof(Dictionary<,>);

            o = CreateInstance(t);
            t = typeof(DictionaryStringkey<>);
            o = CreateInstance(t);
            t = typeof(DictionaryStringkey<Guid>);
            o = CreateInstance(t);
        }

        private static Object CreateInstance(Type t)
        {
            Object o = null;
            try
            {
                o = Activator.CreateInstance(t);
                Console.WriteLine("已创建{0}的实例", t.ToString());
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            return o;
        }
    }
}
