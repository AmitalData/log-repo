using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AccountingTransferHeaderPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransferNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TransferDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FileName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingTransferTypeCode { get; set; }

        public string AccountingTransferTypeName { get; set; }

        public string SearchFields { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UserId { get; set; }

        public string UserName { get; set; }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        List<AccountingTransferLinePM> transferLines;
        [Include]
        [Composition]
        [Association("TransferHeaderTransferLines", "Id", "AccountingTransferHeaderId")]
        public virtual List<AccountingTransferLinePM> TransferLines
        {
            get
            {
                if (transferLines == null)
                {
                    transferLines = new List<AccountingTransferLinePM>();
                }

                return this.transferLines;
            }

            set
            {
                if (value != null)
                {
                    transferLines = value;
                }
            }
        }

    }
}