
   
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
   public class ActivityStatusDetails : ActivityStatus, ICloseTable<ActivityStatus, ActivityStatusDetails>
   {
       public List<ActivityStatusDetails> GetAll()
       {
		    var all = new List<ActivityStatusDetails>();  
            all.Add(new ActivityStatusDetails()
            {    
                Code = "C", 
                SearchFields = "C,Completed", 
                Name = "Completed", 
			});
			 
            all.Add(new ActivityStatusDetails()
            {    
                Code = "D", 
                SearchFields = "D,Deferred", 
                Name = "Deferred", 
			});
			 
            all.Add(new ActivityStatusDetails()
            {    
                Code = "I", 
                SearchFields = "I,In Progress", 
                Name = "In Progress", 
			});
			 
            all.Add(new ActivityStatusDetails()
            {    
                Code = "N", 
                SearchFields = "N,Not Started", 
                Name = "Not Started", 
			});
			 
            all.Add(new ActivityStatusDetails()
            {    
                Code = "W", 
                SearchFields = "W,Waiting on Someone else", 
                Name = "Waiting on Someone else", 
			});
			 
            all.Add(new ActivityStatusDetails()
            {    
                Code = "X", 
                SearchFields = "X,Canceled", 
                Name = "Canceled", 
			});
			
            return all;
       }

	    public void MapPoco(ActivityStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ActivityStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

