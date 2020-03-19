

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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel
{
   public class EventTypeCategoryDetails : EventTypeCategory, ICloseTable<EventTypeCategory, EventTypeCategoryDetails>
   {
       public List<EventTypeCategoryDetails> GetAll()
       {
		    var all = new List<EventTypeCategoryDetails>();  
            all.Add(new EventTypeCategoryDetails()
            {    
                Code = "DOC", 
                SearchFields = "DOC,Documents", 
                Name = "Documents", 
			});
			 
            all.Add(new EventTypeCategoryDetails()
            {    
                Code = "LOG", 
                SearchFields = "LOG,Logs", 
                Name = "Logs", 
			});
			 
            all.Add(new EventTypeCategoryDetails()
            {    
                Code = "OPE", 
                SearchFields = "OPE,Operations", 
                Name = "Operations", 
			});
			 
            all.Add(new EventTypeCategoryDetails()
            {    
                Code = "LEG", 
                SearchFields = "LEG,Routings", 
                Name = "Routings", 
			});
			
            return all;
       }

	    public void MapPoco(EventTypeCategory newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(EventTypeCategory rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

