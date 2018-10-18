

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
   public class ARInvoiceStatusDetails : ARInvoiceStatus, ICloseTable<ARInvoiceStatus, ARInvoiceStatusDetails>
   {
       public List<ARInvoiceStatusDetails> GetAll()
       {
		    var all = new List<ARInvoiceStatusDetails>();  
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "AC,Auto Credit", 
                Code = "AC", 
                Name = "Auto Credit", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "AD,Unpaid", 
                Code = "AD", 
                Name = "Unpaid", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "AR,Auto Credited", 
                Code = "AR", 
                Name = "Auto Credited", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "CN,Connected", 
                Code = "CN", 
                Name = "Connected", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "DR,Draft", 
                Code = "DR", 
                Name = "Draft", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "LL,Cancelled", 
                Code = "LL", 
                Name = "Cancelled", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "NT,Not Connected", 
                Code = "NT", 
                Name = "Not Connected", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "PD,Paid", 
                Code = "PD", 
                Name = "Paid", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "PP,Partially Paid", 
                Code = "PP", 
                Name = "Partially Paid", 
			});
			 
            all.Add(new ARInvoiceStatusDetails()
            {    
                SearchFields = "VD,Void", 
                Code = "VD", 
                Name = "Void", 
			});
			
            return all;
       }

	    public void MapPoco(ARInvoiceStatus newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ARInvoiceStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

