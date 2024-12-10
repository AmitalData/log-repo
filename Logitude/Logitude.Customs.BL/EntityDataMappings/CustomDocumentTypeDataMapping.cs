
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
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomDocumentTypeDataMapping: IMapping<CustomDocumentTypePM, CustomDocumentType>
   {

        public void CustomPMToPOCO(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.PointerLevelName);

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (authToken != null)
                {
                    int tenant = authToken.Tenant;
                    
                    CustomDocumentTypeTenantQueryService customDocumentTypeTenantQueryService = new CustomDocumentTypeTenantQueryService(tenant);
                    CustomDocumentTypeTenantPM customDocumentTypeTenantPm = customDocumentTypeTenantQueryService.GetByTenant(tenant, entityPOCO.Code).FirstOrDefault();
                    if (customDocumentTypeTenantPm != null)
                    {
                        entityPM.AutoSetOriginalDocumentTrue = customDocumentTypeTenantPm.AutoSetOriginalDocumentTrue;
                        entityPM.CustomsDocumentUpload = customDocumentTypeTenantPm.CustomsDocumentUpload;
                        entityPM.IsCourierManadatory = customDocumentTypeTenantPm.IsCourierManadatory;
                        entityPM.IsDiamondManadatory = customDocumentTypeTenantPm.IsDiamondManadatory;
                        entityPM.PointerLevel = customDocumentTypeTenantPm.PointerLevel;
                    }

                    if (entityPOCO.PointerLevel != null)
                    {
                        PointerLevelQueryService pointerLevelQueryService = new PointerLevelQueryService(tenant);
                        PointerLevelPM pointerLevelPM = pointerLevelQueryService.GetSingle(entityPOCO.PointerLevel, false, true);
                        if (pointerLevelPM != null)
                        {
                            entityPM.PointerLevelName = pointerLevelPM.LocalName;
                        }
                    }

                    if (entityPOCO.CustomsDocumentUpload != null)
                    {
                        CustomsDocumentUploadQueryService customsDocumentUploadQueryService = new CustomsDocumentUploadQueryService(tenant);
                        CustomsDocumentUploadPM customsDocumentUploadPM = customsDocumentUploadQueryService.GetSingle(entityPOCO.CustomsDocumentUpload, false, true);
                        if (customsDocumentUploadPM != null)
                        {
                            entityPM.CustomsDocumentUploadName = customsDocumentUploadPM.LocalName;
                        }
                    }
                    
                }
            }
        }
   }


}
   