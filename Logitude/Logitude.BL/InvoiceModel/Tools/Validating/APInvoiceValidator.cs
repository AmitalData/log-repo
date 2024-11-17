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

using Logitude.BL.Resolvers;

using System.Text.RegularExpressions;
using System.Data.Entity.Core.Objects;
using Logitude.BL.CommonDataModel.EntityLists;


namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class APInvoiceValidator
    {
        public static void Validate(APInvoicePM entityPM, APInvoice entityPOCO, bool isNew, IInvoiceContext context, ICommonDataContext commonContext, string MainShipmentConcurrencyGUID = null)
        {
            string msgRequired = TranslateTextsClass.Translate("General.M.FieldIsRequired", entityPM.Tenant);

            if (!isNew)
            {
                ValidateConcurrencyGUID(entityPM, entityPOCO);
            }

            ValidateInvoiceFields(entityPM);

            AccountingSetting accountingSetting = (from d in commonContext.AccountingSettings where d.Id == entityPM.Tenant select d).FirstOrDefault();
            if (accountingSetting != null)
            {
                if (accountingSetting.IsVatNumberMandatoryInAP)
                {
                    if (string.IsNullOrEmpty(entityPM.VATNumber))
                    {
                        throw new ApplicationException(msgRequired.Replace("%FieldName", TranslateTextsClass.Translate("APInvoice.F.VATNumber", entityPM.Tenant)));
                    }
                }

                if (!accountingSetting.EnableEnteringTotalVAT)
                {
                    if (entityPM.TotalVATOnly)
                    {
                        if (isNew)
                        {
                            throw new ApplicationException("Tenant setting doesn’t allow total VATs");
                        }

                        else if (!entityPOCO.TotalVATOnly)
                        {
                            throw new ApplicationException("Tenant setting doesn’t allow total VATs");
                        }
                    }
                }
            }

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

                        if (entityPM.AmountInInvoiceCurrency == null)
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
                if (entityPM.InvoiceExpectedAmount == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", TranslateTextsClass.Translate("APInvoice.F.AmountInInvoiceCurrency", entityPM.Tenant)));
                }

                else if (entityPM.InvoiceExpectedAmount != entityPM.AmountInInvoiceCurrency)
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceAmountNotMatched", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }

                ValidateInvoiceAmountDue(entityPM, entityPOCO, isNew, context);
                ValidateNormalInvoiceLines(entityPM, commonContext, accountingSetting, msgRequired);
                ValidateShipmentConcurrencyGUID(entityPM, MainShipmentConcurrencyGUID);
            }

            ValidateOnVoid(entityPM);
            ValidateAirlineRestriction(entityPM.VendorId, entityPM.Tenant);
            ValidateFullAccounting(entityPM, isNew);
            ValidateExternalAPI(entityPM, commonContext);
            ValidateUnUpdateFields(entityPM, entityPOCO, isNew);
        }

        private static void ValidateNormalInvoiceLines(APInvoicePM entityPM, ICommonDataContext commonContext, AccountingSetting accountingSetting, string msgRequired)
        {
            List<APInvoiceLinePM> activeLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            Tenant tenantPOCO = GetTenant(entityPM.Tenant);
            if (activeLines.Count == 0)
            {
                string msg = TranslateTextsClass.Translate("APInvoice.M.YouShouldHaveOneLineAtLeast", entityPM.Tenant);
                throw new ApplicationException(msg);
            }

            else if (activeLines.Where(d => d.InvoiceCurrencyAmount == 0).Any())
            {
                if (entityPM.CreatedFromAPI && entityPM.IsGeneralInvoice && tenantPOCO.AccountingActivated) { }
                else
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceLineAmountNotZero", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }

            else
            {
                if (entityPM.TotalVATOnly)
                {
                    ValidateTotalVATOnly(entityPM, commonContext, accountingSetting, msgRequired);
                }

                else
                {
                    ValidateInvoiceLinesVAT(entityPM, commonContext, accountingSetting, msgRequired);
                }
            }
        }

        private static void ValidateTotalVATOnly(APInvoicePM entityPM, ICommonDataContext commonContext, AccountingSetting accountingSetting, string msgRequired)
        {
            List<APInvoiceTotalVATPM> activeTotalVats = entityPM.TotalVATs.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            if (activeTotalVats.Count == 0)
            {
                throw new ApplicationException("You should have at least 1 invoice total VAT");
            }

            else
            {
                List<VatType> allVatTypes = (from f in commonContext.VatTypes where f.Tenant == entityPM.Tenant select f).ToList();

                foreach (APInvoiceTotalVATPM item in activeTotalVats)
                {
                    if (item.VatTypeId == null)
                    {
                        string field = TranslateTextsClass.Translate("APInvoiceTotalVAT.F.VatTypeId", entityPM.Tenant);
                        throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                    }

                    else if (item.VatPercent == null)
                    {
                        VatType itemVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                        if (itemVatType != null)
                        {
                            if (itemVatType.IsMultiPercentage)
                            {
                                if (entityPM.StatusCode == null || entityPM.StatusCode == "WA")
                                {
                                    if (!accountingSetting.EnableMultiPercentageVATTypes)
                                    {
                                        throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                                    }
                                }
                            }

                            else
                            {
                                string field = TranslateTextsClass.Translate("APInvoiceTotalVAT.F.VatPercent", entityPM.Tenant);
                                throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                            }
                        }
                    }
                }


                List<APInvoiceLinePM> activeLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
                if (activeLines != null && activeLines.Count > 0 && activeTotalVats != null && activeTotalVats.Count > 0)
                {
                    List<string> expenses = new List<string>();

                    List<string> chTypeIds = activeLines.Select(ln => ln.ChargesTypeId).ToList();
                    if (chTypeIds != null && chTypeIds.Count > 0)
                    {

                        ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(entityPM.Tenant);
                        IQueryable<ChargesTypeList> query = chargesTypeQuery.GetChargesTypeListsByTenant(entityPM.Tenant);
                        if (query != null)
                        {
                            expenses = query.Where(ch => ch.IsExpense == true).Select(ch => ch.Id).ToList();
                        }
                    }



                    double? localtotal = 0;
                    double? totallines = 0;
                    if (expenses != null && expenses.Count > 0)
                    {
                        foreach (APInvoiceLinePM line in activeLines)
                        {
                            var chTypeId = line.ChargesTypeId;
                            if (chTypeId == null || (chTypeId != null && !expenses.Contains(chTypeId)))
                            {
                                localtotal += line.LocalCurrencyAmount;
                            }
                        }
                        totallines = activeLines.Sum(ln => ln.LocalCurrencyAmount);
                    }
                    else
                    {
                        localtotal = activeLines.Sum(ln => ln.LocalCurrencyAmount);
                        totallines = localtotal;
                    }
                    double totalVat = activeTotalVats.Sum(tv => tv.LocalVATAmount);
                    if (localtotal > 0 && totalVat < 0)
                    {
                        throw new ApplicationException("Reference " + entityPM.InvoiceNumber + ":   total lines is " + totallines.ToString() + ", of which reportable amount " + localtotal.ToString() + " is positive,  but VAT " + totalVat + " is negative");
                    }
                    else if (localtotal < 0 && totalVat > 0)
                    {
                        throw new ApplicationException("Reference " + entityPM.InvoiceNumber + ":   total lines is " + totallines.ToString() + ", of which reportable amount " + localtotal.ToString() + " is negaive,  but VAT " + totalVat + " is positive");
                    }
                }
            }
        }

        private static void ValidateInvoiceLinesVAT(APInvoicePM entityPM, ICommonDataContext commonContext, AccountingSetting accountingSetting, string msgRequired)
        {
            List<VatType> allVatTypes = (from f in commonContext.VatTypes where f.Tenant == entityPM.Tenant select f).ToList();

            List<APInvoiceLinePM> lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            foreach (APInvoiceLinePM item in lines)
            {
                if (item.VatTypeId == null)
                {
                    string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatTypeId", entityPM.Tenant);
                    throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                }

                else if (item.VatPercentage == null)
                {
                    VatType itemVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                    if (itemVatType != null)
                    {
                        if (itemVatType.IsMultiPercentage)
                        {
                            if (entityPM.StatusCode == null || entityPM.StatusCode == "WA")
                            {
                                if (!accountingSetting.EnableMultiPercentageVATTypes)
                                {
                                    throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                                }
                            }
                        }

                        else
                        {
                            string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatPercentage", entityPM.Tenant);
                            throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                        }
                    }
                }
            }




            List<APInvoiceTotalVATPM> activeTotalVats = entityPM.TotalVATs.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            List<APInvoiceLinePM> activeLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            if (activeLines != null && activeLines.Count > 0 && activeTotalVats != null && activeTotalVats.Count > 0)
            {
                List<string> expenses = new List<string>();

                List<string> chTypeIds = activeLines.Select(ln => ln.ChargesTypeId).ToList();
                if (chTypeIds != null && chTypeIds.Count > 0)
                {

                    ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(entityPM.Tenant);
                    IQueryable<ChargesTypeList> query = chargesTypeQuery.GetChargesTypeListsByTenant(entityPM.Tenant);
                    if (query != null)
                    {
                        expenses = query.Where(ch => ch.IsExpense == true).Select(ch => ch.Id).ToList();
                    }
                }

                double? localtotal = 0;
                if (expenses != null && expenses.Count > 0)
                {
                    foreach (APInvoiceLinePM line in activeLines)
                    {
                        var chTypeId = line.ChargesTypeId;
                        if (chTypeId == null || (chTypeId != null && !expenses.Contains(chTypeId)))
                        {
                            localtotal += line.LocalCurrencyAmount;
                        }
                    }
                }
                else
                    localtotal = activeLines.Sum(ln => ln.LocalCurrencyAmount);

                double totalVat = activeTotalVats.Sum(tv => tv.LocalVATAmount);
                if (localtotal > 0 && totalVat < 0)
                {
                    throw new ApplicationException("Reference " + entityPM.InvoiceNumber + "   total " + localtotal.ToString() + " is positive,  but VAT " + totalVat + " is negative");
                }
                else if (localtotal < 0 && totalVat > 0)
                {
                    throw new ApplicationException("Reference " + entityPM.InvoiceNumber + "   total " + localtotal.ToString() + " is negaive,  but VAT " + totalVat + " is positive");
                }
            }

        }

        private static void ValidateInvoiceFields(APInvoicePM entityPM)
        {
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
        }

        private static void ValidateUnUpdateFields(APInvoicePM entityPM, APInvoice entityPOCO, bool isNew)
        {
            if (!isNew)
            {
                bool isEditingEnabled = IsEditingEntityEnabled(entityPOCO);

                if (!isEditingEnabled)
                {
                    if (entityPM.InvoiceCurrencyExchangeRate != entityPOCO.InvoiceCurrencyExchangeRate)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("APInvoice.F.InvoiceCurrencyExchangeRate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.AmountInInvoiceCurrency != entityPOCO.AmountInInvoiceCurrency)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("APInvoice.F.AmountInInvoiceCurrency", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }
                }
            }
        }

        public static void CheckInvoiceNumberFormat(string invoiceNumber, int tenant)
        {
            bool showLocal = SetShowLocal(tenant);
            Regex regex = new Regex("^[A-Za-z0-9]*$");
            if (!regex.IsMatch(invoiceNumber))
            {
                string msg = TranslateTextsClass.Translate("APInvoice.O.InvalidNumber", tenant, showLocal);
                throw new ApplicationException(msg);

            }

        }

        private static bool SetShowLocal(int tenant)
        {
            bool showLocal = false;
            var user = GetLoggedContact(tenant);
            if (user != null)
            {
                showLocal = !(GetLoggedContact(tenant).DontShowLocal);
            }
            return showLocal;
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

                #region Sub-Totals
                double? subTotal = 0;
                double? subTotal_Local = 0;
                List<APInvoiceLinePM> lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                if (lines.Count > 0)
                {
                    subTotal = MethodHelper.Round(lines.Sum(s => s.InvoiceCurrencyAmount), 2);
                    subTotal_Local = MethodHelper.Round(lines.Sum(s => s.LocalCurrencyAmount), 2);
                }

                if (entityPM.SubTotalInInvoiceCurrency != subTotal)
                {
                    throw new ApplicationException("Wrong Sub Total Amount");
                }

                if (entityPM.SubTotalInLocalCurrency != subTotal_Local)
                {
                    throw new ApplicationException("Wrong Sub Total Local Amount");
                }
                #endregion

                #region Invoice Amount
                double? sumOfVATsAmounts = 0;
                double? sumOfVATsAmounts_Local = 0;
                double? sumOfVATsAmounts_Profit = 0;
                double? Amount = 0;
                double? Amount_Local = 0;
                double? Amount_Profit = 0;
                List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();
                List<InvoiceTotalsClass> group_TotalVATs = new List<InvoiceTotalsClass>();

                if (entityPM.TotalVATOnly)
                {
                    List<APInvoiceTotalVATPM> items = entityPM.TotalVATs.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();
                    if (items.Count > 0)
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        List<VatType> allVatTypes = (from d in myCommonContext.VatTypes where d.Tenant == tenant select d).ToList();
                        List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
                        VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                        VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                        List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, todayDate);

                        foreach (APInvoiceTotalVATPM item in items)
                        {
                            VatType lineVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                            if (lineVatType != null)
                            {
                                if (!lineVatType.IsMultiPercentage)
                                {
                                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                    {
                                        Id = item.VatTypeId,
                                        VatTypeId = item.VatTypeId,
                                        VatTypePercentage = MethodHelper.GetValue(item.VatPercent),
                                        LocalCurrencyAmount = item.LocalVATAmount,
                                        InvoiceCurrencyAmount = item.InvoiceCurrencyVATAmount,
                                        ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyVATAmount),
                                        ExternalVatCard = item.ExternalVATCard,
                                        ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                    };

                                    group_Source.Add(newItem);
                                }

                                else
                                {
                                    List<VATTypesGroup> vatTypesGroup = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();

                                    foreach (VATTypesGroup itemGroup in vatTypesGroup)
                                    {
                                        InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                        {
                                            Id = itemGroup.SingleVATTypeId,
                                            VatTypeId = itemGroup.SingleVATTypeId,
                                            LocalCurrencyAmount = item.LocalVATAmount,
                                            InvoiceCurrencyAmount = item.InvoiceCurrencyVATAmount,
                                            ProfitCurrencyAmount = MethodHelper.GetValue(item.ProfitCurrencyVATAmount),
                                        };

                                        VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (vatType != null)
                                        {
                                            newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                        }

                                        VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (myPercentagePM != null)
                                        {
                                            newItem.VatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                                        }

                                        group_Source.Add(newItem);
                                    }
                                }
                            }
                        }
                    }
                }

                else if (lines.Count > 0)
                {
                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                    List<VatType> allVatTypes = (from d in myCommonContext.VatTypes where d.Tenant == tenant select d).ToList();
                    List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
                    VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                    VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                    List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, todayDate);

                    foreach (APInvoiceLinePM item in lines)
                    {
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
                    }
                }

                if (group_Source.Count > 0)
                {
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
                        item.VatTypePercentage = MethodHelper.Roundd(item.VatTypePercentage, 3);

                        if (entityPM.TotalVATOnly)
                        {
                            item.LocalCurrencyVATAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                            item.ProfitCurrencyVATAmount = MethodHelper.Roundd(item.ProfitCurrencyAmount, 2);
                            item.InvoiceCurrencyVATAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                            item.LocalCurrencyAmount = 0;
                            item.ProfitCurrencyAmount = 0;
                            item.InvoiceCurrencyAmount = 0;
                        }

                        else
                        {
                            item.LocalCurrencyAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                            item.ProfitCurrencyAmount = MethodHelper.Roundd(item.ProfitCurrencyAmount, 2);
                            item.InvoiceCurrencyAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                            item.LocalCurrencyVATAmount = MethodHelper.Roundd((item.LocalCurrencyAmount * item.VatTypePercentage / 100), 2);
                            item.ProfitCurrencyVATAmount = MethodHelper.Roundd((item.ProfitCurrencyAmount * item.VatTypePercentage / 100), 2);
                            item.InvoiceCurrencyVATAmount = MethodHelper.Roundd((item.InvoiceCurrencyAmount * item.VatTypePercentage / 100), 2);
                        }

                        sumOfVATsAmounts += item.InvoiceCurrencyVATAmount;
                        sumOfVATsAmounts_Local += item.LocalCurrencyVATAmount;
                        sumOfVATsAmounts_Profit += item.ProfitCurrencyVATAmount;
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
                }

                if (entityPM.AmountInInvoiceCurrency != Amount)
                {
                    throw new ApplicationException("Wrong Invoice Total Amount");
                }

                if (entityPM.AmountInLocalCurrency != Amount_Local)
                {
                    throw new ApplicationException("Wrong Invoice Total Local Amount");
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

        public static void ValidateFullAccounting(APInvoicePM invoicePM, bool inNew)//int tenant, string vendorId, string invoiceCurrencyId, DateTime? accountingDate)
        {
            Tenant tenantPOCO = GetTenant(invoicePM.Tenant);

            if (tenantPOCO != null && tenantPOCO.AccountingActivated && !invoicePM.IsUpdateFromPaymentService)
            {
                string errors = "";
                ValidateInvoiceGLaccount(invoicePM, ref errors, invoicePM.Tenant);
                if (inNew)
                    ValidateAccountingPeriod(invoicePM, ref errors, invoicePM.Tenant);
                ThrowErrors(errors);
            }
        }

        private static Tenant GetTenant(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            return tenantPOCO;
        }
        private static void ThrowErrors(string errors)
        {
            if (!string.IsNullOrEmpty(errors))
            {
                errors = errors.TrimEnd(';');
                throw new ApplicationException(errors);
            }

        }

        private static void ValidateAccountingPeriod(APInvoicePM invoicePM, ref string errors, int tenant)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);


            IAccountingContext myContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService accountingPeriodQuery = new AccountingPeriodListQueryService(myContext);
            AccountingPeriodList accountingPeriodList = accountingPeriodQuery.GetByYear(invoicePM.AccountingDate.Value.Year, "1", tenant);
            if (accountingPeriodList != null && invoicePM.AccountingDate != null)
            {
                var month = invoicePM.AccountingDate.Value.Month;

                if (invoicePM.IsExternalEntity)
                {
                    if (month <= accountingPeriodList.ClosedMonth)
                    {

                        string msg = TranslateTextsClass.Translate("Accounting.General.O.ClosedMonth", tenant, showLocal);
                        errors += msg + ";";
                    }
                }
                else
                {
                    if (month > accountingPeriodList.OpenMonth || month <= accountingPeriodList.ClosedMonth)
                    {

                        string msg = TranslateTextsClass.Translate("Accounting.General.O.ClosedMonth", tenant, showLocal);
                        errors += msg + ";";
                    }
                }
            }
            else
            {
                string msg = TranslateTextsClass.Translate("Accounting.General.O.ClosedMonth", tenant, showLocal);
                errors += msg + ";";
            }

        }

        private static void ValidateInvoiceGLaccount(APInvoicePM invoicePM, ref string errors, int tenant)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);

            GLAccountPM glAccount = GetInvoiceGLAccount(invoicePM, tenant);

            if (glAccount == null)
            {
                string msg = TranslateTextsClass.Translate("APInvoice.M.VendorNoGLAccount", tenant, showLocal);
                errors += msg + ";";
            }

            if (glAccount != null && (glAccount.IsMultiCurrency == null || glAccount.IsMultiCurrency == false))
            {
                if (glAccount.CurrencyId != invoicePM.InvoiceCurrencyId)
                {
                    string msg = TranslateTextsClass.Translate("APInvoice.M.InvoiceCurrNotMatch", tenant, showLocal) + " " + glAccount.CurrencyName + " ";
                    errors += msg + ";";
                }
            }
        }

        private static GLAccountPM GetInvoiceGLAccount(APInvoicePM invoicePM, int tenant)
        {
            GLAccountPM glAccount;
            if (invoicePM.VendorGLAccountId != null)
                glAccount = GetGLAccountById(invoicePM.VendorGLAccountId, tenant);
            else
                glAccount = GetGLAccountByCardId(invoicePM.VendorId, tenant);
            return glAccount;
        }

        public static string ValidateFullAccountingInvoiceDate(DateTime? invoiceDate, int tenant, string email)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {

                bool useLocal = !(GetLoggedContact(tenant, email).DontShowLocal);

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
        public static string ValidateConfirmationNumber(DateTime? invoiceDate, decimal localVATAmount, int tenant, string email)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            if (tenantPOCO != null && tenantPOCO.AccountingActivated)
            {

                bool useLocal = !(GetLoggedContact(tenant, email).DontShowLocal);
                IInvoiceContext objectContext = InvoiceContext.GetContext(tenant);
                var confirmationNumberDefault = (from a in objectContext.ConfirmationNumberDefaults
                                                 where a.Tenant == tenant && a.FromDate <= invoiceDate && a.InActive == false
                                                 orderby a.FromDate descending
                                                 select a
                                            ).FirstOrDefault();
                if (localVATAmount >= confirmationNumberDefault?.AmountForConfirmationNumber)
                {
                    return TranslateTextsClass.Translate("Accounting.General.O.ConfirmationNumberValidation", tenant, useLocal);
                }
                else return null;
            }
            else return null;
        }
        private static GLAccountPM GetGLAccountByCardId(string cardId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(cardId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }
        private static GLAccountPM GetGLAccountById(string glaccountId, int tenant)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            GLAccountPM glaAccount = glAccountQuery.GetSingleGLAccountPM(glaccountId, tenant);
            return glaAccount;
        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private static ContactPM GetLoggedContact(int tenant, string email = null)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = null;
            if (email != null)
            {
                loggedContact = GetLoggedContactByAuthTokenEmail(email, tenant);
            }
            else
            {
                loggedContact = new ContactQuery(tenant).GetSingleByEmail(AuthenticationUtil.ResolveUserIdentityName(tenant), tenant);
            }
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetSingleByEmail("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { };
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
        private static bool IsEditingEntityEnabled(APInvoice entityPOCO)
        {
            bool myResult = false;

            if (entityPOCO != null)
            {
                if (string.IsNullOrEmpty(entityPOCO.Id))
                {
                    myResult = true;
                }

                else if (string.IsNullOrEmpty(entityPOCO.StatusCode))
                {
                    myResult = true;
                }

                else if (entityPOCO.StatusCode == "WA")
                {
                    myResult = true;
                }
            }

            return myResult;
        }

        private static void ValidateInvoiceAmountDue(APInvoicePM entityPM, APInvoice entityPOCO, bool isNew, IInvoiceContext context)
        {
            if (!isNew && !entityPM.IsUpdateFromPaymentService)
            {
                IQueryable<APInvoicePayment> allConnectedPaymentsFromDB = (from a in context.APInvoicePayments where a.APInvoiceId == entityPM.Id && a.Tenant == entityPM.Tenant select a);
                List<APInvoicePaymentPM> allConnectedPaymentsFromUI = entityPM.InvoicePayments;

                if (allConnectedPaymentsFromDB.Count() != allConnectedPaymentsFromUI.Count && entityPM.AmountDue != entityPOCO.AmountDue)
                {
                    string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }
        }
        private static void ValidateConcurrencyGUID(APInvoicePM entityPM, APInvoice entityPOCO)
        {
            if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }
        }
    }
}