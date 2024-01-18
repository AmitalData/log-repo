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
    public partial class CertificateOfOriginItemQueryService : EntityQueryService<CertificateOfOriginItem, CertificateOfOriginItemKeys, CertificateOfOriginItemPM, CertificateOfOriginPM, CertificateOfOriginKeys>
    {


        public List<CertificateOfOriginItemPM> GetCertificateOfOriginItemsByCertificateId(string certificateId, int tenant)
		{
			var CertificateOfOriginItemList = repository.GetCertificateOfOriginItemsByCertificateId(certificateId, tenant);
			List<CertificateOfOriginItemPM> CertificateOfOriginItemPMList = new List<CertificateOfOriginItemPM>();

			if (CertificateOfOriginItemList != null)
			{
				foreach (var item in CertificateOfOriginItemList)
				{
					CertificateOfOriginItemPMList.Add(this.GetEntityPM(item, true, new CertificateOfOriginItemKeys { Id = item.Id}));
				}
			}
			return CertificateOfOriginItemPMList;
		}
   }
}
