 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityMapping;
using System.Security.Policy;

namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CustomDocumentTypeQueryService
   {


        public List<CustomDocumentTypePM> GetMandatoryCustomDocumentTypes( int tenant)
        {
          var customDocumentTypes = repository.GetAll().Where(x => x.IsDiamondManadatory).ToList();

         return customDocumentTypes.Select(poco => this.GetEntityPM(poco)).ToList();

        }


        public List<CustomDocumentTypePM> GetMandatoryCustomDocumentTypesForCourier(int tenant)
        {
            var customDocumentTypes = repository.GetAll().Where(x => x.IsCourierManadatory).ToList();
            return customDocumentTypes.Select(poco => this.GetEntityPM(poco)).ToList();

        }

		//public List<CustomDocumentTypePM> GetCustomDocumentTypes(List<string> codes)
		//{
		//    var customDocumentTypes = repository.GetAll().Where(x => codes.Contains(x.Code)).ToList();
		//    return customDocumentTypes.Select(poco => this.GetEntityPM(poco)).ToList();

		//}
		public List<CustomDocumentTypePM> GetCustomDocumentTypesByTenant(int tenant)
		{		
			var customDocumentTypes = repository.GetAll().ToList();
			return customDocumentTypes.Select(poco => this.GetEntityPM(poco)).ToList();

		}

		public CustomDocumentTypePM GetSingleCustomDocumentTypeWithTenant(string code, int tenant)
		{

			string key = $"GetSingleCustomDocumentTypeWithTenant({code}, {tenant})";
			return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<CustomDocumentTypePM>(key, () =>
			{
				return GetSingleCustomDocumentTypeWithTenantReal(code, tenant);
			});
		}
		public CustomDocumentTypePM GetSingleCustomDocumentTypeWithTenantReal(string code, int tenant)
		{
			CustomDocumentTypePM customDocumentType = null;

			if (!string.IsNullOrWhiteSpace(code))
			{
				CustomDocumentType CustomDocumentType = repository.GetSingleCustomDocumentType(new CustomDocumentTypeKeys() { Code = code });

				CustomDocumentTypeTenantRepository documentTypeTenantRep = new CustomDocumentTypeTenantRepository(context);
				CustomDocumentTypeTenant customDocumentTypeTenant = documentTypeTenantRep.GetSingleByCode(code, tenant);
				if (CustomDocumentType != null)
				{
					customDocumentType = new CustomDocumentTypePM()
					{
						Code = CustomDocumentType.Code,
						EnglishName = CustomDocumentType.EnglishName,
						LocalName = CustomDocumentType.LocalName,		
						PointerLevel = CustomDocumentType.PointerLevel,									
						AutoSetOriginalDocumentTrue = CustomDocumentType.AutoSetOriginalDocumentTrue,							
						IsCourierManadatory = CustomDocumentType.IsCourierManadatory,							
						IsDiamondManadatory = CustomDocumentType.IsDiamondManadatory,
						CustomsDocumentUpload = CustomDocumentType.CustomsDocumentUpload,
					
				    };
					
					if (customDocumentTypeTenant != null)
					{
						customDocumentType.PointerLevel = customDocumentTypeTenant.PointerLevel;
						customDocumentType.AutoSetOriginalDocumentTrue = customDocumentTypeTenant.AutoSetOriginalDocumentTrue;
						customDocumentType.IsCourierManadatory = customDocumentTypeTenant.IsCourierManadatory;
						customDocumentType.IsDiamondManadatory = customDocumentTypeTenant.IsDiamondManadatory;
						customDocumentType.CustomsDocumentUpload = customDocumentTypeTenant.CustomsDocumentUpload;
						customDocumentType.Tenant = tenant;
					}
				}
			}
			return customDocumentType;
		}

   }

}
	 