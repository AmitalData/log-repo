using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Profact.TimbraCFDI40;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityQueries;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATPaymentComprobante
    {
        private ARPaymentPM arPaymentPM;
        private IInvoiceContext invoiceContext;
        private ICommonDataContext commonContext;
        private TenantRepository tenantRepository;
        private AddressRepository addressReposirory;
        private CardRepository cardRepository;
        private BranchRepository branchRepository;
        private CurrencyRepository currencyRepository;
        private Tenant currentTenant;
        public SATPaymentComprobante(ARPaymentPM arPaymentPM)
        {
            this.arPaymentPM = arPaymentPM;
            InitalizeContexts(arPaymentPM.Tenant);
            InitalizeRepositories(); 
            currentTenant = tenantRepository.GetSingleTenant(arPaymentPM.Tenant);
        }

        private void InitalizeContexts(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
        }

        private void InitalizeRepositories()
        {
            tenantRepository = new TenantRepository(commonContext);
            addressReposirory = new AddressRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            branchRepository = new BranchRepository(commonContext);
            currencyRepository = new CurrencyRepository(commonContext);
        }

        public Comprobante BuildNewPaymentComprobante()
        {
            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(arPaymentPM.Tenant);

            List<Currency> currencies = currencyRepository.GetCurrencies(arPaymentPM.Tenant).ToList();
            Currency paymentCurrency = currencies.FirstOrDefault(c => c.Id == arPaymentPM.PaymentCurrencyId);

            string arPaymentCounterPrefix = TableCounter.GetCounterPrefix(arPaymentPM.Tenant, "ARPT", "DR", null);

            Comprobante comprobante = new Comprobante
            {
                CfdiRelacionados = GetCfdiRelacionados(),
                Total = 0,
                Version = SATData.CurrentComprobanteVersion,
                TipoDeComprobante = "P",
                SubTotal = 0,
                Serie = GetSerie(arPaymentCounterPrefix),
                Folio = GetFolio(arPaymentCounterPrefix),
                Moneda = "XXX",
                Fecha = GetFecha(),
                LugarExpedicion = GetLugarExpedicion(),
                Emisor = GetEmisor(),
                Conceptos = GetConceptos()
            };

            Address billToAddress = null;
            if (!string.IsNullOrEmpty(arPaymentPM.BillToAddressId))
            {
                billToAddress = addressReposirory.GetSingleAddress(arPaymentPM.BillToAddressId, arPaymentPM.Tenant);
            }

            string billToCountryCode = GetBillToCountryCode(computingPartnerHelper, billToAddress);

            comprobante.Receptor = GetReceptor(billToCountryCode);

            List<PagosPago> pagosPagoList = new List<PagosPago>();
            Pagos pagos = new Pagos
            {
                Version = "2.0",
            };

            PagosPago pagoItem = GetNewPagosPagoInstance(arPaymentPM, paymentCurrency, billToCountryCode);

            List<ARInvoice> paymentARInvoices = GetPaymentARInvoices();
            List<PagosPagoDoctoRelacionado> doctos = GetDoctoRelacionados(currencies, paymentARInvoices);
            pagoItem.DoctoRelacionado = doctos.ToArray();

            List<PagosPagoImpuestosP> impuestosPs = GetPagosPagoImpuestosPs(doctos);
            pagoItem.ImpuestosP = impuestosPs.ToArray();

            pagosPagoList.Add(pagoItem);
            pagos.Pago = pagosPagoList.ToArray();
            pagos.Totales = GetNewPagosTotalesInstance(pagoItem);

            comprobante.Pagos20Specified = true;
            comprobante.Complemento = GetComplemento(pagos);

            return comprobante;
        }

        private static List<PagosPagoImpuestosP> GetPagosPagoImpuestosPs(List<PagosPagoDoctoRelacionado> doctos)
        {
            List<PagosPagoImpuestosP> impuestosPs = new List<PagosPagoImpuestosP>();
            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();
            doctos.ForEach(d =>
            {
                pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.Concat(d.ImpuestosDR.TrasladosDR.ToArray()).ToList();
                pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.Concat(d.ImpuestosDR.RetencionesDR.ToArray()).ToList();
            });

            PagosPagoImpuestosP pagosPagoImpuestosP = new PagosPagoImpuestosP();
            List<PagosPagoImpuestosPTrasladoP> trasladosP = GetPagosPagoImpuestosPTrasladoPs(pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs);
            List<PagosPagoImpuestosPRetencionP> retencionesP = GetPagosPagoImpuestosPRetencionPs(pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs);

            pagosPagoImpuestosP.TrasladosP = trasladosP.ToArray();
            pagosPagoImpuestosP.RetencionesP = retencionesP.ToArray();

            impuestosPs.Add(pagosPagoImpuestosP);
            return impuestosPs;
        }

        private static List<PagosPagoImpuestosPRetencionP> GetPagosPagoImpuestosPRetencionPs(List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs)
        {
            List<PagosPagoImpuestosPRetencionP> retencionesP = new List<PagosPagoImpuestosPRetencionP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.GroupBy(x => x.ImporteDR);
            foreach (var group in groups)
            {
                retencionesP.Add(GetNewPagosPagoImpuestosPRetencionP(group));
            }

            return retencionesP;
        }

        private static PagosPagoImpuestosPRetencionP GetNewPagosPagoImpuestosPRetencionP(IGrouping<decimal, PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> group)
        {
            return new PagosPagoImpuestosPRetencionP
            {
                ImporteP = group.Key,
                ImpuestoP = "002",
            };
        }

        private static List<PagosPagoImpuestosPTrasladoP> GetPagosPagoImpuestosPTrasladoPs(List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs)
        {
            List<PagosPagoImpuestosPTrasladoP> trasladosP = new List<PagosPagoImpuestosPTrasladoP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.GroupBy(x => x.ImporteDR);
            foreach (var group in groups)
            {
                trasladosP.Add(GetNewPagosPagoImpuestosPTrasladoP(group));
            }

            return trasladosP;
        }

        private static PagosPagoImpuestosPTrasladoP GetNewPagosPagoImpuestosPTrasladoP(IGrouping<decimal, PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> group)
        {
            decimal totalBaseDR = 0;
            decimal totalTasaOCuotaDR = 0;
            foreach (var product in group)
            {
                totalBaseDR += product.BaseDR;
                totalTasaOCuotaDR += product.TasaOCuotaDR;
            }
            return new PagosPagoImpuestosPTrasladoP
            {
                BaseP = totalBaseDR,
                ImporteP = group.Key,
                ImpuestoP = "002",
                TasaOCuotaP = totalTasaOCuotaDR,
                TipoFactorP = group.Key == 0 ? "Exento" : "Tasa",
            };
        }

        private List<PagosPagoDoctoRelacionado> GetDoctoRelacionados(List<Currency> currencies, List<ARInvoice> paymentARInvoices)
        {
            List<PagosPagoDoctoRelacionado> doctos = new List<PagosPagoDoctoRelacionado>();
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(arPaymentPM.Tenant);
            paymentARInvoices.ForEach(invoice =>
            {
                Comprobante invoiceComprobante = null;
                Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33 = null;
                try
                {
                    invoiceComprobante = LogitudeXmlSerializer.DeserializeObject<Comprobante>(invoice.SATXML);
                }
                catch (Exception ex)
                {
                     invoiceComprobanteV33 = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(invoice.SATXML);
                }

                List<ARInvoicePayment> allInvoicePayments = (from a in invoiceContext.ARInvoicePayments.Include("ARPayment")
                                                             where a.ARInvoiceId == invoice.Id && a.Tenant == invoice.Tenant
                                                             select a).ToList();

                XmlElement[] comprobanteComplementoAnyXmlElements = invoiceComprobante != null ? invoiceComprobante.Complemento.Any : invoiceComprobanteV33.Complemento.Any;
                decimal totalInvoiceComprobante = invoiceComprobante != null ? invoiceComprobante.Total : invoiceComprobanteV33.Total;
                PagosPagoDoctoRelacionado doctoItem = new PagosPagoDoctoRelacionado
                {
                    MonedaDR = GetPagosPagoDoctoRelacionadoMonedaDR(currencies, invoice),
                    Serie = invoiceComprobante != null ? invoiceComprobante.Serie : invoiceComprobanteV33.Serie,
                    Folio = invoiceComprobante != null ? invoiceComprobante.Folio : invoiceComprobanteV33.Folio,
                    ObjetoImpDR = "02",
                    ImpPagado = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)(allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == arPaymentPM.Id)).ForeignAmount),
                    ImpPagadoSpecified = true,
                    EquivalenciaDR = (invoice.InvoiceCurrencyExchangeRate != null ? Convert.ToDecimal(invoice.InvoiceCurrencyExchangeRate.Value) : 0),
                    EquivalenciaDRSpecified = true,
                    IdDocumento = GetPagosPagoDoctoRelacionadoIdDocumento(comprobanteComplementoAnyXmlElements),
                    ImpSaldoAnt = GetPagosPagoDoctoRelacionadoImpSaldoAnt(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante }),
                    NumParcialidad = GetPagosPagoDoctoRelacionadoImpSaldoAntNumParcialidad(invoice, arPaymentPM, allInvoicePayments),
                    ImpSaldoInsoluto = GetGetPagosPagoDoctoRelacionadoImpSaldoInsoluto(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante })
                };
                doctoItem.ImpuestosDR = new PagosPagoDoctoRelacionadoImpuestosDR();
                List<ComprobanteImpuestos> impuestosList = new List<ComprobanteImpuestos>();
                List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> trasladoDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
                List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> retencionDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();

                ARInvoicePM arInvoicePM = aRInvoiceQuery.GetSinglePM(invoice.Id, arPaymentPM.Tenant);
                SATInvoiceComprobante sATInvoiceComprobante = new SATInvoiceComprobante(arInvoicePM, currentTenant, null);

                ComprobanteConcepto[] comprobanteConceptos = sATInvoiceComprobante.GetConceptoList().ToArray();
                List<ARInvoiceTotalVATPM> arTotalVats = sATInvoiceComprobante.GetARInvoiceTotalVATPMs();

                ComprobanteImpuestosResults comprobanteImpuestosResults = sATInvoiceComprobante.GetComprobanteImpuestos(arTotalVats, comprobanteConceptos);
                comprobanteImpuestosResults.ComprobanteImpuestos.Traslados.ToList().ForEach(comprobanteImpuestosTraslado =>
                {
                    trasladoDRList.Add(new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
                    {
                        BaseDR = comprobanteImpuestosTraslado.Base,
                        ImporteDR = comprobanteImpuestosTraslado.Importe,
                        ImporteDRSpecified = true,
                        ImpuestoDR = comprobanteImpuestosTraslado.Impuesto,
                        TasaOCuotaDR = comprobanteImpuestosTraslado.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosTraslado.TasaOCuota) : 0,
                        TasaOCuotaDRSpecified = true,
                        TipoFactorDR = comprobanteImpuestosTraslado.TipoFactor,
                    });
                });

                comprobanteImpuestosResults.ComprobanteImpuestosRetencionDRs.ToList().ForEach(comprobanteImpuestosRetencion =>
                {
                    retencionDRList.Add(new PagosPagoDoctoRelacionadoImpuestosDRRetencionDR
                    {
                        ImporteDR = comprobanteImpuestosRetencion.Importe,
                        ImpuestoDR = comprobanteImpuestosRetencion.Impuesto,
                        BaseDR = comprobanteImpuestosRetencion.Base,
                        TasaOCuotaDR = comprobanteImpuestosRetencion.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosRetencion.TasaOCuota) : 0,
                        TipoFactorDR = comprobanteImpuestosRetencion.TipoFactor,
                    });
                });

                doctoItem.ImpuestosDR.TrasladosDR = trasladoDRList.ToArray();
                doctoItem.ImpuestosDR.RetencionesDR = retencionDRList.ToArray();

                doctos.Add(doctoItem);
            });
            return doctos;
        }

        private List<ARInvoice> GetPaymentARInvoices()
        {
            List<string> invoiceIds = arPaymentPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();
            List<ARInvoice> paymentARInvoices = (from a in invoiceContext.ARInvoices
                                                 where a.Tenant == arPaymentPM.Tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();
            return paymentARInvoices;
        }

        private ComprobanteComplemento GetComplemento(Pagos pagos)
        {
            List<XmlElement> LXmlComplementos = new List<XmlElement>();
            System.Xml.Serialization.XmlSerializerNamespaces nsPagos = new System.Xml.Serialization.XmlSerializerNamespaces();
            nsPagos.Add("pago20", "http://www.sat.gob.mx/Pagos20");
            string xmlPagos = Profact.TimbraCFDI.XMLUtilerias.SerializaObjeto(pagos, typeof(Pagos), nsPagos);
            XmlDocument docNominas = new XmlDocument();
            docNominas.LoadXml(xmlPagos);
            LXmlComplementos.Add(docNominas.DocumentElement);

            return new ComprobanteComplemento
            {
                Any = LXmlComplementos.ToArray<XmlElement>()
            };
        }

        private ComprobanteConcepto[] GetConceptos()
        {
            List<ComprobanteConcepto> conceptosList = new List<ComprobanteConcepto>();
            ComprobanteConcepto concepto = new ComprobanteConcepto()
            {
                Cantidad = 1,
                Descripcion = "Pago",
                Importe = 0,
                ValorUnitario = 0,
                ClaveProdServ = "84111506",
                ClaveUnidad = "ACT",
            };

            conceptosList.Add(concepto);
            return conceptosList.ToArray();
        }

        private ComprobanteReceptor GetReceptor(string billToCountryCode)
        {
            ComprobanteReceptor comprobanteReceptor = new ComprobanteReceptor();
            Card billToCard = cardRepository.GetSingleCard(arPaymentPM.BillToId, arPaymentPM.Tenant);
            if (billToCountryCode == "MEX" || billToCountryCode == "MX")
            {
                if (!string.IsNullOrEmpty(billToCard.VatNumber))
                    comprobanteReceptor.Rfc = billToCard.VatNumber;
                else
                    comprobanteReceptor.Rfc = "AAA010101AAA";
            }
            else
            {
                if (!string.IsNullOrEmpty(billToCard.SATForeignRFC))
                    comprobanteReceptor.Rfc = billToCard.SATForeignRFC;
                else
                    comprobanteReceptor.Rfc = "XEXX010101000";

                comprobanteReceptor.ResidenciaFiscal = billToCountryCode;
                comprobanteReceptor.ResidenciaFiscalSpecified = true;
            }
            comprobanteReceptor.Nombre = billToCard.EnglishName;
            comprobanteReceptor.UsoCFDI = "P01";

            return comprobanteReceptor;
        }

        private ComprobanteEmisor GetEmisor()
        {
            return new ComprobanteEmisor
            {
                Rfc = currentTenant.VatNumber,
                Nombre = currentTenant.Company,
                RegimenFiscal = "601"
            };
        }

        private string GetLugarExpedicion()
        {
            Address branchAddress = null;
            if (!string.IsNullOrEmpty(arPaymentPM.BranchId))
            {
                branchAddress = GetBranchAddress(branchAddress);
            }

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                return branchAddress.ZipCode;
            }
            else
                return currentTenant.Address.ZipCode;
        }

        private Address GetBranchAddress(Address branchAddress)
        {
            Branch branch = branchRepository.GetSingleBranch(arPaymentPM.BranchId, arPaymentPM.Tenant);
            if (!string.IsNullOrEmpty(branch.AddressId))
            {
                branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
            }

            return branchAddress;
        }

        private DateTime GetFecha()
        {
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(arPaymentPM.Tenant);
            return new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
        }

        private string GetFolio(string arPaymentCounterPrefix)
        {
            string folio = arPaymentPM.PaymentNo;

            if (string.IsNullOrEmpty(arPaymentCounterPrefix)) return folio;
            

            if (string.IsNullOrEmpty(arPaymentPM.PaymentNo)) return folio;
            

            if (arPaymentPM.PaymentNo.Contains(arPaymentCounterPrefix))
            {
                folio = arPaymentPM.PaymentNo.Remove(0, arPaymentCounterPrefix.Length);
            }

            return folio;
        }

        private static string GetSerie(string counterPrefix)
        {
            string serie = "A";
            if (!string.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;
            }

            return serie;
        }

        private ComprobanteCfdiRelacionados[] GetCfdiRelacionados()
        {
            ComprobanteCfdiRelacionados[] comprobanteCfdiRelacionados = null;
            if (string.IsNullOrEmpty(arPaymentPM.SATXML)) return comprobanteCfdiRelacionados;

            Comprobante paymentComprobante = LogitudeXmlSerializer.DeserializeObject<Comprobante>(arPaymentPM.SATXML);

            if (paymentComprobante.Complemento.Any == null) return comprobanteCfdiRelacionados;

            List<XmlElement> myLXmlComplementos = paymentComprobante.Complemento.Any.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return comprobanteCfdiRelacionados;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            comprobanteCfdiRelacionados = new ComprobanteCfdiRelacionados[1];
            comprobanteCfdiRelacionados[0] = new ComprobanteCfdiRelacionados
            {
                TipoRelacion = "04",
                CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>() { new ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = digitalTi.UUID } }.ToArray()
            };

            return comprobanteCfdiRelacionados;
        }

        private string GetPagosPagoDoctoRelacionadoMonedaDR(List<Currency> currencies, ARInvoice invoice)
        {
            Currency invoiceCurrency = currencies.FirstOrDefault(c => c.Id == invoice.InvoiceCurrencyId);
            return invoiceCurrency.Code;
        }

        private decimal GetGetPagosPagoDoctoRelacionadoImpSaldoInsoluto(PagosPagoDoctoRelacionadoImpSaldo pagosPagoDoctoRelacionadoImpSaldo)
        {
            decimal pagosPagoDoctoRelacionadoImpSaldoAnt = GetPagosPagoDoctoRelacionadoImpSaldoAnt(pagosPagoDoctoRelacionadoImpSaldo);
            decimal invoicePaymentForeignAmount = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)(pagosPagoDoctoRelacionadoImpSaldo.AllInvoicePayments.FirstOrDefault(p => p.ARPaymentId == pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.Id)).ForeignAmount);
            if (pagosPagoDoctoRelacionadoImpSaldoAnt != 0)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagosPagoDoctoRelacionadoImpSaldoAnt - invoicePaymentForeignAmount);
            }
            return 0;
        }

        private static string GetPagosPagoDoctoRelacionadoImpSaldoAntNumParcialidad(ARInvoice invoice, ARPaymentPM entityPM, List<ARInvoicePayment> allInvoicePayments)
        {
            List<ARInvoicePayment> sentInvoicePayments = allInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != entityPM.Id).ToList();
            if (entityPM.AmountInPaymentCurrency == invoice.AmountInInvoiceCurrency || sentInvoicePayments.Count == 0)
            {
                return "1";
            }
            else
            {
                return (sentInvoicePayments.Count() + 1).ToString();
            }
        }

        private decimal GetPagosPagoDoctoRelacionadoImpSaldoAnt(PagosPagoDoctoRelacionadoImpSaldo pagosPagoDoctoRelacionadoImpSaldo)
        {
            decimal previouslySentPaymentsTotal = 0;
            if (pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.AmountInPaymentCurrency == pagosPagoDoctoRelacionadoImpSaldo.ARInvoice.AmountInInvoiceCurrency)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
            }

            List<ARInvoicePayment> sentInvoicePayments = pagosPagoDoctoRelacionadoImpSaldo.AllInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.Id).ToList();
            if (sentInvoicePayments.Count == 0)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
            }

            sentInvoicePayments.ForEach(p =>
            {
                previouslySentPaymentsTotal += (decimal)(p.ForeignAmount.Value);
            });

            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
        }

        private static string GetPagosPagoDoctoRelacionadoIdDocumento(XmlElement[] comprobanteComplementoAnyXmlElements)
        {
            if (comprobanteComplementoAnyXmlElements == null) return null;

            List<System.Xml.XmlElement> myLXmlComplementos = comprobanteComplementoAnyXmlElements.ToList<System.Xml.XmlElement>();
            System.Xml.XmlElement timbreFiscalDigitalXmlElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalXmlElement != null)
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalXmlElement.OuterXml).UUID;
            }

            return null;
        }

        private static string GetBillToCountryCode(ComputingPartnerTranslationHelper computingPartnerHelper, Address billToAddress)
        {
            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            return billToCountryCode;
        }

        private static PagosTotales GetNewPagosTotalesInstance(PagosPago pagoItem)
        {
            return new PagosTotales
            {
                MontoTotalPagos = pagoItem.Monto * pagoItem.TipoCambioP,
                //TotalRetencionesIVA = 0,
                //TotalRetencionesISR = 0,
                //TotalRetencionesIEPS = 0,
                //TotalTrasladosBaseIVA16 = 0,
                //TotalTrasladosImpuestoIVA16 = 0,
                //TotalTrasladosBaseIVA8 = 0,
                //TotalTrasladosImpuestoIVA8 = 0,
                //TotalTrasladosBaseIVA0 = 0,
                //TotalTrasladosImpuestoIVA0 = 0,
                //TotalTrasladosBaseIVAExento = 0
            };
        }

        private PagosPago GetNewPagosPagoInstance(ARPaymentPM entityPM, Currency paymentCurrency, string billToCountryCode)
        {
            PagosPago pagoItem = new PagosPago
            {
                Monto = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(entityPM.AmountInPaymentCurrency != null ? (decimal)entityPM.AmountInPaymentCurrency.Value : 0),
                MonedaP = paymentCurrency.Code,
                FormaDePagoP = entityPM.SATPaymentMethodCode,
                FechaPago = GetPagosPagoFechaPago(entityPM),
                NumOperacion = GetPagosPagoNumOperacion(entityPM),
                TipoCadPagoSpecified = GetPagosPagoTipoCadPagoSpecified(entityPM),
                TipoCadPago = GetPagosPagoTipoCadPago(entityPM),
                CertPago = GetPagosPagoCertPago(entityPM),
                CadPago = GetPagosPagoCadPago(entityPM),
                SelloPago = GetPagosPagoSelloPago(entityPM),
                TipoCambioPSpecified = GetTipoCambioPSpecified(paymentCurrency),
                TipoCambioP = GetTipoCambioP(entityPM, paymentCurrency),
                RfcEmisorCtaOrd = GetRfcEmisorCtaOrd(entityPM, billToCountryCode),
            };

            return pagoItem;
        }

        private string GetRfcEmisorCtaOrd(ARPaymentPM entityPM, string billToCountryCode)
        {
            List<string> paymentMethods = new List<string> { "02", "03", "04", "28", "29" };
            bool isMexicoCountry = billToCountryCode == SATData.MexicanInvoiceCurrencyCode || billToCountryCode == SATData.MexicoCountryCode;

            if (!isMexicoCountry && paymentMethods.Contains(entityPM.SATPaymentMethodCode))
            {
                return SATData.OutSideMexicoRfc;
            }

            return "";
        }

        private decimal GetTipoCambioP(ARPaymentPM entityPM, Currency paymentCurrency)
        {
            if (paymentCurrency.Code != SATData.MexicanInvoiceCurrencyCode)
                return (entityPM.PaymentCurrencyExchangeRate != null ? Convert.ToDecimal(entityPM.PaymentCurrencyExchangeRate.Value) : 0);

            return 1;
        }

        private bool GetTipoCambioPSpecified(Currency paymentCurrency)
        {
            return paymentCurrency.Code != SATData.MexicanInvoiceCurrencyCode;
        }

        private byte[] GetPagosPagoSelloPago(ARPaymentPM entityPM)
        {
            if (!IsValidPaymentBankTransfer(entityPM)) return null;

            return Encoding.ASCII.GetBytes(entityPM.SelloPago);
        }

        private bool IsValidPaymentBankTransfer(ARPaymentPM entityPM)
        {
            if (entityPM.SATPaymentMethodCode != SATData.LogitudeSATPaymentBankTransferMethod || entityPM.TipoCadenaPago != SATData.SATPaymentBankTransferTypeCode) return false;
            ValidatePagosPago(entityPM);
            return true;
        }

        private string GetPagosPagoCadPago(ARPaymentPM entityPM)
        {
            if (!IsValidPaymentBankTransfer(entityPM)) return null;

            if (!string.IsNullOrEmpty(entityPM.CadPago))
                return entityPM.CadPago.Replace("|", "&#124;");

            return null;
        }

        private byte[] GetPagosPagoCertPago(ARPaymentPM entityPM)
        {
            if (!IsValidPaymentBankTransfer(entityPM)) return null;

            return Encoding.ASCII.GetBytes(entityPM.CertPago);
        }

        private string GetPagosPagoTipoCadPago(ARPaymentPM entityPM)
        {
            if (!IsValidPaymentBankTransfer(entityPM)) return null;

            return SATData.SATPaymentBankTransferTypeCode;
        }

        private bool GetPagosPagoTipoCadPagoSpecified(ARPaymentPM entityPM)
        {
            if (!IsValidPaymentBankTransfer(entityPM)) return false;
            return true;
        }

        private void ValidatePagosPago(ARPaymentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.CertPago))
                throw new ApplicationException("Cert Pago is required");

            if (string.IsNullOrEmpty(entityPM.SelloPago))
                throw new ApplicationException("Sello Pago is required");

            if (string.IsNullOrEmpty(entityPM.CadPago))
                throw new ApplicationException("Cad Pago is required");
        }

        private static string GetPagosPagoNumOperacion(ARPaymentPM entityPM)
        {
            string numOperacion = null;
            if ((entityPM.AccountingPaymentMethodCode == "CC" || entityPM.AccountingPaymentMethodCode == "BT" || entityPM.AccountingPaymentMethodCode == "CH") && !string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef))
            {
                numOperacion = StringHelper.TruncateLongString(entityPM.ChequeOrPaymentRef, 100);
            }

            return numOperacion;
        }

        private static DateTime GetPagosPagoFechaPago(ARPaymentPM entityPM)
        {
            DateTime fechaPago = entityPM.RegisterDate.Value;

            TimeSpan time = new TimeSpan(12, 00, 00);
            DateTime resultdate = fechaPago.Date + time;
            fechaPago = resultdate;
            if (entityPM.FechaPago != null)
            {
                fechaPago = entityPM.FechaPago.Value;
            }

            return fechaPago;
        }
    }

    public class PagosPagoDoctoRelacionadoImpSaldo
    {
        public ARPaymentPM ARPaymentPM { get; set; } 
        public ARInvoice ARInvoice { get; set; }
        public List<ARInvoicePayment> AllInvoicePayments { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
