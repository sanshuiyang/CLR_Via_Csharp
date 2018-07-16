using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace _18_Attribute
{
    [Flags]
    internal enum Accounts
    {
        Savings=0x0001,
        Checking=0x0002,
        Brokerage=0x0004
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AccountsAttribute:System.Attribute
    {
        private Accounts m_accounts;

        public AccountsAttribute(Accounts accounts)
        {
            m_accounts = accounts;
        }

        public override bool Match(object obj)
        {
            /*如果基类实现了match,而且基类不是Attibute，就取消对下面这行代码的注释
             //if(!base.Match(obj)) return false;
             */

            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            AccountsAttribute other = (AccountsAttribute)obj;

            //判断this是不是other的一个子集
            if ((other.m_accounts & m_accounts) != m_accounts)
                return false;

            return true;//对象配对
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            AccountsAttribute other = (AccountsAttribute)obj;

            if (other.m_accounts != m_accounts)
                return false;

            return true;
        }

        //因为重写了Equals,所以重写GetHashCode
        public override int GetHashCode()
        {
            return (Int32)m_accounts;
        }
    }

    [Accounts(Accounts.Savings)]
    internal sealed class ChildAccount { }

    [Accounts(Accounts.Savings|Accounts.Checking|Accounts.Brokerage)]
    internal sealed class AdultAccount { }

    public sealed class Program
    {
        public static void Main()
        {
            CanWriteCheck(new ChildAccount());
            CanWriteCheck(new AdultAccount());

            CanWriteCheck(new Program());
        }

        private static void CanWriteCheck(Object obj)
        {
            //构造Attribute类型的一个实例，并把它初始化成我们要显示查找的内容
            Attribute checking = new AccountsAttribute(Accounts.Checking);

            //构造用于类型的特性实例
            Attribute validAccounts = Attribute.GetCustomAttribute(
                obj.GetType(), typeof(AccountsAttribute), false);

            if ((validAccounts != null) && checking.Match(validAccounts))
            {
                Console.WriteLine("{0} types can write checks.",obj.GetType());
            }
            else
            {
                Console.WriteLine("{0} types can NOT write checks.", obj.GetType());
            }
        }
    }
}
