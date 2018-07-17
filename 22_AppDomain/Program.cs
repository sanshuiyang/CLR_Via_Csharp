using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _22_AppDomain
{
    class Program
    {
        static void Main(string[] args)
        {
            Marshalling();
        }

        private static void Marshalling()
        {
            //获取AppDomain引用（“调用线程”当前正在该AppDomain中执行）
            AppDomain adCallingThreadDomain = Thread.GetDomain();

            String callingDomainName = adCallingThreadDomain.FriendlyName;
            Console.WriteLine("Default AppDomain's friendly name = {0}",callingDomainName);

            String exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("Main Assembly = {0}",exeAssembly);

            AppDomain ad2 = null;

            Console.WriteLine("{0}Demo #1，",Environment.NewLine);

            ad2 = AppDomain.CreateDomain("AD #2", null, null);
            MarshlByRefType mbrt = null;

            mbrt = (MarshlByRefType)ad2.CreateInstanceAndUnwrap(exeAssembly, "_22_AppDomain.MarshlByRefType");

            Console.WriteLine("Type={0}",mbrt.GetType());

            Console.WriteLine("Is proxy={0}",RemotingServices.IsTransparentProxy(mbrt));

            mbrt.SomeMethod();

            AppDomain.Unload(ad2);

            try
            {
                mbrt.SomeMethod();
                Console.WriteLine("OK!!!");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Failed call");
            }

            Console.WriteLine("{0}Demo #2，", Environment.NewLine);

            ad2 = AppDomain.CreateDomain("AD #2", null, null);
            mbrt = (MarshlByRefType)ad2.CreateInstanceAndUnwrap(exeAssembly, "_22_AppDomain.MarshlByRefType");

            MarshalByValType mbvt = mbrt.MethodWithReturn();

            Console.WriteLine("Is proxy={0}",RemotingServices.IsTransparentProxy(mbvt));

            Console.WriteLine("Return object created "+mbvt.ToString());

            AppDomain.Unload(ad2);

            try
            {
                Console.WriteLine("Return object created " + mbvt.ToString());
                Console.WriteLine("OK!!!");
            }
            catch(AppDomainUnloadedException)
            {
                Console.WriteLine("Failed");
            }
            
            Console.WriteLine("{0}Demo #3，", Environment.NewLine);

            ad2 = AppDomain.CreateDomain("AD #2", null, null);
            mbrt = (MarshlByRefType)ad2.CreateInstanceAndUnwrap(exeAssembly, "_22_AppDomain.MarshlByRefType");
            NonMarshalableType nmt = mbrt.MethonArgAndReturn(callingDomainName); //执行报错
        }
    }

    //该类的实例可跨越AppDomain的边界“按引用封送”
    public sealed class MarshlByRefType : MarshalByRefObject
    {
        public MarshlByRefType()
        {
            Console.WriteLine("{0} ctor running in {1}",this.GetType().ToString(),
                Thread.GetDomain().FriendlyName);
        }

        public void SomeMethod()
        {
            Console.WriteLine("Executing in"+ Thread.GetDomain().FriendlyName);
        }

        public MarshalByValType MethodWithReturn()
        {
            Console.WriteLine("Executing in "+Thread.GetDomain().FriendlyName);
            MarshalByValType t = new MarshalByValType();
            return t;
        }

        public NonMarshalableType MethonArgAndReturn(String callingDomainName)
        {
            Console.WriteLine("Calling from '{0}' to '{1}'.",
                callingDomainName,Thread.GetDomain().FriendlyName);
            NonMarshalableType t = new NonMarshalableType();
            return t;
        }
    }

    //该类的实例可跨越AppDomain的边界"按值封送"
    [Serializable]
    public sealed class MarshalByValType : Object
    {
        private DateTime m_creationTime = DateTime.Now;//dateTime可序列化

        public MarshalByValType()
        {
            Console.WriteLine("{0} ctor running in {1},Created on {2:D}",
                this.GetType().ToString(),Thread.GetDomain().FriendlyName,m_creationTime);
        }

        public override string ToString()
        {
            return m_creationTime.ToLongDateString();
        }
    }

    //该类的实例不能跨AppDomain边界进行封送
    //[Serializable]
    public sealed class NonMarshalableType : Object
    {
        public NonMarshalableType()
        {
            Console.WriteLine("Executing in "+Thread.GetDomain().FriendlyName);
        }
    }
}
