
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountCurrencyDataMapping: IMapping<GLAccountCurrencyPM, GLAccountCurrency>
   {

        public void CustomPMToPOCO(GLAccountCurrencyPM entityPM, GLAccountCurrency entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.MainGLAccountId);
            AddPOCOPropertyName(POCOPropertyNames.CurrencyId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.MainGLAccountId = entityPM.MainGLAccountId;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.CurrencyId = entityPM.CurrencyId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        
        }

        public void CustomPOCOToPM(GLAccountCurrencyPM entityPM, GLAccountCurrency entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GLAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GLAccountNumber);

            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);

                if (currency != null)
                {
                    entityPM.CurrencyName = currency.EnglishName;
                    entityPM.CurrencyCode = currency.Code;
                }
            }


            if (entityPOCO.GLAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM gLAccount = gLAccountQueryService.GetSingle(entityPOCO.GLAccountId, false, false);

                if (gLAccount != null)
                {
                    entityPM.GLAccountName = gLAccount.LocalName ??gLAccount.EnglishName ;
                    entityPM.GLAccountNumber = gLAccount.DisplayNumber;
                }
            }

        }
   }


}
   