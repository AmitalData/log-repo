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
                pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = d.ImpuestosDR.TrasladosDR != null ? pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.Concat(d.ImpuestosDR.TrasladosDR.ToArray()).ToList() : pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs;
                pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = d.ImpuestosDR.RetencionesDR != null ? pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.Concat(d.ImpuestosDR.RetencionesDR.ToArray()).ToList() : pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs;
                BuildPagosTotales(d);
            });

            PagosPagoImpuestosP pagosPagoImpuestosP = new PagosPagoImpuestosP();
            List<PagosPagoImpuestosPTrasladoP> trasladosP = GetPagosPagoImpuestosPTrasladoPs(pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs);
            List<PagosPagoImpuestosPRetencionP> retencionesP = GetPagosPagoImpuestosPRetencionPs(pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs);

            if (trasladosP.Count > 0) pagosPagoImpuestosP.TrasladosP = trasladosP.ToArray();
            if (retencionesP.Count > 0) pagosPagoImpuestosP.RetencionesP = retencionesP.ToArray();

            impuestosPs.Add(pagosPagoImpuestosP);
            return impuestosPs;
        }

        private static void BuildPagosTotales(PagosPagoDoctoRelacionado pagosPagoDoctoRelacionado)
        {
            decimal invoiceTaxAmount = pagosPagoDoctoRelacionado.EquivalenciaDR;

            pagosPagoDoctoRelacionado?.ImpuestosDR?.TrasladosDR?.ToList()?.ForEach(pTDR => {
                BuildTrasladoPagosTotales(pTDR, invoiceTaxAmount);
            });

            pagosPagoDoctoRelacionado?.ImpuestosDR?.RetencionesDR?.ToList()?.ForEach(pRDR => {
                BuildRetencionPagosTotales(pRDR, invoiceTaxAmount);
            });
        }

        private static List<PagosPagoImpuestosPTrasladoP> GetPagosPagoImpuestosPTrasladoPs(List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs)
        {
            List<PagosPagoImpuestosPTrasladoP> trasladosP = new List<PagosPagoImpuestosPTrasladoP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.GroupBy(x => x.TasaOCuotaDR);
            foreach (var group in groups)
            {
                PagosPagoImpuestosPTrasladoP pagosPagoImpuestosPTrasladoP = GetNewPagosPagoImpuestosPTrasladoP(group);
                trasladosP.Add(pagosPagoImpuestosPTrasladoP);
            }

            SetTrasladoPagosTotalesSpecified();
            return trasladosP;
        }

        private static void BuildTrasladoPagosTotales(PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR, decimal invoiceTaxAmount)
        {
            switch (pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.TasaOCuotaDR.ToString())
            {
                case "0.160000":
                    pagos.Totales.TotalTrasladosBaseIVA16 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR * invoiceTaxAmount;
                    pagos.Totales.TotalTrasladosImpuestoIVA16 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR * invoiceTaxAmount;
                    break;
                case "0.080000":
                    pagos.Totales.TotalTrasladosBaseIVA8 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR * invoiceTaxAmount;
                    pagos.Totales.TotalTrasladosImpuestoIVA8 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR * invoiceTaxAmount;
                    break;
                case "0.000000":case "0":
                    if (pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.TipoFactorDR == "Exento")
                    {
                        pagos.Totales.TotalTrasladosBaseIVAExento += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR * invoiceTaxAmount;
                    }
                    else
                    {
                        pagos.Totales.TotalTrasladosBaseIVA0 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.BaseDR * invoiceTaxAmount;
                        pagos.Totales.TotalTrasladosImpuestoIVA0 += pagosPagoDoctoRelacionadoImpuestosDRTrasladoDR.ImporteDR * invoiceTaxAmount;
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

        private static PagosPagoImpuestosPTrasladoP GetNewPagosPagoImpuestosPTrasladoP(IGrouping<decimal, PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> group)
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
                BaseP = totalBaseDR,
                ImpuestoP = taxCode,
                TipoFactorP = "Exento"
            };

            if (group.First().TipoFactorDR == "Exento") return pagosPagoImpuestosPTrasladoP;

            pagosPagoImpuestosPTrasladoP.ImporteP = totalImporteDR;
            pagosPagoImpuestosPTrasladoP.TasaOCuotaP = group.Key;
            pagosPagoImpuestosPTrasladoP.TipoFactorP = "Tasa";

            return pagosPagoImpuestosPTrasladoP;
        }

        private static List<PagosPagoImpuestosPRetencionP> GetPagosPagoImpuestosPRetencionPs(List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs)
        {
            List<PagosPagoImpuestosPRetencionP> retencionesP = new List<PagosPagoImpuestosPRetencionP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.GroupBy(x => x.TasaOCuotaDR);
            foreach (var group in groups)
            {
                PagosPagoImpuestosPRetencionP pagoImpuestosPRetencionP = GetNewPagosPagoImpuestosPRetencionP(group);
                retencionesP.Add(pagoImpuestosPRetencionP);
            }

            SetRetencionPagosTotalesSpecified();
            return retencionesP;
        }

        private static PagosPagoImpuestosPRetencionP GetNewPagosPagoImpuestosPRetencionP(IGrouping<decimal, PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> group)
        {
            decimal totalImporteDR = 0;
            foreach (var product in group)
            {
                totalImporteDR += product.ImporteDR;
            }

            return new PagosPagoImpuestosPRetencionP
            {
                ImporteP = totalImporteDR,
                ImpuestoP = taxCode,
            };
        }

        private static void BuildRetencionPagosTotales(PagosPagoDoctoRelacionadoImpuestosDRRetencionDR pagosPagoDoctoRelacionadoImpuestosDRRetencionDR, decimal invoiceTaxAmount)
        {
            pagos.Totales.TotalRetencionesIVA += pagosPagoDoctoRelacionadoImpuestosDRRetencionDR.ImporteDR * invoiceTaxAmount;
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
