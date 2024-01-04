using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;


namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CertificateOfOriginInvoiceUpdateService
	{

	


		protected override void UpdateComposition(CertificateOfOriginInvoicePM entityPM)
        {
			CertificateOfOriginItemUpdateService certificateOfOriginItemUpdateService = new CertificateOfOriginItemUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
			certificateOfOriginItemUpdateService.UpdateMulti(entityPM.CertificateOriginItemItems, entityPM.DeletedCertificateOriginItemItems, entityPM, false);
            base.UpdateComposition(entityPM);
        }

    }
}
