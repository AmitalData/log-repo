
   
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
   public class CheckEntityTypeDetails : CheckEntityType, ICloseTable<CheckEntityType, CheckEntityTypeDetails>
   {
       public List<CheckEntityTypeDetails> GetAll()
       {
		    var all = new List<CheckEntityTypeDetails>();  
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,מכולת יבוא", 
                Inactive = false, 
                LocalName = "מכולת יבוא", 
                EnglishName = "Import Container", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "10", 
                SearchFields = "10,משאית TIR", 
                Inactive = false, 
                LocalName = "משאית TIR", 
                EnglishName = "TIR truck", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "11", 
                SearchFields = "11,מחסן", 
                Inactive = false, 
                LocalName = "מחסן", 
                EnglishName = "warehouse", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "12", 
                SearchFields = "12,גוש במחסן", 
                Inactive = false, 
                LocalName = "גוש במחסן", 
                EnglishName = "Block in warehouse", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "13", 
                SearchFields = "13,תפיסה", 
                Inactive = false, 
                LocalName = "תפיסה", 
                EnglishName = "perception", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "14", 
                SearchFields = "14,בדיקת טיסה", 
                Inactive = false, 
                LocalName = "בדיקת טיסה", 
                EnglishName = "Flight check", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "15", 
                SearchFields = "15,מטען ליווי לנוסע", 
                Inactive = false, 
                LocalName = "מטען ליווי לנוסע", 
                EnglishName = "Passenger escort charge", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "16", 
                SearchFields = "16,אוניה", 
                Inactive = false, 
                LocalName = "אוניה", 
                EnglishName = "Ship", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "17", 
                SearchFields = "17,אספקה צידה לאוניה", 
                Inactive = false, 
                LocalName = "אספקה צידה לאוניה", 
                EnglishName = "Shipside supplies", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "18", 
                SearchFields = "18,יאכטה", 
                Inactive = false, 
                LocalName = "יאכטה", 
                EnglishName = "yacht", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "19", 
                SearchFields = "19,אדם", 
                Inactive = false, 
                LocalName = "אדם", 
                EnglishName = "person", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,מכולת יצוא", 
                Inactive = false, 
                LocalName = "מכולת יצוא", 
                EnglishName = "Export Container", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "20", 
                SearchFields = "20,מטען בלדר יבוא", 
                Inactive = false, 
                LocalName = "מטען בלדר יבוא", 
                EnglishName = "Import Courier Cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "21", 
                SearchFields = "21,מטען בלדר יצוא", 
                Inactive = false, 
                LocalName = "מטען בלדר יצוא", 
                EnglishName = "Export Courier Cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "22", 
                SearchFields = "22,מטען לא מזוהה מהסבות", 
                Inactive = false, 
                LocalName = "מטען לא מזוהה מהסבות", 
                EnglishName = "Unidentified cargo from the causes", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "23", 
                SearchFields = "23,יבואן/סוכן/מחסן מהסבה", 
                Inactive = false, 
                LocalName = "יבואן/סוכן/מחסן מהסבה", 
                EnglishName = "Importer_agent_cause warehouse", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "24", 
                SearchFields = "24,אספקה למטוס מהסבה", 
                Inactive = false, 
                LocalName = "אספקה למטוס מהסבה", 
                EnglishName = "Supply to the aircraft from the cause", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,מטען יבוא", 
                Inactive = false, 
                LocalName = "מטען יבוא", 
                EnglishName = "import cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,מטען יצוא", 
                Inactive = false, 
                LocalName = "מטען יצוא", 
                EnglishName = "Export Cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "5", 
                SearchFields = "5,מטען אווירי יבוא", 
                Inactive = false, 
                LocalName = "מטען אווירי יבוא", 
                EnglishName = "import air cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "6", 
                SearchFields = "6,מטען אווירי יצוא", 
                Inactive = false, 
                LocalName = "מטען אווירי יצוא", 
                EnglishName = "Export air cargo", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "7", 
                SearchFields = "7,רכב", 
                Inactive = false, 
                LocalName = "רכב", 
                EnglishName = "vehicle", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "8", 
                SearchFields = "8,רכב זמני", 
                Inactive = false, 
                LocalName = "רכב זמני", 
                EnglishName = "temporary vehicle", 
			});
			 
            all.Add(new CheckEntityTypeDetails()
            {    
                Code = "9", 
                SearchFields = "9,משאית", 
                Inactive = false, 
                LocalName = "משאית", 
                EnglishName = "truck", 
			});
			
            return all;
       }

	    public void MapPoco(CheckEntityType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(CheckEntityType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

