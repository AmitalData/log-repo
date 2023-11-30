using Profact.TimbraCFDI40.Complementos.Pagos20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoImpuestosPs : ComplementoPagosPago
    {
        private const string taxCode = "002";
        private static decimal paymentTaxAmount; 
        public static List<PagosPagoImpuestosP> Get(PagosPago pagoItem)
        {
            paymentTaxAmount = pagoItem.TipoCambioP;
            List<PagosPagoImpuestosP> impuestosPs = new List<PagosPagoImpuestosP>();
            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();
            pagoItem.DoctoRelacionado.ToList().ForEach(d =>
            {
                pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = GetPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs, d);
                pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = GetPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs, d);
            });

            PagosPagoImpuestosP pagosPagoImpuestosP = new PagosPagoImpuestosP();
            List<PagosPagoImpuestosPTrasladoP> trasladosP = GetPagosPagoImpuestosPTrasladoPs(pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs);
            List<PagosPagoImpuestosPRetencionP> retencionesP = GetPagosPagoImpuestosPRetencionPs(pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs);

            if (trasladosP.Count > 0) pagosPagoImpuestosP.TrasladosP = trasladosP.ToArray();
            if (retencionesP.Count > 0) pagosPagoImpuestosP.RetencionesP = retencionesP.ToArray();

            impuestosPs.Add(pagosPagoImpuestosP);
            return impuestosPs;
        }

        private static List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> GetPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs, PagosPagoDoctoRelacionado pagosPagoDoctoRelacionado)
        {
            if(pagosPagoDoctoRelacionado.ImpuestosDR.TrasladosDR == null)
                return pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs;

            decimal invoiceTaxAmount = pagosPagoDoctoRelacionado.EquivalenciaDR;

            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> impuestosDRTrasladoDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();


            pagosPagoDoctoRelacionado.ImpuestosDR.TrasladosDR.ToList().ForEach(pTDR =>
            {
                AddTrasladoDRWithTax(pTDR, invoiceTaxAmount, impuestosDRTrasladoDRs);
            });

            return pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.Concat(impuestosDRTrasladoDRs.ToArray()).ToList();
        }

        private static void AddTrasladoDRWithTax(PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR impuestosDRTrasladoDR, decimal invoiceTaxAmount, List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> impuestosDRTrasladoDRs)
        {
            PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR impuestosDRTrasladoDRCloner = CloneImpuestosDRTrasladoDR(impuestosDRTrasladoDR);
            CalculateTrasladoDRWithTax(impuestosDRTrasladoDRCloner, invoiceTaxAmount);
            impuestosDRTrasladoDRs.Add(impuestosDRTrasladoDRCloner);
        }

        private static PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR CloneImpuestosDRTrasladoDR(PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR impuestosDRTrasladoDR)
        {
            return new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
            {
                BaseDR = impuestosDRTrasladoDR.BaseDR,
                ImporteDR = impuestosDRTrasladoDR.ImporteDR,
                ImporteDRSpecified = impuestosDRTrasladoDR.ImporteDRSpecified,
                ImpuestoDR = impuestosDRTrasladoDR.ImpuestoDR,
                TasaOCuotaDR = impuestosDRTrasladoDR.TasaOCuotaDR,
                TasaOCuotaDRSpecified = impuestosDRTrasladoDR.TasaOCuotaDRSpecified,
                TipoFactorDR = impuestosDRTrasladoDR.TipoFactorDR,
            };
        }

        private static void CalculateTrasladoDRWithTax(PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR impuestosDRTrasladoDR, decimal invoiceTaxAmount)
        {
            impuestosDRTrasladoDR.BaseDR /= invoiceTaxAmount;
            impuestosDRTrasladoDR.ImporteDR /= invoiceTaxAmount;
            BuildTrasladoPagosTotales(impuestosDRTrasladoDR);
        }

        private static List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> GetPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs, PagosPagoDoctoRelacionado pagosPagoDoctoRelacionado)
        {
            if (pagosPagoDoctoRelacionado.ImpuestosDR.RetencionesDR == null)
                return pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs;

            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> impuestosDRRetencionDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();
            decimal invoiceTaxAmount = pagosPagoDoctoRelacionado.EquivalenciaDR;
            pagosPagoDoctoRelacionado.ImpuestosDR.RetencionesDR.ToList().ForEach(pRDR =>
            {
                AddRetencionDRWithTax(pRDR, invoiceTaxAmount, impuestosDRRetencionDRs);
            });

            return pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.Concat(impuestosDRRetencionDRs.ToArray()).ToList();
        }

        private static void AddRetencionDRWithTax(PagosPagoDoctoRelacionadoImpuestosDRRetencionDR impuestosDRRetencionDR, decimal invoiceTaxAmount, List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> impuestosDRRetencionDRs)
        {
            PagosPagoDoctoRelacionadoImpuestosDRRetencionDR impuestosDRRetencionDRCloner = CloneImpuestosDRRetencionDR(impuestosDRRetencionDR);
            CalculateRetencionDRWithTax(impuestosDRRetencionDRCloner, invoiceTaxAmount);
            impuestosDRRetencionDRs.Add(impuestosDRRetencionDRCloner);
        }

        private static PagosPagoDoctoRelacionadoImpuestosDRRetencionDR CloneImpuestosDRRetencionDR(PagosPagoDoctoRelacionadoImpuestosDRRetencionDR impuestosDRRetencionDR)
        {
            return new PagosPagoDoctoRelacionadoImpuestosDRRetencionDR
            {
                BaseDR = impuestosDRRetencionDR.BaseDR,
                ImporteDR = impuestosDRRetencionDR.ImporteDR,
                ImpuestoDR = impuestosDRRetencionDR.ImpuestoDR,
                TasaOCuotaDR = impuestosDRRetencionDR.TasaOCuotaDR,
                TipoFactorDR = impuestosDRRetencionDR.TipoFactorDR,
            };
        }

        private static void CalculateRetencionDRWithTax(PagosPagoDoctoRelacionadoImpuestosDRRetencionDR impuestosDRRetencionDR, decimal invoiceTaxAmount)
        {
            impuestosDRRetencionDR.BaseDR /= invoiceTaxAmount;
            impuestosDRRetencionDR.ImporteDR /= invoiceTaxAmount;
            BuildRetencionPagosTotales(impuestosDRRetencionDR);
        }

        private static List<PagosPagoImpuestosPTrasladoP> GetPagosPagoImpuestosPTrasladoPs(List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs)
        {
            List<PagosPagoImpuestosPTrasladoP> trasladosP = new List<PagosPagoImpuestosPTrasladoP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.GroupBy(x => x.TasaOCuotaDR.ToString());
            foreach (var group in groups)
            {
                PagosPagoImpuestosPTrasladoP pagosPagoImpuestosPTrasladoP = GetNewPagosPagoImpuestosPTrasladoP(group);
                trasladosP.Add(pagosPagoImpuestosPTrasladoP);
            }

            SetTrasladoPagosTotalesSpecified();
            return trasladosP;
        }

        private static void BuildTrasladoPagosTotales(PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR)
        {
            switch (pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.TasaOCuotaDR.ToString())
            {
                case "0.160000":
                    pagos.Totales.TotalTrasladosBaseIVA16 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR;
                    pagos.Totales.TotalTrasladosImpuestoIVA16 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR;
                    break;
                case "0.080000":
                    pagos.Totales.TotalTrasladosBaseIVA8 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR;
                    pagos.Totales.TotalTrasladosImpuestoIVA8 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR;
                    break;
                case "0.000000":case "0":
                    if (pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.TipoFactorDR == "Exento")
                    {
                        pagos.Totales.TotalTrasladosBaseIVAExento += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR;
                    }
                    else
                    {
                        pagos.Totales.TotalTrasladosBaseIVA0 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR;
                        pagos.Totales.TotalTrasladosImpuestoIVA0 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR;
                    } break;
            }
        }

        private static void SetTrasladoPagosTotalesSpecified()
        {
            if(pagos.Totales.TotalTrasladosBaseIVA16 > 0)
            {
                SetTotalTrasladosIVA16();
            }
            if (pagos.Totales.TotalTrasladosBaseIVA8 > 0)
            {
                SetTotalTrasladosIVA8();
            }
            if (pagos.Totales.TotalTrasladosBaseIVA0 > 0)
            {
                SetTotalTrasladosIVA0();
            }
            if (pagos.Totales.TotalTrasladosBaseIVAExento > 0)
            {
                SetTotalTrasladosIVAExento();
            }
        }

        private static void SetTotalTrasladosIVAExento()
        {
            pagos.Totales.TotalTrasladosBaseIVAExento = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVAExento * paymentTaxAmount);
            pagos.Totales.TotalTrasladosBaseIVAExentoSpecified = true;
        }

        private static void SetTotalTrasladosIVA0()
        {
            pagos.Totales.TotalTrasladosBaseIVA0 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA0 * paymentTaxAmount); 
            pagos.Totales.TotalTrasladosImpuestoIVA0 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA0 * paymentTaxAmount);
            pagos.Totales.TotalTrasladosBaseIVA0Specified = true;
            pagos.Totales.TotalTrasladosImpuestoIVA0Specified = true;
        }

        private static void SetTotalTrasladosIVA8()
        {
            pagos.Totales.TotalTrasladosBaseIVA8 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA8 * paymentTaxAmount);
            pagos.Totales.TotalTrasladosImpuestoIVA8 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA8 * paymentTaxAmount);
            pagos.Totales.TotalTrasladosBaseIVA8Specified = true;
            pagos.Totales.TotalTrasladosImpuestoIVA8Specified = true;
        }

        private static void SetTotalTrasladosIVA16()
        {
            pagos.Totales.TotalTrasladosBaseIVA16 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA16 * paymentTaxAmount);
            pagos.Totales.TotalTrasladosImpuestoIVA16 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA16 * paymentTaxAmount);
            pagos.Totales.TotalTrasladosBaseIVA16Specified = true;
            pagos.Totales.TotalTrasladosImpuestoIVA16Specified = true;
        }

        private static PagosPagoImpuestosPTrasladoP GetNewPagosPagoImpuestosPTrasladoP(IGrouping<string, PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> group)
        {
            decimal totalBaseDR = 0;
            decimal totalImporteDR = 0;
            foreach (var product in group)
            {
                totalBaseDR += product.BaseDR;
                totalImporteDR += product.ImporteDR;
            }
            PagosPagoImpuestosPTrasladoP pagosPagoImpuestosPTrasladoP = new PagosPagoImpuestosPTrasladoP
            {
                BaseP = SATBaseProfact40Service.TruncateDecimalWithNDigitsAfterPoint(totalBaseDR, 6),
                ImpuestoP = taxCode,
                TipoFactorP = "Exento"
            };

            if (group.First().TipoFactorDR == "Exento") return pagosPagoImpuestosPTrasladoP;

            pagosPagoImpuestosPTrasladoP.ImporteP = SATBaseProfact40Service.TruncateDecimalWithNDigitsAfterPoint(totalImporteDR, 6);
            pagosPagoImpuestosPTrasladoP.TasaOCuotaP = Convert.ToDecimal(group.Key);
            pagosPagoImpuestosPTrasladoP.TipoFactorP = "Tasa";
            pagosPagoImpuestosPTrasladoP.ImportePSpecified = true;
            pagosPagoImpuestosPTrasladoP.TasaOCuotaPSpecified = true;

            return pagosPagoImpuestosPTrasladoP;
        }

        private static List<PagosPagoImpuestosPRetencionP> GetPagosPagoImpuestosPRetencionPs(List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs)
        {
            List<PagosPagoImpuestosPRetencionP> retencionesP = new List<PagosPagoImpuestosPRetencionP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.GroupBy(x => x.TasaOCuotaDR.ToString());
            foreach (var group in groups)
            {
                PagosPagoImpuestosPRetencionP pagoImpuestosPRetencionP = GetNewPagosPagoImpuestosPRetencionP(group);
                retencionesP.Add(pagoImpuestosPRetencionP);
            }

            SetRetencionPagosTotalesSpecified();
            return retencionesP;
        }

        private static PagosPagoImpuestosPRetencionP GetNewPagosPagoImpuestosPRetencionP(IGrouping<string, PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> group)
        {
            decimal totalImporteDR = 0;
            foreach (var product in group)
            {
                totalImporteDR += product.ImporteDR;
            }

            return new PagosPagoImpuestosPRetencionP
            {
                ImporteP = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(totalImporteDR),
                ImpuestoP = taxCode,
            };
        }

        private static void BuildRetencionPagosTotales(PagosPagoDoctoRelacionadoImpuestosDRRetencionDR pagosPagoDoctoRelacionadoImpuestosDRRetencionDR)
        {
            pagos.Totales.TotalRetencionesIVA += pagosPagoDoctoRelacionadoImpuestosDRRetencionDR.ImporteDR;
        }

        private static void SetRetencionPagosTotalesSpecified()
        {
            if (pagos.Totales.TotalRetencionesIVA > 0)
            {
                pagos.Totales.TotalRetencionesIVA = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalRetencionesIVA * paymentTaxAmount);
                pagos.Totales.TotalRetencionesIVASpecified = true;
            }
        }
    }
}
