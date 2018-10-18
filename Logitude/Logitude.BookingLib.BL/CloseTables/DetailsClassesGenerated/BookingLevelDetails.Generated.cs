
   
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
   public class BookingLevelDetails : BookingLevel, ICloseTable<BookingLevel, BookingLevelDetails>
   {
       public List<BookingLevelDetails> GetAll()
       {
		    var all = new List<BookingLevelDetails>();  
            all.Add(new BookingLevelDetails()
            {    
                Code = "C", 
                Name = "Consol", 
                SearchFields = "C,Consol,", 
			});
			 
            all.Add(new BookingLevelDetails()
            {    
                Code = "D", 
                Name = "Direct", 
                SearchFields = "D,Direct,", 
			});
			
            return all;
       }

	    public void MapPoco(BookingLevel newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(BookingLevel rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

