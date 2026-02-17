using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Accounting.BL.EntityDataMappings
{
    public class BankAccountCustomDataMapping: IBankAccountCustomDataMapping
    {
        public virtual void PMToPOCO(BankAccountPM entityPM, BankAccount entityPOCO, List<BankAccountDataMapping.POCOPropertyNames> customMappedPOCOProperties)
        {
            customMappedPOCOProperties.Add(BankAccountDataMapping.POCOPropertyNames.Id);
            customMappedPOCOProperties.Add(BankAccountDataMapping.POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            customMappedPOCOProperties.Add(BankAccountDataMapping.POCOPropertyNames.SearchFields);
            //BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
            
        }
        public virtual void POCOToPM(BankAccountPM entityPM, BankAccount entityPOCO, List<BankAccountDataMapping.PMPropertyNames> customMappedPMProperties)
        {
            customMappedPMProperties.Add(BankAccountDataMapping.PMPropertyNames.GLAccountCurrencyId);
            customMappedPMProperties.Add(BankAccountDataMapping.PMPropertyNames.GLAccountNumber);
            customMappedPMProperties.Add(BankAccountDataMapping.PMPropertyNames.DeferedGLAccountNumber);

            // Get currency id of account to compare it when creating a new deposit in html version
            if (entityPOCO.GLAccountId != null)
            {
                GLAccountPM gla = GetSingleGLAccountPM(entityPOCO.GLAccountId, entityPOCO.Tenant,false);
                if (gla != null)
                {
                    entityPM.GLAccountCurrencyId = gla.IsMultiCurrency == true ? "multi" : gla.CurrencyId;
                    entityPM.GLAccountNumber = gla.DisplayNumber;
                }
            }

            if (entityPOCO.DeferredGLAccountId != null)
            {
                GLAccountPM dgla = GetSingleGLAccountPM(entityPOCO.DeferredGLAccountId, entityPOCO.Tenant, true);
                if (dgla != null)
                {
                    entityPM.DeferedGLAccountNumber = dgla.DisplayNumber;
                }
            }

            if (entityPOCO.BankId != null)
            {
                BankCodePM bank = GetSingleBankCodePM(entityPOCO.BankId, entityPOCO.Tenant, true);
                if (bank != null)
                {
                    entityPM.BankCode = bank.Code;
                }
            }
        }

        public virtual GLAccountPM GetSingleGLAccountPM(string gLAccountId,int tenant,bool getFromCache)
        {
            GLAccountQueryService glaService = new GLAccountQueryService(tenant);
            GLAccountPM gla = glaService.GetSingle(gLAccountId, false, getFromCache);
            return gla;
        }

        public virtual BankCodePM GetSingleBankCodePM(string bankId, int tenant, bool getFromCache)
        {
            BankCodeQueryService bankService = new BankCodeQueryService(tenant);
            BankCodePM bankCodePM = bankService.GetSingle(bankId, false, getFromCache);
            return bankCodePM;
        }
    }

    public interface IBankAccountCustomDataMapping
    {
        void PMToPOCO(BankAccountPM entityPM, BankAccount entityPOCO, List<BankAccountDataMapping.POCOPropertyNames> customMappedPOCOProperties);
        void POCOToPM(BankAccountPM entityPM, BankAccount entityPOCO, List<BankAccountDataMapping.PMPropertyNames> customMappedPMProperties);
        GLAccountPM GetSingleGLAccountPM(string gLAccountId, int tenant, bool getFromCache);
        BankCodePM GetSingleBankCodePM(string bankId, int tenant, bool getFromCache);
    }
}
