

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
   public class PaymentChannelDetails : PaymentChannel, ICloseTable<PaymentChannel, PaymentChannelDetails>
   {
       public List<PaymentChannelDetails> GetAll()
       {
		    var all = new List<PaymentChannelDetails>();  
            all.Add(new PaymentChannelDetails()
            {    
                Code = "PL", 
                SearchFields = "PL,Bluesnap", 
                Name = "Bluesnap", 
			});
			 
            all.Add(new PaymentChannelDetails()
            {    
                Code = "DI", 
                SearchFields = "DI,Direct", 
                Name = "Direct", 
			});
			
            return all;
       }

	    public void MapPoco(PaymentChannel newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PaymentChannel rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

