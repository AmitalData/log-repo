
   
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
   public class CustomsBranchDetails : CustomsBranch, ICloseTable<CustomsBranch, CustomsBranchDetails>
   {
       public List<CustomsBranchDetails> GetAll()
       {
		    var all = new List<CustomsBranchDetails>();  
            all.Add(new CustomsBranchDetails()
            {    
                Code = "0", 
                SearchFields = "0,בנק אמריקאי ישראלי", 
                Inactive = false, 
                LocalName = "בנק אמריקאי ישראלי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "1", 
                EnglishName = "main", 
                SearchFields = "1,משרד ראשי", 
                Inactive = false, 
                LocalName = "משרד ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "10", 
                SearchFields = "10,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "100", 
                SearchFields = "100,שדרות רוטשילד", 
                Inactive = false, 
                LocalName = "שדרות רוטשילד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "101", 
                SearchFields = "101,המלך שלמה", 
                Inactive = false, 
                LocalName = "המלך שלמה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "102", 
                SearchFields = "102,סניף פנימי", 
                Inactive = false, 
                LocalName = "סניף פנימי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "103", 
                SearchFields = "103,נאות שושנים", 
                Inactive = false, 
                LocalName = "נאות שושנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "104", 
                SearchFields = "104,העצמאות", 
                Inactive = false, 
                LocalName = "העצמאות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "105", 
                SearchFields = "105,הדר חיפה", 
                Inactive = false, 
                LocalName = "הדר חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "106", 
                SearchFields = "106,''באר שבע-פלמ''''ח", 
                Inactive = false, 
                LocalName = "באר שבע-פלמ''''ח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "107", 
                SearchFields = "107,מוריה", 
                Inactive = false, 
                LocalName = "מוריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "108", 
                SearchFields = "108,גונן", 
                Inactive = false, 
                LocalName = "גונן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "109", 
                SearchFields = "109,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "11", 
                SearchFields = "11,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "110", 
                SearchFields = "110,נצרת", 
                Inactive = false, 
                LocalName = "נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "111", 
                SearchFields = "111,משרד ראשי", 
                Inactive = false, 
                LocalName = "משרד ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "112", 
                SearchFields = "112,דניה", 
                Inactive = false, 
                LocalName = "דניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "113", 
                SearchFields = "113,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "114", 
                SearchFields = "114,נס ציונה", 
                Inactive = false, 
                LocalName = "נס ציונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "115", 
                SearchFields = "115,יד חרוצים", 
                Inactive = false, 
                LocalName = "יד חרוצים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "116", 
                SearchFields = "116,אור עקיבא", 
                Inactive = false, 
                LocalName = "אור עקיבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "117", 
                SearchFields = "117,פרדס חנה", 
                Inactive = false, 
                LocalName = "פרדס חנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "118", 
                SearchFields = "118,קרית שמונה", 
                Inactive = false, 
                LocalName = "קרית שמונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "119", 
                SearchFields = "119,מודיעין", 
                Inactive = false, 
                LocalName = "מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "12", 
                SearchFields = "12,אילת", 
                Inactive = false, 
                LocalName = "אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "120", 
                SearchFields = "120,כפר ורדים", 
                Inactive = false, 
                LocalName = "כפר ורדים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "121", 
                SearchFields = "121,שלוחת מודיעין", 
                Inactive = false, 
                LocalName = "שלוחת מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "122", 
                SearchFields = "122,פסגת זאב", 
                Inactive = false, 
                LocalName = "פסגת זאב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "123", 
                SearchFields = "123,דרת", 
                Inactive = false, 
                LocalName = "דרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "124", 
                SearchFields = "124,מרכז גילת", 
                Inactive = false, 
                LocalName = "מרכז גילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "125", 
                SearchFields = "125,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "126", 
                SearchFields = "126,רמת שאול", 
                Inactive = false, 
                LocalName = "רמת שאול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "127", 
                SearchFields = "127,אביבים", 
                Inactive = false, 
                LocalName = "אביבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "128", 
                SearchFields = "128,מודיעין", 
                Inactive = false, 
                LocalName = "מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "129", 
                SearchFields = "129,מרכז בנקאות פרטית בינלאומית", 
                Inactive = false, 
                LocalName = "מרכז בנקאות פרטית בינלאומית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "13", 
                SearchFields = "13,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "130", 
                SearchFields = "130,מת''ם", 
                Inactive = false, 
                LocalName = "מת''ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "131", 
                SearchFields = "131,יוקנעם", 
                Inactive = false, 
                LocalName = "יוקנעם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "132", 
                SearchFields = "132,''תה''''ש", 
                Inactive = false, 
                LocalName = "תה''''ש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "133", 
                SearchFields = "133,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "134", 
                SearchFields = "134,נתב''ג", 
                Inactive = false, 
                LocalName = "נתב''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "135", 
                SearchFields = "135,משרד הבטחון", 
                Inactive = false, 
                LocalName = "משרד הבטחון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "136", 
                SearchFields = "136,קיראון", 
                Inactive = false, 
                LocalName = "קיראון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "137", 
                SearchFields = "137,מגדל העמק", 
                Inactive = false, 
                LocalName = "מגדל העמק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "138", 
                SearchFields = "138,קרית ים", 
                Inactive = false, 
                LocalName = "קרית ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "139", 
                SearchFields = "139,בני ברק", 
                Inactive = false, 
                LocalName = "בני ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "14", 
                SearchFields = "14,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "140", 
                SearchFields = "140,שד' בן גוריון", 
                Inactive = false, 
                LocalName = "שד' בן גוריון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "141", 
                SearchFields = "141,בן-נון", 
                Inactive = false, 
                LocalName = "בן-נון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "142", 
                SearchFields = "142,''רש''''י", 
                Inactive = false, 
                LocalName = "רש''''י", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "143", 
                SearchFields = "143,אלעד", 
                Inactive = false, 
                LocalName = "אלעד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "144", 
                SearchFields = "144,נווה סביון", 
                Inactive = false, 
                LocalName = "נווה סביון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "145", 
                SearchFields = "145,ראשל''צ מערב", 
                Inactive = false, 
                LocalName = "ראשל''צ מערב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "146", 
                SearchFields = "146,קרית גת", 
                Inactive = false, 
                LocalName = "קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "147", 
                SearchFields = "147,בית שמש", 
                Inactive = false, 
                LocalName = "בית שמש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "148", 
                SearchFields = "148,רימונים", 
                Inactive = false, 
                LocalName = "רימונים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "149", 
                SearchFields = "149,הנהלה ראשית", 
                Inactive = false, 
                LocalName = "הנהלה ראשית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "15", 
                SearchFields = "15,כפר סבא", 
                Inactive = false, 
                LocalName = "כפר סבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "150", 
                SearchFields = "150,קניון הנגב", 
                Inactive = false, 
                LocalName = "קניון הנגב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "151", 
                SearchFields = "151,יד אליהו", 
                Inactive = false, 
                LocalName = "יד אליהו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "152", 
                SearchFields = "152,הוד השרון", 
                Inactive = false, 
                LocalName = "הוד השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "153", 
                SearchFields = "153,כפר גנים", 
                Inactive = false, 
                LocalName = "כפר גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "154", 
                SearchFields = "154,תל אביב", 
                Inactive = false, 
                LocalName = "תל אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "155", 
                SearchFields = "155,רמת השרון", 
                Inactive = false, 
                LocalName = "רמת השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "156", 
                SearchFields = "156,כפר גנים", 
                Inactive = false, 
                LocalName = "כפר גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "157", 
                SearchFields = "157,התעשיה חולון", 
                Inactive = false, 
                LocalName = "התעשיה חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "158", 
                SearchFields = "158,המושבה הגרמנית", 
                Inactive = false, 
                LocalName = "המושבה הגרמנית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "159", 
                SearchFields = "159,תל - אביב עסקים", 
                Inactive = false, 
                LocalName = "תל - אביב עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "16", 
                SearchFields = "16,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "160", 
                SearchFields = "160,נווה זאב", 
                Inactive = false, 
                LocalName = "נווה זאב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "161", 
                SearchFields = "161,תלמי מנשה", 
                Inactive = false, 
                LocalName = "תלמי מנשה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "162", 
                SearchFields = "162,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "163", 
                SearchFields = "163,ככר מנורה", 
                Inactive = false, 
                LocalName = "ככר מנורה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "164", 
                SearchFields = "164,גבעת מרדכי", 
                Inactive = false, 
                LocalName = "גבעת מרדכי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "165", 
                SearchFields = "165,כפר סבא", 
                Inactive = false, 
                LocalName = "כפר סבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "166", 
                SearchFields = "166,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "167", 
                SearchFields = "167,נתניה עסקים", 
                Inactive = false, 
                LocalName = "נתניה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "168", 
                SearchFields = "168,נהריה עסקים", 
                Inactive = false, 
                LocalName = "נהריה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "169", 
                SearchFields = "169,המפרץ עסקים", 
                Inactive = false, 
                LocalName = "המפרץ עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "17", 
                SearchFields = "17,קרית גת", 
                Inactive = false, 
                LocalName = "קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "170", 
                SearchFields = "170,הסניף הראשי", 
                Inactive = false, 
                LocalName = "הסניף הראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "171", 
                SearchFields = "171,בת ים", 
                Inactive = false, 
                LocalName = "בת ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "172", 
                SearchFields = "172,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "173", 
                SearchFields = "173,נתיבות", 
                Inactive = false, 
                LocalName = "נתיבות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "174", 
                SearchFields = "174,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "175", 
                SearchFields = "175,איירפורט סיטי עסקים", 
                Inactive = false, 
                LocalName = "איירפורט סיטי עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "176", 
                SearchFields = "176,רמת גן עסקים", 
                Inactive = false, 
                LocalName = "רמת גן עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "177", 
                SearchFields = "177,באר-שבע עסקים", 
                Inactive = false, 
                LocalName = "באר-שבע עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "178", 
                SearchFields = "178,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "179", 
                SearchFields = "179,רמת בית שמש", 
                Inactive = false, 
                LocalName = "רמת בית שמש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "18", 
                SearchFields = "18,רוטשילד", 
                Inactive = false, 
                LocalName = "רוטשילד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "180", 
                SearchFields = "180,סניף מרכזי", 
                Inactive = false, 
                LocalName = "סניף מרכזי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "181", 
                SearchFields = "181,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "182", 
                SearchFields = "182,שלומציון", 
                Inactive = false, 
                LocalName = "שלומציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "183", 
                SearchFields = "183,רבי עקיבא", 
                Inactive = false, 
                LocalName = "רבי עקיבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "184", 
                SearchFields = "184,מרכז מזומנים", 
                Inactive = false, 
                LocalName = "מרכז מזומנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "185", 
                SearchFields = "185,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "186", 
                SearchFields = "186,שילה", 
                Inactive = false, 
                LocalName = "שילה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "187", 
                SearchFields = "187,שכר דירה מרחב ירושלים", 
                Inactive = false, 
                LocalName = "שכר דירה מרחב ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "188", 
                SearchFields = "188,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "189", 
                SearchFields = "189,''התחנה המרכזית החדשה - ת''''א", 
                Inactive = false, 
                LocalName = "התחנה המרכזית החדשה - ת''''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "19", 
                SearchFields = "19,גאולה", 
                Inactive = false, 
                LocalName = "גאולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "190", 
                SearchFields = "190,סניף הים", 
                Inactive = false, 
                LocalName = "סניף הים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "191", 
                SearchFields = "191,תל אביב", 
                Inactive = false, 
                LocalName = "תל אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "192", 
                SearchFields = "192,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "193", 
                SearchFields = "193,מרכז בילדינג", 
                Inactive = false, 
                LocalName = "מרכז בילדינג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "194", 
                SearchFields = "194,ככר העצמאות", 
                Inactive = false, 
                LocalName = "ככר העצמאות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "195", 
                SearchFields = "195,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "196", 
                SearchFields = "196,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "197", 
                SearchFields = "197,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "198", 
                SearchFields = "198,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "199", 
                SearchFields = "199,רמת אשכול", 
                Inactive = false, 
                LocalName = "רמת אשכול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "2", 
                SearchFields = "2,באר שבע מרכז הנגב", 
                Inactive = false, 
                LocalName = "באר שבע מרכז הנגב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "20", 
                SearchFields = "20,עליה", 
                Inactive = false, 
                LocalName = "עליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "200", 
                SearchFields = "200,דטצך", 
                Inactive = false, 
                LocalName = "דטצך", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "201", 
                SearchFields = "201,מרכזי", 
                Inactive = false, 
                LocalName = "מרכזי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "202", 
                SearchFields = "202,אבן-גבירול", 
                Inactive = false, 
                LocalName = "אבן-גבירול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "203", 
                SearchFields = "203,בן-יהודה", 
                Inactive = false, 
                LocalName = "בן-יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "204", 
                SearchFields = "204,סניף הירדן", 
                Inactive = false, 
                LocalName = "סניף הירדן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "205", 
                SearchFields = "205,השגרירות האמריקאית", 
                Inactive = false, 
                LocalName = "השגרירות האמריקאית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "206", 
                SearchFields = "206,רמת גן", 
                Inactive = false, 
                LocalName = "רמת גן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "207", 
                SearchFields = "207,אחד העם הרצליה", 
                Inactive = false, 
                LocalName = "אחד העם הרצליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "208", 
                SearchFields = "208,בית התעשיה", 
                Inactive = false, 
                LocalName = "בית התעשיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "209", 
                SearchFields = "209,נורדאו", 
                Inactive = false, 
                LocalName = "נורדאו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "21", 
                SearchFields = "21,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "210", 
                SearchFields = "210,אילת", 
                Inactive = false, 
                LocalName = "אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "211", 
                SearchFields = "211,הלל", 
                Inactive = false, 
                LocalName = "הלל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "212", 
                SearchFields = "212,רחביה", 
                Inactive = false, 
                LocalName = "רחביה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "213", 
                SearchFields = "213,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "214", 
                SearchFields = "214,רחוב יפו", 
                Inactive = false, 
                LocalName = "רחוב יפו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "216", 
                SearchFields = "216,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "217", 
                SearchFields = "217,קרית גת", 
                Inactive = false, 
                LocalName = "קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "218", 
                SearchFields = "218,בת ים", 
                Inactive = false, 
                LocalName = "בת ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "219", 
                SearchFields = "219,''ירושלים,גאולה", 
                Inactive = false, 
                LocalName = ""ירושלים,גאולה"", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "22", 
                SearchFields = "22,קיבוץ געש", 
                Inactive = false, 
                LocalName = "קיבוץ געש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "221", 
                SearchFields = "221,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "222", 
                SearchFields = "222,הבנק הבינלאומי", 
                Inactive = false, 
                LocalName = "הבנק הבינלאומי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "224", 
                SearchFields = "224,קריון", 
                Inactive = false, 
                LocalName = "קריון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "225", 
                SearchFields = "225,דקלים", 
                Inactive = false, 
                LocalName = "דקלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "226", 
                SearchFields = "226,רננים", 
                Inactive = false, 
                LocalName = "רננים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "227", 
                SearchFields = "227,עכו", 
                Inactive = false, 
                LocalName = "עכו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "228", 
                SearchFields = "228,מצפה ספיר", 
                Inactive = false, 
                LocalName = "מצפה ספיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "229", 
                SearchFields = "229,להב", 
                Inactive = false, 
                LocalName = "להב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "23", 
                SearchFields = "23,ראש העין", 
                Inactive = false, 
                LocalName = "ראש העין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "230", 
                SearchFields = "230,רותם", 
                Inactive = false, 
                LocalName = "רותם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "231", 
                SearchFields = "231,אילן", 
                Inactive = false, 
                LocalName = "אילן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "234", 
                SearchFields = "234,ראשון לציון", 
                Inactive = false, 
                LocalName = "ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "235", 
                SearchFields = "235,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "236", 
                SearchFields = "236,חדרה", 
                Inactive = false, 
                LocalName = "חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "237", 
                SearchFields = "237,הרצליה", 
                Inactive = false, 
                LocalName = "הרצליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "238", 
                SearchFields = "238,טבריה", 
                Inactive = false, 
                LocalName = "טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "24", 
                SearchFields = "24,לוד", 
                Inactive = false, 
                LocalName = "לוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "240", 
                SearchFields = "240,האירוסים", 
                Inactive = false, 
                LocalName = "האירוסים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "241", 
                SearchFields = "241,רמת גן", 
                Inactive = false, 
                LocalName = "רמת גן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "243", 
                SearchFields = "243,רמז", 
                Inactive = false, 
                LocalName = "רמז", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "245", 
                SearchFields = "245,עפולה", 
                Inactive = false, 
                LocalName = "עפולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "246", 
                SearchFields = "246,סניף טבריה", 
                Inactive = false, 
                LocalName = "סניף טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "247", 
                SearchFields = "247,הנדיב", 
                Inactive = false, 
                LocalName = "הנדיב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "248", 
                SearchFields = "248,סוקולוב", 
                Inactive = false, 
                LocalName = "סוקולוב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "25", 
                SearchFields = "25,קרית ים", 
                Inactive = false, 
                LocalName = "קרית ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "251", 
                SearchFields = "251,תל אביב", 
                Inactive = false, 
                LocalName = "תל אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "252", 
                SearchFields = "252,סניף ירושלים", 
                Inactive = false, 
                LocalName = "סניף ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "253", 
                SearchFields = "253,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "254", 
                SearchFields = "254,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "255", 
                SearchFields = "255,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "256", 
                SearchFields = "256,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "257", 
                SearchFields = "257,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "258", 
                SearchFields = "258,הדר", 
                Inactive = false, 
                LocalName = "הדר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "26", 
                SearchFields = "26,גאולה", 
                Inactive = false, 
                LocalName = "גאולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "260", 
                SearchFields = "260,מונטיפיורי", 
                Inactive = false, 
                LocalName = "מונטיפיורי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "261", 
                SearchFields = "261,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "262", 
                SearchFields = "262,בני ברק", 
                Inactive = false, 
                LocalName = "בני ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "266", 
                SearchFields = "266,אקספרס בני ברק", 
                Inactive = false, 
                LocalName = "אקספרס בני ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "269", 
                SearchFields = "269,אקספרס אשדוד", 
                Inactive = false, 
                LocalName = "אקספרס אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "27", 
                SearchFields = "27,עכו", 
                Inactive = false, 
                LocalName = "עכו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "275", 
                SearchFields = "275,כיכר המדינה", 
                Inactive = false, 
                LocalName = "כיכר המדינה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "276", 
                SearchFields = "276,רמת השרון", 
                Inactive = false, 
                LocalName = "רמת השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "277", 
                SearchFields = "277,מלחה", 
                Inactive = false, 
                LocalName = "מלחה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "278", 
                SearchFields = "278,ראשון לציון", 
                Inactive = false, 
                LocalName = "ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "279", 
                SearchFields = "279,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "28", 
                SearchFields = "28,''בית אבות ''''לב גנים", 
                Inactive = false, 
                LocalName = "בית אבות ''''לב גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "280", 
                SearchFields = "280,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "281", 
                SearchFields = "281,אילת", 
                Inactive = false, 
                LocalName = "אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "283", 
                SearchFields = "283,רמת החייל", 
                Inactive = false, 
                LocalName = "רמת החייל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "284", 
                SearchFields = "284,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "285", 
                SearchFields = "285,אקספרס פתח- תקוה", 
                Inactive = false, 
                LocalName = "אקספרס פתח- תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "286", 
                SearchFields = "286,אקספרס אריאל", 
                Inactive = false, 
                LocalName = "אקספרס אריאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "287", 
                SearchFields = "287,תל אביב (ראשי)", 
                Inactive = false, 
                LocalName = "תל אביב (ראשי)", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "288", 
                SearchFields = "288,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "289", 
                SearchFields = "289,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "29", 
                SearchFields = "29,קרית מלאכי", 
                Inactive = false, 
                LocalName = "קרית מלאכי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "290", 
                SearchFields = "290,אקספרס ביתר עילית", 
                Inactive = false, 
                LocalName = "אקספרס ביתר עילית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "291", 
                SearchFields = "291,אקספרס מעלה אדומים", 
                Inactive = false, 
                LocalName = "אקספרס מעלה אדומים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "292", 
                SearchFields = "292,אקספרס מודיעין עילית", 
                Inactive = false, 
                LocalName = "אקספרס מודיעין עילית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "293", 
                SearchFields = "293,אקספרס קרית אתא", 
                Inactive = false, 
                LocalName = "אקספרס קרית אתא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "295", 
                SearchFields = "295,בינלאומי קול", 
                Inactive = false, 
                LocalName = "בינלאומי קול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "296", 
                SearchFields = "296,אקספרס המכללה למנהל", 
                Inactive = false, 
                LocalName = "אקספרס המכללה למנהל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "297", 
                SearchFields = "297,יחידה מרכזת", 
                Inactive = false, 
                LocalName = "יחידה מרכזת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "298", 
                SearchFields = "298,הנהלת חשבונות ראשית", 
                Inactive = false, 
                LocalName = "הנהלת חשבונות ראשית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "3", 
                SearchFields = "3,סיטיבנק תל-אביב", 
                Inactive = false, 
                LocalName = "סיטיבנק תל-אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "30", 
                SearchFields = "30,דיאנא נצרת", 
                Inactive = false, 
                LocalName = "דיאנא נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "302", 
                SearchFields = "302,גבעת טל", 
                Inactive = false, 
                LocalName = "גבעת טל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "303", 
                SearchFields = "303,חיפה", 
                Inactive = false, 
                LocalName = "חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "305", 
                SearchFields = "305,ילמ", 
                Inactive = false, 
                LocalName = "ילמ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "308", 
                SearchFields = "308,ייעוץ פנסיוני", 
                Inactive = false, 
                LocalName = "ייעוץ פנסיוני", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "31", 
                SearchFields = "31,בני-ברק", 
                Inactive = false, 
                LocalName = "בני-ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "311", 
                SearchFields = "311,כרמל", 
                Inactive = false, 
                LocalName = "כרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "313", 
                SearchFields = "313,ראש פינה", 
                Inactive = false, 
                LocalName = "ראש פינה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "314", 
                SearchFields = "314,גלילות", 
                Inactive = false, 
                LocalName = "גלילות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "315", 
                SearchFields = "315,מודיעין", 
                Inactive = false, 
                LocalName = "מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "316", 
                SearchFields = "316,רמת פולג", 
                Inactive = false, 
                LocalName = "רמת פולג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "317", 
                SearchFields = "317,כפר-גנים", 
                Inactive = false, 
                LocalName = "כפר-גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "318", 
                SearchFields = "318,עיר ימים", 
                Inactive = false, 
                LocalName = "עיר ימים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "319", 
                SearchFields = "319,גבעת שמואל", 
                Inactive = false, 
                LocalName = "גבעת שמואל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "32", 
                SearchFields = "32,נצרת עלית", 
                Inactive = false, 
                LocalName = "נצרת עלית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "320", 
                SearchFields = "320,קריית השרון", 
                Inactive = false, 
                LocalName = "קריית השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "321", 
                SearchFields = "321,המרכז למימון מתמחה", 
                Inactive = false, 
                LocalName = "המרכז למימון מתמחה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "324", 
                SearchFields = "324,הגוש הגדול", 
                Inactive = false, 
                LocalName = "הגוש הגדול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "329", 
                SearchFields = "329,מת''ם", 
                Inactive = false, 
                LocalName = "מת''ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "33", 
                SearchFields = "33,שלוחת קרית גת", 
                Inactive = false, 
                LocalName = "שלוחת קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "330", 
                SearchFields = "330,בי''ח וולפסון", 
                Inactive = false, 
                LocalName = "בי''ח וולפסון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "331", 
                SearchFields = "331,כנפי נשרים", 
                Inactive = false, 
                LocalName = "כנפי נשרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "338", 
                SearchFields = "338,לקוחות נבחרים", 
                Inactive = false, 
                LocalName = "לקוחות נבחרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "34", 
                SearchFields = "34,שלוחת רחובות", 
                Inactive = false, 
                LocalName = "שלוחת רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "340", 
                SearchFields = "340,המרכז לבנקאות פרטית השרון", 
                Inactive = false, 
                LocalName = "המרכז לבנקאות פרטית השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "341", 
                SearchFields = "341,צריפין", 
                Inactive = false, 
                LocalName = "צריפין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "342", 
                SearchFields = "342,נבטים", 
                Inactive = false, 
                LocalName = "נבטים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "343", 
                SearchFields = "343,אריאל", 
                Inactive = false, 
                LocalName = "אריאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "344", 
                SearchFields = "344,ראשון לציון", 
                Inactive = false, 
                LocalName = "ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "345", 
                SearchFields = "345,רעות", 
                Inactive = false, 
                LocalName = "רעות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "346", 
                SearchFields = "346,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "347", 
                SearchFields = "347,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "348", 
                SearchFields = "348,אפק", 
                Inactive = false, 
                LocalName = "אפק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "349", 
                SearchFields = "349,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "35", 
                SearchFields = "35,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "350", 
                SearchFields = "350,מת''מ אומגה", 
                Inactive = false, 
                LocalName = "מת''מ אומגה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "351", 
                SearchFields = "351,משאבי שדה", 
                Inactive = false, 
                LocalName = "משאבי שדה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "352", 
                SearchFields = "352,''התעשיה רשל''''צ", 
                Inactive = false, 
                LocalName = "התעשיה רשל''''צ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "353", 
                SearchFields = "353,שקמונה", 
                Inactive = false, 
                LocalName = "שקמונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "354", 
                SearchFields = "354,רמת החייל", 
                Inactive = false, 
                LocalName = "רמת החייל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "355", 
                SearchFields = "355,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "356", 
                SearchFields = "356,היוצרים", 
                Inactive = false, 
                LocalName = "היוצרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "357", 
                SearchFields = "357,הסניף המרכזי", 
                Inactive = false, 
                LocalName = "הסניף המרכזי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "36", 
                SearchFields = "36,חדרה", 
                Inactive = false, 
                LocalName = "חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "360", 
                SearchFields = "360,סביונים", 
                Inactive = false, 
                LocalName = "סביונים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "361", 
                SearchFields = "361,החשמונאים", 
                Inactive = false, 
                LocalName = "החשמונאים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "362", 
                SearchFields = "362,אם המושבות", 
                Inactive = false, 
                LocalName = "אם המושבות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "363", 
                SearchFields = "363,דיסקונט בדרך שלך שרונים", 
                Inactive = false, 
                LocalName = "דיסקונט בדרך שלך שרונים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "365", 
                SearchFields = "365,צהלה", 
                Inactive = false, 
                LocalName = "צהלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "366", 
                SearchFields = "366,תל נוף", 
                Inactive = false, 
                LocalName = "תל נוף", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "367", 
                SearchFields = "367,חצור", 
                Inactive = false, 
                LocalName = "חצור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "368", 
                SearchFields = "368,ויצמן", 
                Inactive = false, 
                LocalName = "ויצמן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "369", 
                SearchFields = "369,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "37", 
                SearchFields = "37,הרצליה", 
                Inactive = false, 
                LocalName = "הרצליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "370", 
                SearchFields = "370,לוד", 
                Inactive = false, 
                LocalName = "לוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "371", 
                SearchFields = "371,רמת דוד", 
                Inactive = false, 
                LocalName = "רמת דוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "372", 
                SearchFields = "372,תל השומר", 
                Inactive = false, 
                LocalName = "תל השומר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "374", 
                SearchFields = "374,לוד תעשיה אוירית", 
                Inactive = false, 
                LocalName = "לוד תעשיה אוירית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "375", 
                SearchFields = "375,רמת השרון", 
                Inactive = false, 
                LocalName = "רמת השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "376", 
                SearchFields = "376,חצרים", 
                Inactive = false, 
                LocalName = "חצרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "377", 
                SearchFields = "377,אילת", 
                Inactive = false, 
                LocalName = "אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "378", 
                SearchFields = "378,הקריה", 
                Inactive = false, 
                LocalName = "הקריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "379", 
                SearchFields = "379,פלמחים", 
                Inactive = false, 
                LocalName = "פלמחים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "38", 
                SearchFields = "38,טבריה", 
                Inactive = false, 
                LocalName = "טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "380", 
                SearchFields = "380,מרכז עסקים דן", 
                Inactive = false, 
                LocalName = "מרכז עסקים דן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "381", 
                SearchFields = "381,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "382", 
                SearchFields = "382,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "383", 
                SearchFields = "383,מרכז עסקים צפון", 
                Inactive = false, 
                LocalName = "מרכז עסקים צפון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "384", 
                SearchFields = "384,''ב''''יס טכני ח''''א", 
                Inactive = false, 
                LocalName = "ב''''יס טכני ח''''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "385", 
                SearchFields = "385,''תע''''ש חיפה", 
                Inactive = false, 
                LocalName = "תע''''ש חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "386", 
                SearchFields = "386,בת גלים", 
                Inactive = false, 
                LocalName = "בת גלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "388", 
                SearchFields = "388,אוצר ישיר", 
                Inactive = false, 
                LocalName = "אוצר ישיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "389", 
                SearchFields = "389,הנהלה ראשית", 
                Inactive = false, 
                LocalName = "הנהלה ראשית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "39", 
                SearchFields = "39,שכר דירה", 
                Inactive = false, 
                LocalName = "שכר דירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "391", 
                SearchFields = "391,ראשון לציון עסקים", 
                Inactive = false, 
                LocalName = "ראשון לציון עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "392", 
                SearchFields = "392,מערב ראשון לציון", 
                Inactive = false, 
                LocalName = "מערב ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "393", 
                SearchFields = "393,רובע י''ז אשדוד", 
                Inactive = false, 
                LocalName = "רובע י''ז אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "394", 
                SearchFields = "394,רעננה עסקים", 
                Inactive = false, 
                LocalName = "רעננה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "395", 
                SearchFields = "395,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "396", 
                SearchFields = "396,אסף הרופא", 
                Inactive = false, 
                LocalName = "אסף הרופא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "397", 
                SearchFields = "397,גדרה", 
                Inactive = false, 
                LocalName = "גדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "398", 
                SearchFields = "398,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "399", 
                SearchFields = "399,אשדוד עסקים", 
                Inactive = false, 
                LocalName = "אשדוד עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "4", 
                SearchFields = "4,בן יהודה", 
                Inactive = false, 
                LocalName = "בן יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "40", 
                SearchFields = "40,שכר דירה", 
                Inactive = false, 
                LocalName = "שכר דירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "400", 
                SearchFields = "400,עוספיה", 
                Inactive = false, 
                LocalName = "עוספיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "401", 
                SearchFields = "401,רמת גן", 
                Inactive = false, 
                LocalName = "רמת גן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "402", 
                SearchFields = "402,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "403", 
                SearchFields = "403,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "404", 
                SearchFields = "404,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "405", 
                SearchFields = "405,חלחול", 
                Inactive = false, 
                LocalName = "חלחול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "406", 
                SearchFields = "406,ביר זית", 
                Inactive = false, 
                LocalName = "ביר זית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "407", 
                SearchFields = "407,בית אל על", 
                Inactive = false, 
                LocalName = "בית אל על", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "408", 
                SearchFields = "408,ריבל", 
                Inactive = false, 
                LocalName = "ריבל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "409", 
                SearchFields = "409,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "41", 
                SearchFields = "41,רמת גן", 
                Inactive = false, 
                LocalName = "רמת גן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "410", 
                SearchFields = "410,יאטה", 
                Inactive = false, 
                LocalName = "יאטה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "411", 
                SearchFields = "411,סופר בנק תל חנן", 
                Inactive = false, 
                LocalName = "סופר בנק תל חנן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "412", 
                SearchFields = "412,סופר בנק הדרים", 
                Inactive = false, 
                LocalName = "סופר בנק הדרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "413", 
                SearchFields = "413,סופר בנק היפרכל", 
                Inactive = false, 
                LocalName = "סופר בנק היפרכל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "414", 
                SearchFields = "414,משרד ראשי", 
                Inactive = false, 
                LocalName = "משרד ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "415", 
                SearchFields = "415,סופר בנק רחובות", 
                Inactive = false, 
                LocalName = "סופר בנק רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "416", 
                SearchFields = "416,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "417", 
                SearchFields = "417,גאולה י-ם", 
                Inactive = false, 
                LocalName = "גאולה י-ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "418", 
                SearchFields = "418,נתב''ג", 
                Inactive = false, 
                LocalName = "נתב''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "419", 
                SearchFields = "419,סופר בנק קניון הזהב", 
                Inactive = false, 
                LocalName = "סופר בנק קניון הזהב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "42", 
                SearchFields = "42,מגדל העמק", 
                Inactive = false, 
                LocalName = "מגדל העמק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "420", 
                SearchFields = "420,חדרה", 
                Inactive = false, 
                LocalName = "חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "421", 
                SearchFields = "421,אקספרס עראבה", 
                Inactive = false, 
                LocalName = "אקספרס עראבה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "422", 
                SearchFields = "422,סופרבנק 7 הכוכבים", 
                Inactive = false, 
                LocalName = "סופרבנק 7 הכוכבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "423", 
                SearchFields = "423,סופר בנק בארות יצחק", 
                Inactive = false, 
                LocalName = "סופר בנק בארות יצחק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "424", 
                SearchFields = "424,סופר בנק גרנד קניון", 
                Inactive = false, 
                LocalName = "סופר בנק גרנד קניון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "425", 
                SearchFields = "425,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "426", 
                SearchFields = "426,אל-עיזריה", 
                Inactive = false, 
                LocalName = "אל-עיזריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "427", 
                SearchFields = "427,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "428", 
                SearchFields = "428,בית ג'אלה", 
                Inactive = false, 
                LocalName = "בית ג'אלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "429", 
                SearchFields = "429,רמאללה", 
                Inactive = false, 
                LocalName = "רמאללה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "43", 
                SearchFields = "43,שלוחת נהריה", 
                Inactive = false, 
                LocalName = "שלוחת נהריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "430", 
                SearchFields = "430,סלפית", 
                Inactive = false, 
                LocalName = "סלפית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "431", 
                SearchFields = "431,רמאללה - הנהלה אזורית", 
                Inactive = false, 
                LocalName = "רמאללה - הנהלה אזורית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "432", 
                SearchFields = "432,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "433", 
                SearchFields = "433,קרית גת", 
                Inactive = false, 
                LocalName = "קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "434", 
                SearchFields = "434,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "435", 
                SearchFields = "435,ראשון לציון", 
                Inactive = false, 
                LocalName = "ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "436", 
                SearchFields = "436,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "437", 
                SearchFields = "437,אופקים", 
                Inactive = false, 
                LocalName = "אופקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "438", 
                SearchFields = "438,קניון אורות", 
                Inactive = false, 
                LocalName = "קניון אורות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "439", 
                SearchFields = "439,בית שאן", 
                Inactive = false, 
                LocalName = "בית שאן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "44", 
                SearchFields = "44,הלל יפה חדרה", 
                Inactive = false, 
                LocalName = "הלל יפה חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "440", 
                SearchFields = "440,סקטור תפעול מט''ח", 
                Inactive = false, 
                LocalName = "סקטור תפעול מט''ח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "441", 
                SearchFields = "441,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "442", 
                SearchFields = "442,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "443", 
                SearchFields = "443,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "444", 
                SearchFields = "444,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "445", 
                SearchFields = "445,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "446", 
                SearchFields = "446,עזה - אלנאסר", 
                Inactive = false, 
                LocalName = "עזה - אלנאסר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "447", 
                SearchFields = "447,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "448", 
                SearchFields = "448,נוסיראת", 
                Inactive = false, 
                LocalName = "נוסיראת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "449", 
                SearchFields = "449,ג'נין", 
                Inactive = false, 
                LocalName = "ג'נין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "45", 
                SearchFields = "45,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "450", 
                SearchFields = "450,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "451", 
                SearchFields = "451,מרכז עסקים תל אביב ב'", 
                Inactive = false, 
                LocalName = "מרכז עסקים תל אביב ב'", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "452", 
                SearchFields = "452,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "453", 
                SearchFields = "453,ג'בליה", 
                Inactive = false, 
                LocalName = "ג'בליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "454", 
                SearchFields = "454,אל-רימאל", 
                Inactive = false, 
                LocalName = "אל-רימאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "455", 
                SearchFields = "455,רפיח", 
                Inactive = false, 
                LocalName = "רפיח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "456", 
                SearchFields = "456,פעו", 
                Inactive = false, 
                LocalName = "פעו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "457", 
                SearchFields = "457,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "458", 
                SearchFields = "458,רמאללה", 
                Inactive = false, 
                LocalName = "רמאללה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "459", 
                SearchFields = "459,היצירה", 
                Inactive = false, 
                LocalName = "היצירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "46", 
                SearchFields = "46,תל אביב ראשי", 
                Inactive = false, 
                LocalName = "תל אביב ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "460", 
                SearchFields = "460,מונטיפיורי", 
                Inactive = false, 
                LocalName = "מונטיפיורי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "461", 
                SearchFields = "461,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "462", 
                SearchFields = "462,עיר ימים", 
                Inactive = false, 
                LocalName = "עיר ימים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "464", 
                SearchFields = "464,יובלים", 
                Inactive = false, 
                LocalName = "יובלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "465", 
                SearchFields = "465,אם המושבות", 
                Inactive = false, 
                LocalName = "אם המושבות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "466", 
                SearchFields = "466,קלקיליה", 
                Inactive = false, 
                LocalName = "קלקיליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "467", 
                SearchFields = "467,טול כרם", 
                Inactive = false, 
                LocalName = "טול כרם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "468", 
                SearchFields = "468,טובס", 
                Inactive = false, 
                LocalName = "טובס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "469", 
                SearchFields = "469,סלפית", 
                Inactive = false, 
                LocalName = "סלפית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "47", 
                SearchFields = "47,ג'דיידה - מכר", 
                Inactive = false, 
                LocalName = "ג'דיידה - מכר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "470", 
                SearchFields = "470,אקספרס ריינה", 
                Inactive = false, 
                LocalName = "אקספרס ריינה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "471", 
                SearchFields = "471,אל מסיון", 
                Inactive = false, 
                LocalName = "אל מסיון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "472", 
                SearchFields = "472,עפולה עסקים", 
                Inactive = false, 
                LocalName = "עפולה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "473", 
                SearchFields = "473,פרדס חנה", 
                Inactive = false, 
                LocalName = "פרדס חנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "474", 
                SearchFields = "474,אלמסיון", 
                Inactive = false, 
                LocalName = "אלמסיון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "475", 
                SearchFields = "475,herib-la", 
                Inactive = false, 
                LocalName = "heriB-lA", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "476", 
                SearchFields = "476,sulban wen", 
                Inactive = false, 
                LocalName = "sulbaN weN", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "477", 
                SearchFields = "477,meraklut", 
                Inactive = false, 
                LocalName = "merakluT", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "478", 
                SearchFields = "478,ohcirej", 
                Inactive = false, 
                LocalName = "ohcireJ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "479", 
                SearchFields = "479,גבעת שאול", 
                Inactive = false, 
                LocalName = "גבעת שאול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "48", 
                SearchFields = "48,''גבעת רמב''''ם", 
                Inactive = false, 
                LocalName = "גבעת רמב''''ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "480", 
                SearchFields = "480,ניירות ערך", 
                Inactive = false, 
                LocalName = "ניירות ערך", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "481", 
                SearchFields = "481,''הנה''''ר וסניף ת''''א", 
                Inactive = false, 
                LocalName = "הנה''''ר וסניף ת''''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "482", 
                SearchFields = "482,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "483", 
                SearchFields = "483,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "484", 
                SearchFields = "484,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "485", 
                SearchFields = "485,אל עזריה", 
                Inactive = false, 
                LocalName = "אל עזריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "486", 
                SearchFields = "486,אל מסיון-אל בירה", 
                Inactive = false, 
                LocalName = "אל מסיון-אל בירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "487", 
                SearchFields = "487,קרית שמונה", 
                Inactive = false, 
                LocalName = "קרית שמונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "488", 
                SearchFields = "488,סקטור קופ''ג", 
                Inactive = false, 
                LocalName = "סקטור קופ''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "489", 
                SearchFields = "489,ערד", 
                Inactive = false, 
                LocalName = "ערד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "49", 
                SearchFields = "49,יחידת ביצוע מחלקת מט''י", 
                Inactive = false, 
                LocalName = "יחידת ביצוע מחלקת מט''י", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "490", 
                SearchFields = "490,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "491", 
                SearchFields = "491,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "492", 
                SearchFields = "492,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "493", 
                SearchFields = "493,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "494", 
                SearchFields = "494,חשמונאים", 
                Inactive = false, 
                LocalName = "חשמונאים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "495", 
                SearchFields = "495,א''ת ראשל''צ", 
                Inactive = false, 
                LocalName = "א''ת ראשל''צ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "496", 
                SearchFields = "496,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "497", 
                SearchFields = "497,גדרה", 
                Inactive = false, 
                LocalName = "גדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "499", 
                SearchFields = "499,משרד ראשי", 
                Inactive = false, 
                LocalName = "משרד ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "5", 
                SearchFields = "5,גאולה", 
                Inactive = false, 
                LocalName = "גאולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "50", 
                SearchFields = "50,ז'בוטינסקי", 
                Inactive = false, 
                LocalName = "ז'בוטינסקי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "500", 
                SearchFields = "500,הרימון", 
                Inactive = false, 
                LocalName = "הרימון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "501", 
                SearchFields = "501,מרכז ארצי לתושבי חוץ", 
                Inactive = false, 
                LocalName = "מרכז ארצי לתושבי חוץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "502", 
                SearchFields = "502,קרן קיימת", 
                Inactive = false, 
                LocalName = "קרן קיימת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "503", 
                SearchFields = "503,מודיעין דיור", 
                Inactive = false, 
                LocalName = "מודיעין דיור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "504", 
                SearchFields = "504,פרימקו", 
                Inactive = false, 
                LocalName = "פרימקו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "505", 
                SearchFields = "505,המעפילים", 
                Inactive = false, 
                LocalName = "המעפילים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "506", 
                SearchFields = "506,שפרעם", 
                Inactive = false, 
                LocalName = "שפרעם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "507", 
                SearchFields = "507,קרית אונו החדשה", 
                Inactive = false, 
                LocalName = "קרית אונו החדשה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "508", 
                SearchFields = "508,מגדיאל - הוד השרון", 
                Inactive = false, 
                LocalName = "מגדיאל - הוד השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "509", 
                SearchFields = "509,הנביאים חיפה", 
                Inactive = false, 
                LocalName = "הנביאים חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "51", 
                SearchFields = "51,תל אביב", 
                Inactive = false, 
                LocalName = "תל אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "510", 
                SearchFields = "510,מרכז מזומנים", 
                Inactive = false, 
                LocalName = "מרכז מזומנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "511", 
                SearchFields = "511,ככר עצמאות", 
                Inactive = false, 
                LocalName = "ככר עצמאות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "512", 
                SearchFields = "512,נוה", 
                Inactive = false, 
                LocalName = "נוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "513", 
                SearchFields = "513,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "514", 
                SearchFields = "514,המלאכה", 
                Inactive = false, 
                LocalName = "המלאכה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "515", 
                SearchFields = "515,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "516", 
                SearchFields = "516,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "517", 
                SearchFields = "517,חדרה", 
                Inactive = false, 
                LocalName = "חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "518", 
                SearchFields = "518,מחלקת בנקים ומוסדות פיננסים", 
                Inactive = false, 
                LocalName = "מחלקת בנקים ומוסדות פיננסים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "519", 
                SearchFields = "519,קרית אריה", 
                Inactive = false, 
                LocalName = "קרית אריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "52", 
                SearchFields = "52,מחנה יהודה", 
                Inactive = false, 
                LocalName = "מחנה יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "520", 
                SearchFields = "520,מרכז בנקאות פרטית לתושבי חוץ י-ם", 
                Inactive = false, 
                LocalName = "מרכז בנקאות פרטית לתושבי חוץ י-ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "521", 
                SearchFields = "521,מודיעין", 
                Inactive = false, 
                LocalName = "מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "522", 
                SearchFields = "522,הרצליה פיתוח", 
                Inactive = false, 
                LocalName = "הרצליה פיתוח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "523", 
                SearchFields = "523,''הרא''''ה", 
                Inactive = false, 
                LocalName = "הרא''''ה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "524", 
                SearchFields = "524,עמק שרה", 
                Inactive = false, 
                LocalName = "עמק שרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "525", 
                SearchFields = "525,שלבים", 
                Inactive = false, 
                LocalName = "שלבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "526", 
                SearchFields = "526,שלומציון", 
                Inactive = false, 
                LocalName = "שלומציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "527", 
                SearchFields = "527,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "528", 
                SearchFields = "528,הלוואות עובדים", 
                Inactive = false, 
                LocalName = "הלוואות עובדים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "529", 
                SearchFields = "529,קניון הבאר", 
                Inactive = false, 
                LocalName = "קניון הבאר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "53", 
                SearchFields = "53,שעריים", 
                Inactive = false, 
                LocalName = "שעריים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "530", 
                SearchFields = "530,קרית מנחם", 
                Inactive = false, 
                LocalName = "קרית מנחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "531", 
                SearchFields = "531,ניהול קופות גמל", 
                Inactive = false, 
                LocalName = "ניהול קופות גמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "532", 
                SearchFields = "532,פארק המדע רחובות", 
                Inactive = false, 
                LocalName = "פארק המדע רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "533", 
                SearchFields = "533,מול הים", 
                Inactive = false, 
                LocalName = "מול הים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "534", 
                SearchFields = "534,הפועלים", 
                Inactive = false, 
                LocalName = "הפועלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "535", 
                SearchFields = "535,הירקון- המרכז הארצי לתושבי חוץ", 
                Inactive = false, 
                LocalName = "הירקון- המרכז הארצי לתושבי חוץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "536", 
                SearchFields = "536,תפעול עורפי", 
                Inactive = false, 
                LocalName = "תפעול עורפי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "537", 
                SearchFields = "537,מגמ''ש (מרכז גביה משפטית", 
                Inactive = false, 
                LocalName = "מגמ''ש (מרכז גביה משפטית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "538", 
                SearchFields = "538,רמות", 
                Inactive = false, 
                LocalName = "רמות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "539", 
                SearchFields = "539,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "54", 
                SearchFields = "54,המלך ג'ורג'", 
                Inactive = false, 
                LocalName = "המלך ג'ורג'", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "540", 
                SearchFields = "540,יבנה", 
                Inactive = false, 
                LocalName = "יבנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "541", 
                SearchFields = "541,מרכז בנקאות פרטת נתניה", 
                Inactive = false, 
                LocalName = "מרכז בנקאות פרטת נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "542", 
                SearchFields = "542,עיר ימים נתניה", 
                Inactive = false, 
                LocalName = "עיר ימים נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "543", 
                SearchFields = "543,כורזין", 
                Inactive = false, 
                LocalName = "כורזין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "544", 
                SearchFields = "544,קק''ל", 
                Inactive = false, 
                LocalName = "קק''ל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "545", 
                SearchFields = "545,הדר", 
                Inactive = false, 
                LocalName = "הדר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "546", 
                SearchFields = "546,גצםמג", 
                Inactive = false, 
                LocalName = "גצםמג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "547", 
                SearchFields = "547,''גלבוע ת''''א", 
                Inactive = false, 
                LocalName = "גלבוע ת''''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "548", 
                SearchFields = "548,כפר גנים", 
                Inactive = false, 
                LocalName = "כפר גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "549", 
                SearchFields = "549,כפר תבור", 
                Inactive = false, 
                LocalName = "כפר תבור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "55", 
                SearchFields = "55,''בית אבות ''''משען פיכמן", 
                Inactive = false, 
                LocalName = "בית אבות ''''משען פיכמן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "550", 
                SearchFields = "550,רוטשילד פ''ת", 
                Inactive = false, 
                LocalName = "רוטשילד פ''ת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "551", 
                SearchFields = "551,פז רמת גן", 
                Inactive = false, 
                LocalName = "פז רמת גן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "552", 
                SearchFields = "552,רוטשילד ראשל''צ", 
                Inactive = false, 
                LocalName = "רוטשילד ראשל''צ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "553", 
                SearchFields = "553,ברקת, נתניה", 
                Inactive = false, 
                LocalName = "ברקת, נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "554", 
                SearchFields = "554,ככר נמיר", 
                Inactive = false, 
                LocalName = "ככר נמיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "555", 
                SearchFields = "555,קוגל,חולון", 
                Inactive = false, 
                LocalName = "קוגל,חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "556", 
                SearchFields = "556,החשמונאים, ת''א", 
                Inactive = false, 
                LocalName = "החשמונאים, ת''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "557", 
                SearchFields = "557,רוטשילד, ראשל''צ", 
                Inactive = false, 
                LocalName = "רוטשילד, ראשל''צ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "558", 
                SearchFields = "558,מעלות", 
                Inactive = false, 
                LocalName = "מעלות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "559", 
                SearchFields = "559,נצרת", 
                Inactive = false, 
                LocalName = "נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "56", 
                SearchFields = "56,''בית אבות ''''משען נווה אביבים", 
                Inactive = false, 
                LocalName = "בית אבות ''''משען נווה אביבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "560", 
                SearchFields = "560,נצרת עלית", 
                Inactive = false, 
                LocalName = "נצרת עלית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "561", 
                SearchFields = "561,המעיין", 
                Inactive = false, 
                LocalName = "המעיין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "562", 
                SearchFields = "562,חיפה עסקים", 
                Inactive = false, 
                LocalName = "חיפה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "563", 
                SearchFields = "563,לוי אשכול,ק.ים", 
                Inactive = false, 
                LocalName = "לוי אשכול,ק.ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "564", 
                SearchFields = "564,פלי''ם, חיפה", 
                Inactive = false, 
                LocalName = "פלי''ם, חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "565", 
                SearchFields = "565,מנחמיה", 
                Inactive = false, 
                LocalName = "מנחמיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "566", 
                SearchFields = "566,דרך מצדה, ב''ש", 
                Inactive = false, 
                LocalName = "דרך מצדה, ב''ש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "567", 
                SearchFields = "567,בית שמש", 
                Inactive = false, 
                LocalName = "בית שמש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "568", 
                SearchFields = "568,הלני המלכה, י-ם", 
                Inactive = false, 
                LocalName = "הלני המלכה, י-ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "569", 
                SearchFields = "569,רמת אשכול, י-ם", 
                Inactive = false, 
                LocalName = "רמת אשכול, י-ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "57", 
                SearchFields = "57,''תחנה מרכזית פ''''ת", 
                Inactive = false, 
                LocalName = "תחנה מרכזית פ''''ת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "570", 
                SearchFields = "570,מכס אילת", 
                Inactive = false, 
                LocalName = "מכס אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "571", 
                SearchFields = "571,עתיד", 
                Inactive = false, 
                LocalName = "עתיד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "572", 
                SearchFields = "572,אופק", 
                Inactive = false, 
                LocalName = "אופק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "573", 
                SearchFields = "573,שחק", 
                Inactive = false, 
                LocalName = "שחק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "574", 
                SearchFields = "574,שגב", 
                Inactive = false, 
                LocalName = "שגב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "575", 
                SearchFields = "575,שמואל הנציב", 
                Inactive = false, 
                LocalName = "שמואל הנציב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "576", 
                SearchFields = "576,ברק", 
                Inactive = false, 
                LocalName = "ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "577", 
                SearchFields = "577,הדקל", 
                Inactive = false, 
                LocalName = "הדקל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "578", 
                SearchFields = "578,כפר קרע", 
                Inactive = false, 
                LocalName = "כפר קרע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "579", 
                SearchFields = "579,חברות בנות", 
                Inactive = false, 
                LocalName = "חברות בנות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "58", 
                SearchFields = "58,דרך בן גוריון", 
                Inactive = false, 
                LocalName = "דרך בן גוריון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "580", 
                SearchFields = "580,בטוח לאומי ושרות תעסוקה", 
                Inactive = false, 
                LocalName = "בטוח לאומי ושרות תעסוקה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "581", 
                SearchFields = "581,מג'אר", 
                Inactive = false, 
                LocalName = "מג'אר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "582", 
                SearchFields = "582,מודיעין", 
                Inactive = false, 
                LocalName = "מודיעין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "584", 
                SearchFields = "584,הגלים", 
                Inactive = false, 
                LocalName = "הגלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "585", 
                SearchFields = "585,גילה", 
                Inactive = false, 
                LocalName = "גילה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "586", 
                SearchFields = "586,חולון עסקים", 
                Inactive = false, 
                LocalName = "חולון עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "588", 
                SearchFields = "588,ארמון הנציב", 
                Inactive = false, 
                LocalName = "ארמון הנציב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "59", 
                SearchFields = "59,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "590", 
                SearchFields = "590,היפרנטו-קרית מוצקין", 
                Inactive = false, 
                LocalName = "היפרנטו-קרית מוצקין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "591", 
                SearchFields = "591,אורן", 
                Inactive = false, 
                LocalName = "אורן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "593", 
                SearchFields = "593,ג'ת", 
                Inactive = false, 
                LocalName = "ג'ת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "594", 
                SearchFields = "594,טוביהו", 
                Inactive = false, 
                LocalName = "טוביהו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "595", 
                SearchFields = "595,הלוואות עובדים", 
                Inactive = false, 
                LocalName = "הלוואות עובדים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "596", 
                SearchFields = "596,הלו' חרושת אמפל וניר", 
                Inactive = false, 
                LocalName = "הלו' חרושת אמפל וניר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "597", 
                SearchFields = "597,גנים", 
                Inactive = false, 
                LocalName = "גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "598", 
                SearchFields = "598,התעשיה נתניה", 
                Inactive = false, 
                LocalName = "התעשיה נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "599", 
                SearchFields = "599,הגן הטכנולוגי", 
                Inactive = false, 
                LocalName = "הגן הטכנולוגי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "6", 
                SearchFields = "6,בני ברק", 
                Inactive = false, 
                LocalName = "בני ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "60", 
                SearchFields = "60,יחידת בצוע ני''ע", 
                Inactive = false, 
                LocalName = "יחידת בצוע ני''ע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "600", 
                SearchFields = "600,קרן חסויים", 
                Inactive = false, 
                LocalName = "קרן חסויים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "601", 
                SearchFields = "601,אלנבי", 
                Inactive = false, 
                LocalName = "אלנבי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "602", 
                SearchFields = "602,ירכא", 
                Inactive = false, 
                LocalName = "ירכא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "603", 
                SearchFields = "603,מיתר", 
                Inactive = false, 
                LocalName = "מיתר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "604", 
                SearchFields = "604,קרית שלום", 
                Inactive = false, 
                LocalName = "קרית שלום", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "605", 
                SearchFields = "605,יהודה המכבי", 
                Inactive = false, 
                LocalName = "יהודה המכבי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "606", 
                SearchFields = "606,רמת אביב", 
                Inactive = false, 
                LocalName = "רמת אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "607", 
                SearchFields = "607,שכונת התקוה", 
                Inactive = false, 
                LocalName = "שכונת התקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "608", 
                SearchFields = "608,ארלוזורוב", 
                Inactive = false, 
                LocalName = "ארלוזורוב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "609", 
                SearchFields = "609,המסגר", 
                Inactive = false, 
                LocalName = "המסגר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "61", 
                SearchFields = "61,פתח-תקוה עסקים", 
                Inactive = false, 
                LocalName = "פתח-תקוה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "610", 
                SearchFields = "610,שוק הכרמל", 
                Inactive = false, 
                LocalName = "שוק הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "611", 
                SearchFields = "611,יפו", 
                Inactive = false, 
                LocalName = "יפו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "612", 
                SearchFields = "612,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "613", 
                SearchFields = "613,מונטיפיורי", 
                Inactive = false, 
                LocalName = "מונטיפיורי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "614", 
                SearchFields = "614,טמרה", 
                Inactive = false, 
                LocalName = "טמרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "615", 
                SearchFields = "615,באקה אל-גרביה", 
                Inactive = false, 
                LocalName = "באקה אל-גרביה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "616", 
                SearchFields = "616,גני הדרים", 
                Inactive = false, 
                LocalName = "גני הדרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "617", 
                SearchFields = "617,יהוד", 
                Inactive = false, 
                LocalName = "יהוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "618", 
                SearchFields = "618,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "619", 
                SearchFields = "619,לוד", 
                Inactive = false, 
                LocalName = "לוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "62", 
                SearchFields = "62,שלוחת טבריה", 
                Inactive = false, 
                LocalName = "שלוחת טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "620", 
                SearchFields = "620,שפרעם", 
                Inactive = false, 
                LocalName = "שפרעם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "621", 
                SearchFields = "621,רזיאל", 
                Inactive = false, 
                LocalName = "רזיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "622", 
                SearchFields = "622,צמרת", 
                Inactive = false, 
                LocalName = "צמרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "623", 
                SearchFields = "623,כרכור", 
                Inactive = false, 
                LocalName = "כרכור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "624", 
                SearchFields = "624,הפניקס קופ''ג", 
                Inactive = false, 
                LocalName = "הפניקס קופ''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "625", 
                SearchFields = "625,אלטשולר שחם קופ''ג", 
                Inactive = false, 
                LocalName = "אלטשולר שחם קופ''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "626", 
                SearchFields = "626,''ד''''ש 2 קופ''''ג", 
                Inactive = false, 
                LocalName = "ד''''ש 2 קופ''''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "627", 
                SearchFields = "627,אלטשולר שחם, קופ''ג", 
                Inactive = false, 
                LocalName = "אלטשולר שחם, קופ''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "628", 
                SearchFields = "628,חברות מנהלות שונות", 
                Inactive = false, 
                LocalName = "חברות מנהלות שונות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "629", 
                SearchFields = "629,''אקסלנס 2 קופ''''ג", 
                Inactive = false, 
                LocalName = "אקסלנס 2 קופ''''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "63", 
                SearchFields = "63,קרית אתא", 
                Inactive = false, 
                LocalName = "קרית אתא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "630", 
                SearchFields = "630,''אקסלנס 3 קופ''''ג", 
                Inactive = false, 
                LocalName = "אקסלנס 3 קופ''''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "631", 
                SearchFields = "631,''ד''''ש 3 קופ''''ג", 
                Inactive = false, 
                LocalName = "ד''''ש 3 קופ''''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "632", 
                SearchFields = "632,רמות", 
                Inactive = false, 
                LocalName = "רמות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "633", 
                SearchFields = "633,כפר מכר", 
                Inactive = false, 
                LocalName = "כפר מכר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "634", 
                SearchFields = "634,עילבון", 
                Inactive = false, 
                LocalName = "עילבון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "635", 
                SearchFields = "635,גאולה", 
                Inactive = false, 
                LocalName = "גאולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "636", 
                SearchFields = "636,נס ציונה", 
                Inactive = false, 
                LocalName = "נס ציונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "637", 
                SearchFields = "637,ככר המולד", 
                Inactive = false, 
                LocalName = "ככר המולד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "638", 
                SearchFields = "638,סאלח א-דין", 
                Inactive = false, 
                LocalName = "סאלח א-דין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "639", 
                SearchFields = "639,נצרת ראשי", 
                Inactive = false, 
                LocalName = "נצרת ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "64", 
                SearchFields = "64,בית שמש", 
                Inactive = false, 
                LocalName = "בית שמש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "640", 
                SearchFields = "640,אל ראם", 
                Inactive = false, 
                LocalName = "אל ראם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "641", 
                SearchFields = "641,שכר דירה תל אביב", 
                Inactive = false, 
                LocalName = "שכר דירה תל אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "642", 
                SearchFields = "642,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "643", 
                SearchFields = "643,שכר דירה חיפה", 
                Inactive = false, 
                LocalName = "שכר דירה חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "644", 
                SearchFields = "644,''קש''''ב", 
                Inactive = false, 
                LocalName = "קש''''ב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "645", 
                SearchFields = "645,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "646", 
                SearchFields = "646,שכר דירה ירושלים", 
                Inactive = false, 
                LocalName = "שכר דירה ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "647", 
                SearchFields = "647,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "648", 
                SearchFields = "648,ערד", 
                Inactive = false, 
                LocalName = "ערד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "649", 
                SearchFields = "649,דימונה", 
                Inactive = false, 
                LocalName = "דימונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "65", 
                SearchFields = "65,אבן גבירול", 
                Inactive = false, 
                LocalName = "אבן גבירול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "650", 
                SearchFields = "650,עקיבא", 
                Inactive = false, 
                LocalName = "עקיבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "651", 
                SearchFields = "651,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "652", 
                SearchFields = "652,התקוה", 
                Inactive = false, 
                LocalName = "התקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "653", 
                SearchFields = "653,יפו", 
                Inactive = false, 
                LocalName = "יפו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "654", 
                SearchFields = "654,תל אביב ראשי", 
                Inactive = false, 
                LocalName = "תל אביב ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "655", 
                SearchFields = "655,דרך הים", 
                Inactive = false, 
                LocalName = "דרך הים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "656", 
                SearchFields = "656,יורדי הסירה", 
                Inactive = false, 
                LocalName = "יורדי הסירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "657", 
                SearchFields = "657,בן יהודה", 
                Inactive = false, 
                LocalName = "בן יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "658", 
                SearchFields = "658,האורגים", 
                Inactive = false, 
                LocalName = "האורגים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "659", 
                SearchFields = "659,חנקין", 
                Inactive = false, 
                LocalName = "חנקין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "66", 
                SearchFields = "66,ערד", 
                Inactive = false, 
                LocalName = "ערד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "660", 
                SearchFields = "660,מכון וייצמן", 
                Inactive = false, 
                LocalName = "מכון וייצמן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "661", 
                SearchFields = "661,קרן קימת", 
                Inactive = false, 
                LocalName = "קרן קימת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "662", 
                SearchFields = "662,טירת הכרמל", 
                Inactive = false, 
                LocalName = "טירת הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "663", 
                SearchFields = "663,יהלום", 
                Inactive = false, 
                LocalName = "יהלום", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "664", 
                SearchFields = "664,אבן גבירול", 
                Inactive = false, 
                LocalName = "אבן גבירול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "665", 
                SearchFields = "665,הגליל", 
                Inactive = false, 
                LocalName = "הגליל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "666", 
                SearchFields = "666,גבעתיים", 
                Inactive = false, 
                LocalName = "גבעתיים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "667", 
                SearchFields = "667,יהלומים", 
                Inactive = false, 
                LocalName = "יהלומים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "668", 
                SearchFields = "668,קניון אילון", 
                Inactive = false, 
                LocalName = "קניון אילון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "669", 
                SearchFields = "669,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "67", 
                SearchFields = "67,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "670", 
                SearchFields = "670,נהורה", 
                Inactive = false, 
                LocalName = "נהורה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "671", 
                SearchFields = "671,אכסאל", 
                Inactive = false, 
                LocalName = "אכסאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "672", 
                SearchFields = "672,תעשיה-נתניה", 
                Inactive = false, 
                LocalName = "תעשיה-נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "673", 
                SearchFields = "673,פקדונות", 
                Inactive = false, 
                LocalName = "פקדונות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "674", 
                SearchFields = "674,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "675", 
                SearchFields = "675,מחנה יהודה", 
                Inactive = false, 
                LocalName = "מחנה יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "676", 
                SearchFields = "676,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "677", 
                SearchFields = "677,סגולה", 
                Inactive = false, 
                LocalName = "סגולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "678", 
                SearchFields = "678,טחכךט", 
                Inactive = false, 
                LocalName = "טחכךט", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "679", 
                SearchFields = "679,מרכז הכרמל", 
                Inactive = false, 
                LocalName = "מרכז הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "68", 
                SearchFields = "68,אור יהודה", 
                Inactive = false, 
                LocalName = "אור יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "680", 
                SearchFields = "680,קרית מוצקין", 
                Inactive = false, 
                LocalName = "קרית מוצקין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "681", 
                SearchFields = "681,ג'דידה-מכר", 
                Inactive = false, 
                LocalName = "ג'דידה-מכר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "682", 
                SearchFields = "682,בועיינה", 
                Inactive = false, 
                LocalName = "בועיינה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "683", 
                SearchFields = "683,אור יהודה", 
                Inactive = false, 
                LocalName = "אור יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "684", 
                SearchFields = "684,פסוטה", 
                Inactive = false, 
                LocalName = "פסוטה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "685", 
                SearchFields = "685,הדס מרכנתיל קופות גמל", 
                Inactive = false, 
                LocalName = "הדס מרכנתיל קופות גמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "686", 
                SearchFields = "686,חולון", 
                Inactive = false, 
                LocalName = "חולון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "687", 
                SearchFields = "687,היקב", 
                Inactive = false, 
                LocalName = "היקב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "688", 
                SearchFields = "688,מסלקה", 
                Inactive = false, 
                LocalName = "מסלקה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "689", 
                SearchFields = "689,נחף", 
                Inactive = false, 
                LocalName = "נחף", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "69", 
                SearchFields = "69,גבעת הרצל", 
                Inactive = false, 
                LocalName = "גבעת הרצל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "690", 
                SearchFields = "690,מרכז ני''ע", 
                Inactive = false, 
                LocalName = "מרכז ני''ע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "691", 
                SearchFields = "691,כפר יאסיף", 
                Inactive = false, 
                LocalName = "כפר יאסיף", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "692", 
                SearchFields = "692,תרשיחא", 
                Inactive = false, 
                LocalName = "תרשיחא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "693", 
                SearchFields = "693,אג'מי", 
                Inactive = false, 
                LocalName = "אג'מי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "694", 
                SearchFields = "694,ואדי ניסנאס", 
                Inactive = false, 
                LocalName = "ואדי ניסנאס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "695", 
                SearchFields = "695,לב הפארק", 
                Inactive = false, 
                LocalName = "לב הפארק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "696", 
                SearchFields = "696,רהט", 
                Inactive = false, 
                LocalName = "רהט", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "697", 
                SearchFields = "697,אום אל פחם", 
                Inactive = false, 
                LocalName = "אום אל פחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "698", 
                SearchFields = "698,יבניאל", 
                Inactive = false, 
                LocalName = "יבניאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "699", 
                SearchFields = "699,כפר סבא", 
                Inactive = false, 
                LocalName = "כפר סבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "7", 
                SearchFields = "7,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "70", 
                SearchFields = "70,שלוחת אילת", 
                Inactive = false, 
                LocalName = "שלוחת אילת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "700", 
                SearchFields = "700,חיפה ראשי", 
                Inactive = false, 
                LocalName = "חיפה ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "701", 
                SearchFields = "701,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "702", 
                SearchFields = "702,מנהלת אזור ירושלים והדרום", 
                Inactive = false, 
                LocalName = "מנהלת אזור ירושלים והדרום", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "703", 
                SearchFields = "703,כפר גנים", 
                Inactive = false, 
                LocalName = "כפר גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "704", 
                SearchFields = "704,באר יעקב", 
                Inactive = false, 
                LocalName = "באר יעקב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "705", 
                SearchFields = "705,מנהלת אזור נצרת", 
                Inactive = false, 
                LocalName = "מנהלת אזור נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "706", 
                SearchFields = "706,פרדסיה", 
                Inactive = false, 
                LocalName = "פרדסיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "707", 
                SearchFields = "707,קרית אליעזר", 
                Inactive = false, 
                LocalName = "קרית אליעזר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "708", 
                SearchFields = "708,קרית שפרינצק", 
                Inactive = false, 
                LocalName = "קרית שפרינצק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "709", 
                SearchFields = "709,התשבי", 
                Inactive = false, 
                LocalName = "התשבי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "71", 
                SearchFields = "71,הדר הכרמל", 
                Inactive = false, 
                LocalName = "הדר הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "710", 
                SearchFields = "710,בית הקרנות", 
                Inactive = false, 
                LocalName = "בית הקרנות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "711", 
                SearchFields = "711,''מרכז השקעות אח''''מים מרכנתיל", 
                Inactive = false, 
                LocalName = "מרכז השקעות אח''''מים מרכנתיל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "712", 
                SearchFields = "712,נשר תל חנן", 
                Inactive = false, 
                LocalName = "נשר תל חנן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "713", 
                SearchFields = "713,אורן", 
                Inactive = false, 
                LocalName = "אורן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "714", 
                SearchFields = "714,צפת", 
                Inactive = false, 
                LocalName = "צפת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "715", 
                SearchFields = "715,חצור", 
                Inactive = false, 
                LocalName = "חצור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "716", 
                SearchFields = "716,נהריה", 
                Inactive = false, 
                LocalName = "נהריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "717", 
                SearchFields = "717,בית שאן", 
                Inactive = false, 
                LocalName = "בית שאן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "718", 
                SearchFields = "718,קרית שמונה", 
                Inactive = false, 
                LocalName = "קרית שמונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "719", 
                SearchFields = "719,קרית טבעון-הנשיא", 
                Inactive = false, 
                LocalName = "קרית טבעון-הנשיא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "72", 
                SearchFields = "72,הנהלה אזורית", 
                Inactive = false, 
                LocalName = "הנהלה אזורית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "720", 
                SearchFields = "720,קרית חיים", 
                Inactive = false, 
                LocalName = "קרית חיים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "721", 
                SearchFields = "721,קרית אתא", 
                Inactive = false, 
                LocalName = "קרית אתא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "722", 
                SearchFields = "722,יסוד המעלה", 
                Inactive = false, 
                LocalName = "יסוד המעלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "723", 
                SearchFields = "723,טבריה", 
                Inactive = false, 
                LocalName = "טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "724", 
                SearchFields = "724,טבריה עלית", 
                Inactive = false, 
                LocalName = "טבריה עלית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "725", 
                SearchFields = "725,יבנאל", 
                Inactive = false, 
                LocalName = "יבנאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "726", 
                SearchFields = "726,נצרת", 
                Inactive = false, 
                LocalName = "נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "727", 
                SearchFields = "727,מרכז סחר חוץ צפוני", 
                Inactive = false, 
                LocalName = "מרכז סחר חוץ צפוני", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "728", 
                SearchFields = "728,מגדל העמק", 
                Inactive = false, 
                LocalName = "מגדל העמק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "729", 
                SearchFields = "729,קרית מוצקין", 
                Inactive = false, 
                LocalName = "קרית מוצקין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "73", 
                SearchFields = "73,תלפיות", 
                Inactive = false, 
                LocalName = "תלפיות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "730", 
                SearchFields = "730,קרית ים", 
                Inactive = false, 
                LocalName = "קרית ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "731", 
                SearchFields = "731,חזון איש", 
                Inactive = false, 
                LocalName = "חזון איש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "732", 
                SearchFields = "732,בני ברק", 
                Inactive = false, 
                LocalName = "בני ברק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "733", 
                SearchFields = "733,רמת השרון", 
                Inactive = false, 
                LocalName = "רמת השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "734", 
                SearchFields = "734,צפת דרום", 
                Inactive = false, 
                LocalName = "צפת דרום", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "735", 
                SearchFields = "735,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "736", 
                SearchFields = "736,סנהדריה", 
                Inactive = false, 
                LocalName = "סנהדריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "737", 
                SearchFields = "737,ירושלים", 
                Inactive = false, 
                LocalName = "ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "738", 
                SearchFields = "738,כאבול", 
                Inactive = false, 
                LocalName = "כאבול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "739", 
                SearchFields = "739,החלוץ", 
                Inactive = false, 
                LocalName = "החלוץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "74", 
                SearchFields = "74,תלפיות", 
                Inactive = false, 
                LocalName = "תלפיות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "740", 
                SearchFields = "740,גבעת המורה", 
                Inactive = false, 
                LocalName = "גבעת המורה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "741", 
                SearchFields = "741,קיסריה", 
                Inactive = false, 
                LocalName = "קיסריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "742", 
                SearchFields = "742,רמות", 
                Inactive = false, 
                LocalName = "רמות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "743", 
                SearchFields = "743,דיר אל אסד - בענה", 
                Inactive = false, 
                LocalName = "דיר אל אסד - בענה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "744", 
                SearchFields = "744,דיר - חנא", 
                Inactive = false, 
                LocalName = "דיר - חנא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "745", 
                SearchFields = "745,רבי עקיבא", 
                Inactive = false, 
                LocalName = "רבי עקיבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "746", 
                SearchFields = "746,הפרדס", 
                Inactive = false, 
                LocalName = "הפרדס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "747", 
                SearchFields = "747,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "748", 
                SearchFields = "748,חוצות המפרץ", 
                Inactive = false, 
                LocalName = "חוצות המפרץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "749", 
                SearchFields = "749,קיון ירושלים", 
                Inactive = false, 
                LocalName = "קיון ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "75", 
                SearchFields = "75,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "750", 
                SearchFields = "750,הועד הפועל", 
                Inactive = false, 
                LocalName = "הועד הפועל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "751", 
                SearchFields = "751,עופר", 
                Inactive = false, 
                LocalName = "עופר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "752", 
                SearchFields = "752,זרזיר", 
                Inactive = false, 
                LocalName = "זרזיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "753", 
                SearchFields = "753,מטרסדורף", 
                Inactive = false, 
                LocalName = "מטרסדורף", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "754", 
                SearchFields = "754,פנקס", 
                Inactive = false, 
                LocalName = "פנקס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "755", 
                SearchFields = "755,המצודה", 
                Inactive = false, 
                LocalName = "המצודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "756", 
                SearchFields = "756,אעבלין", 
                Inactive = false, 
                LocalName = "אעבלין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "757", 
                SearchFields = "757,הפארק", 
                Inactive = false, 
                LocalName = "הפארק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "758", 
                SearchFields = "758,ששת הימים", 
                Inactive = false, 
                LocalName = "ששת הימים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "759", 
                SearchFields = "759,נאות שושנים", 
                Inactive = false, 
                LocalName = "נאות שושנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "76", 
                SearchFields = "76,העצמאות", 
                Inactive = false, 
                LocalName = "העצמאות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "760", 
                SearchFields = "760,פועלים בסופר", 
                Inactive = false, 
                LocalName = "פועלים בסופר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "761", 
                SearchFields = "761,חיים עוזר", 
                Inactive = false, 
                LocalName = "חיים עוזר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "762", 
                SearchFields = "762,עסקים הר חוצבים", 
                Inactive = false, 
                LocalName = "עסקים הר חוצבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "763", 
                SearchFields = "763,ערד", 
                Inactive = false, 
                LocalName = "ערד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "764", 
                SearchFields = "764,התנאים", 
                Inactive = false, 
                LocalName = "התנאים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "765", 
                SearchFields = "765,עתידים עסקים", 
                Inactive = false, 
                LocalName = "עתידים עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "766", 
                SearchFields = "766,גור", 
                Inactive = false, 
                LocalName = "גור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "768", 
                SearchFields = "768,נוה-ים", 
                Inactive = false, 
                LocalName = "נוה-ים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "769", 
                SearchFields = "769,נוה מגן", 
                Inactive = false, 
                LocalName = "נוה מגן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "77", 
                SearchFields = "77,כפר סבא", 
                Inactive = false, 
                LocalName = "כפר סבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "770", 
                SearchFields = "770,האשל", 
                Inactive = false, 
                LocalName = "האשל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "771", 
                SearchFields = "771,פולג", 
                Inactive = false, 
                LocalName = "פולג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "772", 
                SearchFields = "772,הדסה ירושלים", 
                Inactive = false, 
                LocalName = "הדסה ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "773", 
                SearchFields = "773,ככר חרות", 
                Inactive = false, 
                LocalName = "ככר חרות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "774", 
                SearchFields = "774,נמל תעופה בן גוריון", 
                Inactive = false, 
                LocalName = "נמל תעופה בן גוריון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "775", 
                SearchFields = "775,היעלים", 
                Inactive = false, 
                LocalName = "היעלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "776", 
                SearchFields = "776,מגדיאל", 
                Inactive = false, 
                LocalName = "מגדיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "777", 
                SearchFields = "777,נוה חן", 
                Inactive = false, 
                LocalName = "נוה חן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "778", 
                SearchFields = "778,אינשטיין", 
                Inactive = false, 
                LocalName = "אינשטיין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "78", 
                SearchFields = "78,רחוב הבנקים", 
                Inactive = false, 
                LocalName = "רחוב הבנקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "780", 
                SearchFields = "780,קאסם", 
                Inactive = false, 
                LocalName = "קאסם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "781", 
                SearchFields = "781,מונטיפיורי", 
                Inactive = false, 
                LocalName = "מונטיפיורי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "782", 
                SearchFields = "782,רחביה", 
                Inactive = false, 
                LocalName = "רחביה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "783", 
                SearchFields = "783,ככר ציון", 
                Inactive = false, 
                LocalName = "ככר ציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "784", 
                SearchFields = "784,הגבעה הצרפתית", 
                Inactive = false, 
                LocalName = "הגבעה הצרפתית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "785", 
                SearchFields = "785,נאות רחל", 
                Inactive = false, 
                LocalName = "נאות רחל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "786", 
                SearchFields = "786,כיכר השבטים", 
                Inactive = false, 
                LocalName = "כיכר השבטים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "788", 
                SearchFields = "788,למד", 
                Inactive = false, 
                LocalName = "למד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "789", 
                SearchFields = "789,אופקים", 
                Inactive = false, 
                LocalName = "אופקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "79", 
                SearchFields = "79,אור יהודה", 
                Inactive = false, 
                LocalName = "אור יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "790", 
                SearchFields = "790,מג'ד אל כרום", 
                Inactive = false, 
                LocalName = "מג'ד אל כרום", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "791", 
                SearchFields = "791,קרית ספיר", 
                Inactive = false, 
                LocalName = "קרית ספיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "792", 
                SearchFields = "792,''שכונה י''''א", 
                Inactive = false, 
                LocalName = "שכונה י''''א", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "794", 
                SearchFields = "794,מרכז תפעולי ארצי לסחר חוץ", 
                Inactive = false, 
                LocalName = "מרכז תפעולי ארצי לסחר חוץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "795", 
                SearchFields = "795,כיכר ההגנה", 
                Inactive = false, 
                LocalName = "כיכר ההגנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "797", 
                SearchFields = "797,ניירות ערך-תפעול", 
                Inactive = false, 
                LocalName = "ניירות ערך-תפעול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "798", 
                SearchFields = "798,רמות", 
                Inactive = false, 
                LocalName = "רמות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "799", 
                SearchFields = "799,הנהלה ראשית", 
                Inactive = false, 
                LocalName = "הנהלה ראשית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "8", 
                SearchFields = "8,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "80", 
                SearchFields = "80,בורסת היהלומים", 
                Inactive = false, 
                LocalName = "בורסת היהלומים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "800", 
                SearchFields = "800,אלמסיון", 
                Inactive = false, 
                LocalName = "אלמסיון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "801", 
                SearchFields = "801,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "802", 
                SearchFields = "802,רמאללה-אל כוליה", 
                Inactive = false, 
                LocalName = "רמאללה-אל כוליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "803", 
                SearchFields = "803,חברון אל-שלאלה", 
                Inactive = false, 
                LocalName = "חברון אל-שלאלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "804", 
                SearchFields = "804,ג'נין", 
                Inactive = false, 
                LocalName = "ג'נין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "805", 
                SearchFields = "805,אל-קודס", 
                Inactive = false, 
                LocalName = "אל-קודס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "806", 
                SearchFields = "806,טול-כרם", 
                Inactive = false, 
                LocalName = "טול-כרם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "807", 
                SearchFields = "807,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "808", 
                SearchFields = "808,קלקיליה", 
                Inactive = false, 
                LocalName = "קלקיליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "809", 
                SearchFields = "809,יריחו", 
                Inactive = false, 
                LocalName = "יריחו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "81", 
                SearchFields = "81,שכונת שפירא", 
                Inactive = false, 
                LocalName = "שכונת שפירא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "810", 
                SearchFields = "810,המלך פייסל", 
                Inactive = false, 
                LocalName = "המלך פייסל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "811", 
                SearchFields = "811,מוניטפיורי", 
                Inactive = false, 
                LocalName = "מוניטפיורי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "812", 
                SearchFields = "812,שכם-איסלאמי", 
                Inactive = false, 
                LocalName = "שכם-איסלאמי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "813", 
                SearchFields = "813,חברון-איסלאמי", 
                Inactive = false, 
                LocalName = "חברון-איסלאמי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "814", 
                SearchFields = "814,עזה-איסלאמי", 
                Inactive = false, 
                LocalName = "עזה-איסלאמי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "815", 
                SearchFields = "815,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "816", 
                SearchFields = "816,סראיה", 
                Inactive = false, 
                LocalName = "סראיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "817", 
                SearchFields = "817,דיר אלבלח", 
                Inactive = false, 
                LocalName = "דיר אלבלח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "818", 
                SearchFields = "818,רפיח", 
                Inactive = false, 
                LocalName = "רפיח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "819", 
                SearchFields = "819,אל-רימאל", 
                Inactive = false, 
                LocalName = "אל-רימאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "82", 
                SearchFields = "82,הדר הכרמל", 
                Inactive = false, 
                LocalName = "הדר הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "820", 
                SearchFields = "820,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "821", 
                SearchFields = "821,עין שרה", 
                Inactive = false, 
                LocalName = "עין שרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "822", 
                SearchFields = "822,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "823", 
                SearchFields = "823,פלורנטין", 
                Inactive = false, 
                LocalName = "פלורנטין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "824", 
                SearchFields = "824,שדרות נורדאו", 
                Inactive = false, 
                LocalName = "שדרות נורדאו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "825", 
                SearchFields = "825,אל ראם", 
                Inactive = false, 
                LocalName = "אל ראם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "826", 
                SearchFields = "826,אל עזריה (ביתוניה)", 
                Inactive = false, 
                LocalName = "אל עזריה (ביתוניה)", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "827", 
                SearchFields = "827,רמאללה אזור תעשיה", 
                Inactive = false, 
                LocalName = "רמאללה אזור תעשיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "828", 
                SearchFields = "828,צהלון", 
                Inactive = false, 
                LocalName = "צהלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "829", 
                SearchFields = "829,טול כרם", 
                Inactive = false, 
                LocalName = "טול כרם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "83", 
                SearchFields = "83,הרצל", 
                Inactive = false, 
                LocalName = "הרצל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "830", 
                SearchFields = "830,עראבה", 
                Inactive = false, 
                LocalName = "עראבה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "831", 
                SearchFields = "831,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "832", 
                SearchFields = "832,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "833", 
                SearchFields = "833,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "834", 
                SearchFields = "834,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "835", 
                SearchFields = "835,נוסירת", 
                Inactive = false, 
                LocalName = "נוסירת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "836", 
                SearchFields = "836,רמאללה", 
                Inactive = false, 
                LocalName = "רמאללה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "837", 
                SearchFields = "837,יהוד", 
                Inactive = false, 
                LocalName = "יהוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "838", 
                SearchFields = "838,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "839", 
                SearchFields = "839,ואדי אלתופאח", 
                Inactive = false, 
                LocalName = "ואדי אלתופאח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "84", 
                SearchFields = "84,ככר מגן דוד", 
                Inactive = false, 
                LocalName = "ככר מגן דוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "840", 
                SearchFields = "840,אלעזריה", 
                Inactive = false, 
                LocalName = "אלעזריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "841", 
                SearchFields = "841,הילטון", 
                Inactive = false, 
                LocalName = "הילטון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "842", 
                SearchFields = "842,המלאכה", 
                Inactive = false, 
                LocalName = "המלאכה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "843", 
                SearchFields = "843,רמאללה", 
                Inactive = false, 
                LocalName = "רמאללה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "844", 
                SearchFields = "844,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "845", 
                SearchFields = "845,אחוזת בית", 
                Inactive = false, 
                LocalName = "אחוזת בית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "848", 
                SearchFields = "848,מגדלור", 
                Inactive = false, 
                LocalName = "מגדלור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "849", 
                SearchFields = "849,המשרד המרכזי-החטיבה לכספים", 
                Inactive = false, 
                LocalName = "המשרד המרכזי-החטיבה לכספים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "85", 
                SearchFields = "85,ככר יצחק רבין", 
                Inactive = false, 
                LocalName = "ככר יצחק רבין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "850", 
                SearchFields = "850,דפנה", 
                Inactive = false, 
                LocalName = "דפנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "851", 
                SearchFields = "851,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "852", 
                SearchFields = "852,רמאללה, אל-מנארה", 
                Inactive = false, 
                LocalName = "רמאללה, אל-מנארה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "853", 
                SearchFields = "853,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "854", 
                SearchFields = "854,ג'נין", 
                Inactive = false, 
                LocalName = "ג'נין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "855", 
                SearchFields = "855,טול-כרם", 
                Inactive = false, 
                LocalName = "טול-כרם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "856", 
                SearchFields = "856,קלקיליה", 
                Inactive = false, 
                LocalName = "קלקיליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "857", 
                SearchFields = "857,רמאללה-העיר", 
                Inactive = false, 
                LocalName = "רמאללה-העיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "858", 
                SearchFields = "858,חברון - אלסלאם", 
                Inactive = false, 
                LocalName = "חברון - אלסלאם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "859", 
                SearchFields = "859,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "86", 
                SearchFields = "86,רמת אביב", 
                Inactive = false, 
                LocalName = "רמת אביב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "860", 
                SearchFields = "860,פרדס כץ", 
                Inactive = false, 
                LocalName = "פרדס כץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "861", 
                SearchFields = "861,אל-ראם", 
                Inactive = false, 
                LocalName = "אל-ראם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "862", 
                SearchFields = "862,הרמה", 
                Inactive = false, 
                LocalName = "הרמה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "863", 
                SearchFields = "863,הרחוב הראשי - ביר זית", 
                Inactive = false, 
                LocalName = "הרחוב הראשי - ביר זית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "864", 
                SearchFields = "864,רמאללה - אל בלד", 
                Inactive = false, 
                LocalName = "רמאללה - אל בלד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "865", 
                SearchFields = "865,סניף עסקים", 
                Inactive = false, 
                LocalName = "סניף עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "866", 
                SearchFields = "866,אלכדר", 
                Inactive = false, 
                LocalName = "אלכדר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "867", 
                SearchFields = "867,נוה שרת", 
                Inactive = false, 
                LocalName = "נוה שרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "868", 
                SearchFields = "868,שטמפפר", 
                Inactive = false, 
                LocalName = "שטמפפר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "869", 
                SearchFields = "869,עסקר - שכם", 
                Inactive = false, 
                LocalName = "עסקר - שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "870", 
                SearchFields = "870,רפידיה - שכם", 
                Inactive = false, 
                LocalName = "רפידיה - שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "871", 
                SearchFields = "871,טול כרם 2", 
                Inactive = false, 
                LocalName = "טול כרם 2", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "872", 
                SearchFields = "872,בידיה - קלקיליה", 
                Inactive = false, 
                LocalName = "בידיה - קלקיליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "873", 
                SearchFields = "873,סלפית", 
                Inactive = false, 
                LocalName = "סלפית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "874", 
                SearchFields = "874,טובאס", 
                Inactive = false, 
                LocalName = "טובאס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "875", 
                SearchFields = "875,אלארסאל", 
                Inactive = false, 
                LocalName = "אלארסאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "876", 
                SearchFields = "876,ראשי חיפה", 
                Inactive = false, 
                LocalName = "ראשי חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "877", 
                SearchFields = "877,פועלים בטלפון", 
                Inactive = false, 
                LocalName = "פועלים בטלפון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "878", 
                SearchFields = "878,אל-רימאל - עזה", 
                Inactive = false, 
                LocalName = "אל-רימאל - עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "879", 
                SearchFields = "879,יריחו", 
                Inactive = false, 
                LocalName = "יריחו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "88", 
                SearchFields = "88,המסגר", 
                Inactive = false, 
                LocalName = "המסגר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "880", 
                SearchFields = "880,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "881", 
                SearchFields = "881,אל-בירה", 
                Inactive = false, 
                LocalName = "אל-בירה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "882", 
                SearchFields = "882,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "883", 
                SearchFields = "883,חאן יונס", 
                Inactive = false, 
                LocalName = "חאן יונס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "884", 
                SearchFields = "884,קיבוץ גלויות", 
                Inactive = false, 
                LocalName = "קיבוץ גלויות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "885", 
                SearchFields = "885,טירת הכרמל", 
                Inactive = false, 
                LocalName = "טירת הכרמל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "886", 
                SearchFields = "886,מוריה", 
                Inactive = false, 
                LocalName = "מוריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "887", 
                SearchFields = "887,נשר", 
                Inactive = false, 
                LocalName = "נשר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "888", 
                SearchFields = "888,עזה", 
                Inactive = false, 
                LocalName = "עזה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "889", 
                SearchFields = "889,עסקים חלוצי התעשיה", 
                Inactive = false, 
                LocalName = "עסקים חלוצי התעשיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "89", 
                SearchFields = "89,רזיאל", 
                Inactive = false, 
                LocalName = "רזיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "890", 
                SearchFields = "890,החלוץ", 
                Inactive = false, 
                LocalName = "החלוץ", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "891", 
                SearchFields = "891,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "892", 
                SearchFields = "892,טול כרם", 
                Inactive = false, 
                LocalName = "טול כרם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "893", 
                SearchFields = "893,הגבורים", 
                Inactive = false, 
                LocalName = "הגבורים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "894", 
                SearchFields = "894,בית לחם", 
                Inactive = false, 
                LocalName = "בית לחם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "895", 
                SearchFields = "895,קרית טבעון", 
                Inactive = false, 
                LocalName = "קרית טבעון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "896", 
                SearchFields = "896,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "897", 
                SearchFields = "897,יריחו", 
                Inactive = false, 
                LocalName = "יריחו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "898", 
                SearchFields = "898,שכם", 
                Inactive = false, 
                LocalName = "שכם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "899", 
                SearchFields = "899,סניף ראשי", 
                Inactive = false, 
                LocalName = "סניף ראשי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "9", 
                SearchFields = "9,נצרת עלית", 
                Inactive = false, 
                LocalName = "נצרת עלית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "90", 
                SearchFields = "90,ירכא", 
                Inactive = false, 
                LocalName = "ירכא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "900", 
                SearchFields = "900,ראש הנקרה", 
                Inactive = false, 
                LocalName = "ראש הנקרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "901", 
                SearchFields = "901,ג'נין", 
                Inactive = false, 
                LocalName = "ג'נין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "902", 
                SearchFields = "902,רפיח מסוף הגבול", 
                Inactive = false, 
                LocalName = "רפיח מסוף הגבול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "903", 
                SearchFields = "903,בית המכס", 
                Inactive = false, 
                LocalName = "בית המכס", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "904", 
                SearchFields = "904,מעלה אפרים", 
                Inactive = false, 
                LocalName = "מעלה אפרים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "905", 
                SearchFields = "905,רמות אשכול", 
                Inactive = false, 
                LocalName = "רמות אשכול", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "906", 
                SearchFields = "906,שלוחת מת''ם", 
                Inactive = false, 
                LocalName = "שלוחת מת''ם", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "907", 
                SearchFields = "907,חברון", 
                Inactive = false, 
                LocalName = "חברון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "91", 
                SearchFields = "91,וינגיט", 
                Inactive = false, 
                LocalName = "וינגיט", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "911", 
                SearchFields = "911,גאולה", 
                Inactive = false, 
                LocalName = "גאולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "912", 
                SearchFields = "912,רחביה", 
                Inactive = false, 
                LocalName = "רחביה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "913", 
                SearchFields = "913,מחנה יהודה", 
                Inactive = false, 
                LocalName = "מחנה יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "914", 
                SearchFields = "914,קרית משה עסקים", 
                Inactive = false, 
                LocalName = "קרית משה עסקים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "915", 
                SearchFields = "915,שלוחת הגבעה הצרפתית", 
                Inactive = false, 
                LocalName = "שלוחת הגבעה הצרפתית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "916", 
                SearchFields = "916,המרכז לבנקאות פרטית - צפון", 
                Inactive = false, 
                LocalName = "המרכז לבנקאות פרטית - צפון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "917", 
                SearchFields = "917,בנק הפועלים", 
                Inactive = false, 
                LocalName = "בנק הפועלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "918", 
                SearchFields = "918,מזרח ירושלים", 
                Inactive = false, 
                LocalName = "מזרח ירושלים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "919", 
                SearchFields = "919,ככר ציון", 
                Inactive = false, 
                LocalName = "ככר ציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "92", 
                SearchFields = "92,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "920", 
                SearchFields = "920,ככר ציון", 
                Inactive = false, 
                LocalName = "ככר ציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "921", 
                SearchFields = "921,באר שבע", 
                Inactive = false, 
                LocalName = "באר שבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "922", 
                SearchFields = "922,מרכז הנגב", 
                Inactive = false, 
                LocalName = "מרכז הנגב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "923", 
                SearchFields = "923,דימונה", 
                Inactive = false, 
                LocalName = "דימונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "924", 
                SearchFields = "924,שדרות הנשיאים", 
                Inactive = false, 
                LocalName = "שדרות הנשיאים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "925", 
                SearchFields = "925,אשקלון", 
                Inactive = false, 
                LocalName = "אשקלון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "926", 
                SearchFields = "926,אפרידר", 
                Inactive = false, 
                LocalName = "אפרידר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "927", 
                SearchFields = "927,קרית גת", 
                Inactive = false, 
                LocalName = "קרית גת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "928", 
                SearchFields = "928,גדרה", 
                Inactive = false, 
                LocalName = "גדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "929", 
                SearchFields = "929,בילו", 
                Inactive = false, 
                LocalName = "בילו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "93", 
                SearchFields = "93,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "930", 
                SearchFields = "930,רחובות", 
                Inactive = false, 
                LocalName = "רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "931", 
                SearchFields = "931,שעריים", 
                Inactive = false, 
                LocalName = "שעריים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "932", 
                SearchFields = "932,אשדוד", 
                Inactive = false, 
                LocalName = "אשדוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "933", 
                SearchFields = "933,נס ציונה", 
                Inactive = false, 
                LocalName = "נס ציונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "934", 
                SearchFields = "934,ראשון לציון", 
                Inactive = false, 
                LocalName = "ראשון לציון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "935", 
                SearchFields = "935,כוכב יאיר", 
                Inactive = false, 
                LocalName = "כוכב יאיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "936", 
                SearchFields = "936,רמלה", 
                Inactive = false, 
                LocalName = "רמלה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "937", 
                SearchFields = "937,לוד", 
                Inactive = false, 
                LocalName = "לוד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "938", 
                SearchFields = "938,נתב''ג", 
                Inactive = false, 
                LocalName = "נתב''ג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "939", 
                SearchFields = "939,יבנה", 
                Inactive = false, 
                LocalName = "יבנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "94", 
                SearchFields = "94,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "940", 
                SearchFields = "940,פתח תקוה", 
                Inactive = false, 
                LocalName = "פתח תקוה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "941", 
                SearchFields = "941,שדרות", 
                Inactive = false, 
                LocalName = "שדרות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "942", 
                SearchFields = "942,רעננה", 
                Inactive = false, 
                LocalName = "רעננה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "943", 
                SearchFields = "943,הוד השרון", 
                Inactive = false, 
                LocalName = "הוד השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "944", 
                SearchFields = "944,שער ראשון", 
                Inactive = false, 
                LocalName = "שער ראשון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "945", 
                SearchFields = "945,מגדיאל הוד השרון", 
                Inactive = false, 
                LocalName = "מגדיאל הוד השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "946", 
                SearchFields = "946,כפר סבא", 
                Inactive = false, 
                LocalName = "כפר סבא", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "947", 
                SearchFields = "947,אורלי", 
                Inactive = false, 
                LocalName = "אורלי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "948", 
                SearchFields = "948,הרצליה", 
                Inactive = false, 
                LocalName = "הרצליה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "949", 
                SearchFields = "949,רמת השרון", 
                Inactive = false, 
                LocalName = "רמת השרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "95", 
                SearchFields = "95,גבעת שמואל", 
                Inactive = false, 
                LocalName = "גבעת שמואל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "950", 
                SearchFields = "950,נתניה", 
                Inactive = false, 
                LocalName = "נתניה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "951", 
                SearchFields = "951,המרכז לבנקאות פרטית -שרון", 
                Inactive = false, 
                LocalName = "המרכז לבנקאות פרטית -שרון", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "952", 
                SearchFields = "952,שדרות בנימין", 
                Inactive = false, 
                LocalName = "שדרות בנימין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "953", 
                SearchFields = "953,חדרה", 
                Inactive = false, 
                LocalName = "חדרה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "954", 
                SearchFields = "954,פרדס חנה", 
                Inactive = false, 
                LocalName = "פרדס חנה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "955", 
                SearchFields = "955,בנימינה", 
                Inactive = false, 
                LocalName = "בנימינה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "956", 
                SearchFields = "956,זכרון יעקב", 
                Inactive = false, 
                LocalName = "זכרון יעקב", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "957", 
                SearchFields = "957,אבן יהודה", 
                Inactive = false, 
                LocalName = "אבן יהודה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "958", 
                SearchFields = "958,הרצליה פתוח", 
                Inactive = false, 
                LocalName = "הרצליה פתוח", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "959", 
                SearchFields = "959,גבעת אולגה", 
                Inactive = false, 
                LocalName = "גבעת אולגה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "96", 
                SearchFields = "96,עפולה", 
                Inactive = false, 
                LocalName = "עפולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "960", 
                SearchFields = "960,עכו", 
                Inactive = false, 
                LocalName = "עכו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "961", 
                SearchFields = "961,כרמיאל", 
                Inactive = false, 
                LocalName = "כרמיאל", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "962", 
                SearchFields = "962,נהריה", 
                Inactive = false, 
                LocalName = "נהריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "963", 
                SearchFields = "963,רוגוזין", 
                Inactive = false, 
                LocalName = "רוגוזין", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "964", 
                SearchFields = "964,נצרת", 
                Inactive = false, 
                LocalName = "נצרת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "965", 
                SearchFields = "965,עפולה", 
                Inactive = false, 
                LocalName = "עפולה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "966", 
                SearchFields = "966,בית שאן", 
                Inactive = false, 
                LocalName = "בית שאן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "967", 
                SearchFields = "967,רמת אליהו", 
                Inactive = false, 
                LocalName = "רמת אליהו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "968", 
                SearchFields = "968,עסקים הר חוצבים", 
                Inactive = false, 
                LocalName = "עסקים הר חוצבים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "969", 
                SearchFields = "969,מחסום ארז", 
                Inactive = false, 
                LocalName = "מחסום ארז", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "97", 
                SearchFields = "97,נהריה", 
                Inactive = false, 
                LocalName = "נהריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "970", 
                SearchFields = "970,טבריה", 
                Inactive = false, 
                LocalName = "טבריה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "972", 
                SearchFields = "972,נצרת עלית", 
                Inactive = false, 
                LocalName = "נצרת עלית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "974", 
                SearchFields = "974,שביט", 
                Inactive = false, 
                LocalName = "שביט", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "975", 
                SearchFields = "975,צפת", 
                Inactive = false, 
                LocalName = "צפת", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "976", 
                SearchFields = "976,קרית שמונה", 
                Inactive = false, 
                LocalName = "קרית שמונה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "977", 
                SearchFields = "977,בלפור", 
                Inactive = false, 
                LocalName = "בלפור", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "978", 
                SearchFields = "978,עסקים רחובות", 
                Inactive = false, 
                LocalName = "עסקים רחובות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "98", 
                SearchFields = "98,טפחות ישיר", 
                Inactive = false, 
                LocalName = "טפחות ישיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "980", 
                SearchFields = "980,רב שפע", 
                Inactive = false, 
                LocalName = "רב שפע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "981", 
                SearchFields = "981,שלוחת הדר גנים", 
                Inactive = false, 
                LocalName = "שלוחת הדר גנים", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "982", 
                SearchFields = "982,עוספיה", 
                Inactive = false, 
                LocalName = "עוספיה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "983", 
                SearchFields = "983,רמת נוף", 
                Inactive = false, 
                LocalName = "רמת נוף", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "984", 
                SearchFields = "984,ברכפלד", 
                Inactive = false, 
                LocalName = "ברכפלד", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "985", 
                SearchFields = "985,קרית מלאכי", 
                Inactive = false, 
                LocalName = "קרית מלאכי", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "986", 
                SearchFields = "986,לב הרובע", 
                Inactive = false, 
                LocalName = "לב הרובע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "987", 
                SearchFields = "987,מגדל העמק", 
                Inactive = false, 
                LocalName = "מגדל העמק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "988", 
                SearchFields = "988,מ.ש.י. כניסה לקבע", 
                Inactive = false, 
                LocalName = "מ.ש.י. כניסה לקבע", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "989", 
                SearchFields = "989,שלוחת עורק", 
                Inactive = false, 
                LocalName = "שלוחת עורק", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "99", 
                SearchFields = "99,עכו", 
                Inactive = false, 
                LocalName = "עכו", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "990", 
                SearchFields = "990,מסוף קרני", 
                Inactive = false, 
                LocalName = "מסוף קרני", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "991", 
                SearchFields = "991,השגרירות האמריקאית", 
                Inactive = false, 
                LocalName = "השגרירות האמריקאית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "992", 
                SearchFields = "992,''מפח''''ש", 
                Inactive = false, 
                LocalName = "מפח''''ש", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "993", 
                SearchFields = "993,מסוף נהר הירדן", 
                Inactive = false, 
                LocalName = "מסוף נהר הירדן", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "994", 
                SearchFields = "994,שדה תעופה חיפה", 
                Inactive = false, 
                LocalName = "שדה תעופה חיפה", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "995", 
                SearchFields = "995,''מכס נת''''בג", 
                Inactive = false, 
                LocalName = "מכס נת''''בג", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "996", 
                SearchFields = "996,שלוחת שפיר", 
                Inactive = false, 
                LocalName = "שלוחת שפיר", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "997", 
                SearchFields = "997,גלילות", 
                Inactive = false, 
                LocalName = "גלילות", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "998", 
                SearchFields = "998,שלוחת עסקית", 
                Inactive = false, 
                LocalName = "שלוחת עסקית", 
			});
			 
            all.Add(new CustomsBranchDetails()
            {    
                Code = "999", 
                SearchFields = "999,אשל השומרון", 
                Inactive = false, 
                LocalName = "אשל השומרון", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsBranch newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsBranch rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

