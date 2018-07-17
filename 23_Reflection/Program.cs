using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _23_Reflection
{
    //展示程序集中定义的类型
    class Program
    {
        static void Main_test(string[] args)
        {
            String dataAssembly = "System.data,version=4.0.0.0," +
                "culture=neutral,PublicKeyToKen=b77a5c561934e089";
            LaodAssemblyAndShowPublicTypes(dataAssembly);
        }

        private static void LaodAssemblyAndShowPublicTypes(String assmID)
        {
            //显示地将程序集加载到这个AppDomain中
            Assembly a = Assembly.Load(assmID);

            foreach (Type t in a.ExportedTypes)
            {
                Console.WriteLine(t.FullName);
            }
        }
    }
}
