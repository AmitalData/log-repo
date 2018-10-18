
   
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
   public class TimeUnitDetails : TimeUnit, ICloseTable<TimeUnit, TimeUnitDetails>
   {
       public List<TimeUnitDetails> GetAll()
       {
		    var all = new List<TimeUnitDetails>();  
            all.Add(new TimeUnitDetails()
            {    
                Code = "II", 
                SearchFields = "II,Minutes,", 
                Name = "Minutes", 
			});
			 
            all.Add(new TimeUnitDetails()
            {    
                Code = "OO", 
                SearchFields = "OO,Hours,", 
                Name = "Hours", 
			});
			
            return all;
       }

	    public void MapPoco(TimeUnit newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TimeUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

