
   
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
   public class AccumalationStateDetails : AccumalationState, ICloseTable<AccumalationState, AccumalationStateDetails>
   {
       public List<AccumalationStateDetails> GetAll()
       {
		    var all = new List<AccumalationStateDetails>();  
            all.Add(new AccumalationStateDetails()
            {    
                Code = "1", 
                SearchFields = "1,accumulate above 998 items,צבור מעל 998 פריטים", 
                Name = "Accumulate Above 998 items", 
                LocalName = "צבור מעל 998 פריטים", 
			});
			 
            all.Add(new AccumalationStateDetails()
            {    
                Code = "2", 
                SearchFields = "2,always accumulate,צבור תמיד", 
                Name = "Always Accumulate", 
                LocalName = "צבור תמיד", 
			});
			 
            all.Add(new AccumalationStateDetails()
            {    
                Code = "3", 
                SearchFields = "3,never accumulate,ללא צבירה", 
                Name = "Never Accumulate", 
                LocalName = "ללא צבירה", 
			});
			
            return all;
       }

	    public void MapPoco(AccumalationState newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(AccumalationState rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.LocalName,",");
        }
   }
}

