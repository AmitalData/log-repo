
using AmitalCustomsWindowsService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.BL
{
    public abstract class WorkerBase
    {


        private double _interval;
        private int _id;
        
        public WorkerBase(double interval, int id)
        {
            _interval =interval;
            _id = id;
        }
        public abstract void DoIt();
        public void ExecuteTask()
        {
            DateTime lastRunTime = DateTime.UtcNow;

            while (ServiceStarted)
            {
                // check the current time against the last run plus interval
                if (((TimeSpan)
                   (DateTime.UtcNow.Subtract(lastRunTime))).TotalSeconds >= _interval)
                {
                    // if time to do something, do so
                    // exception handling omitted here for simplicity
                    //Logger.LogMe("Multithreaded Service working; id = " + this._id.ToString(), false);
                    try
                    {
                        DoIt();
                    }
                    catch (Exception e) 
                    {

                        Logger.LogMe(this.GetType().FullName + ":" + e.ToString(), true);
                    }    
                    

                    // set new run time
                    lastRunTime = DateTime.UtcNow;
                }

                // yield
                if (ServiceStarted)
                {
                    Thread.Sleep(new TimeSpan(0, 0, 15));
                }
            }

            Thread.CurrentThread.Abort();
        }

        public bool ServiceStarted { get; set; }


    }
}
