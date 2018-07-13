using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14_String
{
    class Program
    {
        static void Main_Test01(string[] args)
        {
            Char c;
            Int32 n;

            c = (Char)65;
            Console.WriteLine(c);

            n = (Int32)c;
            Console.WriteLine(n);

            c = unchecked((Char)(65536 + 65));
            Console.WriteLine(c);

            c = Convert.ToChar(65);
            Console.WriteLine(c);

            try
            {
                c = Convert.ToChar(70000);
                Console.WriteLine(c);
            }
            catch {
                Console.WriteLine("Cant convert 70000 to char");
            }

            c = ((IConvertible)65).ToChar(null);
            Console.WriteLine(c);
        }
    }
}
