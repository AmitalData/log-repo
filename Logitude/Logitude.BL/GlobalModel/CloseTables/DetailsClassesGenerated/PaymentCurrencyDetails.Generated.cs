

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
   public class PaymentCurrencyDetails : PaymentCurrency, ICloseTable<PaymentCurrency, PaymentCurrencyDetails>
   {
       public List<PaymentCurrencyDetails> GetAll()
       {
		    var all = new List<PaymentCurrencyDetails>();  
            all.Add(new PaymentCurrencyDetails()
            {    
                Code = "EUR", 
                SearchFields = "EUR,Euro", 
                Name = "Euro", 
			});
			 
            all.Add(new PaymentCurrencyDetails()
            {    
                Code = "NIS", 
                SearchFields = "NIS,Shekel", 
                Name = "Shekel", 
			});
			 
            all.Add(new PaymentCurrencyDetails()
            {    
                Code = "USD", 
                SearchFields = "USD,United States Of America Dollar", 
                Name = "United States Of America Dollar", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentCurrency newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PaymentCurrency rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

