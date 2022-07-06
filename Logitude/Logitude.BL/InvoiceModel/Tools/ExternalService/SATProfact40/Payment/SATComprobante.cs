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
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base;
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATComprobante
    {
        public static ARPaymentPM arPaymentPM;
        public static IInvoiceContext invoiceContext;
        public static ICommonDataContext commonContext;
        private SATInterfaceSetting satSetting;
        public SATComprobante()
        {

        }

        public SATComprobante(ARPaymentPM aRPaymentPM, SATInterfaceSetting satSetting)
        {
            arPaymentPM = aRPaymentPM;
            this.satSetting = satSetting;
            InitalizeContexts(arPaymentPM.Tenant);
        }

        private void InitalizeContexts(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
        }

        public Comprobante Get()
        {
            string arPaymentCounterPrefix = TableCounter.GetCounterPrefix(arPaymentPM.Tenant, "ARPT", "DR", null);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            Tenant currentTenant = tenantRepository.GetSingleTenant(arPaymentPM.Tenant);

            Comprobante comprobante = new Comprobante
            {
                Version = SATData.CurrentComprobanteVersion,
                Exportacion = SATData.ComprobanteExportacion,
                CfdiRelacionados = CfdiRelacionados.Get(),
                Total = 0,
                SubTotal = 0,
                TipoDeComprobante = SATData.PaymentReceiptType,
                Moneda = SATData.PaymentCurrency,
                Serie = Serie.Get(arPaymentCounterPrefix),
                Folio = Folio.Get(arPaymentPM.PaymentNo, arPaymentCounterPrefix),
                Fecha = Fecha.Get(),
                LugarExpedicion = LugarExpedicion.Get(new LugarExpedicionArgs { CommonContext = commonContext, BranchId = arPaymentPM.BranchId, CurrentTenantZipCode = currentTenant.Address.ZipCode, Tenant = arPaymentPM.Tenant }),
                Emisor = Emisor.Get(currentTenant, satSetting),
                Receptor = Receptor.Get(currentTenant),
                Conceptos = Conceptos.Get(),
                Complemento = Complemento.Get(currentTenant),
                Pagos20Specified = true,
            };

            comprobante.InformacionGlobal = GetInformacionGlobal(comprobante.Receptor);

            return comprobante;
        }

        private ComprobanteInformacionGlobal GetInformacionGlobal(ComprobanteReceptor comprobanteReceptor)
        {
            bool isPublicInGeneral = comprobanteReceptor.Nombre == SATData.PublicInGeneralNombre;
            if (!isPublicInGeneral) return null;

            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(arPaymentPM.Tenant);
            const string dailyPeriodCode = "01";
            return new ComprobanteInformacionGlobal
            {
                Periodicidad = dailyPeriodCode,
                Meses = currentDateTime.Month.ToString().PadLeft(2, '0'),
                Año = Convert.ToInt16(currentDateTime.Year),
            };
        }
    }
}
