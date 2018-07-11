using System;
using System.Collections.Generic;
using System.Text;

namespace _10_01_05_SystemTuple
{
    class Program
    {
        static void Main(string[] args)
        {
            BitArray ba = new BitArray(14);

            for(Int32 x = 0; x < 14; x++)
            {
                ba[x] = (x % 2 == 0);
            }

            for(Int32 x = 0; x < 14; x++) {
                Console.WriteLine("Bit " + x + "is " + (ba[x] ? "On" : "Off"));
            }
        }
    }

    [Serializable]
    public class Tuple<T1>
    {
        private T1 m_Item1;
        public Tuple(T1 iteml) { m_Item1 = iteml; }
        public T1 Iteml { get { return m_Item1; } }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        private T1 m_Item1; private T2 m_Item2; private T3 m_Item3; private T4 m_Item4; private T5 m_Item5;
        private T6 m_Item6; private T7 m_Item7; private TRest m_Rest;
        public Tuple(T1 item1,T2 item2,T3 item3,T4 item4,T5 item5,T6 item6,T7 item7,
            TRest rest)
        {
            m_Item1 = item1;m_Item2 = item2;m_Item3 = item3;m_Item4 = item4;
            m_Item5 = item5;m_Item6 = item6;m_Item7 = item7;m_Rest = rest;
        }

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }
        public T5 Item5 { get { return m_Item5; } }
        public T6 Item6 { get { return m_Item6; } }
        public T7 Item7 { get { return m_Item7; } }
        public TRest Rest { get { return m_Rest; } }
    }
}
