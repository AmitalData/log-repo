using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.CommonDataModel;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Globalization;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Logitude.BL.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;

namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class ARInvoiceValidator
    {
        public static void Validate(ARInvoicePM entityPM, ARInvoice entityPOCO, IInvoiceContext myContext, ICommonDataContext myCommonContext, bool isNew)
        {
            if (!isNew)
            {
                ValidateConcurrencyGUID(entityPM, entityPOCO);
            }

            ContactPM loggedUser = GetLoggedContactPM(entityPM.Tenant);
            bool useLocal = !(bool)loggedUser?.DontShowLocal;

            string msgRequired = TranslateTextsClass.Translate("General.M.FieldIsRequired", entityPM.Tenant, useLocal);

            ValidateRequiredFields(entityPM, msgRequired);
            ValidateUnUpdateFields(entityPM, entityPOCO, isNew);
            ValidateExternalAPI(entityPM, myCommonContext);

            ARInvoiceRepository entityRepository = new ARInvoiceRepository(myContext);

            AccountingSetting accountingSetting = (from d in myCommonContext.AccountingSettings
                                                   where d.Id == entityPM.Tenant
                                                   select d).FirstOrDefault();

            if (entityPM.InvoiceNumber != null)
            {
                bool isInvoiceNumberExists = entityRepository.IsInvoiceNumberExists(entityPM.Id, entityPM.InvoiceNumber, entityPM.Tenant);
                if (isInvoiceNumberExists)
                {
                    string msg = TranslateTextsClass.Translate("Accounting.General.B.InvoiceNumber", entityPM.Tenant, useLocal) + " " + entityPM.InvoiceNumber + " " + TranslateTextsClass.Translate("Accounting.General.B.AlreadyExist", entityPM.Tenant, useLocal);
                    throw new ApplicationException(msg);
                }
            }

            if (entityPM.IsInvoiceNumberManuallySet && entityPM.InvoiceNumber == null)
            {
                string msg = TranslateTextsClass.Translate("ARInvoice.M.YouShouldSetInvoiceNumber", entityPM.Tenant, useLocal);
                throw new ApplicationException(msg);
            }

            if (entityPM.IsInvoiceNumberFromStock && entityPM.InvoiceNumber == null)
            {
                string msg = TranslateTextsClass.Translate("ARInvoice.M.YouShouldSetInvoiceNumber", entityPM.Tenant, useLocal);
                throw new ApplicationException(msg);
            }

            if (entityPM.InvoiceDate > TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant))
            {
                string msg = TranslateTextsClass.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate", entityPM.Tenant, useLocal);
                throw new ApplicationException(msg);
            }

            if (accountingSetting != null)
            {
                if (accountingSetting.IsVatNumberMandatoryInAR)
                {
                    if (string.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR")
                    {
                        if (string.IsNullOrEmpty(entityPM.VatNumber))
                        {
                            string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.VatNumber", entityPM.Tenant, useLocal);
                            throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                        }
                    }
                }

                if (!accountingSetting.AllowManualInvoiceNumber)
                {
                    if (entityPM.IsInvoiceNumberManuallySet)
                    {
                        if (entityPM.StatusCode == null || entityPM.StatusCode == "DR")
                        {
                            string msg = TranslateTextsClass.Translate("ARInvoice.M.ManualInvoiceNumberNotAllowed", entityPM.Tenant, useLocal);
                            throw new ApplicationException(msg);
                        }
                    }
                }
            }

            List<ARInvoiceLinePM> activeLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            List<string> allVatsIds = (from d in activeLines
                                       where d.VatTypeId != null
                                       group d by d.VatTypeId into g
                                       select g.Key).ToList();

            List<VatType> allVats = (from f in myCommonContext.VatTypes
                                     where allVatsIds.Contains(f.Id)
                                     && f.Tenant == entityPM.Tenant
                                     select f).ToList();

            if (entityPM.IsConsolidationInvoice)
            {
                #region
                if (entityPM.SetVoided || entityPM.StatusCode == "VD" || entityPM.StatusCode == "AC" || entityPM.StatusCode == "AR")
                {

                }

                else
                {
                    if (entityPM.ConstituentInvoices.Count == 0)
                    {
                        string msg = TranslateTextsClass.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast", entityPM.Tenant, useLocal);
                        throw new ApplicationException(msg);
                    }

                    else
                    {
                        if (entityPM.ARInvoiceTypeCode == "CD")
                        {
                            if (!entityPM.IsAutoCredit)
                            {
                                if (entityPM.SubTotalInInvoiceCurrency > 0)
                                {
                                    throw new ApplicationException("Subtotal amount can't be positive");
                                }

                                if (entityPM.AmountInInvoiceCurrency > 0)
                                {
                                    throw new ApplicationException("Invoice amount can't be positive");
                                }
                            }
                        }

                        if (entityPM.ARInvoiceTypeCode != "CD")
                        {
                            if (entityPM.SubTotalInInvoiceCurrency < 0)
                            {
                                throw new ApplicationException("Subtotal amount can't be minus");
                            }

                            if (entityPM.AmountInInvoiceCurrency < 0)
                            {
                                throw new ApplicationException("Invoice amount can't be minus");
                            }
                        }
                    }
                }
                #endregion
            }

            else
            {
                #region

                if (activeLines.Count == 0)
                {
                    string msg = TranslateTextsClass.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast", entityPM.Tenant, useLocal);
                    throw new ApplicationException(msg);
                }

                else
                {
                    foreach (ARInvoiceLinePM item in activeLines)
                    {
                        if (item.VatTypeId == null)
                        {
                            string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatTypeId", entityPM.Tenant, useLocal);
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
                                        string field = TranslateTextsClass.Translate("ARInvoiceLine.F.VatPercentage", entityPM.Tenant, useLocal);
                                        throw new ApplicationException(msgRequired.Replace("%FieldName", field));
                                    }
                                }
                            }
                        }
                    }
                    if (!IsFullAccountingActivated(entityPM.Tenant))
                    {
                        List<string> gr1 = activeLines.GroupBy(g => new { g.ForiegnCurrencyId }).Select(s => s.Key.ForiegnCurrencyId).ToList();
                        foreach (string ob in gr1)
                        {
                            string ob1 = ob;
                            var gr2 = activeLines.Where(w => w.ForiegnCurrencyId == ob1).GroupBy(g => new { g.ForiegnExchangeRate }).Select(s => s.Key.ForiegnExchangeRate);
                            if (gr2.Count() > 1)
                            {
                                Currency curr = CurrencyRepository.GetSingleCurrency(ob, entityPM.Tenant, true);

                                string msg = TranslateTextsClass.Translate("ARInvoice.M.InvoiceLinesHaveDifferentExchangeRates", entityPM.Tenant, useLocal);
                                msg = msg.Replace("%Currency", curr.Code);
                                throw new ApplicationException(msg);
                            }
                        }
                    }
                }
                #endregion
            }

            ValidateAirlineRestriction(entityPM, myCommonContext);
            ValidateBillToCreditLimit(entityPM, entityPOCO, myContext, myCommonContext, isNew);
            ValidateAccountingSetting(entityPM, myContext, myCommonContext, isNew);
            ValidateFullAccounting(entityPM, isNew);
            ValidateMultiVatPercentages(entityPM, accountingSetting, allVats);
            ValidateRegionalTax(entityPM);

            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting.SATInterfaceCode == "PROF33")
            {
                if (entityPM.SATPaymentMethodCode == "99" && entityPM.MetodoPagoCode == "PUE")
                {
                    throw new ApplicationException("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
                }
            }
        }



        private static void ValidateConcurrencyGUID(ARInvoicePM entityPM, ARInvoice entityPOCO)
        {
            if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);

                //if (entityPOCO.UpdatedByPartner != null)
                //{
                //    msg = msg.Replace("another user", entityPOCO.UpdatedByPartner);
                //}

                throw new OptimisticConcurrencyException(msg);
            }
        }

        private static void ValidateRequiredFields(ARInvoicePM entityPM, string msgRequired)
        {
            if (string.IsNullOrEmpty(entityPM.StatusCode))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.StatusCode", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (string.IsNullOrEmpty(entityPM.ARInvoiceTypeCode))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.ARInvoiceTypeCode", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (string.IsNullOrEmpty(entityPM.BranchId))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BranchId", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.CreatedByUserId", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (string.IsNullOrEmpty(entityPM.BillToId))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BillToId", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            //if (string.IsNullOrEmpty(entityPM.BillToAddressId))
            //{
            //    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BillToAddressId", entityPM.Tenant);
            //    throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            //}

            if (string.IsNullOrEmpty(entityPM.LocalCurrencyId))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.LocalCurrencyId", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (string.IsNullOrEmpty(entityPM.InvoiceCurrencyId))
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceCurrencyId", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (entityPM.InvoiceCurrencyExchangeRate == null)
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceCurrencyExchangeRate", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (entityPM.DueDate == null)
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.DueDate", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            if (entityPM.InvoiceDate == null)
            {
                string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceDate", entityPM.Tenant);
                throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
            }

            //if (!entityPM.IsConsolidationInvoice && !entityPM.IsGeneralInvoice)
            //{
            //    if (string.IsNullOrEmpty(entityPM.MainEntityId))
            //    {
            //        throw new ApplicationException(msgRequired.Replace("%FieldName", "Main Entity Id"));
            //    }
            //}

            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(entityPM.Tenant);
            List<ARInvoiceLinePM> lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            foreach (ARInvoiceLinePM item in lines)
            {
                string importStorageChargeId = null;
                if (item.ChargesTypeId == null)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoiceLine.F.ChargesTypeId", entityPM.Tenant);
                    throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                }

                else
                {
                    ChargesType chargesType = chargesTypeRepository.GetSingleChargesTypeByCode("ISTOR", entityPM.Tenant);
                    if(chargesType != null)
                    {
                        importStorageChargeId = chargesType.Id;
                    }
                }

                if (item.VatTypeId == null)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoiceLine.F.VatTypeId", entityPM.Tenant);
                    throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                }

                if (item.ForiegnCurrencyId == null)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoiceLine.F.ForiegnCurrencyId", entityPM.Tenant);
                    throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                }

                if (item.MeasurementCode == "STFE" && item.ChargesTypeId == importStorageChargeId)
                {
                    // import storage charge has no quantity or price
                }

                else
                {
                    if (item.Quantity == null)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoiceLine.F.Quantity", entityPM.Tenant);
                        throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                    }

                    if (item.UnitPrice == null)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoiceLine.F.UnitPrice", entityPM.Tenant);
                        throw new ApplicationException(msgRequired.Replace("%FieldName", fieldLabel));
                    }
                }

                if (item.ForiegnExchangeRate == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", "Foriegn Exchange Rate"));
                }

                if (item.ForiegnCurrencyAmount == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", "Foriegn Currency Amount"));
                }

                if (item.LocalCurrencyAmount == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", "Local Currency Amount"));
                }

                if (item.InvoiceCurrencyAmount == null)
                {
                    throw new ApplicationException(msgRequired.Replace("%FieldName", "Invoice Currency Amount"));
                }
            }
        }
        private static void ValidateUnUpdateFields(ARInvoicePM entityPM, ARInvoice entityPOCO, bool isNew)
        {
            if (!isNew)
            {
                if (entityPM.ARInvoiceTypeCode != entityPOCO.ARInvoiceTypeCode)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.ARInvoiceTypeCode", entityPM.Tenant);
                    throw new ApplicationException("Can't update " + fieldLabel);
                }

                if (entityPM.CreatedByUserId != entityPOCO.CreatedByUserId)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.CreatedByUserId", entityPM.Tenant);
                    throw new ApplicationException("Can't update " + fieldLabel);
                }

                if (entityPM.MainEntityReference != entityPOCO.MainEntityReference)
                {
                    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.MainEntityReference", entityPM.Tenant);
                    throw new ApplicationException("Can't update " + fieldLabel);
                }

                if (entityPM.IsConstituentInvoice != entityPOCO.IsConstituentInvoice)
                {
                    throw new ApplicationException("Can't change Constituent invoice type");
                }

                if (entityPM.IsConsolidationInvoice != entityPOCO.IsConsolidationInvoice)
                {
                    throw new ApplicationException("Can't change Consolidation invoice type");
                }

                bool isEditingEnabled = IsEditingEntityEnabled(entityPOCO);

                if (!isEditingEnabled)
                {
                    if (entityPM.BranchId != entityPOCO.BranchId)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BranchId", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.BillToId != entityPOCO.BillToId)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BillToId", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    //if (entityPM.BillToAddressId != entityPOCO.BillToAddressId)
                    //{
                    //    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.BillToAddressId", entityPM.Tenant);
                    //    throw new ApplicationException("Can't update " + fieldLabel);
                    //}

                    if (entityPM.LocalCurrencyId != entityPOCO.LocalCurrencyId)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.LocalCurrencyId", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.InvoiceCurrencyId != entityPOCO.InvoiceCurrencyId)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceCurrencyId", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.DueDate != entityPOCO.DueDate)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.DueDate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.InvoiceDate != entityPOCO.InvoiceDate)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceCurrencyExchangeRate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if ( Math.Abs((decimal)( entityPM.InvoiceCurrencyExchangeRate- entityPOCO.InvoiceCurrencyExchangeRate)) > (decimal) 0.00001)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.InvoiceCurrencyExchangeRate", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                    if (entityPM.AmountInInvoiceCurrency != entityPOCO.AmountInInvoiceCurrency)
                    {
                        string fieldLabel = TranslateTextsClass.Translate("ARInvoice.F.AmountInInvoiceCurrency", entityPM.Tenant);
                        throw new ApplicationException("Can't update " + fieldLabel);
                    }

                }
            }
        }
        private static void ValidateExternalAPI(ARInvoicePM entityPM, ICommonDataContext myCommonContext)
        {
            double? localAmount_Computed = 0;
            if (entityPM.IsExternalAPI)
            {
                int tenant = entityPM.Tenant;
                Tenant TenantObject = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();

                if (entityPM.LocalCurrencyId != TenantObject.CurrencyId)
                {
                    throw new ApplicationException("The local currency is different from Tenant local currency");
                }

                #region Line Amounts
                //if (!IsFullAccountingActivated(entityPM.Tenant))
                //{
                foreach (ARInvoiceLinePM item in entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
                {
                    double? lineForiegnAmount = MethodHelper.Round(item.ForiegnCurrencyAmount, 2);
                    double? lineForiegnAmount_Computed = MethodHelper.Round(item.Quantity * item.UnitPrice, 2);
                    if (lineForiegnAmount != lineForiegnAmount_Computed)
                    {
                        throw new ApplicationException("Wrong Line Foriegn Amount");
                    }


                    double? lineLocalAmount = MethodHelper.Round(item.LocalCurrencyAmount, 2);
                    double? lineLocalAmount_Computed = MethodHelper.Round(item.ForiegnCurrencyAmount * item.ForiegnExchangeRate, 2);
                    localAmount_Computed = localAmount_Computed + lineLocalAmount_Computed;
                    if (item.VatPercentage != 0)
                    {
                        localAmount_Computed = localAmount_Computed + (lineLocalAmount_Computed * item.VatPercentage / 100);
                    }
                    if (lineLocalAmount != lineLocalAmount_Computed && !IsFullAccountingActivated(entityPM.Tenant))
                    {
                        throw new ApplicationException("Wrong Line Local Amount");
                    }
                    if (IsFullAccountingActivated(entityPM.Tenant) && item.ForiegnExchangeRate != null && !entityPM.IsExternalEntity)
                    {
                        CheckForeignAmountForInvoiceLineFullAccounting(item);
                    }

                    double? lineInvoiceAmount = MethodHelper.Round(item.InvoiceCurrencyAmount, 2);
                    double? exchangeRate = entityPM.InvoiceCurrencyExchangeRate;
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
                        //   if (item.InvoiceCurrencyExchangeRate == null) item.InvoiceCurrencyExchangeRate = entityPM.InvoiceCurrencyExchangeRate;
                        //  lineInvoiceAmount_Computed = IsFullAccountingActivated(entityPM.Tenant) ? MethodHelper.Round(item.ForiegnCurrencyAmount * item.InvoiceCurrencyExchangeRate, 2) : lineInvoiceAmount_Computed;
                        if (IsFullAccountingActivated(entityPM.Tenant) && !entityPM.IsExternalEntity)
                        {
                            CheckFullAccountingLineLocalAmount(item);
                        }
                        else
                        {
                            if (lineInvoiceAmount != lineInvoiceAmount_Computed && !entityPM.IsExternalEntity)
                            {
                                throw new ApplicationException("Wrong Line Invoice Amount");
                            }



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
                    List<ARInvoiceLinePM> lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();

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

                        foreach (ARInvoiceLinePM Item in lines)
                        {
                            #region
                            VatType lineVatType = allVatTypes.Where(d => d.Id == Item.VatTypeId).FirstOrDefault();

                            if (lineVatType != null)
                            {
                                if (!lineVatType.IsMultiPercentage)
                                {
                                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                    {
                                        Id = Item.VatTypeId,
                                        VatTypeId = Item.VatTypeId,
                                        VatTypePercentage = Item.VatPercentage,
                                        LocalCurrencyAmount = Item.LocalCurrencyAmount,
                                        InvoiceCurrencyAmount = Item.InvoiceCurrencyAmount,
                                        ProfitCurrencyAmount = Item.ProfitCurrencyAmount,
                                    };

                                    group_Source.Add(newItem);
                                }

                                else
                                {
                                    List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == Item.VatTypeId).ToList();
                                    foreach (VATTypesGroup itemGroup in myVatGroups)
                                    {
                                        InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                        {
                                            Id = itemGroup.SingleVATTypeId,
                                            VatTypeId = itemGroup.SingleVATTypeId,
                                            LocalCurrencyAmount = Item.LocalCurrencyAmount,
                                            InvoiceCurrencyAmount = Item.InvoiceCurrencyAmount,
                                            ProfitCurrencyAmount = Item.ProfitCurrencyAmount,
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

                        foreach (InvoiceTotalsClass Item in group_data)
                        {
                            ARInvoiceTotalVAT record = new ARInvoiceTotalVAT()
                            {
                                Tenant = entityPM.Tenant,
                                ARInvoiceId = entityPM.Id,
                                VatTypeId = Item.Id,
                                VatPercent = MethodHelper.Roundd(Item.VatTypePercentage, 2),
                                LocalVatableAmount = MethodHelper.Roundd(Item.LocalCurrencyAmount, 2),
                                InvoiceCurrencyVatableAmount = MethodHelper.Roundd(Item.InvoiceCurrencyAmount, 2),
                                ProfitVatableAmount = MethodHelper.Round(Item.ProfitCurrencyAmount, 2),
                            };

                        // Calculate the Local VAT amount
                        record.LocalVATAmount = record.LocalVatableAmount * record.VatPercent / 100;

                        // Calculate the Invoice Currency VAT amount
                        record.InvoiceCurrencyVATAmount = record.InvoiceCurrencyVatableAmount * record.VatPercent / 100;

                        // Calculate the Profit Currency VAT amount
                        record.ProfitCurrencyVATAmount = record.ProfitVatableAmount * record.VatPercent / 100;

                        // Accumulate the VAT amounts
                        sumOfVATsAmounts += record.InvoiceCurrencyVATAmount;
                        sumOfVATsAmounts_Local += record.LocalVATAmount;
                        sumOfVATsAmounts_Profit += record.ProfitCurrencyVATAmount;

                        // Round the VAT amounts to 2 decimal places
                        record.LocalVATAmount = MethodHelper.Roundd(record.LocalVATAmount, 2);
                        record.InvoiceCurrencyVATAmount = MethodHelper.Roundd(record.InvoiceCurrencyVATAmount, 2);
                        record.ProfitCurrencyVATAmount = MethodHelper.Roundd(record.ProfitCurrencyVATAmount, 2);
                    }
                        double? sumOfVATsAmountsRounded = MethodHelper.Round(sumOfVATsAmounts??0, 2); ;
                        Amount = MethodHelper.Round(subTotal + sumOfVATsAmountsRounded, 2);

                        double? sumOfVATsAmounts_LocalRounded = MethodHelper.Round(sumOfVATsAmounts_Local ?? 0, 2);
                        Amount_Local = MethodHelper.Round(subTotal_Local + sumOfVATsAmounts_LocalRounded, 2);

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
                    double computedInvoiceAmount = (double)(entityPM.AmountInInvoiceCurrency * entityPM.InvoiceCurrencyExchangeRate.Value);
                    localAmount_Computed = MethodHelper.Round(localAmount_Computed, 2);
                    var difference = Math.Abs((double)(localAmount - localAmount_Computed));
                    if (localAmount != localAmount_Computed && difference > 0.011 && !entityPM.IsExternalEntity)
                    {
                        throw new ApplicationException("Wrong Invoice Local Amount");
                    }
                    if (IsFullAccountingActivated(entityPM.Tenant) && !entityPM.IsExternalEntity)
                    {
                        computedInvoiceAmount = Math.Round(computedInvoiceAmount, 2);
                        if (Math.Abs((double)(computedInvoiceAmount - localAmount)) >= 0.1)
                        {


                            throw new ApplicationException(" Amount in Invoice Currency (" + entityPM.AmountInInvoiceCurrency + ") * Exchange Rate (" + entityPM.InvoiceCurrencyExchangeRate + ") is not equal to local amount (" + entityPM.AmountInLocalCurrency + ") +- 0.1 ");

                        }


                    }

                    #endregion

                    #region Multi Currency 
                    TenantRepository tenantRepository = new TenantRepository(tenant);
                    Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                    if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                    {
                        if (entityPM.IsMultiCurrency)
                        {
                            if (entityPM.InvoiceCurrencyId != tenantPOCO.CurrencyId)
                            {
                                throw new ApplicationException(TranslateTextsClass.Translate("ARInvoice.M.MultiCurrencyMustInLocalCurrency", tenant));
                            }
                        }
                    }
                    #endregion

            }
        }

        private static void CheckFullAccountingLineLocalAmount(ARInvoiceLinePM line)
        {
            if (line.InvoiceCurrencyExchangeRate != null)
            {
                double? lineLocalAmount = MethodHelper.Round(line.LocalCurrencyAmount, 2);

                double? lineLocalAmount_Computed = MethodHelper.Round((line.InvoiceCurrencyAmount * line.InvoiceCurrencyExchangeRate), 2);
                if (Math.Abs((double)(lineLocalAmount_Computed - lineLocalAmount)) >= 0.1)
                    throw new ApplicationException("Invoice Amount (" + line.InvoiceCurrencyAmount + ") in line (" + line.LineNumber + ") * Exchange Rate (" + line.InvoiceCurrencyExchangeRate + ") is not equal to local amount (" + line.LocalCurrencyAmount + ") + -0.1");
            }

        }
        private static void CheckForeignAmountForInvoiceLineFullAccounting(ARInvoiceLinePM line)
        {
            if(line.ForiegnExchangeRate != null)
            {
                double? lineLocalAmount = MethodHelper.Round(line.LocalCurrencyAmount, 2);
                double? lineLocalAmount_Computed = MethodHelper.Round(line.ForiegnCurrencyAmount * line.ForiegnExchangeRate, 2);
                if(Math.Abs((double)(lineLocalAmount - lineLocalAmount_Computed)) >= 0.1)
                {
                    throw new ApplicationException("Foriegn Amount (" + line.ForiegnCurrencyAmount + ") in line (" + line.LineNumber + ") * Exchange Rate (" + line.ForiegnExchangeRate + ") is not equal to local amount (" + line.LocalCurrencyAmount + ") +-0.1");
                }

            }

        }
        private static void ValidateAirlineRestriction(ARInvoicePM entityPM, ICommonDataContext myCommonContext)
        {
            int tenant = entityPM.Tenant;
            string myCardId = entityPM.BillToId;

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
                    scope.Complete();
                }

                if (isRestrictedByAirline)
                {
                    Card myCard = CardRepository.GetSingleCard(myCardId, tenant, true);
                    if (myCard != null)
                    {
                        if (myCard.PartnerTypeId == "AL")
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);

                            if (MethodHelper.IsAirlineRestricted(myCardId, airlineRepository, tenant))
                            {
                                throw new ApplicationException("Bill to Airline is not allowed");
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateBillToCreditLimit(ARInvoicePM entityPM, ARInvoice entityPOCO, IInvoiceContext myContext, ICommonDataContext myCommonContext, bool isNew)
        {
            int tenant = entityPM.Tenant;
            string id = tenant.ToString();
            CreditLimitSetting mySettings = (from d in myCommonContext.CreditLimitSettings where d.Id == id select d).FirstOrDefault();
            if (mySettings != null)
            {
                if (mySettings.IsCreditLimitEnabled)
                {
                    ValidateCreditLimitPartnersRestrictions(entityPM, entityPOCO, mySettings, isNew);

                    if (isNew)
                    {
                        if (!entityPM.HasCreditLimitOverrideFeature)
                        {
                            if (mySettings.InvoiceCreationBlock)
                            {
                                string errorText_Blocking = "Credit limit setting is blocking invoice for";
                                string myPartnerText = TranslateTextsClass.Translate("ARInvoice.F.BillToId", tenant) + ": " + entityPM.BillToName;

                                // Bill to may be other partners. (not Customr)
                                CustomerRepository myCustomerRepository = new CustomerRepository(myCommonContext);
                                Customer myCustomer = myCustomerRepository.GetSingleCustomer(entityPM.BillToId, tenant, false);
                                if (myCustomer != null)
                                {
                                    if (myCustomer.IsCreditLimitEnabled)
                                    {
                                        if (myCustomer.BlockNewInvoiceCreation)
                                        {
                                            throw new ApplicationException(errorText_Blocking + " " + myPartnerText);
                                        }

                                        if (myCustomer.CreditLimitAmount != null)
                                        {
                                            ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(myContext);
                                            ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                                            double? myResult = invoiceQuery.GetCustomerCreditLimitActualAmount(entityPM.BillToId, tenant);

                                            double LimitAmount = myCustomer.CreditLimitAmount == null ? 0 : myCustomer.CreditLimitAmount.Value;
                                            double ActualBalance = myResult == null ? 0 : myResult.Value;
                                            if (myCustomer.CreditLimitOpenBalance != null)
                                            {
                                                ActualBalance += myCustomer.CreditLimitOpenBalance.Value;
                                            }

                                            if (ActualBalance > LimitAmount)
                                            {
                                                //var msg = "Can't create a new invoice. The customer exceeded the credit limit available";

                                                var myLocalCurrencyCode = "";
                                                if (!string.IsNullOrEmpty(entityPM.LocalCurrencyCode))
                                                {
                                                    myLocalCurrencyCode = entityPM.LocalCurrencyCode;
                                                }

                                                var LimitError = "";
                                                LimitError += "Bill To exceeded its credit limit of " + String.Format("{0:N2}", LimitAmount) + " (" + myLocalCurrencyCode + ").";
                                                LimitError += " ";
                                                LimitError += "The current balance stands on " + String.Format("{0:N2}", ActualBalance) + " (" + myLocalCurrencyCode + ").";

                                                throw new ApplicationException(LimitError);
                                            }

                                        }
                                    }
                                }

                                else
                                {
                                    AgentRepository myAgentRepository = new AgentRepository(myCommonContext);
                                    Agent myAgent = myAgentRepository.GetSingleAgent(tenant, entityPM.BillToId);
                                    if (myAgent != null)
                                    {
                                        if (myAgent.IsCreditLimitEnabled)
                                        {
                                            if (myAgent.BlockNewInvoiceCreation)
                                            {
                                                throw new ApplicationException(errorText_Blocking + " " + myPartnerText);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ValidateCreditLimitPartnersRestrictions(ARInvoicePM entityPM, ARInvoice entityPOCO, CreditLimitSetting mySettings, bool isNewEntity)
        {
            if (entityPM.BillToId != null)
            {
                bool isValidating = false;

                if (isNewEntity)
                {
                    isValidating = true;
                }

                else if (entityPM.BillToId != entityPOCO.BillToId)
                {
                    isValidating = true;
                }

                if (isValidating)
                {
                    Card iCard = CardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant, true);

                    if (iCard != null)
                    {
                        string errorText_Blocking = "Credit limit setting is blocking invoice for ";

                        switch (iCard.PartnerTypeId)
                        {
                            case "CS":
                                {
                                    if (iCard.IsCustomer)
                                    {
                                        if (mySettings.CustomersInvoicesBlock)
                                        {
                                            throw new ApplicationException(errorText_Blocking + "Customers");
                                        }
                                    }

                                    else
                                    {
                                        if (mySettings.ShipperConsigneeInvoiceBlock)
                                        {
                                            throw new ApplicationException(errorText_Blocking + "Shippers and Consignees");
                                        }
                                    }

                                    break;
                                }

                            case "AG":
                                {
                                    if (mySettings.AgentsInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Agents");
                                    }

                                    break;
                                }

                            case "CG":
                                {
                                    if (mySettings.CustomsAgentsInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Customs Agents");
                                    }

                                    break;
                                }

                            case "SG":
                                {
                                    if (mySettings.ShippingAgentsInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Shipping Agents");
                                    }

                                    break;
                                }

                            case "AL":
                                {
                                    if (mySettings.AirlinesInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Airlines");
                                    }

                                    break;
                                }

                            case "SL":
                                {
                                    if (mySettings.ShippingLinesInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Shipping Lines");
                                    }

                                    break;
                                }

                            case "TR":
                                {
                                    if (mySettings.TruckersInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Truckers");
                                    }

                                    break;
                                }

                            case "VD":
                                {
                                    if (mySettings.VendorsInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Vendors");
                                    }

                                    break;
                                }

                            case "WH":
                                {
                                    if (mySettings.WarehousesInvoicesBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Warehouses");
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }

        private static void ValidateAccountingSetting(ARInvoicePM entityPM, IInvoiceContext myContext, ICommonDataContext myCommonContext, bool isNew)
        {
            Tenant loggedTenant = (from a in myCommonContext.Tenants.Include("AccountingSetting")
                                   where a.Id == entityPM.Tenant
                                   select a).FirstOrDefault();

            if (loggedTenant != null)
            {
                if (loggedTenant.AccountingSetting != null)
                {
                    if (!loggedTenant.AccountingSetting.AllowManualInvoiceNumber)
                    {
                        if (entityPM.IsInvoiceNumberManuallySet)
                        {
                            if (entityPM.StatusCode == null || entityPM.StatusCode == "DR")
                            {
                                string msg = TranslateTextsClass.Translate("ARInvoice.M.ManualInvoiceNumberNotAllowed", entityPM.Tenant);
                                throw new ApplicationException(msg);
                            }
                        }
                    }

                    var isValidatingChronological = false;
                    if (entityPM.SetApproved || entityPM.StatusCode == "PR")
                    {
                        isValidatingChronological = true;
                    }

                    else if (entityPM.IsAutoCredit && isNew)
                    {
                        isValidatingChronological = true;
                    }

                    if (isValidatingChronological)
                    {
                        if (!entityPM.IsExternalEntity)
                        {
                            bool HasInterestFeature = entityPM.HasInterestFeature;

                            ARInvoice lastApprovedInvoice = (from a in myContext.ARInvoices
                                                             where a.Tenant == entityPM.Tenant
                                                             && a.IsInvoiceNumberManuallySet == false
                                                             && a.IsExternalEntity == false
                                                             && a.StatusCode != "DR"
                                                             && a.StatusCode != "PR"
                                                             && a.StatusCode != "VD"
                                                             && a.InvoiceNumber != a.Id
                                                             && ((HasInterestFeature || entityPM.ARInvoiceTypeCode  == "IT") ? a.ARInvoiceTypeCode == "IT": a.ARInvoiceTypeCode != "IT")
                                                             select a).OrderByDescending(d => d.ApprovedDate).FirstOrDefault();

                            if (lastApprovedInvoice != null)
                            {
                                if (entityPM.InvoiceDate < lastApprovedInvoice.InvoiceDate)
                                {
                                    string datetimeformat = @"dd\/MM\/yyyy";

                                    if (!string.IsNullOrEmpty(loggedTenant.DateTimeFormat))
                                    {
                                        datetimeformat = loggedTenant.DateTimeFormat;
                                    }

                                    string dateString = lastApprovedInvoice.InvoiceDate.Value.ToString(datetimeformat, CultureInfo.CurrentCulture);

                                    bool useLocal = true;
                                    var user = GetLoggedContact(entityPM.Tenant);
                                    if (user != null) useLocal = !(GetLoggedContact(entityPM.Tenant).DontShowLocal);

                                    string fieldLabel = TranslateTextsClass.Translate("ARInvoice.M.ChronologicalDate", entityPM.Tenant, useLocal);
                                    throw new ApplicationException(fieldLabel.Replace("%Date", dateString));
                                }
                            }
                        }
                    }
                }
            }
        }
        private static List<string> errorsList = new List<string>();
        public static void ValidateFullAccounting(ARInvoicePM invoice, bool isNew)
        {
            errorsList = new List<string>();
            if (IsFullAccountingActivated(invoice.Tenant) && isNew)
            {
                if (invoice.BillToGLAccountId == null)
                {
                    CheckCardConnectedGLAccount(invoice.Tenant, invoice.BillToId);
                }
                CheckInvoiceCurrency(invoice);
                CheckClosedMonth(invoice.InvoiceDate, invoice.Tenant, invoice.ARInvoiceTypeCode, invoice.IsExternalAPI);

                if (errorsList.Count > 0)
                    throw new ApplicationException(string.Join(";", errorsList));
            }
        }

        private static bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRepository.GetSingleTenant(tenant);
            return (tenantPoco != null && tenantPoco.AccountingActivated);
        }

        private static void CheckInvoiceCurrency(ARInvoicePM invoice)
        {
            GLAccountPM account = null;
            if (invoice.BillToGLAccountId != null)
            {
                account = GetGLaccountById(invoice.BillToGLAccountId, invoice.Tenant);
            }
            else
            {
                account = getGLAccount(invoice.BillToId, invoice.Tenant);
            }
            if (account != null && (account.IsMultiCurrency == null || account.IsMultiCurrency == false))
            {
                if (account.CurrencyId != invoice.InvoiceCurrencyId)
                {
                    bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(invoice.Tenant);
                    string msg = TranslateTextsClass.Translate("ARInvoice.M.InvoiceCurrencyGLAccount", invoice.Tenant, useLocal) + " " + account.CurrencyCode;
                    errorsList.Add(msg);
                }
            }

        }
        private static GLAccountPM GetGLaccountById(string accountId, int tenant)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            return glAccountQuery.GetSingleGLAccountPM(accountId, tenant);
        }
        private static void CheckCardConnectedGLAccount(int tenant, string billToId)
        {
            GLAccountPM glAccount = getGLAccount(billToId, tenant);

            if (glAccount == null)
            {
                bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
                string msg = TranslateTextsClass.Translate("ARInvoice.M.BillToGLAccount", tenant, useLocal);
                errorsList.Add(msg);
            }

        }

        private static void CheckClosedMonth(DateTime? accountingDate, int tenant ,string Type=null, bool isExternalAPI = false)
        {
            AccountingPeriodList period = GetInvoiceAccountPeriodByYear(tenant, accountingDate.Value.Year, Type);

            if (period != null && accountingDate != null)
            {
                var month = accountingDate.Value.Month;
                if (isExternalAPI)
                {
                    if (month < period.ClosedMonth)

                    errorsList.Add(GetClosedMonthErrorMessage(tenant));
                }
                else {
                    if (month > period.OpenMonth || month < period.ClosedMonth)
                        errorsList.Add(GetClosedMonthErrorMessage(tenant));
                }
                
            }
            else
            {
                errorsList.Add(GetClosedMonthErrorMessage(tenant));
            }
        }

        private static string GetClosedMonthErrorMessage(int tenant)
        {
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            string msg = TranslateTextsClass.Translate("ARInvoice.M.ClosedMonth", tenant, useLocal) + ";";
            return msg;
        }

        private static AccountingPeriodList GetInvoiceAccountPeriodByYear(int tenant, int year,string Type=null)
        {
            string AccountPeriodCode = "2";// 2- Invoice
            if (Type == "IT")
            {
                AccountPeriodCode = "3";// 2- Interest Invoice
            }
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService accountingPeriodQuery = new AccountingPeriodListQueryService(accountingContext);
            AccountingPeriodList accountingPeriod = accountingPeriodQuery.GetByYear(year, AccountPeriodCode, tenant); 
            return accountingPeriod;
        }
 
        private static GLAccountPM getGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                glaAccount = GetGLaccountById(card.GLAccountId, card.Tenant);
            }

            return glaAccount;
        }
        private static void ValidateMultiVatPercentages(ARInvoicePM entityPM, AccountingSetting accountingSetting, List<VatType> allVats)
        {
            if (entityPM.StatusCode == null || entityPM.StatusCode == "DR")
            {
                if (allVats.Count > 0)
                {
                    if (accountingSetting != null)
                    {
                        if (!accountingSetting.EnableMultiPercentageVATTypes)
                        {
                            if (allVats.Where(d => d.IsMultiPercentage).Any())
                            {
                                string msg = TranslateTextsClass.Translate("ARInvoice.M.MultiPercentageVATs", entityPM.Tenant);
                                throw new ApplicationException(msg);
                            }
                        }
                    }
                }
            }
        }
        private static bool IsEditingEntityEnabled(ARInvoice entityPOCO)
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

                else if (entityPOCO.StatusCode == "DR")
                {
                    myResult = true;
                }

                else if (entityPOCO.IsConstituentInvoice)
                {
                    if (string.IsNullOrEmpty(entityPOCO.ConsolidationInvoiceId) && entityPOCO.StatusCode == "NT")
                    {
                        myResult = true;
                    }
                }
            }

            return myResult;
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
        public static ContactPM GetLoggedContactPM(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
        private static void ValidateOnVoid(ARInvoicePM entityPM)
        {
            if (entityPM.SetVoided)
            {
                if (entityPM.InvoicePayments.Count > 0)
                {
                    string msg = TranslateTextsClass.Translate("ARInvoice.M.DisconnectPayments", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }
        }

        private static void ValidateRegionalTax(ARInvoicePM entityPM)
        {
            List<ARInvoiceLinePM> allRegionalTaxLines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.IsRegionalTax).ToList();

            if (allRegionalTaxLines.Count > 0)
            {
                if (allRegionalTaxLines.Where(d => d.VatIsMultiPercentage).Any())
                {
                    throw new ApplicationException("Can't set regional Tax for multi VAT");
                }

                else if (entityPM.RegionalTaxId == null)
                {
                    throw new ApplicationException("Invoice regional tax field is required");
                }

                else
                {
                    var groupedIds = (from d in allRegionalTaxLines
                                      where d.VatTypeId != null
                                      group d by d.VatTypeId into g
                                      select g.Key).ToList();

                    if (groupedIds.Count > 1)
                    {
                        throw new ApplicationException("Can't set regional tax for different VATs");
                    }

                    else
                    {
                        var groupedPercentages = (from d in allRegionalTaxLines
                                                  where d.VatTypeId != null
                                                  group d by new { d.VatTypeId, d.VatPercentage } into g
                                                  select g.Key).ToList();

                        if (groupedPercentages.Count > 1)
                        {
                            throw new ApplicationException("Can't set different regional Tax percentages");
                        }
                    }
                }
            }
        }
    }
}