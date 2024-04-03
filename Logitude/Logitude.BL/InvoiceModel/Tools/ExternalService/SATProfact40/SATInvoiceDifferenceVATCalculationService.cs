using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI40;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATInvoiceDifferenceVATCalculationService
    {
        const int maxRoundedDigits = 2;
        private ARInvoicePM arInvoicePM;
        public List<ARInvoiceLinePM> CorrectARInvoiceLinesPM(SATInvoiceDifferenceVATCalculationServiceArgs args)
        {
            List<ARInvoiceLinePM> correctedLinesVatAmounts = new List<ARInvoiceLinePM>();
            
            if (args.CorrectARInvoiceLinesVatAmount)
            {
                correctedLinesVatAmounts = GetCorrectedLinesVatAmounts(args);
            }

            if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;

            decimal precentage = (decimal.Parse(args.TasaOCuota.TrimEnd('0')) * 100);
            throw new Exception("Due to the SAT Invoice Transmission we calculate the VAT amount per line. There is a difference between the lines VAT sum and the total VAT (" + args.SATVatAmount + ") at the invoice level. You are not allowed to approve the invoice unless you adjust the lines with the following VAT : " + precentage.ToString().TrimEnd('0').TrimEnd('.') + "%");
        }

        private List<ARInvoiceLinePM> GetCorrectedLinesVatAmounts(SATInvoiceDifferenceVATCalculationServiceArgs args)
        {
            string tasaOCuota = args.TasaOCuota;
            decimal sATVatAmount = args.SATVatAmount;
            arInvoicePM = args.ARInvoicePM;
            double vatPrecentage = (double)(decimal.Parse(tasaOCuota.TrimEnd('0')) * 100);
            ARInvoiceLineRepository aRInvoiceLineRepository = new ARInvoiceLineRepository(arInvoicePM.Tenant);
            List<ARInvoiceLinePM> aRInvoiceLinePMs = GetARInvoiceLinePMs(args, vatPrecentage);
            double totalSumLogitudeVatAmountPerLines = GetTotalSumLogitudeVatAmountPerLines(aRInvoiceLinePMs);

            CorrectedVatAmountArgs correctedVatAmountArgs = new CorrectedVatAmountArgs
            {
                sATVatAmount = sATVatAmount,
                vatPrecentage = vatPrecentage,
                aRInvoiceLineRepository = aRInvoiceLineRepository,
                totalSumLogitudeVatAmountPerLines = totalSumLogitudeVatAmountPerLines,
            };

            List<ARInvoiceLinePM> correctedLinesVatAmounts = CalcualteCorrectedLinesVatAmountsByOneLine(aRInvoiceLinePMs, correctedVatAmountArgs);
            
            if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;

            //Try with two lines:::
            correctedLinesVatAmounts = CalcualteCorrectedLinesVatAmountsByTwoLines(aRInvoiceLinePMs, correctedVatAmountArgs);

            return correctedLinesVatAmounts;
        }

        private List<ARInvoiceLinePM> CalcualteCorrectedLinesVatAmountsByOneLine(List<ARInvoiceLinePM> aRInvoiceLinePMs, CorrectedVatAmountArgs correctedVatAmountArgs)
        {
            List<ARInvoiceLinePM> correctedLinesVatAmounts = new List<ARInvoiceLinePM>();
            foreach (ARInvoiceLinePM firstARInvoiceLinePM in aRInvoiceLinePMs)
            {
                correctedVatAmountArgs.FirstARInvoiceLinePM = firstARInvoiceLinePM;
                correctedLinesVatAmounts = CalcualteCorrectedLinesVatAmounts(correctedVatAmountArgs);
                if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;
            }

            return correctedLinesVatAmounts;
        }

        private List<ARInvoiceLinePM> CalcualteCorrectedLinesVatAmountsByTwoLines(List<ARInvoiceLinePM> aRInvoiceLinePMs, CorrectedVatAmountArgs correctedVatAmountArgs)
        {
            List<ARInvoiceLinePM> correctedLinesVatAmounts = new List<ARInvoiceLinePM>();
            int firstLineIndex = 0;
            foreach (ARInvoiceLinePM firstARInvoiceLinePM in aRInvoiceLinePMs)
            {
                int secondLineIndex = 0;
                foreach (ARInvoiceLinePM secondARInvoiceLinePM in aRInvoiceLinePMs)
                {
                    if (firstLineIndex != secondLineIndex)
                    {
                        correctedVatAmountArgs.FirstARInvoiceLinePM = firstARInvoiceLinePM;
                        correctedVatAmountArgs.SecondARInvoiceLinePM = secondARInvoiceLinePM;
                        correctedLinesVatAmounts = CalcualteCorrectedLinesVatAmounts(correctedVatAmountArgs);

                        if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;
                    }
                    secondLineIndex++;
                }
                firstLineIndex++;
            }

            return correctedLinesVatAmounts;
        }

        private List<ARInvoiceLinePM> CalcualteCorrectedLinesVatAmounts(CorrectedVatAmountArgs correctedVatAmountArgs)
        {
            correctedVatAmountArgs.StartedAddedVatAmount = -0.01;
            correctedVatAmountArgs.EndedAddedVatAmount = -0.3;
            correctedVatAmountArgs.AddIncrementalVatAmountBy = -0.01;
            List<ARInvoiceLinePM> correctedLinesVatAmounts = GetCorrectedLineVatAmountsPerLine(correctedVatAmountArgs);
            if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;

            correctedVatAmountArgs.StartedAddedVatAmount = 0.01;
            correctedVatAmountArgs.EndedAddedVatAmount = 0.3;
            correctedVatAmountArgs.AddIncrementalVatAmountBy = 0.01;
            correctedLinesVatAmounts = GetCorrectedLineVatAmountsPerLine(correctedVatAmountArgs);

            return correctedLinesVatAmounts;
        }

        private List<ARInvoiceLinePM> GetCorrectedLineVatAmountsPerLine(CorrectedVatAmountArgs correctedVatAmountArgs)
        {
            double startedAddedVatAmount = correctedVatAmountArgs.StartedAddedVatAmount;
            double endedAddedVatAmount = correctedVatAmountArgs.EndedAddedVatAmount;
            double addIncrementalVatAmountBy = correctedVatAmountArgs.AddIncrementalVatAmountBy;
            List<ARInvoiceLinePM> correctedLinesVatAmounts = new List<ARInvoiceLinePM>();
            for (double addedVatAmount = startedAddedVatAmount; ((addedVatAmount < endedAddedVatAmount) && addIncrementalVatAmountBy > 0) || ((addedVatAmount > endedAddedVatAmount) && addIncrementalVatAmountBy < 0); addedVatAmount += addIncrementalVatAmountBy)
            {
                addedVatAmount = MethodHelper.Roundd(addedVatAmount, maxRoundedDigits);
                correctedLinesVatAmounts = GetCorrectedLineVatAmountsPerAddedVatAmountLine(new CorrectedVatAmountArgs
                {
                    sATVatAmount = correctedVatAmountArgs.sATVatAmount,
                    vatPrecentage = correctedVatAmountArgs.vatPrecentage,
                    aRInvoiceLineRepository = correctedVatAmountArgs.aRInvoiceLineRepository,
                    totalSumLogitudeVatAmountPerLines = correctedVatAmountArgs.totalSumLogitudeVatAmountPerLines,
                    FirstARInvoiceLinePM = correctedVatAmountArgs.FirstARInvoiceLinePM,
                    SecondARInvoiceLinePM = correctedVatAmountArgs.SecondARInvoiceLinePM,
                    addedVatAmount = addedVatAmount
                });
                if (correctedLinesVatAmounts.Count() > 0) return correctedLinesVatAmounts;
            }

            return correctedLinesVatAmounts;
        }

        private List<ARInvoiceLinePM> GetCorrectedLineVatAmountsPerAddedVatAmountLine(CorrectedVatAmountArgs correctedVatAmountArgs)
        {
            decimal sATVatAmount = correctedVatAmountArgs.sATVatAmount;
            double vatPrecentage = correctedVatAmountArgs.vatPrecentage;
            double totalSumLogitudeVatAmountPerLines = correctedVatAmountArgs.totalSumLogitudeVatAmountPerLines;
            ARInvoiceLinePM firstARInvoiceLinePM = correctedVatAmountArgs.FirstARInvoiceLinePM;
            ARInvoiceLinePM secondARInvoiceLinePM = correctedVatAmountArgs.SecondARInvoiceLinePM;
            double firstInvoiceLineCurrencyAmount = GetInvoiceLineCurrencyAmount(correctedVatAmountArgs.vatPrecentage, correctedVatAmountArgs.FirstARInvoiceLinePM);
            double secondInvoiceLineCurrencyAmount = GetInvoiceLineCurrencyAmount(correctedVatAmountArgs.vatPrecentage, correctedVatAmountArgs.SecondARInvoiceLinePM);
            double addedVatAmount = correctedVatAmountArgs.addedVatAmount;
            double newTotalSumLogitudeVatAmountPerLines = totalSumLogitudeVatAmountPerLines + addedVatAmount;
            double newSATVatAmount = (double)sATVatAmount - firstInvoiceLineCurrencyAmount + MethodHelper.Roundd((MethodHelper.Roundd(firstARInvoiceLinePM.InvoiceCurrencyAmount + addedVatAmount, maxRoundedDigits) * (vatPrecentage / 100)), maxRoundedDigits);
            newSATVatAmount = secondARInvoiceLinePM == null ? newSATVatAmount : newSATVatAmount - secondInvoiceLineCurrencyAmount + MethodHelper.Roundd((MethodHelper.Roundd(secondARInvoiceLinePM.InvoiceCurrencyAmount - addedVatAmount, maxRoundedDigits) * (vatPrecentage / 100)), maxRoundedDigits);
            newTotalSumLogitudeVatAmountPerLines = secondARInvoiceLinePM == null ? newTotalSumLogitudeVatAmountPerLines : newTotalSumLogitudeVatAmountPerLines - addedVatAmount;
            double newTotalLogitudeVatAmountPerLines = MethodHelper.Roundd(newTotalSumLogitudeVatAmountPerLines * (vatPrecentage / 100), maxRoundedDigits);
            newSATVatAmount = MethodHelper.Roundd(newSATVatAmount, maxRoundedDigits);

            if (newSATVatAmount != newTotalLogitudeVatAmountPerLines) return new List<ARInvoiceLinePM>();

            List<ARInvoiceLinePM> correctedLines = GetARInvoiceCorrectedLines(firstARInvoiceLinePM, secondARInvoiceLinePM, addedVatAmount);

            return correctedLines;
        }

        private List<ARInvoiceLinePM> GetARInvoiceCorrectedLines(ARInvoiceLinePM firstARInvoiceLinePM, ARInvoiceLinePM secondARInvoiceLinePM, double addedVatAmount)
        {
            List<ARInvoiceLinePM> correctedLines = new List<ARInvoiceLinePM>();
            firstARInvoiceLinePM.InvoiceCurrencyAmount = MethodHelper.Roundd(firstARInvoiceLinePM.InvoiceCurrencyAmount + addedVatAmount, maxRoundedDigits);
            firstARInvoiceLinePM.ChangeSetOp = firstARInvoiceLinePM.ChangeSetOp == ChangeSetOperation.None ? ChangeSetOperation.Update : firstARInvoiceLinePM.ChangeSetOp;
            correctedLines.Add(firstARInvoiceLinePM);

            if (secondARInvoiceLinePM == null) return correctedLines;

            secondARInvoiceLinePM.InvoiceCurrencyAmount = MethodHelper.Roundd(secondARInvoiceLinePM.InvoiceCurrencyAmount - addedVatAmount, maxRoundedDigits);
            secondARInvoiceLinePM.ChangeSetOp = secondARInvoiceLinePM.ChangeSetOp == ChangeSetOperation.None ? ChangeSetOperation.Update : secondARInvoiceLinePM.ChangeSetOp;
            correctedLines.Add(secondARInvoiceLinePM);

            return correctedLines;
        }

        private double GetInvoiceLineCurrencyAmount(double vatPrecentage, ARInvoiceLinePM aRInvoiceLinePM)
        {
            if (aRInvoiceLinePM == null) return 0.0;
            double invoiceLineCurrencyAmount = MethodHelper.Roundd(aRInvoiceLinePM.InvoiceCurrencyAmount, maxRoundedDigits);
            double roundedInvoiceLineCurrencyAmount = MethodHelper.Roundd((invoiceLineCurrencyAmount * (vatPrecentage / 100)), maxRoundedDigits);
            return roundedInvoiceLineCurrencyAmount;
        }

        private List<ARInvoiceLinePM> GetARInvoiceLinePMs(SATInvoiceDifferenceVATCalculationServiceArgs args, double vatPrecentage)
        {
            double invoiceLinesVatPercentage = args.IsNegativeTax ? (vatPrecentage * -1) : vatPrecentage;
            if (!arInvoicePM.InvoiceLines.Where(invoiceLine => invoiceLine.VatIsMultiPercentage).Any())
            {
                return arInvoicePM.InvoiceLines.Where(invoiceLine => IsARInvoiceLineWithSamePercentage(args, invoiceLine, invoiceLinesVatPercentage))
                .OrderByDescending(a => a.InvoiceCurrencyAmount).ToList();
            }

            List<ARInvoiceLinePM> aRInvoiceLinePMs = arInvoicePM.InvoiceLines.Where(invoiceLine => IsARInvoiceLineWithSamePercentage(args, invoiceLine, invoiceLinesVatPercentage)).ToList();

            List<string> multiVatPercetnageIds = arInvoicePM.InvoiceLines.Where(invoiceLine => invoiceLine.VatIsMultiPercentage).GroupBy(line => line.VatTypeId).Select(line => line.Key).ToList();
            List<VatTypePM> vatTypePMs = GetSelectedVatTypePMs(multiVatPercetnageIds);

            arInvoicePM.InvoiceLines.ForEach(line =>
            {
                AddLineToMultVatPercentageARInvoiceLines(new MultVatTypePercentageARInvoiceLineAdder {
                    AllExpenseShipmentReceivables = args.AllExpenseShipmentReceivables,
                    line = line,
                    invoiceLinesVatPercentage = invoiceLinesVatPercentage,
                    aRInvoiceLinePMs = aRInvoiceLinePMs,
                    vatTypePMs = vatTypePMs
                });
            });

            return aRInvoiceLinePMs.OrderByDescending(a => a.InvoiceCurrencyAmount).ToList();
        }

        private void AddLineToMultVatPercentageARInvoiceLines(MultVatTypePercentageARInvoiceLineAdder multVatTypePercentageARInvoiceLineAdder)
        {
            ARInvoiceLinePM line = multVatTypePercentageARInvoiceLineAdder.line;
            if (IsExpenseLineWithoutPayableVendor(multVatTypePercentageARInvoiceLineAdder.AllExpenseShipmentReceivables, line) || line.VatTypeId == null || !line.VatIsMultiPercentage) return;

            VatTypePM vatTypePM = multVatTypePercentageARInvoiceLineAdder.vatTypePMs.Where(vatType => vatType.Id == line.VatTypeId).FirstOrDefault();
            if (vatTypePM == null || vatTypePM.VatTypePercentages == null) return;

            if (vatTypePM.VatTypePercentages.Any(vatTypePercentage => vatTypePercentage.Percentage == multVatTypePercentageARInvoiceLineAdder.invoiceLinesVatPercentage))
            {
                multVatTypePercentageARInvoiceLineAdder.aRInvoiceLinePMs.Add(line);
            }
        }

        private List<VatTypePM> GetSelectedVatTypePMs(List<string> multiVatPercetnageIds)
        {
            VatTypeQuery vatTypeQuery = new VatTypeQuery(arInvoicePM.Tenant);
            List<VatTypePM> VatTypePMs = new List<VatTypePM>();
            multiVatPercetnageIds.ForEach(multiVatPercetnageId =>
            {
                VatTypePM vatTypePM = AddVatTypePercentagesToMultiVat(multiVatPercetnageId, vatTypeQuery);
                if (vatTypePM != null) VatTypePMs.Add(vatTypePM);
            });
            return VatTypePMs;
        }

        private VatTypePM AddVatTypePercentagesToMultiVat(string multiVatPercetnageId, VatTypeQuery vatTypeQuery)
        {
            VatTypePM multiVatTypePM = vatTypeQuery.GetSinglePM(multiVatPercetnageId, arInvoicePM.Tenant);
            if (multiVatTypePM == null) return null;
            if (multiVatTypePM.VatTypeGroups == null) return multiVatTypePM;
            if (multiVatTypePM.VatTypePercentages == null) multiVatTypePM.VatTypePercentages = new List<VatTypePercentagePM>();

            multiVatTypePM.VatTypeGroups.ForEach(vatType =>
            {
                AddSingleVatTypePercentagesToMultiVatTypePercentages(vatTypeQuery, vatType, multiVatTypePM);
            });
            return multiVatTypePM;
        }

        private void AddSingleVatTypePercentagesToMultiVatTypePercentages(VatTypeQuery vatTypeQuery, VATTypesGroupPM vatType, VatTypePM multiVatTypePM)
        {
            VatTypePM singleVatTypePM = vatTypeQuery.GetSinglePM(vatType.SingleVATTypeId, arInvoicePM.Tenant);
            if (singleVatTypePM == null) return;
            if (singleVatTypePM.VatTypePercentages == null) return;

            multiVatTypePM.VatTypePercentages = multiVatTypePM.VatTypePercentages.Concat(singleVatTypePM.VatTypePercentages).ToList();
        }

        private bool IsARInvoiceLineWithSamePercentage(SATInvoiceDifferenceVATCalculationServiceArgs args, ARInvoiceLinePM invoiceLine, double invoiceLinesVatPercentage)
        {
            return !IsExpenseLineWithoutPayableVendor(args.AllExpenseShipmentReceivables, invoiceLine) && invoiceLine.VatTypeId != null && invoiceLine.VatPercentage == invoiceLinesVatPercentage;
        }

        private double GetTotalSumLogitudeVatAmountPerLines(List<ARInvoiceLinePM> aRInvoiceLinePMs)
        {
            return MethodHelper.Roundd((double)aRInvoiceLinePMs.Sum(aRInvoiceLinePM => aRInvoiceLinePM.InvoiceCurrencyAmount), maxRoundedDigits);
        }

        private bool IsExpenseLineWithoutPayableVendor(List<ShipmentReceivable>  allExpenseShipmentReceivables, ARInvoiceLinePM line)
        {
            return allExpenseShipmentReceivables.Where(receivable => receivable.Id == line.ReceivableId && string.IsNullOrEmpty(receivable.PayableVendorId)).Any();
        }
    }

    public class SATInvoiceDifferenceVATCalculationServiceArgs
    {
        public ARInvoicePM ARInvoicePM { get; set; }
        public decimal SATVatAmount { get; set; }
        public decimal LogitudeVatAmount { get; set; }
        public string TasaOCuota { get; set; }
        public List<ShipmentReceivable> AllExpenseShipmentReceivables {get;set; }
        public bool CorrectARInvoiceLinesVatAmount { get; set; }
        public bool IsNegativeTax { get; set; }
    }

    public class CorrectedVatAmountArgs
    {
        public double vatPrecentage { get; set; }
        public decimal sATVatAmount { get; set; }
        public ARInvoiceLineRepository aRInvoiceLineRepository { get; set; }
        public double totalSumLogitudeVatAmountPerLines { get; set; }
        public ARInvoiceLinePM FirstARInvoiceLinePM { get; set; }
        public ARInvoiceLinePM SecondARInvoiceLinePM { get; set; }
        public double addedVatAmount { get; set; }
        public double StartedAddedVatAmount { get; set; }
        public double EndedAddedVatAmount { get; set; }
        public double AddIncrementalVatAmountBy { get; set; }
    }

    public class MultVatTypePercentageARInvoiceLineAdder
    {
        public ARInvoiceLinePM line { get; set; }
        public double invoiceLinesVatPercentage { get; set; }
        public List<ARInvoiceLinePM> aRInvoiceLinePMs { get; set; }
        public List<VatTypePM> vatTypePMs { get; set; }
        public List<ShipmentReceivable> AllExpenseShipmentReceivables { get; set; }
    }
}
