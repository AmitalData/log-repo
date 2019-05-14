using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Transactions;

namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class APPaymentValidator
    {
        public static void Validate(APPaymentPM entityPM)
        {
            int tenant = entityPM.Tenant;

            string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant);

            string paymentMethodCode = "";
            APPaymentMethodRepository paymentMethodRepository = new APPaymentMethodRepository(tenant);
            APPaymentMethod paymentMethod = paymentMethodRepository.GetSingleAPPaymentMethod(entityPM.AccountingPaymentMethodId, tenant);
            if(paymentMethod != null)
            {
                paymentMethodCode = paymentMethod.Code;
            }

            bool isNegativeAmountEnabled = false;
            if (paymentMethodCode == "FS")
            {
                ICommonDataContext myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);

                AccountingSetting myAccountingSetting = (from d in myCommonContext.AccountingSettings
                                                         where d.Id == entityPM.Tenant
                                                         select d).FirstOrDefault();

                if (myAccountingSetting != null)
                {
                    if (myAccountingSetting.EnableNegativeOffsetAPPayments)
                    {
                        isNegativeAmountEnabled = true;
                    }
                }
            }

            if (entityPM.RegisterDate > TenantServerConfigration.GetCurrentDateTime(tenant))
            {
                string msg = TranslateTextsClass.Translate("APPayment.M.CantSetFutureDatePayment", tenant);
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
                    string msg = TranslateTextsClass.Translate("APPayment.M.CantSetZeroAmount", tenant);
                    throw new ApplicationException(msg);
                }
            }

            if (paymentMethodCode == "CH")
            {
                if (string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef))
                {
                    throw new ApplicationException(rmsg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.ChequeOrPaymentRef", tenant)));
                }
            }

            if (paymentMethodCode == "CC")
            {
                if (string.IsNullOrEmpty(entityPM.CreditCardTypeId))
                {
                    throw new ApplicationException(rmsg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.CreditCardTypeId", tenant)));
                }
            }

            if (entityPM.HasInvoicesErrors)
            {
                string msg = TranslateTextsClass.Translate("APPayment.M.PaymentInvoicesHasErrors", tenant);
                throw new ApplicationException(msg);
            }

            double? result = entityPM.PaymentInvoices.Where(a => a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Sum(d => d.PaymentAmount);
            double? paymentAmountPaid = MethodHelper.Roundd(result, 2);

            if (isNegativeAmountEnabled == false)
            {
                if (entityPM.AmountInPaymentCurrency < 0)
                {
                    string msg = TranslateTextsClass.Translate("APPayment.M.CantSetMinusAmount", tenant);
                    throw new ApplicationException(msg);
                }

                if (paymentAmountPaid < 0)
                {
                    string msg = TranslateTextsClass.Translate("APPayment.M.PaymentAmountPaidCantBeMinus", tenant);
                    throw new ApplicationException(msg);
                }
            }

            if (paymentAmountPaid > entityPM.AmountInPaymentCurrency)
            {
                string msg = TranslateTextsClass.Translate("APPayment.M.PaymentAmountPaidCantBeBigger", tenant);
                throw new ApplicationException(msg);
            }

            if (entityPM.PaymentInvoices.Where(a => a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && (a.ForeignAmount == null || a.ForeignAmount == 0)).Any())
            {
                throw new ApplicationException("Can't connect lines with zero Amount to Pay");
            }

            ValidateAirlineRestriction(entityPM.VendorId, tenant);
            ValidateFullAccounting(entityPM);
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
                                throw new ApplicationException("Vendor Airline is not allowed");
                            }
                        }
                    }
                }
            }
        }

        private static void ValidateFullAccounting(APPaymentPM entityPM)
        {
            var errors = "";
            var tenant = entityPM.Tenant;
            bool useLocal = true;
            ContactPM user = GetLoggedContact(tenant);
            useLocal = user == null ? true : (!user.DontShowLocal);

            string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, useLocal);
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {



                if (entityPM.PaymentMethodCode == "BT" && entityPM.ValueDate != null && entityPM.ValueDate > TenantServerConfigration.GetCurrentDateTime(tenant))
                {
                    string msg = TranslateTextsClass.Translate("APPayment.M.ValueDateCantBeFutureDate", tenant, useLocal);
                    errors += msg + ";";
                }

                if(entityPM.BankAccountId == null 
                    && (entityPM.PaymentMethodCode == "BT" || entityPM.PaymentMethodCode == "CH" || entityPM.PaymentMethodCode == "CC"))
                {
                    errors += (rmsg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.BankAccountId", tenant, useLocal))) + ";";
                }

                if (entityPM.TaxDeductionPercentage == null)
                {
                    errors += (rmsg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.TaxDeductionPercentage", tenant, useLocal))) + ";";
                }

                if (entityPM.TaxDeductionLocalAmount == null)
                {
                    errors += (rmsg.Replace("%FieldName", TranslateTextsClass.Translate("APPayment.F.TaxDeductionLocalAmount", tenant, useLocal))) + ";";
                }

                decimal? percentage = null;
                IGLAccountWithholdingTaxQueryServiceExt gLAccountWithholdingTaxQueryService = ContainerAccessor.Container.Resolve(typeof(IGLAccountWithholdingTaxQueryServiceExt), "GLAccountWithholdingTaxQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountWithholdingTaxQueryServiceExt;
                CardRepository cardRep = new CardRepository(tenant);
                Card card = cardRep.GetSingleCard(entityPM.VendorId, tenant);
                GLAccountWithholdingTaxPM withholdingTaxPM = gLAccountWithholdingTaxQueryService.GetAccountWithholdingTaxPMByglAccountAndDate(card.GLAccountId, entityPM.RegisterDate, tenant);
                if (withholdingTaxPM == null)
                {
                    IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                    FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
                    percentage = accountingSettings!= null ? accountingSettings.DefaultTaxWithholdPercentage: null;
                    if (percentage == null)
                    {
                        errors += (TranslateTextsClass.Translate("Accounting.O.MissingDefaultPercentage", tenant, useLocal));
                    }
                }
                
                if (!string.IsNullOrEmpty(errors))
                {
                    errors = errors.TrimEnd(';');
                    throw new ApplicationException(errors);
                }
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
    }
}