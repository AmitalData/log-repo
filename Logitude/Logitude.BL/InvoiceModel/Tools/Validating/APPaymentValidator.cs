using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
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
        public static void Validate(APPaymentPM entityPM, APPayment entityPOCO, bool isNew)
        {
            int tenant = entityPM.Tenant;

            string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant);

            string paymentMethodCode = "";
            //APPaymentMethodRepository paymentMethodRepository = new APPaymentMethodRepository(tenant);
            //APPaymentMethod paymentMethod = paymentMethodRepository.GetSingleAPPaymentMethod(entityPM.AccountingPaymentMethodId, tenant);

            AccountingPaymentMethodRepository paymentMethodRepository = new AccountingPaymentMethodRepository(tenant);
            AccountingPaymentMethod paymentMethod = paymentMethodRepository.GetSingleAccountingPaymentMethod(entityPM.AccountingPaymentMethodId, tenant);            
            if (paymentMethod != null)
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
            if (entityPM.RegisterDate > entityPM.ValueDate && entityPM.PaymentMethodCode == "BT")
            {
                string msg = TranslateTextsClass.Translate("APPayment.M.ValueDateBiggerOrEqualRegisterDate", tenant);
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
                    }
                }

                if (!isAllowed)
                {
                    string msg = TranslateTextsClass.Translate("APPayment.M.CantSetZeroAmount", tenant);
                    throw new ApplicationException(msg);
                }
            }
          
            if (paymentMethodCode == "CH" && !entityPM.AutomaticPaymentCheque)
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
                if (paymentMethodCode.ToUpper() != "FS")
                {
                    throw new ApplicationException("Can't connect lines with zero Amount to Pay");
                }
            }

            ValidateAirlineRestriction(entityPM.VendorId, tenant);
            ValidateFullAccounting(entityPM);
            ValidateUnUpdateFields(entityPM, entityPOCO, isNew);
            ValidateOnVoiding(entityPM);
        }

        private static void ValidateUnUpdateFields(APPaymentPM entityPM, APPayment entityPOCO, bool isNew)
        {
            if (!isNew)
            {
                bool isEditingEnabled = IsEditingEntityEnabled(entityPOCO);

                if (!isEditingEnabled)
                {
                    if (entityPM.PaymentCurrencyExchangeRate != entityPOCO.PaymentCurrencyExchangeRate)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("APPayment.F.PaymentCurrencyExchangeRate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.AmountInPaymentCurrency != entityPOCO.AmountInPaymentCurrency)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("APPayment.F.AmountInPaymentCurrency", entityPM.Tenant);
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
            
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);

            string rmsg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant, useLocal);
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {


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

                GLAccountPM glAccount = getGLAccount(entityPM.VendorId, tenant);
                if (glAccount == null)
                {

                    string msg = TranslateTextsClass.Translate("APPayment.O.VendorGLAccount", tenant, useLocal);
                    errors += msg + ";";
                    //throw new ApplicationException(msg);
                }



                decimal? percentage = null;
                IGLAccountWithholdingTaxQueryServiceExt gLAccountWithholdingTaxQueryService = ContainerAccessor.Container.Resolve(typeof(IGLAccountWithholdingTaxQueryServiceExt), "GLAccountWithholdingTaxQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountWithholdingTaxQueryServiceExt;
                CardRepository cardRep = new CardRepository(tenant);
                Card card = cardRep.GetSingleCard(entityPM.VendorId, tenant);


                if (entityPM.SetApproved && card.CountryId == null)
                {
                    string msg = TranslateTextsClass.Translate("APPayment.O.NoVendorCountry", tenant, useLocal);
                    errors += msg + ";";
                }

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

        private static GLAccountPM getGLAccount(string vandorId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(vandorId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }

        public static FullAccountingSettingPM GetFullAccountingSetting(APPaymentPM entityPM)
        {
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            return query.GetFullAccountingSettingByTenant(entityPM.Tenant );

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

        private static bool IsEditingEntityEnabled(APPayment entityPOCO)
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

        private static void ValidateOnVoiding(APPaymentPM entityPM)
        {
            if (entityPM.SetVoided)
            {
                bool hasConnectedInvoices = entityPM.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Any();
                bool hasExternalPaymentAmount = (entityPM.ExternalPaymentAmount != null && entityPM.ExternalPaymentAmount != 0) ? true : false;

                if (hasConnectedInvoices || hasExternalPaymentAmount)
                {
                    string msg = null;

                    if (hasConnectedInvoices && hasExternalPaymentAmount)
                    {
                        msg = "Please disconnect all invoices and external payment amount";
                    }

                    else if (hasConnectedInvoices && !hasExternalPaymentAmount)
                    {
                        msg = TranslateTextsClass.Translate("APPayment.M.DisconnectInvoices", entityPM.Tenant);
                    }

                    else if (!hasConnectedInvoices && hasExternalPaymentAmount)
                    {
                        msg = "Please disconnect external payment amount";
                    }

                    throw new ApplicationException(msg);
                }
            }
        }
    }
}