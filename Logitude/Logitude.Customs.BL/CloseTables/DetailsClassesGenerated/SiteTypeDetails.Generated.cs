
   
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
   public class SiteTypeDetails : SiteType, ICloseTable<SiteType, SiteTypeDetails>
   {
       public List<SiteTypeDetails> GetAll()
       {
		    var all = new List<SiteTypeDetails>();  
            all.Add(new SiteTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,נמל ימי", 
                Inactive = false, 
                LocalName = "נמל ימי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "10", 
                SearchFields = "10,מחסן מסירה לחוזרים מחו''ל(פטור ושמור)", 
                Inactive = false, 
                LocalName = "מחסן מסירה לחוזרים מחו''ל(פטור ושמור)", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "100", 
                SearchFields = "100,אתר בדיקה לבית המכס", 
                Inactive = false, 
                LocalName = "אתר בדיקה לבית המכס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "12", 
                SearchFields = "12,משקף מכולות - רמפה", 
                Inactive = false, 
                LocalName = "משקף מכולות - רמפה", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "13", 
                SearchFields = "13,מעבר יבשתי פנימי", 
                Inactive = false, 
                LocalName = "מעבר יבשתי פנימי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "14", 
                SearchFields = "14,תחנת מכס מעבר גבול בינלאומי", 
                Inactive = false, 
                LocalName = "תחנת מכס מעבר גבול בינלאומי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "15", 
                SearchFields = "15,מעבר גבול בינלאומי צבאי", 
                Inactive = false, 
                LocalName = "מעבר גבול בינלאומי צבאי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "16", 
                SearchFields = "16,תחנת מכס מעבר יבשתי פנימי קו תפר", 
                Inactive = false, 
                LocalName = "תחנת מכס מעבר יבשתי פנימי קו תפר", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "17", 
                SearchFields = "17,תחנת מכס מעבר פנימי אס''ח", 
                Inactive = false, 
                LocalName = "תחנת מכס מעבר פנימי אס''ח", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "19", 
                SearchFields = "19,בור בידוק כלי רכב במעבר גבול", 
                Inactive = false, 
                LocalName = "בור בידוק כלי רכב במעבר גבול", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,בית מכס", 
                Inactive = false, 
                LocalName = "בית מכס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "20", 
                SearchFields = "20,מסוף דואר בנמל אווירי", 
                Inactive = false, 
                LocalName = "מסוף דואר בנמל אווירי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "21", 
                SearchFields = "21,בית מכס דואר חבילות", 
                Inactive = false, 
                LocalName = "בית מכס דואר חבילות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "22", 
                SearchFields = "22,אולם בדיקות במסוף מטען אווירי", 
                Inactive = false, 
                LocalName = "אולם בדיקות במסוף מטען אווירי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "23", 
                SearchFields = "23,שער נמל ימי", 
                Inactive = false, 
                LocalName = "שער נמל ימי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "24", 
                SearchFields = "24,מסוף מטען אווירי קדמי פנימי", 
                Inactive = false, 
                LocalName = "מסוף מטען אווירי קדמי פנימי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "25", 
                SearchFields = "25,תחנת מכס", 
                Inactive = false, 
                LocalName = "תחנת מכס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "27", 
                SearchFields = "27,תחנת מכס אולם נוסעים", 
                Inactive = false, 
                LocalName = "תחנת מכס אולם נוסעים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "29", 
                SearchFields = "29,אזור בידוק יבוא אישי באולם נוסעים", 
                Inactive = false, 
                LocalName = "אזור בידוק יבוא אישי באולם נוסעים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,מעבר גבול בינלאומי אזרחי", 
                Inactive = false, 
                LocalName = "מעבר גבול בינלאומי אזרחי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "30", 
                SearchFields = "30,מחסן מטענים כללי", 
                Inactive = false, 
                LocalName = "מחסן מטענים כללי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "31", 
                SearchFields = "31,מחסן רכב", 
                Inactive = false, 
                LocalName = "מחסן רכב", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "32", 
                SearchFields = "32,אולם תצוגה/מחסן תצוגת כלי רכב", 
                Inactive = false, 
                LocalName = "אולם תצוגה/מחסן תצוגת כלי רכב", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "34", 
                SearchFields = "34,אתר בדיקה", 
                Inactive = false, 
                LocalName = "אתר בדיקה", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "37", 
                SearchFields = "37,מחסן כימיקלים", 
                Inactive = false, 
                LocalName = "מחסן כימיקלים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "38", 
                SearchFields = "38,מחסן דלק", 
                Inactive = false, 
                LocalName = "מחסן דלק", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "39", 
                SearchFields = "39,ממגורות", 
                Inactive = false, 
                LocalName = "ממגורות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,נמל אוירי", 
                Inactive = false, 
                LocalName = "נמל אוירי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "40", 
                SearchFields = "40,מחסן לצידת אוניות וכלי טיס", 
                Inactive = false, 
                LocalName = "מחסן לצידת אוניות וכלי טיס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "41", 
                SearchFields = "41,מחסן מכירה ליוצאים מישראל", 
                Inactive = false, 
                LocalName = "מחסן מכירה ליוצאים מישראל", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "42", 
                SearchFields = "42,מחסן בלדר בתוך נמל אווירי", 
                Inactive = false, 
                LocalName = "מחסן בלדר בתוך נמל אווירי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "43", 
                SearchFields = "43,מחסן בלדר מחוץ לנמל אווירי", 
                Inactive = false, 
                LocalName = "מחסן בלדר מחוץ לנמל אווירי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "44", 
                SearchFields = "44,מחסן תפיסות", 
                Inactive = false, 
                LocalName = "מחסן תפיסות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "45", 
                SearchFields = "45,אתר מכירה (סבנים)", 
                Inactive = false, 
                LocalName = "אתר מכירה (סבנים)", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "46", 
                SearchFields = "46,תחנת מעבר מטעני רכבת", 
                Inactive = false, 
                LocalName = "תחנת מעבר מטעני רכבת", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "47", 
                SearchFields = "47,רמפת בידוק מכולות/מטענים", 
                Inactive = false, 
                LocalName = "רמפת בידוק מכולות/מטענים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "48", 
                SearchFields = "48,אתר בידוק באתר אחסון בפיקוח המכס", 
                Inactive = false, 
                LocalName = "אתר בידוק באתר אחסון בפיקוח המכס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "49", 
                SearchFields = "49,אתר בידוק באתר בפיקוח המכס", 
                Inactive = false, 
                LocalName = "אתר בידוק באתר בפיקוח המכס", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "5", 
                SearchFields = "5,דואר חבילות מרכזי", 
                Inactive = false, 
                LocalName = "דואר חבילות מרכזי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "51", 
                SearchFields = "51,משקף מכולות - משקף", 
                Inactive = false, 
                LocalName = "משקף מכולות - משקף", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "53", 
                SearchFields = "53,מחסן דיוטיפרי", 
                Inactive = false, 
                LocalName = "מחסן דיוטיפרי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "54", 
                SearchFields = "54,מרינה/מעגן סירות", 
                Inactive = false, 
                LocalName = "מרינה/מעגן סירות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "55", 
                SearchFields = "55,מחסן לוגיסטי", 
                Inactive = false, 
                LocalName = "מחסן לוגיסטי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "56", 
                SearchFields = "56,מחסן קרור", 
                Inactive = false, 
                LocalName = "מחסן קרור", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "57", 
                SearchFields = "57,מחסן להספקת ציוד אלקטרוני לאניות", 
                Inactive = false, 
                LocalName = "מחסן להספקת ציוד אלקטרוני לאניות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "58", 
                SearchFields = "58,נמל פריקה דלק", 
                Inactive = false, 
                LocalName = "נמל פריקה דלק", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "59", 
                SearchFields = "59,מחסן מסוף קירור", 
                Inactive = false, 
                LocalName = "מחסן מסוף קירור", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "6", 
                SearchFields = "6,מחסן רשוי", 
                Inactive = false, 
                LocalName = "מחסן רשוי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "60", 
                SearchFields = "60,מחסן מטעני עולים", 
                Inactive = false, 
                LocalName = "מחסן מטעני עולים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "61", 
                SearchFields = "61,דואר משרד הבטחון", 
                Inactive = false, 
                LocalName = "דואר משרד הבטחון", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "62", 
                SearchFields = "62,דואר דיפלומטים", 
                Inactive = false, 
                LocalName = "דואר דיפלומטים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "63", 
                SearchFields = "63,מחסן מתכות יקרות", 
                Inactive = false, 
                LocalName = "מחסן מתכות יקרות", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "64", 
                SearchFields = "64,נמל פריקה פחם", 
                Inactive = false, 
                LocalName = "נמל פריקה פחם", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "65", 
                SearchFields = "65,מחסן צידה לדיפלומטים", 
                Inactive = false, 
                LocalName = "מחסן צידה לדיפלומטים", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "66", 
                SearchFields = "66,מסוף מטען אווירי חיצוני", 
                Inactive = false, 
                LocalName = "מסוף מטען אווירי חיצוני", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "7", 
                SearchFields = "7,מסוף מטען עורפי חיצוני ימי", 
                Inactive = false, 
                LocalName = "מסוף מטען עורפי חיצוני ימי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "70", 
                SearchFields = "70,מסוף מטען עורפי חיצוני יבשתי", 
                Inactive = false, 
                LocalName = "מסוף מטען עורפי חיצוני יבשתי", 
			});
			 
            all.Add(new SiteTypeDetails()
            {    
                Code = "8", 
                SearchFields = "8,מחסן משהב''ט", 
                Inactive = false, 
                LocalName = "מחסן משהב''ט", 
			});
			
            return all;
       }

	    public void MapPoco(SiteType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(SiteType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

