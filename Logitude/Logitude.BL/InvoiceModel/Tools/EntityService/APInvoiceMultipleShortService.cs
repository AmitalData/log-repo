using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.Helpers;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APInvoiceMultipleShortService
    {
        private int tenant;
        private bool isUpdatingTotalVats;
        public APInvoice invoice { get; set; }
        private APInvoiceMultipleShortPM entityPM;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext shipmentsContext;
        private string loggedContactId;
        private AccountingSetting accountingSetting;
        private IInvoiceContext objectContext;
        private APInvoiceRepository invoiceRepository;
        private APInvoiceLineRepository invoiceLineRepository;
        private APInvoiceTotalVATRepository invoiceTotalVatRepository;
        private List<ShipmentPayable> allPayables;
        private ShipmentPayableRepository shipmentPayableRepository;
        private PayableProratedAmountRepository payableProratedAmountRepository;
        private DateTime todayDateTime;
        public APInvoiceMultipleShortService(IInvoiceContext objectContext, APInvoiceMultipleShortPM entityPM)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.shipmentsContext = ShipmentsContext.GetContext(tenant);
            this.invoiceRepository = new APInvoiceRepository(objectContext);
            this.invoiceLineRepository = new APInvoiceLineRepository(objectContext);
            this.invoiceTotalVatRepository = new APInvoiceTotalVATRepository(objectContext);
            this.shipmentPayableRepository = new ShipmentPayableRepository(shipmentsContext);
            this.payableProratedAmountRepository = new PayableProratedAmountRepository(shipmentsContext);
            this.todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.allPayables = new List<ShipmentPayable>();
            this.GetLoggedContact();

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(myCommonContext);
            this.accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;

            if (email != null)
            {
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                this.loggedContactId = loggedContact.Id;
            }
        }

        private List<APInvoiceLinePM> invoiceLinesChangeSet;
        public void Update(List<APInvoiceLinePM> invoiceLinesChangeSet)
        {
            this.invoiceLinesChangeSet = invoiceLinesChangeSet;

            this.invoice = invoiceRepository.GetSingleAPInvoice(entityPM.Id, tenant);
            this.invoice.UpdateDate = todayDateTime;
            this.invoice.UpdatedByUserId = loggedContactId;

            this.BuildUnexpectedPayables();
            this.GetShipmentsData();
            this.UpdateInvoiceLines();
            this.UpdateTotalVats();

            invoiceRepository.Update(invoice);
            invoiceRepository.SubmitChanges();

            this.UpdateAllPayablesAccountedAmountAndStatus();
            this.shipmentPayableRepository.SubmitChanges();

            PayablesLinesVatAmounts payablesLinesVatAmounts = new PayablesLinesVatAmounts(allPayables, tenant, shipmentPayableRepository);
            payablesLinesVatAmounts.UpdateAllPayablesVatAmount();

            this.RunStoredProcedures();
        }

        private void BuildUnexpectedPayables()
        {
            if (entityPM.InvoiceLines.Where(d => d.EntityPayableId == null).Any())
            {
                foreach (APInvoiceLinePM invoicelinePM in entityPM.InvoiceLines.Where(d => d.EntityPayableId == null))
                {
                    ShipmentPayable payable = new ShipmentPayable()
                    {
                        Id = IdCounter.GetNumber("ShipmentPayable", entityPM.Tenant),
                        VendorId = entityPM.VendorId,
                        ChargesTypeId = invoicelinePM.ChargesTypeId,
                        CurrencyId = entityPM.InvoiceCurrencyId,
                        ShipmentPayableLineStatusCode = "ACCT",
                        ShipmentPayableAmountTypeCode = "NEXP",
                        Rate = entityPM.InvoiceCurrencyExchangeRate,
                        ProfitCurrencyExchangeRate = entityPM.ProfitCurrencyExchangeRate,
                        ShipmentId = invoicelinePM.EntityId,
                        Tenant = entityPM.Tenant,
                        UpdateDate = todayDateTime,
                        ValueDate = todayDateTime,
                        CreateDate = todayDateTime,
                        CreatedByUserId = loggedContactId,
                        UpdateByUserId = loggedContactId,
                        AWBPrint = false,
                        IsEditedByUser = false,
                        IsFromQuote = false,
                    };

                    invoicelinePM.EntityPayableId = payable.Id;
                    shipmentPayableRepository.Add(payable);
                }

                shipmentPayableRepository.SubmitChanges();
            }
        }

        private void GetShipmentsData()
        {
            if (invoiceLinesChangeSet.Count > 0)
            {
                List<APInvoiceLinePM> activeLines = invoiceLinesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();

                if (activeLines.Count > 0)
                {
                    List<string> allPayablesIds = activeLines.Select(s => s.EntityPayableId).ToList();

                    allPayables = (from d in shipmentsContext.ShipmentPayables
                                   where allPayablesIds.Contains(d.Id)
                                   select d).ToList();

                }
            }
        }

        #region InvoiceLines
        private void UpdateInvoiceLines()
        {
            if (invoiceLinesChangeSet.Count > 0)
            {
                IQueryable<APInvoiceLine> iQueryable
                 = (from a in invoiceLineRepository.context.APInvoiceLines
                    where a.APInvoiceId == entityPM.Id && a.Tenant == tenant
                    select a);

                int lastLineNumber = 0;

                if (iQueryable.Count() > 0)
                {
                    lastLineNumber = iQueryable.Max(m => m.LineNumber);
                }

                foreach (APInvoiceLinePM item in invoiceLinesChangeSet)
                {
                    switch (item.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                lastLineNumber += 1;
                                item.LineNumber = lastLineNumber;
                                this.CreateInvoiceLine(item);
                                this.UpdatePayable(item);
                                this.isUpdatingTotalVats = true;
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateInvoiceLines(item);
                                this.UpdatePayable(item);
                                this.isUpdatingTotalVats = true;
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteInvoiceLine(item);
                                this.isUpdatingTotalVats = true;
                                break;
                            }

                        default: { break; }
                    }
                }

                invoiceLineRepository.SubmitChanges();
            }
        }
        private void CreateInvoiceLine(APInvoiceLinePM item)
        {
            item.APInvoiceId = entityPM.Id;
            APInvoiceLine invoiceLine = new APInvoiceLine();
            APInvoiceMapping.MapInvoiceLine(item, invoiceLine, true);
            invoiceLineRepository.Add(invoiceLine);
        }
        private void UpdateInvoiceLines(APInvoiceLinePM item)
        {
            APInvoiceLine invoiceLine = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);
            APInvoiceMapping.MapInvoiceLine(item, invoiceLine, false);
            invoiceLineRepository.Update(invoiceLine);
        }
        private void DeleteInvoiceLine(APInvoiceLinePM item)
        {
            APInvoiceLine line = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);

            this.DisconnectPayable(line.EntityPayableId, line.ForiegnCurrencyAmount);

            invoiceLineRepository.Remove(line);
        }
        private void DisconnectInvoiceLine(APInvoiceLinePM item)
        {
            APInvoiceLine line = invoiceLineRepository.GetSingleAPInvoiceLine(item.APInvoiceId, item.LineNumber, entityPM.Tenant);
            line.EntityPayableId = null;
            item.EntityPayableId = null;
            invoiceLineRepository.Update(line);
        }
        #endregion

        #region TotalVats
        private List<VatType> allVatTypes = new List<VatType>();
        private List<VatTypePercentagePM> allVatPercentages = new List<VatTypePercentagePM>();
        private void UpdateTotalVats()
        {
            if (isUpdatingTotalVats)
            {
                VatTypeRepository vatTypeRepository = new VatTypeRepository(myCommonContext);
                this.allVatTypes = vatTypeRepository.GetVatTypes(this.tenant).ToList();

                VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                this.allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                double? subTotal = 0;
                double? subTotal_Local = 0;
                double? sumOfVATsAmounts = 0;
                double? sumOfVATsAmounts_Local = 0;
                double? sumOfVATsAmounts_Profit = 0;
                double? Amount = 0;
                double? Amount_Local = 0;
                double? Amount_Profit = 0;

                List<APInvoiceTotalVAT> invoiceTotalVATs = invoiceTotalVatRepository.GetInvoiceTotalVatsByInvoiceId(entityPM.Id, entityPM.Tenant).ToList();
                foreach (APInvoiceTotalVAT item in invoiceTotalVATs)
                {
                    invoiceTotalVatRepository.Remove(item);
                }

                List<APInvoiceLine> myDataLines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant).ToList();
                if (myDataLines.Count > 0)
                {
                    subTotal = MethodHelper.Round(myDataLines.Sum(s => s.InvoiceCurrencyAmount), 2);
                    subTotal_Local = MethodHelper.Round(myDataLines.Sum(s => s.LocalCurrencyAmount), 2);

                    #region
                    List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                        where d.Tenant == this.tenant
                                                        select d).ToList();

                    List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

                    foreach (APInvoiceLine item in myDataLines)
                    {
                        #region
                        VatType lineVatType = this.allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

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
                                    ExternalVatCard = this.accountingSetting == null ? null : this.accountingSetting.PayableVATCard,
                                    ExternalTAXItemId = lineVatType.ExternalTAXItemId,
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

                                    VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                    if (vatType != null)
                                    {
                                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                    }

                                    if (this.accountingSetting != null)
                                    {
                                        newItem.ExternalVatCard = this.accountingSetting.PayableVATCard;
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
                               ExternalVatCard = g.Key.ExternalVatCard,
                               ExternalTAXItemId = g.Key.ExternalTAXItemId
                           }).ToList();

                    foreach (InvoiceTotalsClass item in group_data)
                    {
                        APInvoiceTotalVAT record = new APInvoiceTotalVAT()
                        {
                            Id = IdCounter.GetNumber("APInvoiceTotalVAT", entityPM.Tenant).ToString(),
                            Tenant = entityPM.Tenant,
                            APInvoiceId = entityPM.Id,
                            VatTypeId = item.Id,
                            VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 3),
                            LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                            InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                            ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                            ExternalVATCard = item.ExternalVatCard,
                            ExternalTAXItemId = item.ExternalTAXItemId
                        };

                        record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VatPercent / 100), 2);
                        record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VatPercent / 100), 2);
                        record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VatPercent / 100), 2);
                        invoiceTotalVatRepository.Add(record);

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

                entityPM.SubTotalInInvoiceCurrency = subTotal;
                entityPM.SubTotalInLocalCurrency = subTotal_Local;
                invoice.SubTotalInLocalCurrency = entityPM.SubTotalInLocalCurrency;
                invoice.SubTotalInInvoiceCurrency = entityPM.SubTotalInInvoiceCurrency;
            }
        }
        #endregion

        #region Payables
        private void UpdatePayable(APInvoiceLinePM item)
        {
            ShipmentPayable payable = (from a in allPayables where a.Id == item.EntityPayableId select a).FirstOrDefault();

            if (payable != null)
            {
                payable.VendorId = invoice.VendorId;
                payable.CorrectionAmount = item.CorrectionAmount;                
                payable.CorrectionDate = todayDateTime;
                payable.CorrectionByUserId = loggedContactId;
                payable.CorrectionNote = item.CorrectionNote;
                payable.OpenAmount = item.OpenAmount;
                payable.OpenAmountInLocalCurrency = (double?)MethodHelper.Round(payable.OpenAmount * payable.Rate, 2);

                if (payable.CurrencyId == invoice.ProfitCurrencyId)
                {
                    payable.OpenAmountInProfitCurrency = payable.OpenAmount;
                }

                else
                {
                    payable.OpenAmountInProfitCurrency = (double?)MethodHelper.Round(payable.OpenAmountInLocalCurrency / payable.ProfitCurrencyExchangeRate, 2);
                }

                shipmentPayableRepository.Update(payable);
            }
        }
        private void DisconnectPayable(string payableId, double? myForiegnCurrencyAmount)
        {
            ShipmentPayable myPayable = (from a in allPayables where a.Id == payableId select a).FirstOrDefault();
            if (myPayable != null)
            {
                if (myPayable.ShipmentPayableAmountTypeCode == "NEXP")
                {
                    List<ShipmentPayable> ChildPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(myPayable.Id, tenant);
                    List<string> payablesId = ChildPayables.Select(s => s.Id).ToList();

                    List<PayableProratedAmount> payableProratedAmounts = payableProratedAmountRepository.GetPayableProratedAmountsByPayablesIds(payablesId, tenant);
                    foreach (PayableProratedAmount item in payableProratedAmounts)
                    {
                        payableProratedAmountRepository.Remove(item);
                    }

                    foreach (ShipmentPayable myChild in ChildPayables)
                    {
                        shipmentPayableRepository.Remove(myChild);
                    }

                    shipmentPayableRepository.Remove(myPayable);
                    allPayables.Remove(myPayable);
                }

                else
                {
                    double? myOtherInvoicesAmounts = myPayable.AccountedAmount - myForiegnCurrencyAmount;
                    double? myOpenAmount = myPayable.ExpectedAmount - myOtherInvoicesAmounts - myPayable.CorrectionAmount;

                    myPayable.OpenAmount = (double?)MethodHelper.Round(myOpenAmount, 2);
                    myPayable.OpenAmountInLocalCurrency = (double?)MethodHelper.Round(myPayable.OpenAmount * myPayable.Rate, 2);
                    myPayable.OpenAmountInProfitCurrency = (double?)MethodHelper.Round(myPayable.OpenAmountInLocalCurrency / myPayable.ProfitCurrencyExchangeRate, 2);
                    shipmentPayableRepository.Update(myPayable);
                }

                shipmentPayableRepository.SubmitChanges();
            }
        }
        private void UpdateAllPayablesAccountedAmountAndStatus()
        {
            if (allPayables.Count > 0)
            {
                List<string> allPayablesIds = allPayables.Select(s => s.Id).ToList();
                List<APInvoiceLine> allPayablesInvoicesLines = invoiceLineRepository.GetPayablesInvoicesLines(allPayablesIds, tenant);

                foreach (ShipmentPayable payable in allPayables)
                {
                    List<APInvoiceLine> myInvoiceslines = allPayablesInvoicesLines.Where(d => d.EntityPayableId == payable.Id).ToList();

                    double? myAccountedAmount = myInvoiceslines.Sum(s => s.ForiegnCurrencyAmount);
                    double? myAccountedAmount_Local = myInvoiceslines.Sum(s => s.LocalCurrencyAmount);
                    double? myAccountedAmount_Profit = myInvoiceslines.Sum(s => s.ProfitCurrencyAmount);

                    payable.AccountedAmount = (double?)MethodHelper.Round(myAccountedAmount, 2);
                    payable.AccountedAmountInLocalCurrency = (double?)MethodHelper.Round(myAccountedAmount_Local, 2);
                    payable.AccountedAmountInProfitCurrency = (double?)MethodHelper.Round(myAccountedAmount_Profit, 2);

                    if (myAccountedAmount != 0)
                    {
                        SetPayableStatus(payable);
                    }

                    else
                    {
                        if (payable.Quantity == null || payable.UnitPrice == null)
                        {
                            payable.ShipmentPayableLineStatusCode = "EMPT";
                        }

                        else
                        {
                            payable.ShipmentPayableLineStatusCode = "OAMT";
                        }
                    }

                    List<ShipmentPayable> ChildPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(payable.Id, tenant);
                    List<string> payablesId = ChildPayables.Select(s => s.Id).ToList();

                    List<PayableProratedAmount> payableProratedAmounts = payableProratedAmountRepository.GetPayableProratedAmountsByPayablesIds(payablesId, tenant);
                    foreach (PayableProratedAmount item in payableProratedAmounts)
                    {
                        payableProratedAmountRepository.Remove(item);
                    }

                    foreach (ShipmentPayable myChild in ChildPayables)
                    {
                        myChild.ShipmentPayableLineStatusCode = payable.ShipmentPayableLineStatusCode;
                        shipmentPayableRepository.Remove(myChild);
                    }

                    shipmentPayableRepository.Update(payable);
                }
            }
        }
        private void SetPayableStatus(ShipmentPayable entity)
        {
            if (entity.ShipmentPayableAmountTypeCode == "NEXP")
            {
                entity.ShipmentPayableLineStatusCode = "ACCT";
            }

            else
            {
                if (entity.Quantity == null || entity.UnitPrice == null)
                {
                    entity.ShipmentPayableLineStatusCode = "EMPT";
                }

                else
                {
                    if (string.IsNullOrEmpty(entity.CorrectionByUserId))
                    {
                        entity.ShipmentPayableLineStatusCode = "OAMT";
                    }

                    if (entity.OpenAmount != 0 && entity.OpenAmount != null)
                    {
                        if (entity.OpenAmount == entity.ExpectedAmount)
                        {
                            entity.ShipmentPayableLineStatusCode = "OAMT";
                        }

                        else
                        {
                            entity.ShipmentPayableLineStatusCode = "PACC";
                        }
                    }

                    else
                    {
                        entity.ShipmentPayableLineStatusCode = "ACCT";
                    }
                }
            }
        }
        #endregion

        private void RunStoredProcedures()
        {
            if (entityPM.IsMultipleEntities)
            {
                if (entityPM.ShipmentId != null)
                {
                    UpdateShipmentProfitClass.UpdatePayables(entityPM.ShipmentId, entityPM.Tenant,true, entityPM.Id);
                    UpdateShipmentProfitClass.UpdateProfit(entityPM.ShipmentId, entityPM.Tenant);
                }
            }
        }
    }
}
