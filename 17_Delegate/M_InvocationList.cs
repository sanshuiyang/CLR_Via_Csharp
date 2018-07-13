using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17_Delegate
{
    /// <summary>
    /// 显示调用委托链中的每个委托对象
    /// </summary>

    //定义一个Light组件
    internal sealed class Light
    {
        public String SwitchPosition()
        {
            return "The light is off";
        }
    }

    //定义一个Fan组件
    internal sealed class Fan
    {
        public String Speed()
        {
            throw new InvalidOperationException("The fan broke due to overheating");
        }
    }

    internal sealed class Speaker
    {
        public String Volume()
        {
            return "The volume is loud";
        }
    }


    public sealed class M_InvocationList
    {
        //定义委托来查询一个组件的状态
        private delegate String GetStatus();

        public static void Main()
        {
            //申明空委托链
            GetStatus getStatus = null;

            //构造三个组件，将它们的状态方法添加到委托链中
            getStatus += new GetStatus(new Light().SwitchPosition);
            getStatus += new GetStatus(new Fan().Speed);
            getStatus += new GetStatus(new Speaker().Volume);

            Console.WriteLine(GetComponentStatusReport(getStatus));
        }

        //该方法查询几个组件并返回状态报告
        private static String GetComponentStatusReport(GetStatus status)
        {
            //如果委托链为空，就不进行任何操作
            if (status == null) return null;

            //用下面的变量来创建状态报告
            StringBuilder report = new StringBuilder();

            //获得一个数组，其中每个元素都是链中的委托
            Delegate[] arrayOfDelegates = status.GetInvocationList();

            //遍历数组中每一个委托
            foreach(GetStatus getStatus in arrayOfDelegates)
            {
                try
                {
                    report.AppendFormat("{0}{1}{1}", getStatus(), Environment.NewLine);
                }
                catch(InvalidOperationException e)
                {
                    Object component = getStatus.Target;
                    report.AppendFormat(
                        "Faild to get status from{1}{2}{0}  Error:{3}{0}{0}",
                        Environment.NewLine,
                        ((component == null)) ? "" : component.GetType() + ".",
                        getStatus.Method.Name,
                        e.Message
                        );
                }
            }
            return report.ToString();
        }
    }
}
