
   
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
   public class WarehouseEntryStatusDetails : WarehouseEntryStatus, ICloseTable<WarehouseEntryStatus, WarehouseEntryStatusDetails>
   {
       public List<WarehouseEntryStatusDetails> GetAll()
       {
		    var all = new List<WarehouseEntryStatusDetails>();  
            all.Add(new WarehouseEntryStatusDetails()
            {    
                Code = "CREA", 
                SearchFields = "CREA,Created,", 
                Name = "Created", 
			});
			 
            all.Add(new WarehouseEntryStatusDetails()
            {    
                Code = "ENTE", 
                SearchFields = "ENTE,Entered,", 
                Name = "Entered", 
			});
			
            return all;
       }

	    public void MapPoco(WarehouseEntryStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(WarehouseEntryStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

