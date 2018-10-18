
   
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
   public class BookingStatusDetails : BookingStatus, ICloseTable<BookingStatus, BookingStatusDetails>
   {
       public List<BookingStatusDetails> GetAll()
       {
		    var all = new List<BookingStatusDetails>();  
            all.Add(new BookingStatusDetails()
            {    
                Code = "AWB", 
                Name = "Air waybill", 
                SearchFields = "AWB,Air waybill,", 
			});
			 
            all.Add(new BookingStatusDetails()
            {    
                Code = "CNF", 
                Name = "Confirmed", 
                SearchFields = "CNF,Confirmed,", 
			});
			 
            all.Add(new BookingStatusDetails()
            {    
                Code = "CRT", 
                Name = "Created", 
                SearchFields = "CRT,Created,", 
			});
			 
            all.Add(new BookingStatusDetails()
            {    
                Code = "SNT", 
                Name = "Sent", 
                SearchFields = "SNT,Sent", 
			});
			 
            all.Add(new BookingStatusDetails()
            {    
                Code = "WCF", 
                Name = "Waiting for Confirmation", 
                SearchFields = "WCF,Waiting for Confirmation,", 
			});
			
            return all;
       }

	    public void MapPoco(BookingStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(BookingStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

