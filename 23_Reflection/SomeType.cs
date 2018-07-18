using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using System.Linq;

namespace _23_Reflection
{
    /// <summary>
    /// 演示了调用成员类型
    /// </summary>
    internal sealed class SomeType
    {
        private Int32 m_someField;
        public SomeType(ref Int32 x) { x *= 2; }
        public override string ToString()
        {
            return m_someField.ToString();
        }
        public Int32 SomeProp
        {
            get { return m_someField; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                m_someField = value;
            }
        }
        public event EventHandler SomeEvent;
        private void NoCompilerWarnings() { SomeEvent.ToString(); }
    }

    public static class InvokeSomeType
    {
        public static void Main_test()
        {
            Type t = typeof(SomeType);
            BindToMenberThenInvokeTheMemner(t);
            Console.WriteLine();

            BindToMenberCreateDelegateToMemberThenInvokeTheMenber(t);
            Console.WriteLine();

            UseDynamicToBindAndInvokeTheMember(t);
            Console.WriteLine();
        }

        //绑定到成员并调用它
        private static void BindToMenberThenInvokeTheMemner(Type t)
        {
            Console.WriteLine("BindToMenberThenInvokeTheMemner");

            //"System.Int32&" 表明参数是传引用的 巴克斯-诺尔范式（BNF）语法的一部分
            Type ctorArgument = Type.GetType("System.Int32&");
            ConstructorInfo ctor = t.GetTypeInfo().DeclaredConstructors.First(
                c => c.GetParameters()[0].ParameterType == ctorArgument);
            Object[] args = new Object[] { 12 };

            Console.WriteLine("x before constructor called: "+args[0]);
            Object obj = ctor.Invoke(args);
            Console.WriteLine("Type: "+obj.GetType().ToString());
            Console.WriteLine("x after constructor called: " + args[0]);

            FieldInfo fi = obj.GetType().GetTypeInfo().GetDeclaredField("m_someField");
            fi.SetValue(obj, 33);
            Console.WriteLine("SomeFeild: "+fi.GetValue(obj));

            MethodInfo mi = obj.GetType().GetTypeInfo().GetDeclaredMethod("ToString");
            String s =(String)mi.Invoke(obj, null);
            Console.WriteLine("ToString: "+s);

            PropertyInfo pi = obj.GetType().GetTypeInfo().GetDeclaredProperty("SomeProp");
            try
            {
                pi.SetValue(obj, 0, null);
            }
            catch(TargetInvocationException e)
            {
                Console.WriteLine("Error");
            }
            pi.SetValue(obj, 2, null);
            Console.WriteLine(pi.GetValue(obj, null));

            //为事件添加和删除委托
            EventInfo ei = obj.GetType().GetTypeInfo().GetDeclaredEvent("SomeEvent");
            EventHandler eh = new EventHandler(EventCallBack);
            ei.RemoveEventHandler(obj, eh);
        }

        private static void EventCallBack(Object sender,EventArgs e) { }

        //绑定到一个对象或成员，然后创建一个委托来引用该对象或成员
        //通过委托来调用的速度很快，如果需要在相同的对象上多次调用相同的成员，性能比上一个好
        private static void BindToMenberCreateDelegateToMemberThenInvokeTheMenber(Type t)
        {
            Console.WriteLine("BindToMenberCreateDelegateToMemberThenInvokeTheMenber");

            //构造实例（不能创建对构造器的委托）
            Object[] args = new Object[] { 12 };
            Console.WriteLine("x before constructor called: " + args[0]);
            Object obj = Activator.CreateInstance(t,args);
            Console.WriteLine("Type: " + obj.GetType().ToString());
            Console.WriteLine("x after constructor called: " + args[0]);

            //没有创建字段的委托，输出0

            MethodInfo mi = obj.GetType().GetTypeInfo().GetDeclaredMethod("ToString");
            var toString = mi.CreateDelegate<Func<String>>(obj);
            String s = toString();
            Console.WriteLine("ToString: "+ s);

            PropertyInfo pi = obj.GetType().GetTypeInfo().GetDeclaredProperty("SomeProp");
            var setSomeProp = pi.SetMethod.CreateDelegate<Action<Int32>>(obj);
            try
            {
                setSomeProp(0);
            }
            catch
            {
                Console.WriteLine("Set Error");
            }
            setSomeProp(2);
            var getSomeProp = pi.GetMethod.CreateDelegate<Func<Int32>>(obj);
            Console.WriteLine("SomeProp: "+getSomeProp());

            EventInfo ei = obj.GetType().GetTypeInfo().GetDeclaredEvent("SomeEvent");
            var addSomeEvent = ei.AddMethod.CreateDelegate<Action<EventHandler>>(obj);
            addSomeEvent(EventCallBack);
            var removeSomeEvent = ei.RemoveMethod.CreateDelegate<Action<EventHandler>>(obj);
            removeSomeEvent(EventCallBack);
        }

        //利用dynamic基元类型简化访问语法，在相同类型的不同对象上调用相同成员时，针对每个类型。
        //绑定都只会发生一次，可以缓存起来，以后调用的速度会非常快，也可以调用不同类型的成员
        private static void UseDynamicToBindAndInvokeTheMember(Type t)
        {
            Console.WriteLine("UseDynamicToBindAndInvokeTheMember");

            Object[] args = new Object[] { 12 };
            Console.WriteLine("x before constructor called: " + args[0]);
            dynamic obj = Activator.CreateInstance(t, args);
            Console.WriteLine("Type: " + obj.GetType().ToString());
            Console.WriteLine("x after constructor called: " + args[0]);

            //读写字段
            try
            {
                obj.m_someFeild = 5;
                Int32 v = (Int32)obj.m_someFeild;
                Console.WriteLine("SomeFeild: " + v);
            }
            catch(RuntimeBinderException e)
            {
                Console.WriteLine("failed to access field: "+e.Message);
            }

            //调用方法
            String s = (String)obj.ToString();
            Console.WriteLine("ToString: " + s);

            //读写属性
            obj.SomeProp = 2;
            Int32 val = (Int32)obj.SomeProp;
            Console.WriteLine("SomeProp: " + val);

            //事件增删委托
            obj.SomeEvent += new EventHandler(EventCallBack);
            obj.SomeEvent -= new EventHandler(EventCallBack);
        }
    }

    internal static class ReflectionExtensions
    {
        //拓展方法简化委托语法
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo mi,Object target = null)
        {
            return (TDelegate)(Object)mi.CreateDelegate(typeof(TDelegate), target);
        }
    }
}
