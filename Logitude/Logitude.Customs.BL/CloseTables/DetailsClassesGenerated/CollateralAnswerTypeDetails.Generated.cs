
   
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
   public class CollateralAnswerTypeDetails : CollateralAnswerType, ICloseTable<CollateralAnswerType, CollateralAnswerTypeDetails>
   {
       public List<CollateralAnswerTypeDetails> GetAll()
       {
		    var all = new List<CollateralAnswerTypeDetails>();  
            all.Add(new CollateralAnswerTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,בקשה לערבות", 
                Inactive = false, 
                LocalName = "בקשה לערבות", 
			});
			 
            all.Add(new CollateralAnswerTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,תיק ערבות", 
                Inactive = false, 
                LocalName = "תיק ערבות", 
			});
			 
            all.Add(new CollateralAnswerTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,בקשה לפיקדון", 
                Inactive = false, 
                LocalName = "בקשה לפיקדון", 
			});
			 
            all.Add(new CollateralAnswerTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,תיק פיקדון", 
                Inactive = false, 
                LocalName = "תיק פיקדון", 
			});
			
            return all;
       }

	    public void MapPoco(CollateralAnswerType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CollateralAnswerType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

