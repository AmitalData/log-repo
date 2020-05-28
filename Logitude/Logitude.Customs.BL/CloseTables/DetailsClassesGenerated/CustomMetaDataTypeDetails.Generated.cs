
   
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
   public class CustomMetaDataTypeDetails : CustomMetaDataType, ICloseTable<CustomMetaDataType, CustomMetaDataTypeDetails>
   {
       public List<CustomMetaDataTypeDetails> GetAll()
       {
		    var all = new List<CustomMetaDataTypeDetails>();  
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "1", 
                EnglishName = "Analysis", 
                LocalName = "ארץ היבוא", 
                SearchFields = "1,ארץ היבוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "10", 
                LocalName = "מדינה מנפיקה", 
                SearchFields = "10,מדינה מנפיקה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "100", 
                LocalName = "מזהה מטען (מפתח 1)", 
                SearchFields = "100,מזהה מטען (מפתח 1)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "101", 
                LocalName = "מזהה מטען (מפתח 2)", 
                SearchFields = "101,מזהה מטען (מפתח 2)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "102", 
                LocalName = "מזהה מטען (מפתח 3)", 
                SearchFields = "102,מזהה מטען (מפתח 3)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "103", 
                LocalName = "מספר פנקס פיזי", 
                SearchFields = "103,מספר פנקס פיזי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "104", 
                LocalName = "מזהה היתר ביטחוני", 
                SearchFields = "104,מזהה היתר ביטחוני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "105", 
                LocalName = "תאריך תעודה", 
                SearchFields = "105,תאריך תעודה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "106", 
                EnglishName = "Damage Report", 
                LocalName = "האם השכלה תיכונית", 
                SearchFields = "106,האם השכלה תיכונית", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "107", 
                LocalName = "האם תעודת השכלה על תיכונית", 
                SearchFields = "107,האם תעודת השכלה על תיכונית", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "108", 
                LocalName = "מספר זהות מעסיק", 
                SearchFields = "108,מספר זהות מעסיק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "109", 
                LocalName = "מספר זהות עובד", 
                SearchFields = "109,מספר זהות עובד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "11", 
                LocalName = "מדינת יעד", 
                SearchFields = "11,מדינת יעד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "110", 
                LocalName = "מספר זהות מועמד", 
                SearchFields = "110,מספר זהות מועמד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "111", 
                LocalName = "מספר טיסה", 
                SearchFields = "111,מספר טיסה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "112", 
                LocalName = "מועד טיסה", 
                SearchFields = "112,מועד טיסה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "119", 
                EnglishName = "Discharge Report", 
                LocalName = "דוח פריקה", 
                SearchFields = "119,דוח פריקה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "12", 
                LocalName = "מדינת יעד/מקור", 
                SearchFields = "12,מדינת יעד/מקור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "122", 
                LocalName = "דוח הטענה", 
                SearchFields = "122,דוח הטענה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "13", 
                LocalName = "מזהה אתר אחסון", 
                SearchFields = "13,מזהה אתר אחסון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "14", 
                LocalName = "מזהה יבואן", 
                SearchFields = "14,מזהה יבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "15", 
                EnglishName = "selling importer", 
                LocalName = "מזהה יבואן מוכר", 
                SearchFields = "15,מזהה יבואן מוכר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "16", 
                LocalName = "מזהה יבואן מצהיר", 
                SearchFields = "16,מזהה יבואן מצהיר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "17", 
                EnglishName = "buying importer", 
                LocalName = "מזהה יבואן רוכש", 
                SearchFields = "17,מזהה יבואן רוכש", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "18", 
                LocalName = "מזהה יצואן", 
                SearchFields = "18,מזהה יצואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "19", 
                LocalName = "מזהה יצואן/יבואן", 
                SearchFields = "19,מזהה יצואן/יבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "2", 
                LocalName = "ארץ הנפקה", 
                SearchFields = "2,ארץ הנפקה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "20", 
                LocalName = "מזהה ישות תביעה", 
                SearchFields = "20,מזהה ישות תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "21", 
                LocalName = "מזהה לקוח", 
                SearchFields = "21,מזהה לקוח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "22", 
                LocalName = "מזהה מאשר", 
                SearchFields = "22,מזהה מאשר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "23", 
                LocalName = "מזהה מוטב להחזר", 
                SearchFields = "23,מזהה מוטב להחזר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "24", 
                LocalName = "מזהה מורשה", 
                SearchFields = "24,מזהה מורשה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "25", 
                LocalName = "מזהה מנפיק החשבון (לא ח.פ)", 
                SearchFields = "25,מזהה מנפיק החשבון (לא ח.פ)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "26", 
                LocalName = "מזהה מרשה", 
                SearchFields = "26,מזהה מרשה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "27", 
                LocalName = "מזהה סוכן", 
                SearchFields = "27,מזהה סוכן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "270", 
                LocalName = "כתב הגעה צפי", 
                SearchFields = "270,כתב הגעה צפי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "271", 
                LocalName = "רשימת אריזה", 
                SearchFields = "271,רשימת אריזה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "28", 
                LocalName = "קוד משלח", 
                SearchFields = "28,קוד משלח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "29", 
                LocalName = "מזהה רואה חשבון (ח.פ)", 
                SearchFields = "29,מזהה רואה חשבון (ח.פ)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "298", 
                LocalName = "רשימת חומרים מסוכנים", 
                SearchFields = "298,רשימת חומרים מסוכנים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "3", 
                EnglishName = "invoice country", 
                LocalName = "ארץ חשבון", 
                SearchFields = "3,ארץ חשבון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "30", 
                LocalName = "מס' אילוץ", 
                SearchFields = "30,מס' אילוץ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "32", 
                LocalName = "מס' סוכן מכס", 
                SearchFields = "32,מס' סוכן מכס", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "325", 
                LocalName = "חשבון פרופורמה", 
                SearchFields = "325,חשבון פרופורמה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "336", 
                LocalName = "רשימת אנשי צוות", 
                SearchFields = "336,רשימת אנשי צוות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "35", 
                LocalName = "מס' תעודה", 
                SearchFields = "35,מס' תעודה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "36", 
                LocalName = "מספר אישור", 
                SearchFields = "36,מספר אישור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "37", 
                LocalName = "מספר דף", 
                SearchFields = "37,מספר דף", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "380", 
                LocalName = "חשבון ספק (חשבון מכר)", 
                SearchFields = "380,חשבון ספק (חשבון מכר)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "39", 
                EnglishName = "invoice number", 
                LocalName = "מספר חשבון", 
                SearchFields = "39,מספר חשבון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "4", 
                LocalName = "ארץ חשבון פרופורמה", 
                SearchFields = "4,ארץ חשבון פרופורמה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "40", 
                LocalName = "מספר חשבון להחזר", 
                SearchFields = "40,מספר חשבון להחזר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "41", 
                EnglishName = "invoice number", 
                LocalName = "מספר חשבון מכר", 
                SearchFields = "41,מספר חשבון מכר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "419", 
                LocalName = "שטר מטען יצוא אווירי", 
                SearchFields = "419,שטר מטען יצוא אווירי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "42", 
                LocalName = "מספר חשבון פרופורמה", 
                SearchFields = "42,מספר חשבון פרופורמה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "422", 
                LocalName = "הצהרת מטענים CRI", 
                SearchFields = "422,הצהרת מטענים CRI", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "43", 
                LocalName = "מספר ספק", 
                SearchFields = "43,מספר ספק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "44", 
                LocalName = "מספר קרנה", 
                SearchFields = "44,מספר קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "45", 
                LocalName = "מספר רשיון", 
                SearchFields = "45,מספר רשיון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "46", 
                LocalName = "מספר תעודה", 
                SearchFields = "46,מספר תעודה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "47", 
                LocalName = "נערב", 
                SearchFields = "47,נערב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "49", 
                LocalName = "סוג רישיון/אישור", 
                SearchFields = "49,סוג רישיון/אישור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "5", 
                LocalName = "ארץ מקור הטובין", 
                SearchFields = "5,ארץ מקור הטובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "50", 
                LocalName = "סכום ערבות", 
                SearchFields = "50,סכום ערבות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "52", 
                LocalName = "קוד פטור", 
                SearchFields = "52,קוד פטור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "53", 
                LocalName = "רשות מוסמכת", 
                SearchFields = "53,רשות מוסמכת", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "54", 
                LocalName = "שם יצרן", 
                SearchFields = "54,שם יצרן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "55", 
                EnglishName = "invoice date", 
                LocalName = "תאריך החשבון", 
                SearchFields = "55,תאריך החשבון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "56", 
                LocalName = "תאריך החשבון פרופורמה", 
                SearchFields = "56,תאריך החשבון פרופורמה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "57", 
                EnglishName = "issue date", 
                LocalName = "תאריך הנפקה", 
                SearchFields = "57,תאריך הנפקה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "58", 
                LocalName = "תאריך הנפקה (תאריך אישור המכס)", 
                SearchFields = "58,תאריך הנפקה (תאריך אישור המכס)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "6", 
                LocalName = "זיהוי הצהרת יבוא", 
                SearchFields = "6,זיהוי הצהרת יבוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "60", 
                LocalName = "תאריך תוקף", 
                SearchFields = "60,תאריך תוקף", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "61", 
                LocalName = "מספר אסמכתא", 
                SearchFields = "61,מספר אסמכתא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "62", 
                LocalName = "מספר שטר מטען", 
                SearchFields = "62,מספר שטר מטען", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "635", 
                LocalName = "תעודת עיכוב", 
                SearchFields = "635,תעודת עיכוב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "64", 
                LocalName = "מספר מכולה", 
                SearchFields = "64,מספר מכולה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "640", 
                LocalName = "פקודת מסירה", 
                SearchFields = "640,פקודת מסירה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "65", 
                LocalName = "מספר תעודת עיכוב", 
                SearchFields = "65,מספר תעודת עיכוב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "66", 
                LocalName = "מספר מצהר", 
                SearchFields = "66,מספר מצהר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "67", 
                LocalName = "זיהוי כלי תובלה", 
                SearchFields = "67,זיהוי כלי תובלה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "68", 
                LocalName = "מספר פקודת מסירה", 
                SearchFields = "68,מספר פקודת מסירה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "69", 
                EnglishName = "is document also preferential doc", 
                LocalName = "האם החשבון משמש כמסמך העדפה", 
                SearchFields = "69,האם החשבון משמש כמסמך העדפה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "7", 
                LocalName = "זיהוי הצהרת יצוא", 
                SearchFields = "7,זיהוי הצהרת יצוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "70", 
                LocalName = "האם הוארך תוקף", 
                SearchFields = "70,האם הוארך תוקף", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "700", 
                LocalName = "שטר מטען רגיל באוויר", 
                SearchFields = "700,שטר מטען רגיל באוויר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "703", 
                LocalName = "שטר מטען פנימי באוויר", 
                SearchFields = "703,שטר מטען פנימי באוויר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "704", 
                LocalName = "שטר מטען ראשי (באוויר ובים)", 
                SearchFields = "704,שטר מטען ראשי (באוויר ובים)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "705", 
                LocalName = "שטר מטען רגיל/ שטר מטען פנימי בים", 
                SearchFields = "705,שטר מטען רגיל/ שטר מטען פנימי בים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "706", 
                LocalName = "שטר מטען מקורי", 
                SearchFields = "706,שטר מטען מקורי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "707", 
                LocalName = "העתק שטר מטען", 
                SearchFields = "707,העתק שטר מטען", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "71", 
                LocalName = "האם שטר מטען משולב", 
                SearchFields = "71,האם שטר מטען משולב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "712", 
                LocalName = "מסמך הובלה ימי לא סחיר", 
                SearchFields = "712,מסמך הובלה ימי לא סחיר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "714", 
                LocalName = "שטר מטען פנימי", 
                SearchFields = "714,שטר מטען פנימי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "72", 
                LocalName = "האם רב פעמי", 
                SearchFields = "72,האם רב פעמי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "740", 
                LocalName = "2 שטר מטען רגיל", 
                SearchFields = "740,2 שטר מטען רגיל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "741", 
                LocalName = "שטר מטען מאסטר", 
                SearchFields = "741,שטר מטען מאסטר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "75", 
                LocalName = "ערכאה", 
                SearchFields = "75,ערכאה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "77", 
                LocalName = "האם בוצע עיבוד", 
                SearchFields = "77,האם בוצע עיבוד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "78", 
                LocalName = "מדינת העיבוד", 
                SearchFields = "78,מדינת העיבוד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "780", 
                LocalName = "חשבון מטענים  (חשבון הובלה)", 
                SearchFields = "780,חשבון מטענים  (חשבון הובלה)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "785", 
                LocalName = "מצהר", 
                SearchFields = "785,מצהר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "79", 
                LocalName = "מספר שלדה", 
                SearchFields = "79,מספר שלדה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "8", 
                LocalName = "ישות תביעה", 
                SearchFields = "8,ישות תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "80", 
                LocalName = "מזהה מגיש הבקשה", 
                SearchFields = "80,מזהה מגיש הבקשה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "81", 
                LocalName = "מזהה מבקש הבקשה", 
                SearchFields = "81,מזהה מבקש הבקשה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "82", 
                LocalName = "תוקף", 
                SearchFields = "82,תוקף", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "83", 
                LocalName = "סכום", 
                SearchFields = "83,סכום", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "84", 
                LocalName = "מספר בקשה לפקדון", 
                SearchFields = "84,מספר בקשה לפקדון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "85", 
                LocalName = "סוג פקדון", 
                SearchFields = "85,סוג פקדון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "86", 
                LocalName = "תאריך קבלת הוכחת ייצוא", 
                SearchFields = "86,תאריך קבלת הוכחת ייצוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "87", 
                EnglishName = "original document", 
                LocalName = "האם מסמך מקורי", 
                SearchFields = "87,האם מסמך מקורי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "88", 
                LocalName = "סוג זיהוי עבור מסמכים", 
                SearchFields = "88,סוג זיהוי עבור מסמכים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "89", 
                LocalName = "מספר רישוי הרכב", 
                SearchFields = "89,מספר רישוי הרכב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "890", 
                LocalName = "הצהרת מטענים מסוכנים", 
                SearchFields = "890,הצהרת מטענים מסוכנים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "9", 
                LocalName = "מגיש", 
                SearchFields = "9,מגיש", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "90", 
                LocalName = "מזהה חיצוני של מקח", 
                SearchFields = "90,מזהה חיצוני של מקח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "91", 
                LocalName = "מועד תפיסה", 
                SearchFields = "91,מועד תפיסה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "92", 
                LocalName = "תאריך מתן כתב ההרשאה", 
                SearchFields = "92,תאריך מתן כתב ההרשאה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "93", 
                LocalName = "מספר דרישת בטוחה", 
                SearchFields = "93,מספר דרישת בטוחה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "94", 
                LocalName = "פרט מכס", 
                SearchFields = "94,פרט מכס", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "95", 
                LocalName = "מספר היטל", 
                SearchFields = "95,מספר היטל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "96", 
                LocalName = "אינדיקצית יצואן מאושר", 
                SearchFields = "96,אינדיקצית יצואן מאושר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "97", 
                LocalName = "קוד בנק", 
                SearchFields = "97,קוד בנק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "98", 
                LocalName = "מספר כתב ערבות", 
                SearchFields = "98,מספר כתב ערבות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "99", 
                LocalName = "סוג מזהה מטען", 
                SearchFields = "99,סוג מזהה מטען", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_1", 
                LocalName = "צ'ק דוגמא", 
                SearchFields = "IL_1,צ'ק דוגמא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_100", 
                LocalName = "כתב ערבות בנקאי או פוליסת ביטוח", 
                SearchFields = "IL_100,כתב ערבות בנקאי או פוליסת ביטוח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_103", 
                LocalName = "בקשה לעדכון תוקף פיקדון", 
                SearchFields = "IL_103,בקשה לעדכון תוקף פיקדון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_104", 
                LocalName = "בקשה להחזרת פיקדון", 
                SearchFields = "IL_104,בקשה להחזרת פיקדון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_105", 
                LocalName = "בקשות לשינויים", 
                SearchFields = "IL_105,בקשות לשינויים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_11", 
                LocalName = "תצהיר יבואן תקופתי – תמונה", 
                SearchFields = "IL_11,תצהיר יבואן תקופתי – תמונה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_119", 
                LocalName = "מחירון של היצרן", 
                SearchFields = "IL_119,מחירון של היצרן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_123", 
                LocalName = "העברת בעלות", 
                SearchFields = "IL_123,העברת בעלות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_124", 
                LocalName = "תעודת מקור (יבוא)", 
                SearchFields = "IL_124,תעודת מקור (יבוא)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_125", 
                LocalName = "מסמך העדפה (יצוא)", 
                SearchFields = "IL_125,מסמך העדפה (יצוא)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_126", 
                LocalName = "תעודת שוק", 
                SearchFields = "IL_126,תעודת שוק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_128", 
                LocalName = "תעודת לשכת המסחר הירדנית", 
                SearchFields = "IL_128,תעודת לשכת המסחר הירדנית", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_129", 
                LocalName = "הצהרה ע''פ צי''ח", 
                SearchFields = "IL_129,הצהרה ע''פ צי''ח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_13", 
                LocalName = "תצהיר יבואן להצהרת יבוא (רשימון) – תמונה", 
                SearchFields = "IL_13,תצהיר יבואן להצהרת יבוא (רשימון) – תמונה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_132", 
                LocalName = "רישיון", 
                SearchFields = "IL_132,רישיון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_133", 
                LocalName = "אישור", 
                SearchFields = "IL_133,אישור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_134", 
                LocalName = "פטור מרישיון/אישור", 
                SearchFields = "IL_134,פטור מרישיון/אישור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_135", 
                LocalName = "העתק קרנה אטא", 
                SearchFields = "IL_135,העתק קרנה אטא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_136", 
                LocalName = "העתק דף קרנה", 
                SearchFields = "IL_136,העתק דף קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_138", 
                LocalName = "טופס 130", 
                SearchFields = "IL_138,טופס 130", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_139", 
                LocalName = "פטור מחשבון מכר", 
                SearchFields = "IL_139,פטור מחשבון מכר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_14", 
                LocalName = "מסמך כללי", 
                SearchFields = "IL_14,מסמך כללי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_140", 
                LocalName = "תצהיר יצואן", 
                SearchFields = "IL_140,תצהיר יצואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_141", 
                LocalName = "פוליסת ביטוח", 
                SearchFields = "IL_141,פוליסת ביטוח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_142", 
                LocalName = "הסבר על יצוא ללא תמורה", 
                SearchFields = "IL_142,הסבר על יצוא ללא תמורה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_143", 
                LocalName = "תעודת הסמכה ליצואן מאושר", 
                SearchFields = "IL_143,תעודת הסמכה ליצואן מאושר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_144", 
                LocalName = "אישור רשם החברות", 
                SearchFields = "IL_144,אישור רשם החברות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_145", 
                LocalName = "תעודת שער נמלית ליצוא", 
                SearchFields = "IL_145,תעודת שער נמלית ליצוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_147", 
                LocalName = "טופס 370 מקוצר", 
                SearchFields = "IL_147,טופס 370 מקוצר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_148", 
                LocalName = "מכתב הצהרה", 
                SearchFields = "IL_148,מכתב הצהרה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_149", 
                LocalName = "התחייבות (טופס 373)", 
                SearchFields = "IL_149,התחייבות (טופס 373)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_15", 
                LocalName = "תמונת תעודה מזהה", 
                SearchFields = "IL_15,תמונת תעודה מזהה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_17", 
                LocalName = "כתב הרשאה (ייפוי כוח) – תמונה", 
                SearchFields = "IL_17,כתב הרשאה (ייפוי כוח) – תמונה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_180", 
                LocalName = "כתב הסמכה לחתום בשם תאגיד", 
                SearchFields = "IL_180,כתב הסמכה לחתום בשם תאגיד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_182", 
                LocalName = "ייפוי כוח לעורך דין לפי סעיף 91 לחוק לשכת עורכי הדין", 
                SearchFields = "IL_182,ייפוי כוח לעורך דין לפי סעיף 91 לחוק לשכת עורכי הדין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_183", 
                LocalName = "כתב שיפוי", 
                SearchFields = "IL_183,כתב שיפוי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_184", 
                LocalName = "תעודת יורומד", 
                SearchFields = "IL_184,תעודת יורומד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_185", 
                LocalName = "תעודת אפטא", 
                SearchFields = "IL_185,תעודת אפטא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_199", 
                LocalName = "אישור בנק בגין קיום חשבון", 
                SearchFields = "IL_199,אישור בנק בגין קיום חשבון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_202", 
                LocalName = "הצהרת ערך נמוך יבוא מסחרי", 
                SearchFields = "IL_202,הצהרת ערך נמוך יבוא מסחרי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_203", 
                LocalName = "הצהרת ערך נמוך יבוא אישי", 
                SearchFields = "IL_203,הצהרת ערך נמוך יבוא אישי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_204", 
                LocalName = "קרנה", 
                SearchFields = "IL_204,קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_205", 
                LocalName = "טופס המרת משפט", 
                SearchFields = "IL_205,טופס המרת משפט", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_206", 
                LocalName = "טופס פניה לוועדת ייבוא", 
                SearchFields = "IL_206,טופס פניה לוועדת ייבוא", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_207", 
                LocalName = "מסמך מצורף לאילוץ", 
                SearchFields = "IL_207,מסמך מצורף לאילוץ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_208", 
                LocalName = "מסמך נדרש למילוי תנאי האילוץ", 
                SearchFields = "IL_208,מסמך נדרש למילוי תנאי האילוץ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_209", 
                LocalName = "חשבוניות מכירה", 
                SearchFields = "IL_209,חשבוניות מכירה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_21", 
                LocalName = "יפוי כוח (טופס 165)", 
                SearchFields = "IL_21,יפוי כוח (טופס 165)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_210", 
                LocalName = "טופס מכס 89", 
                SearchFields = "IL_210,טופס מכס 89", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_211", 
                LocalName = "טופס מכס 89 א", 
                SearchFields = "IL_211,טופס מכס 89 א", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_212", 
                LocalName = "אסמכתאות על חסר ( גרעון עצמי)", 
                SearchFields = "IL_212,אסמכתאות על חסר ( גרעון עצמי)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_22", 
                LocalName = "יפוי כח נוטריוני (לא טופס 165).", 
                SearchFields = "IL_22,יפוי כח נוטריוני (לא טופס 165).", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_23", 
                LocalName = "טופס מכס 91", 
                SearchFields = "IL_23,טופס מכס 91", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_234", 
                LocalName = "טופס מענה לדרישה לבטוחה", 
                SearchFields = "IL_234,טופס מענה לדרישה לבטוחה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_235", 
                LocalName = "כתב תביעה", 
                SearchFields = "IL_235,כתב תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_236", 
                LocalName = "בקשה לסעד זמני", 
                SearchFields = "IL_236,בקשה לסעד זמני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_237", 
                LocalName = "בקשה למתן צו עיכוב הליכי גביה", 
                SearchFields = "IL_237,בקשה למתן צו עיכוב הליכי גביה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_238", 
                LocalName = "כתב הגנה", 
                SearchFields = "IL_238,כתב הגנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_239", 
                LocalName = "תגובה לסעד זמני", 
                SearchFields = "IL_239,תגובה לסעד זמני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_24", 
                LocalName = "טופס מכס 91 א", 
                SearchFields = "IL_24,טופס מכס 91 א", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_240", 
                LocalName = "תגובה למתן צו עיכוב הליכי גביה", 
                SearchFields = "IL_240,תגובה למתן צו עיכוב הליכי גביה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_241", 
                LocalName = "שאלון", 
                SearchFields = "IL_241,שאלון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_242", 
                LocalName = "תצהיר גילוי מסמכים", 
                SearchFields = "IL_242,תצהיר גילוי מסמכים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_243", 
                LocalName = "תצהירי עדות ראשית (תובעת)", 
                SearchFields = "IL_243,תצהירי עדות ראשית (תובעת)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_244", 
                LocalName = "תצהירי עדות ראשית (נתבעת)", 
                SearchFields = "IL_244,תצהירי עדות ראשית (נתבעת)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_245", 
                LocalName = "פרוטוקול", 
                SearchFields = "IL_245,פרוטוקול", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_246", 
                LocalName = "סיכומי התובעת", 
                SearchFields = "IL_246,סיכומי התובעת", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_247", 
                LocalName = "סיכומי הנתבעת", 
                SearchFields = "IL_247,סיכומי הנתבעת", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_248", 
                LocalName = "החלטות ביניים של בית משפט", 
                SearchFields = "IL_248,החלטות ביניים של בית משפט", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_249", 
                LocalName = "הסכם פשרה", 
                SearchFields = "IL_249,הסכם פשרה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_25", 
                LocalName = "פרטי חשבון בנק להחזר", 
                SearchFields = "IL_25,פרטי חשבון בנק להחזר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_252", 
                LocalName = "כתב תביעה מעודכן", 
                SearchFields = "IL_252,כתב תביעה מעודכן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "Il_253", 
                LocalName = "בקשה לצו מניעה זמני", 
                SearchFields = "Il_253,בקשה לצו מניעה זמני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_254", 
                LocalName = "תגובה לבקשה לצו מניעה זמני", 
                SearchFields = "IL_254,תגובה לבקשה לצו מניעה זמני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_255", 
                LocalName = "דרישה לגילוי מסמכים", 
                SearchFields = "IL_255,דרישה לגילוי מסמכים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_256", 
                LocalName = "תצהיר תשובות לשאלון", 
                SearchFields = "IL_256,תצהיר תשובות לשאלון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_257", 
                LocalName = "בקשת ביניים", 
                SearchFields = "IL_257,בקשת ביניים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_258", 
                LocalName = "תשובה לבקשת ביניים", 
                SearchFields = "IL_258,תשובה לבקשת ביניים", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_26", 
                LocalName = "פרוספקט", 
                SearchFields = "IL_26,פרוספקט", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_261", 
                LocalName = "Declaration of Non-Qualifying Operation", 
                SearchFields = "IL_261,Declaration of Non-Qualifying Operation", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_262", 
                LocalName = "תעודת עיבוד מינורי", 
                SearchFields = "IL_262,תעודת עיבוד מינורי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_265", 
                LocalName = "תמונת רכב", 
                SearchFields = "IL_265,תמונת רכב", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_27", 
                LocalName = "קטלוג של היצרן", 
                SearchFields = "IL_27,קטלוג של היצרן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_271", 
                LocalName = "הרשאה לחיוב חשבון", 
                SearchFields = "IL_271,הרשאה לחיוב חשבון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_272", 
                LocalName = "מסמך בקשה מקורית  פרהרולינג", 
                SearchFields = "IL_272,מסמך בקשה מקורית  פרהרולינג", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_273", 
                LocalName = "טופס בקשה מאתר תהילה", 
                SearchFields = "IL_273,טופס בקשה מאתר תהילה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_278", 
                LocalName = "בקשה סווגה", 
                SearchFields = "IL_278,בקשה סווגה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_28", 
                LocalName = "מרכיבי הטובין", 
                SearchFields = "IL_28,מרכיבי הטובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_281", 
                LocalName = "דו''ח מעבדה", 
                SearchFields = "IL_281,דו''ח מעבדה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_282", 
                LocalName = "תמונה", 
                SearchFields = "IL_282,תמונה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_283", 
                LocalName = "קטלוג/פרוספקט", 
                SearchFields = "IL_283,קטלוג/פרוספקט", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_284", 
                LocalName = "בקשה לפקדון", 
                SearchFields = "IL_284,בקשה לפקדון", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_285", 
                LocalName = "הוכחת ייצוא קרנה", 
                SearchFields = "IL_285,הוכחת ייצוא קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "Il_286", 
                LocalName = "ספח תנועת קרנה", 
                SearchFields = "Il_286,ספח תנועת קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_289", 
                LocalName = "צילום תצהיר יבואן בטחוני", 
                SearchFields = "IL_289,צילום תצהיר יבואן בטחוני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_29", 
                LocalName = "תהליך יצור", 
                SearchFields = "IL_29,תהליך יצור", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_290", 
                LocalName = "אישור מיוחד לזכאי", 
                SearchFields = "IL_290,אישור מיוחד לזכאי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_3", 
                LocalName = "קטלוג החברה המפרט פעילות החברה ומוצריה", 
                SearchFields = "IL_3,קטלוג החברה המפרט פעילות החברה ומוצריה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_30", 
                LocalName = "מפרט טכני", 
                SearchFields = "IL_30,מפרט טכני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_300", 
                LocalName = "מק''ח", 
                SearchFields = "IL_300,מק''ח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_303", 
                LocalName = "מסמך תפיסה", 
                SearchFields = "IL_303,מסמך תפיסה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_304", 
                LocalName = "ייפוי כוח מבעל הרכב לנהג", 
                SearchFields = "IL_304,ייפוי כוח מבעל הרכב לנהג", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_306", 
                LocalName = "אישור תקינה ממשרד התחבורה", 
                SearchFields = "IL_306,אישור תקינה ממשרד התחבורה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_309", 
                LocalName = "פסק דין לפרט מכס", 
                SearchFields = "IL_309,פסק דין לפרט מכס", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_31", 
                LocalName = "הסבר יצרן על יעוד הטובין", 
                SearchFields = "IL_31,הסבר יצרן על יעוד הטובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_310", 
                LocalName = "בקשה לחקירה", 
                SearchFields = "IL_310,בקשה לחקירה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_311", 
                LocalName = "היטל", 
                SearchFields = "IL_311,היטל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_32", 
                LocalName = "הסבר יבואן על יעוד הטובין", 
                SearchFields = "IL_32,הסבר יבואן על יעוד הטובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_325", 
                LocalName = "מ.ב 71", 
                SearchFields = "IL_325,מ.ב 71", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_327", 
                LocalName = "הצהרה על אי ניכוי מס תשומות בכפל", 
                SearchFields = "IL_327,הצהרה על אי ניכוי מס תשומות בכפל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_33", 
                LocalName = "דוגמאות טובין", 
                SearchFields = "IL_33,דוגמאות טובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_331", 
                LocalName = "תצהיר מנכ''ל", 
                SearchFields = "IL_331,תצהיר מנכ''ל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_333", 
                LocalName = "עיקול בהסכמה", 
                SearchFields = "IL_333,עיקול בהסכמה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_334", 
                LocalName = "עיקול ברישום", 
                SearchFields = "IL_334,עיקול ברישום", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_336", 
                LocalName = "הודעת זיכוי למימוש ערבות", 
                SearchFields = "IL_336,הודעת זיכוי למימוש ערבות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_338", 
                LocalName = "בקשה להמרת קרנה", 
                SearchFields = "IL_338,בקשה להמרת קרנה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_339", 
                LocalName = "אישור לשכת מסחר להארכת תוקף", 
                SearchFields = "IL_339,אישור לשכת מסחר להארכת תוקף", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_34", 
                LocalName = "אישור משלח על עלות הובלה", 
                SearchFields = "IL_34,אישור משלח על עלות הובלה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_340", 
                LocalName = "אישור כניסה לארץ", 
                SearchFields = "IL_340,אישור כניסה לארץ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_341", 
                LocalName = "היתר בטחוני", 
                SearchFields = "IL_341,היתר בטחוני", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_342", 
                LocalName = "אישור קמ''ת", 
                SearchFields = "IL_342,אישור קמ''ת", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_345", 
                LocalName = "תעודת השכלה", 
                SearchFields = "IL_345,תעודת השכלה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_346", 
                LocalName = "הצהרה במינוי סוכן מכס/משלח בי''ל אחראי", 
                SearchFields = "IL_346,הצהרה במינוי סוכן מכס/משלח בי''ל אחראי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_347", 
                LocalName = "הצהרה במינוי פקיד רישוי (מ.ב. 126)", 
                SearchFields = "IL_347,הצהרה במינוי פקיד רישוי (מ.ב. 126)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_348", 
                LocalName = "יפוי כוח לקבלת מירשם פלילי", 
                SearchFields = "IL_348,יפוי כוח לקבלת מירשם פלילי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_349", 
                LocalName = "בקשה להתמנות כפקיד רישוי (מט/7410 א')", 
                SearchFields = "IL_349,בקשה להתמנות כפקיד רישוי (מט/7410 א')", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_35", 
                LocalName = "הסבר על מהות ההנחה", 
                SearchFields = "IL_35,הסבר על מהות ההנחה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_350", 
                LocalName = "תקנון החברה", 
                SearchFields = "IL_350,תקנון החברה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_351", 
                LocalName = "תעודה על רישום החברה ברשם החברות (תעודת התאגדות)", 
                SearchFields = "IL_351,תעודה על רישום החברה ברשם החברות (תעודת התאגדות)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_352", 
                LocalName = "אישור עו''ד/רו''ח לגבי בעלי המניות ומורשי החתימה בתאגיד", 
                SearchFields = "IL_352,אישור עו''ד/רו''ח לגבי בעלי המניות ומורשי החתימה בתאגיד", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_353", 
                LocalName = "אישור עו''ד/רו''ח על מורשה חתימה", 
                SearchFields = "IL_353,אישור עו''ד/רו''ח על מורשה חתימה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_354", 
                LocalName = "אישור רו''ח בדבר ניהול פנקסי חשבונות", 
                SearchFields = "IL_354,אישור רו''ח בדבר ניהול פנקסי חשבונות", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_355", 
                LocalName = "הצהרה בהצטרפות למינוי פקיד רישוי", 
                SearchFields = "IL_355,הצהרה בהצטרפות למינוי פקיד רישוי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_356", 
                LocalName = "סריקת טופס חתום ע''י המבקש", 
                SearchFields = "IL_356,סריקת טופס חתום ע''י המבקש", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_37", 
                LocalName = "אסמכתה על ערך העסקה", 
                SearchFields = "IL_37,אסמכתה על ערך העסקה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_38", 
                LocalName = "אסמכתאות על ערך העסקה בפועל", 
                SearchFields = "IL_38,אסמכתאות על ערך העסקה בפועל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_4", 
                LocalName = "מסמכי עזר", 
                SearchFields = "IL_4,מסמכי עזר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_40", 
                LocalName = "מחירון של הספק", 
                SearchFields = "IL_40,מחירון של הספק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_41", 
                LocalName = "אישור ביצוע זיכוי מטעם היצרן", 
                SearchFields = "IL_41,אישור ביצוע זיכוי מטעם היצרן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_42", 
                LocalName = "אסמכתאות על החוסר", 
                SearchFields = "IL_42,אסמכתאות על החוסר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_43", 
                LocalName = "אסמכתה לביצוע התחשבנות על חוסר", 
                SearchFields = "IL_43,אסמכתה לביצוע התחשבנות על חוסר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_44", 
                LocalName = "אישור משלח מחול", 
                SearchFields = "IL_44,אישור משלח מחול", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_45", 
                LocalName = "הצהרת אי קיזוז מע''מ", 
                SearchFields = "IL_45,הצהרת אי קיזוז מע''מ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_48", 
                LocalName = "אסמכתת אובדן", 
                SearchFields = "IL_48,אסמכתת אובדן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_49", 
                LocalName = "אישורי ליקוי או נזק", 
                SearchFields = "IL_49,אישורי ליקוי או נזק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_50", 
                LocalName = "נימוקי יבואן לאיחור בהגשת תביעה", 
                SearchFields = "IL_50,נימוקי יבואן לאיחור בהגשת תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_52", 
                LocalName = "אסמכתת השמדת טובין", 
                SearchFields = "IL_52,אסמכתת השמדת טובין", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_53", 
                LocalName = "נימוקי יבואן לאיחור מעל 6 חודשים בהגשת תביעה", 
                SearchFields = "IL_53,נימוקי יבואן לאיחור מעל 6 חודשים בהגשת תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_54", 
                LocalName = "נימוקי יבואן לאיחור מעל שנה בהגשת תביעה", 
                SearchFields = "IL_54,נימוקי יבואן לאיחור מעל שנה בהגשת תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_55", 
                LocalName = "הצהרת יבואן בשינוי שם יבואן", 
                SearchFields = "IL_55,הצהרת יבואן בשינוי שם יבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_56", 
                LocalName = "הצהרת רואה חשבון בשינוי שם יבואן", 
                SearchFields = "IL_56,הצהרת רואה חשבון בשינוי שם יבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_57", 
                LocalName = "הצהרת סוכן בשינוי שם יבואן", 
                SearchFields = "IL_57,הצהרת סוכן בשינוי שם יבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_58", 
                LocalName = "הסבר על טעות בשם היבואן", 
                SearchFields = "IL_58,הסבר על טעות בשם היבואן", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_60", 
                LocalName = "דו''ח חברת הביטוח", 
                SearchFields = "IL_60,דו''ח חברת הביטוח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_61", 
                LocalName = "דוח שמאי", 
                SearchFields = "IL_61,דוח שמאי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_63", 
                LocalName = "אישור מהנמל על  כמות הטובין שיובאו ( דו''ח טלי)", 
                SearchFields = "IL_63,אישור מהנמל על  כמות הטובין שיובאו ( דו''ח טלי)", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_64", 
                LocalName = "אישור ממחסן רשוי על כמות הטובין שהתקבלו בפועל", 
                SearchFields = "IL_64,אישור ממחסן רשוי על כמות הטובין שהתקבלו בפועל", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_67", 
                LocalName = "מכתב ערעור על החלטת בתיק תביעה", 
                SearchFields = "IL_67,מכתב ערעור על החלטת בתיק תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_68", 
                LocalName = "מכתב פניה לביטול תיק תביעה", 
                SearchFields = "IL_68,מכתב פניה לביטול תיק תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_69", 
                LocalName = "מכתב בקשת תביעה", 
                SearchFields = "IL_69,מכתב בקשת תביעה", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_70", 
                LocalName = "חשבון ביטוח", 
                SearchFields = "IL_70,חשבון ביטוח", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_81", 
                LocalName = "אחר", 
                SearchFields = "IL_81,אחר", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_83", 
                LocalName = "הצהרת אב''כ", 
                SearchFields = "IL_83,הצהרת אב''כ", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_85", 
                LocalName = "מוצג פיזי", 
                SearchFields = "IL_85,מוצג פיזי", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_9", 
                LocalName = "אישור רו''ח על ניהול פנקסים כחוק", 
                SearchFields = "IL_9,אישור רו''ח על ניהול פנקסים כחוק", 
                Inactive = false, 
			});
			 
            all.Add(new CustomMetaDataTypeDetails()
            {    
                Code = "IL_97", 
                LocalName = "פסק דין", 
                SearchFields = "IL_97,פסק דין", 
                Inactive = false, 
			});
			
            return all;
       }

	    public void MapPoco(CustomMetaDataType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;   
        }

		public string GetSearchFields(CustomMetaDataType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",",rec.Inactive,",");
        }
   }
}

