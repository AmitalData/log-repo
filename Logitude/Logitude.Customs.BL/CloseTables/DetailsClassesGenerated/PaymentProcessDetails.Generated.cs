
   
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
   public class PaymentProcessDetails : PaymentProcess, ICloseTable<PaymentProcess, PaymentProcessDetails>
   {
       public List<PaymentProcessDetails> GetAll()
       {
		    var all = new List<PaymentProcessDetails>();  
            all.Add(new PaymentProcessDetails()
            {    
                Code = "1", 
                SearchFields = "1,הגשת הצהרה", 
                Inactive = false, 
                LocalName = "הגשת הצהרה", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "10", 
                SearchFields = "10,ריכוז גרעונות", 
                Inactive = false, 
                LocalName = "ריכוז גרעונות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "11", 
                SearchFields = "11,תפ''ג-החזרי תביעות", 
                Inactive = false, 
                LocalName = "תפ''ג-החזרי תביעות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "12", 
                SearchFields = "12,תפ''ג-פקדונות", 
                Inactive = false, 
                LocalName = "תפ''ג-פקדונות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "13", 
                SearchFields = "13,תפ''ג-גרעונות", 
                Inactive = false, 
                LocalName = "תפ''ג-גרעונות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "14", 
                SearchFields = "14,פיקדון אחר התרה", 
                Inactive = false, 
                LocalName = "פיקדון אחר התרה", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "15", 
                SearchFields = "15,תפ''ג-פקדון ייצוא", 
                Inactive = false, 
                LocalName = "תפ''ג-פקדון ייצוא", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "16", 
                SearchFields = "16,תביעות ידני", 
                Inactive = false, 
                LocalName = "תביעות ידני", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "18", 
                SearchFields = "18,פט''מ", 
                Inactive = false, 
                LocalName = "פט''מ", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "2", 
                SearchFields = "2,תיקון הצהרה", 
                Inactive = false, 
                LocalName = "תיקון הצהרה", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "23", 
                SearchFields = "23,אולם נוסעים", 
                Inactive = false, 
                LocalName = "אולם נוסעים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "24", 
                SearchFields = "24,מסי חפצים ביתים", 
                Inactive = false, 
                LocalName = "מסי חפצים ביתים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "25", 
                SearchFields = "25,החזר מע''מ אילת", 
                Inactive = false, 
                LocalName = "החזר מע''מ אילת", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "26", 
                SearchFields = "26,תפיסות", 
                Inactive = false, 
                LocalName = "תפיסות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "27", 
                SearchFields = "27,ענישה בהסדר", 
                Inactive = false, 
                LocalName = "ענישה בהסדר", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "29", 
                SearchFields = "29,סב''נים", 
                Inactive = false, 
                LocalName = "סב''נים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "3", 
                SearchFields = "3,יבוא אישי", 
                Inactive = false, 
                LocalName = "יבוא אישי", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "30", 
                SearchFields = "30,הזמנת רשימון", 
                Inactive = false, 
                LocalName = "הזמנת רשימון", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "39", 
                SearchFields = "39,רכב זכאים", 
                Inactive = false, 
                LocalName = "רכב זכאים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "41", 
                SearchFields = "41,הישבון-הכנסות", 
                Inactive = false, 
                LocalName = "הישבון-הכנסות", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "42", 
                SearchFields = "42,הישבון-החזרים", 
                Inactive = false, 
                LocalName = "הישבון-החזרים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "43", 
                SearchFields = "43,עקול/רטרו/חילוט", 
                Inactive = false, 
                LocalName = "עקול/רטרו/חילוט", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "44", 
                SearchFields = "44,עיקולים-הישבון", 
                Inactive = false, 
                LocalName = "עיקולים-הישבון", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "45", 
                SearchFields = "45,א. נוסעים", 
                Inactive = false, 
                LocalName = "א. נוסעים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "49", 
                SearchFields = "49,קרנה", 
                Inactive = false, 
                LocalName = "קרנה", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "5", 
                SearchFields = "5,מסי רכב אישי", 
                Inactive = false, 
                LocalName = "מסי רכב אישי", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "52", 
                SearchFields = "52,ערבויות-מימוש", 
                Inactive = false, 
                LocalName = "ערבויות-מימוש", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "55", 
                SearchFields = "55,חבילות שי", 
                Inactive = false, 
                LocalName = "חבילות שי", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "6", 
                SearchFields = "6,גרעונות - הסדר תשלומים", 
                Inactive = false, 
                LocalName = "גרעונות - הסדר תשלומים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "72", 
                SearchFields = "72,תפיסות א.נוסעים", 
                Inactive = false, 
                LocalName = "תפיסות א.נוסעים", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "8", 
                SearchFields = "8,פתיחת פקדון ייצוא", 
                Inactive = false, 
                LocalName = "פתיחת פקדון ייצוא", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "85", 
                SearchFields = "85,ידני -מ. הבטחון", 
                Inactive = false, 
                LocalName = "ידני -מ. הבטחון", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "9", 
                SearchFields = "9,פתיחת פקדון", 
                Inactive = false, 
                LocalName = "פתיחת פקדון", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "90", 
                SearchFields = "90,תהליך ידני", 
                Inactive = false, 
                LocalName = "תהליך ידני", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "91", 
                SearchFields = "91,העברה בנקאית-אשראי", 
                Inactive = false, 
                LocalName = "העברה בנקאית-אשראי", 
			});
			 
            all.Add(new PaymentProcessDetails()
            {    
                Code = "99", 
                SearchFields = "99,כל התהליכים", 
                Inactive = false, 
                LocalName = "כל התהליכים", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentProcess newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(PaymentProcess rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

