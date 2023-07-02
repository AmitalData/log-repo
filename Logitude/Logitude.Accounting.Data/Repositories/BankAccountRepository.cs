 
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
   public partial class BankAccountRepository:IRepository<BankAccount>
   {
		public List<BankAccount> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public BankAccount GetSingleBankAccount(string id, int tenant)
        {
            BankAccount entity; 

            entity = (from a in context.BankAccounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();

            return entity;
        }

        public BankAccount GetBankAccountByNumber(string number, int tenant)
        {
            BankAccount entity;

            entity = (from a in context.BankAccounts
                      where a.AccountNumber == number && a.Tenant == tenant
                      select a).FirstOrDefault();

            return entity;
        }

        public BankAccount GetBankAccountByGLAccountId(string accountId, int tenant)
        {
            BankAccount entity;

            entity = (from a in context.BankAccounts
                      where a.GLAccountId == accountId && a.Tenant == tenant
                      select a).FirstOrDefault();

            return entity;
        }
        public List<BankAccount> GetBankAccountListByBankIdAccNumber(string  BankId, string AccountNumber, int tenant)
        {
            

            var entityList = (from a in context.BankAccounts
                      where
                      a.BankId == BankId &&
                      a.AccountNumber == AccountNumber &&
                      a.Tenant == tenant
                      select a)/*.FirstOrDefault();*/
                      ;

            return entityList.ToList();
        }

        public BankAccount GetBankAccountByTransferGLAcccountId(string transferGLAcccountId, int tenant)
        {
            BankAccount entity;

            entity = (from a in context.BankAccounts
                      where
                      a.TransferGLAcccountId== transferGLAcccountId &&
                      
                      a.Tenant == tenant
                      select a).FirstOrDefault();

            return entity;
        }

        public bool CheckIfGlAccountExistsInBankAccount(string accountId, int tenant)
        {

            return (from a in context.BankAccounts
                      where (a.GLAccountId == accountId || a.DeferredGLAccountId == accountId || a.TransferGLAcccountId == accountId) && a.Tenant == tenant
                      select a).Any();
        }

    }
}
   