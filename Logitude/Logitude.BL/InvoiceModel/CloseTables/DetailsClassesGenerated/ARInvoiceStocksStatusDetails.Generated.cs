

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
   public class ARInvoiceStocksStatusDetails : ARInvoiceStocksStatus, ICloseTable<ARInvoiceStocksStatus, ARInvoiceStocksStatusDetails>
   {
       public List<ARInvoiceStocksStatusDetails> GetAll()
       {
		    var all = new List<ARInvoiceStocksStatusDetails>();  
            all.Add(new ARInvoiceStocksStatusDetails()
            {    
                Code = "N", 
                Name = "New", 
                SearchFields = "N,New", 
			});
			 
            all.Add(new ARInvoiceStocksStatusDetails()
            {    
                Code = "A", 
                Name = "Active", 
                SearchFields = "A,Active", 
			});
			 
            all.Add(new ARInvoiceStocksStatusDetails()
            {    
                Code = "U", 
                Name = "Used", 
                SearchFields = "U,Used", 
			});
			 
            all.Add(new ARInvoiceStocksStatusDetails()
            {    
                Code = "E", 
                Name = "Expired", 
                SearchFields = "E,Expired", 
			});
			 
            all.Add(new ARInvoiceStocksStatusDetails()
            {    
                Code = "C", 
                Name = "Cancelled ", 
                SearchFields = "C,Cancelled ", 
			});
			
            return all;
       }

	    public void MapPoco(ARInvoiceStocksStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ARInvoiceStocksStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

