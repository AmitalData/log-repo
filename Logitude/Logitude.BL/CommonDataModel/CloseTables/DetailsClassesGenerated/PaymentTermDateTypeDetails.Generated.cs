

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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class PaymentTermDateTypeDetails : PaymentTermDateType, ICloseTable<PaymentTermDateType, PaymentTermDateTypeDetails>
   {
       public List<PaymentTermDateTypeDetails> GetAll()
       {
		    var all = new List<PaymentTermDateTypeDetails>();  
            all.Add(new PaymentTermDateTypeDetails()
            {    
                Code = "INV", 
                SearchFields = "INV, Invoice Date", 
                Name = "Invoice Date", 
			});
			 
            all.Add(new PaymentTermDateTypeDetails()
            {    
                Code = "SHI", 
                SearchFields = "SHI, Shipment Date", 
                Name = "Shipment Date", 
			});
			 
            all.Add(new PaymentTermDateTypeDetails()
            {    
                Code = "OPR", 
                Name = "Operational Date", 
                SearchFields = "OPR, Operational Date", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentTermDateType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PaymentTermDateType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

