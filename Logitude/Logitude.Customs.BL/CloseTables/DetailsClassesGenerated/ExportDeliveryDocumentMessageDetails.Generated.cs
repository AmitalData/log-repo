
   
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
   public class ExportDeliveryDocumentMessageDetails : ExportDeliveryDocumentMessage, ICloseTable<ExportDeliveryDocumentMessage, ExportDeliveryDocumentMessageDetails>
   {
       public List<ExportDeliveryDocumentMessageDetails> GetAll()
       {
		    var all = new List<ExportDeliveryDocumentMessageDetails>();  
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "1", 
                SearchFields = "1,יורו טרייד", 
                Inactive = false, 
                LocalName = "יורו טרייד", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "10", 
                SearchFields = "10,לאומי", 
                Inactive = false, 
                LocalName = "לאומי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "11", 
                SearchFields = "11,דיסקונט", 
                Inactive = false, 
                LocalName = "דיסקונט", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "12", 
                SearchFields = "12,הפועלים", 
                Inactive = false, 
                LocalName = "הפועלים", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "13", 
                SearchFields = "13,אגוד", 
                Inactive = false, 
                LocalName = "אגוד", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "14", 
                SearchFields = "14,אוצר החייל", 
                Inactive = false, 
                LocalName = "אוצר החייל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "17", 
                SearchFields = "17,מרכנתיל דיסקונט", 
                Inactive = false, 
                LocalName = "מרכנתיל דיסקונט", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "19", 
                SearchFields = "19,החקלאות לישראל", 
                Inactive = false, 
                LocalName = "החקלאות לישראל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "2", 
                SearchFields = "2,צפון אמריקה", 
                Inactive = false, 
                LocalName = "צפון אמריקה", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "20", 
                SearchFields = "20,מזרחי טפחות", 
                Inactive = false, 
                LocalName = "מזרחי טפחות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "21", 
                SearchFields = "21,deretrahC dradnat", 
                Inactive = false, 
                LocalName = "deretrahC dradnat", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "22", 
                SearchFields = "22,CitiExportDeliveryDocumentMessage", 
                Inactive = false, 
                LocalName = "CitiExportDeliveryDocumentMessage", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "23", 
                SearchFields = "23,HSBC  ExportDeliveryDocumentMessage plc", 
                Inactive = false, 
                LocalName = "HSBC  ExportDeliveryDocumentMessage plc", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "24", 
                SearchFields = "24,בנק אמריקאי ישראלי", 
                Inactive = false, 
                LocalName = "בנק אמריקאי ישראלי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "25", 
                SearchFields = "25,BNP paribas", 
                Inactive = false, 
                LocalName = "BNP paribas", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "26", 
                SearchFields = "26,יובנק", 
                Inactive = false, 
                LocalName = "יובנק", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "27", 
                SearchFields = "27,עין חי", 
                Inactive = false, 
                LocalName = "עין חי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "28", 
                SearchFields = "28,קונטיננטל לישראל", 
                Inactive = false, 
                LocalName = "קונטיננטל לישראל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "3", 
                SearchFields = "3,גמול", 
                Inactive = false, 
                LocalName = "גמול", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "30", 
                SearchFields = "30,למסחר", 
                Inactive = false, 
                LocalName = "למסחר", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "31", 
                SearchFields = "31,הבינלאומי הראשון", 
                Inactive = false, 
                LocalName = "הבינלאומי הראשון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "32", 
                SearchFields = "32,למימון ומסחר", 
                Inactive = false, 
                LocalName = "למימון ומסחר", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "33", 
                SearchFields = "33,מרכנתיל", 
                Inactive = false, 
                LocalName = "מרכנתיל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "34", 
                SearchFields = "34,ערבי ישראלי", 
                Inactive = false, 
                LocalName = "ערבי ישראלי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "35", 
                SearchFields = "35,פולסקא קאסא", 
                Inactive = false, 
                LocalName = "פולסקא קאסא", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "37", 
                SearchFields = "37,אלאורדון", 
                Inactive = false, 
                LocalName = "אלאורדון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "38", 
                SearchFields = "38,אל תיג'ארי אלפלסטיני", 
                Inactive = false, 
                LocalName = "אל תיג'ארי אלפלסטיני", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "39", 
                SearchFields = "39,SBI State Babk", 
                Inactive = false, 
                LocalName = "SBI State Babk", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "4", 
                SearchFields = "4,יהב", 
                Inactive = false, 
                LocalName = "יהב", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "40", 
                SearchFields = "40,טפחות- חברה להנפק", 
                Inactive = false, 
                LocalName = "טפחות- חברה להנפק", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "41", 
                SearchFields = "41,מזרחי השק.", 
                Inactive = false, 
                LocalName = "מזרחי השק.", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "42", 
                SearchFields = "42,משאבים", 
                Inactive = false, 
                LocalName = "משאבים", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "43", 
                SearchFields = "43,בנק אלאהלי אלאורדוני", 
                Inactive = false, 
                LocalName = "בנק אלאהלי אלאורדוני", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "44", 
                SearchFields = "44,פועלים שוקי הון", 
                Inactive = false, 
                LocalName = "פועלים שוקי הון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "46", 
                SearchFields = "46,מסד", 
                Inactive = false, 
                LocalName = "מסד", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "47", 
                SearchFields = "47,בנק עולמי", 
                Inactive = false, 
                LocalName = "בנק עולמי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "48", 
                SearchFields = "48,קופת העובד הלאומי", 
                Inactive = false, 
                LocalName = "קופת העובד הלאומי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "49", 
                SearchFields = "49,אלבנק אלערבי", 
                Inactive = false, 
                LocalName = "אלבנק אלערבי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "5", 
                SearchFields = "5,מניב", 
                Inactive = false, 
                LocalName = "מניב", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "50", 
                SearchFields = "50,מ.ס.ב", 
                Inactive = false, 
                LocalName = "מ.ס.ב", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "51", 
                SearchFields = "51,כאל", 
                Inactive = false, 
                LocalName = "כאל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "52", 
                SearchFields = "52,פאג''י", 
                Inactive = false, 
                LocalName = "פאג''י", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "53", 
                SearchFields = "53,עליה לאומי", 
                Inactive = false, 
                LocalName = "עליה לאומי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "54", 
                SearchFields = "54,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "56", 
                SearchFields = "56,ד. מימון", 
                Inactive = false, 
                LocalName = "ד. מימון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "57", 
                SearchFields = "57,מזרחי תעשי", 
                Inactive = false, 
                LocalName = "מזרחי תעשי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "58", 
                SearchFields = "58,לאומי ושות", 
                Inactive = false, 
                LocalName = "לאומי ושות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "59", 
                SearchFields = "59,ש.ב.א", 
                Inactive = false, 
                LocalName = "ש.ב.א", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "6", 
                SearchFields = "6,אדנים למשכנתאות", 
                Inactive = false, 
                LocalName = "אדנים למשכנתאות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "64", 
                SearchFields = "64,גחלת", 
                Inactive = false, 
                LocalName = "גחלת", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "65", 
                SearchFields = "65,חסך", 
                Inactive = false, 
                LocalName = "חסך", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "66", 
                SearchFields = "66,בנק אלקהירה- עמאן", 
                Inactive = false, 
                LocalName = "בנק אלקהירה- עמאן", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "67", 
                SearchFields = "67,אלעקרי אלמצרי אלערבי", 
                Inactive = false, 
                LocalName = "אלעקרי אלמצרי אלערבי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "68", 
                SearchFields = "68,בנק דקסיה ישראל", 
                Inactive = false, 
                LocalName = "בנק דקסיה ישראל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "69", 
                SearchFields = "69,בלל לתעשיה", 
                Inactive = false, 
                LocalName = "בלל לתעשיה", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "7", 
                SearchFields = "7,בנק לפיתוח התעשיה", 
                Inactive = false, 
                LocalName = "בנק לפיתוח התעשיה", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "70", 
                SearchFields = "70,לאומי נחל", 
                Inactive = false, 
                LocalName = "לאומי נחל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "71", 
                SearchFields = "71,קומרציאל ג'ורדן בנק", 
                Inactive = false, 
                LocalName = "קומרציאל ג'ורדן בנק", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "73", 
                SearchFields = "73,אלאסלאמי אלערבי", 
                Inactive = false, 
                LocalName = "אלאסלאמי אלערבי", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "74", 
                SearchFields = "74,HSBC בנק מזרח תיכון", 
                Inactive = false, 
                LocalName = "HSBC בנק מזרח תיכון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "75", 
                SearchFields = "75,חב. למימון", 
                Inactive = false, 
                LocalName = "חב. למימון", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "76", 
                SearchFields = "76,אלאסתת'מאר אלפלסטיני", 
                Inactive = false, 
                LocalName = "אלאסתת'מאר אלפלסטיני", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "77", 
                SearchFields = "77,לאומי למשכנתאות", 
                Inactive = false, 
                LocalName = "לאומי למשכנתאות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "78", 
                SearchFields = "78,אלראיסי ללתנמיה", 
                Inactive = false, 
                LocalName = "אלראיסי ללתנמיה", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "8", 
                SearchFields = "8,הספנות", 
                Inactive = false, 
                LocalName = "הספנות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "80", 
                SearchFields = "80,טפחות", 
                Inactive = false, 
                LocalName = "טפחות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "81", 
                SearchFields = "81,אלאסלאמי אלפלסטינ", 
                Inactive = false, 
                LocalName = "אלאסלאמי אלפלסטינ", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "82", 
                SearchFields = "82,אלקודס ללתנמיה וללסת", 
                Inactive = false, 
                LocalName = "אלקודס ללתנמיה וללסת", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "83", 
                SearchFields = "83,אל-אתיחאד ללדיכאר", 
                Inactive = false, 
                LocalName = "אל-אתיחאד ללדיכאר", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "84", 
                SearchFields = "84,אלאסכאן אלאורדוני", 
                Inactive = false, 
                LocalName = "אלאסכאן אלאורדוני", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "86", 
                SearchFields = "86,כרמל", 
                Inactive = false, 
                LocalName = "כרמל", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "87", 
                SearchFields = "87,אלדאולי אלפלסטיני", 
                Inactive = false, 
                LocalName = "אלדאולי אלפלסטיני", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "89", 
                SearchFields = "89,בנק פלסטין", 
                Inactive = false, 
                LocalName = "בנק פלסטין", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "9", 
                SearchFields = "9,הדואר", 
                Inactive = false, 
                LocalName = "הדואר", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "90", 
                SearchFields = "90,דיסקונט למשכנתאות", 
                Inactive = false, 
                LocalName = "דיסקונט למשכנתאות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "91", 
                SearchFields = "91,משכן", 
                Inactive = false, 
                LocalName = "משכן", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "92", 
                SearchFields = "92,הבינלאומי למשכנתא", 
                Inactive = false, 
                LocalName = "הבינלאומי למשכנתא", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "93", 
                SearchFields = "93,אלאורדון ואלכווית", 
                Inactive = false, 
                LocalName = "אלאורדון ואלכווית", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "94", 
                SearchFields = "94,עצמאות למשכנתאות", 
                Inactive = false, 
                LocalName = "עצמאות למשכנתאות", 
			});
			 
            all.Add(new ExportDeliveryDocumentMessageDetails()
            {    
                Code = "99", 
                SearchFields = "99,בנק ישראל", 
                Inactive = false, 
                LocalName = "בנק ישראל", 
			});
			
            return all;
       }

	    public void MapPoco(ExportDeliveryDocumentMessage newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(ExportDeliveryDocumentMessage rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

