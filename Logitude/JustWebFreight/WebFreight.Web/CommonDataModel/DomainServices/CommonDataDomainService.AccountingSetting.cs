using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
  
    public partial class CommonDataDomainService
    {
        public IQueryable<AccountingSettingPM> GetAccountingSettingPMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSettingQuery = new AccountingSettingQuery(tenant);
            return accountingSettingQuery.GetAccountSettingPMs();
        }

        public AccountingSettingPM GetTenantAccountingSetting(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSettingQuery = new AccountingSettingQuery(tenant);

            AccountingSettingPM acc = accountingSettingQuery.GetSingleAccountingSettingPMById(tenant);
            if (acc == null)
            {
                acc = new AccountingSettingPM()
                {
                    Id = tenant,
                    AllowVoidAPP = true,
                    AllowVoidAPI = true,
                    AllowVoidARI = true,
                    AllowManualInvoiceNumber = true,
                    AllowVoidARP = true,
                    IsARInvoiceChronologicalDates = false,
                    IsARPaymentChronologicalDates = false,
                    IsVatNumberMandatoryInAP = false,
                    IsVatNumberMandatoryInAR = false,
                    ReceivableVATableTempCard = null,
                    ReceivableVATExemptTempCard = null,
                    PayableVATableTempCard = null,
                    PayableVATExemptTempCard = null,
                    AllowMinusInvoicelines = false,
                    AllowClosureWithoutPayables = false,
                    IsAPInvoicesTransferEnabled = false,
                    IsARInvoicesTransferEnabled = false,
                    APInvoiceTransferStartDate = null,
                    ARInvoiceTransferStartDate = null,
                    AllowPositiveAmountsInTheCreditNote = false,                    
                };
            }

            return acc;
        }

        public void InsertAccountingSetting(AccountingSettingPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Id);
            }

            accountingSettingRepository = new AccountingSettingRepository(objectContext);
            AccountingSetting entityPOCO = new AccountingSetting() { Id = entityPM.Id };
            MapAccountingSettingAccountingSettingPM(entityPM, entityPOCO);
            accountingSettingRepository.Add(entityPOCO);
        }

        public void UpdateAccountingSetting(AccountingSettingPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Id);
            }

            string entityName = "AccountingSetting" + entityPM.Id;
            string entityPmName = "AccountingSettingPM" + entityPM.Id;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            if (!string.IsNullOrEmpty(entityPM.AccountingSystemCode))
            {
                string accountingEntityName = "AccountingSystem" + entityPM.AccountingSystemCode;
                string accountingEntityPmName = "AccountingSystemPM" + entityPM.AccountingSystemCode;

                if (CacheManager.CacheWrapper.Get(accountingEntityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(accountingEntityName);
                }

                if (CacheManager.CacheWrapper.Get(accountingEntityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(accountingEntityPmName);
                }
            }

            accountingSettingRepository = new AccountingSettingRepository(objectContext);            
            AccountingSetting entityPOCO = accountingSettingRepository.GetSingleAccountSetting(entityPM.Id);

            if (entityPOCO != null)
            {
                MapAccountingSettingAccountingSettingPM(entityPM, entityPOCO);
                accountingSettingRepository.Update(entityPOCO);
            }

            else
            {
                InsertAccountingSetting(entityPM);
            }
        }

        public void DeleteAccountingSetting(AccountingSettingPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Id);
            }
            accountingSettingRepository = new AccountingSettingRepository(objectContext);
            AccountingSetting acc = accountingSettingRepository.GetSingleAccountSetting(entity.Id);
            accountingSettingRepository.Remove(acc);
        }

        public void MapAccountingSettingAccountingSettingPM(AccountingSettingPM entityPM, AccountingSetting entityPOCO)
        {
            entityPOCO.AccountingSystemCode = entityPM.AccountingSystemCode;
            entityPOCO.AllowManualInvoiceNumber = entityPM.AllowManualInvoiceNumber;
            entityPOCO.AllowVoidAPI = entityPM.AllowVoidAPI;
            entityPOCO.AllowVoidAPP = entityPM.AllowVoidAPP;
            entityPOCO.AllowVoidARI = entityPM.AllowVoidARI;
            entityPOCO.AllowVoidARP = entityPM.AllowVoidARP;
            entityPOCO.IsARInvoiceChronologicalDates = entityPM.IsARInvoiceChronologicalDates;
            entityPOCO.IsARPaymentChronologicalDates = entityPM.IsARPaymentChronologicalDates;
            entityPOCO.IsVatNumberMandatoryInAP = entityPM.IsVatNumberMandatoryInAP;
            entityPOCO.IsVatNumberMandatoryInAR = entityPM.IsVatNumberMandatoryInAR;
            entityPOCO.ReceivableVATableTempCard = entityPM.ReceivableVATableTempCard;
            entityPOCO.ReceivableVATExemptTempCard = entityPM.ReceivableVATExemptTempCard;
            entityPOCO.PayableVATableTempCard = entityPM.PayableVATableTempCard;
            entityPOCO.PayableVATExemptTempCard = entityPM.PayableVATExemptTempCard;
            entityPOCO.AllowMinusInvoicelines = entityPM.AllowMinusInvoicelines;
            entityPOCO.AllowClosureWithoutPayables = entityPM.AllowClosureWithoutPayables;
            entityPOCO.IsAPInvoicesTransferEnabled = entityPM.IsAPInvoicesTransferEnabled;
            entityPOCO.IsARInvoicesTransferEnabled = entityPM.IsARInvoicesTransferEnabled;
            entityPOCO.APInvoiceTransferStartDate = entityPM.APInvoiceTransferStartDate;
            entityPOCO.ARInvoiceTransferStartDate = entityPM.ARInvoiceTransferStartDate;
            entityPOCO.AllowPositiveAmountsInTheCreditNote = entityPM.AllowPositiveAmountsInTheCreditNote;
            entityPOCO.IsSingleTaxPerInvoice = entityPM.IsSingleTaxPerInvoice;
            entityPOCO.IsARPaymentsTransferEnabled = entityPM.IsARPaymentsTransferEnabled;
            entityPOCO.ARPaymentTransferStartDate = entityPM.ARPaymentTransferStartDate;

            if (entityPOCO.AllowManualInvoiceNumber)
            {
                entityPOCO.IsARInvoiceChronologicalDates = false;
            }
        }

        public void InsertAccountingSystem(AccountingSystem entity)
        {
            accountingSystemRepository.Add(entity);
        }

        public void UpdateAccountingSystem(AccountingSystem currentEntity)
        {
            accountingSystemRepository.Update(currentEntity);
        }

        public void DeleteAccountingSystem(AccountingSystem entity)
        {
            accountingSystemRepository.Remove(entity);
        }      
    }
}