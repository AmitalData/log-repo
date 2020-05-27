
   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class CustomsRequestsSheetStatusDetails : CustomsRequestsSheetStatus, ICloseTable<CustomsRequestsSheetStatus, CustomsRequestsSheetStatusDetails>
   {
       public List<CustomsRequestsSheetStatusDetails> GetAll()
       {
		    var all = new List<CustomsRequestsSheetStatusDetails>();  
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "1", 
                EnglishName = "Created", 
                SearchFields = "1,Created,בקשה נרשמה", 
                Inactive = false, 
                LocalName = "בקשה נרשמה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "15", 
                EnglishName = "Send Failed", 
                SearchFields = "15,Send Failed,שליחה נכשלה", 
                Inactive = false, 
                LocalName = "שליחה נכשלה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "2", 
                EnglishName = "In Process", 
                SearchFields = "2,In Process,באמצע טיפול", 
                Inactive = false, 
                LocalName = "באמצע טיפול", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "20", 
                EnglishName = "Sent", 
                SearchFields = "20,Sent,נשלח", 
                Inactive = false, 
                LocalName = "נשלח", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "21", 
                EnglishName = "Received", 
                SearchFields = "21,Received,תשובה תקינה", 
                Inactive = false, 
                LocalName = "תשובה תקינה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "22", 
                EnglishName = "Received Failed", 
                SearchFields = "22,Received Failed,תשובה שגויה", 
                Inactive = false, 
                LocalName = "תשובה שגויה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "23", 
                EnglishName = "Sent-Response will arrive Via Safe", 
                SearchFields = "23,Sent-Response will arrive Via Safe,נשלח-המשוב יתקבל בכספת", 
                Inactive = false, 
                LocalName = "נשלח-המשוב יתקבל בכספת", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "25", 
                EnglishName = "Analyze Failed", 
                SearchFields = "25,Analyze Failed,ניתוח נכשל", 
                Inactive = false, 
                LocalName = "ניתוח נכשל", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "3", 
                EnglishName = "In Process", 
                SearchFields = "3,In Process,אמצע טיפול", 
                Inactive = false, 
                LocalName = "אמצע טיפול", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "30", 
                EnglishName = "Analyzed", 
                SearchFields = "30,Analyzed,תשובה נותחה", 
                Inactive = false, 
                LocalName = "תשובה נותחה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "4", 
                EnglishName = "Analyze Failed", 
                SearchFields = "4,Analyze Failed,טיפול שגוי", 
                Inactive = false, 
                LocalName = "טיפול שגוי", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "5", 
                EnglishName = "Waiting For Signing", 
                SearchFields = "5,Waiting For Signing,ממתין לחתימה", 
                Inactive = false, 
                LocalName = "ממתין לחתימה", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "6", 
                EnglishName = "Cancelled", 
                SearchFields = "6,Cancelled,מבוטלת", 
                Inactive = false, 
                LocalName = "מבוטלת", 
			});
			 
            all.Add(new CustomsRequestsSheetStatusDetails()
            {    
                Code = "99", 
                EnglishName = "Cancelled", 
                SearchFields = "99,Cancelled,מבוטלת", 
                Inactive = false, 
                LocalName = "מבוטלת", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsRequestsSheetStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsRequestsSheetStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

