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
	public partial class CertificateOfOriginInvoiceQueryService : EntityQueryService<CertificateOfOriginInvoice, CertificateOfOriginInvoiceKeys, CertificateOfOriginInvoicePM, CertificateOfOriginPM, CertificateOfOriginKeys>
	{
		public override void GetComposition(EntityKeyFields entityKeys, CertificateOfOriginInvoicePM entityPM)
		{
			ICustomContext context = MainContext as CustomContext;
			CertificateOfOriginInvoiceKeys CertificateOfOriginInvoiceKeys = entityKeys as CertificateOfOriginInvoiceKeys;

			CertificateOfOriginItemQueryService certificateOfOriginItemQueryService = new CertificateOfOriginItemQueryService(context);
			entityPM.CertificateOriginItemItems = certificateOfOriginItemQueryService.GetMulti(CertificateOfOriginInvoiceKeys, false);

		}

	}
}
