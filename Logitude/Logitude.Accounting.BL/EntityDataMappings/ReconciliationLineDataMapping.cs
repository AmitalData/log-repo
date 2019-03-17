using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;


namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ReconciliationLineDataMapping: IMapping<ReconciliationLinePM, ReconciliationLine>
   {

        public void CustomPMToPOCO(ReconciliationLinePM entityPM, ReconciliationLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.ReconciliationId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.SearchFields);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ReconciliationId = entityPM.ReconciliationId;
                entityPOCO.Line = entityPM.Line;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.SearchFields = entityPM.SearchFields;

            }
        }

        public void CustomPOCOToPM(ReconciliationLinePM entityPM, ReconciliationLine entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencySign);

            this.CustomMappedPMProperties.Add(PMPropertyNames.DueDate);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ForeignAmountCredit);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ForeignAmountDebit);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Reference1);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Reference2);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Reference3);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Notes);
            this.CustomMappedPMProperties.Add(PMPropertyNames.JournalNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.JournalId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.JournalId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.JournalId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SearchFields);

            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);

                if (currency != null)
                {
                    entityPM.CurrencyName = currency.EnglishName;
                    entityPM.CurrencyCode = currency.Code;
                    entityPM.CurrencySign = currency.Sign;
                }
            }

            // This properties only to view it on Reconciliatio OPC
            if (entityPOCO.TransactionId != null)
            {
                LedgerTransactionQueryService queryService = new LedgerTransactionQueryService(entityPOCO.Tenant);
                LedgerTransactionPM transaction = queryService.GetSingle(entityPOCO.TransactionId, false,false);

                if (transaction != null)
                {
                    entityPM.CreateDate = transaction.CreateDate;
                    entityPM.DueDate = transaction.DueDate;
                    entityPM.ForeignAmountCredit = transaction.ForeignAmountCredit;
                    entityPM.ForeignAmountDebit = transaction.ForeignAmountDebit;
                    entityPM.Reference1 = transaction.Reference1;
                    entityPM.Reference2 = transaction.Reference2;
                    entityPM.Reference3 = transaction.Reference3;
                    entityPM.Notes = transaction.Notes;
                    entityPM.JournalNumber = transaction.JournalNumber;
                    entityPM.JournalId = transaction.JournalId;
                    entityPM.OpenAmountCurrencySign = transaction.OpenAmountCurrencySign;
                    entityPM.SearchFields = transaction.SearchFields;
                }
            }
        }
   }


}
   