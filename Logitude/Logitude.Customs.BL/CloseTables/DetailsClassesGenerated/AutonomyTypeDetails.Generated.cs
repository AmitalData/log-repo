
   
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
   public class AutonomyTypeDetails : AutonomyType, ICloseTable<AutonomyType, AutonomyTypeDetails>
   {
       public List<AutonomyTypeDetails> GetAll()
       {
		    var all = new List<AutonomyTypeDetails>();  
            all.Add(new AutonomyTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,אוטונומיה רש''פ איוש", 
                Inactive = false, 
                LocalName = "אוטונומיה רש''פ איוש", 
			});
			 
            all.Add(new AutonomyTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,אוטונומיה רש''פ עזה", 
                Inactive = false, 
                LocalName = "אוטונומיה רש''פ עזה", 
			});
			 
            all.Add(new AutonomyTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,ישראל (לא אוטונומיה)", 
                Inactive = false, 
                LocalName = "ישראל (לא אוטונומיה)", 
			});
			
            return all;
       }

	    public void MapPoco(AutonomyType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(AutonomyType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

