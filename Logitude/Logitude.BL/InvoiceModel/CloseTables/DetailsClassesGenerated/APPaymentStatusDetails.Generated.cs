

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
   public class APPaymentStatusDetails : APPaymentStatus, ICloseTable<APPaymentStatus, APPaymentStatusDetails>
   {
       public List<APPaymentStatusDetails> GetAll()
       {
		    var all = new List<APPaymentStatusDetails>();  
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "AC", 
                SearchFields = "AC,Approval Canceled", 
                Name = "Approval Canceled", 
			});
			 
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "AD", 
                SearchFields = "ad,approved", 
                Name = "Approved", 
			});
			 
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "CL", 
                SearchFields = "cl,closed", 
                Name = "Closed", 
			});
			 
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "DR", 
                SearchFields = "dr,draft", 
                Name = "Draft", 
			});
			 
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "PR", 
                SearchFields = "PR,Printed", 
                Name = "Printed", 
			});
			 
            all.Add(new APPaymentStatusDetails()
            {    
                Code = "VD", 
                SearchFields = "vd,void", 
                Name = "Void", 
			});
			
            return all;
       }

	    public void MapPoco(APPaymentStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(APPaymentStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

