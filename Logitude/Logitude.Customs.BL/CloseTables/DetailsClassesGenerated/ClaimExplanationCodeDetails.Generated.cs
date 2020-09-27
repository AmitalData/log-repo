
   
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
   public class ClaimExplanationCodeDetails : ClaimExplanationCode, ICloseTable<ClaimExplanationCode, ClaimExplanationCodeDetails>
   {
       public List<ClaimExplanationCodeDetails> GetAll()
       {
		    var all = new List<ClaimExplanationCodeDetails>();  
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "1", 
                SearchFields = "1,עקרון סיווג מספר", 
                Inactive = false, 
                LocalName = "עקרון סיווג מספר", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "10", 
                SearchFields = "10,שינוי בערך יצוא מוחזר", 
                Inactive = false, 
                LocalName = "שינוי בערך יצוא מוחזר", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "11", 
                SearchFields = "11,סגירת שנה", 
                Inactive = false, 
                LocalName = "סגירת שנה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "12", 
                SearchFields = "12,תיקון זיכוי מתביעה", 
                Inactive = false, 
                LocalName = "תיקון זיכוי מתביעה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "13", 
                SearchFields = "13,תיקון מלאי בתביעה", 
                Inactive = false, 
                LocalName = "תיקון מלאי בתביעה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "14", 
                SearchFields = "14,מלאי פתיחה לשנה סגורה", 
                Inactive = false, 
                LocalName = "מלאי פתיחה לשנה סגורה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "15", 
                SearchFields = "15,תיקון ניצול אשראי  לשנה סגורה", 
                Inactive = false, 
                LocalName = "תיקון ניצול אשראי  לשנה סגורה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "16", 
                SearchFields = "16,איפוס  עקב אי הגשת תביעה", 
                Inactive = false, 
                LocalName = "איפוס  עקב אי הגשת תביעה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "17", 
                SearchFields = "17,חישוב חודשי", 
                Inactive = false, 
                LocalName = "חישוב חודשי", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "18", 
                SearchFields = "18,תביעה חריגה", 
                Inactive = false, 
                LocalName = "תביעה חריגה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "19", 
                SearchFields = "19,תיקון תביעה חריגה", 
                Inactive = false, 
                LocalName = "תיקון תביעה חריגה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "2", 
                SearchFields = "2,הנחיות סיווג", 
                Inactive = false, 
                LocalName = "הנחיות סיווג", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "20", 
                SearchFields = "20,שינוי אחוז ניכוי מס-הסבה", 
                Inactive = false, 
                LocalName = "שינוי אחוז ניכוי מס-הסבה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "21", 
                SearchFields = "21,הסבות", 
                Inactive = false, 
                LocalName = "הסבות", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "22", 
                SearchFields = "22,מס''ב שחזר", 
                Inactive = false, 
                LocalName = "מס''ב שחזר", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "23", 
                SearchFields = "23,שיק שחזר", 
                Inactive = false, 
                LocalName = "שיק שחזר", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "24", 
                SearchFields = "24,הכחשת עסקה", 
                Inactive = false, 
                LocalName = "הכחשת עסקה", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "25", 
                SearchFields = "25,פירוט האסמכתא לתיקון", 
                Inactive = false, 
                LocalName = "פירוט האסמכתא לתיקון", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "3", 
                SearchFields = "3,לא התקיים אחד התנאים הבאים", 
                Inactive = false, 
                LocalName = "לא התקיים אחד התנאים הבאים", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "4", 
                SearchFields = "4,כללי", 
                Inactive = false, 
                LocalName = "כללי", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "5", 
                SearchFields = "5,הפרשי קנסות", 
                Inactive = false, 
                LocalName = "הפרשי קנסות", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "6", 
                SearchFields = "6,החזר יצוא מוחזר", 
                Inactive = false, 
                LocalName = "החזר יצוא מוחזר", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "7", 
                SearchFields = "7,שינוי אחוז מס", 
                Inactive = false, 
                LocalName = "שינוי אחוז מס", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "8", 
                SearchFields = "8,שינוי במחזוריות", 
                Inactive = false, 
                LocalName = "שינוי במחזוריות", 
			});
			 
            all.Add(new ClaimExplanationCodeDetails()
            {    
                Code = "9", 
                SearchFields = "9,שינוי בערך יצוא", 
                Inactive = false, 
                LocalName = "שינוי בערך יצוא", 
			});
			
            return all;
       }

	    public void MapPoco(ClaimExplanationCode newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(ClaimExplanationCode rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

