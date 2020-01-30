using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AccountingSettingQuery
    {
        AccountingSettingRepository repository;
        public AccountingSettingQuery()
        {
            repository = new AccountingSettingRepository(); 
        }
        public AccountingSettingQuery(int tenant)
        {
            repository = new AccountingSettingRepository(tenant);
        }
        public AccountingSettingQuery(AccountingSettingRepository accountingSettingRepository)
        {
            repository = accountingSettingRepository;
        }

        public AccountingSettingPM GetSingleAccountingSettingPMById(int id)
        {
            AccountingSettingPM account = (from a in repository.context.AccountingSettings
                                           where a.Id == id
                                           select new AccountingSettingPM()
                                           {
                                               Id = a.Id,
                                               AccountingSystemCode = a.AccountingSystemCode,
                                               AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                                               AllowVoidAPI = a.AllowVoidAPI,
                                               AllowVoidAPP = a.AllowVoidAPP,
                                               AllowVoidARI = a.AllowVoidARI,
                                               AllowVoidARP = a.AllowVoidARP,
                                               IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                                               IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                                               IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                                               IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                                               ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                                               ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                                               PayableVATableTempCard = a.PayableVATableTempCard,
                                               PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                                               AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                                               AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                                               IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                                               IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                                               APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                                               ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                                               AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                                               QBOAccessToken = a.QBOAccessToken,
                                               QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                                               QBOrealMeID = a.QBOrealMeID,
                                               IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                                               IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                                               ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                                               TransferToDropboxActivated = a.TransferToDropboxActivated,
                                               EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                                               NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                                               EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                                               RegistryDateTypeCode = a.RegistryDateTypeCode,
                                               ReceivableVATCard = a.ReceivableVATCard,
                                               PayableVATCard = a.PayableVATCard,
                                               EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                                               IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                                               EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                                               EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                                               EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                                               EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                                               RefreshToken=a.RefreshToken,
                                               QBOOAuth=a.QBOOAuth,
                                               AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                                           }).FirstOrDefault();

            return account;
        }

        public AccountingSettingPM GetSinglePM(int id)
        {
            AccountingSettingPM account = (from a in repository.context.AccountingSettings
                                           where a.Id == id
                                           select new AccountingSettingPM()
                                           {
                                               Id = a.Id,
                                               AccountingSystemCode = a.AccountingSystemCode,
                                               AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                                               AllowVoidAPI = a.AllowVoidAPI,
                                               AllowVoidAPP = a.AllowVoidAPP,
                                               AllowVoidARI = a.AllowVoidARI,
                                               AllowVoidARP = a.AllowVoidARP,
                                               IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                                               IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                                               IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                                               IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                                               ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                                               ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                                               PayableVATableTempCard = a.PayableVATableTempCard,
                                               PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                                               AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                                               AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                                               IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                                               IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                                               APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                                               ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                                               AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                                               QBOAccessToken = a.QBOAccessToken,
                                               QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                                               QBOrealMeID = a.QBOrealMeID,
                                               IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                                               IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                                               ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                                               TransferToDropboxActivated = a.TransferToDropboxActivated,
                                               EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                                               NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                                               EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                                               RegistryDateTypeCode = a.RegistryDateTypeCode,
                                               ReceivableVATCard = a.ReceivableVATCard,
                                               PayableVATCard = a.PayableVATCard,
                                               EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                                               IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                                               EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                                               EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                                               EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                                               EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                                               RefreshToken = a.RefreshToken,
                                               QBOOAuth = a.QBOOAuth,
                                               AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                                           }).FirstOrDefault();

            if (account != null)
            {
                TenantPM tenantpm = TenantQuery.GetSingleTenantPM(account.Id, false);
                account.PaymentTermId = tenantpm.PaymentTermId;
                account.VatNumber = tenantpm.VatNumber;
            }

            return account;
        }

        public AccountingSettingPM GetSingleAccountSettingPM(int id)
        {
            AccountingSettingPM entity = null;
            string entityName = "AccountingSettingPM" + id;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {

                    var systems = (from a in repository.context.AccountingSettings
                                   where a.Id == id
                                   select new AccountingSettingPM()
                                   {
                                       Id = a.Id,
                                       AccountingSystemCode = a.AccountingSystemCode,
                                       AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                                       AllowVoidAPI = a.AllowVoidAPI,
                                       AllowVoidAPP = a.AllowVoidAPP,
                                       AllowVoidARI = a.AllowVoidARI,
                                       AllowVoidARP = a.AllowVoidARP,
                                       IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                                       IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                                       IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                                       IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                                       ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                                       ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                                       PayableVATableTempCard = a.PayableVATableTempCard,
                                       PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                                       AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                                       AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                                       IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                                       IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                                       APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                                       ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                                       AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                                       QBOAccessToken = a.QBOAccessToken,
                                       QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                                       QBOrealMeID = a.QBOrealMeID,
                                       IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                                       IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                                       ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                                       TransferToDropboxActivated = a.TransferToDropboxActivated,
                                       EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                                       NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                                       EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                                       RegistryDateTypeCode = a.RegistryDateTypeCode,
                                       ReceivableVATCard = a.ReceivableVATCard,
                                       PayableVATCard = a.PayableVATCard,
                                       EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                                       IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                                       EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                                       EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                                       EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                                       EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                                       RefreshToken = a.RefreshToken,
                                       QBOOAuth = a.QBOOAuth,
                                       AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                                   });

                    foreach (var c in systems)
                    {
                        string cname = "AccountingSettingPM" + c.Id;

                        if (CacheManager.CacheWrapper.Get(cname) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                    entity = (AccountingSettingPM)CacheManager.CacheWrapper.Get(entityName);

                }
                else
                {
                    entity = (AccountingSettingPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.AccountingSettings
                          where a.Id == id
                          select new AccountingSettingPM()
                          {
                              Id = a.Id,
                              AccountingSystemCode = a.AccountingSystemCode,
                              AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                              AllowVoidAPI = a.AllowVoidAPI,
                              AllowVoidAPP = a.AllowVoidAPP,
                              AllowVoidARI = a.AllowVoidARI,
                              AllowVoidARP = a.AllowVoidARP,
                              IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                              IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                              IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                              IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                              ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                              ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                              PayableVATableTempCard = a.PayableVATableTempCard,
                              PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                              AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                              AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                              IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                              IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                              APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                              ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                              AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                              QBOAccessToken = a.QBOAccessToken,
                              QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                              QBOrealMeID = a.QBOrealMeID,
                              IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                              IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                              ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                              TransferToDropboxActivated = a.TransferToDropboxActivated,
                              EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                              NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                              EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                              RegistryDateTypeCode = a.RegistryDateTypeCode,
                              ReceivableVATCard = a.ReceivableVATCard,
                              PayableVATCard = a.PayableVATCard,
                              EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                              IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                              EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                              EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                              EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                              EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                              RefreshToken = a.RefreshToken,
                              QBOOAuth = a.QBOOAuth,
                              AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                          }).FirstOrDefault();
            }

            return entity;
        }        

        public IQueryable<AccountingSettingPM> GetAccountSettingPMs()
        {
            IQueryable<AccountingSettingPM> accounts = (from a in repository.context.AccountingSettings

                                                        select new AccountingSettingPM()
                                                        {
                                                            Id = a.Id,
                                                            AccountingSystemCode = a.AccountingSystemCode,
                                                            AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                                                            AllowVoidAPI = a.AllowVoidAPI,
                                                            AllowVoidAPP = a.AllowVoidAPP,
                                                            AllowVoidARI = a.AllowVoidARI,
                                                            AllowVoidARP = a.AllowVoidARP,
                                                            IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                                                            IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                                                            IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                                                            IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                                                            ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                                                            ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                                                            PayableVATableTempCard = a.PayableVATableTempCard,
                                                            PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                                                            AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                                                            AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                                                            IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                                                            IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                                                            APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                                                            ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                                                            AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                                                            QBOAccessToken = a.QBOAccessToken,
                                                            QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                                                            QBOrealMeID = a.QBOrealMeID,
                                                            IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                                                            IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                                                            ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                                                            TransferToDropboxActivated = a.TransferToDropboxActivated,
                                                            EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                                                            NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                                                            EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                                                            RegistryDateTypeCode = a.RegistryDateTypeCode,
                                                            ReceivableVATCard = a.ReceivableVATCard,
                                                            PayableVATCard = a.PayableVATCard,
                                                            EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                                                            IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                                                            EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                                                            EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                                                            EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                                                            EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                                                            RefreshToken = a.RefreshToken,
                                                            QBOOAuth = a.QBOOAuth,
                                                            AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                                                        });

            return accounts;
        }

        public IQueryable<AccountingSettingList> GetIQueryableEntityList(IQueryable<AccountingSetting> iQueryable)
        {
            IQueryable<AccountingSettingList> result = from a in iQueryable
                                                       select new AccountingSettingList()
                                                       {
                                                           Id = a.Id,
                                                           AccountingSystemCode = a.AccountingSystemCode,
                                                           AllowManualInvoiceNumber = a.AllowManualInvoiceNumber,
                                                           AllowVoidAPI = a.AllowVoidAPI,
                                                           AllowVoidAPP = a.AllowVoidAPP,
                                                           AllowVoidARI = a.AllowVoidARI,
                                                           AllowVoidARP = a.AllowVoidARP,
                                                           IsARInvoiceChronologicalDates = a.IsARInvoiceChronologicalDates,
                                                           IsARPaymentChronologicalDates = a.IsARPaymentChronologicalDates,
                                                           IsVatNumberMandatoryInAP = a.IsVatNumberMandatoryInAP,
                                                           IsVatNumberMandatoryInAR = a.IsVatNumberMandatoryInAR,
                                                           ReceivableVATableTempCard = a.ReceivableVATableTempCard,
                                                           ReceivableVATExemptTempCard = a.ReceivableVATExemptTempCard,
                                                           PayableVATableTempCard = a.PayableVATableTempCard,
                                                           PayableVATExemptTempCard = a.PayableVATExemptTempCard,
                                                           AllowMinusInvoicelines = a.AllowMinusInvoicelines,
                                                           AllowClosureWithoutPayables = a.AllowClosureWithoutPayables,
                                                           IsAPInvoicesTransferEnabled = a.IsAPInvoicesTransferEnabled,
                                                           IsARInvoicesTransferEnabled = a.IsARInvoicesTransferEnabled,
                                                           APInvoiceTransferStartDate = a.APInvoiceTransferStartDate,
                                                           ARInvoiceTransferStartDate = a.ARInvoiceTransferStartDate,
                                                           AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                                                           QBOAccessToken = a.QBOAccessToken,
                                                           QBOAccessTokenSecret = a.QBOAccessTokenSecret,
                                                           QBOrealMeID = a.QBOrealMeID,
                                                           IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                                                           IsARPaymentsTransferEnabled = a.IsARPaymentsTransferEnabled,
                                                           ARPaymentTransferStartDate = a.ARPaymentTransferStartDate,
                                                           TransferToDropboxActivated = a.TransferToDropboxActivated,
                                                           EnableMultiPercentageVATTypes = a.EnableMultiPercentageVATTypes,
                                                           NotifyPastDateOnInvoiceEdit = a.NotifyPastDateOnInvoiceEdit,
                                                           EnableMultiRateAPInvoices = a.EnableMultiRateAPInvoices,
                                                           RegistryDateTypeCode = a.RegistryDateTypeCode,
                                                           ReceivableVATCard = a.ReceivableVATCard,
                                                           PayableVATCard = a.PayableVATCard,
                                                           EnableMultiCurrencyARPayments = a.EnableMultiCurrencyARPayments,
                                                           IsAPPaymentsTransferEnabled = a.IsAPPaymentsTransferEnabled,
                                                           EnableNegativeOffsetARPayments = a.EnableNegativeOffsetARPayments,
                                                           EnableNegativeOffsetAPPayments = a.EnableNegativeOffsetAPPayments,
                                                           EnableMultiCurrencyAPPayments = a.EnableMultiCurrencyAPPayments,
                                                           EnableInvoiceStocksManagement = a.EnableInvoiceStocksManagement,
                                                           RefreshToken = a.RefreshToken,
                                                           QBOOAuth = a.QBOOAuth,
                                                           AllowManualARPaymentNumber = a.AllowManualARPaymentNumber,
                                                       };
            return result;
        }
    }
}
