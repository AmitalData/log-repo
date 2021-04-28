using AmitalCustomsWindowsService.Utils;
using CustomsWorkerRole.Test;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.BL
{
    class ServiceState
    {


        public delegate void AddSuccessDelegate();
        public delegate void AddErrorDelegate(string Operation, string Error);

        public static string BaseAddress = "";
        public static DateTime ServiceStartAt = DateTime.Now;
        public static DateTime CurrentDate = DateTime.Now.Date;
        static DateTime LastSuccessResponseAt = DateTime.MinValue;
        static DateTime LastRequestAt =  DateTime.MinValue;
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
        static string FileVer()
        {
            String str = "";
            var myVer = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            str = "version " + myVer.FileMajorPart.ToString() + "." + myVer.FileMinorPart.ToString();

            String strDate;
            String fName;

            strDate = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            str += " <font size=2>(" + strDate + ")</font>";
            return str;
        }
        static DateTime _LastGetServiceBusStateAt;
        static string _AllQ;


        internal static string GetState()
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {


                try
                {
                    if (DateTime.Now.Subtract(_LastGetServiceBusStateAt) > TimeSpan.FromMinutes(5))
                    {
                        _LastGetServiceBusStateAt = DateTime.Now;
                        _AllQ = CustomsWorkerRole.Utils.ServiceBusUtil.ShowAll();
                        _AllQ = "Retrieve ServiceBus at " + DateTime.Now.ToString() + " :" + _AllQ;
                        Debug.WriteLine(_AllQ);
                    }


                }
                catch (Exception eee)
                {
                    _AllQ = "CustomsWorkerRole.Utils.ServiceBusUtil.ShowAll failed :" + eee.ToString();
                    Debug.WriteLine(_AllQ);
                }

                //SendReqSheetStatistic();

                string bad = "bgcolor=red";
                string Good = "bgcolor=Green";
                return string.Format(
            @"<table id=""State_{0}"" >
<caption >Daily DCA Requests Report </caption>
<tr><td>MachineName</td><td>{0}</td></tr>
<tr><td>Service Host Uri</td><td>{9}</td></tr>
<tr><td>Service Start At</td><td>{1}</td></tr>
<tr><td>Last Success Response At</td><td>{2}</td></tr>

<tr {8} ><td>Total Success</td><td>{3}</td></tr>
<tr {6} ><td>Total Failed</td><td>{4}</td></tr>
<tr {7} ><td >Last Error</td><td>{5}</td></tr>
<tr><td >Filever</td><td>{10}</td></tr>
<tr><td >All Q</td><td>{11}</td></tr>
<tr><td >Sheet Statistic </td><td>{12}</td></tr>
</table>
",
             Environment.MachineName, ServiceStartAt,
            LastSuccessResponseAt,
            SuccessRequestedToday,
            FailedRequestedToday,
            LastError,
            FailedRequestedToday > 0 ? bad : "",
            LastError != "" ? bad : "",
            FailedRequestedToday == 0 ? Good : "", BaseAddress,
            FileVer(),
            "LastGetServiceBusStateAt :" + _LastGetServiceBusStateAt.ToString() + Environment.NewLine + _AllQ,
            "LastClacReqSheetStatistic At:" + ReqSheetStatisticClass._LastSendReqSheetStatistic.ToString() + Environment.NewLine + ReqSheetStatisticClass.MySheetStatistic

            );
            }
            catch (Exception e)
            {

                return "Error !!! " + e.ToString();
                //throw;
            }
        }

        private static void SendReqSheetStatistic()
        {
            return;
            ReqSheetStatisticClass.DoIt(
                            (emailbody, subj) =>
                            {
                                var parameters = new CommunicationWorkerRole.EmailParameters()
                                {
                                    From = "admin@fnarsoft.com",
                                    SwitchFromWithUserNameIfValid = true,
                                    To = "itzik@amital.co.il;YaronC@AMITAL.CO.IL;bbwrweim@mailparser.io",
                                    Cc = "",
                                    Bcc = "",
                                    Subject = subj,
                                    Body =
                                    "ReqSheetStatistic " + Environment.MachineName + "/ " + Environment.UserDomainName +
                                    emailbody,
                                };
                    //parameters.To += ";itzik@amital.co.il;YaronC@AMITAL.CO.IL";
                    //parameters.Tenant = 92;
                    Debug.WriteLine(parameters.To);
                                CommunicationWorkerRole.EmailingHelper.SendEmail(parameters);
                            });
        }

        internal static void RaiseAnotherDay()
        {
            try
            {
                RequestedToday = 0;
                FailedRequestedToday = 0;
                SuccessRequestedToday = 0;
                ServiceState.CurrentDate = DateTime.Now.Date;
                //DCAService.ServerRequsetCounter = 0;
            }
            catch (Exception)
            {

                //throw;
            }
        }
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










        internal static void Dispose()
        {



        }

    }
}
