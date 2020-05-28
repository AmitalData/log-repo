using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;

using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   public partial class APInvoiceLineQueryService
    {
        
        public List<APInvoiceLine> APInvoiceLineCustomDataMapping(APInvoicePM myEntityPM, List<APInvoiceLinePM> invoiceLines, int tenant)
        {
            return null;
        }

        public List<APInvoiceLinePM> APInvoiceLineCustomDataMappingAndValidatin(APInvoice myEntity, List<APInvoiceLine> invoiceLines, int tenant, string computingPartnerName)
        {
            throw new NotImplementedException();
        }
    }
}
