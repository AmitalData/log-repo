
   
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
   public class OccasionStatusDetails : OccasionStatus, ICloseTable<OccasionStatus, OccasionStatusDetails>
   {
       public List<OccasionStatusDetails> GetAll()
       {
		    var all = new List<OccasionStatusDetails>();  
            all.Add(new OccasionStatusDetails()
            {    
                Code = "PL", 
                Name = "Planned", 
                SearchFields = "PL,Planned", 
			});
			 
            all.Add(new OccasionStatusDetails()
            {    
                Code = "IP", 
                Name = "In Progress", 
                SearchFields = "IP,In Progress", 
			});
			 
            all.Add(new OccasionStatusDetails()
            {    
                Code = "CD", 
                Name = "Closed", 
                SearchFields = "CD,Closed", 
			});
			
            return all;
       }

	    public void MapPoco(OccasionStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(OccasionStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

