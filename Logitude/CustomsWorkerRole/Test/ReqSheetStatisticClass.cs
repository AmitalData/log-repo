using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;

namespace CustomsWorkerRole.Test
{
    public class ReqSheetStatisticClass
    {


        public static void DoIt(Action<string, string> sendEmail, bool fromTesterForceSend = false)
        {
            try
            {
                if (fromTesterForceSend)
                {
                    SendIt(sendEmail);
                }
                //if (DateTime.Now.Subtract(LastClacReqSheetStatistic) > TimeSpan.FromHours(3))
                //{
                //    LastClacReqSheetStatistic = DateTime.Now;
                //    var reqSheetStatisticClass = new ReqSheetStatisticClass();
                //    MySheetStatistic = reqSheetStatisticClass.GetStatistic();
                //    MySheetStatisticSubject = reqSheetStatisticClass.GetStatisticSub();

                //}

                if (DateTime.Now.Subtract(_LastSendReqSheetStatistic) > TimeSpan.FromHours(2))///yaron daily --if (_LastSendReqSheetStatistic < DateTime.Now.Date)
                {
                    if (
                         ///yaron daily --(DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday) &&
                         IsGoodDay2Send() &&


                         (
                         //(DateTime.Now.TimeOfDay > TimeSpan.FromHours(10) && DateTime.Now.TimeOfDay < TimeSpan.FromHours(11))
                         //||
                         (DateTime.Now.TimeOfDay > TimeSpan.FromHours(16) && DateTime.Now.TimeOfDay < TimeSpan.FromHours(17))
                         )


                         )
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Starting Send Email");
                        _LastSendReqSheetStatistic = DateTime.Now;
                        //LastClacReqSheetStatistic = DateTime.Now;
                        var reqSheetStatisticClass = new ReqSheetStatisticClass();
                        MySheetStatistic = reqSheetStatisticClass.GetStatistic();
                        MySheetStatisticSubject = reqSheetStatisticClass.GetStatisticSub();


                        SendIt(sendEmail);
                    }

                }
            }
            catch (Exception stsE)
            {

                MySheetStatistic = "ReqSheetStatisticClass failed :" + stsE.ToString();
                //Debug.WriteLine(MySheetStatistic);
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(stsE,MySheetStatistic);
            }
        }

       

        private static void SendIt(Action<string, string> sendEmail)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Start SendIt");
            //_LastSendReqSheetStatistic = DateTime.Now;
            var doNotSendEmail = true;
            if (doNotSendEmail)
            {
                Task.Factory.StartNew(() =>
                {
                    sendEmail(MySheetStatistic, MySheetStatisticSubject);
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Email Sent !!!");
                }); ;
            }
        }

        private static bool IsGoodDay2Send()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    return true;
                    break;
                case DayOfWeek.Sunday:
                case DayOfWeek.Monday:
                case DayOfWeek.Saturday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                default:
                    return false;
                    break;
            }

        }

        public string GetStatistic()

        {


            var customsSettingQueryService = new CustomsSettingQueryService(1);
            var _AllCustomsSetting = customsSettingQueryService.GetAll();

            CustomsRequestsSheetQueryService qs;

            var sb = new StringBuilder();

            foreach (var costomSetting in _AllCustomsSetting.Where(rec => !string.IsNullOrWhiteSpace(rec.IIGServiceAddress)))
            {
                qs = new CustomsRequestsSheetQueryService(costomSetting.Tenant);
                sb.AppendLine(qs.GetDailyStatistic(costomSetting.Tenant));

                DateTime fromDate = DateTime.Now.Date;
                DateTime toDay = DateTime.Now.Date.AddDays(1);
                sb.AppendLine("CustomsAgentId=" + costomSetting.CustomsAgentId);
                qs = new CustomsRequestsSheetQueryService(costomSetting.Tenant);
                sb.AppendLine(qs.GetDailyStatistic(costomSetting.Tenant, fromDate, toDay));

            }
            return sb.ToString();
        }

        public string GetStatisticSub()
        //ICLWEBFREIGHT 24.08.17 50 40 2
        //UniqID         date   tot  totPay OpenToday
        {
            var tenant = 1;
            var repo = new DeclarationRepository(1);
            int TotDec;
            int TotDecPay;
            int TotDecOpen2Date;

            int TotLastMonthCreatedByUserId;
            int TotLastMonthReferentUserId;

            int LastMonthTotDecStandAlone = -1;
            int LastMonthTotdecIsConnectedToUnf = -1;
            int TotWithHATARA = -1;
            DateTime LastPaidDeclarationDate = DateTime.MinValue;
            ///20171227
            repo.GetDailyStatistic(tenant,
                out TotDec,
            out TotDecPay,
            out TotDecOpen2Date,
            out TotLastMonthCreatedByUserId,
            out TotLastMonthReferentUserId,
            out LastMonthTotDecStandAlone,
            out LastMonthTotdecIsConnectedToUnf,
            out LastPaidDeclarationDate,
            out TotWithHATARA

            );

            int LastMonthOpenByUserCCU = -1;
            int TotalOpenCCULastWeek = -1;

            var myCCUFILEMRepository = new CCUFILEMRepository(tenant);
            ///20171227
            myCCUFILEMRepository.GetWeeklyStatistic(tenant, out LastMonthOpenByUserCCU, out TotalOpenCCULastWeek);

            var repoSupplierInvoiceItem = new SupplierInvoiceItemRepository(1);
            int TotDeclarationAbove10Items = -1;
            int TotDeclarationAbove500Items = -1;
            ///20171227
            repoSupplierInvoiceItem.GetWeeklyStatistic(tenant, out TotDeclarationAbove10Items, out TotDeclarationAbove500Items);

            int analyzefailed = -1;
            int SentFailed = -1;
            int Answererror = -1;
            int totRequestsSheet = -1;
            DateTime LastRequestFromDCA = DateTime.MinValue;
            var customsRequestsSheetRepository = new CustomsRequestsSheetRepository(1);
            ///20171227
            customsRequestsSheetRepository.GetWeeklyStatistic(1,
                out analyzefailed,

            out SentFailed,
            out Answererror,
            out totRequestsSheet,
            out LastRequestFromDCA
            );

            int TotalClaimedOpened = -1;
            var claimRepository = new ClaimRepository(tenant);


            int logBoxDocuments = -1;
            int TotalSplitedDocsLastMonth = -1;
            ///20171227
            claimRepository.GetWeeklyStatistic(tenant, out TotalClaimedOpened);
            var myGDMFILINGRepository = new GDMFILINGRepository(1);
            ///20171227
            myGDMFILINGRepository.GetStatisticWeekly(out TotalSplitedDocsLastMonth,out logBoxDocuments);



            int totalTasks = -1;
            //int over30sectoanalyze = -1;int over30secfromlog2start = -1;
            int problemTasks = -1;

            var myYCULTASKRepository = new YCULTASKRepository(1);
            ///20171227
            myYCULTASKRepository.GetStatisticWeekly(
out totalTasks,
//out over30sectoanalyze,out over30secfromlog2start,
out problemTasks
);

            

            var customsBookRepository = new CustomsBookRepository(tenant);
            DateTime customsBookLastUpdateDate = DateTime.MinValue;
            customsBookLastUpdateDate = customsBookRepository.GetLastUpdateDate(tenant).GetValueOrDefault();


            string pordInfo = LogitudeSettings.ProductInfo ?? "";
            //var ver = GetVersion(pordInfo);
            //var burnAt = GatBurnAt(pordInfo);
            pordInfo = pordInfo.Replace(",Burn At:", "B").Replace("Version:", "").Replace(" ", "T");


            var val = string.Format(" {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16} {17} {18} {19} {20} {21} {22} {23} {24} {25} {26}",
LogitudeSettings.LogitudeURL,  //{0} 
DateTime.Now.Date.ToShortDateString(), //{1} 
TotDec,//{2} 
TotDecPay,//{3} 
TotDecOpen2Date,//{4} 
TotLastMonthCreatedByUserId,//{5} 
TotLastMonthReferentUserId,// {6} 

analyzefailed,//{7} 
SentFailed,//{8} 
Answererror,//{9} 
totRequestsSheet,//{10} 

totalTasks,//{11} 
           //over30sectoanalyze,over30secfromlog2start,
problemTasks,//{12}


LastMonthOpenByUserCCU,//13  
LastMonthTotDecStandAlone,//14
LastMonthTotdecIsConnectedToUnf,//15
TotDeclarationAbove10Items,//16
TotDeclarationAbove500Items,//17
LastRequestFromDCA.ToString().Replace(' ', 'T'),//18
TotalClaimedOpened,//19
TotalOpenCCULastWeek,//20
TotalSplitedDocsLastMonth,//21
LastPaidDeclarationDate.ToString().Replace(' ', 'T'),//22
customsBookLastUpdateDate.ToString().Replace(' ', 'T'),//23
pordInfo,//24
TotWithHATARA,//25
logBoxDocuments//26
            );
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(val);
            return val;
        }

        private void SetUniqueID()
        {

        }

        //public static DateTime LastClacReqSheetStatistic { get; set; }
        public static string MySheetStatistic { get; set; }
        public static string MySheetStatisticSubject { get; set; }
        public static DateTime _LastSendReqSheetStatistic { get; set; }

    }
}
