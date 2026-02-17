using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class BankAccountQueryService : EntityQueryService<BankAccount, BankAccountKeys, BankAccountPM, object, BankAccountKeys>
    {
        public BankAccountPM GetByFirstOrDefault(string id, int tenant)
        {
            BankAccount bankAccount = this.repository.GetSingleBankAccount(id, tenant);

            if (bankAccount != null)
            {
                EntityPM = new BankAccountPM();
                mapping.CustomPOCOToPM(EntityPM, bankAccount);
                mapping.POCOToPM(EntityPM, bankAccount);
            }

            return EntityPM;
        }
        public BankAccountPM GetByAccountNumber(string number, int tenant)
        {
            BankAccount bankAccount = this.repository.GetBankAccountByNumber(number, tenant);
            return this.GetEntityPM(bankAccount);

            var EntityPM = new BankAccountPM();
            if (bankAccount != null)
            {

                mapping.CustomPOCOToPM(EntityPM, bankAccount);
                mapping.POCOToPM(EntityPM, bankAccount);
            }

        }

        public BankAccountPM GetBankAccountByTransferGLAcccountId(string transferGLAcccountId, int tenant)
        {
            BankAccount bankAccount = this.repository.GetBankAccountByTransferGLAcccountId(transferGLAcccountId, tenant);

            if (bankAccount != null)
            {
                return this.GetEntityPM(bankAccount);
            }

            return null;
        }

        public BankAccountPM GetBankAccountByBankIdAccNumber(string BankId, string AccountNumber, int tenant)
        {
            BankAccount poco = this.repository.GetBankAccountByBankIdAccNumber(BankId, AccountNumber, tenant);

            return this.GetEntityPM(poco);

        }

        public BankAccountPM GetByGLAccountId(string accountId, int tenant)
        {
            BankAccount bankAccount = this.repository.GetBankAccountByGLAccountId(accountId, tenant);
            return this.GetEntityPM(bankAccount);

            var EntityPM = new BankAccountPM();
            if (bankAccount != null)
            {
                
                mapping.CustomPOCOToPM(EntityPM, bankAccount);
                mapping.POCOToPM(EntityPM, bankAccount);
            }

            return EntityPM;
        }

    }
}