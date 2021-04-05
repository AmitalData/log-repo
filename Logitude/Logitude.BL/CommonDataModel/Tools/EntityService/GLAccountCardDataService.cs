using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
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
        Card card;
        int tenant;
        public GLAccountCardDataService(Card Card, int Tenant)
        {
            tenant = Tenant;
            card = Card;
            cardGLaccount = GetGLaccount(card.GLAccountId);
            gLAccountCardsDataPM = GetGLAccountCardsDataPM();
          
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
        public void UpdateGLaccountCardsDara()
        {
            UpdategLAccountCardsDataPM();
        }
        private void UpdategLAccountCardsDataPM()
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
            gLAccountCardsDataPM.CollectorUserId = card.CollectorId;
            gLAccountCardsDataPM.CreditLimit = GetCustomerCreditLimitAmount();
            gLAccountCardsDataPM.PaymentTermId = card.PaymentTermId;
            gLAccountCardsDataPM.Tenant = card.Tenant;
            gLAccountCardsDataPM.SalesmanUserId = card.SalesmanUserId;
            gLAccountCardsDataPM.Phone = card.Phone;
            gLAccountCardsDataPM.VatNumber = card.VatNumber;
            gLAccountCardsDataPM.TotalOpenShipments = GetTotalOpenFilesAmount();
            return gLAccountCardsDataPM;
        }
        private CustomerPM GetCustomer()
        {
            CustomerQuery customerQuery = new CustomerQuery(tenant);
            return customerQuery.GetSinglePM(card.Id, tenant);
        }
        private double? GetCustomerCreditLimitAmount()
        {
            CustomerPM customer = GetCustomer();
            if (customer == null) { return null; }
            return customer.CreditLimitAmount;
        }
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
            SetGLAccountCardsData(cardGLaccount, gLAccountCardsDataPM);
            SaveGLAccountChanges(cardGLaccount);
        }
        private void SetGLAccountCardsDataForSingleCurrencyGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            GLAccountPM glaccount = GetGlAccountAccordingToCurrencyDiversity();
            SetGLAccountCardsData(glaccount, gLAccountCardsDataPM);
          
            SaveGLAccountChanges(glaccount);
        }

        private void SetGLAccountCardsData(GLAccountPM accountPM, GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            accountPM.CardsDataId = gLAccountCardsDataPM.Id;        
        }
        private decimal? GetTotalOpenFilesAmount()
        {
            CustomerOpenFilesAmountPM customerOpenFilesAmountPM = GetCustomerOpenFilesAmount();
            if (customerOpenFilesAmountPM == null) { return (decimal)0.0; }
            else { return customerOpenFilesAmountPM.TotalOpenFilesAmount; }
        }
        private void SaveGLAccountChanges(GLAccountPM accountPM)
        {
            IGLAccountUpdateServiceExt glaccountUpdate = ContainerAccessor.Container.Resolve(typeof(IGLAccountUpdateServiceExt), "GLAccountUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountUpdateServiceExt;
            accountPM.ChangeSetOp = ChangeSetOperation.Update;
            glaccountUpdate.Update(accountPM);
        }

        private CustomerOpenFilesAmountPM GetCustomerOpenFilesAmount()
        {
            CustomerOpenFilesAmountQuery customerOpenFilesAmount = new CustomerOpenFilesAmountQuery(tenant);
            return customerOpenFilesAmount.GetSinglePMByCustomerId(card.Id, card.Tenant);
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
           gLAccountCardsDataPM = GetGLAccountCardsData(glaccount);
            return gLAccountCardsDataPM;
        }
        private GLAccountCardsDataPM GetGLAccountCardsDataForMultiCurrencyGLAccount()
        {
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
