﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17_Delegate
{
    internal delegate void Feedback(Int32 value);

    public sealed class UseDelegate
    {
        static void Main_Test(string[] args)
        {
            StaticDelegateDemo();
            InstanceDelegateDemo();
            ChainDelegateDemo1(new UseDelegate());
            ChainDelegateDemo2(new UseDelegate());
        }

        private static void StaticDelegateDemo()
        {
            Console.WriteLine("------------Static Delegate Demo--------");
            Counter(1, 3, null);
            Counter(1, 3, new Feedback(UseDelegate.FeedbackToConsole));
            Counter(1, 3, new Feedback(UseDelegate.FeedbackToMsgBox));
            Console.WriteLine();
        }

        private static void InstanceDelegateDemo()
        {
            Console.WriteLine("----------Instance Delegate Demo-----");
            UseDelegate p = new UseDelegate();
            Counter(1, 3, new Feedback(p.FeedbackToFile));
            Console.WriteLine();
        }

        private static void ChainDelegateDemo1(UseDelegate p)
        {
            Console.WriteLine("----------Chain Delegate Demo 1------");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);
            
            Feedback fbChain = null;
            fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
            Counter(1, 2, fbChain);
            Console.WriteLine();

            fbChain = (Feedback)Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox));
            Counter(1, 2, fbChain);
        }

        private static void ChainDelegateDemo2(UseDelegate p)
        {
            Console.WriteLine("---------Chain Delegate Demo 2------");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);

            Feedback fbChain = null;
            fbChain += fb1;
            fbChain += fb2;
            fbChain += fb3;
            Counter(1, 2, fbChain);

            Console.WriteLine();
            fbChain -= new Feedback(FeedbackToMsgBox);
            Counter(1, 2, fbChain);

        }

        private static void Counter(Int32 from,Int32 to,Feedback fb)
        {
            for(Int32 val = from; val <= to; val++)
            {
                //如果指定了任何回调，就调用它们
                if (fb != null)
                    fb(val);
            }
        }

        private static void FeedbackToConsole(Int32 value)
        {
            Console.WriteLine("Item= "+value);
        }

        private static void FeedbackToMsgBox(Int32 value)
        {
            Console.WriteLine("Item Box= "+value);
        }

        private void FeedbackToFile(Int32 value)
        {
            using (StreamWriter sw = new StreamWriter("Status", true))
            {
                sw.WriteLine("Item= " + value);
            }
        }
    }
}
