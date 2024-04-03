using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools
{
    public class FullAccountingARPaymentValidatorArguments
    {
        public List<ARPaymentChequeReplicaPM> ChequeReplicas { get; set; }
        public List<ARPaymentBankTranferPM> bankTransfers { get; set; }
        public int Tenant { get; set; }
        public string BillToId { get; set; }
        public string PaymentCurrencyId { get; set; }
        public CashBookPM CashBook { get; set; }
        public string PaymentMethodCode { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string BankAccountId { get; set; }
        public bool IsOut { get; set; }
        public bool IsNewEntity { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }
        public string Bank { get; set; }
        public bool IsFromReconcileScreen { get; set; }
        public bool IsExternalEntity { get; set; }

    }
}
