

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
   public class ShipmentTypeDetails : ShipmentType, ICloseTable<ShipmentType, ShipmentTypeDetails>
   {
       public List<ShipmentTypeDetails> GetAll()
       {
		    var all = new List<ShipmentTypeDetails>();  
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "A", 
                Name = "Console", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "A", 
                Name = "Direct", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "O", 
                Name = "FCL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "O", 
                Name = "FCL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "I", 
                Name = "FTL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "O", 
                Name = "LCL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "O", 
                Name = "LCL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "I", 
                Name = "LTL", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "I", 
                Name = "My Groupage Inland", 
			});
			 
            all.Add(new ShipmentTypeDetails()
            {    
                TransportModeId = "O", 
                Name = "My Groupage Ocean", 
			});
			
            return all;
       }

	    public void MapPoco(ShipmentType newPoco)
        {   
		    newPoco.TransportModeId = this.TransportModeId;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ShipmentType rec)
        {   
           return String.Concat(rec.TransportModeId,",",rec.Name,",");
        }
		public string Code
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
   }
}

