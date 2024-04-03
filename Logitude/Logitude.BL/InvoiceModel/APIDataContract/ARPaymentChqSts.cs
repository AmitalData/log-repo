using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract
{
    public class ARPaymentChqSts
    {
        public string PaymentId { get; set; }
        public string PaymentChequeId { get; set; }
        public string ChequeNumber { get; set; }
        public string BankAccount { get; set; }
        public string StatusCode { get; set; }

    }



}
