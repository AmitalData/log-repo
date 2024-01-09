using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;


namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CertificateOfOriginQueryService : EntityQueryService<CertificateOfOrigin, CertificateOfOriginKeys, CertificateOfOriginPM, object, CertificateOfOriginKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, CertificateOfOriginPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CertificateOfOriginKeys CertificateOfOriginKeys = entityKeys as CertificateOfOriginKeys;
           
			CertificateOfOriginInvoiceQueryService certificateOfOriginInvoiceQueryService = new CertificateOfOriginInvoiceQueryService(context);
			entityPM.CertificateOriginInvoiceItems = certificateOfOriginInvoiceQueryService.GetMulti(CertificateOfOriginKeys, false);

			CertificateOfOriginItemQueryService certificateOfOriginItemQueryService = new CertificateOfOriginItemQueryService(context);
			entityPM.CertificateOriginItemItems = certificateOfOriginItemQueryService.GetMulti(CertificateOfOriginKeys, false);

		}

	}
}
