
   
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
   public class ActivityTypeDetails : ActivityType, ICloseTable<ActivityType, ActivityTypeDetails>
   {
       public List<ActivityTypeDetails> GetAll()
       {
		    var all = new List<ActivityTypeDetails>();  
            all.Add(new ActivityTypeDetails()
            {    
                Code = "AP", 
                SearchFields = "AP,Appointment", 
                Name = "Appointment", 
			});
			 
            all.Add(new ActivityTypeDetails()
            {    
                Code = "CL", 
                SearchFields = "CL,Call", 
                Name = "Call", 
			});
			 
            all.Add(new ActivityTypeDetails()
            {    
                Code = "EI", 
                SearchFields = "EI,Email In", 
                Name = "Email In", 
			});
			 
            all.Add(new ActivityTypeDetails()
            {    
                Code = "EO", 
                SearchFields = "EO,Email Out", 
                Name = "Email Out", 
			});
			 
            all.Add(new ActivityTypeDetails()
            {    
                Code = "TS", 
                SearchFields = "TS,Task", 
                Name = "Task", 
			});
			 
            all.Add(new ActivityTypeDetails()
            {    
                Code = "TX", 
                SearchFields = "TX,Task Extended", 
                Name = "Task Extended", 
			});
			
            return all;
       }

	    public void MapPoco(ActivityType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ActivityType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

