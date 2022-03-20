using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base
{
    internal class Emisor
    {
        public static ComprobanteEmisor Get(Tenant currentTenant)
        {
            const string GeneralLegalPersonsLawTaxRegime = "601";
            return new ComprobanteEmisor
            {
                Rfc = currentTenant.VatNumber,
                Nombre = currentTenant.Company,
                RegimenFiscal = GeneralLegalPersonsLawTaxRegime
            };
        }
    }
}
