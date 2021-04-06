using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AccountingSettingMapping
    {
        public static void MapEntity(AccountingSettingPM entityPM, AccountingSetting poco, bool isNewEntity)
        {
            poco.Id = entityPM.Id;
            poco.AccountingSystemCode = entityPM.AccountingSystemCode;
            poco.AllowManualInvoiceNumber = entityPM.AllowManualInvoiceNumber;
            poco.AllowVoidAPI = entityPM.AllowVoidAPI;
            poco.AllowVoidAPP = entityPM.AllowVoidAPP;
            poco.AllowVoidARI = entityPM.AllowVoidARI;
            poco.AllowVoidARP = entityPM.AllowVoidARP;
            poco.IsARInvoiceChronologicalDates = entityPM.IsARInvoiceChronologicalDates;
            poco.IsARPaymentChronologicalDates = entityPM.IsARPaymentChronologicalDates;
            poco.IsVatNumberMandatoryInAP = entityPM.IsVatNumberMandatoryInAP;
            poco.IsVatNumberMandatoryInAR = entityPM.IsVatNumberMandatoryInAR;
            poco.ReceivableVATableTempCard = entityPM.ReceivableVATableTempCard;
            poco.ReceivableVATExemptTempCard = entityPM.ReceivableVATExemptTempCard;
            poco.PayableVATableTempCard = entityPM.PayableVATableTempCard;
            poco.PayableVATExemptTempCard = entityPM.PayableVATExemptTempCard;
            poco.AllowMinusInvoicelines = entityPM.AllowMinusInvoicelines;
            poco.AllowClosureWithoutPayables = entityPM.AllowClosureWithoutPayables;
            poco.IsAPInvoicesTransferEnabled = entityPM.IsAPInvoicesTransferEnabled;
            poco.IsARInvoicesTransferEnabled = entityPM.IsARInvoicesTransferEnabled;
            poco.APInvoiceTransferStartDate = entityPM.APInvoiceTransferStartDate;
            poco.ARInvoiceTransferStartDate = entityPM.ARInvoiceTransferStartDate;
            poco.AllowPositiveAmountsInTheCreditNote = entityPM.AllowPositiveAmountsInTheCreditNote;
            poco.QBOrealMeID = entityPM.QBOrealMeID;
            poco.IsSingleTaxPerInvoice = entityPM.IsSingleTaxPerInvoice;
            poco.IsARPaymentsTransferEnabled = entityPM.IsARPaymentsTransferEnabled;
            poco.ARPaymentTransferStartDate = entityPM.ARPaymentTransferStartDate;
            poco.TransferToDropboxActivated = entityPM.TransferToDropboxActivated;
            poco.EnableMultiPercentageVATTypes = entityPM.EnableMultiPercentageVATTypes;
            poco.NotifyPastDateOnInvoiceEdit = entityPM.NotifyPastDateOnInvoiceEdit;
            poco.EnableMultiRateAPInvoices = entityPM.EnableMultiRateAPInvoices;
            poco.RegistryDateTypeCode = entityPM.RegistryDateTypeCode;
            poco.ReceivableVATCard = entityPM.ReceivableVATCard;
            poco.PayableVATCard = entityPM.PayableVATCard;
            poco.EnableMultiCurrencyARPayments = entityPM.EnableMultiCurrencyARPayments;
            poco.IsAPPaymentsTransferEnabled = entityPM.IsAPPaymentsTransferEnabled;
            poco.EnableNegativeOffsetARPayments = entityPM.EnableNegativeOffsetARPayments;
            poco.EnableNegativeOffsetAPPayments = entityPM.EnableNegativeOffsetAPPayments;
            poco.EnableMultiCurrencyAPPayments = entityPM.EnableMultiCurrencyAPPayments;
            poco.EnableInvoiceStocksManagement = entityPM.EnableInvoiceStocksManagement;
            poco.RefreshToken = entityPM.RefreshToken;
            poco.QBOOAuth = entityPM.QBOOAuth;
            poco.AllowManualARPaymentNumber = entityPM.AllowManualARPaymentNumber;
            poco.AllowRegionalTaxManagement = entityPM.AllowRegionalTaxManagement;
            poco.EnableAPPaymentExternalPayment = entityPM.EnableAPPaymentExternalPayment;
            poco.TransferToFTPActivated = entityPM.TransferToFTPActivated;
            poco.TransferFTPDetailId = entityPM.TransferFTPDetailId;
            poco.EnableEnteringTotalVAT = entityPM.EnableEnteringTotalVAT;
            poco.BlockSendInvoiceOriginalCopy = entityPM.BlockSendInvoiceOriginalCopy;
        }
    }
}