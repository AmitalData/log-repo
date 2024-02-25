using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.Accounting.BL.CoreBL
{
    //http://jeffhandley.com/archive/2010/10/25/CrossEntityValidation.aspx
    public class AccountingValidationContextServiceProvider : IServiceProvider
    {
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            if (this._services.ContainsKey(serviceType))
            {
                return this._services[serviceType];
            }

            return null;
        }

        public void AddService<T>(T service)
        {
            this._services[typeof(T)] = service;
        }

        public static ValidationContext NewJournalValidatorContextByAContext(
            IAccountingContext _AccountingContext, JournalPM journalPM,
            bool SuppressCheckGLAccountIsMultiCurrencyWI40640)
        {
            //http://jeffhandley.com/archive/2010/10/25/CrossEntityValidation.aspx
            var typeregular = "1"; //1	Regular	רגיל	1,Regular,רגיל	0
            var accountingPeriodQueryService = new AccountingPeriodQueryService(_AccountingContext);
            var accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodByType(typeregular, journalPM.Tenant);
            var newJournalValidatorDataProvider = new JournalValidatorDataProvider(_AccountingContext);
            
            var newExternalReconcileDataProvider = new ExternalReconcileDataProvider(_AccountingContext);
            var myFullAccountingSettingPM = FullAccountingSettingQueryService.Get(journalPM.Tenant);
            var myAccountingSettingResolver = new AccountingSettingResolver();
            var newIJournalValidatorRateDataProvider = new JournalValidatorRateDataProvider(journalPM.Tenant);
            string tenantCurrencyId = GetTenantCurrencyId(journalPM.Tenant);
            return NewJournalValidatorContext(journalPM, accountingPeriodsByTypeRegular, 
                newJournalValidatorDataProvider,
                newExternalReconcileDataProvider,
                myAccountingSettingResolver,
                myFullAccountingSettingPM,
                SuppressCheckGLAccountIsMultiCurrencyWI40640,
                newIJournalValidatorRateDataProvider,
                tenantCurrencyId

                );
        }

        private static string GetTenantCurrencyId(int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);
            return tenantPM.CurrencyId;
        }

        public static ValidationContext NewJournalValidatorContext(
            JournalPM journalPM, 
            List<AccountingPeriodPM> accountingPeriodsByTypeRegular, 
            IJournalValidatorContextDataProvider newJournalValidatorDataProvider,
            IExternalReconcileDataProvider newExternalReconcileDataProvider,
            IAccountingSettingResolver newAccountingSettingResolver,
            FullAccountingSettingPM myFullAccountingSettingPM,
            bool SuppressCheckGLAccountIsMultiCurrencyWI40640,
            IJournalValidatorRateDataProvider journalValidatorRateDataProvider,string tenantCurrencyId,
            DateTime? DateTimeUtcNow = null
            )
        {
            //var newAccountingSettingResolver = new AccountingSettingResolver();
            var contextServiceProvider = new AccountingValidationContextServiceProvider();
            contextServiceProvider.AddService<IJournalValidatorContextDataProvider>(newJournalValidatorDataProvider);
            contextServiceProvider.AddService<IAccountingSettingResolver>(newAccountingSettingResolver);
            contextServiceProvider.AddService<IExternalReconcileDataProvider>(newExternalReconcileDataProvider);
            contextServiceProvider.AddService<IJournalValidatorRateDataProvider>(journalValidatorRateDataProvider);


            //var myFullAccountingSettingPM = FullAccountingSettingQueryService.Get(journalPM.Tenant);

            var contextItems = new Dictionary<object, object>
            {
                { JournalValidator.K_AccountingPeriodsByTypeRegular, accountingPeriodsByTypeRegular },
                { JournalValidator.K_TenantCurrencyId, tenantCurrencyId },
                { JournalValidator.K_FullAccountingSettingPM, myFullAccountingSettingPM },
                {
                    JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640 //טיפול בסרביס לפקודת יומן - במקרה של כרטיס מפוצל לרשום על הפיצול
                ,
                    SuppressCheckGLAccountIsMultiCurrencyWI40640
                },
            };
            if (DateTimeUtcNow != null)
            {
                contextItems.Add(JournalValidator.K_DateTimeUtcNow, DateTimeUtcNow);
            }
            var _JournalValidatorContext = new System.ComponentModel.DataAnnotations.ValidationContext(journalPM, contextServiceProvider, contextItems);
            return _JournalValidatorContext
            ;
        }


        public static ValidationContext NewReconciliationValidatorContext(
    IAccountingContext _AccountingContext, ReconciliationPM reconciliationPM)
        {
            //http://jeffhandley.com/archive/2010/10/25/CrossEntityValidation.aspx
            

            var contextItems = new Dictionary<object, object>
            {
            //    { "AccountingPeriodsByTypeRegular", _AccountingPeriodsByTypeRegular }
            };
            var contextServiceProvider = new AccountingValidationContextServiceProvider();

            contextServiceProvider.AddService<IReconciliationValidatorContextDataProvider>(new ReconciliationDataProvider(_AccountingContext));
            var _JournalValidatorContext = new System.ComponentModel.DataAnnotations.ValidationContext(reconciliationPM, contextServiceProvider, contextItems);
            return _JournalValidatorContext
            ;
        }
    }

    public class ReconciliationDataProvider : IReconciliationValidatorContextDataProvider
    {
        private Data.IAccountingContext _AccountingContext;

        public ReconciliationDataProvider(Data.IAccountingContext _AccountingContext)
        {
            // TODO: Complete member initialization
            this._AccountingContext = _AccountingContext;
        }
        public GLAccountPM GetGLAccount(string GLAccountId, int tenant)
        {
            var a = new GLAccountQueryService(_AccountingContext);
            return a.GetSinglePM(GLAccountId, tenant);
        }
        public List<JournalLine> GetJournalLineByLedgerTransactionIdList(List<string> transactionIdList, int tenant)
        {
            var a = new JournalLineRepository(_AccountingContext);
            return a.GetJournalLineByLedgerTransactionIdList(transactionIdList, tenant).ToList();
        }
        public List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> transactionIdList, int tenant)
        {
            var a = new LedgerTransactionQueryService(_AccountingContext);
            return a.GetLedgerTransactionPMsByIdList(transactionIdList, tenant);
        }

        
    }
    public class JournalValidatorDataProvider : IJournalValidatorContextDataProvider
    {
        private Data.IAccountingContext _AccountingContext;

        public JournalValidatorDataProvider(Data.IAccountingContext _AccountingContext)
        {
            // TODO: Complete member initialization
            this._AccountingContext = _AccountingContext;
        }


        public GLAccountPM GetGLAccount(string GLAccountId, int tenant)
        {
            bool fromCache = true;//ohad said :NOT USUAL SCENARIO
            var a = new GLAccountQueryService(_AccountingContext);
            return a.GetSingle(GLAccountId, false, fromCache);
        }


        public Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM GetCurrency(string CurrencyId, int tenant)
        {


            string key = $"GetCurrency_P({tenant})";
            return CacheManager.GetOrInsertNewObject<Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM>(key, () =>
            {
                var a = new CurrencyQuery(tenant);
                return a.GetSinglePM(CurrencyId, tenant);
            });
        }


        public List<string> GetGLAccountCurrencyList(string CustomerGLAccountId, int tenant)
        {
            string key = $"GetGLAccountCurrencyList({CustomerGLAccountId},{tenant})";
            return CacheManager.GetOrInsertNewObject<List<string>>(key, () =>
            {
                var a = new GLAccountCurrencyQueryService(_AccountingContext);
                return a.GetRelatedCurrenciesAccountCurrencyId(tenant, CustomerGLAccountId).ToList();
            });
            
            
        }





        public string CheckExternalNoAndSystemReturnJournalNumber(string externalNo, string externalSystem, int tenant)
        {
            string journalNumber = null;
            JournalQueryService journalQuery = new JournalQueryService(_AccountingContext);
            journalQuery.CheckIfExternalNoAndSystemExist(externalNo, externalSystem, out journalNumber, tenant);
            return journalNumber;

        }
    }










    public class JournalValidatorRateDataProvider : IJournalValidatorRateDataProvider
    {
        

        public JournalValidatorRateDataProvider(int tenant)
        {
            this.Tenant = tenant;


        }

        public int Tenant { get; }

        public bool ExistRate(string TenantCurrency, string foreignCurrencyId, DateTime? date, int tenent)
        {
            var webFreightContext = WebFreightContext.GetContext(this.Tenant);
            var ratesTableRepository = new RatesTableRepository(webFreightContext);

            var entityPoco = ratesTableRepository.GetExchageRateByValueAndDate(TenantCurrency, foreignCurrencyId, date, tenent);
            if (entityPoco == null)
            {
                return false;
            }
            return true; 

        }



    }

}
