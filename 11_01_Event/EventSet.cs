using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _11_01_Event
{
    //这个类目的是在使用EventSet时，提供多一点的类型安全性和代码可维护性
    public sealed class EventKey { }

    public sealed class EventSet
    {
        //私有字典用于维护EventKey -> Delegate映射
        private readonly Dictionary<EventKey, Delegate> m_events =
            new Dictionary<EventKey, Delegate>();

        public void Add(EventKey eventKey,Delegate handler)
        {
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handler);
            Monitor.Exit(m_events);
        }
        
    }
}
