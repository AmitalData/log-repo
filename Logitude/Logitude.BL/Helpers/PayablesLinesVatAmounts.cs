using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.BL.Helpers
{
    public class PayablesLinesVatAmounts
    {
        private List<ShipmentPayable> allPayables;
        private APInvoiceLineRepository invoiceLineRepository;
        private int tenant;
        private ShipmentPayableRepository shipmentPayableRepository;
        private ICommonDataContext CommonContext;

        public PayablesLinesVatAmounts(List<ShipmentPayable>  allPayables, int tenant, ShipmentPayableRepository shipmentPayableRepository) {
            this.allPayables = allPayables;
            this.tenant = tenant;
            this.invoiceLineRepository = new APInvoiceLineRepository(tenant);
            this.shipmentPayableRepository = shipmentPayableRepository;
            CommonContext = CommonDataContext.GetContext(tenant);
            InitializeVATs();
        }

        double? invoiceLinesVatAmountLocal = 0;
        double? invoiceLinesVatAmountProfit = 0;
        public void UpdateAllPayablesVatAmount()
        {
            if (allPayables.Count > 0)
            {
                List<string> allPayablesIds = allPayables.Select(s => s.Id).ToList();
                List<APInvoiceLine> allPayablesInvoicesLines = invoiceLineRepository.GetPayablesInvoicesLines(allPayablesIds, tenant);
                foreach (ShipmentPayable myPayable in allPayables)
                {
                    List<APInvoiceLine> myInvoiceslines = allPayablesInvoicesLines.Where(d => d.EntityPayableId == myPayable.Id).ToList();
                    invoiceLinesVatAmountLocal = 0;
                    invoiceLinesVatAmountProfit = 0;
                    foreach (APInvoiceLine invoiceline in myInvoiceslines)
                    {
                        this.ManagePayableLinesVatAmount(invoiceline);
                    }
                    this.UpdateOpenPayablesVatAmount(myPayable);
                    shipmentPayableRepository.Update(myPayable);
                }
                shipmentPayableRepository.SubmitChanges();
            }
        }
        private void ManagePayableLinesVatAmount(APInvoiceLine invoiceline)
        {
            VatType lineVatType = AllVatTypes.Where(d => d.Id == invoiceline.VatTypeId).FirstOrDefault();
            if (lineVatType != null)
            {
                if (!lineVatType.IsMultiPercentage)
                {
                    invoiceLinesVatAmountLocal = invoiceLinesVatAmountLocal + (invoiceline.LocalCurrencyAmount + invoiceline.LocalCurrencyAmount * invoiceline.VatPercentage / 100);
                    invoiceLinesVatAmountProfit = invoiceLinesVatAmountProfit + (invoiceline.ProfitCurrencyAmount + invoiceline.ProfitCurrencyAmount * invoiceline.VatPercentage / 100);
                }

                else
                {
                    this.CalculatePayablesVatAmountInMultiVat_InvoiceLine(invoiceline.VatTypeId, invoiceline.LocalCurrencyAmount, invoiceline.ProfitCurrencyAmount);
                }
            }
        }

        private void CalculatePayablesVatAmountInMultiVat_InvoiceLine(string vatTypeId, double? LocalAmount, double? profitAmount)
        {
            List<VATTypesGroup> allVATTypesGroup = (from d in CommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
            List<VATTypesGroup> vatTypesGroup = allVATTypesGroup.Where(d => d.GroupVATTypeId == vatTypeId).ToList();
            invoiceLinesVatAmountLocal = invoiceLinesVatAmountLocal + LocalAmount;
            invoiceLinesVatAmountProfit = invoiceLinesVatAmountProfit + profitAmount;
            foreach (VATTypesGroup itemGroup in vatTypesGroup)
            {
                VatType vatType = AllVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                VatTypePercentagePM myPercentagePM = AllVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    var vatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                    invoiceLinesVatAmountLocal = invoiceLinesVatAmountLocal + (LocalAmount * vatTypePercentage / 100);
                    invoiceLinesVatAmountProfit = invoiceLinesVatAmountProfit + (profitAmount * vatTypePercentage / 100);
                }
            }
        }

        private void UpdateOpenPayablesVatAmount(ShipmentPayable myPayable)
        {
            double? openAmountInLocalCurrency = CalculatePayableVatOpenAmount(myPayable, myPayable.OpenAmountInLocalCurrency);
            double? openAmountInProfitCurrency = CalculatePayableVatOpenAmount(myPayable, myPayable.OpenAmountInProfitCurrency);
            myPayable.VatAmountLocal = MethodHelper.Round(invoiceLinesVatAmountLocal + (openAmountInLocalCurrency == null ? 0 : openAmountInLocalCurrency), 2);
            myPayable.VatAmountProfit = MethodHelper.Round(invoiceLinesVatAmountProfit + (openAmountInProfitCurrency == null ? 0 : openAmountInProfitCurrency), 2);
        }

        private double? CalculatePayableVatOpenAmount(ShipmentPayable shipmentPayable, double? amount)
        {
            string payableVatTypeId = this.GetPayableVatTypeId(shipmentPayable);
            double? vatAmount = amount;
            if (!string.IsNullOrEmpty(payableVatTypeId))
            {
                VatType lineVatType = AllVatTypes.Where(d => d.Id == payableVatTypeId).FirstOrDefault();
                if (lineVatType != null)
                {
                    if (!lineVatType.IsMultiPercentage)
                    {
                        var vatTypePercentagePM = AllVatPercentages.Where(d => d.VatTypeId == payableVatTypeId).FirstOrDefault();
                        if (vatTypePercentagePM != null)
                        {
                            var percentage = vatTypePercentagePM.Percentage;
                            vatAmount = MethodHelper.Round(amount + (amount * percentage / 100), 2);
                        }
                    }
                    else
                    {
                        vatAmount = CalculatePayablesVatAmountInMultiVat_OpenLine(lineVatType.Id, amount);
                    }
                }
            }
            return vatAmount;
        }
        private string GetPayableVatTypeId(ShipmentPayable shipmentPayable)
        {
            string payableVatTypeId = null;
            if (!string.IsNullOrEmpty(shipmentPayable.VendorId))
            {
                Card myCard = CardRepository.GetSingleCard(shipmentPayable.VendorId, tenant, false);
                if (myCard != null)
                    payableVatTypeId = myCard.VatTypeId;
            }

            if (string.IsNullOrEmpty(payableVatTypeId))
                payableVatTypeId = shipmentPayable.VatTypeId;

            return payableVatTypeId;
        }

        private double? CalculatePayablesVatAmountInMultiVat_OpenLine(string vatTypeId, double? amount)
        {
            double? vatAmount = amount;
            List<VATTypesGroup> allVATTypesGroup = (from d in CommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
            List<VATTypesGroup> vatTypesGroup = allVATTypesGroup.Where(d => d.GroupVATTypeId == vatTypeId).ToList();
            foreach (VATTypesGroup itemGroup in vatTypesGroup)
            {
                VatTypePercentagePM myPercentagePM = AllVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    var vatTypePercentage = MethodHelper.GetValue(myPercentagePM.Percentage);
                    vatAmount = vatAmount + (amount * vatTypePercentage / 100);
                }
            }
            return MethodHelper.Round(vatAmount, 2);
        }

        private  List<VatType> AllVatTypes { get;  set; }
        private List<VatTypePercentagePM> AllVatPercentages { get; set; }
        private void InitializeVATs()
        {
            DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? todayDate = todayDateTime.Value.Date;
            VatTypeRepository vatTypeRepository = new VatTypeRepository(CommonContext);
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(CommonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
            AllVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();
            AllVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, todayDate);
        }
    }
}
