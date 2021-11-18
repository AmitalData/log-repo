
   
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
                SearchFields = "1,Created,משלוח נוצר", 
                LocalName = "משלוח נוצר", 
                Inactive = false, 
                Weight = 10, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "2", 
                EnglishName = "Booking", 
                SearchFields = "2,Booking,בוצעה הזמנה", 
                LocalName = "בוצעה הזמנה", 
                Weight = 20, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "3", 
                EnglishName = "Pickup", 
                SearchFields = "3,Pickup,נאסף מהספק", 
                LocalName = "נאסף מהספק", 
                Weight = 30, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "4", 
                EnglishName = "Origin Warehouse", 
                SearchFields = "4,Origin Warehouse, אחסנה בנמל מוצא", 
                LocalName = " אחסנה בנמל מוצא", 
                Inactive = false, 
                Weight = 40, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "5", 
                EnglishName = "Departure", 
                SearchFields = "5,Departure,המראה", 
                LocalName = "המראה", 
                Weight = 50, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "6", 
                EnglishName = "Arrival", 
                SearchFields = "6,Arrival,הגעה", 
                LocalName = "הגעה", 
                Weight = 60, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "7", 
                EnglishName = "Destination Warehouse", 
                SearchFields = "7,Destination Warehouse,אחסנה בנמל יעד", 
                LocalName = "אחסנה בנמל יעד", 
                Weight = 70, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "8", 
                EnglishName = "Assigned To Customs Broker", 
                SearchFields = "8,Assigned To Customs Broker,הועבר לסוכן מכס", 
                LocalName = "הועבר לסוכן מכס", 
                Inactive = false, 
                Weight = 80, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "9", 
                EnglishName = "Customs Process", 
                SearchFields = "9,Customs Process,בתהליך מכס", 
                LocalName = "בתהליך מכס", 
                Weight = 90, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "10", 
                EnglishName = "Goods Classification", 
                SearchFields = "10,Goods Classification,סיווג", 
                LocalName = "סיווג", 
                Weight = 100, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "11", 
                EnglishName = "Document Inspection", 
                SearchFields = "11,Document Inspection,ביקורת מסמכים", 
                LocalName = "ביקורת מסמכים", 
                Weight = 110, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "12", 
                EnglishName = "Payment Requested", 
                SearchFields = "12,Payment Requested,נשלחה בקשת תשלום", 
                LocalName = "נשלחה בקשת תשלום", 
                Inactive = false, 
                Weight = 120, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "13", 
                EnglishName = "Payment Received", 
                SearchFields = "13,Payment Received,התקבל תשלום", 
                LocalName = "התקבל תשלום", 
                Inactive = false, 
                Weight = 130, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "14", 
                EnglishName = "Customs Payment", 
                SearchFields = "14,Customs Payment,שולם למכס", 
                LocalName = "שולם למכס", 
                Inactive = false, 
                Weight = 140, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "15", 
                EnglishName = "Clearance", 
                SearchFields = "15,Clearance,התרת מכס", 
                LocalName = "התרת מכס", 
                Inactive = false, 
                Weight = 150, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "16", 
                EnglishName = "Gatepass Arrived", 
                SearchFields = "16,Gatepass Arrived,גייטפס מוכן", 
                LocalName = "גייטפס מוכן", 
                Inactive = false, 
                Weight = 160, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "17", 
                EnglishName = "Assigned to Trucker", 
                SearchFields = "17,Assigned to Trucker,נמסר למוביל", 
                LocalName = "נמסר למוביל", 
                Inactive = false, 
                Weight = 170, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "18", 
                EnglishName = "Delivery on the way", 
                SearchFields = "18,Delivery on the way,יצא להפצה", 
                LocalName = "יצא להפצה", 
                Inactive = false, 
                Weight = 180, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "19", 
                EnglishName = "Delivered", 
                SearchFields = "19,Delivered,נמסר ללקוח", 
                LocalName = "נמסר ללקוח", 
                Inactive = false, 
                Weight = 190, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "20", 
                EnglishName = "Invoiced", 
                SearchFields = "20,Invoiced,הופקה חשבונית", 
                LocalName = "הופקה חשבונית", 
                Inactive = false, 
                Weight = 200, 
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
		    newPoco.Weight = this.Weight;   
        }

		public string GetSearchFields(CargoTrackingMilestone rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",",rec.Inactive,",",rec.Weight,",");
        }
   }
}

