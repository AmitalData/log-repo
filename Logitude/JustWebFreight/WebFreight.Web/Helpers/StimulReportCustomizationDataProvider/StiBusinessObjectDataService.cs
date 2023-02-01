using Logitude.Accounting.BL.DataContract;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.AccountingModel.Reports.BankDeposit;
using WebFreight.Web.AccountingModel.Reports.Interest;
using WebFreight.Web.AccountingModel.Reports.Journal;
using WebFreight.Web.AccountingModel.Reports.PaymentCheque;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class StiBusinessObjectDataService
    {

        public List<StiBusinessObjectData> Get(DocumentTypeTemplatePM documentTypeTemplatePM)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("SSP", documentTypeTemplatePM.Tenant))
                return new List<StiBusinessObjectData>();
            List<StiBusinessObjectData> businessObjects = new List<StiBusinessObjectData>();
            DocumentDataProviderArgs documentDataProviderArgs = null;
            switch (documentTypeTemplatePM.DocumentTypeCode)
            {
                case "EXCU":
                case "SELE":
                case "TML":
                case "740PP":
                case "740":
                case "AVISC":
                    {
                        //businessObjects.Add(new StiBusinessObjectData("ShipmentPM", "ShipmentPMDataProvider", "ShipmentPMDataProvider", typeof(ShipmentPM)));
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(AWBDataProvider), Category = "AWB" };
                        break;
                    }
                case "714":
                case "714PP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(AWBDataProvider), Category = "HAWB" };
                        break;
                    }
                case "TZU":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(DeclarationFormsDataProvider), Category = "FORM" };
                        break;

                    }
                case "ITDT":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(InterestDataProvider), Category = "ITDT" };
                        break;
                    }
                case "JRPR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(JournalDataProvider), Category = "JRPR" };
                        break;
                    }
                case "BDPR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(BankDepositDataProvider), Category = "BDPR" };
                        break;
                    }
                case "PCDR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(PaymentChequeDataProvider), Category = "PCDR" };
                        break;

                    }
                case "TDDP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(TaxDeductionReportData), Category = "TDDP" };
                        break;
                    }
                case "OFDP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(OpenFormatReportDataProvider), Category = "OFDP" };
                        break;
                    }
                case "MBOL":
                case "SBOL":
                case "716":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(FBLDataProvider), Category = "FBL" };
                        break;
                    }
                case "ESU":
                case "890":
                case "DEOR":
                case "LCOT":
                case "ARNT":
                case "COO":
                case "BCO":
                case "716SD":
                case "PND":
                case "SFBL":
                case "BCS":
                case "IFI":
                case "GAPS":
                case "DOR":
                case "PGDF":
                case "REOR":
                case "860":
                case "865":
                case "852":
                case "PROD":
                case "ATME":
                case "DRA":
                case "SVDF":
                case "CA":
                case "WHR":
                case "AVDE":
                case "861":
                case "862":
                case "863":
                case "BCA":
                case "CRCT":
                case "CRCD":
                case "CRCC":
                case "PCRC":
                case "HORD":
                case "SOPI":
                case "ETO":
                case "ITO":
                case "SSN":
                case "CRCW":
                case "CRCO":
                case "CRCI":
                case "CRCCU":
                case "CRCCM":
                case "CRCCB":
                case "DESCH":
                case "WESL":
                case "SHCO":
                case "ABOCO":
                case "SHCMR":
                case "NCR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ShippingDeclarationDataProvider), Category = "Shipping declaration" };
                        break;
                    }
                case "740L":
                case "740HL":
                case "LCLL":
                case "LAL":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(AWBLabelsDataProvider), Category = "AWB Labels" };
                        break;
                    }
                case "BDE":
                case "782":
                case "783":
                case "784":// Delivery note
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(DeliveryNoteDataProvider), Category = "Delivery Note" };
                        break;
                    }
                case "781":// Pickup note
                case "DORE":
                case "TBOL":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(DeliveryNoteDataProvider), Category = "Pickup Note" };
                        break;
                    }
                case "999CI":
                case "999S":// Shipment invoice
                case "999M":
                case "ARINV":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(InvoiceDataProvider), Category = "Shipment Invoice" };
                        break;
                    }
                case "999G":
                case "999C":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(InvoiceDataProvider), Category = "Consolidation Invoice" };
                        break;
                    }
                case "CMR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CMRDataProvider), Category = "CMR" };
                        break;
                    }
                case "SCMR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CMRDataProvider), Category = "SCMR" };
                        break;
                    }
                case "DELI":
                case "785A":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ManifestDataProvider), Category = "785A" };
                        break;
                    }
                case "OMBC":
                case "785O":
                case "INMA":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ManifestDataProvider), Category = "785O" };
                        break;
                    }
                case "PROF":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ShipmentProfitDataProvider), Category = "Shipment Profit" };
                        break;
                    }
                case "PAO":
                case "PAA":
                case "CPA":
                case "CPO":
                case "CPI":
                case "CPIO":
                case "CPE":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(PreAlertDataProvider), Category = "Pre Alert" };
                        break;
                    }
                case "ARP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(PaymentDataProvider), Category = "Paymant" };
                        break;
                    }
                case "APP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(APPaymentDataProvider), Category = "APPaymant" };
                        break;
                    }
                case "CARICOM":
                case "PALI":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ShipmentPackingDataProvider), Category = "Packing List" };
                        break;
                    }
                case "PALN":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ShipmentProfitInvoicesDataProvider), Category = "Shipment Profit Invoice" };
                        break;
                    }
                case "999P":// Shipment AP invoice
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(APInvoiceDataProvider), Category = "Shipment AP Invoice" };
                        break;
                    }
                case "999MP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(APInvoiceDataProvider), Category = "Multiple Shipment AP Invoice" };
                        break;
                    }
                case "POA":
                case "OPPA":
                case "OPPB":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(OpportunitySummaryDataProvider), Category = "Opportunity Summary" };
                        break;
                    }
                case "CDE":
                case "WHL":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CrossDockEntryDataProvider), Category = "CrossDockEntry" };
                        break;
                    }
                case "CDR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CrossDockReleaseDataProvider), Category = "CrossDockRelease" };
                        break;
                    }
                case "CRR":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CrossDockReleaseDataProvider), Category = "CrossDockRelease" };
                        break;
                    }
                case "INVS":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(ShipmentInventoryDataProvider), Category = "Shipment Inventory" };
                        break;
                    }
                case "SBOLP":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(FBLDataProvider), Category = "FBL" };
                        break;
                    }
                case "WELB":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(CrossDockEntryDataProvider), Category = "Cross Docks Entry Labels" };
                        break;
                    }
                case "TEST":
                    {
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(FBLDataProvider), Category = "FBL" };
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (documentDataProviderArgs == null) return businessObjects;

            documentDataProviderArgs.DocumentTypeTemplatePM = documentTypeTemplatePM;
            var documentDataProvider = new DocumentDataProviderGreator(documentDataProviderArgs).Create();
            businessObjects.Add(new StiBusinessObjectData(documentDataProviderArgs.Category, documentDataProvider.Name, documentDataProvider.Name, documentDataProvider.Type));
            return businessObjects;


        }


    }
}