using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;

namespace _14_String
{
    class M_SecureString
    {
        public static void Main()
        {
            using(SecureString ss =new SecureString()) {
                Console.WriteLine("Please enter password");
                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter) break;

                    //将密码字符附加到SecureString中
                    ss.AppendChar(cki.KeyChar);
                    Console.Write("*");
                }
                Console.WriteLine();
                DisplaySecureString(ss);
            }
        }

        private unsafe static void DisplaySecureString(SecureString ss)
        {
            Char* pc = null;
            try
            {
                //将SecureString解密到一个非托管内存缓冲区中
                pc = (char*)Marshal.SecureStringToCoTaskMemUnicode(ss);

                //访问包含已解密的SecureString的非托管内存缓冲区
                for(Int32 index = 0; pc[index] != 0; index++)
                {
                    Console.Write(pc[index]);
                }
            }
            finally
            {
                //确定清零并释放包含已解密SecureString字符的非托管内存缓冲区
                if (pc != null)
                    Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
            }
        }

    }
}
