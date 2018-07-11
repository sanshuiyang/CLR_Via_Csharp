using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace _11_01_Event
{
    class MailManager
    {
        //定义事件成员
        public event EventHandler<NewMailEventArgs> NewMail;

        //定义负责引发事件的方法来通知已登记的对象
        //如果类是密封的，该方法要声明为私有和非虚
        protected virtual void OnNewMail(NewMailEventArgs e)
        {
            EventHandler<NewMailEventArgs> temp = Volatile.Read(ref NewMail);
            if (temp != null) temp(this, e);
        }
    }
}
