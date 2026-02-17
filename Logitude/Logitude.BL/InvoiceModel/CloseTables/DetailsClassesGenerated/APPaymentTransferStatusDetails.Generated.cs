

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
   public class APPaymentTransferStatusDetails : APPaymentTransferStatus, ICloseTable<APPaymentTransferStatus, APPaymentTransferStatusDetails>
   {
       public List<APPaymentTransferStatusDetails> GetAll()
       {
		    var all = new List<APPaymentTransferStatusDetails>(); 
            return all;
       }

	    public void MapPoco(APPaymentTransferStatus newPoco)
        {    
        }

		public string GetSearchFields(APPaymentTransferStatus rec)
        {   
           return string.Empty;
        }
   }
}

