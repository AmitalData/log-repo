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
using Logitude.BL.CommonDataModel.EntityQueries;
using Microsoft.Practices.Unity;
using System.IO;



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
		public CertificateOfOriginPM GetCertificateOfOriginByCounter(string Counter, int tenant=0)
		{
			 var certificateOfOrigin = repository.GetCertificateOfOriginByCounter(Counter, tenant);
			CertificateOfOriginPM certificateOfOriginPM = null;
			if (certificateOfOrigin != null)
			{
				certificateOfOriginPM = this.GetEntityPM(certificateOfOrigin, false, null);			
			}
			return certificateOfOriginPM;
		}
		public List<CertificateOfOriginPM> GetCertificateOfOriginsByDeclarationId(string declarationId, string amendmentOriginalDeclartation, int tenant,bool IsFromUI = false)
		{
			ICustomContext context = MainContext as CustomContext;

			DocumentsFilingQuery documentsFilingQueryService = new DocumentsFilingQuery(tenant);
			var decId = string.IsNullOrEmpty(amendmentOriginalDeclartation) ? declarationId : amendmentOriginalDeclartation;


			var certificateOfOriginList = repository.GetCertificateOfOriginsByDeclarationId(decId, tenant);
			List<CertificateOfOriginPM> certificateOfOriginPMList = new List<CertificateOfOriginPM>();

			if (certificateOfOriginList != null)
			{
				foreach (var item in certificateOfOriginList)
				{
					var certificateOfOriginPM = this.GetEntityPM(item, false, new CertificateOfOriginKeys { Id = item.Id });
					if (IsFromUI) 
					{ 
					  certificateOfOriginPM.CertificateOriginDocuments = documentsFilingQueryService.GetCOOEDocument(decId, item.Id, tenant);
					}
					certificateOfOriginPMList.Add(certificateOfOriginPM);

				}
			}
			return certificateOfOriginPMList;
		}

        public CertificateOfOriginPM GetCertificateOfOriginsByDeclarationIdIncludeChildrens(string certificateId, string declarationId, int tenant)
        {
            CertificateOfOrigin certificateOfOrigin = repository.GetCertificateOfOriginsByDeclarationIdIncludeChildrens(certificateId, declarationId, tenant);
            CertificateOfOriginPM certificateOfOriginPM = new CertificateOfOriginPM();

            if (certificateOfOrigin != null)
            {
                certificateOfOriginPM = this.GetEntityPM(certificateOfOrigin, true, new CertificateOfOriginKeys { Id = certificateOfOrigin.Id });
            }
            return certificateOfOriginPM;
        }

        public Dictionary<string, byte[]> GetToolTipFromStorage()
        {
			if (LogitudeSettings.StorageServiceMode == "Azure")
			{
				Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
				Dictionary<string, byte[]> ArryByte = storageservice.ReadAllFilesInFolder("tenant0", "CertificateOfOrigin");
				return ArryByte;
			}
			return null;
        }

    }
}
