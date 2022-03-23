using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base
{
    internal class Emisor
    {
        public static ComprobanteEmisor Get(Tenant currentTenant, SATInterfaceSetting satSetting)
        {
            const string GeneralLegalPersonsLawTaxRegime = "601";
            return new ComprobanteEmisor
            {
                Rfc = currentTenant.VatNumber,
                Nombre = GetSATCompanyName(currentTenant, satSetting),
                RegimenFiscal = GeneralLegalPersonsLawTaxRegime
            };
        }

        private static string GetSATCompanyName(Tenant currentTenant, SATInterfaceSetting satSetting)
        {
            if (satSetting != null && !String.IsNullOrEmpty(satSetting.SATCompanyName)) 
                return satSetting.SATCompanyName;

            return currentTenant.Company;
        }
    }
}
