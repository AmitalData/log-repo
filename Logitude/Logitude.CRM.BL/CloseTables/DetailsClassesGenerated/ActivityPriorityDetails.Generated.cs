
   
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
   public class ActivityPriorityDetails : ActivityPriority, ICloseTable<ActivityPriority, ActivityPriorityDetails>
   {
       public List<ActivityPriorityDetails> GetAll()
       {
		    var all = new List<ActivityPriorityDetails>();  
            all.Add(new ActivityPriorityDetails()
            {    
                Code = "01", 
                SearchFields = "01,Low,", 
                Name = "Low", 
			});
			 
            all.Add(new ActivityPriorityDetails()
            {    
                Code = "02", 
                SearchFields = "02,Normal,", 
                Name = "Normal", 
			});
			 
            all.Add(new ActivityPriorityDetails()
            {    
                Code = "03", 
                SearchFields = "03,High,", 
                Name = "High", 
			});
			
            return all;
       }

	    public void MapPoco(ActivityPriority newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ActivityPriority rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

