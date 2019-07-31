
   
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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.CLoseTable
{
   public class TMDayOffTypeDetails : TMDayOffType, ICloseTable<TMDayOffType, TMDayOffTypeDetails>
   {
       public List<TMDayOffTypeDetails> GetAll()
       {
		    var all = new List<TMDayOffTypeDetails>();  
            all.Add(new TMDayOffTypeDetails()
            {    
                Name = "Vacation", 
                Code = "V", 
                SearchFields = "V,Vacation", 
			});
			 
            all.Add(new TMDayOffTypeDetails()
            {    
                Name = "Holiday", 
                Code = "H", 
                SearchFields = "H,Holiday", 
			});
			 
            all.Add(new TMDayOffTypeDetails()
            {    
                Name = "Sickness ", 
                Code = "S", 
                SearchFields = "S,Sickness ", 
			});
			
            return all;
       }

	    public void MapPoco(TMDayOffType newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TMDayOffType rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

