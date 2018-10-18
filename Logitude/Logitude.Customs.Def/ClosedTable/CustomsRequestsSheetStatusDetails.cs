using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.ClosedTable
{
    public class CustomsRequestsSheetStatusDetails : CustomsRequestsSheetStatus, ICloseTable<CustomsRequestsSheetStatus, CustomsRequestsSheetStatusDetails>
    {
        public  List<CustomsRequestsSheetStatusDetails> GetAll()
        {
            var all = new List<CustomsRequestsSheetStatusDetails>();
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.Created),
                EnglishName = "Created",
                LocalName = "בקשה נרשמה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.SendFailed),
                EnglishName = "Send Failed",
                LocalName = "שליחה נכשלה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.InProcess),
                EnglishName = "In Process",
                LocalName = "באמצע טיפול"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.WaitingForSigning),
                EnglishName = "Waiting For Signing",
                LocalName = "ממתין לחתימה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.AnalyzeFailed),
                EnglishName = "Analyze Failed",
                LocalName = "ניתוח נכשל"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.Analyzed),
                EnglishName = "Analyzed",
                LocalName = "תשובה נותחה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.Cancelled),
                EnglishName = "Cancelled",
                LocalName = "מבוטלת"
            });


            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.Sent),
                EnglishName = "Sent",
                LocalName = "נשלח"
            });

            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.Received),
                EnglishName = "Received",
                LocalName = "תשובה תקינה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.ReceivedFailed),
                EnglishName = "Received Failed",
                LocalName = "תשובה שגויה"
            });
            all.Add(new CustomsRequestsSheetStatusDetails()
            {
                Code = GetCode(SheetStatusEnum.SentResponseOnDCA),
                EnglishName = "Sent-Response will arrive Via Safe",
                LocalName = "נשלח-המשוב יתקבל בכספת"
            });

            all.ForEach(rec => rec.SearchFields = GetSearchFields(rec));
            return all;
        }

        private static string GetCode(SheetStatusEnum sheetStatusEnum)
        {
            int i = (int)sheetStatusEnum;
            return i.ToString();
        }



        public string GetSearchFields(CustomsRequestsSheetStatus rec)
        {
            return string.Concat(rec.Code, ",", rec.EnglishName, ",", rec.LocalName);
        }

        

        public void MapPoco(CustomsRequestsSheetStatus poco)
        {
            poco.Code = this.Code;
            poco.EnglishName = this.EnglishName;
            poco.LocalName = this.LocalName;
            poco.SearchFields = GetSearchFields(this);
        }
    }
    //public enum CustomsRequestStepEnum : int
    //{
    //    RequestParams = 0,
    //    CustomRequest,
    //    CustomRequestSign,
    //    CustomResponseCorrelation,
    //    ResponseData

    //}
 



    public enum CommStatusEnum
    {
        W, D, F
            , P
    }
    public enum InOutEnum
    {
        I,O
    }
}
