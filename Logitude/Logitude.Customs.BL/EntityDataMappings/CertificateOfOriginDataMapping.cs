
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CertificateOfOriginDataMapping: IMapping<CertificateOfOriginPM, CertificateOfOrigin>
   {

        public void CustomPMToPOCO(CertificateOfOriginPM entityPM, CertificateOfOrigin entityPOCO)
        {
            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
            {
                entityPOCO.Id = entityPM.Id;
                
            }
        }

        public void CustomPOCOToPM(CertificateOfOriginPM entityPM, CertificateOfOrigin entityPOCO)
        {
            UserQueryService UserQueryServiceRepository = new UserQueryService(entityPOCO.Tenant);
            User User = UserQueryServiceRepository.GetUserById(entityPM.OpenByUser, entityPOCO.Tenant);
            if(User != null)
            {
                entityPM.OpenByUserName = User.ExternalCode;
            }
			var certificateOfOriginTypeCodeEnumQueryService = new CertificateOfOriginTypeCodeEnumQueryService(entityPOCO.Tenant);
			var certificateOfOriginTypeCodeEnum = certificateOfOriginTypeCodeEnumQueryService.GetSingle(entityPOCO.CooTypeCode, false, true);
			if (certificateOfOriginTypeCodeEnum != null)
			{
				entityPM.CooTypeCodeName = certificateOfOriginTypeCodeEnum.LocalName;
			}
			var requestReasonCodeEnumQueryService = new RequestReasonCodeEnumQueryService(entityPOCO.Tenant);
			var requestReasonCodeEnum = requestReasonCodeEnumQueryService.GetSingle(entityPOCO.RequestReasonCode, false, true);
			if (requestReasonCodeEnum != null)
			{
				entityPM.RequestReasonCodeName = requestReasonCodeEnum.LocalName;
			}
			var certificateOfOriginStatusCodeEnumQueryService = new CertificateOfOriginStatusCodeEnumQueryService(entityPOCO.Tenant);
			var certificateOfOriginStatusCodeEnum = certificateOfOriginStatusCodeEnumQueryService.GetSingle(entityPOCO.CooStatusCode, false, true);
			if (certificateOfOriginStatusCodeEnum != null)
			{
				entityPM.CooStatusCodeName = certificateOfOriginStatusCodeEnum.LocalName;
			}


		
        }
   }


}
   