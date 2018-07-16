using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_Attribute
{
    /// <summary>
    /// 定义自己的特性类
    /// </summary>
    //告诉编译器这个特性的合法范围  
    //AttributeUsage两个属性：AllowMultiple：多次应用于同一目标 Inherted：应用于基类时，是都应用于派生类和重写的方法
    [AttributeUsage(AttributeTargets.Enum,Inherited =false)]
    //使用Attribute后缀是为保持与标准的相容性，不是必须的
    class FlagsAttribute:System.Attribute
    {
        public FlagsAttribute() { }
    }
}
