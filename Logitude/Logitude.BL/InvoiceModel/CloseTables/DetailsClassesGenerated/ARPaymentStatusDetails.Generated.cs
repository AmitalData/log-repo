

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
   public class ARPaymentStatusDetails : ARPaymentStatus, ICloseTable<ARPaymentStatus, ARPaymentStatusDetails>
   {
       public List<ARPaymentStatusDetails> GetAll()
       {
		    var all = new List<ARPaymentStatusDetails>();  
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "AC", 
                SearchFields = "AC,Approval Canceled", 
                Name = "Approval Canceled", 
			});
			 
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "AD", 
                SearchFields = "ad,approved", 
                Name = "Approved", 
			});
			 
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "CL", 
                SearchFields = "cl,closed", 
                Name = "Closed", 
			});
			 
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "DR", 
                SearchFields = "dr,draft", 
                Name = "Draft", 
			});
			 
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "PR", 
                SearchFields = "PR,Printed", 
                Name = "Printed", 
			});
			 
            all.Add(new ARPaymentStatusDetails()
            {    
                Code = "VD", 
                SearchFields = "vd,void", 
                Name = "Void", 
			});
			
            return all;
       }

	    public void MapPoco(ARPaymentStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ARPaymentStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

