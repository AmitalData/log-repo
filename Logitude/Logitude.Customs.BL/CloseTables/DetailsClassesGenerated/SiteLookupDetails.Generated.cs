
   
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
   public class SiteLookupDetails : SiteLookup, ICloseTable<SiteLookup, SiteLookupDetails>
   {
       public List<SiteLookupDetails> GetAll()
       {
		    var all = new List<SiteLookupDetails>();  
            all.Add(new SiteLookupDetails()
            {    
                Code = "-1", 
                SearchFields = "-1,סופה", 
                Inactive = false, 
                LocalName = "סופה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-10", 
                SearchFields = "-10,עובדה", 
                Inactive = false, 
                LocalName = "עובדה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-11", 
                SearchFields = "-11,אולם נוסעים נמל חיפה", 
                Inactive = false, 
                LocalName = "אולם נוסעים נמל חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-12", 
                SearchFields = "-12,תחנת מכס משרד הביטחון", 
                Inactive = false, 
                LocalName = "תחנת מכס משרד הביטחון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-13", 
                SearchFields = "-13,טרקלין מצדה", 
                Inactive = false, 
                LocalName = "טרקלין מצדה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-14", 
                SearchFields = "-14,אולם נוסעים  שדה תעופה אילת", 
                Inactive = false, 
                LocalName = "אולם נוסעים  שדה תעופה אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-15", 
                SearchFields = "-15,אולם נוסעים נמל אילת", 
                Inactive = false, 
                LocalName = "אולם נוסעים נמל אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-16", 
                SearchFields = "-16,חסם צהוב", 
                Inactive = false, 
                LocalName = "חסם צהוב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-2", 
                SearchFields = "-2,מזמוריה-הר חומה", 
                Inactive = false, 
                LocalName = "מזמוריה-הר חומה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-5", 
                SearchFields = "-5,אולם נוסעים אשדוד", 
                Inactive = false, 
                LocalName = "אולם נוסעים אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-6", 
                SearchFields = "-6,מטולה", 
                Inactive = false, 
                LocalName = "מטולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-7", 
                SearchFields = "-7,ראש הנקרה", 
                Inactive = false, 
                LocalName = "ראש הנקרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "-8", 
                SearchFields = "-8,קונטרה", 
                Inactive = false, 
                LocalName = "קונטרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1", 
                SearchFields = "1,בית מכס חיפה", 
                Inactive = false, 
                LocalName = "בית מכס חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10000", 
                SearchFields = "10000,קשר ימי בע''מ- ת''א", 
                Inactive = false, 
                LocalName = "קשר ימי בע''מ- ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10001", 
                SearchFields = "10001,מבט קדימה", 
                Inactive = false, 
                LocalName = "מבט קדימה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10002", 
                SearchFields = "10002,יפנאוטו חב' ישראלית לרכב בע''מ- חיפה", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10003", 
                SearchFields = "10003,יפנאוטו חב' ישראלית לרכב בע''מ- עכו", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10004", 
                SearchFields = "10004,יפנאוטו חב' ישראלית לרכב בע''מ- נצרת", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10005", 
                SearchFields = "10005,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 19", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 19", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10006", 
                SearchFields = "10006,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 20", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 20", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10007", 
                SearchFields = "10007,יפנאוטו חב' ישראלית לרכב בע''מ- מעלות", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- מעלות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10008", 
                SearchFields = "10008,יפנאוטו חב' ישראלית לרכב בע''מ-ת''א", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ-ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10009", 
                SearchFields = "10009,יפנאוטו חב' ישראלית לרכב בע''מ- הרצליה", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10010", 
                SearchFields = "10010,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 21", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 21", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10011", 
                SearchFields = "10011,יפנאוטו חב' ישראלית לרכב בע''מ- רחובות", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10012", 
                SearchFields = "10012,יפנאוטו חב' ישראלית לרכב בע''מ- יפה", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- יפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10013", 
                SearchFields = "10013,יפנאוטו חב' ישראלית לרכב בע''מ- נתניה", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10014", 
                SearchFields = "10014,יפנאוטו חב' ישראלית לרכב בע''מ- רעננה", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- רעננה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10015", 
                SearchFields = "10015,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10016", 
                SearchFields = "10016,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10017", 
                SearchFields = "10017,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10018", 
                SearchFields = "10018,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10019", 
                SearchFields = "10019,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10020", 
                SearchFields = "10020,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10021", 
                SearchFields = "10021,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10022", 
                SearchFields = "10022,יפנאוטו חב' ישראלית לרכב בע''מ- ב''ש", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10023", 
                SearchFields = "10023,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10024", 
                SearchFields = "10024,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10025", 
                SearchFields = "10025,יפנאוטו חב' ישראלית לרכב בע''מ- אשדוד", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10026", 
                SearchFields = "10026,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10027", 
                SearchFields = "10027,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10028", 
                SearchFields = "10028,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 12", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10029", 
                SearchFields = "10029,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 13", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10030", 
                SearchFields = "10030,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 14", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10031", 
                SearchFields = "10031,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 15", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10032", 
                SearchFields = "10032,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 16", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 16", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10033", 
                SearchFields = "10033,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 17", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 17", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10034", 
                SearchFields = "10034,יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 18", 
                Inactive = false, 
                LocalName = "יפנאוטו חב' ישראלית לרכב בע''מ- לא פעיל 18", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10035", 
                SearchFields = "10035,סוכנות מכוניות לים התיכון בע''מ- עכו", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''מ- עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10036", 
                SearchFields = "10036,סוכנות מכוניות לים התיכון בע''מ- חיפה", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10037", 
                SearchFields = "10037,סוכנות מכוניות לים התיכון בע''מ- ת''א", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''מ- ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10038", 
                SearchFields = "10038,א.ב ליגל סוכנות תחבורה בע''מ   - לא פעיל 1", 
                Inactive = false, 
                LocalName = "א.ב ליגל סוכנות תחבורה בע''מ   - לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10039", 
                SearchFields = "10039,א.ב ליגל סוכנות תחבורה בע''מ    - לא פעיל 2", 
                Inactive = false, 
                LocalName = "א.ב ליגל סוכנות תחבורה בע''מ    - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10040", 
                SearchFields = "10040,דוד לובינסקי בע''מ- חיפה", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10041", 
                SearchFields = "10041,דוד לובינסקי בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10042", 
                SearchFields = "10042,דוד לובינסקי בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10043", 
                SearchFields = "10043,דוד לובינסקי בע''מ-נצרת", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ-נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10044", 
                SearchFields = "10044,דוד לובינסקי בע''מ- עפולה", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- עפולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10045", 
                SearchFields = "10045,דוד לובינסקי בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10046", 
                SearchFields = "10046,דוד לובינסקי בע''מ- נהריה", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- נהריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10047", 
                SearchFields = "10047,דוד לובינסקי בע''מ- כרמיאל", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- כרמיאל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10048", 
                SearchFields = "10048,דוד לובינסקי בע''מ- ב''ש", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10049", 
                SearchFields = "10049,דוד לובינסקי בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10050", 
                SearchFields = "10050,דוד לובינסקי בע''מ-אשדוד", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ-אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10051", 
                SearchFields = "10051,דוד לובינסקי בע''מ- אשקלון", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10052", 
                SearchFields = "10052,דוד לובינסקי בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10053", 
                SearchFields = "10053,דוד לובינסקי בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10054", 
                SearchFields = "10054,דוד לובינסקי בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10055", 
                SearchFields = "10055,דוד לובינסקי בע''מ- בני ברק אולם 1", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- בני ברק אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10056", 
                SearchFields = "10056,דוד לובינסקי בע''מ- בני ברק אולם 2", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- בני ברק אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10057", 
                SearchFields = "10057,דוד לובינסקי בע''מ- רחובות", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10058", 
                SearchFields = "10058,דוד לובינסקי בע''מ- הרצליה", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10059", 
                SearchFields = "10059,דוד לובינסקי בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10060", 
                SearchFields = "10060,דוד לובינסקי בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10061", 
                SearchFields = "10061,דוד לובינסקי בע''מ- נתניה", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10062", 
                SearchFields = "10062,דוד לובינסקי בע''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10063", 
                SearchFields = "10063,דוד לובינסקי בע''מ- ת''א אולם 1", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- ת''א אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10064", 
                SearchFields = "10064,דוד לובינסקי בע''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10065", 
                SearchFields = "10065,דוד לובינסקי בע''מ- ת''א אולם 2", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- ת''א אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10066", 
                SearchFields = "10066,דוד לובינסקי בע''מ- ראש''ל", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- ראש''ל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10067", 
                SearchFields = "10067,דוד לובינסקי בע''מ- כפר סבא", 
                Inactive = false, 
                LocalName = "דוד לובינסקי בע''מ- כפר סבא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10068", 
                SearchFields = "10068,מאיר חברה למכוניות ומשאיות בע''''מ- חיפה אולם 1", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- חיפה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10069", 
                SearchFields = "10069,מאיר חברה למכוניות ומשאיות בע''''מ- חיפה האלשג", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- חיפה האלשג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10070", 
                SearchFields = "10070,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10071", 
                SearchFields = "10071,מאיר חברה למכוניות ומשאיות בע''''מ- חדרה", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10072", 
                SearchFields = "10072,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10073", 
                SearchFields = "10073,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10074", 
                SearchFields = "10074,מאיר חברה למכוניות ומשאיות בע''''מ-עפולה", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ-עפולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10075", 
                SearchFields = "10075,מאיר חברה למכוניות ומשאיות בע''''מ- מעלות", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- מעלות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10076", 
                SearchFields = "10076,מאיר חברה למכוניות ומשאיות בע''''מ- חיפה אולם 2", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- חיפה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10077", 
                SearchFields = "10077,מאיר חברה למכוניות ומשאיות בע''''מ- נצרת", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10078", 
                SearchFields = "10078,מאיר חברה למכוניות ומשאיות בע''''מ- ב''ש", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10079", 
                SearchFields = "10079,מאיר חברה למכוניות ומשאיות בע''''מ- אשקלון", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10080", 
                SearchFields = "10080,מאיר חברה למכוניות ומשאיות בע''''מ- אשדוד", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10081", 
                SearchFields = "10081,מאיר חברה למכוניות ומשאיות בע''''מ- באר שבע", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10082", 
                SearchFields = "10082,מאיר חברה למכוניות ומשאיות בע''''מ- ת''א אולם 1", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- ת''א אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10083", 
                SearchFields = "10083,מאיר חברה למכוניות ומשאיות בע''''מ- רחובות", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10084", 
                SearchFields = "10084,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10085", 
                SearchFields = "10085,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10086", 
                SearchFields = "10086,מאיר חברה למכוניות ומשאיות בע''''מ- פתח תקווה", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- פתח תקווה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10087", 
                SearchFields = "10087,מאיר חברה למכוניות ומשאיות בע''''מ- ת''א המסגר", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- ת''א המסגר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10088", 
                SearchFields = "10088,מאיר חברה למכוניות ומשאיות בע''''מ- ת''א אולם 2", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- ת''א אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10089", 
                SearchFields = "10089,מאיר חברה למכוניות ומשאיות בע''''מ- נתניה", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10090", 
                SearchFields = "10090,מאיר חברה למכוניות ומשאיות בע''''מ- הרצליה", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10091", 
                SearchFields = "10091,מאיר חברה למכוניות ומשאיות בע''''מ- ראשון לציון", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- ראשון לציון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10092", 
                SearchFields = "10092,קרסו מוטורס (7002) בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10093", 
                SearchFields = "10093,קרסו מוטורס 2007 בע''מ-חיפה", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10094", 
                SearchFields = "10094,קרסו מוטורס 2007 בע''מ- כרמיאל", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- כרמיאל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10095", 
                SearchFields = "10095,קרסו מוטורס 2007 בע''מ-טבריה", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10096", 
                SearchFields = "10096,קרסו מוטורס 2007 בע''מ-חיפה ההסתדרות", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-חיפה ההסתדרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10097", 
                SearchFields = "10097,קרסו מוטורס 2007 בע''מ-חיפה  יפו", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-חיפה  יפו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10098", 
                SearchFields = "10098,קרסו מוטורס (7002) בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10099", 
                SearchFields = "10099,קרסו מוטורס 2007 בע''מ- נצרת", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10100", 
                SearchFields = "10100,קרסו מוטורס 2007 בע''מ-רחובות", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10101", 
                SearchFields = "10101,קרסו מוטורס 2007 בע''מ- ת''א בן צבי", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- ת''א בן צבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10102", 
                SearchFields = "10102,קרסו מוטורס (7002) בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10103", 
                SearchFields = "10103,קרסו מוטורס (7002) בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10104", 
                SearchFields = "10104,קרסו מוטורס (7002) בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10105", 
                SearchFields = "10105,קרסו מוטורס 2007 בע''מ- ת''א יד חרוצים", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- ת''א יד חרוצים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10106", 
                SearchFields = "10106,קרסו מוטורס 2007 בע''מ- הרצליה אולם 1", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- הרצליה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10107", 
                SearchFields = "10107,קרסו מוטורס (7002) בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10108", 
                SearchFields = "10108,קרסו מוטורס (7002) בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10109", 
                SearchFields = "10109,קרסו מוטורס (7002) בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10110", 
                SearchFields = "10110,קרסו מוטורס (7002) בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10111", 
                SearchFields = "10111,קרסו מוטורס (7002) בע''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10112", 
                SearchFields = "10112,קרסו מוטורס (7002) בע''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "קרסו מוטורס (7002) בע''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10113", 
                SearchFields = "10113,קרסו מוטורס 2007 בע''מ- ת''א ריב''ל", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- ת''א ריב''ל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10114", 
                SearchFields = "10114,קרסו מוטורס 2007 בע''מ- הרצליה אולם 2", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- הרצליה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10115", 
                SearchFields = "10115,קרסו מוטורס2007 בע''מ-רעננה אולם 1", 
                Inactive = false, 
                LocalName = "קרסו מוטורס2007 בע''מ-רעננה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10116", 
                SearchFields = "10116,קרסו מוטורס 2007 בע''מ-רעננה אולם 2", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ-רעננה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10117", 
                SearchFields = "10117,דלק מוטורס בע''מ- נצרת", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10118", 
                SearchFields = "10118,דלק מוטורס בע''מ- חיפה אולם 1", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- חיפה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10119", 
                SearchFields = "10119,דלק מוטורס בע''מ- טבריה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10120", 
                SearchFields = "10120,דלק מוטורס בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10121", 
                SearchFields = "10121,דלק מוטורס בע''מ- אולם 2", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10122", 
                SearchFields = "10122,דלק מוטורס בע''מ- מפרץ חיפה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- מפרץ חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10123", 
                SearchFields = "10123,דלק מוטורס בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10124", 
                SearchFields = "10124,דלק מוטורס בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10125", 
                SearchFields = "10125,דלק מוטורס בע''מ- אשדוד", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10126", 
                SearchFields = "10126,דלק מוטורס בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10127", 
                SearchFields = "10127,דלק מוטורס בע''מ- באר שבע", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10128", 
                SearchFields = "10128,דלק מוטורס בע''מ- ת''א   יצחק שדה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- ת''א   יצחק שדה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10129", 
                SearchFields = "10129,דלק מוטורס בע''מ- נתניה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10130", 
                SearchFields = "10130,דלק מוטורס בע''מ- ת''א המלאכה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- ת''א המלאכה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10131", 
                SearchFields = "10131,דלק מוטורס בע''מ- רעננה", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- רעננה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10132", 
                SearchFields = "10132,מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות בע''''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10133", 
                SearchFields = "10133,בני משה קרסו בע''''מ  - לא פעיל 1", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10134", 
                SearchFields = "10134,בני משה קרסו בע''''מ  - לא פעיל 2", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10135", 
                SearchFields = "10135,בני משה קרסו בע''''מ  - לא פעיל 3", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10136", 
                SearchFields = "10136,בני משה קרסו בע''''מ  - לא פעיל 4", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10137", 
                SearchFields = "10137,בני משה קרסו בע''''מ  - לא פעיל 5", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10138", 
                SearchFields = "10138,בני משה קרסו בע''''מ  - לא פעיל 6", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10139", 
                SearchFields = "10139,בני משה קרסו בע''''מ  - לא פעיל 7", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10140", 
                SearchFields = "10140,בני משה קרסו בע''''מ  - לא פעיל 8", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10141", 
                SearchFields = "10141,בני משה קרסו בע''''מ  - לא פעיל 9", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10142", 
                SearchFields = "10142,בני משה קרסו בע''''מ  - לא פעיל 10", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10143", 
                SearchFields = "10143,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10144", 
                SearchFields = "10144,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10145", 
                SearchFields = "10145,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10146", 
                SearchFields = "10146,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10147", 
                SearchFields = "10147,סוכנות מכוניות לים התיכון בע''''מ - אשדוד", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10148", 
                SearchFields = "10148,סוכנות מכוניות לים התיכון בע''''מ באר שבע", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10149", 
                SearchFields = "10149,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10150", 
                SearchFields = "10150,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10151", 
                SearchFields = "10151,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 12", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10152", 
                SearchFields = "10152,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 13", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10153", 
                SearchFields = "10153,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 1", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10154", 
                SearchFields = "10154,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 2", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10155", 
                SearchFields = "10155,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 3", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10156", 
                SearchFields = "10156,ק מ י קוריאה מוטורס ישראל בע''''מ", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10157", 
                SearchFields = "10157,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 4", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10158", 
                SearchFields = "10158,חב' עופר יבואני רכב בע''מ- ת''א אולם 1", 
                Inactive = false, 
                LocalName = "חב' עופר יבואני רכב בע''מ- ת''א אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10159", 
                SearchFields = "10159,חב' עופר יבואני רכב בע''מ- ת''א אולם 2", 
                Inactive = false, 
                LocalName = "חב' עופר יבואני רכב בע''מ- ת''א אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1016", 
                SearchFields = "1016,סביריט בעמבה", 
                Inactive = false, 
                LocalName = "סביריט בעמבה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10160", 
                SearchFields = "10160,חב' מטרו מוטור שווק (1981) בע''''מ-לא פעיל 1", 
                Inactive = false, 
                LocalName = "חב' מטרו מוטור שווק (1981) בע''''מ-לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10161", 
                SearchFields = "10161,חב' מטרו מוטור שווק (1981) בע''מ", 
                Inactive = false, 
                LocalName = "חב' מטרו מוטור שווק (1981) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10162", 
                SearchFields = "10162,חב' מטרו מוטור שווק (1981) בע''''מ-לא פעיל 2", 
                Inactive = false, 
                LocalName = "חב' מטרו מוטור שווק (1981) בע''''מ-לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10163", 
                SearchFields = "10163,אבניר חברה לרכב בע''''מ", 
                Inactive = false, 
                LocalName = "אבניר חברה לרכב בע''''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10164", 
                SearchFields = "10164,קמור רכב בע''''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10165", 
                SearchFields = "10165,קמור רכב בע''''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10166", 
                SearchFields = "10166,קמור רכב בע''''מ- נשר", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- נשר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10167", 
                SearchFields = "10167,קמור רכב בע''''מ- ת''א אולם 1", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- ת''א אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10168", 
                SearchFields = "10168,קמור רכב בע''''מ- ת''א בן יהודה", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- ת''א בן יהודה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10169", 
                SearchFields = "10169,קמור רכב בע''''מ- ת''א אולם 2", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- ת''א אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10170", 
                SearchFields = "10170,קמור רכב בע''''מ- פ''ת", 
                Inactive = false, 
                LocalName = "קמור רכב בע''''מ- פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10171", 
                SearchFields = "10171,לינקסים לא פעיל 1", 
                Inactive = false, 
                LocalName = "לינקסים לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10172", 
                SearchFields = "10172,לינקסים לא פעיל 2", 
                Inactive = false, 
                LocalName = "לינקסים לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10173", 
                SearchFields = "10173,חב' המזרח לשיווק מכוניות (1994)בע''''מ- מושב היוגב", 
                Inactive = false, 
                LocalName = "חב' המזרח לשיווק מכוניות (1994)בע''''מ- מושב היוגב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10174", 
                SearchFields = "10174,חב' המזרח לשיווק מכוניות (1994)בע''''מ- אשדוד", 
                Inactive = false, 
                LocalName = "חב' המזרח לשיווק מכוניות (1994)בע''''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10175", 
                SearchFields = "10175,חב' המזרח לשיווק מכוניות (1994)בע''''מ- פ''ת", 
                Inactive = false, 
                LocalName = "חב' המזרח לשיווק מכוניות (1994)בע''''מ- פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10176", 
                SearchFields = "10176,חב' המזרח לשיווק מכוניות (1994)בע''''מ- קיבוץ שפיים", 
                Inactive = false, 
                LocalName = "חב' המזרח לשיווק מכוניות (1994)בע''''מ- קיבוץ שפיים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10177", 
                SearchFields = "10177,חב' המזרח לשיווק מכוניות (1994)בע''''מ- ת''א", 
                Inactive = false, 
                LocalName = "חב' המזרח לשיווק מכוניות (1994)בע''''מ- ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10178", 
                SearchFields = "10178,כלמוטור בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10179", 
                SearchFields = "10179,כלמוטור בע''מ- חדרה", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10180", 
                SearchFields = "10180,כלמוטור בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10181", 
                SearchFields = "10181,כלמוטור בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10182", 
                SearchFields = "10182,כלמוטור בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10183", 
                SearchFields = "10183,כלמוטור בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10184", 
                SearchFields = "10184,כלמוטור בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10185", 
                SearchFields = "10185,כלמוטור בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10186", 
                SearchFields = "10186,כלמוטור בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10187", 
                SearchFields = "10187,כלמוטור בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10188", 
                SearchFields = "10188,כלמוטור בע''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10189", 
                SearchFields = "10189,כלמוטור בע''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10190", 
                SearchFields = "10190,כלמוטור בע''מ- לא פעיל 12", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10191", 
                SearchFields = "10191,כלמוטור בע''מ- לא פעיל 13", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10192", 
                SearchFields = "10192,כלמוטור בע''מ- לא פעיל 14", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10193", 
                SearchFields = "10193,כלמוטור בע''מ- לא פעיל 15", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10194", 
                SearchFields = "10194,כלמוטור בע''מ- לא פעיל 16", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 16", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10195", 
                SearchFields = "10195,כלמוטור בע''מ- לא פעיל 17", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 17", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10196", 
                SearchFields = "10196,כלמוטור בע''מ- לא פעיל 18", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 18", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10197", 
                SearchFields = "10197,כלמוטור בע''מ- לא פעיל 19", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ- לא פעיל 19", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10198", 
                SearchFields = "10198,כולמוביל בע''מ- עפולה", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- עפולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10199", 
                SearchFields = "10199,כולמוביל בע''מ- חיפה ההסתדרות", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- חיפה ההסתדרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10200", 
                SearchFields = "10200,כולמוביל בע''מ- חדרה", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10201", 
                SearchFields = "10201,כולמוביל בע''מ- עכו", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10202", 
                SearchFields = "10202,כולמוביל בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10203", 
                SearchFields = "10203,כולמוביל בע''מ- טבריה", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10204", 
                SearchFields = "10204,כולמוביל בע''מ- חיפה אולם 1", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- חיפה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10205", 
                SearchFields = "10205,כולמוביל בע''מ- כפר כנא", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- כפר כנא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10206", 
                SearchFields = "10206,כולמוביל בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10207", 
                SearchFields = "10207,כולמוביל בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10208", 
                SearchFields = "10208,כולמוביל בע''מ- חיפה אולם 2", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- חיפה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10209", 
                SearchFields = "10209,כולמוביל בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10210", 
                SearchFields = "10210,כולמוביל בע''מ- קריית שמונה", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- קריית שמונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10211", 
                SearchFields = "10211,כולמוביל בע''מ- ב''ש רח' היוצרים", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ב''ש רח' היוצרים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10212", 
                SearchFields = "10212,כולמוביל בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10213", 
                SearchFields = "10213,כולמוביל בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10214", 
                SearchFields = "10214,כולמוביל בע''מ- באר שבע", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10215", 
                SearchFields = "10215,כולמוביל בע''מ- אשקלון", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10216", 
                SearchFields = "10216,כולמוביל בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10217", 
                SearchFields = "10217,כולמוביל בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10218", 
                SearchFields = "10218,כולמוביל בע''מ- אשדוד א.ת צפוני", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- אשדוד א.ת צפוני", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10219", 
                SearchFields = "10219,כולמוביל בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10220", 
                SearchFields = "10220,כולמוביל בע''מ- אשדוד רח' הבושם", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- אשדוד רח' הבושם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10221", 
                SearchFields = "10221,כולמוביל בע''מ ב''ש רח' העמל", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ ב''ש רח' העמל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10222", 
                SearchFields = "10222,כולמוביל בע''מ- ת''א רח' תוצרת הארץ", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ת''א רח' תוצרת הארץ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10223", 
                SearchFields = "10223,כולמוביל בע''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10224", 
                SearchFields = "10224,כולמוביל בע''מ- ת''א ביזניס סנטר", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ת''א ביזניס סנטר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10225", 
                SearchFields = "10225,כולמוביל בע''מ- ת''א יגאל אלון", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ת''א יגאל אלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10226", 
                SearchFields = "10226,כולמוביל בע''מ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10227", 
                SearchFields = "10227,כולמוביל בע''מ- ראשון לציון", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ראשון לציון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10228", 
                SearchFields = "10228,כולמוביל בע''מ- יהוד", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- יהוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10229", 
                SearchFields = "10229,כולמוביל בע''מ- הרצליה", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10230", 
                SearchFields = "10230,כולמוביל בע''מ- לא פעיל 12", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10231", 
                SearchFields = "10231,כולמוביל בע''מ- נתניה אולם 1", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- נתניה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10232", 
                SearchFields = "10232,כולמוביל בע''מ- לא פעיל 13", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10233", 
                SearchFields = "10233,כולמוביל בע''מ- רעננה רח' זרחין", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- רעננה רח' זרחין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10234", 
                SearchFields = "10234,כולמוביל בע''מ- ת''א המסגר", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ת''א המסגר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10235", 
                SearchFields = "10235,כולמוביל בע''מ- לא פעיל 14", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10236", 
                SearchFields = "10236,כולמוביל בע''מ - רחובות", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ - רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10237", 
                SearchFields = "10237,כולמוביל בע''מ- לא פעיל 15", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10238", 
                SearchFields = "10238,כולמוביל בע''מ- לא פעיל 16", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 16", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10239", 
                SearchFields = "10239,כולמוביל בע''מ- נתניה אולם 2", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- נתניה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1024", 
                SearchFields = "1024,קשר ימי בע''מ", 
                Inactive = false, 
                LocalName = "קשר ימי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10240", 
                SearchFields = "10240,כולמוביל בע''מ- לא פעיל 17", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- לא פעיל 17", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10241", 
                SearchFields = "10241,כולמוביל בע''מ- ת''א דרך בן צבי", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- ת''א דרך בן צבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10242", 
                SearchFields = "10242,כולמוביל בע''מ- חולון", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- חולון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10243", 
                SearchFields = "10243,כולמוביל בע''מ- בני ברק", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- בני ברק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10244", 
                SearchFields = "10244,כולמוביל בע''מ- רעננה אולם 1", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- רעננה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10245", 
                SearchFields = "10245,כולמוביל בע''מ- רשל''צ אולם 1", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- רשל''צ אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10246", 
                SearchFields = "10246,כולמוביל בע''מ- רשל''צ אולם 2", 
                Inactive = false, 
                LocalName = "כולמוביל בע''מ- רשל''צ אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10247", 
                SearchFields = "10247,טלקאר חברה בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10248", 
                SearchFields = "10248,טלקאר חברה בע''מ- ביאליק", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- ביאליק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10249", 
                SearchFields = "10249,טלקאר חברה בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10250", 
                SearchFields = "10250,טלקאר חברה בע''מ-מפרץ  חיפה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ-מפרץ  חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10251", 
                SearchFields = "10251,טלקאר חברה בע''מ- טבריה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10252", 
                SearchFields = "10252,טלקאר חברה בע''מ- חיפה יפו", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- חיפה יפו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10253", 
                SearchFields = "10253,טלקאר חברה בע''מ- קרית שמונה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- קרית שמונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10254", 
                SearchFields = "10254,טלקאר חברה בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10255", 
                SearchFields = "10255,טלקאר חברה בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10256", 
                SearchFields = "10256,טלקאר חברה בע''מ- עכו", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10257", 
                SearchFields = "10257,טלקאר חברה בע''מ- נצרת", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10258", 
                SearchFields = "10258,טלקאר חברה בע''מ- חדרה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10259", 
                SearchFields = "10259,טלקאר חברה בע''מ- כרמיאל", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- כרמיאל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10260", 
                SearchFields = "10260,טלקאר חברה בע''מ-חיפה ההסתדרות", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ-חיפה ההסתדרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10261", 
                SearchFields = "10261,טלקאר חברה בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10262", 
                SearchFields = "10262,טלקאר חברה בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10263", 
                SearchFields = "10263,טלקאר חברה בע''מ- ב''ש", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10264", 
                SearchFields = "10264,טלקאר חברה בע''מ- באר שבע", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10265", 
                SearchFields = "10265,טלקאר חברה בע''מ-אשדוד", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ-אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10266", 
                SearchFields = "10266,טלקאר חברה בע''מ- ת''א אולם 1", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- ת''א אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10267", 
                SearchFields = "10267,טלקאר חברה בע''מ- חולון  סוקולוב", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- חולון  סוקולוב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10268", 
                SearchFields = "10268,טלקאר חברה בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10269", 
                SearchFields = "10269,טלקאר חברה בע''מ-רחובות הרצל", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ-רחובות הרצל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10270", 
                SearchFields = "10270,טלקאר חברה בע''מ-רחובות ז'בוטינסקי", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ-רחובות ז'בוטינסקי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10271", 
                SearchFields = "10271,טלקאר חברה בע''מ- רמת גן", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- רמת גן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10272", 
                SearchFields = "10272,טלקאר חברה בע''מ- ראשון לציון", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- ראשון לציון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10273", 
                SearchFields = "10273,טלקאר חברה בע''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10274", 
                SearchFields = "10274,טלקאר חברה בע''מ- ת''א אולם 2", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- ת''א אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10275", 
                SearchFields = "10275,טלקאר חברה בע''מ- פ''ת", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10276", 
                SearchFields = "10276,טלקאר חברה בע''מ- חולון  הפלד", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- חולון  הפלד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10277", 
                SearchFields = "10277,טלקאר חברה בע''מ- רחובות גבריאלוב", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- רחובות גבריאלוב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10278", 
                SearchFields = "10278,טלקאר חברה בע''מ- רעננה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- רעננה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10279", 
                SearchFields = "10279,טלקאר חברה בע''מ- נתניה", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10280", 
                SearchFields = "10280,טלקאר חברה בע''מ- אילת", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10281", 
                SearchFields = "10281,יוניון מוטורס בע''מ- תל חנן", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- תל חנן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10282", 
                SearchFields = "10282,יוניון מוטורס בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10283", 
                SearchFields = "10283,יוניון מוטורס בע''מ- נצרת", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10284", 
                SearchFields = "10284,יוניון מוטורס בע''מ- טבריה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10285", 
                SearchFields = "10285,יוניון מוטורס בע''מ- חדרה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10286", 
                SearchFields = "10286,יוניון מוטורס בע''מ- חיפה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10287", 
                SearchFields = "10287,יוניון מוטורס בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10288", 
                SearchFields = "10288,יוניון מוטורס בע''מ- מפרץ חיפה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- מפרץ חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10289", 
                SearchFields = "10289,יוניון מוטורס בע''מ- ב''ש רח' חברון", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- ב''ש רח' חברון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10290", 
                SearchFields = "10290,יוניון מוטורס בע''מ- ב''ש  רח' הנפח", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- ב''ש  רח' הנפח", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10291", 
                SearchFields = "10291,יוניון מוטורס בע''מ- ת''א יגאל אלון", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- ת''א יגאל אלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10292", 
                SearchFields = "10292,יוניון מוטורס בע''מ- רחובות", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10293", 
                SearchFields = "10293,יוניון מוטורס בע''מ בני ברק", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ בני ברק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10294", 
                SearchFields = "10294,יוניון מוטורס בע''מ- פ''ת", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10295", 
                SearchFields = "10295,יוניון מוטורס בע''מ- ת''א בן צבי", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- ת''א בן צבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10296", 
                SearchFields = "10296,יוניון מוטורס בע''מ- נס ציונה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- נס ציונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10297", 
                SearchFields = "10297,יוניון מוטורס בע''מ- הרצליה פיתוח", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- הרצליה פיתוח", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10298", 
                SearchFields = "10298,יוניון מוטורס בע''מ- הרצליה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10299", 
                SearchFields = "10299,יוניון מוטורס בע''מ- רעננה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- רעננה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10300", 
                SearchFields = "10300,יוניון מוטורס בע''מ- רמלה", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- רמלה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10301", 
                SearchFields = "10301,יוניון מוטורס בע''מ- כפר סבא", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- כפר סבא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10302", 
                SearchFields = "10302,כלמוביל בע''מ- רעננה אולם 2", 
                Inactive = false, 
                LocalName = "כלמוביל בע''מ- רעננה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10303", 
                SearchFields = "10303,צ'מפיון מוטורס בעמ-חיפה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ-חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10304", 
                SearchFields = "10304,צ'מפיון מוטורס בעמ-חיפה אולם 1", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ-חיפה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10305", 
                SearchFields = "10305,צ'מפיון מוטורס בעמ-חדרה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ-חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10306", 
                SearchFields = "10306,צ'מפיון מוטורס בעמ- נצרת אולם 1", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- נצרת אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10307", 
                SearchFields = "10307,צ'מפיון מוטורס בעמ- אל גבייה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- אל גבייה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10308", 
                SearchFields = "10308,צ'מפיון מוטורס בעמ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10309", 
                SearchFields = "10309,צ'מפיון מוטורס בעמ- קרית שמונה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- קרית שמונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10310", 
                SearchFields = "10310,צ'מפיון מוטורס בעמ- חיפה חוף שמן", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- חיפה חוף שמן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10311", 
                SearchFields = "10311,צ'מפיון מוטורס בעמ- חיפה אולם 2", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- חיפה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10312", 
                SearchFields = "10312,צ'מפיון מוטורס בעמ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10313", 
                SearchFields = "10313,צ'מפיון מוטורס בעמ- חיפה אולם 3", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- חיפה אולם 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10314", 
                SearchFields = "10314,צ'מפיון מוטורס בעמ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10315", 
                SearchFields = "10315,צ'מפיון מוטורס בעמ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10316", 
                SearchFields = "10316,צ'מפיון מוטורס בעמ-נצרת אולם 2", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ-נצרת אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10317", 
                SearchFields = "10317,צ'מפיון מוטורס בעמ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10318", 
                SearchFields = "10318,צ'מפיון מוטורס בעמ- ב''ש", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10319", 
                SearchFields = "10319,צ'מפיון מוטורס בעמ- אשדוד", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10320", 
                SearchFields = "10320,צ'מפיון מוטורס בעמ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10321", 
                SearchFields = "10321,צ'מפיון מוטורס בעמ- אשקלון", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10322", 
                SearchFields = "10322,צ'מפיון מוטורס בעמ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10323", 
                SearchFields = "10323,צ'מפיון מוטורס בעמ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10324", 
                SearchFields = "10324,צ'מפיון מוטורס בעמ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10325", 
                SearchFields = "10325,צ'מפיון מוטורס בעמ- חולון", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- חולון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10326", 
                SearchFields = "10326,צ'מפיון מוטורס בעמ ת''א יצחק שדה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ ת''א יצחק שדה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10327", 
                SearchFields = "10327,צ'מפיון מוטורס בעמ- בני ברק ברוך הירש", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- בני ברק ברוך הירש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10328", 
                SearchFields = "10328,צ'מפיון מוטורס בעמ- ת''א דרך השלום", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- ת''א דרך השלום", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10329", 
                SearchFields = "10329,צ'מפיון מוטורס בעמ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10330", 
                SearchFields = "10330,צ'מפיון מוטורס בעמ- לא פעיל 11", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10331", 
                SearchFields = "10331,צ'מפיון מוטורס בעמ- פ''ת", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10332", 
                SearchFields = "10332,צ'מפיון מוטורס בעמ- הוד השרון", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- הוד השרון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10333", 
                SearchFields = "10333,צ'מפיון מוטורס בעמ- רעננה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- רעננה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10334", 
                SearchFields = "10334,צ'מפיון מוטורס בעמ- ראשל''צ", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- ראשל''צ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10335", 
                SearchFields = "10335,צ'מפיון מוטורס בעמ- בני ברק הלח''י", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- בני ברק הלח''י", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10336", 
                SearchFields = "10336,צ'מפיון מוטורס בעמ- לא פעיל 12", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10337", 
                SearchFields = "10337,צ'מפיון מוטורס בעמ- נתניה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10338", 
                SearchFields = "10338,צ'מפיון מוטורס בעמ- רחובות", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10339", 
                SearchFields = "10339,צ'מפיון מוטורס בעמ- לא פעיל 13", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10340", 
                SearchFields = "10340,צ'מפיון מוטורס בעמ- הרצליה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10341", 
                SearchFields = "10341,צ'מפיון מוטורס בעמ- ירושלים רח' התעופה", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- ירושלים רח' התעופה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10342", 
                SearchFields = "10342,צ'מפיון מוטורס בעמ- ירושלים רח' האומן", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- ירושלים רח' האומן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10343", 
                SearchFields = "10343,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10344", 
                SearchFields = "10344,יו אם אי יוניברסל מוטורס ישראל בע''מ- חיפה ההסתדרות", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- חיפה ההסתדרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10345", 
                SearchFields = "10345,יו אם אי יוניברסל מוטורס ישראל בע''מ- נצרת עלית", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- נצרת עלית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10346", 
                SearchFields = "10346,יו אם אי יוניברסל מוטורס ישראל בע''מ- עכו", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10347", 
                SearchFields = "10347,יו אם אי יוניברסל מוטורס ישראל בע''מ- חיפה  יפו", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- חיפה  יפו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10348", 
                SearchFields = "10348,יו אם אי יוניברסל מוטורס ישראל בע''מ- קרית שמונה", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- קרית שמונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10349", 
                SearchFields = "10349,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10350", 
                SearchFields = "10350,יו אם אי יוניברסל מוטורס ישראל בע''מ-חדרה", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10351", 
                SearchFields = "10351,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10352", 
                SearchFields = "10352,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10353", 
                SearchFields = "10353,יו אם אי יוניברסל מוטורס ישראל בע''מ-אשקלון", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10354", 
                SearchFields = "10354,יו אם אי יוניברסל מוטורס ישראל בע''מ-אשדוד", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10355", 
                SearchFields = "10355,יו אם אי יוניברסל מוטורס ישראל בע''מ-ב''ש", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-ב''ש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10356", 
                SearchFields = "10356,יו אם אי יוניברסל מוטורס ישראל בע''מ-ת''א דרך פ''ת", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-ת''א דרך פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10357", 
                SearchFields = "10357,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10358", 
                SearchFields = "10358,יו אם אי יוניברסל מוטורס ישראל בע''מ-ראשון לציון", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-ראשון לציון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10359", 
                SearchFields = "10359,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10360", 
                SearchFields = "10360,יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10361", 
                SearchFields = "10361,יו אם אי יוניברסל מוטורס ישראל בע''מ-נתניה", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10362", 
                SearchFields = "10362,יו אם אי יוניברסל מוטורס ישראל בע''מ-רשל''צ אולם 1", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-רשל''צ אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10363", 
                SearchFields = "10363,יו אם אי יוניברסל מוטורס ישראל בע''מ-רחובות", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10364", 
                SearchFields = "10364,יו אם אי יוניברסל מוטורס ישראל בע''מ-פתח תקווה", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-פתח תקווה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10365", 
                SearchFields = "10365,יו אם אי יוניברסל מוטורס ישראל בע''מ-רשל''צ אולם 2", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ-רשל''צ אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10366", 
                SearchFields = "10366,יו אם אי יוניברסל מוטורס ישראל בע''מ- ת''א המלאכה", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- ת''א המלאכה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10367", 
                SearchFields = "10367,יו אם אי יוניברסל מוטורס ישראל בע''מ- כפר סבא", 
                Inactive = false, 
                LocalName = "יו אם אי יוניברסל מוטורס ישראל בע''מ- כפר סבא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10368", 
                SearchFields = "10368,מכשירי תנועה בע''''מ- חיפה אולם 1", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- חיפה אולם 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10369", 
                SearchFields = "10369,מכשירי תנועה בע''''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10370", 
                SearchFields = "10370,מכשירי תנועה בע''''מ- חיפה ההסתדרות", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- חיפה ההסתדרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10371", 
                SearchFields = "10371,מכשירי תנועה בע''''מ- נצרת עלית", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- נצרת עלית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10372", 
                SearchFields = "10372,מכשירי תנועה בע''''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10373", 
                SearchFields = "10373,מכשירי תנועה בע''''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10374", 
                SearchFields = "10374,מכשירי תנועה בע''''מ- לא פעיל 4", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10375", 
                SearchFields = "10375,מכשירי תנועה בע''''מ- רמת ישי", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- רמת ישי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10376", 
                SearchFields = "10376,מכשירי תנועה בע''''מ- ק.שמונה", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- ק.שמונה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10377", 
                SearchFields = "10377,מכשירי תנועה בע''''מ- חיפה אולם 2", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- חיפה אולם 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10378", 
                SearchFields = "10378,מכשירי תנועה בע''''מ- חדרה", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10379", 
                SearchFields = "10379,מכשירי תנועה בע''''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10380", 
                SearchFields = "10380,מכשירי תנועה בע''''מ- לא פעיל 6", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10381", 
                SearchFields = "10381,מכשירי תנועה בע''''מ- כרמיאל", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- כרמיאל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10382", 
                SearchFields = "10382,מכשירי תנועה בע''''מ- עפולה", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- עפולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10383", 
                SearchFields = "10383,מכשירי תנועה בע''''מ- אשקלון", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10384", 
                SearchFields = "10384,מכשירי תנועה בע''''מ- אשדוד", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10385", 
                SearchFields = "10385,מכשירי תנועה בע''''מ- באר שבע", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10386", 
                SearchFields = "10386,מכשירי תנועה בע''''מ- ת''א דרך פ''ת", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- ת''א דרך פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10387", 
                SearchFields = "10387,מכשירי תנועה בע''''מ- לא פעיל 7", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10388", 
                SearchFields = "10388,מכשירי תנועה בע''''מ- לא פעיל 8", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10389", 
                SearchFields = "10389,מכשירי תנועה בע''''מ- נתניה", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10390", 
                SearchFields = "10390,מכשירי תנועה בע''''מ- ת''א המסגר", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- ת''א המסגר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10391", 
                SearchFields = "10391,מכשירי תנועה בע''''מ-פ''ת", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ-פ''ת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10392", 
                SearchFields = "10392,מכשירי תנועה בע''''מ- יפו", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- יפו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10393", 
                SearchFields = "10393,מכשירי תנועה בע''''מ- הרצליה", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- הרצליה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10394", 
                SearchFields = "10394,מכשירי תנועה בע''''מ-ראשל''צ", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ-ראשל''צ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10395", 
                SearchFields = "10395,מכשירי תנועה בע''''מ-כפר סבא", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ-כפר סבא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10396", 
                SearchFields = "10396,מכשירי תנועה בע''''מ-פ''ת ז'בוטינסקי", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ-פ''ת ז'בוטינסקי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10397", 
                SearchFields = "10397,מכשירי תנועה בע''''מ-פ''ת  אינשטיין", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ-פ''ת  אינשטיין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10398", 
                SearchFields = "10398,מכשירי תנועה בע''''מ- רחובות", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10399", 
                SearchFields = "10399,מכשירי תנועה בע''''מ- חולון", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- חולון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10400", 
                SearchFields = "10400,מכשירי תנועה בע''''מ- מודיעין", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- מודיעין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10401", 
                SearchFields = "10401,בני משה קרסו בע''''מ  - לא פעיל 11", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10402", 
                SearchFields = "10402,בני משה קרסו בע''''מ  - לא פעיל 12", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10403", 
                SearchFields = "10403,בני משה קרסו בע''''מ  - לא פעיל 13", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10404", 
                SearchFields = "10404,בני משה קרסו בע''''מ  - לא פעיל 14", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10405", 
                SearchFields = "10405,בני משה קרסו בע''''מ  - לא פעיל 15", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10406", 
                SearchFields = "10406,בני משה קרסו בע''''מ  - לא פעיל 16", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 16", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10407", 
                SearchFields = "10407,בני משה קרסו בע''''מ  - לא פעיל 17", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 17", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10408", 
                SearchFields = "10408,בני משה קרסו בע''''מ  - לא פעיל 18", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 18", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10409", 
                SearchFields = "10409,בני משה קרסו בע''''מ  - לא פעיל 19", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 19", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10410", 
                SearchFields = "10410,בני משה קרסו בע''''מ  - לא פעיל 20", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - לא פעיל 20", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10411", 
                SearchFields = "10411,בני משה קרסו בע''''מ - אשקלון", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ - אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10412", 
                SearchFields = "10412,בני משה קרסו בע''''מ - באר שבע", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ - באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10413", 
                SearchFields = "10413,בני משה קרסו בע''''מ  - אשדוד", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10414", 
                SearchFields = "10414,בני משה קרסו בע''''מ  - אילת", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''''מ  - אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10415", 
                SearchFields = "10415,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 1", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10416", 
                SearchFields = "10416,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 2", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10417", 
                SearchFields = "10417,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 3", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10418", 
                SearchFields = "10418,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 4", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10419", 
                SearchFields = "10419,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 5", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10420", 
                SearchFields = "10420,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 6", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10421", 
                SearchFields = "10421,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 7", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 7", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10422", 
                SearchFields = "10422,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 8", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 8", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10423", 
                SearchFields = "10423,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 9", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10424", 
                SearchFields = "10424,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 10", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10425", 
                SearchFields = "10425,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 11", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 11", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10426", 
                SearchFields = "10426,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 12", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 12", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10427", 
                SearchFields = "10427,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 13", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 13", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10428", 
                SearchFields = "10428,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 14", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10429", 
                SearchFields = "10429,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 15", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10430", 
                SearchFields = "10430,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 16", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 16", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10431", 
                SearchFields = "10431,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 17", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 17", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10432", 
                SearchFields = "10432,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 18", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 18", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10433", 
                SearchFields = "10433,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 19", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 19", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10434", 
                SearchFields = "10434,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 20", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 20", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10435", 
                SearchFields = "10435,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 21", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 21", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10436", 
                SearchFields = "10436,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 22", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 22", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10437", 
                SearchFields = "10437,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 23", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 23", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10438", 
                SearchFields = "10438,פסיפיק יבואני מכוניות בע''''מ", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10439", 
                SearchFields = "10439,צ'מפיון מוטורס בעמ- לא פעיל 14", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בעמ- לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10440", 
                SearchFields = "10440,צ'מפיון סכו קאר בע''''מ- עפולה", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- עפולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10441", 
                SearchFields = "10441,צ'מפיון סכו קאר בע''''מ- חיפה", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10442", 
                SearchFields = "10442,צ'מפיון סכו קאר בע''''מ- אשדוד", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10443", 
                SearchFields = "10443,צ'מפיון סכו קאר בע''''מ- ת''א", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10444", 
                SearchFields = "10444,צ'מפיון סכו קאר בע''''מ- בני ברק", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- בני ברק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10445", 
                SearchFields = "10445,צ'מפיון סכו קאר בע''''מ- רחובות", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- רחובות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10446", 
                SearchFields = "10446,צ'מפיון סכו קאר בע''''מ-לא פעיל 1", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ-לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10447", 
                SearchFields = "10447,צ'מפיון סכו קאר בע''''מ-לא פעיל 2", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ-לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10448", 
                SearchFields = "10448,צ'מפיון סכו קאר בע''''מ- ירושלים", 
                Inactive = false, 
                LocalName = "צ'מפיון סכו קאר בע''''מ- ירושלים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10449", 
                SearchFields = "10449,ש.י.ר שלמה יבוא רכב בע''מ- צריפין", 
                Inactive = false, 
                LocalName = "ש.י.ר שלמה יבוא רכב בע''מ- צריפין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10450", 
                SearchFields = "10450,נמל קישון - חיפה", 
                Inactive = false, 
                LocalName = "נמל קישון - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10451", 
                SearchFields = "10451,מחסן 3 - חיפה", 
                Inactive = false, 
                LocalName = "מחסן 3 - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10452", 
                SearchFields = "10452,מחסן 15 חיפה", 
                Inactive = false, 
                LocalName = "מחסן 15 חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10453", 
                SearchFields = "10453,שער הנמל - חיפה", 
                Inactive = false, 
                LocalName = "שער הנמל - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10454", 
                SearchFields = "10454,נמל - אשדוד", 
                Inactive = false, 
                LocalName = "נמל - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10455", 
                SearchFields = "10455,נמל - אילת", 
                Inactive = false, 
                LocalName = "נמל - אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10456", 
                SearchFields = "10456,משקף רמפה - חיפה", 
                Inactive = false, 
                LocalName = "משקף רמפה - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10457", 
                SearchFields = "10457,משקף רדיוגרף - חיפה", 
                Inactive = false, 
                LocalName = "משקף רדיוגרף - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10458", 
                SearchFields = "10458,משקף רמפה - אשדוד", 
                Inactive = false, 
                LocalName = "משקף רמפה - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10459", 
                SearchFields = "10459,משקף רדיוגרף - אשדוד", 
                Inactive = false, 
                LocalName = "משקף רדיוגרף - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10460", 
                SearchFields = "10460,גשר אדם", 
                Inactive = false, 
                LocalName = "גשר אדם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10461", 
                SearchFields = "10461,גשר אלנבי", 
                Inactive = false, 
                LocalName = "גשר אלנבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10462", 
                SearchFields = "10462,מסוף ניצנה", 
                Inactive = false, 
                LocalName = "מסוף ניצנה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10463", 
                SearchFields = "10463,מסוף רפיח", 
                Inactive = false, 
                LocalName = "מסוף רפיח", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10464", 
                SearchFields = "10464,מסוף טבה", 
                Inactive = false, 
                LocalName = "מסוף טבה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10465", 
                SearchFields = "10465,גשר נהר הירדן", 
                Inactive = false, 
                LocalName = "גשר נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10466", 
                SearchFields = "10466,מסוף ערבה", 
                Inactive = false, 
                LocalName = "מסוף ערבה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10467", 
                SearchFields = "10467,עובדה", 
                Inactive = false, 
                LocalName = "עובדה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10468", 
                SearchFields = "10468,שדה תעופה אילת", 
                Inactive = false, 
                LocalName = "שדה תעופה אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10469", 
                SearchFields = "10469,סוויספורט מסחרי + יבוא אישי", 
                Inactive = false, 
                LocalName = "סוויספורט מסחרי + יבוא אישי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10470", 
                SearchFields = "10470,ממ''ן מסחרי + יבוא אישי", 
                Inactive = false, 
                LocalName = "ממ''ן מסחרי + יבוא אישי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10471", 
                SearchFields = "10471,בלדרים", 
                Inactive = false, 
                LocalName = "בלדרים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10472", 
                SearchFields = "10472,מחסן מזרחי", 
                Inactive = false, 
                LocalName = "מחסן מזרחי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10473", 
                SearchFields = "10473,בלדרים - יצוא", 
                Inactive = false, 
                LocalName = "בלדרים - יצוא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10474", 
                SearchFields = "10474,דואר חבילות - ירושלים", 
                Inactive = false, 
                LocalName = "דואר חבילות - ירושלים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10475", 
                SearchFields = "10475,דואר חבילות אילת", 
                Inactive = false, 
                LocalName = "דואר חבילות אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10476", 
                SearchFields = "10476,דואר חבילות - חיפה", 
                Inactive = false, 
                LocalName = "דואר חבילות - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10477", 
                SearchFields = "10477,דואר חבילות - ת''א", 
                Inactive = false, 
                LocalName = "דואר חבילות - ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10479", 
                SearchFields = "10479,מכולות - אוברסיז חיפה", 
                Inactive = false, 
                LocalName = "מכולות - אוברסיז חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10480", 
                SearchFields = "10480,חלקי - אוברסיז חיפה", 
                Inactive = false, 
                LocalName = "חלקי - אוברסיז חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10481", 
                SearchFields = "10481,שער אוברסיז - חיפה", 
                Inactive = false, 
                LocalName = "שער אוברסיז - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10482", 
                SearchFields = "10482,מסוף מילניום - חיפה", 
                Inactive = false, 
                LocalName = "מסוף מילניום - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10483", 
                SearchFields = "10483,מ. מדידה וסריקה בי''ל-נהר הירדן", 
                Inactive = false, 
                LocalName = "מ. מדידה וסריקה בי''ל-נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10484", 
                SearchFields = "10484,קונטרם - אשדוד", 
                Inactive = false, 
                LocalName = "קונטרם - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10485", 
                SearchFields = "10485,אוברסיז - אשדוד", 
                Inactive = false, 
                LocalName = "אוברסיז - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10486", 
                SearchFields = "10486,בונדד - אשדוד", 
                Inactive = false, 
                LocalName = "בונדד - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10487", 
                SearchFields = "10487,מסוף 207 בעמ - 8268", 
                Inactive = false, 
                LocalName = "מסוף 207 בעמ - 8268", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10488", 
                SearchFields = "10488,אוברסיז-2 - אשדוד", 
                Inactive = false, 
                LocalName = "אוברסיז-2 - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "10489", 
                SearchFields = "10489,מחסן מכירות נעמן - אשדוד", 
                Inactive = false, 
                LocalName = "מחסן מכירות נעמן - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "109", 
                SearchFields = "109,אשדוד ים", 
                Inactive = false, 
                LocalName = "אשדוד ים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1321", 
                SearchFields = "1321,לים בע''מ לא פעיל 1", 
                Inactive = false, 
                LocalName = "לים בע''מ לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1347", 
                SearchFields = "1347,שרותי תעופה בע''מ שדה עטרות ים", 
                Inactive = false, 
                LocalName = "שרותי תעופה בע''מ שדה עטרות ים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1354", 
                SearchFields = "1354,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 15", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 15", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1362", 
                SearchFields = "1362,ד. לובינסקי", 
                Inactive = false, 
                LocalName = "ד. לובינסקי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "14", 
                SearchFields = "14,תחנת מכב טבריה", 
                Inactive = false, 
                LocalName = "תחנת מכב טבריה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "15", 
                SearchFields = "15,תחנת מכס נצרת", 
                Inactive = false, 
                LocalName = "תחנת מכס נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1529", 
                SearchFields = "1529,מחסן לים בע''מ אילת", 
                Inactive = false, 
                LocalName = "מחסן לים בע''מ אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1537", 
                SearchFields = "1537,ס.ב. חברת אילת -דיוטיפרי בע''מ", 
                Inactive = false, 
                LocalName = "ס.ב. חברת אילת -דיוטיפרי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1545", 
                SearchFields = "1545,שרותי תעופה בע''מ נ תעופה אילת", 
                Inactive = false, 
                LocalName = "שרותי תעופה בע''מ נ תעופה אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1552", 
                SearchFields = "1552,רמתם בע''מ, אילת", 
                Inactive = false, 
                LocalName = "רמתם בע''מ, אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1560", 
                SearchFields = "1560,לים בע''מ-ת''א", 
                Inactive = false, 
                LocalName = "לים בע''מ-ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1578", 
                SearchFields = "1578,טאבה", 
                Inactive = false, 
                LocalName = "טאבה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "158", 
                SearchFields = "158,מפרץ חיפה - חברה להשקעות בע''מ", 
                Inactive = false, 
                LocalName = "מפרץ חיפה - חברה להשקעות בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1586", 
                SearchFields = "1586,ש.שטרן(ישראל) תכשיטים ואבני חן", 
                Inactive = false, 
                LocalName = "ש.שטרן(ישראל) תכשיטים ואבני חן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1594", 
                SearchFields = "1594,נעמן עתידי לא פעיל", 
                Inactive = false, 
                LocalName = "נעמן עתידי לא פעיל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "16", 
                SearchFields = "16,תחנת מכס עכו", 
                Inactive = false, 
                LocalName = "תחנת מכס עכו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1602", 
                SearchFields = "1602,טק מרין שירותים בע''מ", 
                Inactive = false, 
                LocalName = "טק מרין שירותים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "174", 
                SearchFields = "174,מרינה יפו", 
                Inactive = false, 
                LocalName = "מרינה יפו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "18", 
                SearchFields = "none,18", 
                Inactive = false, 
                LocalName = "none", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "1811", 
                SearchFields = "1811,נמל חיפה-אחסנת כלי רכב", 
                Inactive = false, 
                LocalName = "נמל חיפה-אחסנת כלי רכב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "182", 
                SearchFields = "182,מרינה ת''א", 
                Inactive = false, 
                LocalName = "מרינה ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "19", 
                SearchFields = "19,נמל חיפה - עסקה", 
                Inactive = false, 
                LocalName = "נמל חיפה - עסקה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "190", 
                SearchFields = "190,נמל עזה", 
                Inactive = false, 
                LocalName = "נמל עזה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2", 
                SearchFields = "2,בית מכס אשדוד", 
                Inactive = false, 
                LocalName = "בית מכס אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "20", 
                SearchFields = "20,קרני", 
                Inactive = false, 
                LocalName = "קרני", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2006", 
                SearchFields = "2006,אורדילן מחסני ערובה(1971)", 
                Inactive = false, 
                LocalName = "אורדילן מחסני ערובה(1971)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2014", 
                SearchFields = "2014,אמנה חברה לאחסנה בערובה בע''מ", 
                Inactive = false, 
                LocalName = "אמנה חברה לאחסנה בערובה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2022", 
                SearchFields = "2022,אורינטקור בע''מ", 
                Inactive = false, 
                LocalName = "אורינטקור בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2030", 
                SearchFields = "2030,מתכות יקרות בנק איגוד לישראל", 
                Inactive = false, 
                LocalName = "מתכות יקרות בנק איגוד לישראל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2048", 
                SearchFields = "2048,קרור אחזקות בע''מ", 
                Inactive = false, 
                LocalName = "קרור אחזקות בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2055", 
                SearchFields = "2055,בנק מרכנתיל דיסקונט בע''מ מתכות", 
                Inactive = false, 
                LocalName = "בנק מרכנתיל דיסקונט בע''מ מתכות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2063", 
                SearchFields = "2063,הסוכנות היהודית", 
                Inactive = false, 
                LocalName = "הסוכנות היהודית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2071", 
                SearchFields = "2071,המחסין בע''מ", 
                Inactive = false, 
                LocalName = "המחסין בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2089", 
                SearchFields = "2089,המחסין אזור התעשיה הישן ראשון", 
                Inactive = false, 
                LocalName = "המחסין אזור התעשיה הישן ראשון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2097", 
                SearchFields = "2097,חב ישראלית מרכזית למימון ואחס", 
                Inactive = false, 
                LocalName = "חב ישראלית מרכזית למימון ואחס", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "21", 
                SearchFields = "21,ארז", 
                Inactive = false, 
                LocalName = "ארז", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2105", 
                SearchFields = "2105,ג'יימס ריצרדסון בע''מ - נמל אשדוד", 
                Inactive = false, 
                LocalName = "ג'יימס ריצרדסון בע''מ - נמל אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2113", 
                SearchFields = "2113,מגלד מחסני קרור בע''מ", 
                Inactive = false, 
                LocalName = "מגלד מחסני קרור בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2121", 
                SearchFields = "2121,קר פרי חולון בע''מ", 
                Inactive = false, 
                LocalName = "קר פרי חולון בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2139", 
                SearchFields = "2139,מחסני ערובה א''י בע''מ", 
                Inactive = false, 
                LocalName = "מחסני ערובה א''י בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2147", 
                SearchFields = "2147,מחסני ערובה אלבני בע''מ לא פעיל 1", 
                Inactive = false, 
                LocalName = "מחסני ערובה אלבני בע''מ לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2154", 
                SearchFields = "2154,מחסני ערובה א''י בע''מ - זמני", 
                Inactive = false, 
                LocalName = "מחסני ערובה א''י בע''מ - זמני", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2162", 
                SearchFields = "2162,בנק כללי לישראל", 
                Inactive = false, 
                LocalName = "בנק כללי לישראל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2170", 
                SearchFields = "2170,למלא", 
                Inactive = false, 
                LocalName = "למלא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2188", 
                SearchFields = "2188,סבירסקי, מחסני ערובה בע''מ לא פעיל 1", 
                Inactive = false, 
                LocalName = "סבירסקי, מחסני ערובה בע''מ לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2196", 
                SearchFields = "2196,סבירסקי, מחסני ערובה בע''מ לא פעיל 2", 
                Inactive = false, 
                LocalName = "סבירסקי, מחסני ערובה בע''מ לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "22", 
                SearchFields = "22,סופה", 
                Inactive = false, 
                LocalName = "סופה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2204", 
                SearchFields = "2204,מ.פ.ל. ירקון בע''מ (זמני)", 
                Inactive = false, 
                LocalName = "מ.פ.ל. ירקון בע''מ (זמני)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2212", 
                SearchFields = "2212,מ.פ.ל.אילון בע''מ בני ברק", 
                Inactive = false, 
                LocalName = "מ.פ.ל.אילון בע''מ בני ברק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2220", 
                SearchFields = "2220,אוברסיז שירותי אחסנה בע''מ", 
                Inactive = false, 
                LocalName = "אוברסיז שירותי אחסנה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2222", 
                SearchFields = "2222,מסוף אשקלון", 
                Inactive = false, 
                LocalName = "מסוף אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "224", 
                SearchFields = "224,עטרות", 
                Inactive = false, 
                LocalName = "עטרות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2253", 
                SearchFields = "2253,פלטרנספורט בע''מ", 
                Inactive = false, 
                LocalName = "פלטרנספורט בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2261", 
                SearchFields = "2261,לבנט בונדד ת''א (2791) בע''מ", 
                Inactive = false, 
                LocalName = "לבנט בונדד ת''א (2791) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2279", 
                SearchFields = "2279,ריב''ל מחסני ערובה כלליים בע''מ", 
                Inactive = false, 
                LocalName = "ריב''ל מחסני ערובה כלליים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2287", 
                SearchFields = "2287,סקאל בע''מ", 
                Inactive = false, 
                LocalName = "סקאל בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2295", 
                SearchFields = "2295,צ'מפיון מחסני ערובה (5891)בע''מ", 
                Inactive = false, 
                LocalName = "צ'מפיון מחסני ערובה (5891)בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "23", 
                SearchFields = "23,ג'אלמה-גלבוע", 
                Inactive = false, 
                LocalName = "ג'אלמה-גלבוע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2311", 
                SearchFields = "2311,אלבני בונדד סחר בינלאומי בע''מ", 
                Inactive = false, 
                LocalName = "אלבני בונדד סחר בינלאומי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2329", 
                SearchFields = "2329,בתי קרור ואחסנה חולון בע''מ", 
                Inactive = false, 
                LocalName = "בתי קרור ואחסנה חולון בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2337", 
                SearchFields = "2337,קרסו, שרותי אחסון וערובה בע''מ", 
                Inactive = false, 
                LocalName = "קרסו, שרותי אחסון וערובה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2345", 
                SearchFields = "2345,דניאל יצחקי תעשיות עץ בע''מ", 
                Inactive = false, 
                LocalName = "דניאל יצחקי תעשיות עץ בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2352", 
                SearchFields = "2352,מ.פ.ל. ראשל''צ  בע''מ", 
                Inactive = false, 
                LocalName = "מ.פ.ל. ראשל''צ  בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2360", 
                SearchFields = "2360,נעמן עתידי", 
                Inactive = false, 
                LocalName = "נעמן עתידי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "2378", 
                SearchFields = "2378,אוברסיז קומרס בע''מ (חצרים)", 
                Inactive = false, 
                LocalName = "אוברסיז קומרס בע''מ (חצרים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "24", 
                SearchFields = "24,שער אפרים", 
                Inactive = false, 
                LocalName = "שער אפרים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "240", 
                SearchFields = "240,ראש הנקרה", 
                Inactive = false, 
                LocalName = "ראש הנקרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "25", 
                SearchFields = "25,ביטוניה-עופר", 
                Inactive = false, 
                LocalName = "ביטוניה-עופר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "257", 
                SearchFields = "257,שדה דב - ת''א", 
                Inactive = false, 
                LocalName = "שדה דב - ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "265", 
                SearchFields = "265,שדה תעופה אילת", 
                Inactive = false, 
                LocalName = "שדה תעופה אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "27", 
                SearchFields = "27,תרקומיה", 
                Inactive = false, 
                LocalName = "תרקומיה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "273", 
                SearchFields = "273,שדה תעופה חיפה", 
                Inactive = false, 
                LocalName = "שדה תעופה חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "28", 
                SearchFields = "28,תחנת מכס נתניה", 
                Inactive = false, 
                LocalName = "תחנת מכס נתניה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "281", 
                SearchFields = "281,שומרה", 
                Inactive = false, 
                LocalName = "שומרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "29", 
                SearchFields = "29,ג'אלמה-גלבוע", 
                Inactive = false, 
                LocalName = "ג'אלמה-גלבוע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "299", 
                SearchFields = "299,רידינג יפו- אוצר מפעלי ים בע''מ", 
                Inactive = false, 
                LocalName = "רידינג יפו- אוצר מפעלי ים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3", 
                SearchFields = "3,בית מכס מרכז", 
                Inactive = false, 
                LocalName = "בית מכס מרכז", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "30", 
                SearchFields = "30,שמעה", 
                Inactive = false, 
                LocalName = "שמעה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3012", 
                SearchFields = "3012,ראסקאר רכב בע''מ", 
                Inactive = false, 
                LocalName = "ראסקאר רכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3038", 
                SearchFields = "3038,בני משה קרסו", 
                Inactive = false, 
                LocalName = "בני משה קרסו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3046", 
                SearchFields = "3046,ליאו גולדברג בע''מ", 
                Inactive = false, 
                LocalName = "ליאו גולדברג בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3053", 
                SearchFields = "3053,מבט קדימה בע''מ", 
                Inactive = false, 
                LocalName = "מבט קדימה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3061", 
                SearchFields = "3061,יפנאוטו חברה ישראלית לרכב בע''מ- רמת השרון", 
                Inactive = false, 
                LocalName = "יפנאוטו חברה ישראלית לרכב בע''מ- רמת השרון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "307", 
                SearchFields = "307,עובדה - יצוא", 
                Inactive = false, 
                LocalName = "עובדה - יצוא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3095", 
                SearchFields = "3095,יפאנאוטו בע''מ", 
                Inactive = false, 
                LocalName = "יפאנאוטו בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3103", 
                SearchFields = "3103,סמל''ת בע''מ", 
                Inactive = false, 
                LocalName = "סמל''ת בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3111", 
                SearchFields = "3111,חברת דוד לובינסקי בע''מ", 
                Inactive = false, 
                LocalName = "חברת דוד לובינסקי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3145", 
                SearchFields = "3145,חב ישראלית לאוטומובילים בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "חב ישראלית לאוטומובילים בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3160", 
                SearchFields = "3160,חב המזרח למכוניות בע''מ", 
                Inactive = false, 
                LocalName = "חב המזרח למכוניות בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3175", 
                SearchFields = "3175,מאיר - חברה למכוניות ומשאיות", 
                Inactive = false, 
                LocalName = "מאיר - חברה למכוניות ומשאיות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3186", 
                SearchFields = "3186,א.ב. ליגל סוכנות תחבורה בע''מ", 
                Inactive = false, 
                LocalName = "א.ב. ליגל סוכנות תחבורה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3202", 
                SearchFields = "3202,חב ישראלית לאוטומובילים בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "חב ישראלית לאוטומובילים בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3212693", 
                SearchFields = "3212693,צמפיון מוטורס ירושלים", 
                Inactive = false, 
                LocalName = "צמפיון מוטורס ירושלים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3236", 
                SearchFields = "3236,לובינסקי דוד בע''מ", 
                Inactive = false, 
                LocalName = "לובינסקי דוד בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3251", 
                SearchFields = "3251,מאיר חב למכוניות ומשאיות בע''מ- פתח תקווה", 
                Inactive = false, 
                LocalName = "מאיר חב למכוניות ומשאיות בע''מ- פתח תקווה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3319", 
                SearchFields = "3319,שרותי תעופה בע''מ הרצליה 1", 
                Inactive = false, 
                LocalName = "שרותי תעופה בע''מ הרצליה 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3333", 
                SearchFields = "3333,מסוף חדרה _(רכבת)", 
                Inactive = false, 
                LocalName = "מסוף חדרה _(רכבת)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3350", 
                SearchFields = "3350,צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 1", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3400", 
                SearchFields = "3400,שרותי תעופה בע''מ שדה תעופה דב", 
                Inactive = false, 
                LocalName = "שרותי תעופה בע''מ שדה תעופה דב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3418", 
                SearchFields = "3418,חברת קלסיקה", 
                Inactive = false, 
                LocalName = "חברת קלסיקה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3426", 
                SearchFields = "3426,לים בע''מ- ת''א סעדיה גאון", 
                Inactive = false, 
                LocalName = "לים בע''מ- ת''א סעדיה גאון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3434", 
                SearchFields = "3434,קרסו מוטורס 2007 בע''מ- בני עייש", 
                Inactive = false, 
                LocalName = "קרסו מוטורס 2007 בע''מ- בני עייש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3442", 
                SearchFields = "3442,לים בע''מ- נתב''ג", 
                Inactive = false, 
                LocalName = "לים בע''מ- נתב''ג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3459", 
                SearchFields = "3459,באג מולטי סיסטיים בע''מ", 
                Inactive = false, 
                LocalName = "באג מולטי סיסטיים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3475", 
                SearchFields = "3475,קלסיקה אנטרנשיונל", 
                Inactive = false, 
                LocalName = "קלסיקה אנטרנשיונל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3491", 
                SearchFields = "3491,פלדטראק סוכנויות רכב בע''מ", 
                Inactive = false, 
                LocalName = "פלדטראק סוכנויות רכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3517", 
                SearchFields = "3517,לובינסקי דוד בע''מ לא פעיל", 
                Inactive = false, 
                LocalName = "לובינסקי דוד בע''מ לא פעיל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3525", 
                SearchFields = "3525,סוכנות מכוניות לים התיכון -לא פעיל 1", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון -לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3533", 
                SearchFields = "3533,סוכנות מכוניות לים התיכון -לא פעיל 2", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון -לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3541", 
                SearchFields = "3541,סוכנות מכוניות לים התיכון -לא פעיל 3", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון -לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3558", 
                SearchFields = "3558,מכשירי תנועה לא פעיל 1", 
                Inactive = false, 
                LocalName = "מכשירי תנועה לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3566", 
                SearchFields = "3566,מכשירי תנועה לא פעיל 2", 
                Inactive = false, 
                LocalName = "מכשירי תנועה לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3574", 
                SearchFields = "3574,יפנאוטו בע''מ", 
                Inactive = false, 
                LocalName = "יפנאוטו בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3582", 
                SearchFields = "3582,ליאו גולדברג בע''מ היצירה 8 פת", 
                Inactive = false, 
                LocalName = "ליאו גולדברג בע''מ היצירה 8 פת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3608", 
                SearchFields = "3608,מכשירי תנועה בע''''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3616", 
                SearchFields = "3616,החב' הישראלית לאוטומובילים- לא פעיל 1", 
                Inactive = false, 
                LocalName = "החב' הישראלית לאוטומובילים- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3624", 
                SearchFields = "3624,החב' הישראלית לאוטומובילים- לא פעיל 2", 
                Inactive = false, 
                LocalName = "החב' הישראלית לאוטומובילים- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3632", 
                SearchFields = "3632,יורוגל בונדד ומחסני קרור בע''מ", 
                Inactive = false, 
                LocalName = "יורוגל בונדד ומחסני קרור בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3640", 
                SearchFields = "3640,צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 2", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3657", 
                SearchFields = "3657,דלק מוטורס בע''מ- א.ת ניר צבי", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- א.ת ניר צבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3665", 
                SearchFields = "3665,פסיפיק יבואני מכוניות בע''''מ - לא פעיל 24", 
                Inactive = false, 
                LocalName = "פסיפיק יבואני מכוניות בע''''מ - לא פעיל 24", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3673", 
                SearchFields = "3673,יוניון מוטורס בע''מ- לא פעיל 3", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ- לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3681", 
                SearchFields = "3681,מאיר חב' למכוניות ומשאיות", 
                Inactive = false, 
                LocalName = "מאיר חב' למכוניות ומשאיות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3699", 
                SearchFields = "3699,מחסני קרור בנמל ת''א", 
                Inactive = false, 
                LocalName = "מחסני קרור בנמל ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3707", 
                SearchFields = "3707,טלקאר תל אביב", 
                Inactive = false, 
                LocalName = "טלקאר תל אביב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3715", 
                SearchFields = "3715,מאיר חב למכוניות ומשאיות בע''מ- ראשון לציון", 
                Inactive = false, 
                LocalName = "מאיר חב למכוניות ומשאיות בע''מ- ראשון לציון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3723", 
                SearchFields = "3723,קמור - רכב בע''מ", 
                Inactive = false, 
                LocalName = "קמור - רכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3731", 
                SearchFields = "3731,בני משה קרסו בע''מ - לא פעיל 21", 
                Inactive = false, 
                LocalName = "בני משה קרסו בע''מ - לא פעיל 21", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3749", 
                SearchFields = "3749,סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 14", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון בע''''מ- לא פעיל 14", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3756", 
                SearchFields = "3756,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 6", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 6", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3764", 
                SearchFields = "3764,מכשירי תנועה בע''''מ- לא פעיל 10", 
                Inactive = false, 
                LocalName = "מכשירי תנועה בע''''מ- לא פעיל 10", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3772", 
                SearchFields = "3772,י. מילר ושות' בע''מ", 
                Inactive = false, 
                LocalName = "י. מילר ושות' בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3780", 
                SearchFields = "3780,החב' המאוחדת למזרח הקרוב בישרא", 
                Inactive = false, 
                LocalName = "החב' המאוחדת למזרח הקרוב בישרא", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3798", 
                SearchFields = "3798,עופר מחסני רכב בע''מ", 
                Inactive = false, 
                LocalName = "עופר מחסני רכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "38", 
                SearchFields = "38,דואר חבילות ת''א", 
                Inactive = false, 
                LocalName = "דואר חבילות ת''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3806", 
                SearchFields = "3806,מטרו מוטור שיווק בע''מ", 
                Inactive = false, 
                LocalName = "מטרו מוטור שיווק בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3814", 
                SearchFields = "3814,דלק מוטורס בע''מ- לא פעיל 5", 
                Inactive = false, 
                LocalName = "דלק מוטורס בע''מ- לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3822", 
                SearchFields = "3822,מחלף-מח.ערובה ולוגיס.(1002)בעמ", 
                Inactive = false, 
                LocalName = "מחלף-מח.ערובה ולוגיס.(1002)בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3830", 
                SearchFields = "3830,אבניר חב' לרכב בע''מ", 
                Inactive = false, 
                LocalName = "אבניר חב' לרכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3848", 
                SearchFields = "3848,כלמוביל בע''מ- לא פעיל 18", 
                Inactive = false, 
                LocalName = "כלמוביל בע''מ- לא פעיל 18", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3855", 
                SearchFields = "3855,קמור רכב בע''מ פתח תקווה", 
                Inactive = false, 
                LocalName = "קמור רכב בע''מ פתח תקווה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3863", 
                SearchFields = "3863,פורגל מחסני קרור בע''מ", 
                Inactive = false, 
                LocalName = "פורגל מחסני קרור בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3871", 
                SearchFields = "3871,חברת אס.סי.ג'י לוגיסטיקה", 
                Inactive = false, 
                LocalName = "חברת אס.סי.ג'י לוגיסטיקה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3889", 
                SearchFields = "3889,לינקסים מוטורס בע''מ", 
                Inactive = false, 
                LocalName = "לינקסים מוטורס בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3897", 
                SearchFields = "3897,לעתיד (לאון)", 
                Inactive = false, 
                LocalName = "לעתיד (לאון)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "3905", 
                SearchFields = "3905,מאיר חברה למכוניות ומשאיות", 
                Inactive = false, 
                LocalName = "מאיר חברה למכוניות ומשאיות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4", 
                SearchFields = "4,בית מכס נתב''ג", 
                Inactive = false, 
                LocalName = "בית מכס נתב''ג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4002", 
                SearchFields = "4002,גדות בע''מ", 
                Inactive = false, 
                LocalName = "גדות בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4028", 
                SearchFields = "4028,גדות מסופים לכימיקלים בע''מ", 
                Inactive = false, 
                LocalName = "גדות מסופים לכימיקלים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4093", 
                SearchFields = "4093,מעבר חברה בע''מ", 
                Inactive = false, 
                LocalName = "מעבר חברה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4119", 
                SearchFields = "4119,מחסני ערובה אלבני בע''מ לא פעיל 2", 
                Inactive = false, 
                LocalName = "מחסני ערובה אלבני בע''מ לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4127", 
                SearchFields = "4127,ניר איסט בונדד (5691) בע''מ", 
                Inactive = false, 
                LocalName = "ניר איסט בונדד (5691) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "414", 
                SearchFields = "414,מחסן מכירות נעמן אשדוד", 
                Inactive = false, 
                LocalName = "מחסן מכירות נעמן אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4143", 
                SearchFields = "4143,מחסני ערובה אלבני בע''מ לא פעיל 3", 
                Inactive = false, 
                LocalName = "מחסני ערובה אלבני בע''מ לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4150", 
                SearchFields = "4150,ח.ל. נכסים בע''מ (קישון)", 
                Inactive = false, 
                LocalName = "ח.ל. נכסים בע''מ (קישון)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4168", 
                SearchFields = "4168,מחסני ערובה נעמן בע''מ", 
                Inactive = false, 
                LocalName = "מחסני ערובה נעמן בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4184", 
                SearchFields = "4184,ח.ל. נכסים בע''מ (עתלית)", 
                Inactive = false, 
                LocalName = "ח.ל. נכסים בע''מ (עתלית)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4218", 
                SearchFields = "4218,מפעלי קירור קרפרי חיפה בע''מ", 
                Inactive = false, 
                LocalName = "מפעלי קירור קרפרי חיפה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "422", 
                SearchFields = "422,מחסן מכירות נעמן חיפה", 
                Inactive = false, 
                LocalName = "מחסן מכירות נעמן חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4226", 
                SearchFields = "4226,ישראל-אמריקן דוולופר(ד.א.) בע", 
                Inactive = false, 
                LocalName = "ישראל-אמריקן דוולופר(ד.א.) בע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4242", 
                SearchFields = "4242,מסוף מחסני ערובה נעמן בע''מ", 
                Inactive = false, 
                LocalName = "מסוף מחסני ערובה נעמן בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4316", 
                SearchFields = "4316,ממגורות דגון - חיפה", 
                Inactive = false, 
                LocalName = "ממגורות דגון - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4325", 
                SearchFields = "4325,דלק חברה בע''מ", 
                Inactive = false, 
                LocalName = "דלק חברה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4374", 
                SearchFields = "4374,כלמוביל בע''מ- לא פעיל 19", 
                Inactive = false, 
                LocalName = "כלמוביל בע''מ- לא פעיל 19", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4390", 
                SearchFields = "4390,מ.נ.ס אינטרנשיונל", 
                Inactive = false, 
                LocalName = "מ.נ.ס אינטרנשיונל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "44415", 
                SearchFields = "44415,תחנת מכס נצרת", 
                Inactive = false, 
                LocalName = "תחנת מכס נצרת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4444", 
                SearchFields = "4444,נחל צין", 
                Inactive = false, 
                LocalName = "נחל צין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "45", 
                SearchFields = "45,אולם נוסעים נתב''ג", 
                Inactive = false, 
                LocalName = "אולם נוסעים נתב''ג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4507", 
                SearchFields = "4507,פז חברת הנפט בע''מ", 
                Inactive = false, 
                LocalName = "פז חברת הנפט בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4515", 
                SearchFields = "4515,סונול בע''מ איזור הנפט חיפה", 
                Inactive = false, 
                LocalName = "סונול בע''מ איזור הנפט חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4523", 
                SearchFields = "4523,צים חב השיט הישראלית בע''מ", 
                Inactive = false, 
                LocalName = "צים חב השיט הישראלית בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4531", 
                SearchFields = "4531,לים בע''מ שדרות פל-ים 9", 
                Inactive = false, 
                LocalName = "לים בע''מ שדרות פל-ים 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4549", 
                SearchFields = "4549,תעשיות רכב בע''מ נצרת עילית", 
                Inactive = false, 
                LocalName = "תעשיות רכב בע''מ נצרת עילית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4564", 
                SearchFields = "4564,אלחוט-ים בע''מ רח חירם 22-02", 
                Inactive = false, 
                LocalName = "אלחוט-ים בע''מ רח חירם 22-02", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4572", 
                SearchFields = "4572,לים בע''מ חוף שמן", 
                Inactive = false, 
                LocalName = "לים בע''מ חוף שמן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4580", 
                SearchFields = "4580,חברת לים בע''מ צידה לאניות", 
                Inactive = false, 
                LocalName = "חברת לים בע''מ צידה לאניות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4598", 
                SearchFields = "4598,אחים סקאל בעמ צידה לאניות וטיס", 
                Inactive = false, 
                LocalName = "אחים סקאל בעמ צידה לאניות וטיס", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4606", 
                SearchFields = "4606,קאר איסט יבוא רכב", 
                Inactive = false, 
                LocalName = "קאר איסט יבוא רכב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4614", 
                SearchFields = "4614,סוכנות מכוניות לים התיכון -לא פעיל 4", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון -לא פעיל 4", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4622", 
                SearchFields = "4622,צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 3", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס (ישראל) בע''מ - לא פעיל 3", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4630", 
                SearchFields = "4630,לים בע''מ לא פעיל 2", 
                Inactive = false, 
                LocalName = "לים בע''מ לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4648", 
                SearchFields = "4648,אחים סקאל דיוטי פרי-מעברים", 
                Inactive = false, 
                LocalName = "אחים סקאל דיוטי פרי-מעברים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4655", 
                SearchFields = "4655,אחים סקאל דיוטי פרי-חיפה", 
                Inactive = false, 
                LocalName = "אחים סקאל דיוטי פרי-חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4663", 
                SearchFields = "4663,חב' סקאל (חיפה) בע''מ", 
                Inactive = false, 
                LocalName = "חב' סקאל (חיפה) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4671", 
                SearchFields = "4671,חברת דיוטי פרי בע''מ - חיפה", 
                Inactive = false, 
                LocalName = "חברת דיוטי פרי בע''מ - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4689", 
                SearchFields = "4689,גדות אחסון ושנוע שותפות מוגבלת", 
                Inactive = false, 
                LocalName = "גדות אחסון ושנוע שותפות מוגבלת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4697", 
                SearchFields = "4697,ג'יימס ריצ'רדסון בע''מ - חיפה", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון בע''מ - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4705", 
                SearchFields = "4705,דיוטי פרי חיפה - פטור ושמור", 
                Inactive = false, 
                LocalName = "דיוטי פרי חיפה - פטור ושמור", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4713", 
                SearchFields = "4713,ג'יימס ריצ'רדסון-חיפה חוץ לנמל", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון-חיפה חוץ לנמל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4721", 
                SearchFields = "4721,מפרם", 
                Inactive = false, 
                LocalName = "מפרם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4739", 
                SearchFields = "4739,ג'יימס ריצ'רדסון- לא פעיל", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון- לא פעיל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4747", 
                SearchFields = "4747,שרותי תעופה חיפה", 
                Inactive = false, 
                LocalName = "שרותי תעופה חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4754", 
                SearchFields = "4754,שרותי תעופה מגידו", 
                Inactive = false, 
                LocalName = "שרותי תעופה מגידו", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4762", 
                SearchFields = "4762,ג'יימס ריצ'רדסון - נהר הירדן", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון - נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4788", 
                SearchFields = "4788,ריצרדסון בע''מ -נהר הירדן", 
                Inactive = false, 
                LocalName = "ריצרדסון בע''מ -נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4796", 
                SearchFields = "4796,ג'יימס ריצ'רדסון בע''מ", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4804", 
                SearchFields = "4804,ג'יימס ריצ'רדסון בע''מ", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4812", 
                SearchFields = "4812,ג'יימס ריצ'רדסון-אל שרד שותפות מוגבלת", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון-אל שרד שותפות מוגבלת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4820", 
                SearchFields = "4820,מיכל נגרין עיצובים בע''מ", 
                Inactive = false, 
                LocalName = "מיכל נגרין עיצובים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4822", 
                SearchFields = "4822,מסוף מילניום אשדוד בעמ", 
                Inactive = false, 
                LocalName = "מסוף מילניום אשדוד בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "49", 
                SearchFields = "49,בלדרות אווירית בינלאומית", 
                Inactive = false, 
                LocalName = "בלדרות אווירית בינלאומית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4960", 
                SearchFields = "4960,לים בע''מ (פטור ושמור)", 
                Inactive = false, 
                LocalName = "לים בע''מ (פטור ושמור)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4978", 
                SearchFields = "4978,לים בע''מ- חיפה", 
                Inactive = false, 
                LocalName = "לים בע''מ- חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4986", 
                SearchFields = "4986,תעשיות אלקטרוכימיות(2591) זמני", 
                Inactive = false, 
                LocalName = "תעשיות אלקטרוכימיות(2591) זמני", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "4994", 
                SearchFields = "4994,נמל חיפה (להעברות מנמל לנמל)", 
                Inactive = false, 
                LocalName = "נמל חיפה (להעברות מנמל לנמל)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "5", 
                SearchFields = "5,בית מכס ירושלים", 
                Inactive = false, 
                LocalName = "בית מכס ירושלים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "5009", 
                SearchFields = "5009,מחסן מכס משב''ט", 
                Inactive = false, 
                LocalName = "מחסן מכס משב''ט", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "505", 
                SearchFields = "505,דואר חבילות מרכז", 
                Inactive = false, 
                LocalName = "דואר חבילות מרכז", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "51", 
                SearchFields = "51,דואר חבילות ירושלים", 
                Inactive = false, 
                LocalName = "דואר חבילות ירושלים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "5181", 
                SearchFields = "5181,דואר חבילות חיפה", 
                Inactive = false, 
                LocalName = "דואר חבילות חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "539", 
                SearchFields = "539,צריפין", 
                Inactive = false, 
                LocalName = "צריפין", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "5397", 
                SearchFields = "5397,מחסן דלק", 
                Inactive = false, 
                LocalName = "מחסן דלק", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "559", 
                SearchFields = "559,בית מכס אילת", 
                Inactive = false, 
                LocalName = "בית מכס אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "596", 
                SearchFields = "596,דואר חבילות אילת", 
                Inactive = false, 
                LocalName = "דואר חבילות אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6460", 
                SearchFields = "6460,באג אלקטרוניקה", 
                Inactive = false, 
                LocalName = "באג אלקטרוניקה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6478", 
                SearchFields = "6478,באג מולטי סיסטיים בע''מ", 
                Inactive = false, 
                LocalName = "באג מולטי סיסטיים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "65", 
                SearchFields = "65,אגף היהלומים", 
                Inactive = false, 
                LocalName = "אגף היהלומים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6502", 
                SearchFields = "6502,פולימוד די.אפ.", 
                Inactive = false, 
                LocalName = "פולימוד די.אפ.", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6510", 
                SearchFields = "6510,אייר פרנס נ.ת. לוד", 
                Inactive = false, 
                LocalName = "אייר פרנס נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6528", 
                SearchFields = "6528,אל-על נ.ת. לוד", 
                Inactive = false, 
                LocalName = "אל-על נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6536", 
                SearchFields = "6536,ת.מ.מ. תעשיות מזון מטוסים בעמ", 
                Inactive = false, 
                LocalName = "ת.מ.מ. תעשיות מזון מטוסים בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6544", 
                SearchFields = "6544,שרותי תעופה בע''מ נ.ת. לוד", 
                Inactive = false, 
                LocalName = "שרותי תעופה בע''מ נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6551", 
                SearchFields = "6551,קיטרינג בע''מ נ.ת. לוד", 
                Inactive = false, 
                LocalName = "קיטרינג בע''מ נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6569", 
                SearchFields = "6569,רמתם חב לתעשיה ומסחר בע''מ לוד", 
                Inactive = false, 
                LocalName = "רמתם חב לתעשיה ומסחר בע''מ לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6577", 
                SearchFields = "6577,ב.או.אי.סי נ.ת. לוד", 
                Inactive = false, 
                LocalName = "ב.או.אי.סי נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6585", 
                SearchFields = "6585,ה. שטרן תכשיטים נ.ת. לוד", 
                Inactive = false, 
                LocalName = "ה. שטרן תכשיטים נ.ת. לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6593", 
                SearchFields = "6593,חברת סטימצקי (5002) בע''מ", 
                Inactive = false, 
                LocalName = "חברת סטימצקי (5002) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6601", 
                SearchFields = "6601,הצורפים בע''מ", 
                Inactive = false, 
                LocalName = "הצורפים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6619", 
                SearchFields = "6619,החברה הממשלתית למדליות ומטבעות", 
                Inactive = false, 
                LocalName = "החברה הממשלתית למדליות ומטבעות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6627", 
                SearchFields = "6627,סקאל שטרן שעונים בע''מ -שותפות", 
                Inactive = false, 
                LocalName = "סקאל שטרן שעונים בע''מ -שותפות", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6635", 
                SearchFields = "6635,ארקיע קוי תעופה ישראליים בע''מ", 
                Inactive = false, 
                LocalName = "ארקיע קוי תעופה ישראליים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6643", 
                SearchFields = "6643,אל-על נתב''ג", 
                Inactive = false, 
                LocalName = "אל-על נתב''ג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6650", 
                SearchFields = "6650,דיוטי פרי ספורט בע''מ", 
                Inactive = false, 
                LocalName = "דיוטי פרי ספורט בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "66666666", 
                SearchFields = "66666666,מעבר גבול גשר נהר הירדן", 
                Inactive = false, 
                LocalName = "מעבר גבול גשר נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6668", 
                SearchFields = "6668,ג'ימס ריצ'רדסון- נתב''ג", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסון- נתב''ג", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6676", 
                SearchFields = "6676,סקאל ספורט בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "סקאל ספורט בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6684", 
                SearchFields = "6684,ג'ימס ריצ'רדסון ישראל בעמ-נתבג לא פעיל 1", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסון ישראל בעמ-נתבג לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6700", 
                SearchFields = "6700,ג'ימס ריצ'רדסון ישראל בעמ-נתבג לא פעיל 2", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסון ישראל בעמ-נתבג לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6718", 
                SearchFields = "6718,ג'ימס ריצ'רדסוון טרמינל 1", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסוון טרמינל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6726", 
                SearchFields = "6726,ג'יימס ריצ'רדסון בע''מ - לא פעיל 2", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון בע''מ - לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6734", 
                SearchFields = "6734,סקאל מגה ספורט (שותפות)", 
                Inactive = false, 
                LocalName = "סקאל מגה ספורט (שותפות)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6742", 
                SearchFields = "6742,חברת סקאל דיוטי-פרי לא פעיל", 
                Inactive = false, 
                LocalName = "חברת סקאל דיוטי-פרי לא פעיל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6759", 
                SearchFields = "6759,חברת התעופה אל-על", 
                Inactive = false, 
                LocalName = "חברת התעופה אל-על", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "67656", 
                SearchFields = "67656,בית מכס אשדוד", 
                Inactive = false, 
                LocalName = "בית מכס אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6767", 
                SearchFields = "6767,חברת אל-שופ בע''מ", 
                Inactive = false, 
                LocalName = "חברת אל-שופ בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6775", 
                SearchFields = "6775,חברת סקאל דיוטי פרי בע''מ", 
                Inactive = false, 
                LocalName = "חברת סקאל דיוטי פרי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6783", 
                SearchFields = "6783,ג'ימס ריצ'רדסון פרופריטאבי", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסון פרופריטאבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6791", 
                SearchFields = "6791,סקאל דיוטי-פרי(לוד) אלקטרוניקה", 
                Inactive = false, 
                LocalName = "סקאל דיוטי-פרי(לוד) אלקטרוניקה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6809", 
                SearchFields = "6809,סקאל דיוטי פרי (פטור ושמור)", 
                Inactive = false, 
                LocalName = "סקאל דיוטי פרי (פטור ושמור)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6817", 
                SearchFields = "6817,ג'יימס ריצ'רדסון (פטור ושמור)", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון (פטור ושמור)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6833", 
                SearchFields = "6833,סקאל דיוטי-פרי אלקטרוניקה (3)", 
                Inactive = false, 
                LocalName = "סקאל דיוטי-פרי אלקטרוניקה (3)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6841", 
                SearchFields = "6841,סקאל דיוטי-פרי (4)", 
                Inactive = false, 
                LocalName = "סקאל דיוטי-פרי (4)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6858", 
                SearchFields = "6858,סקאל מגה ספורט (שותפות) (2)", 
                Inactive = false, 
                LocalName = "סקאל מגה ספורט (שותפות) (2)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6866", 
                SearchFields = "6866,ריצ'רדסון (עגלות)", 
                Inactive = false, 
                LocalName = "ריצ'רדסון (עגלות)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6874", 
                SearchFields = "6874,ריצ'רדסון-אם.ג'י.אס ש.מוגבלת", 
                Inactive = false, 
                LocalName = "ריצ'רדסון-אם.ג'י.אס ש.מוגבלת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6882", 
                SearchFields = "6882,סקאל ספורט (אופנת ילדים)", 
                Inactive = false, 
                LocalName = "סקאל ספורט (אופנת ילדים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6890", 
                SearchFields = "6890,ס.ח.א. נעליים", 
                Inactive = false, 
                LocalName = "ס.ח.א. נעליים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6908", 
                SearchFields = "6908,או.אל.בר (תכשיטים ואופנה)", 
                Inactive = false, 
                LocalName = "או.אל.בר (תכשיטים ואופנה)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6916", 
                SearchFields = "6916,בר פוינט אוף סיל-ספרים ועתונים", 
                Inactive = false, 
                LocalName = "בר פוינט אוף סיל-ספרים ועתונים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6924", 
                SearchFields = "6924,ג'ק קובה", 
                Inactive = false, 
                LocalName = "ג'ק קובה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6932", 
                SearchFields = "6932,התו השמיני דיוטי פרי", 
                Inactive = false, 
                LocalName = "התו השמיני דיוטי פרי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6940", 
                SearchFields = "6940,סולריס (משקפיים)", 
                Inactive = false, 
                LocalName = "סולריס (משקפיים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6957", 
                SearchFields = "6957,חדד מלאכת יד בע''מ", 
                Inactive = false, 
                LocalName = "חדד מלאכת יד בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6965", 
                SearchFields = "6965,סטימצקי גרופ בע''מ", 
                Inactive = false, 
                LocalName = "סטימצקי גרופ בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6973", 
                SearchFields = "6973,דן קסידי (3991) חברה בע''מ", 
                Inactive = false, 
                LocalName = "דן קסידי (3991) חברה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "6999", 
                SearchFields = "6999,ג.ר.וי שותפות מוגבלת", 
                Inactive = false, 
                LocalName = "ג.ר.וי שותפות מוגבלת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "70", 
                SearchFields = "70,מטה מעברים", 
                Inactive = false, 
                LocalName = "מטה מעברים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "72", 
                SearchFields = "72,תחנת מכס באר שבע", 
                Inactive = false, 
                LocalName = "תחנת מכס באר שבע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "78", 
                SearchFields = "78,מסוף רפיח", 
                Inactive = false, 
                LocalName = "מסוף רפיח", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "79", 
                SearchFields = "79,כרם שלום", 
                Inactive = false, 
                LocalName = "כרם שלום", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "7997", 
                SearchFields = "7997,יורוגל בונדד  (חצרים)", 
                Inactive = false, 
                LocalName = "יורוגל בונדד  (חצרים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8003", 
                SearchFields = "8003,אפשטיין ושות  בע''מ", 
                Inactive = false, 
                LocalName = "אפשטיין ושות  בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8011", 
                SearchFields = "8011,מסוף אוברסיז קומרס בע''מ - אשדוד", 
                Inactive = false, 
                LocalName = "מסוף אוברסיז קומרס בע''מ - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8021", 
                SearchFields = "8021,אתר לא ידוע", 
                Inactive = false, 
                LocalName = "אתר לא ידוע", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8029", 
                SearchFields = "8029,אשדוד בונדד בע''מ", 
                Inactive = false, 
                LocalName = "אשדוד בונדד בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8037", 
                SearchFields = "8037,ישקול סחר", 
                Inactive = false, 
                LocalName = "ישקול סחר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8045", 
                SearchFields = "8045,דקל מחסני ערובה בע''מ", 
                Inactive = false, 
                LocalName = "דקל מחסני ערובה בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8052", 
                SearchFields = "8052,אורשר", 
                Inactive = false, 
                LocalName = "אורשר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8060", 
                SearchFields = "8060,מחסני ערובה כללים אוסטרליים", 
                Inactive = false, 
                LocalName = "מחסני ערובה כללים אוסטרליים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8078", 
                SearchFields = "8078,אוברסיז קומרס בע''מ חצרים", 
                Inactive = false, 
                LocalName = "אוברסיז קומרס בע''מ חצרים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8102", 
                SearchFields = "8102,מסוף היובל  (אוברסיס) לא פעיל 2", 
                Inactive = false, 
                LocalName = "מסוף היובל  (אוברסיס) לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8110", 
                SearchFields = "8110,סלע ש.ג. מסחר בע''מ", 
                Inactive = false, 
                LocalName = "סלע ש.ג. מסחר בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8128", 
                SearchFields = "8128,מעבר בע''מ אשדוד", 
                Inactive = false, 
                LocalName = "מעבר בע''מ אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8136", 
                SearchFields = "8136,שיאון שרותי אחסון שנוע והובלה", 
                Inactive = false, 
                LocalName = "שיאון שרותי אחסון שנוע והובלה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8144", 
                SearchFields = "8144,אחים סקאל דיוטי פרי בע''מ", 
                Inactive = false, 
                LocalName = "אחים סקאל דיוטי פרי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8151", 
                SearchFields = "8151,לוגיסטיקר בונדד (8991) בע''מ", 
                Inactive = false, 
                LocalName = "לוגיסטיקר בונדד (8991) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8169", 
                SearchFields = "8169,עץ ירוק - השקעות ומסחר", 
                Inactive = false, 
                LocalName = "עץ ירוק - השקעות ומסחר", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8177", 
                SearchFields = "8177,חברת ג'יימס ריצ'רדסון בעמ", 
                Inactive = false, 
                LocalName = "חברת ג'יימס ריצ'רדסון בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8185", 
                SearchFields = "8185,אשדוד בונדד מ.מטעןומחסן בע''מ", 
                Inactive = false, 
                LocalName = "אשדוד בונדד מ.מטעןומחסן בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8193", 
                SearchFields = "8193,לוגיסטי פוינט בע''מ", 
                Inactive = false, 
                LocalName = "לוגיסטי פוינט בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "82", 
                SearchFields = "82,מטולה", 
                Inactive = false, 
                LocalName = "מטולה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8219", 
                SearchFields = "8219,סלע ש.ג. מסחר בע''מ- לא פעיל", 
                Inactive = false, 
                LocalName = "סלע ש.ג. מסחר בע''מ- לא פעיל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8227", 
                SearchFields = "8227,קונטרם בע''מ", 
                Inactive = false, 
                LocalName = "קונטרם בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8235", 
                SearchFields = "8235,מפעלי קרור ואחסנה אשדוד בע''מ", 
                Inactive = false, 
                LocalName = "מפעלי קרור ואחסנה אשדוד בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8243", 
                SearchFields = "8243,אדנים טרי.פי.אל. בע''מ", 
                Inactive = false, 
                LocalName = "אדנים טרי.פי.אל. בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8250", 
                SearchFields = "8250,חב' כלמוביל בע''מ אשדוד", 
                Inactive = false, 
                LocalName = "חב' כלמוביל בע''מ אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8276", 
                SearchFields = "8276,אחים סקאל דיוטי פרי בע''מ", 
                Inactive = false, 
                LocalName = "אחים סקאל דיוטי פרי בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8284", 
                SearchFields = "8284,ג'ימס ריצ'רדסון -אשדוד מחסן 301", 
                Inactive = false, 
                LocalName = "ג'ימס ריצ'רדסון -אשדוד מחסן 301", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8318", 
                SearchFields = "8318,טלקאר חברה בע''מ- לא פעיל 9", 
                Inactive = false, 
                LocalName = "טלקאר חברה בע''מ- לא פעיל 9", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8326", 
                SearchFields = "8326,ממגורות דגון - אשדוד", 
                Inactive = false, 
                LocalName = "ממגורות דגון - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8334", 
                SearchFields = "8334,אחים סקאל בע''מ", 
                Inactive = false, 
                LocalName = "אחים סקאל בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8342", 
                SearchFields = "8342,חברה לים", 
                Inactive = false, 
                LocalName = "חברה לים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8359", 
                SearchFields = "8359,מפעלי רכב (מיל בע''מ)", 
                Inactive = false, 
                LocalName = "מפעלי רכב (מיל בע''מ)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8367", 
                SearchFields = "8367,חברת לים בע''מ", 
                Inactive = false, 
                LocalName = "חברת לים בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8375", 
                SearchFields = "8375,חברת יוניון מוטורס בע''מ", 
                Inactive = false, 
                LocalName = "חברת יוניון מוטורס בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8383", 
                SearchFields = "8383,חברת המזרח לשווק מכוניות 4891", 
                Inactive = false, 
                LocalName = "חברת המזרח לשווק מכוניות 4891", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8391", 
                SearchFields = "8391,לובינסקי דוד בע''מ אשדוד", 
                Inactive = false, 
                LocalName = "לובינסקי דוד בע''מ אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8409", 
                SearchFields = "8409,כלמוטור בע''מ אשדוד", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8417", 
                SearchFields = "8417,רמתם (8891) בע''מ", 
                Inactive = false, 
                LocalName = "רמתם (8891) בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8425", 
                SearchFields = "8425,קמור רכב בע''מ- בני עייש", 
                Inactive = false, 
                LocalName = "קמור רכב בע''מ- בני עייש", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8433", 
                SearchFields = "8433,ת.ס. אוטומוטיב (ישראל)", 
                Inactive = false, 
                LocalName = "ת.ס. אוטומוטיב (ישראל)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8441", 
                SearchFields = "8441,כלמוטור בע''מ", 
                Inactive = false, 
                LocalName = "כלמוטור בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8458", 
                SearchFields = "8458,יו.אמ.אי יוניברסל מוטורס בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "יו.אמ.אי יוניברסל מוטורס בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8466", 
                SearchFields = "8466,יו.אמ.אי יוניברסל מוטורס בע''מ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "יו.אמ.אי יוניברסל מוטורס בע''מ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8474", 
                SearchFields = "8474,כלמוביל בע''מ- אשדוד רח' היוזמה", 
                Inactive = false, 
                LocalName = "כלמוביל בע''מ- אשדוד רח' היוזמה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8482", 
                SearchFields = "8482,פרי שופינג בע''מ", 
                Inactive = false, 
                LocalName = "פרי שופינג בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8490", 
                SearchFields = "8490,טלקאר בע''מ אשקלון", 
                Inactive = false, 
                LocalName = "טלקאר בע''מ אשקלון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "85", 
                SearchFields = "85,יהוד - משרד הביטחון", 
                Inactive = false, 
                LocalName = "יהוד - משרד הביטחון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8508", 
                SearchFields = "8508,יוניון מוטורס בע''מ פארק ראם", 
                Inactive = false, 
                LocalName = "יוניון מוטורס בע''מ פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8516", 
                SearchFields = "8516,חברת כלמוביל בע''מ", 
                Inactive = false, 
                LocalName = "חברת כלמוביל בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8524", 
                SearchFields = "8524,קונטרם (חצרים)", 
                Inactive = false, 
                LocalName = "קונטרם (חצרים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8532", 
                SearchFields = "8532,צ'מפיון מוטורס בע''מ", 
                Inactive = false, 
                LocalName = "צ'מפיון מוטורס בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8540", 
                SearchFields = "8540,יו.אמ.אי בעמ (פארק ראם)", 
                Inactive = false, 
                LocalName = "יו.אמ.אי בעמ (פארק ראם)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8557", 
                SearchFields = "8557,סלע ש.ג. למסחר בע''מ (חצרים)", 
                Inactive = false, 
                LocalName = "סלע ש.ג. למסחר בע''מ (חצרים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8565", 
                SearchFields = "8565,תברה ישראלית לאוטומובילים", 
                Inactive = false, 
                LocalName = "תברה ישראלית לאוטומובילים", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8573", 
                SearchFields = "8573,מכשירי תנועה ומכוניות 4002 בעמ", 
                Inactive = false, 
                LocalName = "מכשירי תנועה ומכוניות 4002 בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8581", 
                SearchFields = "8581,חברת שיר בע''מ -שלמה יבואני רכב", 
                Inactive = false, 
                LocalName = "חברת שיר בע''מ -שלמה יבואני רכב", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8599", 
                SearchFields = "8599,חב' בני משה קרסו פארק ראם", 
                Inactive = false, 
                LocalName = "חב' בני משה קרסו פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8607", 
                SearchFields = "8607,חב' פסיפיק יבואני רכב פארק ראם", 
                Inactive = false, 
                LocalName = "חב' פסיפיק יבואני רכב פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8615", 
                SearchFields = "8615,דיוטי פרי עזה - מושתהה", 
                Inactive = false, 
                LocalName = "דיוטי פרי עזה - מושתהה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8623", 
                SearchFields = "8623,חב' דנאל בע''מ (2)", 
                Inactive = false, 
                LocalName = "חב' דנאל בע''מ (2)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8631", 
                SearchFields = "8631,קמור בע''מ פארק ראם", 
                Inactive = false, 
                LocalName = "קמור בע''מ פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8649", 
                SearchFields = "8649,צ'מפיון-סכוקאר פארק ראם", 
                Inactive = false, 
                LocalName = "צ'מפיון-סכוקאר פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8656", 
                SearchFields = "8656,צ'מפיון (חצרים) - פארק ראם", 
                Inactive = false, 
                LocalName = "צ'מפיון (חצרים) - פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8664", 
                SearchFields = "8664,צ'מפיון2 - פארק ראם", 
                Inactive = false, 
                LocalName = "צ'מפיון2 - פארק ראם", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8672", 
                SearchFields = "8672,אוברסיז קומרס בע''מ - זמני", 
                Inactive = false, 
                LocalName = "אוברסיז קומרס בע''מ - זמני", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8680", 
                SearchFields = "8680,אורשר (ניר גלים)", 
                Inactive = false, 
                LocalName = "אורשר (ניר גלים)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8698", 
                SearchFields = "8698,סוכנות מכוניות לים התיכון -לא פעיל 5", 
                Inactive = false, 
                LocalName = "סוכנות מכוניות לים התיכון -לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8706", 
                SearchFields = "8706,ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 5", 
                Inactive = false, 
                LocalName = "ק מ י קוריאה מוטורס ישראל בע''''מ - לא פעיל 5", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8714", 
                SearchFields = "8714,סלע מסחר ולוגיסטיקה (1999) בעמ- לא פעיל 1", 
                Inactive = false, 
                LocalName = "סלע מסחר ולוגיסטיקה (1999) בעמ- לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8722", 
                SearchFields = "8722,סקאל ספורט בע''מ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "סקאל ספורט בע''מ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8730", 
                SearchFields = "8730,ג'יימס ריצ'רדסון בע''מ - אשדוד", 
                Inactive = false, 
                LocalName = "ג'יימס ריצ'רדסון בע''מ - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8748", 
                SearchFields = "8748,סלע מסחר ולוגיסטיקה (1999) בעמ- לא פעיל 2", 
                Inactive = false, 
                LocalName = "סלע מסחר ולוגיסטיקה (1999) בעמ- לא פעיל 2", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8771", 
                SearchFields = "8771,שרותי תעופה שדה תימן", 
                Inactive = false, 
                LocalName = "שרותי תעופה שדה תימן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8789", 
                SearchFields = "8789,צ'מפיון קאר ש.מ.", 
                Inactive = false, 
                LocalName = "צ'מפיון קאר ש.מ.", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8797", 
                SearchFields = "8797,יוניברסל מוטורס בע''מ", 
                Inactive = false, 
                LocalName = "יוניברסל מוטורס בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "8805", 
                SearchFields = "8805,ש.י.ר- שלומה יבואני רכב בע''מ", 
                Inactive = false, 
                LocalName = "ש.י.ר- שלומה יבואני רכב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9", 
                SearchFields = "9,בית מכס אילת", 
                Inactive = false, 
                LocalName = "בית מכס אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9001", 
                SearchFields = "9001,פרמוט", 
                Inactive = false, 
                LocalName = "פרמוט", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9019", 
                SearchFields = "9019,טיטן בנץ", 
                Inactive = false, 
                LocalName = "טיטן בנץ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9027", 
                SearchFields = "9027,חוטי הנגב בע''מ", 
                Inactive = false, 
                LocalName = "חוטי הנגב בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9043", 
                SearchFields = "9043,רד סי פיש פארם בע''מ", 
                Inactive = false, 
                LocalName = "רד סי פיש פארם בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9076", 
                SearchFields = "9076,רויאל קרסט בע''מ", 
                Inactive = false, 
                LocalName = "רויאל קרסט בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9092", 
                SearchFields = "9092,מיקרוקול בע''מ", 
                Inactive = false, 
                LocalName = "מיקרוקול בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9118", 
                SearchFields = "9118,נטפים-מגל בע''מ", 
                Inactive = false, 
                LocalName = "נטפים-מגל בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9589", 
                SearchFields = "9589,מזמוריה-הר חומה", 
                Inactive = false, 
                LocalName = "מזמוריה-הר חומה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "99", 
                SearchFields = "99,הנהלת המכס", 
                Inactive = false, 
                LocalName = "הנהלת המכס", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "9993", 
                SearchFields = "9993,ללא מעבר פנימי", 
                Inactive = false, 
                LocalName = "ללא מעבר פנימי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILAKK", 
                SearchFields = "ILAKK,מפרץ עכו-חיפה 1934 בעמ", 
                Inactive = false, 
                LocalName = "מפרץ עכו-חיפה 1934 בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILAKL", 
                SearchFields = "ILAKL,מסוף אשקלון- קצא''א", 
                Inactive = false, 
                LocalName = "מסוף אשקלון- קצא''א", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILALN", 
                SearchFields = "ILALN,מעבר גבול אלנבי", 
                Inactive = false, 
                LocalName = "מעבר גבול אלנבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILALN1", 
                SearchFields = "ILALN1,תחנת מכס גשר אלנבי", 
                Inactive = false, 
                LocalName = "תחנת מכס גשר אלנבי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILARV", 
                SearchFields = "ILARV,מעבר גבול רבין (ערבה)", 
                Inactive = false, 
                LocalName = "מעבר גבול רבין (ערבה)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILARV1", 
                SearchFields = "ILARV1,תחנת מכס רבין (ערבה)", 
                Inactive = false, 
                LocalName = "תחנת מכס רבין (ערבה)", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILASB", 
                SearchFields = "ILASB,אשדוד בונדד- אוירי", 
                Inactive = false, 
                LocalName = "אשדוד בונדד- אוירי", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILASH", 
                SearchFields = "ILASH,נמל אשדוד", 
                Inactive = false, 
                LocalName = "נמל אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILBGA", 
                SearchFields = "ILBGA,נמל תעופה בן גוריון", 
                Inactive = false, 
                LocalName = "נמל תעופה בן גוריון", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILBRN", 
                SearchFields = "ILBRN,בירנית", 
                Inactive = false, 
                LocalName = "בירנית", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILBXQ", 
                SearchFields = "ILBXQ,אשדוד בונדד מ.מטען ומחסן בע''מ", 
                Inactive = false, 
                LocalName = "אשדוד בונדד מ.מטען ומחסן בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILCXQ", 
                SearchFields = "ILCXQ,קונטרם אשדוד", 
                Inactive = false, 
                LocalName = "קונטרם אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILETH", 
                SearchFields = "ILETH,נמל אילת", 
                Inactive = false, 
                LocalName = "נמל אילת", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILHAD", 
                SearchFields = "ILHAD,נמל חדרה", 
                Inactive = false, 
                LocalName = "נמל חדרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILHFA", 
                SearchFields = "ILHFA,נמל חיפה", 
                Inactive = false, 
                LocalName = "נמל חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILJCT", 
                SearchFields = "ILJCT,מ. מדידה וסריקה בי''ל-נהר הירדן", 
                Inactive = false, 
                LocalName = "מ. מדידה וסריקה בי''ל-נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILJOR", 
                SearchFields = "ILJOR,מעבר גבול גשר נהר הירדן", 
                Inactive = false, 
                LocalName = "מעבר גבול גשר נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILJOR1", 
                SearchFields = "ILJOR1,בית מכס גשר נהר הירדן", 
                Inactive = false, 
                LocalName = "בית מכס גשר נהר הירדן", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILKNT", 
                SearchFields = "ILKNT,קונטרה", 
                Inactive = false, 
                LocalName = "קונטרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILMLA", 
                SearchFields = "ILMLA,מסוף מילניום אשדוד בעמ", 
                Inactive = false, 
                LocalName = "מסוף מילניום אשדוד בעמ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILMMN", 
                SearchFields = "ILMMN,ממן לוד", 
                Inactive = false, 
                LocalName = "ממן לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILMSP", 
                SearchFields = "ILMSP,מספנות ישראל", 
                Inactive = false, 
                LocalName = "מספנות ישראל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILMTS", 
                SearchFields = "ILMTS,מסוף 207 בע''מ", 
                Inactive = false, 
                LocalName = "מסוף 207 בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILNZN", 
                SearchFields = "ILNZN,מעבר גבול ניצנה", 
                Inactive = false, 
                LocalName = "מעבר גבול ניצנה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILNZN1", 
                SearchFields = "ILNZN1,תחנת מכס מעבר ניצנה", 
                Inactive = false, 
                LocalName = "תחנת מכס מעבר ניצנה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILOVM", 
                SearchFields = "ILOVM,מילניום בע''מ", 
                Inactive = false, 
                LocalName = "מילניום בע''מ", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILOVO", 
                SearchFields = "ILOVO,מסוף אוברסיז קומרס בע''מ - אשדוד", 
                Inactive = false, 
                LocalName = "מסוף אוברסיז קומרס בע''מ - אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILOVR", 
                SearchFields = "ILOVR,מסוף אוברסיז קומרס בע''מ - חיפה", 
                Inactive = false, 
                LocalName = "מסוף אוברסיז קומרס בע''מ - חיפה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILOVS", 
                SearchFields = "ILOVS,מחסן מסוף אוברסיז קומרס בע''מ -  אשדוד", 
                Inactive = false, 
                LocalName = "מחסן מסוף אוברסיז קומרס בע''מ -  אשדוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILOVY", 
                SearchFields = "ILOVY,מסוף היובל  (אוברסיס) לא פעיל 1", 
                Inactive = false, 
                LocalName = "מסוף היובל  (אוברסיס) לא פעיל 1", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILRHN", 
                SearchFields = "ILRHN,ראש הנקרה", 
                Inactive = false, 
                LocalName = "ראש הנקרה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILSWS", 
                SearchFields = "ILSWS,סויספורט", 
                Inactive = false, 
                LocalName = "סויספורט", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILTAB", 
                SearchFields = "ILTAB,אולם נוסעים טאבה", 
                Inactive = false, 
                LocalName = "אולם נוסעים טאבה", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILTLV", 
                SearchFields = "ILTLV,ממן לוד", 
                Inactive = false, 
                LocalName = "ממן לוד", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILURO", 
                SearchFields = "ILURO,יורוגל", 
                Inactive = false, 
                LocalName = "יורוגל", 
			});
			 
            all.Add(new SiteLookupDetails()
            {    
                Code = "ILVDA", 
                SearchFields = "ILVDA,עובדה", 
                Inactive = false, 
                LocalName = "עובדה", 
			});
			
            return all;
       }

	    public void MapPoco(SiteLookup newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(SiteLookup rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

