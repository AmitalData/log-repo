

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
   public class INTTRABookingTransStatusDetails : INTTRABookingTransStatus, ICloseTable<INTTRABookingTransStatus, INTTRABookingTransStatusDetails>
   {
       public List<INTTRABookingTransStatusDetails> GetAll()
       {
		    var all = new List<INTTRABookingTransStatusDetails>();  
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Not Sent", 
                Code = "NST", 
                SearchFields = "NST,Not Sent", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Booking Request Sent", 
                Code = "BRS", 
                SearchFields = "BRS,Booking Request Sent", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Booking Confirmed", 
                Code = "BCD", 
                SearchFields = "BCD,Booking Confirmed", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Booking Request Rejected", 
                Code = "BRR", 
                SearchFields = "BRR,Booking Request Rejected", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Update Request Sent", 
                Code = "URS", 
                SearchFields = "URS,Update Request Sent", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Update Request Rejected", 
                Code = "URR", 
                SearchFields = "URR,Update Request Rejected", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Update Request Confirmed", 
                Code = "URC", 
                SearchFields = "URC,Update Request Confirmed", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Cancellation Request Sent", 
                Code = "CRS", 
                SearchFields = "CRS,Cancellation Request Sent", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Cancellation Confirmed", 
                Code = "CCD", 
                SearchFields = "CCD,Cancellation Confirmed", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Cancellation Request Rejected", 
                Code = "CRR", 
                SearchFields = "CRR,Cancellation Request Rejected", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Accepted by INTTRA ", 
                Code = "ACI", 
                SearchFields = "ACI,Accepted by INTTRA ", 
			});
			 
            all.Add(new INTTRABookingTransStatusDetails()
            {    
                Name = "Rejected by INTTRA", 
                Code = "RBI", 
                SearchFields = "RBI,Rejected by INTTRA", 
			});
			
            return all;
       }

	    public void MapPoco(INTTRABookingTransStatus newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(INTTRABookingTransStatus rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

