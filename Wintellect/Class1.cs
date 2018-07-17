using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.HostSDK;

namespace Wintellect
{
    public class Class1
    {
    }
}

namespace Wintellect.HostSDK
{
    public interface IAddIn
    {
        String DoSomething(Int32 x);
    }
}

namespace Wintellect.HostExe
{
    public sealed class AddIn_A : IAddIn
    {
        public AddIn_A() { }

        public String DoSomething(Int32 x)
        {
            return "A +" + x.ToString();
        }
    }

    public sealed class AddIn_B : IAddIn
    {
        public AddIn_B() { }

        public String DoSomething(Int32 x)
        {
            return "B +" + x.ToString();
        }
    }
}
