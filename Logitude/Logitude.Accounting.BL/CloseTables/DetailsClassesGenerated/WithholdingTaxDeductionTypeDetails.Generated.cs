
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Counters;

namespace Logitude.Accounting.BL
{
   public class WithholdingTaxDeductionTypeDetails : WithholdingTaxDeductionType, ICloseTable<WithholdingTaxDeductionType, WithholdingTaxDeductionTypeDetails>
   {
       public List<WithholdingTaxDeductionTypeDetails> GetAll()
       {
		    var all = new List<WithholdingTaxDeductionTypeDetails>();  
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
              Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "01", 
                SearchFields = "01,מקבלי ריבית והפרשי הצמדה, תשלומים בעד דמי השאלה, תשלומים תמורת מכירת נייר ערך זר.,", 
                Inactive = false, 
                EnglishName = " ",
                Tenant = 0,
                LocalName = "מקבלי ריבית והפרשי הצמדה, תשלומים בעד דמי השאלה, תשלומים תמורת מכירת נייר ערך זר.", 
			});
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "02", 
                SearchFields = "02,מקבלי עמלות ביטוח.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מקבלי עמלות ביטוח.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "03", 
                SearchFields = "03,מקבלי שכר סופרים, אמנים, בוחנים, מרצים, מעניקי שרותי משרד וספורטאים שאינם שכירים, דירקטורים.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מקבלי שכר סופרים, אמנים, בוחנים, מרצים, מעניקי שרותי משרד וספורטאים שאינם שכירים, דירקטורים.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "05", 
                SearchFields = "05,תשלומים בעד עבודה חקלאית או תוצרת חקלאית,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלומים בעד עבודה חקלאית או תוצרת חקלאית",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "06", 
                SearchFields = "06,מקבלי תשלומים בעד שירותים,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מקבלי תשלומים בעד שירותים",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "11", 
                SearchFields = "11,תשלום שלא כדין מקופת גמל.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלום שלא כדין מקופת גמל.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "12", 
                SearchFields = "12,החזר תשלום למעביד מקופת גמל לפיצויים.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "החזר תשלום למעביד מקופת גמל לפיצויים.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "13", 
                SearchFields = "13,תשלומים בעד שכירות מקרקעין שניתן לתבוע כהוצאה.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלומים בעד שכירות מקרקעין שניתן לתבוע כהוצאה.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "14", 
                SearchFields = "14,תשלום מקרן השתלמות לעצמאי.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלום מקרן השתלמות לעצמאי.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "15", 
                SearchFields = "15,תשלומים מהשתכרות או רווח שמקורם בהימורים, הגרלות ופעילות נושאת פרסים.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלומים מהשתכרות או רווח שמקורם בהימורים, הגרלות ופעילות נושאת פרסים.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "16", 
                SearchFields = "16,ניכוי מתשלומים בעד מטבעות וירטואליים,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "ניכוי מתשלומים בעד מטבעות וירטואליים",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "18", 
                SearchFields = "18,תשלום דיבידנד,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלום דיבידנד",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "19", 
                SearchFields = "19,רווח הון מפדיון מניות/ אופציות לפי סעיף 102.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "רווח הון מפדיון מניות/ אופציות לפי סעיף 102.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "20", 
                SearchFields = "20,סוג זה מיוחד אך ורק לתיק המוסד לביטוח לאומי לצורך דיווח תשלומי גמלאות וקצבאות.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "סוג זה מיוחד אך ורק לתיק המוסד לביטוח לאומי לצורך דיווח תשלומי גמלאות וקצבאות.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "21", 
                SearchFields = "21,הכנסה מהפקת חשמל במסלול פטור,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "הכנסה מהפקת חשמל במסלול פטור",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "22", 
                SearchFields = "22,הכנסה מהפקת חשמל במסלול מס מופחת,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "הכנסה מהפקת חשמל במסלול מס מופחת",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "53", 
                SearchFields = "53,החזרת סכומים מקופת הגמל למעסיק.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "החזרת סכומים מקופת הגמל למעסיק.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "54", 
                SearchFields = "54,החזרת סכומים מקופת הגמל למעסיק לפי אישור פקיד שומה.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "החזרת סכומים מקופת הגמל למעסיק לפי אישור פקיד שומה.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "56", 
                SearchFields = "56,מענק פרישה פטור ממס,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מענק פרישה פטור ממס",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "57", 
                SearchFields = "57,מענק פרישה עקב מוות פטור ממס,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מענק פרישה עקב מוות פטור ממס",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "58", 
                SearchFields = "58,מענק פרישה חייב במס,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מענק פרישה חייב במס",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "59", 
                SearchFields = "59,מענק פרישה עקב מוות חייב במס,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מענק פרישה עקב מוות חייב במס",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "60", 
                SearchFields = "60,מענק פרישה שחויב בשווי בעת ההפקדה (פטור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "מענק פרישה שחויב בשווי בעת ההפקדה (פטור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "61", 
                SearchFields = "61,רווחים צבורים מהפקדות שחויבו בשווי בעת ההפקדה (15%).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "רווחים צבורים מהפקדות שחויבו בשווי בעת ההפקדה (15%).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "63", 
                SearchFields = "63,משיכה של קרן ורווחים מקופת הגמל, שניתן למשוך בפטור,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכה של קרן ורווחים מקופת הגמל, שניתן למשוך בפטור",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "64", 
                SearchFields = "64,משיכה מקופת הגמל בניגוד להוראות סעיף 87 לפקודה,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכה מקופת הגמל בניגוד להוראות סעיף 87 לפקודה",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "65", 
                SearchFields = "65,משיכה מקופת הגמל בעילה של העדר הכנסות (פטור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכה מקופת הגמל בעילה של העדר הכנסות (פטור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "66", 
                SearchFields = "66,משיכה אחרת מקופת הגמל באישור פקיד שומה.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכה אחרת מקופת הגמל באישור פקיד שומה.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "67", 
                SearchFields = "67,משיכה מקופת הגמל בעילה סוציאלית ולפי אישור פקיד שומה.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכה מקופת הגמל בעילה סוציאלית ולפי אישור פקיד שומה.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "68", 
                SearchFields = "68,היוון קצבה מזכה על פי אישור פש,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "היוון קצבה מזכה על פי אישור פש",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "69", 
                SearchFields = "69,היוון קצבה בפטור על פי אישור פש,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "היוון קצבה בפטור על פי אישור פש",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "70", 
                SearchFields = "70,היוון סכום צבירה מזערי על פי אישור פקיד שומה (פטור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "היוון סכום צבירה מזערי על פי אישור פקיד שומה (פטור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "74", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת קרן ורווחים שמקורם ב-הפקדה מוטבת- לשם השתלמות או הגעה ל\''גיל פרישה (פטור).", 
                SearchFields = "74,קרן השתלמות - משיכת קרן ורווחים שמקורם ב-הפקדה מוטבת- לשם השתלמות או הגעה ל\''גיל פרישה (פטור).", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "75", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת \''סכומים מקרן השתלמות\'' כהגדרתם בתקנות מקרן השתלמות לשכירים.", 
                SearchFields = "74,קרן השתלמות - משיכת \''סכומים מקרן השתלמות\'' כהגדרתם בתקנות מקרן השתלמות לשכירים.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "76", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת \''סכומים מקרן השתלמות\'' כהגדרתם בתקנות מקרן השתלמות לעצמאים.", 
                SearchFields = "76,קרן השתלמות - משיכת \''סכומים מקרן השתלמות\'' כהגדרתם בתקנות מקרן השתלמות לעצמאים.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "77", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת סכומים שמועטו מהגדרת \''סכומים מקרן השתלמות\'' (פטור).", 
                SearchFields = "77,קרן השתלמות - משיכת סכומים שמועטו מהגדרת \''סכומים מקרן השתלמות\'' (פטור).", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {    
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "78", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת הפקדה שאינה \''הפקדה מוטבת\'' בתוספת הפרשי הצמדה (פטור) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                SearchFields = "78,קרן השתלמות - משיכת הפקדה שאינה \''הפקדה מוטבת\'' בתוספת הפרשי הצמדה (פטור) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "79", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה \''הפקדה מוטבת\'' (15%) בהתאם לסעיף 3(ה4)(1)לפקודה", 
                SearchFields = "79,קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה \''הפקדה מוטבת\'' (15%) בהתאם לסעיף 3(ה4)(1)לפקודה", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "80", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה \''הפקדה מוטבת\'' (20%) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                SearchFields = "80,קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה \''הפקדה מוטבת\'' (20%) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "81", 
                EnglishName = " ", 
                LocalName = "קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה ''הפקדה מוטבת'' (25%) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                SearchFields = "81,קרן השתלמות - משיכת רווחים צבורים מהפקדה שאינה ''הפקדה מוטבת'' (25%) בהתאם לסעיף 3(ה4)(1) לפקודה.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "82", 
                SearchFields = "82,העברה לחשבונו של בן הזוג לשעבר - העברת סכומים שמתחת לתקרה (פטור).,", 
                Inactive = false,
                Tenant = 0,
                EnglishName = " ", 
                LocalName = "העברה לחשבונו של בן הזוג לשעבר - העברת סכומים שמתחת לתקרה (פטור).", 
			});
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "83", 
                SearchFields = "83,העברה לחשבונו של בן הזוג לשעבר - העברת סכומים שמעל לתקרה (סעיף 124ג).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "העברה לחשבונו של בן הזוג לשעבר - העברת סכומים שמעל לתקרה (סעיף 124ג).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
              Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "85", 
                SearchFields = "85,משיכת סכומים בגין ימי חופשה, מחלה או חג במהלך תקופת עבודה (25%/40%).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכת סכומים בגין ימי חופשה, מחלה או חג במהלך תקופת עבודה (25%/40%).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "86", 
                SearchFields = "86,משיכת סכומים בגין ימי חופשה, מחלה או חג במועד סיום עבודה (40%/25%).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "משיכת סכומים בגין ימי חופשה, מחלה או חג במועד סיום עבודה (40%/25%).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "87", 
                SearchFields = "87,תשלום מחשבון חדש או קופת גמל להשקעה - קרן כולל הפרשי הצמדה על הקרן (פטור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלום מחשבון חדש או קופת גמל להשקעה - קרן כולל הפרשי הצמדה על הקרן (פטור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "88", 
                LocalName = "תשלום מחשבון חדש או קופת גמל להשקעה - \''ריבית ורווחים אחרים\'' (25%).", 
                EnglishName = " ", 
                SearchFields = "88,תשלום מחשבון חדש או קופת גמל להשקעה - \''ריבית ורווחים אחרים\'' (25%).",
                Tenant = 0,
                Inactive = false, 
			});
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "91", 
                SearchFields = "91,קופת גמל פנסיה לעצמאים - משיכת סכומים במצב אבטלה (פטור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "קופת גמל פנסיה לעצמאים - משיכת סכומים במצב אבטלה (פטור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "92", 
                SearchFields = "92,קופת גמל פנסיה לעצמאים - משיכת סכומים במצב אבטלה (לפי אישור).,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "קופת גמל פנסיה לעצמאים - משיכת סכומים במצב אבטלה (לפי אישור).",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "95", 
                SearchFields = "95,תשלום חד פעמי בגין אובדן כושר עבודה - סכום הקצבאות ששולמו בסכום חד פעמי.,", 
                Inactive = false, 
                EnglishName = " ", 
                LocalName = "תשלום חד פעמי בגין אובדן כושר עבודה - סכום הקצבאות ששולמו בסכום חד פעמי.",
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "07", 
                EnglishName = " ", 
                LocalName = "תשלומים לתושב חוץ לפי סעיף 170 לפקודה כשהמס נוכה והועבר לפקיד השומה ע\''י המנכה.", 
                SearchFields = "07,תשלומים לתושב חוץ לפי סעיף 170 לפקודה כשהמס נוכה והועבר לפקיד השומה ע\''י המנכה.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "08", 
                EnglishName = " ", 
                LocalName = "תשלומים לתושב חוץ לפי סעיף 170 לפקודה כשהמס נוכה והועבר לפקיד השומה ע\''י הבנק.", 
                SearchFields = "08,תשלומים לתושב חוץ לפי סעיף 170 לפקודה כשהמס נוכה והועבר לפקיד השומה ע\''י הבנק.", 
                Inactive = false,
                Tenant = 0,
            });
			 
            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "71", 
                EnglishName = " ", 
                LocalName = "היוון קצבה מוכרת, חלק \''תשלומים פטורים\'' (פטור).", 
                SearchFields = "71,היוון קצבה מוכרת, חלק \''תשלומים פטורים\'' (פטור).", 
                Inactive = false,
                Tenant = 0,
            });

            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
               Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0),
                Code = "72",
                EnglishName = " ",
                LocalName = "היוון קצבה מוכרת, חלק \''מרכיב הרווח היחסי\'' (15%).",
                SearchFields = "72,היוון קצבה מוכרת, חלק \''מרכיב הרווח היחסי\'' (15%).",
                Inactive = false,
                Tenant = 0,
			});

            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0) ,
                Code = "73",
                EnglishName = " ",
                LocalName = "קרן השתלמות - משיכת קרן ורווחים שמקורם ב\"הפקדה מוטבת\" (פטור).",
                SearchFields = "73,קרן השתלמות - משיכת קרן ורווחים שמקורם ב\"הפקדה מוטבת\" (פטור).",
                Inactive = false,
                Tenant = 0,
            });


            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0) ,
                Code = "96",
                EnglishName = " ",
                LocalName = "תשלום חד פעמי בגין אובדן כושר עבודה - החזר הוצאות שכ\"ט והוצאות משפטיות.",
                SearchFields = "96,תשלום חד פעמי בגין אובדן כושר עבודה - החזר הוצאות שכ\"ט והוצאות משפטיות.",
                Inactive = false,
                Tenant = 0,
            });

            all.Add(new WithholdingTaxDeductionTypeDetails()
            {
                Id =  IdCounter.GetNumber("WithholdingTaxDeductionType", 0) ,
                Code = "97",
                EnglishName = " ",
                LocalName = "תשלום חד פעמי בגין אובדן כושר עבודה - החזר פרמיות",
                SearchFields = "97,תשלום חד פעמי בגין אובדן כושר עבודה - החזר פרמיות",
                Inactive = false,
                Tenant = 0,
            });
            return all;
       }

	    public void MapPoco(WithholdingTaxDeductionType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;
            if (newPoco.Id == null)
            newPoco.Id = this.Id;
            newPoco.Tenant = this.Tenant;
        }

		public string GetSearchFields(WithholdingTaxDeductionType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

