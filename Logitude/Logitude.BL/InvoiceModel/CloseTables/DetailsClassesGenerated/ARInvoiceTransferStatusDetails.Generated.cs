

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
   public class ARInvoiceTransferStatusDetails : ARInvoiceTransferStatus, ICloseTable<ARInvoiceTransferStatus, ARInvoiceTransferStatusDetails>
   {
       public List<ARInvoiceTransferStatusDetails> GetAll()
       {
		    var all = new List<ARInvoiceTransferStatusDetails>();  
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "bl,blocked", 
                Code = "BL", 
                Name = "Blocked", 
			});
			 
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "et,error in transfer", 
                Code = "ET", 
                Name = "Error In Transfer", 
			});
			 
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "ip,in progress", 
                Code = "IP", 
                Name = "In progress", 
			});
			 
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "nr,not ready", 
                Code = "NR", 
                Name = "Not Ready", 
			});
			 
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "rd,ready", 
                Code = "RD", 
                Name = "Ready", 
			});
			 
            all.Add(new ARInvoiceTransferStatusDetails()
            {    
                SearchFields = "tr,transferred", 
                Code = "TR", 
                Name = "Transferred", 
			});
			
            return all;
       }

	    public void MapPoco(ARInvoiceTransferStatus newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ARInvoiceTransferStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

