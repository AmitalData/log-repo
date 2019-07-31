using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract
{
   public class ARPaymentInvoice
    {

   
        public double? LocalAmount { get; set; }
        public double? ForeignAmount { get; set; }

        public Currency ForeignCurrency { get; set; }
        public string ARInvoiceNumber { get; set; }
    
    }
}
