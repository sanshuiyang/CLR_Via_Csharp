using System;
using System.Collections.Generic;
using System.Text;

namespace _10_01_05_SystemTuple
{
    /// <summary>
    /// 允许使用数组风格的语法来索引由该类的实例维护的一组二进制位
    /// </summary>
    public sealed class BitArray
    {
        private Byte[] m_byteArray;
        private Int32 m_numBits;

        /// <summary>
        /// 构造器用于分配字节数组，并将所有位设为0
        /// </summary>
        public BitArray(Int32 numBits)
        {
            if (numBits <= 0)
                throw new ArgumentOutOfRangeException("numBits must be > 0");

            //保存位的个数
            m_numBits = numBits;

            //为位数组分配字节
            m_byteArray = new Byte[(numBits + 7) / 8];
        }

        //下面是索引器（有参属性）
        public Boolean this[Int32 bitPos]
        {
            get
            {
                if ((bitPos < 0) || (bitPos >= m_numBits))
                {
                    throw new ArgumentOutOfRangeException("bitpos");
                }
                return (m_byteArray[bitPos/8]&(1<<(bitPos%8)))!=0;
            }
            set
            {
                if ((bitPos < 0) || (bitPos >= m_numBits))
                    throw new ArgumentOutOfRangeException("bitPos", bitPos.ToString());

                if (value)
                {
                    m_byteArray[bitPos / 8] = (Byte)(m_byteArray[bitPos / 8] | (1 << (bitPos % 8)));
                }
                else
                {
                    m_byteArray[bitPos / 8] = (Byte)(m_byteArray[bitPos / 8] & ~(1 << (bitPos % 8)));
                }
            }
        }
    }
}
