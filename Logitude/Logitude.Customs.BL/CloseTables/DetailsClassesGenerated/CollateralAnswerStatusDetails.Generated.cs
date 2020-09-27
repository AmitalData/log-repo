
   
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
   public class CollateralAnswerStatusDetails : CollateralAnswerStatus, ICloseTable<CollateralAnswerStatus, CollateralAnswerStatusDetails>
   {
       public List<CollateralAnswerStatusDetails> GetAll()
       {
		    var all = new List<CollateralAnswerStatusDetails>();  
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "1", 
                SearchFields = "1,מאושר", 
                Inactive = false, 
                LocalName = "מאושר", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "10", 
                SearchFields = "10,ממתין לפתיחת תיק", 
                Inactive = false, 
                LocalName = "ממתין לפתיחת תיק", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "2", 
                SearchFields = "2,לא קיים", 
                Inactive = false, 
                LocalName = "לא קיים", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "3", 
                SearchFields = "3,לא בתוקף", 
                Inactive = false, 
                LocalName = "לא בתוקף", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "4", 
                SearchFields = "4,חסר", 
                Inactive = false, 
                LocalName = "חסר", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "5", 
                SearchFields = "5,שיוך שגוי", 
                Inactive = false, 
                LocalName = "שיוך שגוי", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "6", 
                SearchFields = "6,סוג בטוחה שגוי", 
                Inactive = false, 
                LocalName = "סוג בטוחה שגוי", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "7", 
                SearchFields = "7,חורג ביתר", 
                Inactive = false, 
                LocalName = "חורג ביתר", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "8", 
                SearchFields = "8,סטאטוס לא תקין", 
                Inactive = false, 
                LocalName = "סטאטוס לא תקין", 
			});
			 
            all.Add(new CollateralAnswerStatusDetails()
            {    
                Code = "9", 
                SearchFields = "9,חסר תנאים", 
                Inactive = false, 
                LocalName = "חסר תנאים", 
			});
			
            return all;
       }

	    public void MapPoco(CollateralAnswerStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CollateralAnswerStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

