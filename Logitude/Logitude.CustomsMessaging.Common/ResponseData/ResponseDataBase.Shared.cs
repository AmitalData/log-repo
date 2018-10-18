using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.CustomsMessaging.Common.ResponseData
{

    public abstract class ResponseDataBase
    {
        public bool HasException { get; set; }
        public string UserMessage { get; set; }
        public bool Succeeded { get; set; }


        //itzik 
        public bool ContinueProcessInBackground { get; set; }
        //public int Tenant { get; set; }
        public string CustomsRequestsSheetId { get; set; }

        public string CorrelationId { get; set; }
        //public bool ContinuePasiveSignInBackground { get; set; }

        ///public SheetStatusEnum RequestSheetStatus { get; set; }



        public static string GetCustomsRequestStepText(CustomsStepEnum customsRequestStep)
        {
            var mess = "";
            switch (customsRequestStep)
            {
                case CustomsStepEnum.CustomRequest:
                    mess = "מכין בקשה";
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    mess = "חותם על הבקשה";
                    mess = "ממתין לחתימה יזומה";
                    break;
                case CustomsStepEnum.DCAInProgressUploading:
                    break;
                case CustomsStepEnum.DCAInProgressUploaded:

                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    mess =
                        "ממתין לתשובת המכס";
                    //"שולח בקשה";
                    break;
                //case CustomsStepEnum.SentResponseOnDCA:
                //    mess = "נשלח המשוב יתקבל בכספת";
                //    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    mess = "מנתח תשובה";
                    break;
                case CustomsStepEnum.StartRequestParams:
                default:
                    mess = "בונה בקשה";
                    break;
            }
            return mess;
        }

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
  FROM [Amital1_Main].[Customs].[CustomsRequestsSheetStatuses]
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
        SentResponseOnDCA = 23,
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
        SendFailed = 15,///
        InProcess = 2,
        WaitingForSigning = 5,
        SentResponseOnDCA = 23,

    }

    public enum SheetStatusInProcessEnum : int
    {
        notSetNotInUse = 0,

        Created = 1,

        InProcess = 2,
        WaitingForSigning = 5,

        Sent = 20,
        Received = 21,
        SentResponseOnDCA = 23,
    }
}
