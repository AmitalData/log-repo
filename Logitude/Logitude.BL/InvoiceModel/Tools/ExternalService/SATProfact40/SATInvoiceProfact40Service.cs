using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATInvoiceProfact40Service
    {
        private ARInvoicePM arInvoicePM;
        private ARInvoice arInvoice;
        private SATInterfaceSetting satSetting;
        private ICommonDataContext commonContext;
        private IInvoiceContext invoiceCotnext;
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        private TenantRepository tenantRepository;
        private AddressRepository addressReposirory;
        private CardRepository cardRepository;
        private BranchRepository branchRepository;
        private CurrencyRepository currencyRepository;
        private MeasurementRepository measurementRepository;
        private ChargesTypeRepository chargesTypeRepository;
        private PaymentTermRepository paymentTermRepository;
        private SATBaseProfact40Service sATBaseProfact40Service;
        
        public SATInvoiceProfact40Service(ARInvoicePM arInvoicePM, ARInvoice arInvoice, SATInterfaceSetting satSetting)
        {
            this.arInvoicePM = arInvoicePM;
            this.arInvoice = arInvoice;
            this.satSetting = satSetting;
            InitalizeContexts(arInvoicePM);
            InitalizeRepositories(arInvoicePM);
            InitalizeServices();
        }

        private void InitalizeContexts(ARInvoicePM arInvoicePM)
        {
            commonContext = CommonDataContext.GetContext(arInvoicePM.Tenant);
            invoiceCotnext = InvoiceContext.GetContext(arInvoicePM.Tenant);
        }

        private void InitalizeRepositories(ARInvoicePM arInvoicePM)
        {
            computingPartnerHelper = new ComputingPartnerTranslationHelper(arInvoicePM.Tenant);
            tenantRepository = new TenantRepository(commonContext);
            addressReposirory = new AddressRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            branchRepository = new BranchRepository(commonContext);
            currencyRepository = new CurrencyRepository(commonContext);
            measurementRepository = new MeasurementRepository(commonContext);
            chargesTypeRepository = new ChargesTypeRepository(commonContext);
            paymentTermRepository = new PaymentTermRepository(commonContext);
        }

        public void InitalizeServices()
        {
            sATBaseProfact40Service = new SATBaseProfact40Service(arInvoicePM.Tenant);
        }

        public void SendProfactoXML()
        {
            ARInvoiceTotalVATQuery aRInvoiceTotalVATQuery = new ARInvoiceTotalVATQuery(arInvoicePM.Tenant);

            List<ChargesType> allChargesTypes = chargesTypeRepository.GetChargesTypes(arInvoicePM.Tenant).ToList();
            List<Measurement> allMeasurements = measurementRepository.GetMeasurements(arInvoicePM.Tenant).ToList();

            if (arInvoicePM.InvoiceLines.All(l => allChargesTypes.First(c => c.Id == l.ChargesTypeId).IsExpense == true))//l.IsExpense == true &&
            {
                arInvoice.SATTransferStatusCode = arInvoicePM.SATTransferStatusCode = "ND";
                return;
            }

            List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(commonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);


            List<VatType> allVatTypes = commonContext.VatTypes.Where(d => d.Tenant == arInvoicePM.Tenant).ToList();
            List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(arInvoicePM.Tenant, TenantServerConfigration.GetCurrentDateTime(arInvoicePM.Tenant).Date);
            List<VATTypesGroup> allVatGroups = (from d in commonContext.VATTypesGroups
                                                where d.Tenant == arInvoicePM.Tenant
                                                select d).ToList();

            Currency invoiceCurrency = currencyRepository.GetSingleCurrency(arInvoicePM.InvoiceCurrencyId, arInvoicePM.Tenant);
            Address branchAddress = null;
            Address mainAddress = null;
            Tenant currentTenant = tenantRepository.GetSingleTenant(arInvoicePM.Tenant);
            PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTerm(arInvoicePM.PaymentTermId);

            if (!string.IsNullOrEmpty(arInvoicePM.BranchId))
            {
                Branch branch = branchRepository.GetSingleBranch(arInvoicePM.BranchId, arInvoicePM.Tenant);
                if (!string.IsNullOrEmpty(branch.AddressId))
                {
                    branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
                }
            }

            Address billToAddress = GetBillToAddress();

            Card billToCard = cardRepository.GetSingleCard(arInvoicePM.BillToId, arInvoicePM.Tenant);

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();

            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }

            if (string.IsNullOrEmpty(currentTenant.Company))
            {
                throw new ApplicationException("Company Name is required");
            }

            if (arInvoicePM.InvoiceDate == null)
            {
                throw new ApplicationException("Invoice Date is required");
            }

            if (string.IsNullOrEmpty(arInvoicePM.SATPaymentMethodCode))
            {
                throw new ApplicationException("Forma Pago is required");
            }

            if (branchAddress != null)
            {

                if (string.IsNullOrEmpty(branchAddress.ZipCode))
                {
                    throw new ApplicationException("Branch Address ZipCode is required ");
                }

                mainAddress = branchAddress;
            }
            else
            {
                if (currentTenant.Address != null)
                {
                    if (string.IsNullOrEmpty(currentTenant.Address.ZipCode))
                    {
                        throw new ApplicationException("Company Address ZipCode is required ");
                    }

                    mainAddress = currentTenant.Address;
                }
                else
                    throw new ApplicationException("Company Address is required ");
            }

            if (string.IsNullOrEmpty(arInvoicePM.MetodoPagoCode))
            {
                if (!string.IsNullOrEmpty(billToCard.MetodoPagoCode))
                    arInvoicePM.MetodoPagoCode = billToCard.MetodoPagoCode;
                else
                    arInvoicePM.MetodoPagoCode = satSetting.MetodoPagoCode;
            }

            if (string.IsNullOrEmpty(arInvoicePM.MetodoPagoCode))
            {
                throw new ApplicationException("Metodo Pago is required ");
            }

            if (string.IsNullOrEmpty(billToCard.EnglishName))
            {
                throw new ApplicationException("Bill to Name is required");
            }
            string serie = "A";
            string folio = arInvoicePM.InvoiceNumber;

            string counterPrefix = null;
            if (arInvoicePM.IsConstituentInvoice)
            {
                counterPrefix = TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "CNST", "CNS", null);
            }
            else if (arInvoicePM.IsConsolidationInvoice)
            {
                counterPrefix = TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "INVC", "CON", null);
            }
            else
            {
                counterPrefix = TableCounter.GetCounterPrefix(arInvoicePM.Tenant, "INVC", arInvoicePM.ARInvoiceTypeCode, null);
            }

            if (!String.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;

                if (!string.IsNullOrEmpty(arInvoicePM.InvoiceNumber))
                {
                    if (arInvoicePM.InvoiceNumber.Contains(counterPrefix))
                    {
                        folio = arInvoicePM.InvoiceNumber.Remove(0, counterPrefix.Length);
                    }
                }
            }

            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == arInvoicePM.SATPaymentMethodCode).FirstOrDefault();
            Profact.TimbraCFDI40.Comprobante comprobante = new Profact.TimbraCFDI40.Comprobante();

            //Llenamos datos del comprobante
            if (invoiceCurrency.Code != "MXN")
            {
                comprobante.TipoCambio = (arInvoicePM.InvoiceCurrencyExchangeRate != null ? Convert.ToDecimal(arInvoicePM.InvoiceCurrencyExchangeRate.Value) : 0);
                comprobante.TipoCambioSpecified = true;
            }

            MapInformacionGlobal(comprobante);
            comprobante.Exportacion = "01";
            comprobante.Moneda = invoiceCurrency.Code;
            comprobante.Serie = serie;
            comprobante.Version = "4.0";
            comprobante.Folio = folio;
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(arInvoicePM.Tenant);
            // var invoiceDate = entityPM.InvoiceDate.Value.ToUniversalTime();
            comprobante.Fecha = arInvoicePM.InvoiceDate.Value;//entityPM.InvoiceDate != null ? entityPM.InvoiceDate.Value : currentDateTime;c
            comprobante.Fecha = new DateTime(comprobante.Fecha.Year, comprobante.Fecha.Month, comprobante.Fecha.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            //comprobante.Fecha = comprobante.Fecha.ToUniversalTime();
            //comprobante.formaDePago = "una sola exhibición";

            comprobante.FormaPago = satPaymentMethod.Code;
            comprobante.FormaPagoSpecified = true;

            comprobante.MetodoPago = arInvoicePM.MetodoPagoCode;//"PUE";
            comprobante.MetodoPagoSpecified = true;

            comprobante.SubTotal = Math.Abs((arInvoicePM.SubTotalInInvoiceCurrency != null ? (decimal)arInvoicePM.SubTotalInInvoiceCurrency.Value : 0));
            comprobante.Total = Math.Abs((arInvoicePM.AmountInInvoiceCurrency != null ? (decimal)arInvoicePM.AmountInInvoiceCurrency.Value : 0));


            //Llenamos datos del emisor
            comprobante.Emisor = new Profact.TimbraCFDI40.ComprobanteEmisor();
            comprobante.Emisor.Rfc = currentTenant.VatNumber;
            comprobante.Emisor.Nombre = currentTenant.Company;
            comprobante.Emisor.RegimenFiscal = "601";

            MapReceptor(comprobante, billToCard, billToAddress);
            
            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            if (billToCountryCode == "MEX" || billToCountryCode == "MX")
            {
                if (!string.IsNullOrEmpty(billToCard.VatNumber))
                    comprobante.Receptor.Rfc = billToCard.VatNumber;
                else
                    comprobante.Receptor.Rfc = "AAA010101AAA";
            }
            else
            {
                if (!string.IsNullOrEmpty(billToCard.SATForeignRFC))
                    comprobante.Receptor.Rfc = billToCard.SATForeignRFC;
                else
                    comprobante.Receptor.Rfc = "XEXX010101000";
            }

            //comprobante.Receptor.ResidenciaFiscal = billToCountryCode;
            //comprobante.Receptor.ResidenciaFiscalSpecified = true;
            //Llena domicilio del receptor

            //comprobante.TipoDeComprobante = "I";
            if (!string.IsNullOrEmpty(arInvoicePM.UsoCFDICode))
            {
                comprobante.Receptor.UsoCFDI = arInvoicePM.UsoCFDICode;
            }
            else
            {

                if (arInvoicePM.ARInvoiceTypeCode == "CD")
                {
                    comprobante.Receptor.UsoCFDI = "G02";
                }
                else
                {
                    comprobante.Receptor.UsoCFDI = "G03";
                }
            }

            if (arInvoicePM.ARInvoiceTypeCode == "CD")
            {
                comprobante.TipoDeComprobante = "E";
            }
            else
            {
                comprobante.TipoDeComprobante = "I";
            }
            if (paymentTerm.LocalName != null)
            {
                comprobante.CondicionesDePago = paymentTerm.LocalName;
            }
            else
            {
                comprobante.CondicionesDePago = paymentTerm.EnglishName;
            }
            BuildRelatedInvoiceTag(comprobante);

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                comprobante.LugarExpedicion = branchAddress.ZipCode;//(mainAddress.City != null ? mainAddress.City : "") + ", " + (mainAddress.State != null ? mainAddress.State.EnglishName : "Estado expedido en");//"Mexico, Distrito Federal";
            }
            else
                comprobante.LugarExpedicion = currentTenant.Address.ZipCode;



            //Llenamos los conceptos

            decimal TotalImpuestosRetenidos = 0;
            decimal TotalImpuestosTrasladados = 0;
            bool hasExpenses = arInvoicePM.InvoiceLines.Any(l => allChargesTypes.First(c => c.Id == l.ChargesTypeId).IsExpense);
            List<Profact.TimbraCFDI40.ComprobanteConcepto> conceptosList = new List<Profact.TimbraCFDI40.ComprobanteConcepto>();
            foreach (ARInvoiceLinePM line in arInvoicePM.InvoiceLines)
            {
                if (!allChargesTypes.First(c => c.Id == line.ChargesTypeId).IsExpense)
                {
                    Profact.TimbraCFDI40.ComprobanteConcepto concepto = new Profact.TimbraCFDI40.ComprobanteConcepto();
                    concepto.ObjetoImp = "02";
                    concepto.Cantidad = Math.Abs((line.Quantity != null ? ((decimal)line.Quantity.Value) : 0));
                    concepto.Unidad = "SERVICIO";
                    //concepto.noIdentificacion = "1";
                    concepto.Descripcion = line.Description;
                    concepto.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? ((decimal)line.InvoiceCurrencyAmount.Value) : 0)));
                    decimal valorUnitario = Math.Abs(concepto.Cantidad != 0 ? (concepto.Importe / concepto.Cantidad) : 0);
                    concepto.ValorUnitario = sATBaseProfact40Service.GetDecimalWith3DigitsAfterPointIfZero(Math.Abs(Math.Truncate(valorUnitario * 1000000m) / 1000000m));

                    concepto.ClaveProdServ = allChargesTypes.FirstOrDefault(c => c.Id == line.ChargesTypeId).SATExternalId;
                    var lineMeasurement = allMeasurements.FirstOrDefault(m => m.Id == line.MeasurementId);
                    if (lineMeasurement != null)
                        concepto.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(lineMeasurement.Code, "G-Profact", "Measurement");//"C81";
                                                                                                                                                           //line.mea
                    if (string.IsNullOrEmpty(concepto.ClaveProdServ))
                    {
                        throw new Exception("Measurement on charge type is required");
                    }

                    if (string.IsNullOrEmpty(concepto.ClaveProdServ))
                    {
                        throw new Exception("SAT External Id on charge type is required");
                    }

                    this.CalucalteLineTotals(line, concepto, allVatTypes, allVatPercentages, allVatGroups);


                    conceptosList.Add(concepto);
                }
            }

            comprobante.Conceptos = conceptosList.ToArray();

            //Llenamos impuestos
            List<Profact.TimbraCFDI40.ComprobanteImpuestos> impuestosList = new List<Profact.TimbraCFDI40.ComprobanteImpuestos>();
            List<Profact.TimbraCFDI40.ComprobanteImpuestosTraslado> trasladoList = new List<Profact.TimbraCFDI40.ComprobanteImpuestosTraslado>();
            List<Profact.TimbraCFDI40.ComprobanteImpuestosRetencion> retencionList = new List<Profact.TimbraCFDI40.ComprobanteImpuestosRetencion>();
            List<ARInvoiceTotalVATPM> arTotalVats = new List<ARInvoiceTotalVATPM>();
            if (!hasExpenses)
            {
                arTotalVats = aRInvoiceTotalVATQuery.GetTotalVATs(arInvoicePM.Id, arInvoicePM.Tenant).ToList();

            }
            else
            {
                arTotalVats = this.CalculateNoneExpenseTotalVats(arInvoicePM, allVatTypes, allVatPercentages, allVatGroups, allChargesTypes);
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
                //var subtotal = Math.Abs(invoiceVatableAmountTotal.Value);
                //var total = Math.Abs((invoiceVatableAmountTotal + invoiceAmountTotal).Value);
                //comprobante.SubTotal = Math.Abs((decimal)subtotal);
                //comprobante.Total = Math.Abs((decimal)total);

                double subtotal = 0;
                List<ARInvoiceLinePM> myDataLines = arInvoicePM.InvoiceLines.Where(d => !allChargesTypes.First(c => c.Id == d.ChargesTypeId).IsExpense && d.VatTypeId != null).ToList();
                foreach (var line in myDataLines)
                {
                    subtotal += (line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount.Value : 0);
                }

                comprobante.SubTotal = Math.Abs((decimal)subtotal);
                var total = Math.Abs((subtotal + invoiceAmountTotal).Value);
                comprobante.Total = Math.Abs((decimal)total);
                //comprobante.Total = Math.Abs((decimal)total);
            }


            foreach (ARInvoiceTotalVATPM totalVat in arTotalVats)
            {
                if (totalVat.VATPercent >= 0)
                {
                    var totalVatVatType = allVatTypes.FirstOrDefault(d => d.Id == totalVat.VatTypeId);

                    TotalImpuestosTrasladados += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
                    string _totaltipoFactor = (totalVat.VATPercent == 0 && totalVatVatType.Code == "EXMPT" ? "Exento" : "Tasa");
                    string total_tasaOCuota = totalVat.VATPercent != 0 ? (totalVat.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(totalVat.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";
                    Profact.TimbraCFDI40.ComprobanteImpuestosTraslado traslado = trasladoList.FirstOrDefault(t => t.TipoFactor == _totaltipoFactor);//&& (totalVat.VATPercent != 0)
                    if (traslado == null
                        || (traslado != null && (totalVat.VATPercent != 0 && traslado.TasaOCuota == "0.000000")
                        || (totalVat.VATPercent == 0 && traslado.TasaOCuota != "0.000000")))
                    {
                        //if (totalVat.VATPercent != 0)
                        //{
                        if (_totaltipoFactor != "Exento")
                        {
                            traslado = GetNewComprobanteImpuestosTrasladoInstance(totalVat, _totaltipoFactor, total_tasaOCuota);
                            //if (traslado.TipoFactor == "Tasa")
                            //{
                            //    traslado.TasaOCuota = (totalVat.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(totalVat.VATPercent.Value / 100).ToString()), '0', 8) : "");//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                            //    traslado.Importe = Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
                            //    //traslado.ImporteSpecified = true;
                            //    //traslado.TasaOCuotaSpecified = true;
                            //}

                            trasladoList.Add(traslado);
                        }
                        //}
                    }
                    else
                    {
                        if (traslado.TipoFactor == "Tasa")
                        {
                            traslado.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(TotalImpuestosTrasladados);
                        }
                    }


                }
                else
                {
                    TotalImpuestosRetenidos += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0));
                    Profact.TimbraCFDI40.ComprobanteImpuestosRetencion retencion = retencionList.FirstOrDefault();
                    if (retencion == null)
                    {
                        //if (retencionList.Any())
                        //{
                        retencion = new Profact.TimbraCFDI40.ComprobanteImpuestosRetencion()
                        {
                            Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0))),
                            Impuesto = "002",
                        };

                        retencionList.Add(retencion);
                        // }
                    }
                    else
                    {
                        retencion.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(TotalImpuestosRetenidos);
                    }


                }
            }

            decimal totalTraslados = 0;
            decimal totalRetenciones = 0;
            foreach (var c in comprobante.Conceptos)
            {
                if (c.Impuestos != null)
                {
                    if (c.Impuestos.Traslados != null)
                    {
                        foreach (var t in c.Impuestos.Traslados)
                        {
                            totalTraslados += t.Importe;
                        }
                    }
                    if (c.Impuestos.Retenciones != null)
                    {
                        foreach (var r in c.Impuestos.Retenciones)
                        {
                            totalRetenciones += r.Importe;
                        }
                    }
                }
            }

            decimal roundedTotalTraslados = (decimal)MethodHelper.Roundd((double)totalTraslados, 2);
            decimal roundedTotalRetenciones = (decimal)MethodHelper.Roundd((double)totalRetenciones, 2);

            if ((trasladoList.Count > 0) || retencionList.Count > 0)
            {
                comprobante.Impuestos = new Profact.TimbraCFDI40.ComprobanteImpuestos();

                if (trasladoList.Count > 0)
                {
                    comprobante.Impuestos.Traslados = trasladoList.ToArray();
                    comprobante.Impuestos.TotalImpuestosTrasladados = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalTraslados);//Math.Abs(TotalImpuestosTrasladados);
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = true;

                    if (comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").Any())
                    {
                        Profact.TimbraCFDI40.ComprobanteImpuestosTraslado traslado = comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").FirstOrDefault();
                        if (comprobante.Impuestos.Traslados.Count() > 1)
                        {
                            traslado = comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa" && t.TasaOCuota != "0.000000").FirstOrDefault();
                        }

                        if (traslado.Importe != sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalTraslados))
                        {
                            decimal precentage = (decimal.Parse(traslado.TasaOCuota.TrimEnd('0')) * 100);
                            throw new Exception("Due to the SAT Invoice Transmission we calculate the VAT amount per line. There is a difference between the lines VAT sum and the total VAT (" + totalTraslados + ") at the invoice level. You are not allowed to approve the invoice unless you adjust the lines with the following VAT : " + precentage.ToString().TrimEnd('0').TrimEnd('.') + "%");
                        }

                        //comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").FirstOrDefault().Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalTraslados);
                        traslado.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalTraslados);
                    }

                    if (comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Exento").Any())
                    {
                        comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Exento").FirstOrDefault().Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(0);
                    }

                }

                if (retencionList.Count > 0)
                {
                    comprobante.Impuestos.Retenciones = retencionList.ToArray();
                    comprobante.Impuestos.TotalImpuestosRetenidos = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalRetenciones);//Math.Abs(TotalImpuestosRetenidos);
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = true;


                    Profact.TimbraCFDI40.ComprobanteImpuestosRetencion retencion = comprobante.Impuestos.Retenciones[0];
                    if (retencion.Importe != sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalRetenciones))
                    {
                        Profact.TimbraCFDI40.ComprobanteConcepto concepto = comprobante.Conceptos.Where(c => c.Impuestos.Retenciones != null && c.Impuestos.Retenciones.Count() > 0).FirstOrDefault();
                        if (concepto != null)
                        {
                            Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion conceptoRetencion = concepto.Impuestos.Retenciones.FirstOrDefault();
                            if (conceptoRetencion != null)
                            {
                                decimal precentage = conceptoRetencion.TasaOCuota * 100;
                                throw new Exception("Due to the SAT Invoice Transmission we calculate the VAT amount per line. There is a difference between the lines VAT sum and the total VAT at the invoice level. You are not allowed to approve the invoice unless you adjust the lines with the following VAT : " + precentage.ToString().TrimEnd('0').TrimEnd('.') + "%");
                            }
                        }

                    }

                    comprobante.Impuestos.Retenciones[0].Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalRetenciones);

                }

            }

            decimal calculatedTotal = comprobante.SubTotal + totalTraslados - totalRetenciones;
            if (calculatedTotal != comprobante.Total)
            {

            }

            sATBaseProfact40Service.BuildProfactCommunicationLog40(new Profact40CommunicationLogArgs { Comprobante = comprobante, EntityId = arInvoicePM.Id, EntityReference = arInvoicePM.InvoiceNumber.ToString() });

            arInvoice.SATTransferStatusCode = arInvoicePM.SATTransferStatusCode = "TG";

        }

        private Profact.TimbraCFDI40.ComprobanteImpuestosTraslado GetNewComprobanteImpuestosTrasladoInstance(ARInvoiceTotalVATPM totalVat, string _totaltipoFactor, string total_tasaOCuota)
        {
            Profact.TimbraCFDI40.ComprobanteImpuestosTraslado traslado;
            traslado = new Profact.TimbraCFDI40.ComprobanteImpuestosTraslado()
            {
                Impuesto = "002",
                Base = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVatableAmount != null ? (decimal)totalVat.InvoiceCurrencyVatableAmount.Value : 0))),
                TipoFactor = _totaltipoFactor,
            };
            if (totalVat.VatTypeCode != "EXMPT")
            {
                traslado.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0)));
                traslado.TasaOCuota = total_tasaOCuota;
            }

            return traslado;
        }

        private void MapReceptor(Profact.TimbraCFDI40.Comprobante comprobante, Card billToCard, Address billToAddress)
        {
            if (string.IsNullOrEmpty(arInvoicePM.RegimenFiscalCode) && !string.IsNullOrEmpty(billToCard.RegimenFiscalCode))
            {
                arInvoicePM.RegimenFiscalCode = billToCard.RegimenFiscalCode;
            }
            if (string.IsNullOrEmpty(arInvoicePM.RegimenFiscalCode))
            {
                throw new ApplicationException("Regimen Fiscal is required ");
            }

            comprobante.Receptor = GetNewComprobanteReceptorInstance(billToCard, billToAddress);

        }

        private Profact.TimbraCFDI40.ComprobanteReceptor GetNewComprobanteReceptorInstance(Card billToCard, Address billToAddress)
        {
            string billToAddressZipCode = "";
            if (billToAddress != null && string.IsNullOrEmpty(billToAddress.ZipCode))
            {
                billToAddressZipCode = GetBillToAddressZipCode(billToAddress, billToAddressZipCode);
            }

            return new Profact.TimbraCFDI40.ComprobanteReceptor
            {
                Nombre = billToCard.EnglishName,
                RegimenFiscalReceptor = arInvoicePM.RegimenFiscalCode,
                DomicilioFiscalReceptor = billToAddressZipCode
            };
        }

        private string GetBillToAddressZipCode(Address billToAddress, string billToAddressZipCode)
        {
            PostalCodeQuery postalCodeQuery = new PostalCodeQuery(arInvoicePM.Tenant);
            PostalCodePM postalCodePM = postalCodeQuery.GetSinglePM(billToAddress.ZipCode);
            if (postalCodePM == null)
            {
                 throw new ApplicationException("Bill To Address Zip Code is not valid");
            }
            else
            {
                billToAddressZipCode = billToAddress.ZipCode;
            }

            return billToAddressZipCode;
        }

        private Address GetBillToAddress()
        {
            if (!string.IsNullOrEmpty(arInvoicePM.BillToAddressId))
            {
                return addressReposirory.GetSingleAddress(arInvoicePM.BillToAddressId, arInvoicePM.Tenant);
            }
            else
                throw new ApplicationException("Bill to Address is required ");
        }

        private void MapInformacionGlobal(Profact.TimbraCFDI40.Comprobante comprobante)
        {
            if (!arInvoice.IsConsolidationInvoice) return;
            Dictionary<int, string> satMonths = GetSatMonths();
            DateTime arInvoiceDate = (DateTime)arInvoicePM.InvoiceDate;
            comprobante.InformacionGlobal = new Profact.TimbraCFDI40.ComprobanteInformacionGlobal
            {
                Periodicidad = "",
                Meses = satMonths[arInvoiceDate.Month],
                Año = Convert.ToInt16(arInvoiceDate.Year),
            };
        }

        public Dictionary<int, string> GetSatMonths()
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

        private void CalucalteLineTotals(ARInvoiceLinePM line, Profact.TimbraCFDI40.ComprobanteConcepto concepto, List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups)
        {
            List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados = new List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado>();
            List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions = new List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion>();

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

        private void AddMultiLineVatTypePercentage(ARInvoiceLinePM line, List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
        {
            List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();

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
                totalNoneExpenseVats.Add(record);
            }

            foreach (ARInvoiceTotalVATPM lineTotal in totalNoneExpenseVats)
            {
                if (lineTotal.VATPercent >= 0)
                {
                    AddMultiPercentageLineTranslados(lineTranslados, lineVatType, lineTotal);
                }
                else
                {
                    AddMultiPercentageLineRetencions(lineRetencions, lineVatType, lineTotal);
                }
            }
        }

        private void AddMultiPercentageLineTranslados(List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados, VatType lineVatType, ARInvoiceTotalVATPM lineTotal)
        {
            Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado()
            {
                Base = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0))),
                Impuesto = "002",
                TipoFactor = (lineTotal.VATPercent == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),
            };

            if (traslado.TipoFactor == "Tasa")
            {
                BuildMultiLineTrasladoTasaTipoFactor(lineTotal, traslado);
            }

            lineTranslados.Add(traslado);
        }

        private void BuildMultiLineTrasladoTasaTipoFactor(ARInvoiceTotalVATPM lineTotal, Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado traslado)
        {
            traslado.TasaOCuota = lineTotal.VATPercent != 0 ? (lineTotal.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
            traslado.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2));
            traslado.ImporteSpecified = true;
            traslado.TasaOCuotaSpecified = true;
        }

        private void AddMultiPercentageLineRetencions(List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType, ARInvoiceTotalVATPM lineTotal)
        {
            Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion()
            {
                Impuesto = "002",
                TipoFactor = (lineTotal.VATPercent == 0 ? "Exento" : "Tasa"),
                TasaOCuota = (lineTotal.VATPercent != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8)) : 0),//(lineTotal.VatTypePercentage != null ? (decimal)(Math.Abs(lineTotal.VatTypePercentage.Value / 100)) : 0),
                Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2)),//Math.Abs(((lineTotal.InvoiceCurrencyAmount != null ? ((decimal)lineTotal.InvoiceCurrencyAmount.Value) : 0) * ((lineTotal.VATPercent != null ? (decimal)lineTotal.VATPercent.Value : 0) / 100))),
            };

            if (lineVatType.Code != "EXMPT")
            {
                retencion.Base = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0)));//retencion.Importe;
            }

            lineRetencions.Add(retencion);
        }

        private void AddSingleLineVatTypePercentage(ARInvoiceLinePM line, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
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

        private void AddSinglePercentageLineRetencions(ARInvoiceLinePM line, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions, VatType lineVatType)
        {
            Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion()
            {
                Impuesto = "002",
                TipoFactor = (line.VatPercentage == 0 ? "Exento" : "Tasa"),
                TasaOCuota = (line.VatPercentage != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8)) : 0),
                Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2)),
            };

            if (lineVatType.Code != "EXMPT")
            {
                retencion.Base = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0)));//retencion.Importe;
            }

            lineRetencions.Add(retencion);
        }

        private void AddSinglePercentageLineTranslados(ARInvoiceLinePM line, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados, VatType lineVatType)
        {
            Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado()
            {
                Base = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0))),
                Impuesto = "002",
                TipoFactor = (line.VatPercentage == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),
            };

            if (traslado.TipoFactor == "Tasa")
            {
                BuildSingleLineTrasladoTasaTipoFactor(line, traslado);
            }

            lineTranslados.Add(traslado);
        }

        private void BuildSingleLineTrasladoTasaTipoFactor(ARInvoiceLinePM line, Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado traslado)
        {
            traslado.TasaOCuota = line.VatPercentage != 0 ? (line.VatPercentage != null ? StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8) : "") : "0.000000";
            traslado.Importe = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2));
            traslado.ImporteSpecified = true;
            traslado.TasaOCuotaSpecified = true;
        }

        private void BuildConceptoImpuestos(Profact.TimbraCFDI40.ComprobanteConcepto concepto, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosTraslado> lineTranslados, List<Profact.TimbraCFDI40.ComprobanteConceptoImpuestosRetencion> lineRetencions)
        {
            concepto.Impuestos = new Profact.TimbraCFDI40.ComprobanteConceptoImpuestos();

            if (lineTranslados.Count > 0)
            {
                concepto.Impuestos.Traslados = lineTranslados.ToArray();
            }
            if (lineRetencions.Count > 0)
            {
                concepto.Impuestos.Retenciones = lineRetencions.ToArray();
            }
        }

        private void BuildRelatedInvoiceTag(Profact.TimbraCFDI40.Comprobante comprobante)
        {
            ARInvoice relatedInvoice = GetRelatedInvoice();

            if (relatedInvoice != null && relatedInvoice.SATTransferStatusCode == "TD" && relatedInvoice.StatusCode == "VD") return;
            
            string relatedInvoiceUUID = GetRelatedInvoiceUUID(relatedInvoice);
            BuildCfdiRelacionados(comprobante, relatedInvoiceUUID);
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
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(relatedInvoiceSATXML).Complemento.Any;
            }
            catch (Exception ex)
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(relatedInvoiceSATXML).Complemento.Any;
            }
        }

        private void BuildCfdiRelacionados(Profact.TimbraCFDI40.Comprobante comprobante, string relatedInvoiceUUID)
        {
            if (string.IsNullOrEmpty(relatedInvoiceUUID)) return;

            string tipoRelacion = BuildTipoRelacion();
            comprobante.CfdiRelacionados = new Profact.TimbraCFDI40.ComprobanteCfdiRelacionados[1];
            comprobante.CfdiRelacionados[0] = new Profact.TimbraCFDI40.ComprobanteCfdiRelacionados
            {
                TipoRelacion = tipoRelacion,
                CfdiRelacionado = new List<Profact.TimbraCFDI40.ComprobanteCfdiRelacionadosCfdiRelacionado>() { new Profact.TimbraCFDI40.ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = relatedInvoiceUUID }, }.ToArray()
            };
        }

        private string BuildTipoRelacion()
        {
            if (arInvoicePM.ARInvoiceTypeCode == "CD") return "01";
            List<ARInvoice> shipmentInvoices = GetShipmentInvoicesByEntityIdAndARInvoiceId();

            if (shipmentInvoices == null) return "";
            if (shipmentInvoices.Any(a => a.IsAutoCredit)) return "04";
            else if (shipmentInvoices.Count >= 1) return "02";

            return "";
        }

        private List<ARInvoice> GetShipmentInvoicesByEntityIdAndARInvoiceId()
        {
            return (from a in invoiceCotnext.ARInvoiceEntities
                    where a.EntityId == arInvoicePM.MainEntityId && a.ARInvoiceId != arInvoicePM.Id && a.Tenant == arInvoicePM.Tenant
                    select a.ARInvoice).ToList();
        }

        private List<ARInvoiceTotalVATPM> CalculateNoneExpenseTotalVats(ARInvoicePM entityPM, List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<ChargesType> chargesTypes)
        {
            int tenant = entityPM.Tenant;

            List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();
            List<ARInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => !chargesTypes.First(c => c.Id == d.ChargesTypeId).IsExpense && d.VatTypeId != null).ToList();
            if (myDataLines.Count <= 0) return totalNoneExpenseVats;


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

                    Tenant = entityPM.Tenant,
                    ARInvoiceId = entityPM.Id,
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
                totalNoneExpenseVats.Add(record);

                localAmountTotal += record.LocalVATAmount;
                invoiceAmountTotal += record.InvoiceCurrencyVATAmount;
                profitAmountTotal += record.ProfitCurrencyVATAmount;

                invoiceVatableAmountTotal += record.InvoiceCurrencyVatableAmount;
            }

            var subtotal = invoiceVatableAmountTotal;
            var total = invoiceVatableAmountTotal + invoiceAmountTotal;

            return totalNoneExpenseVats;
        }

        private static void AddLineVatTypePercentage(List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item)
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

        private static void AddMultLineVatTypePercentageInvoiceTotalsClass(List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item)
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

        private static void AddSingleLineVatTypePercentageInvoiceTotalsClass(List<InvoiceTotalsClass> group_Source, ARInvoiceLinePM item, VatType lineVatType)
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

        private static List<InvoiceTotalsClass> GetInvoiceTotalsClassGroupData(List<InvoiceTotalsClass> group_Source)
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
    }
}
