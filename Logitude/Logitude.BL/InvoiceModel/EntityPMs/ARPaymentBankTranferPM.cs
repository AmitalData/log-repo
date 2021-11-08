using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ARPaymentBankTranferPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string SearchFields { get; set; }

        public string PaymentId { get; set; }


        public int LineNumber { get; set; }
        public string PaymentRef { get; set; }
        public DateTime ValueDate { get; set; }
        public string BankAccountId { get; set; }
        public string CurrencyId { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal ExchageRate { get; set; }
        public BankAccountLightPM BankAccount { get; set; }
    }

    public class BankAccountLightPM
    {
        public string Id { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
    }
}
