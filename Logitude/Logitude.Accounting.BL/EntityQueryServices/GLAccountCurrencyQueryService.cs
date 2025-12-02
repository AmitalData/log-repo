using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using DemoCSTimbraCFDI;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountCurrencyQueryService : EntityQueryService<GLAccountCurrency, GLAccountCurrencyKeys, GLAccountCurrencyPM, GLAccountPM, GLAccountKeys>
    {
        public List<string> GetRelatedCurrenciesAccountCurrencyId(int tenant, string GLAccountId)
        {
            return this.repository.GetQRelatedCurrenciesAccountByCustomerGLAccountActive(tenant, GLAccountId).Select(r => r.CurrencyId).ToList();
        }

        public List<string> GetRelatedCurrenciesAccounts(int tenant, string GLAccountId)
        {
            return this.repository.GetQRelatedCurrenciesAccountByCustomerGLAccountActive(tenant, GLAccountId).Select(r => r.GLAccountId).ToList();
        }

        public List<GLAccountCurrencyPM> GetRelatedCurrenciesAccount(int tenant, string GLAccountId)
        {
            string key = $"GetRelatedCurrenciesAccount({tenant}, {GLAccountId})";
            return CacheManager.GetOrInsertNewObject<List<GLAccountCurrencyPM>>(key, () =>
            {
                var pocos = this.repository.GetRelatedCurrenciesAccountByCustomerGLAccountActive(tenant, GLAccountId);
                return pocos.Select(r => this.GetEntityPM(r, false)).ToList();
            });
            

        }

        public List<GLAccountCurrencyPM> GetTenantCurrenciesAccount(int tenant )
        {
            var pocos = this.repository.GetCurrenciesAccounts(tenant).ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();

        }

        public List<GLAccountCurrency> GetCurrenciesAccountsByTenant(int tenant)
        {
            var pocos = this.repository.GetCurrenciesAccounts(tenant).ToList();
            return pocos.ToList();

        }


        public List<GLAccountCurrencyPM> GetGLAccountCurrenciesByAccountIds(int tenant, List<string> accountIds)
        {
            var pocos = this.repository.GetGLAccountCurrenciesByGLAccountIds(tenant, accountIds).ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();

        }
        public GLAccountCurrencyPM GetEntityByCurrencyAndGLAccountId(string accountId, string currencyId, int tenant)
        {

            var poco = repository.GetEntityByCurrencyAndGLAccountId(accountId, currencyId, tenant);
            return GetEntityPM(poco);
        }

        public string GetReconcileMethodCodeByCurrencyAndGLAccountId(string accountId, string currencyId, int tenant)
        {
            return repository.GetReconcileMethodCodeByCurrencyAndGLAccountId(accountId, currencyId, tenant);
        }

        public GLAccountCurrencyPM GetEntityByGLAccountId(string accountId, int tenant)
        {

            var poco = repository.GetEntityByGLAccountId(accountId,  tenant);
            return GetEntityPM(poco);
        }
        public GLAccountCurrency GetGLAccountCurrencyByGLAccountId(string accountId, int tenant)
        {

            var poco = repository.GetEntityByGLAccountId(accountId, tenant);
            return poco;
        }

        public List<GLAccountCurrencyPM> GetRelatedCurrenciesAccountByCustomerGLAccount(string accountId, int tenant)
        {
            var relatedCurrenciesAccountByCustomerGLAccount = this.repository.
                GetRelatedCurrenciesAccountByCustomerGLAccountAll(tenant, accountId);
            return relatedCurrenciesAccountByCustomerGLAccount.Select(r => this.GetEntityPM(r, false)).ToList();
        }
    }
}
