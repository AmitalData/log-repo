

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
   public class PickUpDeliveryTransportModeDetails : PickUpDeliveryTransportMode, ICloseTable<PickUpDeliveryTransportMode, PickUpDeliveryTransportModeDetails>
   {
       public List<PickUpDeliveryTransportModeDetails> GetAll()
       {
		    var all = new List<PickUpDeliveryTransportModeDetails>();  
            all.Add(new PickUpDeliveryTransportModeDetails()
            {    
                Code = "BYRA", 
                SearchFields = "BYRA,By Rail", 
                Name = "By Rail", 
			});
			 
            all.Add(new PickUpDeliveryTransportModeDetails()
            {    
                Code = "BYTR", 
                SearchFields = "BYTR,By Truck", 
                Name = "By Truck", 
			});
			
            return all;
       }

	    public void MapPoco(PickUpDeliveryTransportMode newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PickUpDeliveryTransportMode rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

