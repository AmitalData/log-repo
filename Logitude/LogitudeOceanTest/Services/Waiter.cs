using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.OceanTest.Services
{
    public static class Waiter
    {
        public static bool RunAndWait(int TryEvreySecound, int TineLifeInSecound, Func<bool> Do)
        {
            var done = false;
            var conter = 0;
            while (!done || conter < TineLifeInSecound)
            {
                done = Do.Invoke();
                Thread.Sleep(TryEvreySecound * 1000);
                conter++;
            }
            return done;
        }
    }
}
