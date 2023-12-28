using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.Customs.BL.Messaging.Customs.PerformanceLogger
{

    public class PerformanceM
    {
        [ThreadStatic]
        private static PerformanceM _Instance;
        public static PerformanceM LastInstance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PerformanceM();
                    _Instance.PID = Process.GetCurrentProcess().Id.ToString();
                    _Instance.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                    _Instance.ServerName = Environment.MachineName;
                }
                return _Instance;
            }


        }

        public static void SleepMSAfterEachQueuePeek()
        {
            string QueuePeekSleepMS = ConfigurationManager.AppSettings.Get("SleepInMSAfterEachQueuePeek");
            if (!string.IsNullOrWhiteSpace(QueuePeekSleepMS))
            {
                int iQueuePeekSleepMS = 0;
                if (int.TryParse(QueuePeekSleepMS, out iQueuePeekSleepMS))
                {
                    Thread.Sleep(iQueuePeekSleepMS);
                }
            }
        }
        public static void EnqueueLastInstance()
        {

            string PerformanceLoggerCSVPath = ConfigurationManager.AppSettings.Get("PerformanceLoggerCSVPath");

            if (!string.IsNullOrWhiteSpace(PerformanceLoggerCSVPath) && System .IO.Directory.Exists(PerformanceLoggerCSVPath))
            {
                if (_Instance != null && _Instance.QueueReceiveDate > DateTime.MinValue)
                {
                    CheckDBTime();
                    PerformanceQueue.Instance.Add(_Instance);
                }
            }

            _Instance = null;
        }
        private static DateTime _LastCheckDBTime ;//all threads
        private static void CheckDBTime()
        {
            if (DateTime.Now.Subtract(_LastCheckDBTime) < TimeSpan.FromSeconds(60))
            {
                return;
            }

            try
            {
                using (var scope = TransactionFactory.GetNewTransaction())
                {


                    var sw = Stopwatch.StartNew();
                    var myDB = GlobalContext.GetContext((int)TimeSpan.FromMinutes(2).TotalSeconds, true) as DbContextBase;
                    var myDualRepository = new DualRepository(myDB);
                    var dt = DateTime.Now;
                    _Instance.DBResponseTime = sw.ElapsedMilliseconds.ToString();
                }
            }
            catch (System.Exception e)
            {
                ///Logger.LogMe(e.ToString(), true, "DbError");


            }
            finally
            {
                _LastCheckDBTime = DateTime.Now;
            }

        }
        PerformanceM()
        {

        }
        public string ServerName { get; set; }
        public string PID { get; set; }
        public string ThreadId { get; set; }
        public string QueueDefinitionCode
        { get; set; }
        public string InterfaceTypeCode { get; set; }
        //public DateTime RequestCreateDate { get; set; }
        public DateTime RequestStartDate { get; set; }
        public DateTime RequestEndDate { get; set; }

        public double RequestDiff { get  { return this.RequestEndDate.Subtract(this.RequestStartDate).TotalMilliseconds; } }
        

        //public DateTime QueueCreateDate { get; set; }
        public DateTime QueueStartDate { get; set; }
        public DateTime QueueEndDate { get; set; }

        public double QueueDiff { get { return this.QueueEndDate.Subtract(this.QueueStartDate).TotalMilliseconds; } }
        public string ServerCPU { get; set; }
        public string RequestSheetID { get; set; }
        public string DBResponseTime { get; set; }
        public DateTime QueueReceiveDate { get; set; }
        public bool QueueSuccessComplete { get; set; }
    }
}
