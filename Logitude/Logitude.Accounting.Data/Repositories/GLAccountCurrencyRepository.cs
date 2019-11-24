using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class GLAccountCurrencyRepository:IRepository<GLAccountCurrency>
   {
        
		public List<GLAccountCurrency> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public List<GLAccountCurrency> GetRelatedCurrenciesAccountByCustomerGLAccountAll(int tenant, string GLAccountId)
        {

            return (from a in context.GLAccountCurrencies
                    where a.MainGLAccountId == GLAccountId && a.Tenant == tenant
                    select a).ToList();
        }
        public List<GLAccountCurrency> GetRelatedCurrenciesAccountByCustomerGLAccountActive(int tenant, string GLAccountId)
        {
            return GetQRelatedCurrenciesAccountByCustomerGLAccountActive(tenant, GLAccountId).ToList();
        }

        public IQueryable<GLAccountCurrency> GetQRelatedCurrenciesAccountByCustomerGLAccountActive(int tenant, string GLAccountId)
        {
            return (from accCurr in context.GLAccountCurrencies
                        //.Include("GLAccount") -- in unitest not work !!!
                    join acc in context.GLAccounts
                    on accCurr.Id equals acc.Id

                    where accCurr.MainGLAccountId == GLAccountId && accCurr.Tenant == tenant
                    where acc.Inactive == false
                    select accCurr)
                                ;
        }

        public IQueryable<GLAccountCurrency> GetQRelatedCurrenciesAccountIdByCustomerGLAccount(int tenant, IQueryable<string> qGLAccountIdS)
        {

            return (from a in context.GLAccountCurrencies
                        //where a.MainGLAccountId == GLAccountId && a.Tenant == tenant
                    where qGLAccountIdS.Contains(a.MainGLAccountId) && a.Tenant == tenant
                    select a/*.Id*/);
        }


        public IQueryable<GLAccountCurrency> GetCurrenciesAccounts(int tenant)
        {

            return (from a in context.GLAccountCurrencies
                       
                    where  a.Tenant == tenant
                    select a/*.Id*/);
        }

        public GLAccountCurrency GetEntityByCurrencyAndGLAccountId(string accountId, string currencyId, int tenant)
        {

            return (from a in context.GLAccountCurrencies
                    where a.MainGLAccountId == accountId && a.CurrencyId == currencyId && a.Tenant == tenant

                    select a).FirstOrDefault();
        }
    }

}
   