
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountDataMapping: IMapping<GLAccountPM, GLAccount>
   {
        public bool SuppressGetCardByGLAccountId { get;  set; }

        public void CustomPMToPOCO(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            if (entityPM.DisplayNumber != entityPOCO.DisplayNumber && (!String.IsNullOrEmpty(entityPM.DisplayNumber) || !String.IsNullOrEmpty(entityPOCO.DisplayNumber)))
            {
                AddPOCOPropertyName(POCOPropertyNames.PreviousNumber);
                AddPOCOPropertyName(POCOPropertyNames.PreviousNumberChangeDate);
                entityPOCO.PreviousNumber = entityPOCO.DisplayNumber;
                entityPM.PreviousNumber = entityPOCO.DisplayNumber;
                entityPOCO.PreviousNumberChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
            {
                AddPOCOPropertyName(POCOPropertyNames.PreviousEnglishName);
                AddPOCOPropertyName(POCOPropertyNames.PreviousEnglishNameChangeDate);
                entityPOCO.PreviousEnglishName = entityPOCO.EnglishName;
                entityPM.PreviousEnglishName = entityPOCO.EnglishName;
                entityPOCO.PreviousEnglishNameChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
            {
                AddPOCOPropertyName(POCOPropertyNames.PreviousLocalName);
                AddPOCOPropertyName(POCOPropertyNames.PreviousLocalNameChangeDate);
                entityPOCO.PreviousLocalName = entityPOCO.LocalName;
                entityPM.PreviousLocalName = entityPOCO.LocalName;
                entityPOCO.PreviousLocalNameChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }


            if (entityPM.ChartOfAccountsId != entityPOCO.ChartOfAccountsId && (!String.IsNullOrEmpty(entityPM.ChartOfAccountsId) || !String.IsNullOrEmpty(entityPOCO.ChartOfAccountsId)))
            {
                AddPOCOPropertyName(POCOPropertyNames.PreviousChartOfAccountsId);
                AddPOCOPropertyName(POCOPropertyNames.PreviousChartOfAccountsChangeDate);
                entityPOCO.PreviousChartOfAccountsId = entityPOCO.ChartOfAccountsId;
                entityPM.PreviousChartOfAccountsId = entityPOCO.PreviousChartOfAccountsId;
                entityPOCO.PreviousChartOfAccountsChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
#if GLAccMoreData
            if (entityPM.BalanceInLocalCurrency == null)
            {
                AddPOCOPropertyName(POCOPropertyNames.BalanceInLocalCurrency);
                entityPOCO.BalanceInLocalCurrency = 0;
            }

            if (entityPM.BalanceInForeignCurrency == null)
            {
                AddPOCOPropertyName(POCOPropertyNames.BalanceInForeignCurrency);
                entityPOCO.BalanceInForeignCurrency = 0;
            }

#endif

        }
        bool showLocals;
        GLAccountPM entityPM;
        public void CustomPOCOToPM(GLAccountPM EntityPM, GLAccount entityPOCO)
        {
            entityPM = EntityPM;
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencySign);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ReconcileMethodName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ExchangeRateName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.RevenueExpenseName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ChartOfAccountsName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ChartOfAccountsTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ActiveStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OldCurrencyId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OldIsMultiCurrency);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutomaticReconcileName);
            //this.CustomMappedPMProperties.Add(PMPropertyNames.ClientName);
            //this.CustomMappedPMProperties.Add(PMPropertyNames.VendorName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Category1Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Category2Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Category3Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Category4Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Category5Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerGLAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerGLAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ParentAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ParentAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PreviousEnglishName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PreviousLocalName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CardId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            // GET logged contact, RTL
            ContactQuery contactQuery = new ContactQuery(entityPOCO.Tenant);
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant)?? new ContactPM();
            showLocals = !contact.DontShowLocal;

            if(entityPOCO.CreatedByUserId != null)
            {
                ContactPM createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (createdByContact == null) 
                    createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, 0); // user is customer care, get it from tenant 0
                if (createdByContact != null)
                    entityPM.CreatedByUserName = showLocals ? createdByContact.LocalName : createdByContact.EnglishName;
            }

            if (entityPOCO.UpdatedByUserId != null)
            {
                ContactPM updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
                if(updatedByContact == null) 
                    updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, 0); // user is customer care, get it from tenant 0
                if (updatedByContact != null)
                    entityPM.UpdatedByUserName = showLocals ? updatedByContact.LocalName : updatedByContact.EnglishName;
            }


            //(showLocals ? xxxxx.LocalName: xxxxx.EnglishName);
            //if (!SuppressFetchOpenReconcilation)
            //{
            //    LedgerTransactionRepository LedgerTransactionreop = new LedgerTransactionRepository(entityPOCO.Tenant);
            //    entityPM.ReconcilationCount = LedgerTransactionreop.getRecoCount(entityPM.Id);

            //}

            if (entityPOCO.AccountTypeCode != null)
            {
                GLAccountTypeQueryService accountTypeQueryService = new GLAccountTypeQueryService(entityPOCO.Tenant);
                GLAccountTypePM accountType = accountTypeQueryService.GetSingle(entityPOCO.AccountTypeCode, false, true);
                if (accountType != null) entityPM.AccountTypeName = (showLocals ? accountType.LocalName : accountType.EnglishName);
            }


            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyName = (showLocals ? currency.LocalName : currency.EnglishName);
                    if (entityPOCO.IsMultiCurrency == true)
                    {
                        entityPM.CurrencyCode = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
                    }
                    else
                    {
                        entityPM.CurrencyCode = currency.Code;
                        entityPM.CurrencySign = currency.Sign;
                    }
                }
            }
            else if (entityPOCO.IsMultiCurrency == true)
            {
                entityPM.CurrencyCode = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
            }
            else
            {
                entityPM.CurrencyCode = "";
            }


            if (entityPOCO.ReconcileMethodCode != null)
            {
                ReconcileMethodQueryService reconcileMethodQueryService = new ReconcileMethodQueryService(entityPOCO.Tenant);
                ReconcileMethodPM reconcileMethod = reconcileMethodQueryService.GetSingle(entityPOCO.ReconcileMethodCode, false, true);
                if (reconcileMethod != null) entityPM.ReconcileMethodName = (showLocals ? reconcileMethod.LocalName : reconcileMethod.EnglishName);
            }
            if (entityPOCO.ExchangeRateId != null)
            {
                AdditionalCurrencyRateRepository AdditionalCurrencyRateRepository = new AdditionalCurrencyRateRepository(entityPOCO.Tenant);
                AdditionalCurrencyRate additionalCurrencyRate = AdditionalCurrencyRateRepository.GetSingle(entityPOCO.ExchangeRateId,entityPOCO.Tenant);
                if (additionalCurrencyRate != null) entityPM.ExchangeRateName = additionalCurrencyRate.Name;
            }

            if (entityPOCO.AutomaticReconcileId != null)
            {
                AutomaticReconcileMethodQueryService automaticReconcileMethodQueryService = new AutomaticReconcileMethodQueryService(entityPOCO.Tenant);
                AutomaticReconcileMethodPM automaticReconcileMethod = automaticReconcileMethodQueryService.GetSingle(entityPOCO.AutomaticReconcileId, false, true);
                if (automaticReconcileMethod != null) entityPM.AutomaticReconcileName = showLocals? automaticReconcileMethod.LocalName: automaticReconcileMethod.Name;
            }


            if (entityPOCO.RevenueExpenseType != null)
            {
                RevenueExpenseTypeQueryService revenueExpenseQueryService = new RevenueExpenseTypeQueryService(entityPOCO.Tenant);
                RevenueExpenseTypePM revenueExpense = revenueExpenseQueryService.GetSingle(entityPOCO.RevenueExpenseType, false, true);
                if (revenueExpense != null) entityPM.RevenueExpenseName = (showLocals ? revenueExpense.LocalName : revenueExpense.EnglishName);
            }


            if (entityPOCO.ChartOfAccountsId != null)
            {
                ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(entityPOCO.Tenant);
                ChartOfAccountPM chartOfAccounts = chartOfAccountQueryService.GetSingle(entityPOCO.ChartOfAccountsId, false, true);
                if (chartOfAccounts != null)
                {
                    entityPM.ChartOfAccountsName = (showLocals ? chartOfAccounts.LocalName : chartOfAccounts.EnglishName);
                    entityPM.ChartOfAccountsCode = chartOfAccounts.Code;
                    entityPM.ChartOfAccountSecurityLevel = chartOfAccounts.ChartOfAccountSecurityLevel;
                }

                if (entityPOCO.ChartOfAccountsTypeCode != null)
                {
                    ChartOfAccountsTypeQueryService chartOfAccountsTypeQueryService = new ChartOfAccountsTypeQueryService(entityPOCO.Tenant);
                    ChartOfAccountsTypePM chartOfAccountsType = chartOfAccountsTypeQueryService.GetSingle(entityPOCO.ChartOfAccountsTypeCode, false, true);
                    if (chartOfAccountsType != null) entityPM.ChartOfAccountsTypeName = (showLocals ? chartOfAccountsType.LocalName : chartOfAccountsType.EnglishName);
                }
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                var gLAccountQueryService = new GLAccountQueryService(context);
                if (entityPOCO.ControlAccountId != null)
                {

                    if (false)
                    {
                        GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                        GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.ControlAccountId);
                        if (gLAccountList != null)
                        {
                            entityPM.ControlAccountName = (showLocals ? gLAccountList.LocalName : gLAccountList.EnglishName);
                            entityPM.ControlAccountNumber = gLAccountList.DisplayNumber;
                        }
                    }
                    else
                    {
                        var gLAccount = gLAccountQueryService.GetSingleByAccountId(entityPOCO.ControlAccountId, entityPOCO.Tenant);
                        entityPM.ControlAccountName = (showLocals ? gLAccount.LocalName : gLAccount.EnglishName);
                        entityPM.ControlAccountNumber = gLAccount.DisplayNumber;
                    }

                }
                if (!this.SuppressGetCardByGLAccountId)
                {

                    CardRepository repo = new CardRepository(entityPOCO.Tenant);
                    Card card = repo.GetCardByGLAccountId(entityPOCO.Id, entityPOCO.Tenant, false);
                    if (card != null)
                    {
                        entityPM.VatNumber = card.VatNumber;
                        entityPM.CardCountryCode =card.CountryCode;
                    }
                }


                GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(entityPM.Tenant);
                GLAccountCurrency  gLAccountCurrency  = gLAccountCurrencyQueryService.GetGLAccountCurrencyByGLAccountId(entityPM.Id, entityPM.Tenant);
                if (gLAccountCurrency != null)
                {
                    entityPM.IsSplitted = true;
                    entityPM.ParentCurrencyId = gLAccountCurrency.MainGLAccountId;
                    CardRepository repo = new CardRepository(entityPOCO.Tenant);
                    List<string> partnerTypes = new List<string>() { "AC", "CS", "AG", "AL", "CG", "SG", "SL", "TR", "VD", "WH" };
                    Card card = repo.GetCardByGLAccountId(gLAccountCurrency.MainGLAccountId, entityPOCO.Tenant, false, partnerTypes);
                    entityPM.ParentCurrencyGLAccountCardId = card?.Id;
                    if (string.IsNullOrEmpty(entityPM.CardCountryCode))
                    {
                        entityPM.CardCountryCode = card?.CountryCode;
                    }
                }

              

                if (entityPOCO.Inactive == true)
                {
                    entityPM.ActiveStatusName = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", 0, showLocals);
                }
                else
                {
                    entityPM.ActiveStatusName = TranslateTextsClass.Translate("GLAccounts.Q.Active", 0, showLocals);
                }

                entityPM.OldCurrencyId = entityPOCO.CurrencyId;
                if (entityPOCO.IsMultiCurrency.HasValue == true)
                {
                    entityPM.OldIsMultiCurrency = entityPOCO.IsMultiCurrency.Value;
                }
                else
                {
                    entityPM.OldIsMultiCurrency = false;
                }



                if (entityPOCO.CustomerGLAccountId != null)
                {

                    if (true)
                    {
                        var gLAccount = gLAccountQueryService.GetSingleByAccountId(entityPOCO.CustomerGLAccountId, entityPOCO.Tenant);
                        entityPM.CustomerGLAccountName = (showLocals ? gLAccount.LocalName : gLAccount.EnglishName);
                        entityPM.CustomerGLAccountNumber = gLAccount.DisplayNumber;
                    }
                    else
                    {
                        //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                        GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                        GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.CustomerGLAccountId);
                        if (gLAccountList != null)
                        {
                            entityPM.CustomerGLAccountName = (showLocals ? gLAccountList.LocalName : gLAccountList.EnglishName);
                            entityPM.CustomerGLAccountNumber = gLAccountList.DisplayNumber;
                        }
                    }
                }

                if (entityPOCO.ParentAccountId != null)
                {
                    if (true)
                    {
                        var gLAccount = gLAccountQueryService.GetSingleByAccountId(entityPOCO.ParentAccountId, entityPOCO.Tenant);
                        entityPM.ParentAccountName = (showLocals ? gLAccount.LocalName : gLAccount.EnglishName);
                        entityPM.ParentAccountNumber = gLAccount.DisplayNumber;
                    }
                    else
                    {
                        //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                        GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                        GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.ParentAccountId);
                        if (gLAccountList != null)
                        {
                            entityPM.ParentAccountName = (showLocals ? gLAccountList.LocalName : gLAccountList.EnglishName);
                            entityPM.ParentAccountNumber = gLAccountList.DisplayNumber;
                        }
                    }
                }

                if (entityPM.ParentCurrencyId != null)
                {

                         var gLAccount  = gLAccountQueryService.GetSingleByAccountId(entityPM.ParentCurrencyId, entityPM.Tenant);
                         entityPM.ParentName = (showLocals ? gLAccount.LocalName : gLAccount.EnglishName);
                         //entityPM.ParentCurrencyId = gLAccountPM.DisplayNumber;
     
                }

                //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);

                //if (entityPOCO.Category1Id != null)
                if (!String.IsNullOrWhiteSpace(entityPOCO.Category1Id))
                {
                    if (true)
                    {
                        var category1QueryService = new Category1QueryService(context);
                        var category1 = category1QueryService.GetSingle(entityPOCO.Category1Id, false, true);
                        if (category1 != null)
                        {
                            entityPM.Category1Name = (showLocals ? category1.LocalName : category1.EnglishName);
                        }
                    }
                    else
                    {

                        //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                        Category1ListQueryService category1ListQueryService = new Category1ListQueryService(context);
                        Category1List category1List = category1ListQueryService.GetSingle(entityPOCO.Category1Id);
                        if (category1List != null)
                        {
                            entityPM.Category1Name = (showLocals ? category1List.LocalName : category1List.EnglishName);
                        }
                    }
                }
                //if (entityPOCO.Category2Id != null)
                if (!String.IsNullOrWhiteSpace(entityPOCO.Category2Id))
                {
                    //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                    if (true)
                    {
                        var category2QueryService = new Category2QueryService(context);
                        var category2 = category2QueryService.GetSingle(entityPOCO.Category2Id, false, true);
                        if (category2 != null)
                        {
                            entityPM.Category2Name = (showLocals ? category2.LocalName : category2.EnglishName);
                        }
                    }
                    else
                    {
                        Category2ListQueryService category2ListQueryService = new Category2ListQueryService(context);
                        Category2List category2List = category2ListQueryService.GetSingle(entityPOCO.Category2Id);
                        if (category2List != null)
                        {
                            entityPM.Category2Name = (showLocals ? category2List.LocalName : category2List.EnglishName);
                        }

                    }
                }
                var myGLAccountMoreDataRepository = new GLAccountMoreDataRepository(context);

                var poco = myGLAccountMoreDataRepository.GetSingle(entityPOCO.Id, entityPOCO.Tenant);
                if (poco != null)
                {
                    entityPM.NextDueDate = (entityPOCO.AccountTypeCode == "2" || entityPOCO.AccountTypeCode == "3") ? poco.NextDueDate : null ;
                    entityPM.LocalBalanceInDue = poco.LocalBalanceInDue;
                    entityPM.BalanceInLocalCurrency = poco.BalanceInLocalCurrency;
                    entityPM.BalanceInForeignCurrency = poco.BalanceInForeignCurrency;
                }
                //if (entityPOCO.Category3Id != null)
                if (!String.IsNullOrWhiteSpace(entityPOCO.Category3Id))
                {
                    //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                    if (true)
                    {
                        var category3QueryService = new Category3QueryService(context);
                        var category3 = category3QueryService.GetSingle(entityPOCO.Category3Id, false, true);
                        if (category3 != null)
                        {
                            entityPM.Category3Name = (showLocals ? category3.LocalName : category3.EnglishName);
                        }
                    }
                    else
                    {
                        Category3ListQueryService category3ListQueryService = new Category3ListQueryService(context);
                        Category3List category3List = category3ListQueryService.GetSingle(entityPOCO.Category3Id);
                        if (category3List != null)
                        {
                            entityPM.Category3Name = (showLocals ? category3List.LocalName : category3List.EnglishName);
                        }

                    }
                }
                //if (entityPOCO.Category4Id != null)
                if (!String.IsNullOrWhiteSpace(entityPOCO.Category4Id))
                {
                    //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                    if (true)
                    {

                        var category4QueryService = new Category4QueryService(context);
                        var category4 = category4QueryService.GetSingle(entityPOCO.Category4Id, false, true);
                        if (category4 != null)
                        {
                            entityPM.Category4Name = (showLocals ? category4.LocalName : category4.EnglishName);
                        }
                    }
                    else
                    {
                        Category4ListQueryService category4ListQueryService = new Category4ListQueryService(context);
                        Category4List category4List = category4ListQueryService.GetSingle(entityPOCO.Category4Id);
                        if (category4List != null)
                        {
                            entityPM.Category4Name = (showLocals ? category4List.LocalName : category4List.EnglishName);
                        }
                    }

                }
                //if (entityPOCO.Category5Id != null)
                if (!String.IsNullOrWhiteSpace(entityPOCO.Category5Id))
                {
                    //IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                    if (true)
                    {
                        var category5QueryService = new Category5QueryService(context);
                        var category5 = category5QueryService.GetSingle(entityPOCO.Category5Id, false, true);
                        if (category5 != null)
                        {
                            entityPM.Category5Name = (showLocals ? category5.LocalName : category5.EnglishName);
                        }
                    }
                    else
                    {
                        Category5ListQueryService category5ListQueryService = new Category5ListQueryService(context);
                        Category5List category5List = category5ListQueryService.GetSingle(entityPOCO.Category5Id);
                        if (category5List != null)
                        {
                            entityPM.Category5Name = (showLocals ? category5List.LocalName : category5List.EnglishName);
                        }

                    }
                }

                // 
                if (entityPOCO.PreviousEnglishName != null)
                {
                    entityPM.PreviousEnglishName = entityPOCO.PreviousEnglishName;
                }
                if (entityPOCO.PreviousLocalName != null)
                {
                    entityPM.PreviousLocalName = entityPOCO.PreviousLocalName;
                }
            }

            //get cardid if exisit
            CardQuery cardQuery = new CardQuery(entityPM.Tenant);
            bool fromCache = true;
            CardList cardList = cardQuery.GetSingleByGLAccount(entityPM.Id, entityPM.Tenant, false);
           if(cardList != null)
            {
                entityPM.CardId = cardList.Id;
                //entityPM.SalesmanUserId = cardList.SalesmanUserId;
                //entityPM.CollectorId = cardList.CollectorId;
            }

            List<CardList> CardLists = cardQuery.GetAllCardsByGLAccount(entityPM.Id, entityPM.Tenant);
            bool IsSalesmanUserIdSameOnAllCards = false;
            bool IsCollectorIdSameOnAllCards = false;
            bool IsPaymentTermIdSameOnAllCards = false;
         
            if (CardLists!=null && CardLists.Count > 0)
            {
                IsSalesmanUserIdSameOnAllCards = true;
                IsCollectorIdSameOnAllCards = true;
                IsPaymentTermIdSameOnAllCards = true;
                bool IsAtLeasOneSalesmanUserIdValid = false;
                bool IsAtLeasOneCollectorIdValid = false;
                string FirstSalesmanUserId = CardLists[0].SalesmanUserId;
                string FirstCollectorId = CardLists[0].CollectorId;
                string FirstPaymentTermId = CardLists[0].PaymentTermId;
                foreach (CardList card in CardLists)
                {
                  
                    
                    if (card.PaymentTermId != FirstPaymentTermId && card.PaymentTermId != null)
                    {
                        IsPaymentTermIdSameOnAllCards = false;
                        FirstPaymentTermId = card.PaymentTermId;
                        entityPM.PaymentTermId = card.PaymentTermId;
                        SetPaymentTermName(entityPM, showLocals);
                    }
                }

               
                    entityPM.SalesmanUserId = entityPOCO.SalesmanUserId;
                    ContactPM SalesmanContact = contactQuery.GetSinglePMFromCache(entityPM.SalesmanUserId, entityPOCO.Tenant);
                    if (SalesmanContact == null)
                        SalesmanContact = contactQuery.GetSinglePMFromCache(entityPM.SalesmanUserId, 0); // user is customer care, get it from tenant 0
                    if (SalesmanContact != null)
                        entityPM.SalesmanName = showLocals ? SalesmanContact.LocalName : SalesmanContact.EnglishName;
              
            
                    ContactPM CollectorContact = contactQuery.GetSinglePMFromCache(entityPM.CollectorId, entityPOCO.Tenant);
                    if (CollectorContact == null)
                        CollectorContact = contactQuery.GetSinglePMFromCache(entityPM.CollectorId, 0); // user is customer care, get it from tenant 0
                    if (CollectorContact != null)
                        entityPM.CollectorName = showLocals ? CollectorContact.LocalName : CollectorContact.EnglishName;
              
               
                
                if (IsPaymentTermIdSameOnAllCards)
                {
                    entityPM.PaymentTermId = FirstPaymentTermId;
                    SetPaymentTermName(entityPM, showLocals);
                }
                SetPaymentTermToMulti(CardLists, FirstPaymentTermId);
            }
            entityPM = GetGLaccountFollowUpDataFields(entityPM);
            if (entityPM.ParentCurrencyId != null)
            {
                SetVariblesFromParentCurrencyGLAccount(entityPM);
            }

            if(FeatureToggleHelper.HasFeatureToggle("SAL", entityPM.Tenant))
            {
                entityPM.Access = CheckIfUserHasSecurityAccessToGLAccount(entityPM.Tenant, entityPM.ChartOfAccountSecurityLevel);

                if (entityPM.Access == false)
                    ResetAccountBalances(entityPM);
            }
            if(entityPM.AccountTypeCode == GLAccountTypeValues.Client) 
            {
			  entityPM.CustomerDebtNotification = GetCustomerDebtNotificationByAccountId(entityPM);
            }

            entityPM.TotalOpenChequesInLocalCur = GetTotalOpenChequesInLocalCur(entityPM);

        }

        private CustomerDebtNotificationPM GetCustomerDebtNotificationByAccountId(GLAccountPM accountPM)
        {
			IAccountingContext MyContext = AccountingContext.GetContext(accountPM.Tenant);
			CustomerDebtNotificationQueryService customerDebtNotificationQueryServiceQuery = new CustomerDebtNotificationQueryService(MyContext);
			return customerDebtNotificationQueryServiceQuery.GetCustomerDebtNotificationByAccountId(accountPM.Tenant, accountPM.Id);
		}
	
		private static void ResetAccountBalances(GLAccountPM account)
        {
            account.BalanceInForeignCurrency = 0;
            account.BalanceInLocalCurrency = 0;
            account.LocalBalanceInDue = 0;
            account.ForeignBalanceInDue = 0;
            account.Period0 = 0;
            account.Period1 = 0;
            account.Period2 = 0;
            account.Period3 = 0;
            account.Period4 = 0;
            account.Period5 = 0;
            account.PeriodFuture = 0;
            account.PeriodPast = 0;
            account.CalculatedAgingPeriod1 = 0;
            account.CalculatedAgingPeriod2 = 0;
            account.CalculatedAgingPeriod3 = 0;
            account.TotalOpenChequesInLocalCur = 0;
            account.TotFutureOpenChequesInLocalCur = 0;
        }

        private GLAccountPM GetGLaccountFollowUpDataFields(GLAccountPM accountPM)
        {
            GLAccountFollowUpDataPM gLAccountFollowUpData = GetGLAccountFollowUpDataPM(accountPM);
            if(gLAccountFollowUpData != null)
            {
                accountPM = MapFollowUpDataFields(accountPM, gLAccountFollowUpData);
            }
            return accountPM;
        }

        private GLAccountPM MapFollowUpDataFields(GLAccountPM accountPM, GLAccountFollowUpDataPM gLAccountFollowUpData)
        {
            if (accountPM.AccountTypeCode == GLAccountTypes.Client)
            {
                accountPM.GLAccountFollowUpDate = gLAccountFollowUpData.FollowUpDate;
                accountPM.GLAccountFollowUpRemarks = gLAccountFollowUpData.FollowUpRemarks;
            }
            else if (accountPM.AccountTypeCode == GLAccountTypes.Card || accountPM.AccountTypeCode == GLAccountTypes.Vendor)
            {
                accountPM.FollowupDate = gLAccountFollowUpData.FollowUpDate;
                accountPM.FollowupNotes = gLAccountFollowUpData.FollowUpRemarks;
            }
            return accountPM;
        }

        private GLAccountFollowUpDataPM GetGLAccountFollowUpDataPM(GLAccountPM account)
        {
            GLAccountFollowUpDataQueryService accountFollowUpDataQueryService = new GLAccountFollowUpDataQueryService(account.Tenant);
            return accountFollowUpDataQueryService.GetSinglePMByAccountId(account.Id, account.Tenant);
        }

        private decimal GetTotalOpenChequesInLocalCur(GLAccountPM account)
        {
            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            return context.AllARPaymentChequesViews
                .Where(a => a.AccountId == account.Id
                         && a.Tenant == account.Tenant
                         && a.Notes != "החזרת שיק ללקוח"
                         && a.ValueDate <= DateTime.UtcNow)
                .Sum(a => (decimal?)a.LocalAmountCredit) ?? 0m;
        }
        private  void SetPaymentTermToMulti(List<CardList> CardLists, string FirstPaymentTermId)
        {
            if (CardLists.Count > 1)
            {
                CardList card = CardLists.Where(d => d.PaymentTermId != null && d.PaymentTermId != FirstPaymentTermId && FirstPaymentTermId != null).FirstOrDefault();
                if (card != null)
                {
                    entityPM.PaymentTermName = TranslateTextsClass.Translate("GLAccount.O.Multi", entityPM.Tenant, showLocals);
                }
            }
        }
        private static void SetVariblesFromParentCurrencyGLAccount(GLAccountPM entityPM)
        {
            GLAccountPM parent = GetParentCurrencyGLAccount(entityPM);

            entityPM.SalesmanName = parent.SalesmanName;
            entityPM.CollectorName = parent.CollectorName;
            entityPM.PaymentTermName = parent.PaymentTermName;
        }

        private static GLAccountPM GetParentCurrencyGLAccount(GLAccountPM entityPM)
        {
            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            var gLAccountQueryService = new GLAccountQueryService(context);
            GLAccountPM parent = gLAccountQueryService.GetSinglePM(entityPM.ParentCurrencyId, entityPM.Tenant);
            return parent;
        }

        private static void SetPaymentTermName(GLAccountPM entityPM, bool showLocals)
        {
            Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PaymentTermQueryService paymentTermQuery = new Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PaymentTermQueryService(entityPM.Tenant);
            Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PaymentTerm paymentTerm = paymentTermQuery.GetPaymentTermById(entityPM.PaymentTermId, entityPM.Tenant);
            if (paymentTerm != null)
                entityPM.PaymentTermName = showLocals ? paymentTerm.LocalName == null ? paymentTerm.EnglishName: paymentTerm.LocalName : paymentTerm.EnglishName;
        }

   
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public bool SuppressFetchOpenReconcilation { get; internal set; }

        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        private bool CheckIfUserHasSecurityAccessToGLAccount(int tenant, int? chartOfAccountSecurityLevel)
        {
            var settings = GetFullAccountingSettings(tenant);
            var loggedUser = GetLoggedUser(tenant);

            bool hasSecurityAccess =
                (settings.IsSecurityLevelActivated && chartOfAccountSecurityLevel  <= (loggedUser?.SecurityLevel ?? 0))
                || (settings.IsSecurityLevelActivated && chartOfAccountSecurityLevel == null)
                || !settings.IsSecurityLevelActivated;
            return hasSecurityAccess;
        }
        public FullAccountingSettingPM GetFullAccountingSettings(int tenant)
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            return fullAccountingSettingQueryService.GetSingle(tenant.ToString(),false,true);
        }
        private UserPM GetLoggedUser(int tenant)
        {
            UserPM loggedUser;
            UserQuery userQuery = new UserQuery(tenant);
            if (!string.IsNullOrEmpty(AuthenticationUtil.AuthenticatedUserEmail))
            { // user set and passed from from WR
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);
            }
            else
            {
                ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                loggedUser = userQuery.GetSinglePM(loggedContact.Id, tenant);
            }
            return loggedUser;
        }



    }


}
   