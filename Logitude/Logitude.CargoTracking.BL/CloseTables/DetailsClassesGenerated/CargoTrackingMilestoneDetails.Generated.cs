
   
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
   public class CargoTrackingMilestoneDetails : CargoTrackingMilestone, ICloseTable<CargoTrackingMilestone, CargoTrackingMilestoneDetails>
   {
       public List<CargoTrackingMilestoneDetails> GetAll()
       {
		    var all = new List<CargoTrackingMilestoneDetails>();  
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "1", 
                EnglishName = "Booking", 
                SearchFields = "1,Booking", 
                LocalName = "Booking", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "2", 
                EnglishName = "Pickup", 
                SearchFields = "2,Pickup", 
                LocalName = "Pickup", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "3", 
                EnglishName = "From Warehouse", 
                SearchFields = "3,From Warehouse", 
                LocalName = "From Warehouse", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "6", 
                EnglishName = "To Warehouse", 
                SearchFields = "6,To Warehouse", 
                LocalName = "To Warehouse", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "7", 
                EnglishName = "Customs Process", 
                SearchFields = "7,Customs Process", 
                LocalName = "Customs Process", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "8", 
                EnglishName = "Customs Payment", 
                SearchFields = "8,Customs Payment", 
                LocalName = "Customs Payment", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "9", 
                EnglishName = "Clearance", 
                SearchFields = "9,Clearance", 
                LocalName = "Clearance", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "10", 
                EnglishName = "Assigned to Trucker", 
                SearchFields = "10,Assigned to Trucker", 
                LocalName = "Assigned to Trucker", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "5", 
                EnglishName = "Arrival", 
                SearchFields = "5,Arrival", 
                LocalName = "Arrival", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "4", 
                EnglishName = "Departure", 
                SearchFields = "4,Departure", 
                LocalName = "Departure", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "11", 
                EnglishName = "Delivered", 
                SearchFields = "11,Delivered", 
                LocalName = "Delivered", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "12", 
                EnglishName = "Invoiced", 
                SearchFields = "2,Invoiced", 
                LocalName = "Invoiced", 
			});
			
            return all;
       }

	    public void MapPoco(CargoTrackingMilestone newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CargoTrackingMilestone rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

