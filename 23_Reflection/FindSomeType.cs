using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace _23_Reflection
{
    /// <summary>
    /// 发现类型成员
    /// </summary>
    public static class FindSomeType
    {
        public static void Main_test()
        {
            Assembly[] assmbies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly a in assmbies)
            {
                Show(0, "Assembly:{0}", a);
                foreach(Type t in a.ExportedTypes)
                {
                    Show(1, "Type:{0}", t);
                    foreach(MemberInfo mi in t.GetTypeInfo().DeclaredMembers)
                    {
                        String typeName = String.Empty;
                        if (mi is Type) typeName = "Type";
                        if (mi is FieldInfo) typeName = "MethodInfo";
                        if (mi is ConstructorInfo) typeName = "ConstructorInfo";
                        if (mi is PropertyInfo) typeName = "PropertyINfo";
                        if (mi is EventInfo) typeName = "EventInfo";
                        Show(2, "{0}:{1}", typeName, mi);
                    }
                }
            }
        }

        public static void Show(Int32 indent,String format,params Object[] args)
        {
            Console.WriteLine(new String(' ',3*indent)+format,args);
        }
    }
}
