

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
   public class SATInvoiceStatusDetails : SATInvoiceStatus, ICloseTable<SATInvoiceStatus, SATInvoiceStatusDetails>
   {
       public List<SATInvoiceStatusDetails> GetAll()
       {
		    var all = new List<SATInvoiceStatusDetails>();  
            all.Add(new SATInvoiceStatusDetails()
            {    
                Code = "NO", 
                SearchFields = "NO,Not Opened in SAT", 
                Name = "Not Opened in SAT", 
			});
			 
            all.Add(new SATInvoiceStatusDetails()
            {    
                Code = "OP", 
                SearchFields = "OP,Opened in SAT", 
                Name = "Opened in SAT", 
			});
			 
            all.Add(new SATInvoiceStatusDetails()
            {    
                Code = "PD", 
                SearchFields = "PD,Paid in SAT", 
                Name = "Paid in SAT", 
			});
			 
            all.Add(new SATInvoiceStatusDetails()
            {    
                Code = "PP", 
                SearchFields = "PP,Partially Paid in SAT", 
                Name = "Partially Paid in SAT", 
			});
			
            return all;
       }

	    public void MapPoco(SATInvoiceStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(SATInvoiceStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

