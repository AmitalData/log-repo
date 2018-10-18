

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
   public class APInvoiceTransferStatusDetails : APInvoiceTransferStatus, ICloseTable<APInvoiceTransferStatus, APInvoiceTransferStatusDetails>
   {
       public List<APInvoiceTransferStatusDetails> GetAll()
       {
		    var all = new List<APInvoiceTransferStatusDetails>();  
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "bl,blocked", 
                Code = "BL", 
                Name = "Blocked", 
			});
			 
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "et,error in transfer", 
                Code = "ET", 
                Name = "Error In Transfer", 
			});
			 
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "ip,in progress", 
                Code = "IP", 
                Name = "In progress", 
			});
			 
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "nr,not ready", 
                Code = "NR", 
                Name = "Not Ready", 
			});
			 
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "rd,ready", 
                Code = "RD", 
                Name = "Ready", 
			});
			 
            all.Add(new APInvoiceTransferStatusDetails()
            {    
                SearchFields = "tr,transferred", 
                Code = "TR", 
                Name = "Transferred", 
			});
			
            return all;
       }

	    public void MapPoco(APInvoiceTransferStatus newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(APInvoiceTransferStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

