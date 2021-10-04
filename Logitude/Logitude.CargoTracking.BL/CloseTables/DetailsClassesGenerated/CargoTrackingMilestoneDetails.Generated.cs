
   
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
                EnglishName = "Created", 
                SearchFields = "1,Created", 
                LocalName = "Created", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "2", 
                EnglishName = "Booking", 
                SearchFields = "2,Booking", 
                LocalName = "Booking", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "3", 
                EnglishName = "Pickup", 
                SearchFields = "3,Pickup", 
                LocalName = "Pickup", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "4", 
                EnglishName = "From Warehouse", 
                SearchFields = "4,From Warehouse", 
                LocalName = "From Warehouse", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "5", 
                EnglishName = "Departure", 
                SearchFields = "5,Departure", 
                LocalName = "Departure", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "6", 
                EnglishName = "Arrival", 
                SearchFields = "6,Arrival", 
                LocalName = "Arrival", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "7", 
                EnglishName = "To Warehouse", 
                SearchFields = "7,To Warehouse", 
                LocalName = "To Warehouse", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "8", 
                EnglishName = "Assigned To Customs Broker", 
                SearchFields = "8,Assigned To Customs Broker", 
                LocalName = "Assigned To Customs Broker", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "9", 
                EnglishName = "Customs Process", 
                SearchFields = "9,Customs Process", 
                LocalName = "Customs Process", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "10", 
                EnglishName = "Goods Classification", 
                SearchFields = "10,Goods Classification", 
                LocalName = "Goods Classification", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "11", 
                EnglishName = "Document Inspection", 
                SearchFields = "11,Document Inspection", 
                LocalName = "Document Inspection", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "12", 
                EnglishName = "Payment Requested", 
                SearchFields = "12,Payment Requested", 
                LocalName = "Payment Requested", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "13", 
                EnglishName = "Payment Received", 
                SearchFields = "13,Payment Received", 
                LocalName = "Payment Received", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "14", 
                EnglishName = "Customs Payment", 
                SearchFields = "14,Customs Payment", 
                LocalName = "Customs Payment", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "15", 
                EnglishName = "Clearance", 
                SearchFields = "15,Clearance", 
                LocalName = "Clearance", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "16", 
                EnglishName = "Gatepass Arrived", 
                SearchFields = "16,Gatepass Arrived", 
                LocalName = "Gatepass Arrived", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "17", 
                EnglishName = "Assigned to Trucker", 
                SearchFields = "17,Assigned to Trucker", 
                LocalName = "Assigned to Trucker", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "18", 
                EnglishName = "Delivery Out", 
                SearchFields = "18,Delivery Out", 
                LocalName = "Delivery Out", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "19", 
                EnglishName = "Delivered", 
                SearchFields = "19,Delivered", 
                LocalName = "Delivered", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "20", 
                EnglishName = "Invoiced", 
                SearchFields = "20,Invoiced", 
                LocalName = "Invoiced", 
                Inactive = false, 
			});
			
            return all;
       }

	    public void MapPoco(CargoTrackingMilestone newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;  
		    newPoco.Inactive = this.Inactive;   
        }

		public string GetSearchFields(CargoTrackingMilestone rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",",rec.Inactive,",");
        }
   }
}

