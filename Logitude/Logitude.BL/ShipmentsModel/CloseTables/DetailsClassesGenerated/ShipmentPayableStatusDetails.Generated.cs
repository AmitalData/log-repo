

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
   public class ShipmentPayableStatusDetails : ShipmentPayableStatus, ICloseTable<ShipmentPayableStatus, ShipmentPayableStatusDetails>
   {
       public List<ShipmentPayableStatusDetails> GetAll()
       {
		    var all = new List<ShipmentPayableStatusDetails>();  
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "CLSD,Closed", 
                Code = "CLSD", 
                Name = "Closed", 
			});
			 
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "COST,Costed", 
                Code = "COST", 
                Name = "Costed", 
			});
			 
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "NOPA,No Payables", 
                Code = "NOPA", 
                Name = "No Payables", 
			});
			 
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "OPEN,Open", 
                Code = "OPEN", 
                Name = "Open", 
			});
			 
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "PAID,Paid", 
                Code = "PAID", 
                Name = "Paid", 
			});
			 
            all.Add(new ShipmentPayableStatusDetails()
            {    
                SearchFields = "PRPD,Partially Paid", 
                Code = "PRPD", 
                Name = "Partially Paid", 
			});
			
            return all;
       }

	    public void MapPoco(ShipmentPayableStatus newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ShipmentPayableStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

