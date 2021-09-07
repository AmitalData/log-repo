using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.Accounting.Data.CustomFilters;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class GLAccountListQueryService
    {
	    public IQueryable<GLAccountList> GetIqueryableList(IQueryable<GLAccount> iQueryable)
        {
            string multi = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
            string active = TranslateTextsClass.Translate("GLAccounts.Q.Active", 0);
            string inactive = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", 0);

            IQueryable<GLAccountList> query = (from a in iQueryable//.Include("ChartOfAccount").Include("ChartOfAccountsType")
                                               join chartOfAccount in context.ChartOfAccounts on a.ChartOfAccountsId equals chartOfAccount.Id
                                               join chartOfAccountsType in context.ChartOfAccountsTypes on a.ChartOfAccountsTypeCode equals chartOfAccountsType.Code
                                               join MoreDatas in context.GLAccountMoreDatas on a.Id equals MoreDatas.AccountId
                                               join AgingDatas in context.GLAccountAgingDatas on a.Id equals AgingDatas.AccountId
                                               join RecocileDatas in context.GLAccountRecocileDatas on a.Id equals RecocileDatas.AccountId

                                               join fullAccountingSettings in context.FullAccountingSettings on a.Tenant equals fullAccountingSettings.Tenant

                                               join CardsDatas in context.GLAccountCardsDatas on a.CardsDataId equals CardsDatas.Id
                                               into CardsDatasjoin
                                               from CardsDatas in CardsDatasjoin.DefaultIfEmpty()

                                               join FollowUpDatas in context.GLAccountFollowUpDatas on a.Id equals FollowUpDatas.GlAccountId
                                               into FollowUpDatasjoin
                                               from FollowUpDatas in FollowUpDatasjoin.DefaultIfEmpty()

                                               select new GLAccountList()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   InternalNumber = a.InternalNumber,
                                                   InterestCreditLimit = a.InterestCreditLimit,
                                                   AccountTypeCode = a.AccountTypeCode,
                                                   DisplayNumber = a.DisplayNumber,
                                                   EnglishName = a.EnglishName,
                                                   LocalName = a.LocalName,
                                                   SearchFields = a.SearchFields,
                                                   IsMultiCurrency = a.IsMultiCurrency,
                                                   CurrencyId = a.CurrencyId,
                                                   RevenueExpenseType = a.RevenueExpenseType,
                                                   IsControlAccount = a.IsControlAccount,
                                                   ChartOfAccountsId = a.ChartOfAccountsId,
                                                   Inactive = a.Inactive,
                                                   ReconcileMethodCode = a.ReconcileMethodCode,
                                                   ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                                                   AccountTypeName = a.GLAccountType != null ? a.GLAccountType.EnglishName : null,
                                                   RevenueExpenseName = a.RevenueExpense != null ? a.RevenueExpense.EnglishName : null,
                                                   ReconcileMethodName = a.ReconcileMethod != null ? a.ReconcileMethod.EnglishName : null,
                                                   ReconcileMethodLocalName = a.ReconcileMethod != null ? a.ReconcileMethod.LocalName : null,
                                                   CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                   ChartOfAccountsTypeName = chartOfAccountsType != null ? chartOfAccountsType.EnglishName : null,
                                                   ChartOfAccountsTypeEnglishName = chartOfAccountsType != null ? chartOfAccountsType.EnglishName : null,
                                                   ChartOfAccountsTypeLocalName = chartOfAccountsType != null ? chartOfAccountsType.LocalName : null,
                                                   CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                                                   CurrencySign = a.IsMultiCurrency == true ? "" : a.Currency != null ? a.Currency.Sign : null,
                                                   ControlAccountName = a.ControlAccount != null ? a.ControlAccount.EnglishName : null,
                                                   ControlAccountId = a.ControlAccountId,
                                                   ControlAccountNumber = a.ControlAccount != null ? a.ControlAccount.DisplayNumber : null,
                                                   ChartOfAccountsName = chartOfAccount != null ? chartOfAccount.LocalName : null,
                                                   ChartOfAccountsEnglishName = chartOfAccount != null ? chartOfAccount.EnglishName : null,
                                                   ChartOfAccountsLocalName = chartOfAccount != null ? chartOfAccount.LocalName : null,
                                                   CardsDataId = a.CardsDataId,
                                                   PostponedChequesCommission = a.PostponedChequesCommission,
                                                   ActiveStatusName = a.Inactive == false ? active : inactive,
                                                   AutomaticReconcileId = a.AutomaticReconcileId,
                                                   AutomaticReconcileName = a.AutomaticReconcile != null ?
                                               !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile2) ?
                                               !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile3) ?
                                               a.AutomaticReconcile.AutomaticReconcileField1.EnglishName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField2.EnglishName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField3.EnglishName
                                               : a.AutomaticReconcile.AutomaticReconcileField1.EnglishName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField2.EnglishName
                                               : a.AutomaticReconcile.AutomaticReconcileField1.EnglishName
                                               : null,
                                                   AutomaticReconcileLocalName = a.AutomaticReconcile != null ?
                                               !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile2) ?
                                               !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile3) ?
                                               a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField2.LocalName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField3.LocalName
                                               : a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                               + "+" + a.AutomaticReconcile.AutomaticReconcileField2.LocalName
                                               : a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                               : null,
                                                   PreviousEnglishName = a.PreviousEnglishName,
                                                   PreviousEnglishNameChangeDate = a.PreviousEnglishNameChangeDate,
                                                   PreviousLocalName = a.PreviousLocalName,
                                                   PreviousLocalNameChangeDate = a.PreviousLocalNameChangeDate,
                                                   PreviousNumber = a.PreviousNumber,
                                                   PreviousNumberChangeDate = a.PreviousNumberChangeDate,
                                                   PreviousChartOfAccountsId = a.PreviousChartOfAccountsId,
                                                   PreviousChartOfAccountsChangeDate = a.PreviousChartOfAccountsChangeDate,
                                                   //ClientName = a.Client != null ? a.Client.Card.EnglishName : null,
                                                   //VendorName = a.Vendor != null ? a.Vendor.Card.EnglishName : null,
                                                   //ClientId = a.ClientId,
                                                   //VendorId = a.VendorId,
                                                   CustomerGLAccountId = a.CustomerGLAccountId,
                                                   BalanceInLocalCurrency = MoreDatas.BalanceInLocalCurrency,
                                                   RevaluationEnabled = a.RevaluationEnabled,
                                                   //ClientCode = a.Client != null ? a.Client.Card.Code : null,
                                                   //VendorCode = a.Vendor != null ? a.Vendor.Card.Code : null,
                                                   ParentAccountId = a.ParentAccountId,
                                                   IsVATExempt = a.IsVATExempt,
                                                   LocalBalanceInDue = MoreDatas.LocalBalanceInDue,
                                                   NextDueDate = MoreDatas.NextDueDate,
                                                   TotalOpenChequesInLocalCur = MoreDatas.TotalOpenChequesInLocalCur,
                                                   TotFutureOpenChequesInLocalCur = MoreDatas.TotFutureOpenChequesInLocalCur,

                                                   BalanceInForeignCurrency = MoreDatas.BalanceInForeignCurrency,
                                                   ForeignBalanceInDue = MoreDatas.ForeignBalanceInDue,


                                                   DeductionFileNumber = a.DeductionFileNumber,

                                                   //categories
                                                   Category1Name = a.Category1.EnglishName,
                                                   Category2Name = a.Category2.EnglishName,
                                                   Category3Name = a.Category3.EnglishName,
                                                   Category4Name = a.Category4.EnglishName,
                                                   Category5Name = a.Category5.EnglishName,
                                                   Category1LocalName = a.Category1.LocalName,
                                                   Category2LocalName = a.Category2.LocalName,
                                                   Category3LocalName = a.Category3.LocalName,
                                                   Category4LocalName = a.Category4.LocalName,
                                                   Category5LocalName = a.Category5.LocalName,


                                                   ActiveForInterest = a.ActiveForInterest,
                                                   ActiveForInterestCreditInvoice = a.ActiveForInterestCreditInvoice,
                                                   MinimumInterestInvoiceBilling = a.MinimumInterestInvoiceBilling,
                                                   InterestCalculationStartDate = a.InterestCalculationStartDate,


                                                   // Created & Updated
                                                   CreateDate = a.CreateDate,
                                                   CreatedByLocalName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                   CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                   UpdateDate = a.UpdateDate,
                                                   UpdatedByLocalName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.LocalName : null,
                                                   UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                   ExcludeFromDeductionReport = a.ExcludeFromDeductionReport,
                                                   AllowEditChequePayToName = a.AllowEditChequePayToName,

                                                   ConsolidationVat = a.ConsolidationVat,
                                                   IsEquipmentVendor = a.IsEquipmentVendor,
                                                   //CustomerGLAccountName = a.CustomerGLAccount.LocalName !=null? a.CustomerGLAccount.LocalName : a.CustomerGLAccount.EnglishName,
                                                   //CustomerGLAccountNumber = a.CustomerGLAccount.DisplayNumber,
                                                   //ParentAccountName = a.ParentAccount.LocalName != null ? a.ParentAccount.LocalName : a.CustomerGLAccount.EnglishName,
                                                   //ParentAccountNumber = a.ParentAccount.DisplayNumber,

                                                   // GLaccount Aging Datas
                                                   Period0 = AgingDatas.Period0,
                                                   Period1 = AgingDatas.Period1,
                                                   Period2 = AgingDatas.Period2,
                                                   Period3 = AgingDatas.Period3,
                                                   Period4 = AgingDatas.Period4,
                                                   Period5 = AgingDatas.Period5,
                                                   PeriodPast = AgingDatas.PeriodPast,
                                                   PeriodFuture = AgingDatas.PeriodFuture,
                                                   TotalOpenTransactions = AgingDatas.TotalOpenTransactions,

                                                   FirstPeriodsMonths = fullAccountingSettings.FirstPeriodsMonths,
                                                   SecondPeriodsMonths = fullAccountingSettings.SecondPeriodsMonths,
                                                   ThirdPeriodsMonths = fullAccountingSettings.ThirdsPeriodsMonths,

                                                   CalculatedAgingPeriod1 = (fullAccountingSettings.FirstPeriodsMonths.Contains("Period0") ? AgingDatas.Period0 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("Period1") ? AgingDatas.Period1 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("Period2") ? AgingDatas.Period2 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("Period3") ? AgingDatas.Period3 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("Period4") ? AgingDatas.Period4 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("Period5") ? AgingDatas.Period5 : 0)
                                                                            + (fullAccountingSettings.FirstPeriodsMonths.Contains("PeriodPast") ? AgingDatas.PeriodPast : 0)
                                                                            ,

                                                   CalculatedAgingPeriod2 = (fullAccountingSettings.SecondPeriodsMonths.Contains("Period0") ? AgingDatas.Period0 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("Period1") ? AgingDatas.Period1 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("Period2") ? AgingDatas.Period2 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("Period3") ? AgingDatas.Period3 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("Period4") ? AgingDatas.Period4 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("Period5") ? AgingDatas.Period5 : 0)
                                                                            + (fullAccountingSettings.SecondPeriodsMonths.Contains("PeriodPast") ? AgingDatas.PeriodPast : 0)
                                                                            ,


                                                   CalculatedAgingPeriod3 = (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period0") ? AgingDatas.Period0 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period1") ? AgingDatas.Period1 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period2") ? AgingDatas.Period2 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period3") ? AgingDatas.Period3 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period4") ? AgingDatas.Period4 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("Period5") ? AgingDatas.Period5 : 0)
                                                                            + (fullAccountingSettings.ThirdsPeriodsMonths.Contains("PeriodPast") ? AgingDatas.PeriodPast : 0)
                                                                            ,


                                                   // GLAccount Recocile Datas
                                                   LastReconciledBy = RecocileDatas.LastReconciledByUser.Contact.LocalName == null ? RecocileDatas.LastReconciledByUser.Contact.EnglishName : RecocileDatas.LastReconciledByUser.Contact.LocalName,
                                                   LastReconcileDate = RecocileDatas.LastReconcileDateTime,

                                                   // GLAccount Cards Datas
                                                   CreditLimit = CardsDatas != null ? CardsDatas.CreditLimit : null,
                                                   VatNumber = CardsDatas != null ? CardsDatas.VatNumber : null,
                                                   PaymentTerm = CardsDatas != null ? CardsDatas.PaymentTerm.LocalName == null ? CardsDatas.PaymentTerm.EnglishName : CardsDatas.PaymentTerm.LocalName : null,
                                                   TotalOpenShipments = CardsDatas != null ? CardsDatas.TotalOpenShipments : null,
                                                   Phone = CardsDatas != null ? CardsDatas.Phone : null,
                                                   Salesman = CardsDatas != null ? CardsDatas.SalesmanUser.Contact.LocalName == null ? CardsDatas.SalesmanUser.Contact.EnglishName : CardsDatas.SalesmanUser.Contact.LocalName : null,
                                                   Collector = CardsDatas != null ? CardsDatas.CollectorUser.Contact.LocalName == null ? CardsDatas.CollectorUser.Contact.EnglishName : CardsDatas.CollectorUser.Contact.LocalName : null,

                                                   // GLAccount Follow Up Datas
                                                   FollowupDate = FollowUpDatas != null ? FollowUpDatas.FollowUpDate : null,
                                                   FollowupNotes = FollowUpDatas != null ? FollowUpDatas.FollowUpRemarks : null,
                                                   InsuredCreditLimit = CardsDatas.InsuredcreditLimit,
                                                   ChartOfAccountSecurityLevel = chartOfAccount.ChartOfAccountSecurityLevel,

                                                   IsSecurityLevelsEnabled = fullAccountingSettings.IsSecurityLevelActivated,
                                                   ChartOfAccountSecurityLevel = chartOfAccount.ChartOfAccountSecurityLevel

                                               }) ;
            return query;
        }
        public IQueryable<GLAccountList> MapListFields(IQueryable<GLAccountList> iQueryable, User loggedUser)
        {
            var list = from glaccount in iQueryable.AsEnumerable()
                       select new GLAccountList()
                       {
                           Id = glaccount.Id,
                           Tenant = glaccount.Tenant,
                           InternalNumber = glaccount.InternalNumber,
                           AccountTypeCode = glaccount.AccountTypeCode,
                           DisplayNumber = glaccount.DisplayNumber,
                           LocalName = glaccount.LocalName,
                           EnglishName = glaccount.EnglishName,
                           SearchFields = glaccount.SearchFields,
                           IsMultiCurrency = glaccount.IsMultiCurrency,
                           CurrencyId = glaccount.CurrencyId,
                           RevenueExpenseType = glaccount.RevenueExpenseType,
                           IsControlAccount = glaccount.IsControlAccount,
                           ChartOfAccountsId = glaccount.ChartOfAccountsId,
                           Inactive = glaccount.Inactive,
                           AccountTypeName = glaccount.AccountTypeName,
                           CurrencyName = glaccount.CurrencyName,
                           RevenueExpenseName = glaccount.RevenueExpenseName,
                           ChartOfAccountsName = glaccount.ChartOfAccountsName,
                           ChartOfAccountsTypeCode = glaccount.ChartOfAccountsTypeCode,
                           ChartOfAccountsTypeName = glaccount.ChartOfAccountsTypeName,
                           CurrencyCode = glaccount.CurrencyCode,
                           ReconcileMethodCode = glaccount.ReconcileMethodCode,
                           ReconcileMethodName = glaccount.ReconcileMethodName,
                           ControlAccountId = glaccount.ControlAccountId,
                           ControlAccountName = glaccount.ControlAccountName,
                           ControlAccountNumber = glaccount.ControlAccountNumber,
                           ActiveStatusName = glaccount.ActiveStatusName,
                           AutomaticReconcileId = glaccount.AutomaticReconcileId,
                           AutomaticReconcileName = glaccount.AutomaticReconcileName,
                           PreviousEnglishName = glaccount.PreviousEnglishName,
                           PreviousEnglishNameChangeDate = glaccount.PreviousEnglishNameChangeDate,
                           PreviousLocalName = glaccount.PreviousLocalName,
                           PreviousLocalNameChangeDate = glaccount.PreviousLocalNameChangeDate,
                           PreviousNumber = glaccount.PreviousNumber,
                           PreviousNumberChangeDate = glaccount.PreviousNumberChangeDate,
                           PreviousChartOfAccountsId = glaccount.PreviousChartOfAccountsId,
                           PreviousChartOfAccountsChangeDate = glaccount.PreviousChartOfAccountsChangeDate,
                           CustomerGLAccountId = glaccount.CustomerGLAccountId,
                           CustomerGLAccountName = glaccount.CustomerGLAccountName,
                           CustomerGLAccountNumber = glaccount.CustomerGLAccountNumber,
                           RevaluationEnabled = glaccount.RevaluationEnabled,
                           ParentAccountId = glaccount.ParentAccountId,
                           ParentAccountName = glaccount.ParentAccountName,
                           ParentAccountNumber = glaccount.ParentAccountNumber,
                           Category1Id = glaccount.Category1Id,
                           Category1Name = glaccount.Category1Name,
                           Category2Id = glaccount.Category2Id,
                           Category2Name = glaccount.Category2Name,
                           Category3Id = glaccount.Category3Id,
                           Category3Name = glaccount.Category3Name,
                           Category4Id = glaccount.Category4Id,
                           Category4Name = glaccount.Category4Name,
                           Category5Id = glaccount.Category5Id,
                           Category5Name = glaccount.Category5Name,
                           IsVATExempt = glaccount.IsVATExempt,
                           LastActivityDate = glaccount.LastActivityDate,
                           LastActivityTypeName = glaccount.LastActivityTypeName,
                           LastActivityByUserName = glaccount.LastActivityByUserName,
                           VatNumber = glaccount.VatNumber,
                           PaymentTermId = glaccount.PaymentTermId,
                           SalesmanUserId = glaccount.SalesmanUserId,
                           NewGLAccountCardId = glaccount.NewGLAccountCardId,
                           NextDueDate = glaccount.NextDueDate,
                           CurrencySign = glaccount.CurrencySign,
                           DeductionFileTypeId = glaccount.DeductionFileTypeId,
                           DeductionFileNumber = glaccount.DeductionFileNumber,
                           AssessingOfficeCode = glaccount.AssessingOfficeCode,
                           Occupation = glaccount.Occupation,
                           DeductionTypeId = glaccount.DeductionTypeId,
                           ConsolidationVat = glaccount.ConsolidationVat,
                           IsEquipmentVendor = glaccount.IsEquipmentVendor,
                           ExcludeFromDeductionReport = glaccount.ExcludeFromDeductionReport,
                           Parent = glaccount.Parent,
                           DeductionTypeName = glaccount.DeductionTypeName,
                           DeductionFileTypeCode = glaccount.DeductionFileTypeCode,
                           DeductionFileTypeName = glaccount.DeductionFileTypeName,
                           AssessingOfficeName = glaccount.AssessingOfficeName,
                           DeductionTypeEnglishName = glaccount.DeductionTypeEnglishName,
                           AutomaticReconcileLocalName = glaccount.AutomaticReconcileLocalName,
                           ReconcileMethodLocalName = glaccount.ReconcileMethodLocalName,
                           CardId = glaccount.CardId,
                           CreatedByUserId = glaccount.CreatedByUserId,
                           UpdatedByUserId = glaccount.UpdatedByUserId,
                           CreateDate = glaccount.CreateDate,
                           UpdateDate = glaccount.UpdateDate,
                           CreatedByUserName = glaccount.CreatedByUserName,
                           UpdatedByUserName = glaccount.UpdatedByUserName,
                           CreatedByLocalName = glaccount.CreatedByLocalName,
                           UpdatedByLocalName = glaccount.UpdatedByLocalName,
                           AllowEditChequePayToName = glaccount.AllowEditChequePayToName,
                           ActiveForInterest = glaccount.ActiveForInterest,
                           InterestCalculationStartDate = glaccount.InterestCalculationStartDate,
                           ActiveForInterestCreditInvoice = glaccount.ActiveForInterestCreditInvoice,
                           InterestCreditLimit = glaccount.InterestCreditLimit,
                           NameForPrintingCheques = glaccount.NameForPrintingCheques,
                           Smallcashbook = glaccount.Smallcashbook,
                           MinimumInterestInvoiceBilling = glaccount.MinimumInterestInvoiceBilling,
                           SalesmanName = glaccount.SalesmanName,
                           CollectorName = glaccount.CollectorName,
                           SplitCurrencyAccount = glaccount.SplitCurrencyAccount,
                           ParentName = glaccount.ParentName,
                           ParentCurrencyId = glaccount.ParentCurrencyId,
                           ReportingAsAnotherDocument = glaccount.ReportingAsAnotherDocument,
                           CreditAllotmentPercentage = glaccount.CreditAllotmentPercentage,
                           Category1LocalName = glaccount.Category1LocalName,
                           Category2LocalName = glaccount.Category2LocalName,
                           Category3LocalName = glaccount.Category3LocalName,
                           Category4LocalName = glaccount.Category4LocalName,
                           Category5LocalName = glaccount.Category5LocalName,
                           RelatedGLAccount = glaccount.RelatedGLAccount,
                           ChartOfAccountsEnglishName = glaccount.ChartOfAccountsEnglishName,
                           ChartOfAccountsTypeEnglishName = glaccount.ChartOfAccountsTypeEnglishName,
                           ChartOfAccountsTypeLocalName = glaccount.ChartOfAccountsTypeLocalName,
                           ChartOfAccountsLocalName = glaccount.ChartOfAccountsLocalName,
                           CardsDataId = glaccount.CardsDataId,
                           PaymentTermName = glaccount.PaymentTermName,
                           TotalOpenTransactions = glaccount.TotalOpenTransactions,
                           LastReconciledBy = glaccount.LastReconciledBy,
                           LastReconcileDate = glaccount.LastReconcileDate,
                           CreditLimit = glaccount.CreditLimit,
                           PaymentTerm = glaccount.PaymentTerm,
                           TotalOpenShipments = glaccount.TotalOpenShipments,
                           Phone = glaccount.Phone,
                           Salesman = glaccount.Salesman,
                           Collector = glaccount.Collector,
                           FollowupDate = glaccount.FollowupDate,
                           FollowupNotes = glaccount.FollowupNotes,
                           FirstPeriodsMonths = glaccount.FirstPeriodsMonths,
                           SecondPeriodsMonths = glaccount.SecondPeriodsMonths,
                           ThirdPeriodsMonths = glaccount.ThirdPeriodsMonths,
                           InsuredCreditLimit = glaccount.InsuredCreditLimit,
                           PostponedChequesCommission = glaccount.PostponedChequesCommission,


                           Period0 = GetPeriodValue("0", glaccount, loggedUser.SecurityLevel),
                           Period1 = GetPeriodValue("1", glaccount, loggedUser.SecurityLevel),
                           Period2 = GetPeriodValue("2", glaccount, loggedUser.SecurityLevel),
                           Period3 = GetPeriodValue("3", glaccount, loggedUser.SecurityLevel),
                           Period4 = GetPeriodValue("4", glaccount, loggedUser.SecurityLevel),
                           Period5 = GetPeriodValue("5", glaccount, loggedUser.SecurityLevel),
                           PeriodFuture = GetPeriodValue("Future", glaccount, loggedUser.SecurityLevel),
                           PeriodPast = GetPeriodValue("Past", glaccount, loggedUser.SecurityLevel),

                           BalanceInForeignCurrency = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.BalanceInForeignCurrency : 0,
                           BalanceInLocalCurrency = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.BalanceInLocalCurrency : 0,
                           ForeignBalanceInDue = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.ForeignBalanceInDue : 0,
                           LocalBalanceInDue = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.LocalBalanceInDue : 0,
                           CalculatedAgingPeriod1 = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.CalculatedAgingPeriod1 : 0,
                           CalculatedAgingPeriod2 = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.CalculatedAgingPeriod2 : 0,
                           CalculatedAgingPeriod3 = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.CalculatedAgingPeriod3 : 0,
                           TotalOpenChequesInLocalCur = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.TotalOpenChequesInLocalCur : 0,
                           TotFutureOpenChequesInLocalCur = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount) ? glaccount.TotFutureOpenChequesInLocalCur : 0,

                           Access = CheckIfUserHasAccessToGLAccount(loggedUser.SecurityLevel, glaccount),
                       };



            return list.AsQueryable();
        }

        private decimal? GetPeriodValue(string periodName,GLAccountList glaccount, int? loggedUserSecurityLevel)
        {
            if (CheckIfUserHasAccessToGLAccount(loggedUserSecurityLevel, glaccount))
                return GetPeriodValueFromGLAccount(periodName, glaccount);
                
            return 0;
        }

        private static bool CheckIfUserHasAccessToGLAccount(int? loggedUserSecurityLevel, GLAccountList glaccount)
        {
            return !glaccount.IsSecurityLevelsEnabled 
                || (glaccount.IsSecurityLevelsEnabled && glaccount.ChartOfAccountSecurityLevel == null)
                || (glaccount.IsSecurityLevelsEnabled && glaccount.ChartOfAccountSecurityLevel <= loggedUserSecurityLevel);
        }

        private static decimal? GetPeriodValueFromGLAccount(string periodName, GLAccountList glaccount)
        {
            switch (periodName)
            {
                case "0": return glaccount.Period0;
                case "1": return glaccount.Period1;
                case "2": return glaccount.Period2;
                case "3": return glaccount.Period3;
                case "4": return glaccount.Period4;
                case "5": return glaccount.Period5;
                case "Future": return glaccount.PeriodFuture;
                case "Past": return glaccount.PeriodPast;
            }
            return 0;
        }

        private Contact GetLoggedContact(int tenant)
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant, true);
            return loggedContact;
        }
        private User GetLoggedUser(int tenant)
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            var loggedUser = userRepository.GetSingleUserByEmail(email, tenant, true);
            return loggedUser;
        }
        private IQueryable<GLAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccount> iQueryable,int tenant)
        {
            GLAccountCustomFilter filters = new GLAccountCustomFilter(tenant);

            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable, context);

            return iQueryable;
		}

		private IQueryable<GLAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccount> iQueryable,int tenant)
        {
			return iQueryable;
		}


        public GLAccountList GetByAccountId(string accountId, int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                              where a.Tenant == tenant && a.Id == accountId
                                                              select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List <GLAccountList> accountList = accountListQuery.ToList();
            GLAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public List<GLAccountList> GetRevenueExpenseGLAccountList(int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && (a.AccountTypeCode == "1" || a.AccountTypeCode=="2" )
                                                  select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<GLAccountList> accountList = accountListQuery.ToList();

            return accountList;
        }

        public List<CardGLAccountListDataView> GetLastActivityGLAccounts(int tenant, string userId, string objectTableId, string accountTypeCode)
        {
            List<CardGLAccountListDataView> entityList = new List<CardGLAccountListDataView>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            GLAccountRepository repository = new GLAccountRepository(tenant);
            IQueryable<CardGLAccountDataView> entities = entities = repository.GetCardGLAccountDataViews(accountTypeCode, tenant);

            string multi = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
            string active = TranslateTextsClass.Translate("GLAccounts.Q.Active", 0);
            string inactive = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", 0);
            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                CardGLAccountDataView a = 
                            (from d in entities
                           where d.Id == lastActivity.EntityId
                           select d).FirstOrDefault();



                if (a != null)
                {
                    //get glamore data
                    GLAccountMoreData glAccountMoreData = GetAccountMoreData(a.Id, a.Tenant);

                    CardGLAccountListDataView list = new CardGLAccountListDataView()
                    {
                        //glaccount
                        Id = a.Id,
                        Tenant = a.Tenant,
                        InternalNumber = a.InternalNumber,
                        AccountTypeCode = a.AccountTypeCode,
                        DisplayNumber = a.DisplayNumber,
                        GLAccountEnglishName = a.GLAccountEnglishName,
                        GLAccountLocalName = a.GLAccountLocalName,
                        SearchFields = a.SearchFields,
                        IsMultiCurrency = a.IsMultiCurrency,
                        CurrencyId = a.CurrencyId,
                        RevenueExpenseType = a.RevenueExpenseType,
                        IsControlAccount = a.IsControlAccount,
                        ChartOfAccountsId = a.ChartOfAccountsId,
                        Inactive = a.Inactive,
                        ReconcileMethodCode = a.ReconcileMethodCode,
                        ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                        ControlAccountId = a.ControlAccountId,
                        ActiveStatusName = a.Inactive == false ? active : inactive,
                        AutomaticReconcileId = a.AutomaticReconcileId,
                        PreviousEnglishName = a.PreviousEnglishName,
                        PreviousEnglishNameChangeDate = a.PreviousEnglishNameChangeDate,
                        PreviousLocalName = a.PreviousLocalName,
                        PreviousLocalNameChangeDate = a.PreviousLocalNameChangeDate,
                        PreviousNumber = a.PreviousNumber,
                        PreviousNumberChangeDate = a.PreviousNumberChangeDate,
                        PreviousChartOfAccountsId = a.PreviousChartOfAccountsId,
                        PreviousChartOfAccountsChangeDate = a.PreviousChartOfAccountsChangeDate,
                        CustomerGLAccountId = a.CustomerGLAccountId,
                  //      BalanceInLocalCurrency = a.BalanceInLocalCurrency,
                        RevaluationEnabled = a.RevaluationEnabled,
                        ParentAccountId = a.ParentAccountId,
                        IsVATExempt = a.IsVATExempt,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.LocalName,
                        ChartOfAccountsName = a.ChartOfAccountsEnglishName != null ? a.ChartOfAccountsEnglishName : null, //ChartOfAccountsLocalName

                        DeductionFileNumber = a.DeductionFileNumber,

                        // Card
                        SalesmanUserId = a.SalesmanUserId    ,
                        CollectorId = a.CollectorId          ,
                        CardEnglishName = a.CardEnglishName  ,
                        CardLocalName = a.CardLocalName      ,
                        CardGLAccountId = a.CardGLAccountId  ,
                        VatTypeId = a.VatTypeId              ,
                        CountryId = a.CountryId              ,
                        CountryCode = a.CountryCode          ,
                        CityName = a.CityName                ,
                        CountryName = a.CountryName          ,
                        PaymentTermId = a.PaymentTermId      ,
                        VatNumber = a.VatNumber              ,

                        // Contacts (SalesMans)
                        SalesManEnglishName = a.SalesManEnglishName,
                        SalesManLocalName = a.SalesManLocalName,

                        // Contacts (Collectors)
                        CollectorEnglishName = a.CollectorEnglishName,
                        CollectorLocalName = a.CollectorLocalName,

                        // GLACCOUNT MORE DATA
                        BalanceInLocalCurrency = glAccountMoreData.BalanceInLocalCurrency,
                        LocalBalanceInDue = glAccountMoreData.LocalBalanceInDue,
                        NextDueDate = glAccountMoreData.NextDueDate,


                    };


                    entityList.Add(list);
                }
            }




            return entityList;
        }
        public GLAccountMoreData GetAccountMoreData(string accountId, int tenant)
        {
            GLAccountMoreData _md = (from a in context.GLAccountMoreDatas
                                                   where a.Tenant == tenant && a.AccountId == accountId
                                                   select a).FirstOrDefault();

            return _md;
        }

        public List<GLAccountList> GetByAccountType(string accountTypeCode, string searchFields, int tenant)
        {
            IQueryable<GLAccount> accountsQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode && searchFields.Contains(searchFields)
                                                  select a);

            IQueryable<GLAccountList> accountsListQuery = this.GetIqueryableList(accountsQuery);
            List<GLAccountList> accountsList = accountsListQuery.ToList();
            return accountsList;
        }

        public List<GLAccountList> GetTopDeptors(string filterString, string accountTypeCode, int tenant)
        {
            IQueryable<GLAccount> accountsQuery;
            if (filterString == "Balance Due")
            {
                accountsQuery = (from a in context.GLAccounts
                                 join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                                 where a.AccountTypeCode == accountTypeCode && md.LocalBalanceInDue > 0 && a.Tenant == tenant
                                 orderby md.LocalBalanceInDue descending
                                 select a).Take(10);
            }
            else
            {
                accountsQuery = (from a in context.GLAccounts
                                 join md in context.GLAccountMoreDatas
                                 on a.Id equals md.AccountId

                                 where a.AccountTypeCode == accountTypeCode && md.BalanceInLocalCurrency > 0 && a.Tenant == tenant
                                 orderby md.BalanceInLocalCurrency descending
                                select a).Take(10);
            }
            

            IQueryable<GLAccountList> accountsListQuery = this.GetIqueryableList(accountsQuery);
            List<GLAccountList> accountsList = accountsListQuery.ToList();
            return accountsList;
        }

        public List<GLAccountList> GetChildrenGLAccounts(string GLAccountId, int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && a.ParentAccountId == GLAccountId 
                                                  select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<GLAccountList> accountList = accountListQuery.ToList();

            return accountList;
        }
       
        public IQueryable<GLAccountList> GetByIds(List<string> ids, int tenant, bool noNeedTenant)
        {
          
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                      //where a.Tenant == tenant && ids.Contains(a.Id)
                                                  where ids.Contains(a.Id) 
                                                  select a);
            if (!noNeedTenant)
            {
                accountQuery = accountQuery.Where(a => a.Tenant == tenant);
            }

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            //var xxx = accountListQuery.ToList();

            return accountListQuery;
        }
   

    }
}
	