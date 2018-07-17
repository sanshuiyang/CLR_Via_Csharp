using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

namespace _21_GC
{
    public abstract class SafeHandle:CriticalFinalizerObject,IDisposable
    {
        //本机资源的句柄
        protected IntPtr handle;

        protected SafeHandle(IntPtr invalidHandleValue,Boolean ownsHandle)
        {
            this.handle = invalidHandleValue;
            //如果ownsHandle为true，那么这个从SafeHandle派生的对象就被回收时，
            //本机资源会被关闭
        }

        protected void SetHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        //可调用Dispose显示释放资源
        public void Dispose() { Dispose(true); }

        //默认的Dispose实现，不要重写该方法
        protected virtual void Dispose(Boolean disposint)
        {
            throw new NotImplementedException();
        }
    }
}
