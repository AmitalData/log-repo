

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
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs; 
using Simplog.Data.ShipmentsModel;

namespace Logitude.BL.ShipmentsModel
{
   public class ShipmentReceivableStatusDetails : ShipmentReceivableStatus, ICloseTable<ShipmentReceivableStatus, ShipmentReceivableStatusDetails>
   {
       public List<ShipmentReceivableStatusDetails> GetAll()
       {
		    var all = new List<ShipmentReceivableStatusDetails>();  
            all.Add(new ShipmentReceivableStatusDetails()
            {    
                SearchFields = "CLSD,Closed", 
                Code = "CLSD", 
                Name = "Closed", 
			});
			 
            all.Add(new ShipmentReceivableStatusDetails()
            {    
                SearchFields = "NORE,No Receivables", 
                Code = "NORE", 
                Name = "No Receivables", 
			});
			 
            all.Add(new ShipmentReceivableStatusDetails()
            {    
                SearchFields = "OPEN,Open", 
                Code = "OPEN", 
                Name = "Open", 
			});
			 
            all.Add(new ShipmentReceivableStatusDetails()
            {    
                SearchFields = "PRIN,Partially Invoiced", 
                Code = "PRIN", 
                Name = "Partially Invoiced", 
			});
			
            return all;
       }

	    public void MapPoco(ShipmentReceivableStatus newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ShipmentReceivableStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

