using System;
using System.Linq;

using Simplog.Data.Helpers;

using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using System.Collections.Generic;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Globalization;
using Simplog.Data.InvoiceModel;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class ARPaymentValidator
    {
        public static void Validate(ARPaymentPM entityPM, ARPayment entityPOCO, bool isNew, IInvoiceContext objectContext, CashBookPM cashBook = null)
        {
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);

            AccountingSetting myAccountingSetting = (from d in myCommonContext.AccountingSettings
                                                     where d.Id == entityPM.Tenant
                                                     select d).FirstOrDefault();

            int tenant = entityPM.Tenant;
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
            string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, showLocal);

            string paymentMethodCode = "";
            Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository paymentMethodRepository = new Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository(tenant);
            Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod paymentMethod = paymentMethodRepository.GetSingleAccountingPaymentMethod(entityPM.AccountingPaymentMethodId, tenant);
            if (paymentMethod != null)
            {
                paymentMethodCode = paymentMethod.Code;
            }
           
            bool isNegativeAmountEnabled = false;
            if (paymentMethodCode == "FS")
            {
                if (myAccountingSetting != null)
                {
                    if (myAccountingSetting.EnableNegativeOffsetARPayments)
                    {
                        isNegativeAmountEnabled = true;
                    }
                }
            }

            if (entityPM.RegisterDate > TenantServerConfigration.GetCurrentDateTime(tenant))
            {
                string msg = TranslateTextsClass.Translate("ARPayment.M.CantSetFutureDatePayment", tenant);
                throw new ApplicationException(msg);
            }
           
            if (entityPM.AmountInPaymentCurrency == 0)
            {
                bool isAllowed = false;
                if (paymentMethodCode != null)
                {
                    if (paymentMethodCode.ToUpper() == "FS")
                    {
                        isAllowed = true;

                        //if (entityPM.PaymentInvoices.Count == 0)
                        //{
                        //    throw new ApplicationException("You should have 1 Invoice line at least");
                        //}

                        //else
                        //{
                        //    isAllowed = true;
                        //}
                    }
                }

                if (!isAllowed)
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.CantSetZeroAmount", tenant);
                    throw new ApplicationException(msg);
                }
            }

            if (paymentMethodCode == "CH")
            {
                if (string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef) && entityPM.ARPaymentChequeReplicas.Count ==0)
                {
                    throw new ApplicationException(rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.ChequeOrPaymentRef", tenant)));
                }              
            }

            if (paymentMethodCode == "CC")
            {
                if (string.IsNullOrEmpty(entityPM.CreditCardTypeId))
                {
                    throw new ApplicationException(rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.CreditCardTypeId", tenant)));
                }
            }

            if (entityPM.HasInvoicesErrors)
            {
                string msg = TranslateTextsClass.Translate("ARPayment.M.PaymentInvoicesHasErrors", tenant);
                throw new ApplicationException(msg);
            }

            double? result = entityPM.PaymentInvoices.Where(a => a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Sum(d => d.PaymentAmount);

            double? paymentAmountPaid = MethodHelper.Roundd(result, 2);

            if (isNegativeAmountEnabled == false)
            {
                if (entityPM.AmountInPaymentCurrency < 0)
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.CantSetMinusAmount", tenant);
                    throw new ApplicationException(msg);
                }

                if (paymentAmountPaid < 0)
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.PaymentAmountPaidCantBeMinus", tenant);
                    throw new ApplicationException(msg);
                }
            }

            if (paymentAmountPaid > entityPM.AmountInPaymentCurrency)
            {
                string msg = TranslateTextsClass.Translate("ARPayment.M.PaymentAmountPaidCantBeBigger", tenant);
                throw new ApplicationException(msg);
            }

            ValidateAirlineRestriction(entityPM.BillToId, tenant);

            if (paymentMethodCode == "CH" && IsInternalAccountingSystem(entityPM.Tenant))//"CH" == Cheque
            {
                ValidateChequeForCashBook(entityPM);
            }

            if (!IsFullAccounting(tenant) && entityPM.AccountingPaymentMethodCode != "CA" && entityPM.AccountingPaymentMethodCode != "FS" && entityPM.ValueDate == null)
            {
                throw new ApplicationException(rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.ValueDate", tenant)));
            }

            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting.SATInterfaceCode == "PROF33")
            {
                if (entityPM.SATPaymentMethodCode == "99")
                {
                    throw new ApplicationException("Forma Pago value can't be 'Por Definir'.Please choose another value.");
                }
            }

            if (entityPM.PaymentInvoices.Where(a => a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && (a.ForeignAmount == null || a.ForeignAmount == 0)).Any())
            {
                throw new ApplicationException("Can't connect lines with zero Amount to Pay");
            }

            if (entityPM.PaymentNo != null)
            {
                var isInvoiceNumberExists = objectContext.ARPayments.Where(d => d.Id != entityPM.Id && d.PaymentNo == entityPM.PaymentNo && d.Tenant == entityPM.Tenant).Any();
                if (isInvoiceNumberExists)
                {
                    string msg = "Payment No " + entityPM.PaymentNo + " already exist in another payment";
                    throw new ApplicationException(msg);
                }
            }

            if (entityPM.IsPaymentNumberManuallySet && entityPM.PaymentNo == null)
            {
                throw new ApplicationException("You Should Set Payment No");
            }

            if (!myAccountingSetting.AllowManualInvoiceNumber)
            {
                if (entityPM.IsPaymentNumberManuallySet)
                {
                    if (entityPM.StatusCode == null || entityPM.StatusCode == "DR")
                    {
                        throw new ApplicationException("Accounting Settings don't allow manual payment number");
                    }
                }
            }
            ValidateAccountingSetting(entityPM);
            ValidateFullAccounting(entityPM.ARPaymentChequeReplicas, entityPM.Tenant, entityPM.BillToId, entityPM.PaymentCurrencyId, cashBook, paymentMethodCode, entityPM.RegisterDate, entityPM.BankAccountId, false, entityPM.ValueDate, entityPM.BankBranch, entityPM.Account, entityPM.Bank );
            ValidateUnUpdateFields(entityPM,entityPOCO, isNew);
        }

        private static bool IsFullAccounting(int tenant)
        {
            var isFullAccounting = false;
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && !tenantPOCO.AccountingActivated)
            {
                isFullAccounting = true;
            }
            return isFullAccounting;
        }

        private static void ValidateUnUpdateFields(ARPaymentPM entityPM, ARPayment entityPOCO, bool isNew)
        {
            if (!isNew)
            {
                bool isEditingEnabled = IsEditingEntityEnabled(entityPOCO);

                if (!isEditingEnabled)
                {
                    if (entityPM.PaymentCurrencyExchangeRate != entityPOCO.PaymentCurrencyExchangeRate)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARPayment.F.PaymentCurrencyExchangeRate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.AmountInPaymentCurrency != entityPOCO.AmountInPaymentCurrency)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARPayment.F.AmountInPaymentCurrency", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }
                }
            }
        }

        private static void ValidateAirlineRestriction(string myCardId, int tenant)
        {
            if (!string.IsNullOrEmpty(myCardId))
            {
                bool isRestrictedByAirline = false;

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                    if (tenantManagement != null)
                    {
                        isRestrictedByAirline = tenantManagement.IsRestrictedByAirline;
                    }
                }

                if (isRestrictedByAirline)
                {
                    Card myCard = CardRepository.GetSingleCard(myCardId, tenant, true);
                    if (myCard != null)
                    {
                        if (myCard.PartnerTypeId == "AL")
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(tenant);

                            if (MethodHelper.IsAirlineRestricted(myCardId, airlineRepository, tenant))
                            {
                                throw new ApplicationException("Bill to Airline is not allowed");
                            }
                        }
                    }
                }
            }
        }

        private static void ValidateAccountingSetting(ARPaymentPM entityPM)
        {
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);
            Tenant loggedTenant = (from a in myCommonContext.Tenants.Include("AccountingSetting")
                                   where a.Id == entityPM.Tenant
                                   select a).FirstOrDefault();

            if (loggedTenant != null)
            {
                if (loggedTenant.AccountingSetting != null)
                {
                    if (!loggedTenant.AccountingSetting.AllowManualInvoiceNumber)
                    {
                        if (entityPM.IsPaymentNumberManuallySet)
                        {
                            if (entityPM.StatusCode == null || entityPM.StatusCode == "DR")
                            {
                                throw new ApplicationException("Accounting Settings don't allow manual payment number");
                            }
                        }
                    }
                }
            }

            if (entityPM.SetApproved)
            {
                IInvoiceContext myContext = InvoiceContext.GetContext(entityPM.Tenant);

                if (loggedTenant != null)
                {
                    if (loggedTenant.AccountingSetting != null)
                    {
                        if (loggedTenant.AccountingSetting.IsARPaymentChronologicalDates && !entityPM.IsExternalEntity)
                        {
                            ARPayment lastApprovedPayment = (from a in myContext.ARPayments
                                                             where a.Tenant == entityPM.Tenant
                                                             && a.StatusCode != "DR"
                                                             && a.StatusCode != "VD"
                                                             && a.PaymentNo != entityPM.PaymentNo
                                                             select a).OrderByDescending(d => d.ApprovedDate).FirstOrDefault();
                            if (lastApprovedPayment != null)
                            {
                                DateTime? entityDate = null;
                                DateTime? lastApprovedDate = null;
                                string approvedDateString = "";

                                entityDate = entityPM.RegisterDate;
                                lastApprovedDate = lastApprovedPayment.RegisterDate;
                                approvedDateString = "Register Date";

                                if (entityDate < lastApprovedDate)
                                {
                                    ICommonDataContext context = CommonDataContext.GetContext(entityPM.Tenant);
                                    Tenant currentTenant = context.Tenants.Where(t => t.Id == entityPM.Tenant).FirstOrDefault();
                                    string datetimeformat = @"dd\/MM\/yyyy";
                                    if (!string.IsNullOrEmpty(currentTenant.DateTimeFormat))
                                    {
                                        datetimeformat = currentTenant.DateTimeFormat;
                                    }
                                    string dateString = lastApprovedDate.Value.ToString(datetimeformat, CultureInfo.CurrentCulture);
                                    bool useLocal = true;
                                    var user = GetLoggedContact(entityPM.Tenant);
                                    if (user != null) useLocal = !(GetLoggedContact(entityPM.Tenant).DontShowLocal);

                                    string fieldLabel = TranslateTextsClass.Translate("ARPayment.M.ChronologicalDate", entityPM.Tenant, useLocal);
                                    string exception = fieldLabel.Replace("%Date", dateString);
                                    exception = exception.Replace("%ApprovedDate", approvedDateString);
                                    throw new ApplicationException(exception);
                                }
                            }
                        }
                    }
                }
            }
        }

        #region InternalAccountingSystem
        private static bool IsInternalAccountingSystem(int tenant)
        {
            bool rv = false;
            //AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository();
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                rv = (accountingSetting.AccountingSystemCode == "LA"); //"Logitude Accounting"
            }
            return rv;
        }

        private static void ValidateChequeForCashBook(ARPaymentPM entityPM)
        {
            //AccountingContext accountingContext = new AccountingContext();
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            CashBookRepository cashBookRepository = new CashBookRepository(accountingContext);
            List<CashBook> cashBookList = cashBookRepository.GetByCurrencyAndTypeAndBranch(entityPM.PaymentCurrencyId, "2", entityPM.BranchId, entityPM.Tenant);//"2" == Cheques
            if (cashBookList == null)
            {
                string msg = TranslateTextsClass.Translate("ARPayment.M.NoChequeCashBookCurr", entityPM.Tenant) + " " + entityPM.PaymentCurrencyCode;
                throw new ApplicationException(msg);
            }
            else
            {
                CashBook cashBook = cashBookList.FirstOrDefault();
                if (cashBook == null)
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.NoChequeCashBookCurr", entityPM.Tenant) + " " + entityPM.PaymentCurrencyCode;
                    throw new ApplicationException(msg);
                }
            }
        }
        #endregion

        private static GLAccountPM getGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }
       
        public static void ValidateFullAccounting(List<ARPaymentChequeReplicaPM> aRPaymentChequeReplicas, int tenant, string billToId, string paymentCurrencyId, CashBookPM cashBook, string code, DateTime? registerDate, string bankAccountId, bool isOut = false, DateTime? valueDate = null, string branch = null, string account = null, string bank=null)
        {
            var errors = "";

           
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {
                bool useLocal = true;
                var user = GetLoggedContact(tenant);
                if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                if (!isOut)
                {
                    if (code == "CH" || code == "CA")
                    {
                        if (cashBook == null)
                        {
                            string msg = TranslateTextsClass.Translate("ARPayment.M.ARPaymentCashbook", tenant, useLocal);
                            //throw new ApplicationException(msg);

                            errors += msg + ";";
                        }
                    }

                    if (code == "BT" && valueDate != null && valueDate > TenantServerConfigration.GetCurrentDateTime(tenant))
                    {
                        string msg = TranslateTextsClass.Translate("ARPayment.M.ValueDateCantBeFutureDate", tenant, useLocal);
                        errors += msg + ";";
                    }
                    bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);

                    if (code == "CH" && string.IsNullOrEmpty(branch) && aRPaymentChequeReplicas.Count ==0)
                    {
                        string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, showLocal);
                        errors += rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.BankBranch", tenant, useLocal)) + ";";
                    }

                    if (code == "CH" && string.IsNullOrEmpty(account) && aRPaymentChequeReplicas.Count == 0)
                    {
                        string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, showLocal);
                        errors += rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.Account", tenant, useLocal)) + ";";
                    }

                    if (code == "CH" && string.IsNullOrEmpty(bank) && aRPaymentChequeReplicas.Count == 0)
                    {
                        string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, showLocal);
                        errors += rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.Bank", tenant, useLocal)) + ";";
                    }
                    if(code != "CA" && valueDate == null)
                    {

                        string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, false);
                        errors += rmsg.Replace("%FieldName", TranslateTextsClass.Translate("ARPayment.F.ValueDate", tenant, false)) + ";";

                    }
                    if (code  == "CH" && valueDate != null && registerDate != null)
                    {
                        ValidateValueDate(valueDate, registerDate , tenant);
                    }
                }
                GLAccountPM glAccount = getGLAccount(billToId, tenant);
                if (glAccount == null)
                {

                    string msg = TranslateTextsClass.Translate("ARPayment.M.BillToGLAccount", tenant, useLocal);
                    errors += msg + ";";
                    //throw new ApplicationException(msg);
                }
                else if (glAccount != null && (glAccount.IsMultiCurrency == null || glAccount.IsMultiCurrency == false))
                {
                    if (glAccount.CurrencyId != paymentCurrencyId)
                    {
                        string msg = TranslateTextsClass.Translate("ARPayment.M.BillToGLAccountCurrency", tenant, useLocal);
                        msg += " " + glAccount.CurrencyCode;
                        //throw new ApplicationException(msg);
                        errors += msg + ";";
                    }
                }

                IAccountingContext myContext = AccountingContext.GetContext(tenant);
                AccountingPeriodListQueryService accountingPeriodQuery = new AccountingPeriodListQueryService(myContext);
                var now = TenantServerConfigration.GetCurrentDateTime(tenant);
                AccountingPeriodList accountingPeriodList = accountingPeriodQuery.GetByYear(registerDate.Value.Year, "1", tenant);
                if (accountingPeriodList != null && registerDate != null)
                {
                    var month = registerDate.Value.Month;
                    if (month > accountingPeriodList.OpenMonth || month <= accountingPeriodList.ClosedMonth)
                    {
                        string msg = TranslateTextsClass.Translate("ARPayment.M.ClosedMonth", tenant, useLocal);
                        errors += msg + ";";
                    }
                }
                else
                {
                    string msg = TranslateTextsClass.Translate("ARPayment.M.ClosedMonth", tenant, useLocal);
                    errors += msg + ";";
                }

                IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(bankAccountId, tenant);

                if (bankAccount != null && bankAccount.GLAccountCurrencyId != null && bankAccount.GLAccountCurrencyId != "multi")
                {
                    if (bankAccount.GLAccountCurrencyId != paymentCurrencyId)
                    {
                        string msg = "The currency of the bank account GLAccount(" + bankAccount.GLAccountNumber + ") is different from ARPayment curreny";
                        errors += msg + ";";
                    }
                }
                if (code == "BT")
                {
                    if (bankAccount != null)
                    {
                        IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                        var glaAccount = glAccountQuery.GetSingleGLAccountPM(bankAccount.GLAccountId, tenant);
                        if (glaAccount != null)
                        {
                            if (glaAccount.CurrencyId != paymentCurrencyId)
                            {
                                string msg = TranslateTextsClass.Translate("ARPayment.M.BanckAccountGLAccount", tenant, useLocal);
                                errors += msg.Replace("%", glaAccount.DisplayNumber) + ";";
                                errors += msg + ";";
                            }
                        }
                    }

                    else
                    {
                        if (!isOut)
                        {
                            string msg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, useLocal);
                            errors += msg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.BankAccountId", tenant, useLocal)) + ";";
                        }
                    }

                }

                if(code == "CH" && aRPaymentChequeReplicas?.Count > 1)
                {
                    errors = ValidateDuplicateChequeNumber(aRPaymentChequeReplicas, tenant, errors, useLocal);
                }

                if (!string.IsNullOrEmpty(errors))
                {
                    errors = errors.TrimEnd(';');
                    throw new ApplicationException(errors);
                }
            }
        }

        private static string ValidateDuplicateChequeNumber(List<ARPaymentChequeReplicaPM> aRPaymentChequeReplicas, int tenant, string errors, bool useLocal)
        {
            foreach (ARPaymentChequeReplicaPM aRPaymentCheque in aRPaymentChequeReplicas)
            {
                bool isDuplicateChequeNumber = aRPaymentChequeReplicas.FindAll(c => c.ChequeNumber == aRPaymentCheque.ChequeNumber).Count > 1;
                if (isDuplicateChequeNumber)
                {
                    errors += TranslateTextsClass.Translate("Accounting.M.MoreThanChequeWithTheSameChequeNumber", tenant, useLocal);
                    break;
                }
            }

            return errors;
        }

        public static void ValidateValueDate(DateTime? valueDate, DateTime? registerDate, int tenant)
        {
            DateTime date = registerDate.Value.AddDays(-180);
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocal =(bool) !contact?.DontShowLocal;
            if (valueDate < date)
            {
                throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.OlderThan180Days",tenant , showLocal));
            }

        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

        private static bool IsEditingEntityEnabled(ARPayment entityPOCO)
        {
            bool myResult = false;

            if (entityPOCO != null)
            {
                if (string.IsNullOrEmpty(entityPOCO.StatusCode) || entityPOCO.StatusCode == "DR")
                {
                    myResult = true;
                }
            }

            return myResult;
        }
    }
}