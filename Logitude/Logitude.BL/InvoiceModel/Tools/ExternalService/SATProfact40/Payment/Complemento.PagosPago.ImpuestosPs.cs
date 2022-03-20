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
        private static decimal taxValue; 
        public static List<PagosPagoImpuestosP> Get(PagosPago pagoItem)
        {
            taxValue = pagoItem.TipoCambioP;
            List<PagosPagoImpuestosP> impuestosPs = new List<PagosPagoImpuestosP>();
            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();
            pagoItem.DoctoRelacionado.ToList().ForEach(d =>
            {
                pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs = d.ImpuestosDR.TrasladosDR != null ? pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.Concat(d.ImpuestosDR.TrasladosDR.ToArray()).ToList() : pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs;
                pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs = d.ImpuestosDR.RetencionesDR != null ? pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs.Concat(d.ImpuestosDR.RetencionesDR.ToArray()).ToList() : pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs;
            });

            PagosPagoImpuestosP pagosPagoImpuestosP = new PagosPagoImpuestosP();
            List<PagosPagoImpuestosPTrasladoP> trasladosP = GetPagosPagoImpuestosPTrasladoPs(pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs);
            List<PagosPagoImpuestosPRetencionP> retencionesP = GetPagosPagoImpuestosPRetencionPs(pagosPagoDoctoRelacionadoImpuestosDRRetencionDRs);

            if (trasladosP.Count > 0) pagosPagoImpuestosP.TrasladosP = trasladosP.ToArray();
            if (retencionesP.Count > 0) pagosPagoImpuestosP.RetencionesP = retencionesP.ToArray();

            impuestosPs.Add(pagosPagoImpuestosP);
            return impuestosPs;
        }

        private static List<PagosPagoImpuestosPTrasladoP> GetPagosPagoImpuestosPTrasladoPs(List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs)
        {
            List<PagosPagoImpuestosPTrasladoP> trasladosP = new List<PagosPagoImpuestosPTrasladoP>();
            var groups = pagosPagoDoctoRelacionadoImpuestosDRTrasladoDRs.GroupBy(x => x.TasaOCuotaDR);
            foreach (var group in groups)
            {
                PagosPagoImpuestosPTrasladoP pagosPagoImpuestosPTrasladoP = GetNewPagosPagoImpuestosPTrasladoP(group);
                BuildTrasladoPagosTotales(pagosPagoImpuestosPTrasladoP);
                trasladosP.Add(pagosPagoImpuestosPTrasladoP);
            }

            SetTrasladoPagosTotalesSpecified();
            return trasladosP;
        }

        private static void BuildTrasladoPagosTotales(PagosPagoImpuestosPTrasladoP pagosPagoImpuestosPTrasladoP)
        {
            switch (pagosPagoImpuestosPTrasladoP.TasaOCuotaP.ToString())
            {
                case "0.160000":
                    pagos.Totales.TotalTrasladosBaseIVA16 += pagosPagoImpuestosPTrasladoP.BaseP;
                    pagos.Totales.TotalTrasladosImpuestoIVA16 += pagosPagoImpuestosPTrasladoP.ImporteP;
                    break;
                case "0.080000":
                    pagos.Totales.TotalTrasladosBaseIVA8 += pagosPagoImpuestosPTrasladoP.BaseP;
                    pagos.Totales.TotalTrasladosImpuestoIVA8 += pagosPagoImpuestosPTrasladoP.ImporteP;
                    break;
                case "0.000000":case "0":
                    if (pagosPagoImpuestosPTrasladoP.TipoFactorP == "Exento")
                    {
                        pagos.Totales.TotalTrasladosBaseIVAExento += pagosPagoImpuestosPTrasladoP.BaseP;
                    }
                    else
                    {
                        pagos.Totales.TotalTrasladosBaseIVA0 += pagosPagoImpuestosPTrasladoP.BaseP;
                        pagos.Totales.TotalTrasladosImpuestoIVA0 += pagosPagoImpuestosPTrasladoP.ImporteP;
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
            pagos.Totales.TotalTrasladosBaseIVAExento = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVAExento * taxValue);
            pagos.Totales.TotalTrasladosBaseIVAExentoSpecified = true;
        }

        private static void SetTotalTrasladosIVA0()
        {
            pagos.Totales.TotalTrasladosBaseIVA0 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA0 * taxValue); 
            pagos.Totales.TotalTrasladosImpuestoIVA0 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA0 * taxValue);
            pagos.Totales.TotalTrasladosBaseIVA0Specified = true;
            pagos.Totales.TotalTrasladosImpuestoIVA0Specified = true;
        }

        private static void SetTotalTrasladosIVA8()
        {
            pagos.Totales.TotalTrasladosBaseIVA8 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA8 * taxValue);
            pagos.Totales.TotalTrasladosImpuestoIVA8 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA8 * taxValue);
            pagos.Totales.TotalTrasladosBaseIVA8Specified = true;
            pagos.Totales.TotalTrasladosImpuestoIVA8Specified = true;
        }

        private static void SetTotalTrasladosIVA16()
        {
            pagos.Totales.TotalTrasladosBaseIVA16 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosBaseIVA16 * taxValue);
            pagos.Totales.TotalTrasladosImpuestoIVA16 = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalTrasladosImpuestoIVA16 * taxValue);
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
                BuildRetencionPagosTotales(pagoImpuestosPRetencionP);
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

        private static void BuildRetencionPagosTotales(PagosPagoImpuestosPRetencionP pagosPagoImpuestosPRetencionP)
        {
            pagos.Totales.TotalRetencionesIVA += pagosPagoImpuestosPRetencionP.ImporteP;
        }

        private static void SetRetencionPagosTotalesSpecified()
        {
            if (pagos.Totales.TotalRetencionesIVA > 0)
            {
                pagos.Totales.TotalRetencionesIVA = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagos.Totales.TotalRetencionesIVA * taxValue);
                pagos.Totales.TotalRetencionesIVASpecified = true;
            }
        }
    }
}
