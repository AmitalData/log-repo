using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL
{
    public class GatewayServiceState
    {
   
        public delegate void AddSuccessDelegate();
        public delegate void AddErrorDelegate(string Operation, string Error);

        public static string BaseAddress = "";
        public static DateTime ServiceStartAt = DateTime.Now;
        public static DateTime CurrentDate = DateTime.Now.Date;
        static DateTime LastSuccessResponseAt = DateTime.MinValue;
        static DateTime LastRequestAt = DateTime.MinValue;
        static string LastError = "";
        static long Requested = 0;
        static long RequestedToday = 0;
        static long FailedRequestedToday = 0;
        static long SuccessRequestedToday = 0;
        public static void AddRequest()
        {
            Requested++;
            RequestedToday++;
        }
        public static void AddSuccess()
        {
            LastSuccessResponseAt = DateTime.Now;
            SuccessRequestedToday++;
        }
        public static void AddError(string Operation, string Error)
        {
            FailedRequestedToday++;

            LastError = string.Format(
         @"Operation={0}/t at ={1}/t Mess:{2}", Operation, DateTime.Now, Error);
        }
        internal static string GetStateOld()
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                return string.Format(
            @"Service Start At:{0} 
Requested  Since Start:{6}  
Requested Today:{2}
Success Requested  Today:{5}
Failed Requested  Today:{3}
Last Success Response At:{1}
LastError:{4}",
            ServiceStartAt, LastSuccessResponseAt, RequestedToday,
            FailedRequestedToday, LastError, SuccessRequestedToday, Requested);
            }
            catch (Exception e)
            {

                return "Error !!! " + e.ToString();
                //throw;
            }
        }

        internal static string GetState()
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                long memAll = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
                string bad = "bgcolor=red";
                string Good = "bgcolor=Green";
                return string.Format(
            @"<table id=""State_{0}"" >
<caption >Daily Customs Export Requests Report </caption>
<tr><td>MachineName</td><td>{0}</td></tr>
<tr><td>Service Host Uri</td><td>{9}</td></tr>
<tr><td>Service Start At</td><td>{1}</td></tr>
<tr><td>Last Success Response At</td><td>{2}</td></tr>

<tr {8} ><td>Total Success</td><td>{3}</td></tr>
<tr {6} ><td>Total Failed</td><td>{4}</td></tr>
<tr {7} ><td >Last Error</td><td>{5}</td></tr>
</table>
",
             Environment.MachineName + " " + memAll + "MB", ServiceStartAt,
            LastSuccessResponseAt,
            SuccessRequestedToday,
            FailedRequestedToday,
            LastError,
            FailedRequestedToday > 0 ? bad : "",
            LastError != "" ? bad : "",
            FailedRequestedToday == 0 ? Good : "", BaseAddress);
            }
            catch (Exception e)
            {

                return "Error !!! " + e.ToString();
                //throw;
            }
        }
        internal static void RaiseAnotherDay()
        {
            try
            {
                RequestedToday = 0;
                FailedRequestedToday = 0;
                SuccessRequestedToday = 0;
                GatewayServiceState.CurrentDate = DateTime.Now.Date;
            }
            catch (Exception)
            {

                //throw;
            }
        }
#if false
        

        internal static void ClearSandBoxDir()
        {
            try
            {
                string Source = SandBoxDir();
                DirectoryInfo DirectoryInfo1 = new DirectoryInfo(Source);
                if (!DirectoryInfo1.Exists)
                    DirectoryInfo1.Create();
                else
                {
                    DirectoryInfo1.Delete(true);
                    DirectoryInfo1.Create();
                }
            }
            catch (Exception e)
            {
                Logger.LogMe(e.ToString(), true);
            }
        }

        internal static string SandBoxDir()
        {
            return Logger.WorkingDir + @"..\SandBox\";
        }
#endif
        #region "PlugeIn"



        #endregion


    }
}