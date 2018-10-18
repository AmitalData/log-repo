

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
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs; 
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel
{
   public class APInvoiceTypeDetails : APInvoiceType, ICloseTable<APInvoiceType, APInvoiceTypeDetails>
   {
       public List<APInvoiceTypeDetails> GetAll()
       {
		    var all = new List<APInvoiceTypeDetails>();  
            all.Add(new APInvoiceTypeDetails()
            {    
                Code = "CD", 
                SearchFields = "cd,credit", 
                Name = "Credit", 
			});
			 
            all.Add(new APInvoiceTypeDetails()
            {    
                Code = "IN", 
                SearchFields = "in,invoice", 
                Name = "Invoice", 
			});
			
            return all;
       }

	    public void MapPoco(APInvoiceType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(APInvoiceType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

