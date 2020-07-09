
   
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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs; 
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL
{
   public class CargoTrackingHeaderEntityTypeDetails : CargoTrackingHeaderEntityType, ICloseTable<CargoTrackingHeaderEntityType, CargoTrackingHeaderEntityTypeDetails>
   {
       public List<CargoTrackingHeaderEntityTypeDetails> GetAll()
       {
		    var all = new List<CargoTrackingHeaderEntityTypeDetails>();  
            all.Add(new CargoTrackingHeaderEntityTypeDetails()
            {    
                Code = "O", 
                EnglishName = "Order", 
                SearchFields = "O,Order", 
                LocalName = "Order", 
			});
			 
            all.Add(new CargoTrackingHeaderEntityTypeDetails()
            {    
                Code = "F", 
                EnglishName = "Forwarding Shipment", 
                SearchFields = "F,Forwarding Shipment", 
                LocalName = "Forwarding Shipment", 
			});
			 
            all.Add(new CargoTrackingHeaderEntityTypeDetails()
            {    
                Code = "C", 
                EnglishName = "Customs Shipment", 
                SearchFields = "C,Customs Shipment", 
                LocalName = "Customs Shipment", 
			});
			
            return all;
       }

	    public void MapPoco(CargoTrackingHeaderEntityType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CargoTrackingHeaderEntityType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

