
   
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
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "2", 
                EnglishName = "Booking", 
                SearchFields = "2,Booking,בוצעה הזמנה", 
                LocalName = "בוצעה הזמנה", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "3", 
                EnglishName = "Pickup", 
                SearchFields = "3,Pickup,נאסף מהספק", 
                LocalName = "נאסף מהספק", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "4", 
                EnglishName = "Origin Warehouse", 
                SearchFields = "4,Origin Warehouse, אחסנה בנמל מוצא", 
                LocalName = " אחסנה בנמל מוצא", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "5", 
                EnglishName = "Departure", 
                SearchFields = "5,Departure,המראה", 
                LocalName = "המראה", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "6", 
                EnglishName = "Arrival", 
                SearchFields = "6,Arrival,הגעה", 
                LocalName = "הגעה", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "7", 
                EnglishName = "Destination Warehouse", 
                SearchFields = "7,Destination Warehouse,אחסנה בנמל יעד", 
                LocalName = "אחסנה בנמל יעד", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "8", 
                EnglishName = "Assigned To Customs Broker", 
                SearchFields = "8,Assigned To Customs Broker,הועבר לסוכן מכס", 
                LocalName = "הועבר לסוכן מכס", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "9", 
                EnglishName = "Customs Process", 
                SearchFields = "9,Customs Process,בתהליך מכס", 
                LocalName = "בתהליך מכס", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "10", 
                EnglishName = "Goods Classification", 
                SearchFields = "10,Goods Classification,סיווג", 
                LocalName = "סיווג", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "11", 
                EnglishName = "Document Inspection", 
                SearchFields = "11,Document Inspection,ביקורת מסמכים", 
                LocalName = "ביקורת מסמכים", 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "12", 
                EnglishName = "Payment Requested", 
                SearchFields = "12,Payment Requested,נשלחה בקשת תשלום", 
                LocalName = "נשלחה בקשת תשלום", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "13", 
                EnglishName = "Payment Received", 
                SearchFields = "13,Payment Received,התקבל תשלום", 
                LocalName = "התקבל תשלום", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "14", 
                EnglishName = "Customs Payment", 
                SearchFields = "14,Customs Payment,שולם למכס", 
                LocalName = "שולם למכס", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "15", 
                EnglishName = "Clearance", 
                SearchFields = "15,Clearance,התרת מכס", 
                LocalName = "התרת מכס", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "16", 
                EnglishName = "Gatepass Arrived", 
                SearchFields = "16,Gatepass Arrived,גייטפס מוכן", 
                LocalName = "גייטפס מוכן", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "17", 
                EnglishName = "Assigned to Trucker", 
                SearchFields = "17,Assigned to Trucker,נמסר למוביל", 
                LocalName = "נמסר למוביל", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "18", 
                EnglishName = "Delivery on the way", 
                SearchFields = "18,Delivery on the way,יצא להפצה", 
                LocalName = "יצא להפצה", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "19", 
                EnglishName = "Delivered", 
                SearchFields = "19,Delivered,נמסר ללקוח", 
                LocalName = "נמסר ללקוח", 
                Inactive = false, 
			});
			 
            all.Add(new CargoTrackingMilestoneDetails()
            {    
                Code = "20", 
                EnglishName = "Invoiced", 
                SearchFields = "20,Invoiced,הופקה חשבונית", 
                LocalName = "הופקה חשבונית", 
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

