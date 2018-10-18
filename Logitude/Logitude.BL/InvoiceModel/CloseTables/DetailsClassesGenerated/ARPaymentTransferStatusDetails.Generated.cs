

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
   public class ARPaymentTransferStatusDetails : ARPaymentTransferStatus, ICloseTable<ARPaymentTransferStatus, ARPaymentTransferStatusDetails>
   {
       public List<ARPaymentTransferStatusDetails> GetAll()
       {
		    var all = new List<ARPaymentTransferStatusDetails>();  
            all.Add(new ARPaymentTransferStatusDetails()
            {    
                Code = "BL", 
                SearchFields = "BL,Blocked", 
                Name = "Blocked", 
			});
			 
            all.Add(new ARPaymentTransferStatusDetails()
            {    
                Code = "ET", 
                SearchFields = "ET,Error In Transfer", 
                Name = "Error In Transfer", 
			});
			 
            all.Add(new ARPaymentTransferStatusDetails()
            {    
                Code = "NR", 
                SearchFields = "NR,Not Ready", 
                Name = "Not Ready", 
			});
			 
            all.Add(new ARPaymentTransferStatusDetails()
            {    
                Code = "RD", 
                SearchFields = "RD,Ready", 
                Name = "Ready", 
			});
			 
            all.Add(new ARPaymentTransferStatusDetails()
            {    
                Code = "TR", 
                SearchFields = "TR,Transferred", 
                Name = "Transferred", 
			});
			
            return all;
       }

	    public void MapPoco(ARPaymentTransferStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ARPaymentTransferStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

