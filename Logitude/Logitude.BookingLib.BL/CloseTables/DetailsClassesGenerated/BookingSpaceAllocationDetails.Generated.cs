
   
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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.CLoseTable
{
   public class BookingSpaceAllocationDetails : BookingSpaceAllocation, ICloseTable<BookingSpaceAllocation, BookingSpaceAllocationDetails>
   {
       public List<BookingSpaceAllocationDetails> GetAll()
       {
		    var all = new List<BookingSpaceAllocationDetails>();  
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "CA", 
                SearchFields = "CA,Selling Space Allocation Against Allotment,True,", 
                IsSelectable = true, 
                Name = "Selling Space Allocation Against Allotment", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "CN", 
                SearchFields = "CN,Cancellation Noted,False,", 
                IsSelectable = false, 
                Name = "Cancellation Noted", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "HK", 
                SearchFields = "HK,Holding Confirmed,False,", 
                IsSelectable = false, 
                Name = "Holding Confirmed", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "HL", 
                SearchFields = "HL,Holding Wait List,False,", 
                IsSelectable = false, 
                Name = "Holding Wait List", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "HN", 
                SearchFields = "HN,Have Requested Space Allocation,False,", 
                IsSelectable = false, 
                Name = "Have Requested Space Allocation", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "KK", 
                SearchFields = "KK,Confirming,False,", 
                IsSelectable = false, 
                Name = "Confirming", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "LL", 
                SearchFields = "LL,Wait List,False,", 
                IsSelectable = false, 
                Name = "Wait List", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "NA", 
                SearchFields = "NA,Requesting Space Allocation, if Not Available Will Accept Alternative,False,", 
                IsSelectable = false, 
                Name = "Requesting Space Allocation, if Not Available Will Accept Alternative", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "NN", 
                SearchFields = "NN,Requesting Space Allocation, Will Not Accept Alternative,True,", 
                IsSelectable = true, 
                Name = "Requesting Space Allocation, Will Not Accept Alternative", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "UN", 
                SearchFields = "UN,Unable, Flight Does Not Operate,False,", 
                IsSelectable = false, 
                Name = "Unable, Flight Does Not Operate", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "UU", 
                SearchFields = "UU,Unable,False,", 
                IsSelectable = false, 
                Name = "Unable", 
			});
			 
            all.Add(new BookingSpaceAllocationDetails()
            {    
                Code = "XX", 
                SearchFields = "XX,Cancel Any Previous Space Allocation,False,", 
                IsSelectable = false, 
                Name = "Cancel Any Previous Space Allocation", 
			});
			
            return all;
       }

	    public void MapPoco(BookingSpaceAllocation newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.IsSelectable = this.IsSelectable;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(BookingSpaceAllocation rec)
        {   
           return String.Concat(rec.Code,",",rec.IsSelectable,",",rec.Name,",");
        }
   }
}

