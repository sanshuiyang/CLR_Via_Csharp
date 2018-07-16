using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _19_Nullable
{
    [Serializable,StructLayout(LayoutKind.Sequential)]
    public struct M_Nullable<T> where T:struct
    {
        private Boolean hasValue;//假定null
        internal T value;//假定所有位都为零

        public M_Nullable(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public Boolean HasValue { get { return hasValue; } }

        public T Value
        {
            get
            {
                if (!hasValue)
                {
                    throw new InvalidOperationException(
                        "Nullable object must have a value");
                }
                return value;
            }
        }

        public T GetValueOrDefault() { return value; }

        public T GetValueOrDefault(T defaultValue)
        {
            if (!hasValue) return defaultValue;
            return value;
        }

        public override bool Equals(object obj)
        {
            if (!hasValue) return (obj == null);
            if (obj == null) return false;
            return value.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (!hasValue) return 0;
            return value.GetHashCode();
        }

        public override string ToString()
        {
            if (!hasValue) return "";
            return value.ToString();
        }

        public static implicit operator M_Nullable<T>(T? value)
        {
            //TODO:需要修改的代码
            // return new M_Nullable<T>(value);
            return null;
        }

        public static explicit operator T?(M_Nullable<T> value)
        {
            return value.Value;
        }
    }

    public sealed class Program
    {
        public static void Main()
        {
            M_Nullable<Int32> x = 5;
            M_Nullable<Int32> y = null;
            Console.WriteLine("x:hasValue={0},value={1}",x.HasValue,x.value);
            Console.WriteLine("y:hasValue={0},value={1}", y.HasValue, y.GetValueOrDefault());
        }
    }
}
