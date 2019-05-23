using System;
using System.Linq;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;

using Logitude.BL.CommonDataModel;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using System.Collections.Generic;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InvoiceModel;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;

namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class APInvoiceValidator
    {
        public static void Validate(APInvoicePM entityPM, IInvoiceContext myContext, string MainShipmentConcurrencyGUID = null)
        {
            string msgRequired = TranslateTextsClass.Translate("General.M.FieldIsRequired", entityPM.Tenant);

            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(myContext);
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);

            AccountingSetting myAccountingSetting = (from d in myCommonContext.AccountingSettings
                                                     where d.Id == entityPM.Tenant
                                                     select d).FirstOrDefault();

            bool isVatNumberMandatoryInAP = false;
            if (myAccountingSetting != null)
            {
                isVatNumberMandatoryInAP = myAccountingSetting.IsVatNumberMandatoryInAP;
            }

            if (entityPM.InvoiceDate > TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant))
            {
                string msg = TranslateTextsClass.Translate("APInvoice.M.CantReceiveFutureDateInvoice", entityPM.Tenant);
                throw new ApplicationException(msg);
            }

            if (entityPM.InvoiceDate > entityPM.AccountingDate)
            {
                string msg = TranslateTextsClass.Translate("APInvoice.O.CheckInvoiceDate", entityPM.Tenant, !(GetLoggedContact(entityPM.Tenant).DontShowLocal));//.t "nvoice Date cant be bigger the the Accounting Date"; // TranslateTextsClass.Translate("APInvoice.M.CantReceiveFutureDateInvoice", entityPM.Tenant);
                throw new ApplicationException(msg);
            }

            if (isVatNumberMandatoryInAP)
            {
                if (string.IsNullOrEmpty(entityPM.VATNumber))
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", TranslateTextsClass.Translate("APInvoice.F.VATNumber", entityPM.Tenant)));
                }
            }


            List<APInvoiceLinePM> activeLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            List<string> allVatsIds = (from d in activeLines
                                       where d.VatTypeId != null
                                       group d by d.VatTypeId into g
                                       select g.Key).ToList();

            List<VatType> allVats = (from f in myCommonContext.VatTypes
                                     where allVatsIds.Contains(f.Id)
                                     && f.Tenant == entityPM.Tenant
                                     select f).ToList();

            if (entityPM.IsMultipleEntities)
            {
                #region
                if (entityPM.SetApproved)
                {
                    if (entityPM.InvoiceMultipleShipments.Count == 0)
                    {
                        string msg = TranslateTextsClass.Translate("APInvoice.M.YouShouldHaveOneLineAtLeast", entityPM.Tenant);
                        throw new ApplicationException(msg);
                    }

                    else
                    {
                        double? invoiceAmount = (double)MethodHelper.Round(entityPM.AmountInInvoiceCurrency, 2);

                        if (invoiceAmount == 0 || invoiceAmount == null)
                        {
                            throw new ApplicationException(msgRequired.Replace("%FieldName", TranslateTextsClass.Translate("APInvoice.F.AmountInInvoiceCurrency", entityPM.Tenant)));
                        }

                        else
                        {
                            //double? d1 = entityPM.InvoiceMultipleShipments.Sum(s => s.SubTotalInInvoiceCurrency);
                            //double? d2 = entityPM.InvoiceMultipleShipments.Sum(s => s.TotalVATAmount);
                            //d1 = (double)MethodHelper.Round(d1, 2);
                            //d2 = (double)MethodHelper.Round(d2, 2);

                            double? d1 = entityPM.SubTotalInInvoiceCurrency;
                            double? d2 = entityPM.TotalVATs.Sum(s => s.InvoiceCurrencyVATAmount);
                            d1 = (double)MethodHelper.Round(d1, 2);
                            d2 = (double)MethodHelper.Round(d2, 2);
                            double myComputedTotalAmount = (double)MethodHelper.Round(d1 + d2, 2);

                            if (invoiceAmount != myComputedTotalAmount)
                            {
                                string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceAmountNotMatched", entityPM.Tenant);
                                throw new ApplicationException(msg);
                            }
                        }
                    }
                }
                #endregion
            }

            else
            {
                if (activeLines.Count == 0)
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.YouShouldHaveOneLineAtLeast", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }

                if (entityPM.InvoiceExpectedAmount == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", TranslateTextsClass.Translate("APInvoice.F.AmountInInvoiceCurrency", entityPM.Tenant)));
                }

                else if (entityPM.InvoiceExpectedAmount != entityPM.AmountInInvoiceCurrency)
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceAmountNotMatched", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }

                foreach (APInvoiceLinePM item in activeLines)
                {
                    if (item.InvoiceCurrencyAmount == 0)
                    {
                        string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceLineAmountNotZero", entityPM.Tenant);
                        throw new ApplicationException(msg);
                    }

                    if (item.VatTypeId == null)
                    {
                        string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatTypeId", entityPM.Tenant);
                        throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                    }

                    else
                    {
                        if (item.VatPercentage == null)
                        {
                            VatType vattType = allVats.Where(d => d.Id == item.VatTypeId).FirstOrDefault();
                            if (vattType != null)
                            {
                                if (!vattType.IsMultiPercentage)
                                {
                                    string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatPercentage", entityPM.Tenant);
                                    throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                                }
                            }
                        }
                    }
                }

                ValidateMultiVatPercentages(entityPM, myAccountingSetting, allVats);
                ValidateShipmentConcurrencyGUID(entityPM, MainShipmentConcurrencyGUID);
            }

            ValidateOnVoid(entityPM);
            ValidateAirlineRestriction(entityPM.VendorId, entityPM.Tenant);
            ValidateFullAccounting(entityPM.Tenant, entityPM.VendorId, entityPM.InvoiceCurrencyId, entityPM.AccountingDate);
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
        private static void ValidateMultiVatPercentages(APInvoicePM entityPM, AccountingSetting accountingSetting, List<VatType> allVats)
        {
            if (entityPM.StatusCode == null || entityPM.StatusCode == "WA")
            {
                if (allVats.Count > 0)
                {
                    if (accountingSetting != null)
                    {
                        if (!accountingSetting.EnableMultiPercentageVATTypes)
                        {
                            if (allVats.Where(d => d.IsMultiPercentage).Any())
                            {
                                throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                            }
                        }
                    }
                }
            }
        }
        public static void ValidateFullAccounting(int tenant, string vendorId, string invoiceCurrencyId, DateTime? accountingDate)
        {
            var errors = "";

            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {
                bool useLocal = true;
                var user = GetLoggedContact(tenant);
                if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                GLAccountPM glAccount = getGLAccount(vendorId, tenant);

                if (glAccount == null)
                {

                    string msg = TranslateTextsClass.Translate("APInvoice.M.VendorNoGLAccount",tenant, useLocal);
                    errors += msg + ";";
                }
                if (glAccount != null && (glAccount.IsMultiCurrency == null || glAccount.IsMultiCurrency == false))
                {
                    if (glAccount.CurrencyId != invoiceCurrencyId)
                    {
                        string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceCurrNotMatch", tenant, useLocal)  + " "+ glAccount.CurrencyName + " ";
                        errors += msg + ";";
                    }
                }

                IAccountingContext myContext = AccountingContext.GetContext(tenant);
                AccountingPeriodListQueryService accountingPeriodQuery = new AccountingPeriodListQueryService(myContext);
                AccountingPeriodList accountingPeriodList = accountingPeriodQuery.GetByYear(accountingDate.Value.Year, "1", tenant);
                if (accountingPeriodList != null && accountingDate != null)
                {
                    var month = accountingDate.Value.Month;
                    if (month > accountingPeriodList.OpenMonth || month <= accountingPeriodList.ClosedMonth)
                    {
                        
                        string msg = TranslateTextsClass.Translate("Accounting.General.O.ClosedMonth", tenant, useLocal);
                        errors += msg + ";";
                    }
                }
                else
                {
                    string msg = TranslateTextsClass.Translate("Accounting.General.O.ClosedMonth", tenant, useLocal);
                    errors += msg + ";";
                }
                if (!string.IsNullOrEmpty(errors))
                {
                    errors = errors.TrimEnd(';');
                    throw new ApplicationException(errors);
                }
            }
        }
        private static GLAccountPM getGLAccount(string vendorId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(vendorId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
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
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() {  };
            return loggedContact;
        }
        private static void ValidateOnVoid(APInvoicePM entityPM)
        {
            if (entityPM.SetVoided)
            {               
                if (entityPM.InvoicePayments.Count > 0)
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.DisconnectPayments", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }
        }

        private static void ValidateShipmentConcurrencyGUID(APInvoicePM entityPM, string MainShipmentConcurrencyGUID)
        {
            if (!string.IsNullOrEmpty(entityPM.ShipmentConcurrencyGUID) && !string.IsNullOrEmpty(entityPM.ShipmentNewConcurrencyGUID) && !string.IsNullOrEmpty(MainShipmentConcurrencyGUID))
            {
                if (!entityPM.ShipmentConcurrencyGUID.Equals(MainShipmentConcurrencyGUID) && !entityPM.ShipmentNewConcurrencyGUID.Equals(MainShipmentConcurrencyGUID))
                {
                    string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                    throw new OptimisticConcurrencyException(msg);
                }
            }
        }
    }
}