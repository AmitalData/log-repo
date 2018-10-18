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
    public class AccountingSystemQuery
    {
        AccountingSystemRepository repository;
        public AccountingSystemQuery()
        {
            repository = new AccountingSystemRepository(); 
        }

        public AccountingSystemQuery(int tenant)
        {
            repository = new AccountingSystemRepository(tenant);
        }

        public AccountingSystemQuery(AccountingSystemRepository accountingSystemRepository)
        {
            repository = accountingSystemRepository;
        }

        public AccountingSystemPM GetSingleAccountingSystemPM(string code)
        {
            string key = $"GetSingleAccountingSystemPM({code})";
            return CacheManager.GetOrInsertNewObject<AccountingSystemPM>(key, () =>
          {
              return (from a in repository.context.AccountingSystems
                      where a.Code == code
                      select new AccountingSystemPM()
                      {
                          Code = a.Code,
                          Name = a.Name,
                          SearchFields = a.SearchFields,
                          IsExternalCodesFromTable = a.IsExternalCodesFromTable,
                          IsExternalCodesFromAPI = a.IsExternalCodesFromAPI,
                          IsExternalCodesSyncEnabled = a.IsExternalCodesSyncEnabled,
                          IsJournalMode = a.IsJournalMode,
                          IsSingleCurrencyAccount = a.IsSingleCurrencyAccount,
                          IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                          AllowManuallyDueDate = a.AllowManuallyDueDate,
                          IsTaxItemManaged = a.IsTaxItemManaged,
                          AllowMinusInvoiceLines = a.AllowMinusInvoiceLines,
                          ShowDownloadScreen = a.ShowDownloadScreen,
                          AllowAPInvoicesTransfer = a.AllowAPInvoicesTransfer,
                          AllowARInvoicesTransfer = a.AllowARInvoicesTransfer,
                          AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                          InActive = a.InActive,
                          AllowARPaymentsTransfer = a.AllowARPaymentsTransfer,
                          CanTransferToDropbox = a.CanTransferToDropbox,
                          AllowAPPaymentsTransfer = a.AllowAPPaymentsTransfer,
                      }).FirstOrDefault();
          });
        }

        public AccountingSystemPM GetSinglePM(string code)
        {
            return (from a in repository.context.AccountingSystems
                    where a.Code == code
                    select new AccountingSystemPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        IsExternalCodesFromTable = a.IsExternalCodesFromTable,
                        IsExternalCodesFromAPI = a.IsExternalCodesFromAPI,
                        IsExternalCodesSyncEnabled = a.IsExternalCodesSyncEnabled,
                        IsJournalMode = a.IsJournalMode,
                        IsSingleCurrencyAccount = a.IsSingleCurrencyAccount,
                        IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                        AllowManuallyDueDate = a.AllowManuallyDueDate,
                        IsTaxItemManaged = a.IsTaxItemManaged,
                        AllowMinusInvoiceLines = a.AllowMinusInvoiceLines,
                        ShowDownloadScreen = a.ShowDownloadScreen,
                        AllowAPInvoicesTransfer = a.AllowAPInvoicesTransfer,
                        AllowARInvoicesTransfer = a.AllowARInvoicesTransfer,
                        AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                        InActive = a.InActive,
                        AllowARPaymentsTransfer = a.AllowARPaymentsTransfer,
                        CanTransferToDropbox = a.CanTransferToDropbox,
                        AllowAPPaymentsTransfer = a.AllowAPPaymentsTransfer,

                    }).FirstOrDefault();
        }


        public AccountingSystemPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.AccountingSystems
                    where a.Code == code
                    select new AccountingSystemPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        IsExternalCodesFromTable = a.IsExternalCodesFromTable,
                        IsExternalCodesFromAPI = a.IsExternalCodesFromAPI,
                        IsExternalCodesSyncEnabled = a.IsExternalCodesSyncEnabled,
                        IsJournalMode = a.IsJournalMode,
                        IsSingleCurrencyAccount = a.IsSingleCurrencyAccount,
                        IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                        AllowManuallyDueDate = a.AllowManuallyDueDate,
                        IsTaxItemManaged = a.IsTaxItemManaged,
                        AllowMinusInvoiceLines = a.AllowMinusInvoiceLines,
                        ShowDownloadScreen = a.ShowDownloadScreen,
                        AllowAPInvoicesTransfer = a.AllowAPInvoicesTransfer,
                        AllowARInvoicesTransfer = a.AllowARInvoicesTransfer,
                        AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                        InActive = a.InActive,
                        AllowARPaymentsTransfer = a.AllowARPaymentsTransfer,
                        CanTransferToDropbox = a.CanTransferToDropbox,
                        AllowAPPaymentsTransfer = a.AllowAPPaymentsTransfer,

                    }).FirstOrDefault();
        }

        public IQueryable<AccountingSystemPM> GetAccountingSystemPMs()
        {
            return from a in repository.context.AccountingSystems
                   select new AccountingSystemPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                       IsExternalCodesFromTable = a.IsExternalCodesFromTable,
                       IsExternalCodesFromAPI = a.IsExternalCodesFromAPI,
                       IsExternalCodesSyncEnabled = a.IsExternalCodesSyncEnabled,
                       IsJournalMode = a.IsJournalMode,
                       IsSingleCurrencyAccount = a.IsSingleCurrencyAccount,
                       IsSingleTaxPerInvoice = a.IsSingleTaxPerInvoice,
                       AllowManuallyDueDate = a.AllowManuallyDueDate,
                       IsTaxItemManaged = a.IsTaxItemManaged,
                       AllowMinusInvoiceLines = a.AllowMinusInvoiceLines,
                       ShowDownloadScreen = a.ShowDownloadScreen,
                       AllowAPInvoicesTransfer = a.AllowAPInvoicesTransfer,
                       AllowARInvoicesTransfer = a.AllowARInvoicesTransfer,
                       AllowPositiveAmountsInTheCreditNote = a.AllowPositiveAmountsInTheCreditNote,
                       InActive = a.InActive,
                       AllowARPaymentsTransfer = a.AllowARPaymentsTransfer,
                       CanTransferToDropbox = a.CanTransferToDropbox,
                       AllowAPPaymentsTransfer = a.AllowAPPaymentsTransfer,

                   };
        }

        public IQueryable<AccountingSystemList> GetIQueryableEntityList(IQueryable<AccountingSystem> iQueryable)
        {
            var result = from entity in iQueryable
                         select new AccountingSystemList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                             IsExternalCodesFromTable = entity.IsExternalCodesFromTable,
                             IsExternalCodesFromAPI = entity.IsExternalCodesFromAPI,
                             IsExternalCodesSyncEnabled = entity.IsExternalCodesSyncEnabled,
                             IsJournalMode = entity.IsJournalMode,
                             IsSingleCurrencyAccount = entity.IsSingleCurrencyAccount,
                             IsSingleTaxPerInvoice = entity.IsSingleTaxPerInvoice,
                             AllowManuallyDueDate = entity.AllowManuallyDueDate,
                             IsTaxItemManaged = entity.IsTaxItemManaged,
                             AllowMinusInvoiceLines = entity.AllowMinusInvoiceLines,
                             ShowDownloadScreen = entity.ShowDownloadScreen,
                             AllowAPInvoicesTransfer = entity.AllowAPInvoicesTransfer,
                             AllowARInvoicesTransfer = entity.AllowARInvoicesTransfer,
                             AllowPositiveAmountsInTheCreditNote = entity.AllowPositiveAmountsInTheCreditNote,
                             InActive = entity.InActive,
                             AllowARPaymentsTransfer = entity.AllowARPaymentsTransfer,
                             CanTransferToDropbox = entity.CanTransferToDropbox,
                             AllowAPPaymentsTransfer = entity.AllowAPPaymentsTransfer,

                         };

            return result;
        }
    }
}
