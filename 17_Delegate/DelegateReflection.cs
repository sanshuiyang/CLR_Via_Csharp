using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;

namespace _17_Delegate
{
    internal delegate Object TwoInt32s(Int32 n1, Int32 n2);
    internal delegate Object OneString(String s1);

    public static class DelegateReflection
    {

        static String[]  str = { "123", "1464", "324" };
        public static void Main(String[] args)
        {
            if (args.Length < 2) {
                String usage =
                    @"Usage:" +
                    "{0} delType methodName [Arg1][Arg2]" +
                    "{0} where delType must be TwoInt32s or Onstring " +
                    "{0} if is TwoInt32s,methodName must be Add or Subtract " +
                    "{0} if is OneString,must be NumChars or Reverse " +
                    "{0}" +
                    "{0} Examples:" +
                    "{0}   TwoInt32s Add 123 321 " +
                    "{0}   TwoInt32s Add 123 321 " +
                    "{0}   OneString NumChars \"Hello there\"" +
                    "{0}   OneString Reverse \"Hello there\"";
                Console.WriteLine(usage,Environment.NewLine);
                return;
            }

            //将delType实参转换为委托类型
            Type delType = args[0].GetType();
            if (delType == null)
            {
                Console.WriteLine("Invalid delType argument: "+args[0]);
                return;
            }

            Delegate d;
            //创建一个数组，其中只包含要通过委托对象传给方法的参数
            Object[] callbcakArgs = new Object[args.Length - 2];
            try
            {
                //将argl实参转换为方法
                MethodInfo mi =
                    typeof(DelegateReflection).GetTypeInfo().GetDeclaredMethod(args[1]);

                //创建包装了静态方法的委托对象
                var temp= from parameter in mi.GetParameters()
                select parameter.ParameterType;

                var cc = temp.Concat(new[] { mi.ReturnType });

                //根据返回的类型转换系数
                if(cc.ToArray()[0].Name == "Int32")
                {
                    try
                    {
                        //将String类型的参数转换为Int32类型的参数
                        for (Int32 a = 2; a < args.Length; a++)
                        {
                            callbcakArgs[a - 2] = Int32.Parse(args[a]);
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Parameters must be integers");
                        return;
                    }
                }

                if (cc.ToArray()[0].Name == "String")
                {
                    //只复制string参数
                    Array.Copy(args, 2, callbcakArgs, 0, callbcakArgs.Length);
                }
                
                Type ep = Expression.GetDelegateType(cc.ToArray());

                d = mi.CreateDelegate(ep);
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Invalid delType argument: " + args[1]);
                return;
            }
            
            #region
            /*书本上的代码运行不成功，改了
            //比较得到的d是哪种类型委托
            if(d.GetType() == typeof(TwoInt32s))
            {
                try
                {
                    //将String类型的参数转换为Int32类型的参数
                    for(Int32 a = 2; a < args.Length; a++)
                    {
                        callbcakArgs[a - 2] = Int32.Parse(args[a]);
                    }
                }
                catch(FormatException)
                {
                    Console.WriteLine("Parameters must be integers");
                    return;
                }
            }

            if (d.GetType() == typeof(OneString))
            {
                //只复制string参数
                Array.Copy(args, 2, callbcakArgs, 0, callbcakArgs.Length);
            }
            */
            #endregion

            try
            {
                //调用委托并显示结果
                Object result = d.DynamicInvoke(callbcakArgs);//调用当前委托表示的方法
                Console.WriteLine("Result = " + result);
            }
            catch (TargetParameterCountException)
            {
                Console.WriteLine("Invarrect number of parameters specified");
            }
        }

        private static Object Add(Int32 n1,Int32 n2)
        {
            return n1 + n2;
        }

        private static Object Subtract(Int32 n1,Int32 n2)
        {
            return n1 - n2;
        }

        private static Object NumChars(String s1)
        {
            return s1.Length;
        }

        private static Object Reverse(String s1)
        {
            return s1.Reverse().ToString();
        }
    }
}
