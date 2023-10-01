
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
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class JournalLineDataMapping: IMapping<JournalLinePM, JournalLine>
   {

        public void CustomPMToPOCO(JournalLinePM entityPM, JournalLine entityPOCO)
        {

          
            
            

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                CustomMappedPOCOProperties.Add(POCOPropertyNames.AccountingDate);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.ActionCode);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalId);

                entityPOCO.Line = entityPM.Line;
                entityPOCO.JournalId = entityPM.JournalId;
                entityPOCO.AccountingDate = entityPM.AccountingDate;
                entityPOCO.ActionCode = entityPM.ActionCode;
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(JournalLinePM entityPM, JournalLine entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ActionCode);

            CustomMappedPOCOProperties.Add(POCOPropertyNames.CurrencyId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreditAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreditControlAccountId);           
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DebitAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DebitControlAccountId);
            var accContext = AccountingContext.GetContext(entityPOCO.Tenant);
            if (entityPOCO.ActionCode != null)
            {
                JournalActionTypeQueryService journalActionTypeQueryService = new JournalActionTypeQueryService(accContext/*entityPOCO.Tenant*/);
                JournalActionTypePM action = journalActionTypeQueryService.GetSingle(entityPOCO.ActionCode, false, true/*false*/);
                if (action != null)
                {
                    entityPM.ActionName = action.LocalName;
                    entityPM.ActionTypeCode = action.Code;
                }
            }


            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant/*accContext*/);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                entityPM.CurrencyCode = currency.Code;
                entityPM.CurrencyName = currency.EnglishName;
             
            }

          
            if (entityPOCO.CreditAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(/*entityPOCO.Tenant*/accContext);
                GLAccountPM parent = gLAccountQueryService.GetSingle(entityPOCO.CreditAccountId, false, true/*false*/);
                entityPM.CreditAccountName = parent.LocalName;
                entityPM.CreditAccountEnglishName = parent.EnglishName;
                entityPM.CreditAccountNumber = parent.DisplayNumber;
                entityPM.CreditAccountCOACode = parent.ChartOfAccountsTypeCode;
            }

            if (entityPOCO.CreditControlAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(/*entityPOCO.Tenant*/accContext);
                GLAccountPM parent = gLAccountQueryService.GetSingle(entityPOCO.CreditControlAccountId, false, true/*false*/);
                entityPM.CreditControlAccountName = parent.LocalName;
                entityPM.CreditControlAccountNumber = parent.InternalNumber;
            }
            if (entityPOCO.DebitAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(/*entityPOCO.Tenant*/accContext);
                GLAccountPM parent = gLAccountQueryService.GetSingle(entityPOCO.DebitAccountId, false, true/*false*/);
                entityPM.DebitAccountName = parent.LocalName;
                entityPM.DebitAccountEnglishName = parent.EnglishName;
                entityPM.DebitAccountNumber = parent.DisplayNumber;
                entityPM.DebitAccountCOACode = parent.ChartOfAccountsTypeCode;


            }
            if (entityPOCO.DebitControlAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(/*entityPOCO.Tenant*/accContext);
                GLAccountPM parent = gLAccountQueryService.GetSingle(entityPOCO.DebitControlAccountId, false, true/*false*/);
                entityPM.DebitControlAccountName = parent.LocalName;
                entityPM.DebitControlAccountNumber = parent.InternalNumber;
            }
        }
   }


}
   