

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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs; 
using Simplog.Global.Data.GlobalModel;

namespace Logitude.BL.GlobalModel
{
   public class PaymentMethodDetails : PaymentMethod, ICloseTable<PaymentMethod, PaymentMethodDetails>
   {
       public List<PaymentMethodDetails> GetAll()
       {
		    var all = new List<PaymentMethodDetails>();  
            all.Add(new PaymentMethodDetails()
            {    
                Code = "BT", 
                SearchFields = "BT,Bank Transfer", 
                Name = "Bank Transfer", 
			});
			 
            all.Add(new PaymentMethodDetails()
            {    
                Code = "CH", 
                SearchFields = "CH,Cheque", 
                Name = "Cheque", 
			});
			 
            all.Add(new PaymentMethodDetails()
            {    
                Code = "CC", 
                SearchFields = "CC,Credit Card", 
                Name = "Credit Card", 
			});
			 
            all.Add(new PaymentMethodDetails()
            {    
                Code = "PP", 
                SearchFields = "PP,Pay Pal", 
                Name = "Pay Pal", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentMethod newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PaymentMethod rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

