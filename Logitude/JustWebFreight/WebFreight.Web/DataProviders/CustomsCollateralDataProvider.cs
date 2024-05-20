using CWXSD;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CustomsCollateralDataProvider : BaseDataProvider
    {

        public List<CustomsCollateral> CustomsCollateral { get; set; }//בטוחות


    }
    public class CustomsCollateral
    {
        public string CollateralRequestNumber { get; set; }//מספר דרישה
        public string FileNo { get; set; }// מספר תיק
        public string EntityIdKey1 { get; set; }//מספר הצהרה
        public DateTime? RequestValidityDate { get; set; }//תוקף דרישה
        public DateTime? CollateralValidityDate { get; set; }//תוקף בטוחה נדרש
        public string CollateralRequestStatusName { get; set; }//שם סטטוס בקשה בטחונות
        
        public List<CustomsCollateralsAnswers> CustomsCollateralsAnswers { get; set; }//חשבונות
       

    }


    public class CustomsCollateralsAnswers
    {
        public string RequestFileTypeName { get; set; }//סוג תיק מבוקש
        public decimal? RequestFileAmount { get; set; }//סכום מבוקש
        public decimal? AllocatedAmount { get; set; }//סכום מענה
        public string AnswerEntityTypeName { get; set; }//סוג מענה לדרישה
        public string CustomsTapgFile { get; set; }//מספר תיק תפ"ג
        public string CustomsNumeral { get; set; }//מספר רץ תפ"ג
        public int LineNumber { get; set; }//מספר רץ תפ"ג
       



    }

   
}