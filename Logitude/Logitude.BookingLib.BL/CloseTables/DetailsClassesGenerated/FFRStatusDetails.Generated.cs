
   
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
   public class FFRStatusDetails : FFRStatus, ICloseTable<FFRStatus, FFRStatusDetails>
   {
       public List<FFRStatusDetails> GetAll()
       {
		    var all = new List<FFRStatusDetails>();  
            all.Add(new FFRStatusDetails()
            {    
                Code = "BCN", 
                Name = "Booking Confirmed", 
                SearchFields = "BCN,Booking Confirmed,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "BRQ", 
                Name = "Booking Request", 
                SearchFields = "BRQ,Booking Request,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "BRR", 
                Name = "Booking Request Rejected", 
                SearchFields = "BRR,Booking Request Rejected,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CAA", 
                Name = "Cancellation Accepted", 
                SearchFields = "CAA,Cancellation Accepted,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CRR", 
                Name = "Cancellation Request Rejected", 
                SearchFields = "CRR,Cancellation Request Rejected,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CRS", 
                Name = "Cancellation Request Sent", 
                SearchFields = "CRS,Cancellation Request Sent,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CNM", 
                Name = "Cancelled Manually", 
                SearchFields = "CNM,Cancelled Manually,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CNF", 
                Name = "Confirmed", 
                SearchFields = "CNF,Confirmed", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "CFM", 
                Name = "Confirmed Manually", 
                SearchFields = "CFM,Confirmed Manually,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "FMA", 
                Name = "FMA", 
                SearchFields = "FMA,FMA", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "FNA", 
                Name = "FNA", 
                SearchFields = "FNA,FNA", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "NST", 
                Name = "Not Sent", 
                SearchFields = "NST,Not Sent,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "PAR", 
                Name = "Partially confirmed", 
                SearchFields = "PAR,Partially confirmed", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "SNT", 
                Name = "Sent", 
                SearchFields = "SNT,Sent", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "RBC", 
                Name = "Waiting for Airline Cancellation", 
                SearchFields = "RBC,Waiting for Airline Cancellation,", 
			});
			 
            all.Add(new FFRStatusDetails()
            {    
                Code = "RBA", 
                Name = "Waiting for Airline Confirmation", 
                SearchFields = "RBA,Waiting for Airline Confirmation,", 
			});
			
            return all;
       }

	    public void MapPoco(FFRStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(FFRStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

