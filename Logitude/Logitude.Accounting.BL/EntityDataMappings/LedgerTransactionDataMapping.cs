
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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;



namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class LedgerTransactionDataMapping: IMapping<LedgerTransactionPM, LedgerTransaction>
   {

        public void CustomPMToPOCO(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;           
            }


            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

        }

        public void CustomPOCOToPM(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO)
        {



            RetrieveJournalFields(entityPM, entityPOCO);

            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                entityPM.CurrencyCode = currency.Code;
                entityPM.CurrencySign = currency.Sign;
            }
            else
            {
                entityPM.CurrencyCode = "";
                entityPM.CurrencySign = "";
            }
         

            this.CustomMappedPMProperties.Add(PMPropertyNames.OpenAmountCurrencyCode);
            if (entityPOCO.OpenAmountCurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.OpenAmountCurrencyId, entityPOCO.Tenant);
                entityPM.OpenAmountCurrencyCode = currency.Code;
                entityPM.OpenAmountCurrencySign = currency.Sign;
            }
            else
            {
                entityPM.OpenAmountCurrencyCode = "";
                entityPM.OpenAmountCurrencySign = "";
            }

            CustomMappedPMProperties.Add(PMPropertyNames.OppositeAccountEnglishName);
            CustomMappedPMProperties.Add(PMPropertyNames.OppositeAccountLocalName);
            CustomMappedPMProperties.Add(PMPropertyNames.OppositeAccountDisplayNumber);
            if (entityPOCO.OppositeAccountId != null)
            {
                GLAccountQueryService glaQuery = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM gla = glaQuery.GetSinglePM(entityPOCO.OppositeAccountId, entityPOCO.Tenant);
                entityPM.OppositeAccountEnglishName = gla.EnglishName;
                entityPM.OppositeAccountLocalName = gla.LocalName;
                entityPM.OppositeAccountDisplayNumber = gla.DisplayNumber;
            }

            // GET reconciliation no. of reconciled LT
            ReconciliationQueryService recoQuery = new ReconciliationQueryService(entityPM.Tenant);
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(entityPM.Tenant);
            if (entityPM.IsReconciled)
            {
                // get reco. line
                CustomMappedPMProperties.Add(PMPropertyNames.RecoNumber);
                ReconciliationLine recoLine = recoLineQuery.GetLineByTransactionId(entityPM.Id, entityPM.Tenant);
                
                // get reconciliaiton
                if (recoLine != null)
                {
                    var reco = recoQuery.GetSingle(recoLine.ReconciliationId, false, false);
                    entityPM.RecoNumber = reco.Number;
                    entityPM.ReconciliationId = reco.Id;
                }
            }

        }

        private static void RetrieveJournalFields(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO)
        {
            var JournalId="";
            int Tenant;
            if (entityPOCO != null && 
                !String.IsNullOrWhiteSpace(entityPOCO.JournalId)
                )
            {
                JournalId=entityPOCO.JournalId;
                Tenant = entityPOCO.Tenant;
            }
            else
            {
                JournalId = entityPM.JournalId;
                Tenant = entityPM.Tenant;
            }
            JournalQueryService journalQueryService = new JournalQueryService(Tenant);
            JournalPM parent = journalQueryService.GetSingle(JournalId, false, false);
            entityPM.Source = parent.AccountingEntityId;
            entityPM.SourceType = parent.AccountingEntityName;
            entityPM.JournalNumber = parent.JournalNumber;

            
            entityPM.SourceId = parent.AccountingEntityId; // hidden id to use in link
            entityPM.SourceNumber = parent.AccountingEntityReference; // display number
            entityPM.SourceTypeCode = parent.AccountingEntityCode; // source type code from AccountingEntities

            entityPM.OriginalJournalId = parent.OriginalJournalId;
        }





        private static void BuildSearchFields(LedgerTransactionPM entityPM, LedgerTransaction poco, bool isNewEntity)
        {
            string result = "";

            if (string.IsNullOrEmpty(entityPM.JournalNumber))
            {
                RetrieveJournalFields(entityPM, poco);
            }

            if (!string.IsNullOrEmpty(entityPM.JournalNumber))
            {
                if (!(result.Split(',').Contains(entityPM.JournalNumber)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.JournalNumber : result + "," + entityPM.JournalNumber;
                }

                JournalQueryService journalQueryService = new JournalQueryService(entityPM.Tenant);
                JournalPM parent = journalQueryService.GetSingle(entityPM.JournalId, false, false);
                string accountingEntityReference = parent.AccountingEntityReference;

                if (!(result.Split(',').Contains(accountingEntityReference)))
                {
                    result = string.IsNullOrEmpty(result) ? accountingEntityReference : result + "," + accountingEntityReference;
                }

            }

            if (!string.IsNullOrEmpty(entityPM.Reference1))
            {

                if (!(result.Split(',').Contains(entityPM.Reference1)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.Reference1 : result + "," + entityPM.Reference1;
                }

            }


            if (!string.IsNullOrEmpty(entityPM.Reference2))
            {
                if (!(result.Split(',').Contains(entityPM.Reference2)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.Reference2 : result + "," + entityPM.Reference2;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Reference3))
            {
                if (!(result.Split(',').Contains(entityPM.Reference3)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.Reference3 : result + "," + entityPM.Reference3;
                }
            }




            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }


   }


}
   