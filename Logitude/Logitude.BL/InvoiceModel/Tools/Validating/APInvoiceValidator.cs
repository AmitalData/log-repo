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
using System.Web;
using Simplog.Server.Infrastructure;
using Logitude.BL.DataContracts;
using System.Text.RegularExpressions;

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
            ValidateFullAccounting(entityPM.Tenant, entityPM.VendorId, entityPM.InvoiceCurrencyId, entityPM.AccountingDate, entityPM.InvoiceNumber);
            ValidateExternalAPI(entityPM, myCommonContext);
        }

        private static void ValidateExternalAPI(APInvoicePM entityPM, ICommonDataContext myCommonContext)
        {
            if (entityPM.CreatedFromAPI)
            {
                int tenant = entityPM.Tenant;
                Tenant TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();

                if (entityPM.LocalCurrencyId != TenantObject.CurrencyId)
                {
                    throw new ApplicationException("The local currency is different from Tenant local currency");
                }

                #region Line Amounts
                foreach (APInvoiceLinePM item in entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
                {
                    if (item.ChargesTypeId == null)
                    {
                        throw new ApplicationException("Charges type is required");
                    }

                    else
                    {
                        ChargesType chargesType = (from d in myCommonContext.ChargesTypes where d.Id == item.ChargesTypeId && d.Tenant == tenant select d).FirstOrDefault();
                        if (chargesType == null)
                        {
                            throw new ApplicationException("Charges type is required");
                        }

                        else
                        {
                            bool isMatched = true;

                            switch (entityPM.ShipmentTransportModeId)
                            {
                                case "A":
                                    {
                                        if (!chargesType.IsAir)
                                        {
                                            isMatched = false;
                                        }

                                        break;
                                    }

                                case "O":
                                    {
                                        if (!chargesType.IsOcean)
                                        {
                                            isMatched = false;
                                        }

                                        break;
                                    }

                                case "I":
                                    {
                                        if (!chargesType.IsInland)
                                        {
                                            isMatched = false;
                                        }

                                        break;
                                    }
                            }

                            if (!isMatched)
                            {
                                throw new ApplicationException("Charge type isn't compatible with the shipment transport mode");
                            }
                        }
                    }

                    double? lineForiegnAmount = MethodHelper.Round(item.ForiegnCurrencyAmount, 2);
                    //double? lineForiegnAmount_Computed = MethodHelper.Round(item.Quantity * item.UnitPrice, 2);
                    //if (lineForiegnAmount != lineForiegnAmount_Computed)
                    //{
                    //    throw new ApplicationException("Wrong Line Foriegn Amount");
                    //}


                    double? lineLocalAmount = MethodHelper.Round(item.LocalCurrencyAmount, 2);
                    double? lineLocalAmount_Computed = MethodHelper.Round(item.ForiegnCurrencyAmount * item.ForiegnExchangeRate, 2);
                    if (lineLocalAmount != lineLocalAmount_Computed)
                    {
                        throw new ApplicationException("Wrong Line Local Amount");
                    }

                    double? lineInvoiceAmount = MethodHelper.Round(item.InvoiceCurrencyAmount, 2);
                    double? exchangeRate = MethodHelper.Round(entityPM.InvoiceCurrencyExchangeRate, 2);
                    double? lineInvoiceAmount_Computed = MethodHelper.Round((item.LocalCurrencyAmount / exchangeRate), 2);
                    if (item.ForiegnCurrencyId == entityPM.InvoiceCurrencyId)
                    {
                        if (lineInvoiceAmount != lineForiegnAmount)
                        {
                            throw new ApplicationException("Wrong Line Invoice Amount");
                        }
                    }

                    else
                    {
                        if (lineInvoiceAmount != lineInvoiceAmount_Computed)
                        {
                            throw new ApplicationException("Wrong Line Invoice Amount");
                        }
                    }
                }
                #endregion

                #region Lines Amounts VS Invoice Amount
                double? subTotal = 0;
                double? subTotal_Local = 0;
                double? sumOfVATsAmounts = 0;
                double? sumOfVATsAmounts_Local = 0;
                double? sumOfVATsAmounts_Profit = 0;
                double? Amount = 0;
                double? Amount_Local = 0;
                double? Amount_Profit = 0;
                List<APInvoiceLinePM> lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();

                if (lines.Count > 0)
                {
                    subTotal = MethodHelper.Round(lines.Sum(s => s.InvoiceCurrencyAmount), 2);
                    subTotal_Local = MethodHelper.Round(lines.Sum(s => s.LocalCurrencyAmount), 2);

                    #region
                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                    List<VatType> allVatTypes = (from d in myCommonContext.VatTypes where d.Tenant == tenant select d).ToList();
                    List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();

                    VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                    VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                    List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, todayDate);

                    List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

                    foreach (APInvoiceLinePM item in lines)
                    {
                        #region
                        VatType lineVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                        if (lineVatType != null)
                        {
                            if (!lineVatType.IsMultiPercentage)
                            {
                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                {
                                    Id = item.VatTypeId,
                                    VatTypeId = item.VatTypeId,
                                    VatTypePercentage = item.VatPercentage,
                                    LocalCurrencyAmount = item.LocalCurrencyAmount,
                                    InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                    ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                };

                                group_Source.Add(newItem);
                            }

                            else
                            {
                                List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                                foreach (VATTypesGroup itemGroup in myVatGroups)
                                {
                                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                    {
                                        Id = itemGroup.SingleVATTypeId,
                                        VatTypeId = itemGroup.SingleVATTypeId,
                                        LocalCurrencyAmount = item.LocalCurrencyAmount,
                                        InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                        ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                    };

                                    VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (vatType != null)
                                    {
                                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                    }

                                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (myPercentagePM != null)
                                    {
                                        newItem.VatTypePercentage = myPercentagePM.Percentage;
                                    }

                                    group_Source.Add(newItem);
                                }
                            }
                        }
                        #endregion
                    }

                    List<InvoiceTotalsClass> group_data
                        = (from items in group_Source
                           group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
                           select new InvoiceTotalsClass()
                           {
                               Id = g.Key.VatTypeId,
                               VatTypeId = g.Key.VatTypeId,
                               VatTypePercentage = g.Key.VatTypePercentage,
                               LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                               InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                               ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                           }).ToList();

                    foreach (InvoiceTotalsClass item in group_data)
                    {
                        ARInvoiceTotalVAT record = new ARInvoiceTotalVAT()
                        {
                            Tenant = entityPM.Tenant,
                            ARInvoiceId = entityPM.Id,
                            VatTypeId = item.Id,
                            VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                            LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                            InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                            ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                        };

                        record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VatPercent / 100), 2);
                        record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VatPercent / 100), 2);
                        record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VatPercent / 100), 2);

                        sumOfVATsAmounts += record.InvoiceCurrencyVATAmount;
                        sumOfVATsAmounts_Local += record.LocalVATAmount;
                        sumOfVATsAmounts_Profit += record.ProfitCurrencyVATAmount;
                    }

                    Amount = MethodHelper.Round(subTotal + sumOfVATsAmounts, 2);
                    Amount_Local = MethodHelper.Round(subTotal_Local + sumOfVATsAmounts_Local, 2);

                    if (entityPM.ProfitCurrencyId == entityPM.InvoiceCurrencyId)
                    {
                        Amount_Profit = Amount;
                    }

                    else
                    {
                        Amount_Profit = MethodHelper.Round(Amount_Local / entityPM.ProfitCurrencyExchangeRate, 2);
                    }
                    #endregion
                }

                if (entityPM.SubTotalInInvoiceCurrency != subTotal)
                {
                    throw new ApplicationException("Wrong Sub Total Amount");
                }

                if (entityPM.SubTotalInLocalCurrency != subTotal_Local)
                {
                    throw new ApplicationException("Wrong Sub Total Local Amount");
                }

                if (entityPM.AmountInInvoiceCurrency != Amount)
                {
                    throw new ApplicationException("Wrong Invoice Total Amount");
                }

                if (entityPM.AmountInLocalCurrency != Amount_Local)
                {
                    throw new ApplicationException("Wrong Invoice Total Local Amount");
                }

                //entityPM.SubTotalInInvoiceCurrency = subTotal;
                //entityPM.SubTotalInLocalCurrency = subTotal_Local;
                //entityPM.AmountInInvoiceCurrency = Amount;
                //entityPM.AmountInLocalCurrency = Amount_Local;
                //entityPM.AmountInProfitCurrency = Amount_Profit;
                #endregion

                #region Local Amount
                double? localAmount = MethodHelper.Round(entityPM.AmountInLocalCurrency, 2);
                //   double? rate = MethodHelper.Round(entityPM.InvoiceCurrencyExchangeRate, 2);
                double? localAmount_Computed = MethodHelper.Round(entityPM.AmountInInvoiceCurrency * entityPM.InvoiceCurrencyExchangeRate, 2);
                if (localAmount != localAmount_Computed)
                {
                    throw new ApplicationException("Wrong Invoice Local Amount");
                }
                #endregion
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
        public static void ValidateFullAccounting(int tenant, string vendorId, string invoiceCurrencyId, DateTime? accountingDate, string invoiceNumber)
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
                Regex regex= new Regex("^[A-Za-z0-9]*$");
                if (!regex.IsMatch(invoiceNumber))
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.O.InvalidNumber", tenant, useLocal);
                    errors += msg + ";";

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

        public static string ValidateFullAccountingInvoiceDate(DateTime? invoiceDate, int tenant, string email)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {
              
                bool useLocal = !(GetLoggedContact(tenant,email).DontShowLocal);
            
                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime last180days = date.AddDays(-180);
                if (invoiceDate < last180days)
                {
                    return TranslateTextsClass.Translate("Accounting.General.O.InvoiceDateValidation", tenant, useLocal);
                }
                else return null;
            }
            else return null;
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
        private static ContactPM GetLoggedContact( int tenant, string email = null)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = null;
            if (email != null)
            {
                loggedContact= GetLoggedContactByAuthTokenEmail(email, tenant);
            }
            else
            {
                loggedContact = new ContactQuery(tenant).GetSingleByEmail( AuthenticationUtil.ResolveUserIdentityName(tenant) , tenant);
            }
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetSingleByEmail("system@tenant" + tenant + ".com", tenant);
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

        private static ContactPM GetLoggedContactByAuthTokenEmail(string email, int tenant)
        {
            ContactPM loggedContact = null;
            loggedContact = new ContactQuery(tenant).GetSingleByEmail(email, tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetSingleByEmail(email, 0);
            }
            return loggedContact;
        }
        private static void ValidateShipmentConcurrencyGUID(APInvoicePM entityPM, string MainShipmentConcurrencyGUID)
        {
            if (!entityPM.CreatedFromAPI)
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
}