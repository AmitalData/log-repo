
   
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.CLoseTable
{
   public class ActivityTimeTypeDetails : ActivityTimeType, ICloseTable<ActivityTimeType, ActivityTimeTypeDetails>
   {
       public List<ActivityTimeTypeDetails> GetAll()
       {
		    var all = new List<ActivityTimeTypeDetails>();  
            all.Add(new ActivityTimeTypeDetails()
            {    
                Code = "BS", 
                SearchFields = "BS,Busy", 
                Name = "Busy", 
			});
			 
            all.Add(new ActivityTimeTypeDetails()
            {    
                Code = "FR", 
                SearchFields = "FR,Free", 
                Name = "Free", 
			});
			 
            all.Add(new ActivityTimeTypeDetails()
            {    
                Code = "OF", 
                SearchFields = "OF,Out Of Office", 
                Name = "Out Of Office", 
			});
			 
            all.Add(new ActivityTimeTypeDetails()
            {    
                Code = "TN", 
                SearchFields = "TN,Tentative", 
                Name = "Tentative", 
			});
			
            return all;
       }

	    public void MapPoco(ActivityTimeType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ActivityTimeType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

