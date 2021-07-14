using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
  public  class GLAccountCardDataService
    {
       public GLAccountCardsDataPM gLAccountCardsDataPM;
       public   GLAccountPM cardGLaccount;
        //Card card;
        string cardId;
        int tenant;
        List<CardList> connectedCards;
        public GLAccountCardDataService(string  CardId,string glAccountId, int Tenant)
        {
            tenant = Tenant;
            cardId = CardId;
            cardGLaccount = GetGLaccount(glAccountId);
            gLAccountCardsDataPM = GetGLAccountCardsDataPM();
          
        }
        private List<CardList> GetGLAccountConnectedCards(GLAccountPM gLAccount)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            List<CardList> connectCards = cardQuery.GetCardPMsByGLAccountId(gLAccount.Id, tenant);
            return connectCards;
        }
        private GLAccountPM GetGLaccount(string accountId)
        {
            IGLAccountQueryServiceExt gLAccountQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            return gLAccountQueryServiceExt.GetSingleGLAccountPM(accountId, tenant);
        }
       
    

        public void CreateGLaccountCardsDara()
        {
            GLAccountCardsDataPM gLAccountCardsDataPM = CreateGLAccountCardsData();
            UpdateGLAccount(gLAccountCardsDataPM);
        }
     
        public void UpdateGLaccountCardsData()
        {
            MapGLAccountCardsDataFields(gLAccountCardsDataPM);
            gLAccountCardsDataPM.ChangeSetOp = ChangeSetOperation.Update;
            SaveChanges(gLAccountCardsDataPM);
        }
        private GLAccountCardsDataPM CreateGLAccountCardsData()
        {
            GLAccountCardsDataPM gLAccountCardsDataPM = new GLAccountCardsDataPM();

            MapGLAccountCardsDataFields(gLAccountCardsDataPM);
            gLAccountCardsDataPM.ChangeSetOp = ChangeSetOperation.Insert;
            SaveChanges(gLAccountCardsDataPM);
            return gLAccountCardsDataPM;
        }
        private void SaveChanges(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            IGLAccountCardsDataUpdateServiceExt IGLAccountCardsDataUpdateServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountCardsDataUpdateServiceExt), "GLAccountCardsDataUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountCardsDataUpdateServiceExt;
            IGLAccountCardsDataUpdateServiceExt.Update(gLAccountCardsDataPM);
        }
        private GLAccountCardsDataPM MapGLAccountCardsDataFields(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            gLAccountCardsDataPM.CollectorUserId = SetCollectorId();
            gLAccountCardsDataPM.CreditLimit = SetCreditLimit();
            gLAccountCardsDataPM.PaymentTermId = SetPaymentTerm();
            gLAccountCardsDataPM.Tenant = tenant;
            gLAccountCardsDataPM.SalesmanUserId = SetSalesmanUserId();
            gLAccountCardsDataPM.Phone = SetPhone(); 
            gLAccountCardsDataPM.VatNumber = SetVatNumber();
            gLAccountCardsDataPM.TotalOpenShipments = GetTotalOpenFilesAmount();
            gLAccountCardsDataPM.InsuredcreditLimit = GetInsuredCreditLimit();
            return gLAccountCardsDataPM;
        }

        private double? SetCreditLimit()
        {
            var cardWithCreditLimit = connectedCards.Where(d => d.CreditLimitAmount != null).Sum(d => d.CreditLimitAmount);
            if (cardWithCreditLimit == null) return null;
            else return cardWithCreditLimit;
        }
        private string SetCollectorId()
        {
            var cardWithCollector = connectedCards.Where(d => d.CollectorId != null).FirstOrDefault();
            if (cardWithCollector == null) return null;
            else return cardWithCollector.CollectorId;
        }
        private string SetPaymentTerm()
        {
            var cardWithPaymentTerm = connectedCards.Where(d => d.PaymentTermId != null).FirstOrDefault();
            if (cardWithPaymentTerm == null) return null;
            else return cardWithPaymentTerm.PaymentTermId;
        }
        private string SetSalesmanUserId()
        {
            var cardWithSalesman = connectedCards.Where(d => d.SalesmanUserId != null).FirstOrDefault();
            if (cardWithSalesman == null) return null;
            else return cardWithSalesman.SalesmanUserId;
        }
        private string SetVatNumber()
        {
            var cardWithVatNumber = connectedCards.Where(d => d.VatNumber != null).FirstOrDefault();
            if (cardWithVatNumber == null) return null;
            else return cardWithVatNumber.VatNumber;
        }
        private string SetPhone()
        {
            var cardWithPhone = connectedCards.Where(d => d.BusinessPhone != null).FirstOrDefault();
            if (cardWithPhone == null) return null;
            else return cardWithPhone.BusinessPhone;
        }
        private List<CustomerPM> GetCardsCustomers()
        {
            CustomerQuery customerQuery = new CustomerQuery(tenant);
            List<string> cardIds = connectedCards.Select(d => d.Id).ToList();
            return customerQuery.GetCustomersByCardsIds(cardIds, tenant);
        }
        //private double? GetCustomerCreditLimitAmount()
        //{
        //    CustomerPM customer = GetCustomer();
        //    if (customer == null) { return null; }
        //    return customer.CreditLimitAmount;
        //}
        private GLAccountPM GetGlAccountAccordingToCurrencyDiversity()
        {
            GLAccountCurrencyPM gLAccountCurrencyPM = GetGLAccountCurrency();
            if (gLAccountCurrencyPM == null) return cardGLaccount;
            GLAccountPM mainAccount = GetGLaccount(gLAccountCurrencyPM.MainGLAccountId);
            return mainAccount;
        }

        private void UpdateGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            if (cardGLaccount.IsMultiCurrency == false)
            {
            
                SetGLAccountCardsDataForSingleCurrencyGLAccount(gLAccountCardsDataPM);
            }
            else
            {
              
                SetGLAccountForMultiCurrencyGLAccount(gLAccountCardsDataPM);
            }

        }
        private void SetGLAccountForMultiCurrencyGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            UpdateSplitByCurrencyAccounts(cardGLaccount, gLAccountCardsDataPM);
            SetGLAccountCardsData(cardGLaccount, gLAccountCardsDataPM);
            SaveGLAccountChanges(cardGLaccount);
        }
        private void SetGLAccountCardsDataForSingleCurrencyGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            GLAccountPM glaccount = GetGlAccountAccordingToCurrencyDiversity();
            if(glaccount != cardGLaccount)
            {
                UpdateSplitByCurrencyAccounts(glaccount, gLAccountCardsDataPM);
            }
            SetGLAccountCardsData(glaccount, gLAccountCardsDataPM);
          
            SaveGLAccountChanges(glaccount);
        }
        private void UpdateSplitByCurrencyAccounts(GLAccountPM accountPM, GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            IGLAccountQueryServiceExt gLAccountQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            List<GLAccountPM> gLAccounts = gLAccountQueryServiceExt.GetSplittedByCurrencyGLAccounts(accountPM.Id, accountPM.Tenant).ToList();
            foreach (GLAccountPM gLAccount in gLAccounts)
            {
                SetGLAccountCardsData(gLAccount, gLAccountCardsDataPM);

                SaveGLAccountChanges(gLAccount);
            }
        }
        private void SetGLAccountCardsData(GLAccountPM accountPM, GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            accountPM.CardsDataId = gLAccountCardsDataPM.Id;
            cardGLaccount.CardsDataId = gLAccountCardsDataPM.Id;

        }
        private double? GetInsuredCreditLimit()
        {
            List<CustomerPM> customerPMs = GetCardsCustomers();
            return customerPMs.Where(d => d.InsuredcreditLimit !=null).Sum(d=> d.InsuredcreditLimit);
           
        }
        private decimal? GetTotalOpenFilesAmount()
        {
            List<CustomerOpenFilesAmountPM> customerOpenFilesAmountPMs = GetCustomerOpenFilesAmount();
            var customerTotalOpenFilesAmount = customerOpenFilesAmountPMs.Where(d => d.TotalOpenFilesAmount != 0).Sum(d => d.TotalOpenFilesAmount);
            if (customerTotalOpenFilesAmount == null) return null;
            else return customerTotalOpenFilesAmount;
        }
        private void SaveGLAccountChanges(GLAccountPM accountPM)
        {
            IGLAccountUpdateServiceExt glaccountUpdate = ContainerAccessor.Container.Resolve(typeof(IGLAccountUpdateServiceExt), "GLAccountUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountUpdateServiceExt;
            accountPM.ChangeSetOp = ChangeSetOperation.Update;
            glaccountUpdate.Update(accountPM);
        }

        private List<CustomerOpenFilesAmountPM> GetCustomerOpenFilesAmount()
        {
            CustomerOpenFilesAmountQuery customerOpenFilesAmount = new CustomerOpenFilesAmountQuery(tenant);
            List<string> cardIds = connectedCards.Select(d => d.Id).ToList();
         return customerOpenFilesAmount.GetCustomerOpenFilesByCustomerIds(cardIds, tenant);
        }

        private GLAccountCardsDataPM GetGLAccountCardsDataPM()
        {
            if (cardGLaccount.IsMultiCurrency == false)
            {
                gLAccountCardsDataPM = GetGLAccountCardsDataForSingleCurrencyGLAccount();
            }
            else
            {
                gLAccountCardsDataPM = GetGLAccountCardsDataForMultiCurrencyGLAccount();
            }
            return gLAccountCardsDataPM;
        }

        private GLAccountCardsDataPM GetGLAccountCardsDataForSingleCurrencyGLAccount()
        {

           GLAccountPM glaccount = GetGlAccountAccordingToCurrencyDiversity();
            connectedCards = GetGLAccountConnectedCards(glaccount);
           gLAccountCardsDataPM = GetGLAccountCardsData(glaccount);
            return gLAccountCardsDataPM;
        }
        private GLAccountCardsDataPM GetGLAccountCardsDataForMultiCurrencyGLAccount()
        {
            connectedCards = GetGLAccountConnectedCards(cardGLaccount);
            return GetGLAccountCardsData(cardGLaccount);
        }
        private GLAccountCardsDataPM GetGLAccountCardsData(GLAccountPM gLAccount)
        {
            IGLAccountCardsDataQueryServiceExt GLAccountCardsDataQueryService = ContainerAccessor.Container.Resolve(typeof(IGLAccountCardsDataQueryServiceExt), "GLAccountCardsDataQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountCardsDataQueryServiceExt;
            return GLAccountCardsDataQueryService.GetSingleGLAccountCardsData(gLAccount.CardsDataId, gLAccount.Tenant);
        }
        private GLAccountCurrencyPM GetGLAccountCurrency()
        {
            IGLAccountCurrencyQueryServiceExt gLAccountCurrencyQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountCurrencyQueryServiceExt), "GLAccountCurrencyQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountCurrencyQueryServiceExt;
            return gLAccountCurrencyQueryServiceExt.GetEntityByGLAccountId(cardGLaccount.Id, cardGLaccount.Tenant);
        }
    }
}
