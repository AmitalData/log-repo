

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
   public class PaymentGatewayPartnerDetails : PaymentGatewayPartner, ICloseTable<PaymentGatewayPartner, PaymentGatewayPartnerDetails>
   {
       public List<PaymentGatewayPartnerDetails> GetAll()
       {
		    var all = new List<PaymentGatewayPartnerDetails>();  
            all.Add(new PaymentGatewayPartnerDetails()
            {    
                Code = "Tranzila", 
                Name = "Tranzila", 
                SearchFields = "Tranzila,Tranzila", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentGatewayPartner newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(PaymentGatewayPartner rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

