
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class BankDepositDataMapping: IMapping<BankDepositPM, BankDeposit>
   {

        public void CustomPMToPOCO(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DepositBankAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CashBookId);

            if (entityPOCO.DepositBankAccountId != null)
                MapBankAccountFields(entityPM, entityPOCO);

            if (entityPOCO.CashBookId != null)
                MapCashbookFields(entityPM, entityPOCO);


            MapJournalFields(entityPM, entityPOCO);

            if (entityPOCO.DepositCurrencyId != null)
                MapCurrencyFields(entityPM, entityPOCO);

            if (entityPOCO.CreatedByUserId != null)
                MapContactFields(entityPM, entityPOCO);

        }

        private void MapContactFields(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            ContactQuery query = new ContactQuery(entityPOCO.Tenant);
            ContactPM contact = query.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            if (contact != null)
            {
                entityPM.CreatedByUserName = contact.LocalName;
            }
        }

        private void MapCurrencyFields(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(entityPOCO.Tenant);
            CurrencyPM currency = currencyQuery.GetSinglePM(entityPOCO.DepositCurrencyId, entityPOCO.Tenant);
            if (currency != null)
            {
                entityPM.DepositCurrencyCode = currency.Code;
            }
        }

        private void MapJournalFields(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            JournalQueryService journalQueryService = new JournalQueryService(entityPOCO.Tenant);

            JournalPM journal = journalQueryService
                .GetByAccountingEntityIdAndAccountingEntityCode(entityPOCO.Id, entityPM.IsCashDeposit ? "7" : "6", entityPOCO.Tenant);

            if (journal != null)
            {
                entityPM.JournalId = journal.Id;
                entityPM.JournalNumber = journal.JournalNumber;
                entityPM.JournalQueueId = journal.QueueId;
            }
        }

        private void MapCashbookFields(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            CashBookQueryService cashBookQueryService = new CashBookQueryService(entityPOCO.Tenant);
            CashBookPM cashBook = cashBookQueryService.GetLightCashbook(entityPOCO.CashBookId, entityPOCO.Tenant);
            if (cashBook != null)
            {
                entityPM.CashBookGLAccountId = cashBook.AccountId;
                entityPM.CashBookName = cashBook.LocalName;
                entityPM.IsCashDeposit = cashBook.CashBookTypeCode == "1";
            }
        }

        private void MapBankAccountFields(BankDepositPM entityPM, BankDeposit entityPOCO)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(entityPOCO.Tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetLightBankAccount(entityPOCO.DepositBankAccountId, entityPOCO.Tenant);
            if (bankAccount != null)
            {
                entityPM.DeferredGLAccountId = bankAccount.DeferredGLAccountId;
                entityPM.CashGLAccountId = bankAccount.GLAccountId;
                entityPM.BankAccountNumber = bankAccount.AccountNumber;
            }
        }

        private void BuildSearchFields(BankDepositPM entityPM, BankDeposit poco, bool isNewEntity)
        {
            string result = "";


            if (!string.IsNullOrEmpty(entityPM.DepositNumber.ToString()))
            {
                if (!(result.Split(',').Contains(entityPM.DepositNumber.ToString())))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.DepositNumber.ToString() : result + "," + entityPM.DepositNumber.ToString();
                }
            }

            // Get cheqeu Numbers from lines
            foreach(BankDepositLinePM line in entityPM.BankDepositLines)
            {
                if (line.ARPaymentChequeId != null)
                {
                    ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);
                    ARPaymentChequePM arPaymentCheque = aRPaymentChequeQueryService.GetSingle(line.ARPaymentChequeId, true, false);
                    if (arPaymentCheque != null)
                    {
                        result += "," + arPaymentCheque.ChequeNumber;
                        result += "," + arPaymentCheque.LocalAmount.ToString();
                        result += "," + arPaymentCheque.ForeignAmount.ToString();
                    }

                }
            }

            //foreach (JournalLinePM item in entityPM.BankDepositLines)
            //{
            //    if (!string.IsNullOrEmpty(item.Reference1))
            //    {

            //        if (!(result.Split(',').Contains(item.Reference1)))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
            //        }

            //    }

            //    if (!string.IsNullOrEmpty(item.Reference2))
            //    {
            //        if (!(result.Split(',').Contains(item.Reference2)))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(item.Reference3))
            //    {
            //        if (!(result.Split(',').Contains(item.Reference3)))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
            //        }
            //    }
            //}
            string resultWithoutDuplicate = RemoveDuplicateInSearchFields(result);
            entityPM.SearchFields = resultWithoutDuplicate;
            poco.SearchFields = resultWithoutDuplicate;

        }

        private string RemoveDuplicateInSearchFields(string result)
        {
            List<string> items = result.Split(',').ToList();
            List <string > array = new List<string > ();
            items.ForEach(item =>
            {
                if (!array.Any(x => x == item))
                {
                    array.Add(item);
                }
            });
            string searchValue = string.Join(",", array);
            return searchValue;

        }


   }


}
   