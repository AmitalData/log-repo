using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AccountingSystemPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public bool IsExternalCodesFromTable { get; set; }
        public bool IsExternalCodesFromAPI { get; set; }
        public bool IsExternalCodesSyncEnabled { get; set; }
        public bool IsSingleTaxPerInvoice { get; set; }
        public bool IsSingleCurrencyAccount { get; set; }
        public bool AllowManuallyDueDate { get; set; }
        public bool IsJournalMode { get; set; }
        public bool IsTaxItemManaged { get; set; }
        public bool ShowDownloadScreen { get; set; }
        public bool AllowARInvoicesTransfer { get; set; }
        public bool AllowAPInvoicesTransfer { get; set; }
        public bool AllowARPaymentsTransfer { get; set; }
        public bool AllowMinusInvoiceLines { get; set; }
        public bool AllowPositiveAmountsCreditNote { get; set; }
        public bool CanTransferToDropbox { get; set; }
        public bool AllowAPPaymentsTransfer { get; set; }
        public bool CanTransferToFTP { get; set; }
    }
}