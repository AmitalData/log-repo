using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.Helpers
{
    public class AccountingSystemHelper
    {
        public AccountingSystemPM GetAccountingSystem(int tenant)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(new AccountingSettingRepository(context));
            AccountingSystemQuery accountingSystemQuery = new AccountingSystemQuery(new AccountingSystemRepository(context));

            AccountingSystemPM accountingSystem = null;
            AccountingSettingPM accountingSetting = accountingSettingQuery.GetSingleAccountSettingPM(tenant);

            if (accountingSetting != null)
            {
                string code = accountingSetting.AccountingSystemCode;
                accountingSystem = accountingSystemQuery.GetSingleAccountingSystemPM(code);
            }

            return accountingSystem;
        }

        public string GetGenericCreditAccount(string cardId, string currencyId, int tenant, bool isPayable)
        {
            string myResult = null;
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
            CardRepository cardRepository = new CardRepository(tenant);

            Card card = cardRepository.GetSingleCard(cardId, tenant);

            if (card != null && card.AccountingVATSplit)
            {
                CardExternalCodeByCurrency crdCurrenciesAccounting = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrencyAndTenant(cardId, currencyId, tenant);

                if (crdCurrenciesAccounting != null)
                {
                    myResult = isPayable == true ? crdCurrenciesAccounting.ExternalPayableTableId : crdCurrenciesAccounting.ExternalRecievableTableId;
                }
            }
            else
            {
                myResult = isPayable == true ? card.PayablesAccountingCard : card.ReceivablesAccountingCard;
            }

            return myResult;
        }
    }
}
