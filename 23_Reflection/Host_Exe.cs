using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wintellect.HostSDK;

namespace _23_Reflection
{
    public static class Host_Exe
    {
        //TODO:未完成，没达到目标
        public static void Main()
        {
            //查找宿主EXE文件所在目录
            String addInDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //假定加载项程序集和宿主文件exe文件在同一个目录
            var addInAssemblies = Directory.EnumerateFiles(addInDir, ".dll");

            //创建可由宿主使用的所有加载type的一个集合
            var addInTypes =
                from file in addInAssemblies
                let assembly = Assembly.Load(file)
                from t in assembly.ExportedTypes //公开导出的类型
                                                 //如果类型实现了IAddIn接口，该类型就可由宿主使用
                where t.IsClass && typeof(IAddIn).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo())
                select t;
            //初始化完成：宿主已发现所有可用加载项

            //下面示范宿主如何构造加载项对象并使用它们
            foreach(Type t in addInTypes)
            {
                IAddIn ai = (IAddIn)Activator.CreateInstance(t);
                Console.WriteLine(ai.DoSomething(5));
            }
        }
    }
}
