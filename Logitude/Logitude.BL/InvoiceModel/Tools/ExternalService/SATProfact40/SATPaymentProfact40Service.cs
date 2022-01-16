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
using Profact.TimbraCFDI40.Complementos.Pagos20;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATPaymentProfact40Service
    {
        private ARPayment arPpayment;
        private XmlElement comprobanteXmlPagos;
        private ARInvoiceRepository arInvoiceRep;
        private int tenant;
        private string paymentId;
        private SATBaseProfact40Service sATBaseProfact40Service;

        public SATPaymentProfact40Service(ARPayment arPpayment, XmlElement comprobanteXmlPagos, ARInvoiceRepository arInvoiceRep)
        {
            tenant = arPpayment.Tenant;
            paymentId = arPpayment.Id;
            this.arPpayment = arPpayment;
            this.comprobanteXmlPagos = comprobanteXmlPagos;
            this.arInvoiceRep = arInvoiceRep;
        }

        public SATPaymentProfact40Service(string paymentId, int tenant)
        {
            this.tenant = tenant;
            this.paymentId = paymentId;
            this.sATBaseProfact40Service = new SATBaseProfact40Service(tenant);
        }

        public void SendRequest()
        {
            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            AddressRepository addressReposirory = new AddressRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);
            BranchRepository branchRepository = new BranchRepository(commonContext);
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);

            List<Currency> currencies = currencyRepository.GetCurrencies(tenant).ToList();
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM entityPM = paymentQuery.GetSinglePM(paymentId, tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(entityPM.Tenant);
            ARPaymentRepository arPaymentRepository = new ARPaymentRepository(entityPM.Tenant);
            ARPayment entityPoco = arPaymentRepository.GetSingleARPayment(paymentId, tenant);

            if (entityPM.MetodoPagoCode == "PUE")
            {
                throw new Exception("You can't proceed with this operation. You are allowed to send to SAT only when the Metodo Pago value is PPD");
            }

            if (entityPM.SATTransferStatusCode == "TD" && (entityPM.StatusCode == "VD"))
            {
                throw new Exception("You can't send this payment to SAT because it was cancelled");
            }

            List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> paymentARInvoices = (from a in invoiceContext.ARInvoices
                                                 where a.Tenant == tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();

            if (paymentARInvoices.Any(p => string.IsNullOrEmpty(p.SATXML)))
            {
                throw new Exception("Some Invoices are not approved from sat");
            }

            Currency paymentCurrency = currencies.FirstOrDefault(c => c.Id == entityPM.PaymentCurrencyId);
            Address branchAddress = null;
            Address mainAddress = null;
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            if (!string.IsNullOrEmpty(entityPM.BranchId))
            {
                Branch branch = branchRepository.GetSingleBranch(entityPM.BranchId, entityPM.Tenant);
                if (!string.IsNullOrEmpty(branch.AddressId))
                {
                    branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
                }
            }


            Address billToAddress = null;
            if (!string.IsNullOrEmpty(entityPM.BillToAddressId))
            {
                billToAddress = addressReposirory.GetSingleAddress(entityPM.BillToAddressId, entityPM.Tenant);
            }
            else
                throw new ApplicationException("Bill to Address is required ");

            Card billToCard = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceContext.SATPaymentMethods select d).ToList();

            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }

            if (string.IsNullOrEmpty(billToCard.SATPaymentMethodCode))
            {
                throw new ApplicationException("Bill to Forma Pago is required");
            }

            if (string.IsNullOrEmpty(entityPM.SATPaymentMethodCode))
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

            string serie = "A";
            string folio = entityPM.PaymentNo;

            string counterPrefix = null;
            counterPrefix = TableCounter.GetCounterPrefix(entityPM.Tenant, "ARPT", "DR", null);
            if (!String.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;

                if (!string.IsNullOrEmpty(entityPM.PaymentNo))
                {
                    if (entityPM.PaymentNo.Contains(counterPrefix))
                    {
                        folio = entityPM.PaymentNo.Remove(0, counterPrefix.Length);
                    }
                }
            }

            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == entityPM.SATPaymentMethodCode).FirstOrDefault();
            Profact.TimbraCFDI40.Comprobante comprobante = new Profact.TimbraCFDI40.Comprobante();
            if (!string.IsNullOrEmpty(entityPM.SATXML))
            {
                Profact.TimbraCFDI40.Comprobante paymentComprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(entityPM.SATXML);
                if (paymentComprobante.Complemento.Any != null)
                {
                    List<XmlElement> myLXmlComplementos = paymentComprobante.Complemento.Any.ToList();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        comprobante.CfdiRelacionados = new Profact.TimbraCFDI40.ComprobanteCfdiRelacionados[1];
                        comprobante.CfdiRelacionados[0] = new Profact.TimbraCFDI40.ComprobanteCfdiRelacionados();
                        comprobante.CfdiRelacionados[0].TipoRelacion = "04";
                        comprobante.CfdiRelacionados[0].CfdiRelacionado = new List<Profact.TimbraCFDI40.ComprobanteCfdiRelacionadosCfdiRelacionado>() { new Profact.TimbraCFDI40.ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = digitalTi.UUID } }.ToArray();
                    }
                }
            }

            comprobante.Total = 0;
            comprobante.Version = "3.3";
            comprobante.TipoDeComprobante = "P";
            comprobante.SubTotal = 0;
            comprobante.Serie = serie;
            comprobante.Moneda = "XXX";
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            comprobante.Fecha = currentDateTime;
            comprobante.Fecha = new DateTime(comprobante.Fecha.Year, comprobante.Fecha.Month, comprobante.Fecha.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                comprobante.LugarExpedicion = branchAddress.ZipCode;
            }
            else
                comprobante.LugarExpedicion = currentTenant.Address.ZipCode;

            comprobante.Folio = folio;

            comprobante.Emisor = new Profact.TimbraCFDI40.ComprobanteEmisor
            {
                Rfc = currentTenant.VatNumber,
                Nombre = currentTenant.Company,
                RegimenFiscal = "601"
            };


            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            comprobante.Receptor = new Profact.TimbraCFDI40.ComprobanteReceptor();
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

                comprobante.Receptor.ResidenciaFiscal = billToCountryCode;
                comprobante.Receptor.ResidenciaFiscalSpecified = true;
            }
            comprobante.Receptor.Nombre = billToCard.EnglishName;
            comprobante.Receptor.UsoCFDI = "P01";


            List<Profact.TimbraCFDI40.ComprobanteConcepto> conceptosList = new List<Profact.TimbraCFDI40.ComprobanteConcepto>();
            Profact.TimbraCFDI40.ComprobanteConcepto concepto = new Profact.TimbraCFDI40.ComprobanteConcepto()
            {
                Cantidad = 1,
                Descripcion = "Pago",
                Importe = 0,
                ValorUnitario = 0,
                ClaveProdServ = "84111506",
                ClaveUnidad = "ACT",
            };

            conceptosList.Add(concepto);
            comprobante.Conceptos = conceptosList.ToArray();



            List<PagosPago> pagosPagoList = new List<PagosPago>();
            Pagos pagos = new Pagos
            {
                Version = "1.0"
            };
            PagosPago pagoItem = new PagosPago
            {
                Monto = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(entityPM.AmountInPaymentCurrency != null ? (decimal)entityPM.AmountInPaymentCurrency.Value : 0),
                MonedaP = paymentCurrency.Code
            };

            if (entityPM.SATPaymentMethodCode == "03" && entityPM.TipoCadenaPago == "01")
            {
                if (string.IsNullOrEmpty(entityPM.CertPago))
                    throw new ApplicationException("Cert Pago is required");

                if (string.IsNullOrEmpty(entityPM.SelloPago))
                    throw new ApplicationException("Sello Pago is required");

                if (string.IsNullOrEmpty(entityPM.CadPago))
                    throw new ApplicationException("Cad Pago is required");

                pagoItem.TipoCadPagoSpecified = true;
                pagoItem.TipoCadPago = "01";
                pagoItem.CertPago = Encoding.ASCII.GetBytes(entityPM.CertPago); //Wesam1
                if (!string.IsNullOrEmpty(entityPM.CadPago))
                    pagoItem.CadPago = entityPM.CadPago.Replace("|", "&#124;");
                pagoItem.SelloPago = Encoding.ASCII.GetBytes(entityPM.SelloPago); //Wesam2

            }


            if (paymentCurrency.Code != "MXN")
            {
                pagoItem.TipoCambioP = (entityPM.PaymentCurrencyExchangeRate != null ? Convert.ToDecimal(entityPM.PaymentCurrencyExchangeRate.Value) : 0);
                pagoItem.TipoCambioPSpecified = true;
            }

            pagoItem.FormaDePagoP = entityPM.SATPaymentMethodCode;
            pagoItem.FechaPago = entityPM.RegisterDate.Value;

            TimeSpan time = new TimeSpan(12, 00, 00);
            DateTime resultdate = pagoItem.FechaPago.Date + time;
            pagoItem.FechaPago = resultdate;
            if (entityPM.FechaPago != null)
            {
                pagoItem.FechaPago = entityPM.FechaPago.Value;
            }

            List<PagosPagoDoctoRelacionado> doctos = new List<PagosPagoDoctoRelacionado>();
            int number = 1;
            paymentARInvoices.ForEach(invoice =>
            {
                Profact.TimbraCFDI40.Comprobante invoiceComprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(invoice.SATXML);

                List<ARInvoicePayment> allInvoicePayments = (from a in invoiceContext.ARInvoicePayments.Include("ARPayment")
                                                             where a.ARInvoiceId == invoice.Id && a.Tenant == invoice.Tenant
                                                             select a).ToList();

                ARPaymentInvoicePM paymentInvoice = entityPM.PaymentInvoices.First(p => p.ARInvoiceId == invoice.Id);
                Currency invoiceCurrency = currencies.FirstOrDefault(c => c.Id == invoice.InvoiceCurrencyId);

                PagosPagoDoctoRelacionado doctoItem = new PagosPagoDoctoRelacionado
                {
                    MonedaDR = invoiceCurrency.Code
                };

                ARInvoicePayment currentARInvoicePayment = allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == entityPM.Id);
                decimal previouslySentPaymentsTotal = 0;
                decimal invoiceAmount = (decimal)invoiceComprobante.Total;
                decimal currentPaymentAmount = (decimal)currentARInvoicePayment.ForeignAmount;

                if (entityPM.AmountInPaymentCurrency == invoice.AmountInInvoiceCurrency)
                {
                    doctoItem.NumParcialidad = "1";
                }
                else
                {
                    List<ARInvoicePayment> sentInvoicePayments = allInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != entityPM.Id).ToList();
                    if (sentInvoicePayments.Count == 0)
                    {
                        doctoItem.NumParcialidad = "1";
                    }
                    else
                    {
                        sentInvoicePayments.ForEach(p =>
                        {
                            previouslySentPaymentsTotal += (decimal)(p.ForeignAmount.Value);

                        });

                        doctoItem.NumParcialidad = (sentInvoicePayments.Count() + 1).ToString();
                    }

                }


                decimal imSaldoAnt = (invoiceAmount - previouslySentPaymentsTotal);
                doctoItem.ImpSaldoAnt = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((invoiceAmount - previouslySentPaymentsTotal)); // previous amount (not sent to profact amount)
                //doctoItem.ImpSaldoAntSpecified = true; //Wesam3

                doctoItem.ImpPagado = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(currentPaymentAmount); // current amount to pay
                doctoItem.ImpPagadoSpecified = true;
                if (doctoItem.ImpSaldoAnt != 0)
                {
                    doctoItem.ImpSaldoInsoluto = sATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(doctoItem.ImpSaldoAnt - doctoItem.ImpPagado); // remaining after the current payment
                    //doctoItem.ImpSaldoInsolutoSpecified = true; //Wesam4
                }

                if (paymentInvoice.ExchangeRate != null && invoiceCurrency.Code != paymentCurrency.Code)
                {
                    if (paymentCurrency.Code == "MXN" || paymentCurrency.Code == "MX")
                    {
                        decimal tipoCambioDR = 1 / (decimal)paymentInvoice.ExchangeRate.Value;
                       // doctoItem.TipoCambioDR = sATBaseProfact40Service.GetDecimalWith6DigitsAfterPoint((tipoCambioDR)) + decimal.Parse("0.000001"); //Wesam5
                    }
                    //else
                       // doctoItem.TipoCambioDR = (decimal)paymentInvoice.ExchangeRate.Value; //Wesam6

                    //doctoItem.TipoCambioDRSpecified = true;//Wesam7
                }


                if (invoiceComprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        doctoItem.IdDocumento = digitalTi.UUID;
                    }
                }

                doctoItem.Serie = invoiceComprobante.Serie;
                doctoItem.Folio = invoiceComprobante.Folio;
                //doctoItem.MetodoDePagoDR = invoiceComprobante.MetodoPago; //Wesam8

                doctos.Add(doctoItem);

                number++;
            });

            if ((entityPM.AccountingPaymentMethodCode == "CC" || entityPM.AccountingPaymentMethodCode == "BT"
               || entityPM.AccountingPaymentMethodCode == "CH")
               && !string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef))
            {
                pagoItem.NumOperacion = StringHelper.TruncateLongString(entityPM.ChequeOrPaymentRef, 100);
            }

            pagoItem.DoctoRelacionado = doctos.ToArray();
            pagosPagoList.Add(pagoItem);


            pagos.Pago = pagosPagoList.ToArray();

            List<XmlElement> LXmlComplementos = new List<XmlElement>();
            System.Xml.Serialization.XmlSerializerNamespaces nsPagos = new System.Xml.Serialization.XmlSerializerNamespaces();
            nsPagos.Add("pago10", "http://www.sat.gob.mx/Pagos");
            string xmlPagos = Profact.TimbraCFDI.XMLUtilerias.SerializaObjeto(pagos, typeof(Profact.TimbraCFDI40.Complementos.Pagos20.Pagos), nsPagos);
            XmlDocument docNominas = new XmlDocument();
            docNominas.LoadXml(xmlPagos);
            comprobante.Pagos20Specified = true; //Wesam9
            LXmlComplementos.Add(docNominas.DocumentElement);

            comprobante.Complemento = new Profact.TimbraCFDI40.ComprobanteComplemento
            {
                Any = LXmlComplementos.ToArray<XmlElement>()
            };

            sATBaseProfact40Service.BuildProfactCommunicationLog40(new Profact40CommunicationLogArgs { Comprobante = comprobante, EntityReference = entityPM.PaymentNo.ToString(), IsCancellation = false, IsPayment = true });

            entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "TG";
            arPaymentRepository.Update(entityPoco);
            arPaymentRepository.SubmitChanges();


        }

        public void UpdateStatus()
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM entityPM = paymentQuery.GetSinglePM(arPpayment.Id, tenant);
            List<ARInvoice> currentPaymentARInvoices = GetCurrentPaymentARInvoices(entityPM);
            List<ARInvoiceSATDetails> currentPaymentInvoiceDetails = GetCurrentPaymentInvoicesDetails(currentPaymentARInvoices);
            List<PagosPagoDoctoRelacionado> doctos = GetPagosPagoDoctoRelacionados();

            foreach (PagosPagoDoctoRelacionado doctoItem in doctos)
            {
                UpdateRelatedARInvoiceStatus(currentPaymentInvoiceDetails, doctoItem);
            }

            arInvoiceRep.SubmitChanges();
        }

        private void UpdateRelatedARInvoiceStatus(List<ARInvoiceSATDetails> currentPaymentInvoiceDetails, PagosPagoDoctoRelacionado doctoItem)
        {
            ARInvoiceSATDetails relatedInvoice = currentPaymentInvoiceDetails.FirstOrDefault(d => d.UUID == doctoItem.IdDocumento);
            if (relatedInvoice == null) return;

            double previouslySentPaymentsTotal = GetPreviouslySentPaymentsTotal(relatedInvoice);
            //double previouslyCanceledSentPaymentsTotal = GetPreviouslyCanceledSentPaymentsTotal(relatedInvoice);
            
            if (previouslySentPaymentsTotal != 0)
            {
                relatedInvoice.Invoice.SATInvoiceStatusCode = relatedInvoice.Invoice.AmountInInvoiceCurrency.Value == previouslySentPaymentsTotal ? "PD" : "PP";
            }
            else
            {
                relatedInvoice.Invoice.SATInvoiceStatusCode = "OP";
            }

            arInvoiceRep.Update(relatedInvoice.Invoice);
        }

        private double GetPreviouslyCanceledSentPaymentsTotal(ARInvoiceSATDetails relatedInvoice)
        {

            List<ARInvoicePayment> sentVoidedInvoicePayments = (from a in arInvoiceRep.context.ARInvoicePayments
                                                                where a.ARInvoiceId == relatedInvoice.Invoice.Id && a.Tenant == relatedInvoice.Invoice.Tenant && !string.IsNullOrEmpty(a.ARPayment.SATXML)
                                                                && a.ARPayment.SATTransferStatusCode == "TD" && (a.ARPayment.StatusCode == "VD" || a.ARPayment.StatusCode == "DR" || a.ARPayment.StatusCode == "AC")
                                                                select a).ToList();

            double previouslyCanceledSentPaymentsTotal = 0;
            sentVoidedInvoicePayments.ForEach(p =>
            {
                previouslyCanceledSentPaymentsTotal += (p.ForeignAmount.Value);

            });

            return previouslyCanceledSentPaymentsTotal;
        }

        private double GetPreviouslySentPaymentsTotal(ARInvoiceSATDetails relatedInvoice)
        {
            List<ARInvoicePayment> sentApprovedInvoicePayments = (from a in arInvoiceRep.context.ARInvoicePayments
                                                                  where a.ARInvoiceId == relatedInvoice.Invoice.Id && a.Tenant == relatedInvoice.Invoice.Tenant && !string.IsNullOrEmpty(a.ARPayment.SATXML)
                                                                  && a.ARPayment.SATTransferStatusCode == "TD" && (a.ARPayment.StatusCode == "AD" || a.ARPayment.StatusCode == "CL")
                                                                  select a).ToList();


            double previouslySentPaymentsTotal = 0;

            sentApprovedInvoicePayments.ForEach(p =>
            {
                previouslySentPaymentsTotal += (p.ForeignAmount.Value);

            });
            return previouslySentPaymentsTotal;
        }

        private List<PagosPagoDoctoRelacionado> GetPagosPagoDoctoRelacionados()
        {
            XmlElement xmlPagos = comprobanteXmlPagos;
            Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Pagos>(xmlPagos.OuterXml);
            PagosPago pagoItem = pagos.Pago[0];
            List<PagosPagoDoctoRelacionado> doctos = pagoItem.DoctoRelacionado.ToList();
            
            return doctos;
        }

        private List<ARInvoice> GetCurrentPaymentARInvoices(ARPaymentPM entityPM)
        {
            List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> currentPaymentARInvoices = (from a in arInvoiceRep.context.ARInvoices
                                                        where a.Tenant == tenant && invoiceIds.Contains(a.Id) && a.SATXML != null
                                                        select a).ToList();
            return currentPaymentARInvoices;
        }

        private List<ARInvoiceSATDetails> GetCurrentPaymentInvoicesDetails(List<ARInvoice> currentPaymentARInvoices)
        {
            List<ARInvoiceSATDetails> paymentInvoicesDetails = new List<ARInvoiceSATDetails>();
            foreach (ARInvoice invoice in currentPaymentARInvoices)
            {
                AddARInvoiceToPaymentInvoicesDetails(paymentInvoicesDetails, invoice);
            }

            return paymentInvoicesDetails;
        }

        private static void AddARInvoiceToPaymentInvoicesDetails(List<ARInvoiceSATDetails> paymentInvoicesDetails, ARInvoice invoice)
        {
            Profact.TimbraCFDI40.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(invoice.SATXML);
            if (invoiceComprobante.Complemento.Any == null) return;

            List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            paymentInvoicesDetails.Add(new ARInvoiceSATDetails() { UUID = digitalTi.UUID, Invoice = invoice });

        }
    }
}
