using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATInvoiceComprobante
    {
        private ICommonDataContext commonContext;
        private IInvoiceContext invoiceCotnext;
        private Currency invoiceCurrency;
        private ARInvoicePM arInvoicePM;
        private int tenant;
        private Tenant currentTenant;
        private Address billToAddress;
        private Card billToCard;
        private PaymentTerm paymentTerm;
        private List<VatType> allVatTypes;
        private List<VatTypePercentagePM> allVatPercentages;
        private List<VATTypesGroup> allVatGroups;
        private List<ChargesType> allChargesTypes;
        private List<ShipmentReceivable> allExpenseShipmentReceivables;
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        private SATInterfaceSetting satSetting;
        private string invoiceCurrencyCode;
        private List<ARInvoiceLinePM> correctedARInvoiceTrasladoLines = new List<ARInvoiceLinePM>();
        private List<ARInvoiceLinePM> correctedARInvoiceRetencionLines = new List<ARInvoiceLinePM>();
        private List<ARInvoiceLinePM> correctedARInvoiceRetencionDRLines = new List<ARInvoiceLinePM>();
        private bool CorrectARInvoiceLinesVATAmount;
        public SATInvoiceComprobante(ARInvoicePM arInvoicePM, Tenant currentTenant, SATInterfaceSetting satSetting)
        {
            this.arInvoicePM = arInvoicePM;
            tenant = arInvoicePM.Tenant;
            this.currentTenant = currentTenant;
            this.satSetting = satSetting;
            commonContext = CommonDataContext.GetContext(tenant);
            invoiceCotnext = InvoiceContext.GetContext(tenant);
            computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
            paymentTerm = GetPaymentTerm();
            billToAddress = GetBillToAddress();
            billToCard = GetBillToCard();
            allVatTypes = GetAllVatTypes();
            allVatPercentages = GetAllVatPercentages();
            allVatGroups = GetAllVatGroups();
            allChargesTypes = GetAllChargesTypes();
            allExpenseShipmentReceivables = GetAllExpenseShipmentReceivables();
            invoiceCurrencyCode = GetInvoiceCurrencyCode();
        }

        private PaymentTerm GetPaymentTerm()
        {
            PaymentTermRepository paymentTermRepository = new PaymentTermRepository(commonContext);
            return paymentTermRepository.GetSinglePaymentTerm(arInvoicePM.PaymentTermId); ;
        }

        private Address GetBillToAddress()
        {
            AddressRepository addressRepository = new AddressRepository(commonContext);
            if (!string.IsNullOrEmpty(arInvoicePM.BillToAddressId))
            {
                return addressRepository.GetSingleAddress(arInvoicePM.BillToAddressId, tenant);
            }

            return null;
        }

        private Card GetBillToCard()
        {
            CardRepository cardRepository = new CardRepository(commonContext);
            return cardRepository.GetSingleCard(arInvoicePM.BillToId, tenant);
        }

        private List<VatType> GetAllVatTypes()
        {
            return commonContext.VatTypes.Where(d => d.Tenant == tenant).ToList();
        }

        private List<VatTypePercentagePM> GetAllVatPercentages()
        {
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(commonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
            return myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
        }

        private List<VATTypesGroup> GetAllVatGroups()
        {
            return (from d in commonContext.VATTypesGroups
                    where d.Tenant == arInvoicePM.Tenant
                    select d).ToList();
        }

        private List<ChargesType> GetAllChargesTypes()
        {
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(commonContext);
            return chargesTypeRepository.GetChargesTypes(tenant).ToList();
        }

        private List<ShipmentReceivable> GetAllExpenseShipmentReceivables()
        {
            List<ShipmentReceivable> allExpenseShipmentReceivables = new List<ShipmentReceivable>();
            List<string> expenseShipmentReceivablesIds = arInvoicePM.InvoiceLines.Where(x => x.IsExpense).Select(a => a.ReceivableId).ToList();
            if (expenseShipmentReceivablesIds.Count == 0) return allExpenseShipmentReceivables;

            ShipmentReceivableRepository shipmentReceivableRepository = new ShipmentReceivableRepository(arInvoicePM.Tenant);
            allExpenseShipmentReceivables = shipmentReceivableRepository.GetShipmentReceivablesByIds(expenseShipmentReceivablesIds, arInvoicePM.Tenant);
            return allExpenseShipmentReceivables;
        }

        public InvoiceComprobanteBuilderResultArgs BuildNewInvoiceComprobante(InvoiceComprobanteBuilderArgs invoiceComprobanteBuilderArgs)
        {
            CorrectARInvoiceLinesVATAmount = invoiceComprobanteBuilderArgs.CorrectARInvoiceLinesVatAmount;
            string arInvoiceCounterPrefix = GetARInvoiceCounterPrefix();

            Comprobante comprobante = new Comprobante
            {
                Version = SATData.CurrentComprobanteVersion,
                Exportacion = SATData.ComprobanteExportacion,
                Moneda = invoiceCurrencyCode,
                Serie = GetSerie(arInvoiceCounterPrefix),
                Folio = GetFolio(arInvoiceCounterPrefix),
                Fecha = GetFecha(),
                FormaPago = GetFormaPago(),
                FormaPagoSpecified = true,
                MetodoPago = GetMetodoPago(),
                MetodoPagoSpecified = true,
                SubTotal = GetARInvoiceCurrencySubTotal(),
                Total = GetARInvoiceCurrencyAmount(),
                Emisor = Emisor.Get(currentTenant, satSetting),
                Receptor = GetComprobanteReceptor(),
                TipoDeComprobante = GetTipoDeComprobante(),
                CondicionesDePago = GetCondicionesDePago(),
                TipoCambio = GetARInvoiceCurrencyExchangeRate(),
                TipoCambioSpecified = !IsMexicanInvoiceCurrency(),
                LugarExpedicion = GetLugarExpedicion(GetBranchAddress(), currentTenant),
                Conceptos = GetConceptoList().ToArray(),
                CfdiRelacionados = GetRelatedInvoiceTag()
            };

            comprobante.InformacionGlobal = GetInformacionGlobal(comprobante.Receptor);


            List<ARInvoiceTotalVATPM> arTotalVats = GetARInvoiceTotalVATPMs();
            MapComprobanteTotalAndSubTotal(comprobante, arTotalVats);
            MapTotalAndSubTotalWithMatchCurrencyDigitsAfterPoint(comprobante);

            comprobante.Impuestos = GetComprobanteImpuestos(arTotalVats, comprobante.Conceptos)?.ComprobanteImpuestos;

            return new InvoiceComprobanteBuilderResultArgs
            {
                Comprobante = comprobante,
                CorrectedARInvoiceTrasladoLines = correctedARInvoiceTrasladoLines,
                CorrectedARInvoiceRetencionLines = correctedARInvoiceRetencionLines,
                CorrectedARInvoiceRetencionDRLines = correctedARInvoiceRetencionDRLines,
            };
        }

        private void MapComprobanteTotalAndSubTotal(Comprobante comprobante, List<ARInvoiceTotalVATPM> arTotalVats)
        {
            ComprobanteTotalAndSubTotal comprobanteTotalAndSubTotal = GetComprobanteTotalAndSubTotal(arTotalVats);
            comprobante.Total = comprobanteTotalAndSubTotal.Total;
            comprobante.SubTotal = comprobanteTotalAndSubTotal.SubTotal;
        }

        private void MapTotalAndSubTotalWithMatchCurrencyDigitsAfterPoint(Comprobante comprobante)
        {
            comprobante.Total = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(comprobante.Total, comprobante.Moneda);
            comprobante.SubTotal = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(comprobante.SubTotal, comprobante.Moneda);
        }

        private string GetInvoiceCurrencyCode()
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);
            invoiceCurrency = currencyRepository.GetSingleCurrency(arInvoicePM.InvoiceCurrencyId, tenant);

            return invoiceCurrency.Code;
        }

        private string GetARInvoiceCounterPrefix()
        {
            if (arInvoicePM.IsConstituentInvoice)
            {
                return TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "CNST", "CNS", null);
            }
            else if (arInvoicePM.IsConsolidationInvoice)
            {
                return TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "INVC", "CON", null);
            }
            else
            {
                return TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "INVC", arInvoicePM.ARInvoiceTypeCode, null);
            }
        }

        private string GetSerie(string arInvoiceCounterPrefix)
        {
            string serie = "A";

            if (!string.IsNullOrEmpty(arInvoiceCounterPrefix))
            {
                serie = arInvoiceCounterPrefix;
            }

            return serie;
        }

        private string GetFolio(string arInvoiceCounterPrefix)
        {
            string folio = arInvoicePM.InvoiceNumber;
            if (string.IsNullOrEmpty(arInvoiceCounterPrefix)) return folio;
            if (string.IsNullOrEmpty(arInvoicePM.InvoiceNumber)) return folio;

            if (arInvoicePM.InvoiceNumber.Contains(arInvoiceCounterPrefix))
            {
                folio = arInvoicePM.InvoiceNumber.Remove(0, arInvoiceCounterPrefix.Length);
            }

            return folio;
        }

        private DateTime GetFecha()
        {
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime arInvoiceDate = arInvoicePM.InvoiceDate.Value;
            return new DateTime(arInvoiceDate.Year, arInvoiceDate.Month, arInvoiceDate.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
        }

        private string GetFormaPago()
        {
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();
            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == arInvoicePM.SATPaymentMethodCode).FirstOrDefault();

            return satPaymentMethod.Code;
        }

        private string GetMetodoPago()
        {
            if (!string.IsNullOrEmpty(arInvoicePM.MetodoPagoCode))
            {
                return arInvoicePM.MetodoPagoCode;
            }

            if (!string.IsNullOrEmpty(billToCard.MetodoPagoCode))
                return billToCard.MetodoPagoCode;
            else
                return satSetting.MetodoPagoCode;
        }
        
        private decimal GetARInvoiceCurrencySubTotal()
        {
            return Math.Abs((arInvoicePM.SubTotalInInvoiceCurrency != null ? (decimal)arInvoicePM.SubTotalInInvoiceCurrency.Value : 0));
        }

        private decimal GetARInvoiceCurrencyAmount()
        {
            return Math.Abs((arInvoicePM.AmountInInvoiceCurrency != null ? (decimal)arInvoicePM.AmountInInvoiceCurrency.Value : 0));
        }

        private ComprobanteReceptor GetComprobanteReceptor()
        {
            string billToAddressZipCode = "";
            if (billToAddress != null && !string.IsNullOrEmpty(billToAddress.ZipCode))
            {
                billToAddressZipCode = SATBaseProfact40Service.GetBillToAddressZipCode(billToAddress, arInvoicePM.Tenant);
            }

            if (string.IsNullOrEmpty(arInvoicePM.RegimenFiscalCode) && !string.IsNullOrEmpty(billToCard.RegimenFiscalCode))
            {
                arInvoicePM.RegimenFiscalCode = billToCard.RegimenFiscalCode;
            }

            string billToCountryCode = GetBillToCountryCode();
            bool isMexicoCountry = IsMexicoCountry(billToCountryCode);
            string publicInGeneralMexicoRfc = GetPublicInGeneralMexicoRfc(isMexicoCountry);

            return new ComprobanteReceptor
            {
                Nombre = GetNombre(publicInGeneralMexicoRfc),
                RegimenFiscalReceptor = GetRegimenFiscalReceptor(isMexicoCountry, publicInGeneralMexicoRfc),
                DomicilioFiscalReceptor = GetDomicilioFiscalReceptor(billToAddressZipCode, isMexicoCountry, publicInGeneralMexicoRfc),
                Rfc = GetReceptorRfc(isMexicoCountry, publicInGeneralMexicoRfc),
                UsoCFDI = GetReceptorUsoCFDI(isMexicoCountry, publicInGeneralMexicoRfc),
                NumRegIdTrib = GetNumRegIdTrib(billToCountryCode),
                ResidenciaFiscal = GetResidenciaFiscal(billToCountryCode),
                ResidenciaFiscalSpecified = GetResidenciaFiscalSpecified(billToCountryCode)
            };
        }

        private string GetPublicInGeneralMexicoRfc(bool isMexicoCountry)
        {
            return isMexicoCountry && string.IsNullOrEmpty(billToCard.VatNumber) ? SATData.PublicInGeneralMexicoRfc : "";
        }

        private string GetNombre(string publicInGeneralMexicoRfc)
        {
            if (publicInGeneralMexicoRfc == SATData.PublicInGeneralMexicoRfc) return SATData.PublicInGeneralNombre;
            return !string.IsNullOrEmpty(billToCard.SATCustomerName) ? billToCard.SATCustomerName : billToCard.EnglishName;
        }

        private string GetRegimenFiscalReceptor(bool isMexicoCountry, string publicInGeneralMexicoRfc)
        {
            const string regimenFiscalReceptor616Code = "616";
            if (!isMexicoCountry || publicInGeneralMexicoRfc == SATData.PublicInGeneralMexicoRfc)
            {
                return regimenFiscalReceptor616Code;
            }

            return arInvoicePM.RegimenFiscalCode;
        }

        private string GetDomicilioFiscalReceptor(string billToAddressZipCode, bool isMexicoCountry, string publicInGeneralMexicoRfc)
        {
            if (!isMexicoCountry || publicInGeneralMexicoRfc == SATData.PublicInGeneralMexicoRfc)
            {
                return GetLugarExpedicion(GetBranchAddress(), currentTenant);
            }

            return billToAddressZipCode;
        }

        private string GetReceptorRfc(bool isMexicoCountry, string publicInGeneralMexicoRfc)
        {
            if (!isMexicoCountry)
            {
                return SATData.OutSideMexicoRfc;
            }

            return !string.IsNullOrEmpty(billToCard.VatNumber) ? billToCard.VatNumber : publicInGeneralMexicoRfc;
        }

        private string GetNumRegIdTrib(string billToCountryCode)
        {
            if (IsMexicoCountry(billToCountryCode))
                return null;

            return !string.IsNullOrEmpty(billToCard.SATForeignRFC) ? billToCard.SATForeignRFC : SATData.OutSideMexicoRfc;
        }

        private string GetResidenciaFiscal(string billToCountryCode)
        {
            if (IsMexicoCountry(billToCountryCode))
                return null;

            return billToCountryCode;
        }

        private bool GetResidenciaFiscalSpecified(string billToCountryCode)
        {
            if (IsMexicoCountry(billToCountryCode))
                return false;

            return true;
        }

        private string GetBillToCountryCode()
        {
            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, SATData.ComputingPartnerCode, SATData.CountryObjectTableName);
            }

            return billToCountryCode;
        }

        private bool IsMexicoCountry(string countryCode)
        {
            return countryCode == "MEX" || countryCode == "MX";
        }

        private string GetReceptorUsoCFDI(bool isMexicoCountry, string publicInGeneralMexicoRfc)
        {
            const string usoCFDIS01Code = "S01";
            if (!isMexicoCountry || publicInGeneralMexicoRfc == SATData.PublicInGeneralMexicoRfc)
            {
                return usoCFDIS01Code;
            }

            if (!string.IsNullOrEmpty(arInvoicePM.UsoCFDICode))
            {
                return arInvoicePM.UsoCFDICode;
            }
            else
            {
                return arInvoicePM.ARInvoiceTypeCode == SATData.ComprobanteCD ? SATData.ComprobanteG02 : SATData.ComprobanteG03;
            }
        }

        private string GetTipoDeComprobante()
        {
            return arInvoicePM.ARInvoiceTypeCode == SATData.ComprobanteCD ? SATData.ComprobanteE : SATData.ComprobanteI;
        }

        private string GetCondicionesDePago()
        {
            return paymentTerm.LocalName != null ? paymentTerm.LocalName : paymentTerm.EnglishName;
        }

        private decimal GetARInvoiceCurrencyExchangeRate()
        {
            if (IsMexicanInvoiceCurrency()) return 0;

            return (arInvoicePM.InvoiceCurrencyExchangeRate != null ? Convert.ToDecimal(arInvoicePM.InvoiceCurrencyExchangeRate.Value) : 0);
        }

        private bool IsMexicanInvoiceCurrency()
        {
            return invoiceCurrency.Code == SATData.MexicanInvoiceCurrencyCode;
        }

        private ComprobanteInformacionGlobal GetInformacionGlobal(ComprobanteReceptor comprobanteReceptor)
        {
            const string dailyPeriodCode = "01";
            bool isPublicInGeneral = comprobanteReceptor.Nombre == SATData.PublicInGeneralNombre;
            string periodCode = isPublicInGeneral && !arInvoicePM.IsConsolidationInvoice ? dailyPeriodCode : arInvoicePM.PeriodCode;

            if (!arInvoicePM.IsConsolidationInvoice && !isPublicInGeneral) return null;

            DateTime arInvoiceDate = (DateTime)arInvoicePM.InvoiceDate;
            return new ComprobanteInformacionGlobal
            {
                Periodicidad = periodCode,
                Meses = arInvoiceDate.Month.ToString().PadLeft(2, '0'),
                Año = Convert.ToInt16(arInvoiceDate.Year),
            };
        }

        private string GetLugarExpedicion(Address branchAddress, Tenant currentTenant)
        {
            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                return branchAddress.ZipCode;
            }
            else
            {
                return currentTenant.Address.ZipCode;
            }
        }

        private Dictionary<int, string> GetSatMonths()
        {
            return new Dictionary<int, string> {
                {01,  "Enero"},
                {02,  "Febrero"},
                {03,  "Marzo"},
                {04,  "Abril"},
                {05,  "Mayo"},
                {06,  "Junio"},
                {07,  "Julio"},
                {08,  "Agosto"},
                {09,  "Septiembre"},
                {10, "Octubre"},
                {11, "Noviembre"},
                {12, "Diciembre"},
                {13, "Enero-Febrero"},
                {14, "Marzo-Abril"},
                {15, "Mayo-Junio"},
                {16, "Julio-Agosto"},
                {17, "Septiembre-Octubre"},
                {18, "Noviembre-Diciembre"}
            };
        }

        private Address GetBranchAddress()
        {
            if (string.IsNullOrEmpty(arInvoicePM.BranchId)) return null;

            BranchRepository branchRepository = new BranchRepository(commonContext);
            Branch branch = branchRepository.GetSingleBranch(arInvoicePM.BranchId, tenant);
            AddressRepository addressReposirory = new AddressRepository(commonContext);
            if (branch != null && !string.IsNullOrEmpty(branch.AddressId))
            {
                return addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
            }

            return null;
        }

        public List<ComprobanteConcepto> GetConceptoList()
        {
            MeasurementRepository measurementRepository = new MeasurementRepository(commonContext);
            List<Measurement> allMeasurements = measurementRepository.GetMeasurements(arInvoicePM.Tenant).ToList();
            List<ComprobanteConcepto> conceptosList = new List<ComprobanteConcepto>();
            foreach (ARInvoiceLinePM line in arInvoicePM.InvoiceLines)
            {
                AddNewComprobanteConcepto(allMeasurements, conceptosList, line);
            }

            return conceptosList;
        }

        private void AddNewComprobanteConcepto(List<Measurement> allMeasurements, List<ComprobanteConcepto> conceptosList, ARInvoiceLinePM line)
        {
            ComprobanteConcepto comprobanteConcepto = GetNewConcepto(allMeasurements, line);
            if (comprobanteConcepto != null) { 
                conceptosList.Add(comprobanteConcepto); 
            }
        }
        private ComprobanteConcepto GetNewConcepto(List<Measurement> allMeasurements, ARInvoiceLinePM line)
        {
            decimal conceptoCantidad = Math.Abs((line.Quantity != null ? ((decimal)line.Quantity.Value) : 0));
            decimal conceptoImporte = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? ((decimal)line.InvoiceCurrencyAmount.Value) : 0)));
            decimal valorUnitario = Math.Abs(conceptoCantidad != 0 ? (conceptoImporte / conceptoCantidad) : 0);
            decimal conceptoValorUnitario = SATBaseProfact40Service.GetDecimalWith3DigitsAfterPointIfZero(Math.Abs(Math.Truncate(valorUnitario * 1000000m) / 1000000m));
            string conceptoClaveProdServ = allChargesTypes.FirstOrDefault(c => c.Id == line.ChargesTypeId).SATExternalId;
            const string conceptoUnidad = "SERVICIO";

            ComprobanteConcepto concepto = new ComprobanteConcepto
            {
                ACuentaTerceros = GetComprobanteConceptoACuentaTerceros(line),
                ObjetoImp = SATData.IncludeTaxObjetoImp,
                Cantidad = conceptoCantidad,
                Unidad = conceptoUnidad,
                Descripcion = line.Description,
                Importe = conceptoImporte,
                ValorUnitario = conceptoValorUnitario,
                ClaveProdServ = conceptoClaveProdServ
            };

            if (IsExpenseLineWithoutPayableVendor(line)) return null;
            var lineMeasurement = allMeasurements.FirstOrDefault(m => m.Id == line.MeasurementId);
            if (lineMeasurement != null)
            {
                concepto.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(lineMeasurement.Code, "G-Profact", "Measurement");
            }

            this.CalucalteLineTotals(line, concepto, allVatTypes, allVatPercentages, allVatGroups);

            return concepto;
        }

        private bool IsExpenseLineWithoutPayableVendor(ARInvoiceLinePM line)
        {
            return allExpenseShipmentReceivables.Where(receivable => receivable.Id == line.ReceivableId && string.IsNullOrEmpty(receivable.PayableVendorId)).Any();
        }

        private ComprobanteConceptoACuentaTerceros GetComprobanteConceptoACuentaTerceros(ARInvoiceLinePM line)
        {
            if (!allChargesTypes.First(c => c.Id == line.ChargesTypeId).IsExpense) { return null; }

            ShipmentReceivableQuery shipmentReceivableQuery = new ShipmentReceivableQuery(arInvoicePM.Tenant);
            ShipmentReceivablePM shipmentReceivablePM = shipmentReceivableQuery.GetSinglePM(line.ReceivableId, arInvoicePM.Tenant);
            CardPM payableVendorPM = GetPayableVendorPM(shipmentReceivablePM);

            if (payableVendorPM == null)
            {
                return null;
            }

            return new ComprobanteConceptoACuentaTerceros
            {
                RfcACuentaTerceros = payableVendorPM.VatNumber,
                NombreACuentaTerceros = payableVendorPM.SATCustomerName,
                RegimenFiscalACuentaTerceros = payableVendorPM.RegimenFiscalCode,
                DomicilioFiscalACuentaTerceros = GetPayableVendorAddressZipCode(payableVendorPM),
            };
        }

        private CardPM GetPayableVendorPM(ShipmentReceivablePM shipmentReceivablePM)
        {
            if (shipmentReceivablePM == null || string.IsNullOrEmpty(shipmentReceivablePM.PayableVendorId))
            {
                return null;
            }

            CardQuery cardQuery = new CardQuery(arInvoicePM.Tenant);
            CardPM payableVendorPM = cardQuery.GetSinglePM(shipmentReceivablePM.PayableVendorId, arInvoicePM.Tenant);
            
            return payableVendorPM;
        }

        private string GetPayableVendorAddressZipCode(CardPM payableVendorPM)
        {
            AddressQuery addressQuery = new AddressQuery(payableVendorPM.Tenant);
            List<AddressPM> payableVendorAddresses = addressQuery.GetAddressesByCardId(payableVendorPM.Id, payableVendorPM.Tenant);
            const string billingAddressTypeCode = "B";
            AddressPM payableVendorBillingAddress = payableVendorAddresses.Where(address => address.AddressTypeId == billingAddressTypeCode).FirstOrDefault();

            if(payableVendorBillingAddress != null && !string.IsNullOrEmpty(payableVendorBillingAddress.ZipCode))
            {
                return payableVendorBillingAddress.ZipCode;
            }

            const string mainAddressTypeCode = "M";
            AddressPM payableVendorMainAddress = payableVendorAddresses.Where(address => address.AddressTypeId == mainAddressTypeCode).FirstOrDefault();

            return payableVendorMainAddress != null ? payableVendorMainAddress.ZipCode : "";
        }

        private void CalucalteLineTotals(ARInvoiceLinePM line, ComprobanteConcepto concepto, List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups)
        {
            List<ComprobanteConceptoImpuestosTraslado> lineTranslados = new List<ComprobanteConceptoImpuestosTraslado>();
            List<ComprobanteConceptoImpuestosRetencion> lineRetencions = new List<ComprobanteConceptoImpuestosRetencion>();

            VatType lineVatType = allVatTypes.Where(d => d.Id == line.VatTypeId).FirstOrDefault();

            if (!lineVatType.IsMultiPercentage)
            {
                AddSingleLineVatTypePercentage(line, lineTranslados, lineRetencions, lineVatType);
            }
            else
            {
                AddMultiLineVatTypePercentage(line, allVatTypes, allVatPercentages, allVatGroups, lineTranslados, lineRetencions, lineVatType);
            }

            BuildConceptoImpuestos(concepto, lineTranslados, lineRetencions);
        }

        private void AddMultiLineVatTypePercentage(ARInvoiceLinePM line, List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<ComprobanteConceptoImpuestosTraslado> lineTranslados, List<ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
        {
            List<ARInvoiceTotalVATPM> totalVats = new List<ARInvoiceTotalVATPM>();

            List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();
            List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == line.VatTypeId).ToList();
            foreach (VATTypesGroup itemGroup in myVatGroups)
            {
                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                {
                    Id = itemGroup.SingleVATTypeId,
                    VatTypeId = itemGroup.SingleVATTypeId,
                    LocalCurrencyAmount = line.LocalCurrencyAmount,
                    InvoiceCurrencyAmount = line.InvoiceCurrencyAmount,
                    ProfitCurrencyAmount = line.ProfitCurrencyAmount,
                };

                VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (vatType != null)
                {
                    newItem.ExternalVatCard = vatType.ReceivablesExternalId;
                    newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                }

                VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    newItem.VatTypePercentage = myPercentagePM.Percentage;
                }

                group_Source.Add(newItem);
            }

            List<InvoiceTotalsClass> group_data = (from items in group_Source
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
                ARInvoiceTotalVATPM record = new ARInvoiceTotalVATPM()
                {
                    Tenant = line.Tenant,
                    ARInvoiceId = line.ARInvoiceId,
                    VatTypeId = item.Id,
                    VATPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                    LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                    InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                    ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                    ExternalVATCard = item.ExternalVatCard,
                    ExternalTAXItemId = item.ExternalTAXItemId,
                };

                record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VATPercent / 100), 2);
                record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VATPercent / 100), 2);
                record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VATPercent / 100), 2);
                totalVats.Add(record);
            }

            foreach (ARInvoiceTotalVATPM lineTotal in totalVats)
            {
                if (lineTotal.VATPercent >= 0)
                {
                    AddMultiPercentageLineTranslados(lineTranslados, lineTotal);
                }
                else
                {
                    AddMultiPercentageLineRetencions(lineRetencions, lineVatType, lineTotal);
                }
            }
        }

        private void AddMultiPercentageLineTranslados(List<ComprobanteConceptoImpuestosTraslado> lineTranslados, ARInvoiceTotalVATPM lineTotal)
        {
            string vatTypeCode = allVatTypes.Where(d => d.Id == lineTotal.VatTypeId).FirstOrDefault()?.Code;
            ComprobanteConceptoImpuestosTraslado traslado = new ComprobanteConceptoImpuestosTraslado()
            {
                Base = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0))),
                Impuesto = "002",
                TipoFactor = (lineTotal.VATPercent == 0 && vatTypeCode == "EXMPT" ? "Exento" : "Tasa"),
            };

            if (traslado.TipoFactor == "Tasa")
            {
                BuildMultiLineTrasladoTasaTipoFactor(lineTotal, traslado);
            }

            lineTranslados.Add(traslado);
        }

        private void BuildMultiLineTrasladoTasaTipoFactor(ARInvoiceTotalVATPM lineTotal, ComprobanteConceptoImpuestosTraslado traslado)
        {
            traslado.TasaOCuota = lineTotal.VATPercent != 0 ? (lineTotal.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
            traslado.Importe = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2));
            traslado.ImporteSpecified = true;
            traslado.TasaOCuotaSpecified = true;
        }

        private void AddMultiPercentageLineRetencions(List<ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType, ARInvoiceTotalVATPM lineTotal)
        {
            ComprobanteConceptoImpuestosRetencion retencion = new ComprobanteConceptoImpuestosRetencion()
            {
                Impuesto = "002",
                TipoFactor = (lineTotal.VATPercent == 0 ? "Exento" : "Tasa"),
                TasaOCuota = (lineTotal.VATPercent != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8)) : 0),//(lineTotal.VatTypePercentage != null ? (decimal)(Math.Abs(lineTotal.VatTypePercentage.Value / 100)) : 0),
                Importe = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2)),//Math.Abs(((lineTotal.InvoiceCurrencyAmount != null ? ((decimal)lineTotal.InvoiceCurrencyAmount.Value) : 0) * ((lineTotal.VATPercent != null ? (decimal)lineTotal.VATPercent.Value : 0) / 100))),
            };

            if (lineVatType.Code != "EXMPT")
            {
                retencion.Base = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0)));//retencion.Importe;
            }

            lineRetencions.Add(retencion);
        }

        private void AddSingleLineVatTypePercentage(ARInvoiceLinePM line, List<ComprobanteConceptoImpuestosTraslado> lineTranslados, List<ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
        {
            if (line.VatPercentage >= 0)
            {
                AddSinglePercentageLineTranslados(line, lineTranslados, lineVatType);
            }
            else
            {
                AddSinglePercentageLineRetencions(line, lineRetencions, lineVatType);
            }
        }

        private void AddSinglePercentageLineRetencions(ARInvoiceLinePM line, List<ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
        {
            ComprobanteConceptoImpuestosRetencion retencion = new ComprobanteConceptoImpuestosRetencion()
            {
                Impuesto = "002",
                TipoFactor = (line.VatPercentage == 0 ? "Exento" : "Tasa"),
                TasaOCuota = (line.VatPercentage != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8)) : 0),
                Importe = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2)),
            };

            if (lineVatType.Code != "EXMPT")
            {
                retencion.Base = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0)));//retencion.Importe;
            }

            lineRetencions.Add(retencion);
        }

        private void AddSinglePercentageLineTranslados(ARInvoiceLinePM line, List<ComprobanteConceptoImpuestosTraslado> lineTranslados, VatType lineVatType)
        {
            ComprobanteConceptoImpuestosTraslado traslado = new ComprobanteConceptoImpuestosTraslado()
            {
                Base = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0))),
                Impuesto = "002",
                TipoFactor = (line.VatPercentage == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),
            };

            if (traslado.TipoFactor == "Tasa")
            {
                BuildSingleLineTrasladoTasaTipoFactor(line, traslado);
            }

            lineTranslados.Add(traslado);
        }

        private void BuildSingleLineTrasladoTasaTipoFactor(ARInvoiceLinePM line, ComprobanteConceptoImpuestosTraslado traslado)
        {
            traslado.TasaOCuota = line.VatPercentage != 0 ? (line.VatPercentage != null ? StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8) : "") : "0.000000";
            traslado.Importe = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2));
            traslado.ImporteSpecified = true;
            traslado.TasaOCuotaSpecified = true;
        }

        private void BuildConceptoImpuestos(ComprobanteConcepto concepto, List<ComprobanteConceptoImpuestosTraslado> lineTranslados, List<ComprobanteConceptoImpuestosRetencion> lineRetencions)
        {
            concepto.Impuestos = new ComprobanteConceptoImpuestos();

            if (lineTranslados.Count > 0)
            {
                concepto.Impuestos.Traslados = lineTranslados.ToArray();
            }
            if (lineRetencions.Count > 0)
            {
                concepto.Impuestos.Retenciones = lineRetencions.ToArray();
            }
        }

        private ComprobanteCfdiRelacionados[] GetRelatedInvoiceTag()
        {
            ARInvoice relatedInvoice = GetRelatedInvoice();

            string relatedInvoiceUUID = GetRelatedInvoiceUUID(relatedInvoice);
            return GetCfdiRelacionados(relatedInvoice, relatedInvoiceUUID);
        }

        private ARInvoice GetRelatedInvoice()
        {
            ARInvoice relatedInvoice = null;
            if (arInvoicePM.IsAutoCredit)
            {
                relatedInvoice = (from a in invoiceCotnext.ARInvoices
                                  where a.Id == arInvoicePM.CreditedByARInvoiceId && a.Tenant == arInvoicePM.Tenant
                                  select a).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(arInvoicePM.RelatedInvoice))
            {
                relatedInvoice = (from a in invoiceCotnext.ARInvoices
                                  where a.InvoiceNumber == arInvoicePM.RelatedInvoice && a.Tenant == arInvoicePM.Tenant
                                  select a).FirstOrDefault();
            }

            return relatedInvoice;
        }

        private static string GetRelatedInvoiceUUID(ARInvoice relatedInvoice)
        {
            if (relatedInvoice == null || (relatedInvoice != null && string.IsNullOrEmpty(relatedInvoice.SATXML))) return "";

            XmlElement[] complementoXmlElement = GetComplementoXmlElementFromRelatedInvoice(relatedInvoice.SATXML);

            if (complementoXmlElement == null) return "";

            List<System.Xml.XmlElement> myLXmlComplementos = complementoXmlElement.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return "";

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            return digitalTi.UUID.Trim();
        }

        private static XmlElement[] GetComplementoXmlElementFromRelatedInvoice(string relatedInvoiceSATXML)
        {
            try
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Comprobante>(relatedInvoiceSATXML).Complemento?.Any;
            }
            catch (Exception ex)
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(relatedInvoiceSATXML).Complemento?.Any;
            }
        }

        private ComprobanteCfdiRelacionados[] GetCfdiRelacionados(ARInvoice relatedInvoice, string relatedInvoiceUUID)
        {
            if (string.IsNullOrEmpty(relatedInvoiceUUID)) return null;

            string tipoRelacion = GetTipoRelacion(relatedInvoice);
            ComprobanteCfdiRelacionados[] comprobanteCfdiRelacionados = new ComprobanteCfdiRelacionados[1];
            comprobanteCfdiRelacionados[0] = new ComprobanteCfdiRelacionados
            {
                TipoRelacion = tipoRelacion,
                CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>() { new ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = relatedInvoiceUUID } }.ToArray()
            };

            return comprobanteCfdiRelacionados;
        }

        private string GetTipoRelacion(ARInvoice relatedInvoice)
        {
            if (arInvoicePM.ARInvoiceTypeCode == "CD") return "01";
            else if (IsCanceledInvoiceFromSAT(relatedInvoice)) return "04";

            List<ARInvoice> shipmentInvoices = GetShipmentInvoicesByEntityIdAndARInvoiceId();

            if (shipmentInvoices == null) return "";
            if (shipmentInvoices.Any(a => a.IsAutoCredit)) return "04";
            else if (shipmentInvoices.Count >= 1) return "02";

            return "";
        }

        private static bool IsCanceledInvoiceFromSAT(ARInvoice relatedInvoice)
        {
            const string cancelWithErrorOperationCode = "01";
            return relatedInvoice != null && relatedInvoice.StatusCode == SATData.VoidedInvoiceStatusCode && (relatedInvoice.SATTransferStatusCode == SATData.CanceledSATTransferStatusCode || relatedInvoice.SATTransferStatusCode == SATData.TransferedSATTransferStatusCode || (relatedInvoice.SATTransferStatusCode == SATData.NotTransferedSATTransferStatusCode && relatedInvoice.SATCancelReasonCode == cancelWithErrorOperationCode));
        }

        private List<ARInvoice> GetShipmentInvoicesByEntityIdAndARInvoiceId()
        {
            return (from a in invoiceCotnext.ARInvoiceEntities
                    where a.EntityId == arInvoicePM.MainEntityId && a.ARInvoiceId != arInvoicePM.Id && a.Tenant == arInvoicePM.Tenant
                    select a.ARInvoice).ToList();
        }

        public List<ARInvoiceTotalVATPM> GetARInvoiceTotalVATPMs()
        {
            return CalculateTotalVats();
        }

        private List<ARInvoiceTotalVATPM> CalculateTotalVats()
        {
            List<ARInvoiceTotalVATPM> totalVats = new List<ARInvoiceTotalVATPM>();
            List<ARInvoiceLinePM> myDataLines = arInvoicePM.InvoiceLines.Where(d => !IsExpenseLineWithoutPayableVendor(d) && d.VatTypeId != null).ToList();
            if (myDataLines.Count <= 0) return totalVats;


            List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

            foreach (ARInvoiceLinePM item in myDataLines)
            {
                AddLineVatTypePercentage(allVatTypes, allVatPercentages, allVatGroups, group_Source, item);
            }
            List<InvoiceTotalsClass> group_data = GetInvoiceTotalsClassGroupData(group_Source);

            double? localAmountTotal = 0;
            double? invoiceAmountTotal = 0;
            double? profitAmountTotal = 0;
            double? invoiceVatableAmountTotal = 0;
            foreach (InvoiceTotalsClass item in group_data)
            {
                ARInvoiceTotalVATPM record = new ARInvoiceTotalVATPM()
                {

                    Tenant = tenant,
                    ARInvoiceId = arInvoicePM.Id,
                    VatTypeId = item.Id,
                    VATPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                    LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                    InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                    ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                    ExternalVATCard = item.ExternalVatCard,
                    ExternalTAXItemId = item.ExternalTAXItemId,
                };

                record.LocalVATAmount = MethodHelper.Round((record.LocalVatableAmount * record.VATPercent / 100), 2);
                record.InvoiceCurrencyVATAmount = MethodHelper.Round((record.InvoiceCurrencyVatableAmount * record.VATPercent / 100), 2);
                record.ProfitCurrencyVATAmount = MethodHelper.Round((record.ProfitVatableAmount * record.VATPercent / 100), 2);
                totalVats.Add(record);

                localAmountTotal += record.LocalVATAmount;
                invoiceAmountTotal += record.InvoiceCurrencyVATAmount;
                profitAmountTotal += record.ProfitCurrencyVATAmount;

                invoiceVatableAmountTotal += record.InvoiceCurrencyVatableAmount;
            }

            var subtotal = invoiceVatableAmountTotal;
            var total = invoiceVatableAmountTotal + invoiceAmountTotal;

            return totalVats;
        }

        private void AddLineVatTypePercentage(List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item)
        {
            VatType lineVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

            if (lineVatType != null)
            {
                if (!lineVatType.IsMultiPercentage)
                {
                    AddSingleLineVatTypePercentageInvoiceTotalsClass(group_Source, item, lineVatType);
                }
                else
                {
                    AddMultLineVatTypePercentageInvoiceTotalsClass(allVatTypes, allVatPercentages, allVatGroups, group_Source, item);
                }
            }
        }

        private void AddMultLineVatTypePercentageInvoiceTotalsClass(List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item)
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
                    newItem.ExternalVatCard = vatType.ReceivablesExternalId;
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

        private void AddSingleLineVatTypePercentageInvoiceTotalsClass(List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item, VatType lineVatType)
        {
            InvoiceTotalsClass newItem = new InvoiceTotalsClass()
            {
                Id = item.VatTypeId,
                VatTypeId = item.VatTypeId,
                VatTypePercentage = item.VatPercentage,
                LocalCurrencyAmount = item.LocalCurrencyAmount,
                InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                ExternalVatCard = lineVatType.ReceivablesExternalId,
                ExternalTAXItemId = lineVatType.ExternalTAXItemId,
            };

            group_Source.Add(newItem);
        }

        private List<InvoiceTotalsClass> GetInvoiceTotalsClassGroupData(List<InvoiceTotalsClass> group_Source)
        {
            return (from items in group_Source
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
        }

        public ComprobanteTotalAndSubTotal GetComprobanteTotalAndSubTotal(List<ARInvoiceTotalVATPM> arTotalVats)
        {
            double? localAmountTotal = 0;
            double? invoiceAmountTotal = 0;
            double? profitAmountTotal = 0;
            double? invoiceVatableAmountTotal = 0;
            foreach (ARInvoiceTotalVATPM record in arTotalVats)
            {
                localAmountTotal += record.LocalVATAmount;
                invoiceAmountTotal += record.InvoiceCurrencyVATAmount;
                profitAmountTotal += record.ProfitCurrencyVATAmount;
                invoiceVatableAmountTotal += record.InvoiceCurrencyVatableAmount;
            }

            double subtotal = 0;
            List<ARInvoiceLinePM> myDataLines = arInvoicePM.InvoiceLines.Where(d => !IsExpenseLineWithoutPayableVendor(d) && d.VatTypeId != null).ToList();
            foreach (var line in myDataLines)
            {
                subtotal += (line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount.Value : 0);
            }

            var total = Math.Abs((subtotal + invoiceAmountTotal).Value);
            return new ComprobanteTotalAndSubTotal
            {
                SubTotal = Math.Abs((decimal)subtotal),
                Total = Math.Abs((decimal)total)
            };
        }

        public ComprobanteImpuestosResults GetComprobanteImpuestos(List<ARInvoiceTotalVATPM> arTotalVats, ComprobanteConcepto[] comprobanteConceptos)
        {
            decimal TotalImpuestosRetenidos = 0;
            decimal TotalImpuestosTrasladados = 0;
            List<ComprobanteImpuestosTraslado> trasladoList = new List<ComprobanteImpuestosTraslado>();
            List<ComprobanteImpuestosRetencion> retencionList = new List<ComprobanteImpuestosRetencion>();
            List<ComprobanteImpuestosRetencionDR> retencionDRList = new List<ComprobanteImpuestosRetencionDR>();


            foreach (ARInvoiceTotalVATPM totalVat in arTotalVats)
            {
                var totalVatVatType = allVatTypes.FirstOrDefault(d => d.Id == totalVat.VatTypeId);
                string _totaltipoFactor = (totalVat.VATPercent == 0 && totalVatVatType.Code == "EXMPT" ? "Exento" : "Tasa");
                string total_tasaOCuota = totalVat.VATPercent != 0 ? (totalVat.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(totalVat.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";

                if (totalVat.VATPercent >= 0)
                {

                    TotalImpuestosTrasladados += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
                     ComprobanteImpuestosTraslado traslado = trasladoList.FirstOrDefault(t => t.TipoFactor == _totaltipoFactor);
                    if (traslado == null
                        || (traslado != null && (totalVat.VATPercent != 0 && traslado.TasaOCuota == "0.000000")
                        || (totalVat.VATPercent == 0 && traslado.TasaOCuota != "0.000000")))
                    {
                        traslado = GetNewComprobanteImpuestosTrasladoInstance(totalVat, _totaltipoFactor, total_tasaOCuota);

                        trasladoList.Add(traslado);
                    }
                    else
                    {
                        if (traslado.TipoFactor == "Tasa")
                        {
                            MapImpuestosTraslado(TotalImpuestosTrasladados, totalVat, traslado);
                        }
                    }
                }
                else
                {
                    TotalImpuestosRetenidos += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0));
                    ComprobanteImpuestosRetencion retencion = retencionList.FirstOrDefault();
                    ComprobanteImpuestosRetencionDR retencionDR = retencionDRList.FirstOrDefault();
                    if (retencion == null)
                    {
                        retencion = new ComprobanteImpuestosRetencion()
                        {
                            Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0)), invoiceCurrencyCode),
                            Impuesto = "002",
                        };

                        retencionDR = new ComprobanteImpuestosRetencionDR()
                        {
                            Impuesto = "002",
                            Base = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVatableAmount != null ? (decimal)totalVat.InvoiceCurrencyVatableAmount.Value : 0)), invoiceCurrencyCode),
                            TipoFactor = _totaltipoFactor
                        };

                        if (_totaltipoFactor != "Exento")
                        {
                            retencionDR.TasaOCuota = total_tasaOCuota;
                            retencionDR.Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0)), invoiceCurrencyCode);
                        }
                        retencionDRList.Add(retencionDR);
                        retencionList.Add(retencion);
                    }
                    else
                    {
                        retencion.Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(TotalImpuestosRetenidos, invoiceCurrencyCode);
                        if(_totaltipoFactor != "Exento") retencionDR.Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(TotalImpuestosRetenidos, invoiceCurrencyCode);
                    }
                }
            }

            decimal totalTraslados = 0;
            decimal totalRetenciones = 0;
            foreach (ComprobanteConcepto comprobanteConcepto in comprobanteConceptos)
            {
                if (comprobanteConcepto.Impuestos != null)
                {
                    if (comprobanteConcepto.Impuestos.Traslados != null)
                    {
                        foreach (var t in comprobanteConcepto.Impuestos.Traslados)
                        {
                            totalTraslados += t.Importe;
                        }
                    }
                    if (comprobanteConcepto.Impuestos.Retenciones != null)
                    {
                        foreach (var r in comprobanteConcepto.Impuestos.Retenciones)
                        {
                            totalRetenciones += r.Importe;
                        }
                    }
                }
            }

            decimal roundedTotalTraslados = (decimal)MethodHelper.Roundd((double)totalTraslados, 2);
            decimal roundedTotalRetenciones = (decimal)MethodHelper.Roundd((double)totalRetenciones, 2);

            if ((trasladoList.Count > 0) || retencionList.Count > 0 || retencionDRList.Count > 0)
            {
                ComprobanteImpuestos comprobanteImpuestos = new ComprobanteImpuestos();

                if (trasladoList.Count > 0)
                {
                    comprobanteImpuestos.Traslados = trasladoList.ToArray();
                    comprobanteImpuestos.TotalImpuestosTrasladados = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalTraslados, invoiceCurrencyCode);//Math.Abs(TotalImpuestosTrasladados);
                    comprobanteImpuestos.TotalImpuestosTrasladadosSpecified = IsTotalImpuestosTrasladadosSpecified(comprobanteImpuestos);

                    if (comprobanteImpuestos.Traslados.Where(t => t.TipoFactor == "Tasa").Any())
                    {
                        ComprobanteImpuestosTraslado traslado = comprobanteImpuestos.Traslados.Where(t => t.TipoFactor == "Tasa").FirstOrDefault();
                        if (comprobanteImpuestos.Traslados.Count() > 1)
                        {
                            traslado = comprobanteImpuestos.Traslados.Where(t => t.TipoFactor == "Tasa" && t.TasaOCuota != "0.000000").FirstOrDefault();
                        }


                        if (traslado != null && (traslado.Importe != SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalTraslados, invoiceCurrencyCode)))
                        {
                            correctedARInvoiceTrasladoLines = CorrectARInvoiceLinePM(new CorrectARInvoiceLineVatAmountsArgs
                            {
                                LogitudeVatAmount = traslado.Importe,
                                SatVatAmount = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalTraslados, invoiceCurrencyCode),
                                TaxPercentageAmount = traslado.TasaOCuota
                            });
                        }
                        //traslado.Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalTraslados, invoiceCurrencyCode);
                    }

                    if (comprobanteImpuestos.Traslados.Where(t => t.TipoFactor == "Exento").Any())
                    {
                        comprobanteImpuestos.Traslados.Where(t => t.TipoFactor == "Exento").FirstOrDefault().Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(0, invoiceCurrencyCode);
                    }

                }

                if (retencionList.Count > 0)
                {
                    comprobanteImpuestos.Retenciones = retencionList.ToArray();
                    comprobanteImpuestos.TotalImpuestosRetenidos = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode);
                    comprobanteImpuestos.TotalImpuestosRetenidosSpecified = true;

                    ComprobanteImpuestosRetencion retencion = comprobanteImpuestos.Retenciones[0];
                    if (retencion.Importe != SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode))
                    {
                        ComprobanteConcepto concepto = comprobanteConceptos.Where(c => c.Impuestos.Retenciones != null && c.Impuestos.Retenciones.Count() > 0).FirstOrDefault();
                        if (concepto != null)
                        {
                            ComprobanteConceptoImpuestosRetencion conceptoRetencion = concepto.Impuestos.Retenciones.FirstOrDefault();
                            if (conceptoRetencion != null)
                            {
                                correctedARInvoiceRetencionLines = CorrectARInvoiceLinePM(new CorrectARInvoiceLineVatAmountsArgs
                                {
                                    LogitudeVatAmount = retencion.Importe,
                                    SatVatAmount = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode),
                                    TaxPercentageAmount = conceptoRetencion.TasaOCuota.ToString(),
                                    IsNegativeTax = true
                                });
                            }
                        }
                    }

                    comprobanteImpuestos.Retenciones[0].Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode);
                }

                if (retencionDRList.Count > 0)
                {
                    comprobanteImpuestos.TotalImpuestosRetenidos = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode);
                    comprobanteImpuestos.TotalImpuestosRetenidosSpecified = true;

                    ComprobanteImpuestosRetencionDR retencion = retencionDRList[0];
                    if (retencion.Importe != SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode))
                    {
                        ComprobanteConcepto concepto = comprobanteConceptos.Where(c => c.Impuestos.Retenciones != null && c.Impuestos.Retenciones.Count() > 0).FirstOrDefault();
                        if (concepto != null)
                        {
                            ComprobanteConceptoImpuestosRetencion conceptoRetencion = concepto.Impuestos.Retenciones.FirstOrDefault();
                            if (conceptoRetencion != null)
                            {
                                correctedARInvoiceRetencionDRLines = CorrectARInvoiceLinePM(new CorrectARInvoiceLineVatAmountsArgs
                                {
                                    LogitudeVatAmount = retencion.Importe,
                                    SatVatAmount = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode),
                                    TaxPercentageAmount = conceptoRetencion.TasaOCuota.ToString(),
                                    IsNegativeTax = true
                                });
                            }
                        }
                    }
                    
                    retencionDRList[0].Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(totalRetenciones, invoiceCurrencyCode);
                }

                return new ComprobanteImpuestosResults
                {
                    ComprobanteImpuestos = comprobanteImpuestos,
                    ComprobanteImpuestosRetencionDRs = retencionDRList
                };
            }

            return null;
        }

        private List<ARInvoiceLinePM> CorrectARInvoiceLinePM(CorrectARInvoiceLineVatAmountsArgs correctARInvoiceLineVatAmountsArgs)
        {
            return new SATInvoiceDifferenceVATCalculationService().CorrectARInvoiceLinesPM(new SATInvoiceDifferenceVATCalculationServiceArgs
            {
                SATVatAmount = correctARInvoiceLineVatAmountsArgs.SatVatAmount,
                LogitudeVatAmount = correctARInvoiceLineVatAmountsArgs.LogitudeVatAmount,
                TasaOCuota = correctARInvoiceLineVatAmountsArgs.TaxPercentageAmount,
                IsNegativeTax = correctARInvoiceLineVatAmountsArgs.IsNegativeTax,
                AllExpenseShipmentReceivables = allExpenseShipmentReceivables,
                ARInvoicePM = arInvoicePM,
                CorrectARInvoiceLinesVatAmount = CorrectARInvoiceLinesVATAmount
            });
        }

        private bool IsTotalImpuestosTrasladadosSpecified(ComprobanteImpuestos comprobanteImpuestos)
        {
            int exentoFactorTrasladosCount = comprobanteImpuestos.Traslados.Where(Traslado => Traslado.TipoFactor == "Exento").Count();
            bool HasExentoFactorOnly = exentoFactorTrasladosCount > 0 && (exentoFactorTrasladosCount == comprobanteImpuestos.Traslados.Count());
            if (HasExentoFactorOnly)
            {
                return comprobanteImpuestos.TotalImpuestosTrasladados != 0;
            }
            return true;
        }

        private void MapImpuestosTraslado(decimal TotalImpuestosTrasladados, ARInvoiceTotalVATPM totalVat, ComprobanteImpuestosTraslado traslado)
        {
            traslado.Importe = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(TotalImpuestosTrasladados, invoiceCurrencyCode);
            traslado.ImporteSpecified = true;
            traslado.Base += SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVatableAmount != null ? (decimal)totalVat.InvoiceCurrencyVatableAmount.Value : 0)), invoiceCurrencyCode);
            traslado.Base = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(traslado.Base, invoiceCurrencyCode);
        }

        private ComprobanteImpuestosTraslado GetNewComprobanteImpuestosTrasladoInstance(ARInvoiceTotalVATPM totalVat, string _totaltipoFactor, string total_tasaOCuota)
        {
            ComprobanteImpuestosTraslado traslado;
            traslado = new ComprobanteImpuestosTraslado()
            {
                Impuesto = "002",
                Base = SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVatableAmount != null ? (decimal)totalVat.InvoiceCurrencyVatableAmount.Value : 0)), invoiceCurrencyCode),
                TipoFactor = _totaltipoFactor,
            };
            if (totalVat.VatTypeCode != "EXMPT" && _totaltipoFactor != "Exento")
            {
                traslado.Importe = GetNewComprobanteImpuestosImporteValue(totalVat, total_tasaOCuota);
                traslado.TasaOCuota = total_tasaOCuota;
                traslado.ImporteSpecified = true;
                traslado.TasaOCuotaSpecified = true;
            }

            return traslado;
        }

        private decimal GetNewComprobanteImpuestosImporteValue(ARInvoiceTotalVATPM totalVat, string tasaOCuota)
        {
            decimal importeValue = Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
            importeValue = tasaOCuota == "0.000000" ? 0 : importeValue;
            return SATBaseProfact40Service.GetDecimalWithMatchCurrencyDigitsAfterPoint(importeValue, invoiceCurrencyCode);
        }
    }

    public class ComprobanteImpuestosRetencionDR
    {
        public string Impuesto { get; set; }
        public decimal Base { get; set; }
        public string TipoFactor { get; set; }
        public decimal Importe { get; set; }
        public string TasaOCuota { get; set; }
    }

    public class ComprobanteImpuestosResults
    {
        public ComprobanteImpuestos ComprobanteImpuestos { get; set; }
        public List<ComprobanteImpuestosRetencionDR> ComprobanteImpuestosRetencionDRs { get; set; }
    }

    public class ComprobanteTotalAndSubTotal
    {
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class CorrectARInvoiceLineVatAmountsArgs
    {
        public decimal LogitudeVatAmount { get; set; }
        public decimal SatVatAmount { get; set; }
        public string TaxPercentageAmount { get; set; }
        public bool IsNegativeTax { get; set; }
    }
}
