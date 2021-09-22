using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Services
{
    public static class Waiter
    {
        public static bool RunAndWait(int TryEvreySecound, int TineLifeInSecound, Func<bool> Do)
        {
            var isDone = false;
            var counter = 0;
            while (!isDone && counter < TineLifeInSecound)
            {
                isDone = Do.Invoke();
                Thread.Sleep(TryEvreySecound * 1000);
                counter++;
            }
            return isDone;
        }
    }
}
