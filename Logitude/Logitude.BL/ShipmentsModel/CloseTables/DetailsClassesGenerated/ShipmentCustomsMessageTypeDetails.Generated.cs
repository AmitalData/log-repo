

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
   public class ShipmentCustomsMessageTypeDetails : ShipmentCustomsMessageType, ICloseTable<ShipmentCustomsMessageType, ShipmentCustomsMessageTypeDetails>
   {
       public List<ShipmentCustomsMessageTypeDetails> GetAll()
       {
		    var all = new List<ShipmentCustomsMessageTypeDetails>();  
            all.Add(new ShipmentCustomsMessageTypeDetails()
            {    
                Code = "ARBL", 
                SearchFields = "ARBL,Artemus Bill of Lading", 
                Name = "Artemus Bill of Lading", 
			});
			 
            all.Add(new ShipmentCustomsMessageTypeDetails()
            {    
                Code = "ASVO", 
                SearchFields = "ASVO,Artemus Voyage", 
                Name = "Artemus Voyage", 
			});
			 
            all.Add(new ShipmentCustomsMessageTypeDetails()
            {    
                Code = "CBAS", 
                SearchFields = "CBAS,CBP AES", 
                Name = "CBP AES", 
			});
			
            return all;
       }

	    public void MapPoco(ShipmentCustomsMessageType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ShipmentCustomsMessageType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

