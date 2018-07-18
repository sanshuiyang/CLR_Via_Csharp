using System;
using System.Reflection;
using System.Collections.Generic;

namespace _23_Reflection
{
    /// <summary>
    /// 使用绑定句柄减少进程的内存消耗
    /// </summary>
    public sealed class UseRuntimeHandle
    {
        private const BindingFlags c_bf = BindingFlags.FlattenHierarchy |
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic;

        public static void Main()
        {
            //显示在任何反射操作之前堆的大小
            Show("Before doing something");

            //为MSCorlib.dll中所有方法构建MethodInfo对象缓存
            List<MethodBase> methondInfos = new List<MethodBase>();
            foreach(Type t in typeof(Object).Assembly.GetExportedTypes())
            {
                //跳过任何泛型类型
                if (t.IsGenericTypeDefinition) continue;

                MethodBase[] mb = t.GetMethods(c_bf);
                methondInfos.AddRange(mb);
            }
            Console.WriteLine("# of methods={0:NO}",methondInfos.Count);
            Show("After building cache of methodInfo objects");

            //为所有MethodInfo对象构建RuntimeMethodHandle缓存
            List<RuntimeMethodHandle> methodHandle = new List<RuntimeMethodHandle>();

            Show("Holding MethodInfo and RuntimeMethodHandle cache");
            GC.KeepAlive(methondInfos);//阻止缓存被过早垃圾回收

            methondInfos = null;
            Show("After freeing MethodInfo objects");

            methondInfos = methodHandle.ConvertAll<MethodBase>(
                rmh => MethodBase.GetMethodFromHandle(rmh));
            Show("Size of heap after re-creating MethodInfo objects");
            GC.KeepAlive(methodHandle);
            GC.KeepAlive(methondInfos);

            methodHandle = null;
            methondInfos = null;
            Show("after freeing methodinfos and runtimethodHandles");
        }

        private static void Show(String s)
        {
            Console.WriteLine("Heap size={0,12:NO} - {1}",
                GC.GetTotalMemory(true).ToString(),s);
        }
    }
}
