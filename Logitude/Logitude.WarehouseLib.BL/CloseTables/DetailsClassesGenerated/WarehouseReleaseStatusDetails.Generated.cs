
   
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
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs; 
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.CLoseTable
{
   public class WarehouseReleaseStatusDetails : WarehouseReleaseStatus, ICloseTable<WarehouseReleaseStatus, WarehouseReleaseStatusDetails>
   {
       public List<WarehouseReleaseStatusDetails> GetAll()
       {
		    var all = new List<WarehouseReleaseStatusDetails>();  
            all.Add(new WarehouseReleaseStatusDetails()
            {    
                Code = "CARE", 
                SearchFields = "CARE,Cancelled,", 
                Name = "Cancelled", 
			});
			 
            all.Add(new WarehouseReleaseStatusDetails()
            {    
                Code = "CREA", 
                SearchFields = "CREA,Created,", 
                Name = "Created", 
			});
			 
            all.Add(new WarehouseReleaseStatusDetails()
            {    
                Code = "RELE", 
                SearchFields = "RELE,Released,", 
                Name = "Released", 
			});
			
            return all;
       }

	    public void MapPoco(WarehouseReleaseStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(WarehouseReleaseStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

