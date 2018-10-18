using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingMessaging.Common.ResponseData
{
    public abstract class ResponseDataBase
    {
        public bool HasException { get; set; }
        public string UserMessage { get; set; }
        public bool Succeeded { get; set; }


        //itzik 
        public bool ContinueProcessInBackground { get; set; }
        //public int Tenant { get; set; }
        public string AccountingRequestsSheetId { get; set; }

        public string CorrelationId { get; set; }
        //public bool ContinuePasiveSignInBackground { get; set; }

        ///public SheetStatusEnum RequestSheetStatus { get; set; }



        //public static string GetAccountingRequestStepText(AccountingStepEnum AccountingRequestStep)
        //{
        //    var mess = "";
        //    switch (AccountingRequestStep)
        //    {
        //        case AccountingStepEnum.CustomRequest:
        //            mess = "מכין בקשה";
        //            break;
        //        case AccountingStepEnum.CustomRequestSign:
        //            mess = "חותם על הבקשה";
        //            mess = "ממתין לחתימה יזומה";
        //            break;
        //        case AccountingStepEnum.DCAInProgressUploading:
        //            break;
        //        case AccountingStepEnum.DCAInProgressUploaded:

        //            break;
        //        case AccountingStepEnum.ReceivedCustomResponseCorrelation:
        //            mess =
        //                "ממתין לתשובת המכס";
        //            //"שולח בקשה";
        //            break;
        //        case AccountingStepEnum.AnalyzeResponseData:
        //            mess = "מנתח תשובה";
        //            break;
        //        case AccountingStepEnum.StartRequestParams:
        //        default:
        //            mess = "בונה בקשה";
        //            break;
        //    }
        //    return mess;
        //}

        public static bool RequestSheetCanCancelled(string requestStatusCode, bool isDCA)
        {

            var listStatusInProcess = Enum.GetValues(typeof(SheetStatusInProcessEnum))
                .AsQueryable().OfType<SheetStatusInProcessEnum>()
                .Select(rec => ((int)rec).ToString())
                .ToList();

            var listSheetStatusCanCancleEnum = Enum.GetValues(typeof(SheetStatusCanCancleEnum))
                .AsQueryable().OfType<SheetStatusCanCancleEnum>()
                .Select(rec => ((int)rec).ToString())
                .ToList();

            if (listSheetStatusCanCancleEnum.Contains(requestStatusCode))
            {
                return true;
            }
            if (isDCA && listStatusInProcess.Contains(requestStatusCode))
            {
                return true;

            }
            return false;
        }
    }

    public enum SheetStatusEnum : int
    {
        notSetNotInUse = 0,
        /*
SELECT TOP 1000 [Code]
      ,[EnglishName]
      ,[LocalName]
      ,[SearchFields]
  FROM [Amital1_Main].[Accounting].[AccountingRequestsSheetStatuses]
  order by cast(Code as decimal)
         * 
         Code	EnglishName
Code	EnglishName
1	Created
2	In Process
15	Send Failed
20	Sent
25	Analyze Failed
30	Analyzed
99	Cancelled
         * 
         */
        Created = 1,
        //CustomRequest=3,
        //CustomRequestSigned = 4,
        InProcess = 2,
        WaitingForSigning = 5,
        SendFailed = 15,
        Sent = 20,
        Received = 21,
        ReceivedFailed = 22,
        AnalyzeFailed = 25,
        Analyzed = 30,
        Cancelled = 99
    }

    public enum SheetStatusEndEnum : int
    {
        notSetNotInUse = 0,
        SendFailed = 15,
        ReceivedFailed = 22,
        AnalyzeFailed = 25,
        Analyzed = 30,
        Cancelled = 99
    }

    public enum SheetStatusCanCancleEnum : int
    {
        notSetNotInUse = 0,

        Created = 1,

        InProcess = 2,
        WaitingForSigning = 5,

    }

    public enum SheetStatusInProcessEnum : int
    {
        notSetNotInUse = 0,

        Created = 1,

        InProcess = 2,
        WaitingForSigning = 5,

        Sent = 20,
        Received = 21,

    }
    
}
