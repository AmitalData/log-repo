using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract
{
  public  class ARPaymentCancellation
    {
        public string PaymentNo { get; set; }
        public string CancelledbyUser { get; set; }
        public string Remark { get; set; }
    }
}
