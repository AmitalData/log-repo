using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.BL.EntityQueryServiceExt;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
    public partial class GLAccountQueryService
    {


        public GLAccount GLAccountDataMappingAndValidatin(GLAccountPM MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new GLAccount();
                //if (!string.IsNullOrEmpty(MyEntity.Id))
                //{
                //    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                //}

                //if (temp == null)
                //{
                //    throw new ApplicationException("GLAccount with Id " + MyEntity.Id + " doesn't exist");
                //}
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                temp.Tenant = MyEntity.Tenant;
                temp.InternalNumber = MyEntity.InternalNumber;
                GLAccountTypeQueryService GLAccountTypeGLAccountTypeService = new GLAccountTypeQueryService(Tenant);
                if (MyEntity.AccountTypeCode != null)
                {
                    var myGLAccountTypePM = GLAccountTypeGLAccountTypeService.GetGLAccountTypeByCode(MyEntity.AccountTypeCode, Tenant);
                    if (myGLAccountTypePM != null)
                    {
                        temp.GLAccountType = new GLAccountType();
                        temp.GLAccountType.Code = myGLAccountTypePM.Code;
                        temp.GLAccountType.LocalName = myGLAccountTypePM.LocalName;
                        temp.GLAccountType.EnglishName = myGLAccountTypePM.EnglishName;



                    }

                }


                temp.DisplayNumber = MyEntity.DisplayNumber;
                temp.LocalName = MyEntity.LocalName;
                temp.EnglishName = MyEntity.EnglishName;
                temp.IsMultiCurrency = MyEntity.IsMultiCurrency;
                CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
                if (MyEntity.CurrencyId  != null)
                {
                    var myCurrencyPM = CurrencyCurrencyService.GetCurrencyById(MyEntity.CurrencyId , Tenant);
                    if (myCurrencyPM != null)
                    {
                        temp.Currency = new Currency();
                        temp.Currency.Id = myCurrencyPM.Id;
                        temp.Currency.Code = myCurrencyPM.Code;
                        temp.Currency.EnglishName = myCurrencyPM.EnglishName;
                        temp.Currency.LocalName = myCurrencyPM.LocalName;
                    }

                }


              //  temp.IsControlAccount = MyEntity.IsControlAccount;
                ChartOfAccountQueryService ChartOfAccountChartOfAccountService = new ChartOfAccountQueryService(Tenant);
                if (MyEntity.ChartOfAccountsId != null)
                {
                    var myChartOfAccountPM = ChartOfAccountChartOfAccountService.GetChartOfAccountById(MyEntity.ChartOfAccountsId, Tenant);
                    if (myChartOfAccountPM != null)
                    {
                        temp.ChartOfAccount = new ChartOfAccount();
                        temp.ChartOfAccount.EnglishName = myChartOfAccountPM.EnglishName;
                        temp.ChartOfAccount.Id = myChartOfAccountPM.Id;
                        temp.ChartOfAccount.LogitudeCode = myChartOfAccountPM.LogitudeCode;
                        

                    }

                }


                temp.Inactive = MyEntity.Inactive;
                temp.AccountTypeName = MyEntity.AccountTypeName;
                temp.CurrencyName = MyEntity.CurrencyName;
                //temp.RevenueExpenseName = MyEntity.RevenueExpenseName;
                temp.ChartOfAccountsName = MyEntity.ChartOfAccountsName;
                ChartOfAccountsTypeQueryService ChartOfAccountsTypeChartOfAccountsTypeService = new ChartOfAccountsTypeQueryService(Tenant);
                if (MyEntity.ChartOfAccountsTypeCode  != null)
                {
                    var myChartOfAccountsTypePM = ChartOfAccountsTypeChartOfAccountsTypeService.GetChartOfAccountsTypeByCode(MyEntity.ChartOfAccountsTypeCode , Tenant);
                    if (myChartOfAccountsTypePM != null)
                    {
                        temp.ChartOfAccountsType = new ChartOfAccountsType();
                        temp.ChartOfAccountsType.LocalName = myChartOfAccountsTypePM.LocalName;
                        temp.ChartOfAccount.EnglishName = myChartOfAccountsTypePM.EnglishName;
                        temp.ChartOfAccountsType.Code = myChartOfAccountsTypePM.Code;

                    }

                }


                temp.ChartOfAccountsTypeName = MyEntity.ChartOfAccountsTypeName;
                temp.CurrencyCode = MyEntity.CurrencyCode;
                ReconcileMethodQueryService ReconcileMethodCodeReconcileMethodService = new ReconcileMethodQueryService(Tenant);
                if (MyEntity.ReconcileMethodCode  != null)
                {
                    var myReconcileMethodCodePM = ReconcileMethodCodeReconcileMethodService.GetReconcileMethodByCode(MyEntity.ReconcileMethodCode, Tenant);
                    if (myReconcileMethodCodePM != null)
                    {
                        temp.ReconcileMethod  = new ReconcileMethod();
                        temp.ReconcileMethod.Code = myReconcileMethodCodePM.Code;
                        temp.ReconcileMethod.LocalName = myReconcileMethodCodePM.LocalName;
                        temp.ReconcileMethod.EnglishName = myReconcileMethodCodePM.EnglishName;
                       

                        
                    }

                }


                temp.ReconcileMethodName = MyEntity.ReconcileMethodName;
                GLAccountQueryService ControlAccountGLAccountService = new GLAccountQueryService(Tenant);
                if (MyEntity.ControlAccountId != null)
                {
                    var myControlAccountPM = ControlAccountGLAccountService.GetGLAccountById(MyEntity.ControlAccountId, Tenant);
                    if (myControlAccountPM != null)
                    {
                        temp.ControlAccount = new GLAccount();
                        temp.ControlAccount.LocalName = myControlAccountPM.LocalName;
                        temp.ControlAccount.EnglishName = myControlAccountPM.EnglishName;
                        temp.ControlAccount.Id = myControlAccountPM.Id;
                    }

                }


                temp.ControlAccountName = MyEntity.ControlAccountName;
                temp.ControlAccountNumber = MyEntity.ControlAccountNumber;
                temp.ActiveStatusName = MyEntity.ActiveStatusName;
                temp.OldCurrencyId = MyEntity.OldCurrencyId;
                temp.OldIsMultiCurrency = MyEntity.OldIsMultiCurrency;
                AutomaticReconcileMethodQueryService AutomaticReconcileAutomaticReconcileMethodService = new AutomaticReconcileMethodQueryService(Tenant);
                if (MyEntity.AutomaticReconcileId != null)
                {
                    var myAutomaticReconcilePM = AutomaticReconcileAutomaticReconcileMethodService.GetAutomaticReconcileMethodById(MyEntity.AutomaticReconcileId, Tenant );
                    if (myAutomaticReconcilePM != null)
                    {
                        temp.AutomaticReconcileMethod = new AutomaticReconcileMethod();
                        temp.AutomaticReconcileMethod.Id = myAutomaticReconcilePM.Id;
                        temp.AutomaticReconcileMethod.LocalName = myAutomaticReconcilePM.LocalName;
                        temp.AutomaticReconcileMethod.LogitudeCode = myAutomaticReconcilePM.LogitudeCode;
                        temp.AutomaticReconcileMethod.Name = myAutomaticReconcilePM.Name;

                    }

                }


                temp.AutomaticReconcileName = MyEntity.AutomaticReconcileName;
                temp.PreviousEnglishName = MyEntity.PreviousEnglishName;
                //temp.PreviousEnglishNameChangeDate = MyEntity.PreviousEnglishNameChangeDate;
                temp.PreviousLocalName = MyEntity.PreviousLocalName;
                //temp.PreviousLocalNameChangeDate = MyEntity.PreviousLocalNameChangeDate;
                temp.PreviousNumber = MyEntity.PreviousNumber;
                //temp.PreviousNumberChangeDate = MyEntity.PreviousNumberChangeDate;
                ChartOfAccountQueryService PreviousChartOfAccountChartOfAccountService = new ChartOfAccountQueryService(Tenant);
                if (MyEntity.PreviousChartOfAccountsId != null)
                {
                    var myPreviousChartOfAccountPM = PreviousChartOfAccountChartOfAccountService.GetChartOfAccountById(MyEntity.PreviousChartOfAccountsId, Tenant );
                    if (myPreviousChartOfAccountPM != null)
                    {
                        temp.PreviousChartOfAccount = new ChartOfAccount();
                        temp.PreviousChartOfAccount.Id = myPreviousChartOfAccountPM.Id;
                        temp.PreviousChartOfAccount.LocalName = myPreviousChartOfAccountPM.LocalName;
                        temp.PreviousChartOfAccount.LogitudeCode = myPreviousChartOfAccountPM.LogitudeCode;
                     
                    }

                }


                //temp.PreviousChartOfAccountsChangeDate = MyEntity.PreviousChartOfAccountsChangeDate;
                GLAccountQueryService CustomerGLAccountGLAccountService = new GLAccountQueryService(Tenant);
                if (MyEntity.CustomerGLAccountId != null)
                {
                    var myCustomerGLAccountPM = CustomerGLAccountGLAccountService.GetGLAccountById(MyEntity.CustomerGLAccountId, Tenant);
                    if (myCustomerGLAccountPM != null)
                    {
                        temp.CustomerGLAccount = new GLAccount();
                        temp.CustomerGLAccount.Id = myCustomerGLAccountPM.Id;
                        temp.CustomerGLAccount.LocalName = myCustomerGLAccountPM.LocalName;
                        temp.CustomerGLAccount.EnglishName = myCustomerGLAccountPM.EnglishName;
                    }

                }


                temp.CustomerGLAccountName = MyEntity.CustomerGLAccountName;
                temp.CustomerGLAccountNumber = MyEntity.CustomerGLAccountNumber;
                temp.BalanceInLocalCurrency = MyEntity.BalanceInLocalCurrency;
                temp.RevaluationEnabled = MyEntity.RevaluationEnabled;
                GLAccountQueryService ParentAccountGLAccountService = new GLAccountQueryService(Tenant);
                if (!string.IsNullOrEmpty( MyEntity.ParentAccountId ))
                {
                    var myParentAccountPM = ParentAccountGLAccountService.GetGLAccountById(MyEntity.ParentAccountId, Tenant );
                    if (myParentAccountPM != null)
                    {
                        temp.ParentAccount = new GLAccount();
                        temp.ParentAccount.Id = myParentAccountPM.Id;
                        temp.ParentAccount.LocalName = myParentAccountPM.LocalName;
                        temp.ParentAccount.EnglishName = myParentAccountPM.EnglishName;
                    }

                }


                temp.ParentAccountName = MyEntity.ParentAccountName;
                temp.ParentAccountNumber = MyEntity.ParentAccountNumber;
                temp.CustomerGLAccountInternalNumber = MyEntity.CustomerGLAccountInternalNumber;
                Category1QueryService Category1Category1Service = new Category1QueryService(Tenant);
                if (MyEntity.Category1Id != null)
                {
                    var myCategory1PM = Category1Category1Service.GetCategory1ById(MyEntity.Category1Id, Tenant);
                    if (myCategory1PM != null)
                    {
                        temp.Category1 = new Category1();
                        temp.Category1.Id = myCategory1PM.Id;
                        temp.Category1.EnglishName = myCategory1PM.EnglishName;
                        temp.Category1.LocalName = myCategory1PM.LocalName;
                       
                    }

                }


                temp.Category1Name = MyEntity.Category1Name;
                Category2QueryService Category2Category2Service = new Category2QueryService(Tenant);
                if (MyEntity.Category2Id != null)
                {
                    var myCategory2PM = Category2Category2Service.GetCategory2ById(MyEntity.Category2Id, Tenant);
                    if (myCategory2PM != null)
                    {
                        temp.Category2 = new Category2();
                        temp.Category2.Id = myCategory2PM.Id;
                        temp.Category2.EnglishName = myCategory2PM.EnglishName;
                        temp.Category2.LocalName = myCategory2PM.LocalName;
                    }

                }


                temp.Category2Name = MyEntity.Category2Name;
                Category3QueryService Category3Category3Service = new Category3QueryService(Tenant);
                if (MyEntity.Category3Id != null)
                {
                    var myCategory3PM = Category3Category3Service.GetCategory3ById(MyEntity.Category3Id, Tenant);
                    if (myCategory3PM != null)
                    {
                        temp.Category3 = new Category3();
                        temp.Category3.Id = myCategory3PM.Id;
                        temp.Category3.EnglishName = myCategory3PM.EnglishName;
                        temp.Category3.LocalName = myCategory3PM.LocalName;
                    }

                }


                temp.Category3Name = MyEntity.Category3Name;
                Category4QueryService Category4Category4Service = new Category4QueryService(Tenant);
                if (MyEntity.Category4Id != null)
                {
                    var myCategory4PM = Category4Category4Service.GetCategory4ById(MyEntity.Category4Id, Tenant );
                    if (myCategory4PM != null)
                    {
                        temp.Category4 = new Category4();
                        temp.Category4.Id = myCategory4PM.Id;
                        temp.Category4.EnglishName = myCategory4PM.EnglishName;
                        temp.Category4.LocalName = myCategory4PM.LocalName;
                    }

                }


                temp.Category4Name = MyEntity.Category4Name;
                Category5QueryService Category5Category5Service = new Category5QueryService(Tenant);
                if (MyEntity.Category5Id != null)
                {
                    var myCategory5PM = Category5Category5Service.GetCategory5ById(MyEntity.Category5Id, Tenant);
                    if (myCategory5PM != null)
                    {
                        temp.Category5 = new Category5();
                        temp.Category5.Id = myCategory5PM.Id;
                        temp.Category5.EnglishName = myCategory5PM.EnglishName;
                        temp.Category5.LocalName = myCategory5PM.LocalName;
                    }

                }


                temp.Category5Name = MyEntity.Category5Name;
                temp.IsVATExempt = MyEntity.IsVATExempt;
                temp.ChartOfAccountsCode = MyEntity.ChartOfAccountsCode;
                temp.CustomerCode = MyEntity.CustomerCode;
                temp.ParentAccountByCurrency = MyEntity.ParentAccountByCurrency;
                temp.VatNumber = MyEntity.VatNumber;
                temp.PaymentTermId = MyEntity.PaymentTermId;
                UserQueryService CollectorUserService = new UserQueryService(Tenant);
                if (MyEntity.CollectorId != null)
                {
                    var myCollectorPM = CollectorUserService.GetUserById(MyEntity.CollectorId, Tenant);
                    if (myCollectorPM != null)
                    {
                        temp.Collector = new User();
                        temp.Collector.Id = myCollectorPM.Id;
                        temp.Collector.LocalName = myCollectorPM.LocalName;
                        temp.Collector.EnglishName = myCollectorPM.EnglishName;

                    }

                }

                RevenueExpenseTypeQueryService RevenueExpenseTypeRevenueExpenseTypeService = new RevenueExpenseTypeQueryService(Tenant);
                if (MyEntity.RevenueExpenseType != null)
                {
                    var myRevenueExpenseTypePM = RevenueExpenseTypeRevenueExpenseTypeService.GetRevenueExpenseTypeByCode(MyEntity.RevenueExpenseType, Tenant);
                    if (myRevenueExpenseTypePM != null)
                    {
                        temp.RevenueExpenseType = new RevenueExpenseType();
                        temp.RevenueExpenseType.Code = myRevenueExpenseTypePM.Code;
                        temp.RevenueExpenseType.LocalName = myRevenueExpenseTypePM.LocalName;
                        temp.RevenueExpenseType.EnglishName = myRevenueExpenseTypePM.EnglishName;
                    }

                }
                temp.SalesmanUserId = MyEntity.SalesmanUserId;
                temp.NewGLAccountCardId = MyEntity.NewGLAccountCardId;
                temp.LocalBalanceInDue = MyEntity.LocalBalanceInDue;
                temp.NextDueDate = MyEntity.NextDueDate;
                temp.CurrencySign = MyEntity.CurrencySign;
                temp.ConnectedItems = MyEntity.ConnectedItems;
                temp.Type = MyEntity.Type;
                temp.DeductionFileTypeId = MyEntity.DeductionFileTypeId;
                temp.DeductionFileNumber = MyEntity.DeductionFileNumber;
                temp.AssessingOfficeCode = MyEntity.AssessingOfficeCode;
                temp.Occupation = MyEntity.Occupation;
                temp.DeductionTypeId = MyEntity.DeductionTypeId;
                temp.ConsolidationVat = MyEntity.ConsolidationVat;
              
                if (MyEntity.Parent != null)
                {

                    temp.Parent = MyEntity.DisplayNumber;
                 CheckParentCurrency(MyEntity);


                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public GLAccount GLAccountCustomDataMapping(string id, int tenant)
        {
            try
            {
                var temp = new GLAccount();
                GLAccountPM gLAccountPM = null;
                if (!string.IsNullOrEmpty(id))
                {
                    gLAccountPM = query.GetSinglePM(id, tenant);
                }

                if (gLAccountPM == null)
                {
                    throw new ApplicationException("GLAccount with Id " + id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(id))
                {
                    temp.Id = id;
                }
                temp.Tenant = tenant;
                temp.InternalNumber = gLAccountPM.InternalNumber;
            

                temp.DisplayNumber = gLAccountPM.DisplayNumber;
                temp.LocalName = gLAccountPM.LocalName;
                temp.EnglishName = gLAccountPM.EnglishName;
                temp.IsMultiCurrency = gLAccountPM.IsMultiCurrency;
        


             
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public GLAccountPM GLAccountCustomDataMappingAndValidatin(GLAccount entity, int tenant)
        {
            GLAccountPM entityPM = null;
            if(entity.InternalNumber != null)
            {
                entityPM = query.GetSinglePMByInternalNumber(entity.InternalNumber, tenant);
            }
          

            if (entityPM == null)
            {
                throw new Exception("GLAccount with internal number " + entity.InternalNumber + " does not exist");
            }

            return entityPM;
        }

        public void CheckParentCurrency(GLAccountPM MyEntity)
        {
            Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService accountQueryService = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(MyEntity.Tenant);
            GLAccountPM parentGLAccount = accountQueryService.GetSinglePMByDisplayNumber(MyEntity.Parent, MyEntity.Tenant);
            if (parentGLAccount != null)
            {

                if (parentGLAccount.IsMultiCurrency == false)
                {
                    throw new Exception(TextCodesTranslator.TranslateText("GLAccounts.O.MustBeMultiCurrency", MyEntity.Tenant));
                }

                else
                {
                    GLAccountCurrencyQueryService accountcurrencyQueryService = new GLAccountCurrencyQueryService(MyEntity.Tenant);
                    GLAccountCurrencyPM gLAccountCurrencyPM = accountcurrencyQueryService.GetEntityByCurrencyAndGLAccountId(parentGLAccount.Id, MyEntity.CurrencyId, parentGLAccount.Tenant);

                    CurrencyQueryService currencyQueryService = new CurrencyQueryService(MyEntity.Tenant);
                    Currency currency = currencyQueryService.GetCurrencyById(MyEntity.CurrencyId, parentGLAccount.Tenant);
                    if (gLAccountCurrencyPM != null)
                    {
                        throw new Exception("The parent GLAccount(" + parentGLAccount.DisplayNumber + ") already has split GLAccount with currency (" + currency.Code + ")");
                    }
                    else
                    {
                        IAccountingContext accountingContext = AccountingContext.GetContext(MyEntity.Tenant);
                        GLAccountCurrencyUpdateService updateService = new GLAccountCurrencyUpdateService(accountingContext, new Dictionary<string, IContext>(), MyEntity.Tenant);

                        GLAccountCurrencyPM newGLAccountCurrency = new GLAccountCurrencyPM()
                        {

                            CurrencyId = MyEntity.CurrencyId,
                            MainGLAccountId = parentGLAccount.Id,

                            GLAccountId = MyEntity.Id,

                            Tenant = MyEntity.Tenant,
                            ChangeSetOp = ChangeSetOperation.Insert,

                        };

                        updateService.Update(newGLAccountCurrency, true);
                    }
                }

            }
        }


        public GLAccount GetGLAccountByDisplayNumber(string number , int tenant)
        {
            Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService queryService = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(tenant);
            GLAccountPM gLAccountPM = queryService.GetSinglePMByDisplayNumber(number, tenant);
            return GLAccountDataMappingAndValidatin(gLAccountPM, tenant);





        }

        public GLAccount GetGLAccountByInternalNumber(string number, int tenant)
        {
            Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService queryService = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(tenant);
            GLAccountPM gLAccountPM = queryService.GetSinglePMByInternalNumber(number, tenant);

            if (gLAccountPM == null)
            {
                throw new Exception("GLAccount with internal number " + number + " does not exist");
            }
            return GLAccountDataMappingAndValidatin(gLAccountPM, tenant);





        }
        public List<Card> GetGLAccountCards(GLAccount gLAccount)
        {
        return MapGLAccountCardFields(gLAccount);
         
        }
        private List<Card> MapGLAccountCardFields(GLAccount gLAccount)
        {
            List<CardList> cardLists = GetCardsByGLAccountId(gLAccount.Id, gLAccount.Tenant);

            List<Card> cards = new List<Card>();
            foreach (CardList card in cardLists)
            {
                Card connectedCard = new Card();
                connectedCard.Code = card.Code;
                connectedCard.PartnerCode = card.PartnerTypeId;
                connectedCard.LocalName = card.LocalName;
                cards.Add(connectedCard);

            }
            return cards;
        

        }
        private List<CardList> GetCardsByGLAccountId(string id, int tenant)
        {

            CardQuery cardQuery = new CardQuery(tenant);
            return cardQuery.GetCardPMsByGLAccountId(id, tenant);

        }

        public GLAccountPM GLAccountDataMappingAndValidatinForExternalAPI(GLAccount glaccount, int tenant)
        {
            CheckIfTheCustomerGLaccountIsSplitGLAccount(glaccount, tenant);

            return GLAccountDataMappingAndValidatin(glaccount, tenant);

        }

        private void CheckIfTheCustomerGLaccountIsSplitGLAccount(GLAccount glaccount, int tenant)
        {
            List<Data.EntityPOCOs.GLAccountCurrency> gLAccountCurrencies = GetRelatedGLAccountCurrencies(glaccount, tenant);
            string customerGLAccountId = GetCustomerGLAccountIdByDisplayNumber(glaccount, tenant);

            if (customerGLAccountId != null && glaccount.IsMultiCurrency == true && gLAccountCurrencies.Any(c => c.GLAccountId == customerGLAccountId))
            {
                throw new ApplicationException("The customer GLaccount you are sending is already defined as a split by currency Account");

            }
        }

        private string GetCustomerGLAccountIdByDisplayNumber(GLAccount glaccount, int tenant)
        {
            string CustomerGLAccountNumber = glaccount.CustomerGLAccountNumber != null ? glaccount.CustomerGLAccountNumber: glaccount.CustomerGLAccount.DisplayNumber;
            if(CustomerGLAccountNumber != null)
                return GetGLAccountByDisplayNumber(CustomerGLAccountNumber, tenant).Id;

            return null;
        }

        private List<Data.EntityPOCOs.GLAccountCurrency> GetRelatedGLAccountCurrencies(GLAccount glaccount, int tenant)
        {
            var gLAccountCurrencyRepository = new GLAccountCurrencyRepository(this.context);
            var gLAccountCurrencies = gLAccountCurrencyRepository.GetRelatedCurrenciesAccountByCustomerGLAccountAll(tenant, glaccount.Id);
            return gLAccountCurrencies;
        }
    }


}