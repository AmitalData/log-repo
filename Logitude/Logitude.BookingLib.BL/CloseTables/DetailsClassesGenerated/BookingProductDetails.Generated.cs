

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
   public class BookingProductDetails : BookingProduct, ICloseTable<BookingProduct, BookingProductDetails>
   {
       public List<BookingProductDetails> GetAll()
       {
		    var all = new List<BookingProductDetails>(); 
            return all;
       }

	    public void MapPoco(BookingProduct newPoco)
        {    
        }

		public string GetSearchFields(BookingProduct rec)
        {   
           return string.Empty;
        }
   }
}

