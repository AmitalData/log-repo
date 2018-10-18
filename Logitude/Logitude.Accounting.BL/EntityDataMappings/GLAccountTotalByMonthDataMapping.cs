
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
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountTotalByMonthDataMapping: IMapping<GLAccountTotalByMonthPM, GLAccountTotalByMonth>
   {

        public void CustomPMToPOCO(GLAccountTotalByMonthPM entityPM, GLAccountTotalByMonth entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.AccountId);
            AddPOCOPropertyName(POCOPropertyNames.DateTypeCode);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Year);
            AddPOCOPropertyName(POCOPropertyNames.Month);
            AddPOCOPropertyName(POCOPropertyNames.CurrencyId);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.AccountId = entityPM.AccountId;
                entityPOCO.DateTypeCode = entityPM.DateTypeCode;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Year = entityPM.Year;
                entityPOCO.Month = entityPM.Month;
                entityPOCO.CurrencyId = entityPM.CurrencyId;
            }
        }

        public void CustomPOCOToPM(GLAccountTotalByMonthPM entityPM, GLAccountTotalByMonth entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencySign);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LocalBalance);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CardId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountTypeCode);
            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                entityPM.CurrencyName = currency.EnglishName;
                entityPM.CurrencySign = currency.Sign;
            }

            if (entityPOCO.AccountId != null)
            {
                GLAccountQueryService queryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM gla = queryService.GetSinglePM(entityPOCO.AccountId, entityPOCO.Tenant);
                entityPM.AccountName = gla.LocalName;
                //entityPM.LocalBalance = gla.BalanceInLocalCurrency;
                entityPM.AccountTypeCode = gla.AccountTypeCode;

                CardQuery cardQuery = new CardQuery(entityPOCO.Tenant);
                CardList card = cardQuery.GetSingleByGLAccountId(entityPOCO.AccountId);
                entityPM.CardId = card == null ? null : card.Id;
                
            }


        }
   }


}
   