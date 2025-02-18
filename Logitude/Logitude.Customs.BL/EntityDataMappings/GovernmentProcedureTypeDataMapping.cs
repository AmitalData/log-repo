
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class GovernmentProcedureTypeDataMapping: IMapping<GovernmentProcedureTypePM, GovernmentProcedureType>
   {

        public void CustomPMToPOCO(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (authToken != null)
                {
                    int tenant = authToken.Tenant;

                    GovernmentProcTypeTenantQueryService customDocumentTypeTenantQueryService = new GovernmentProcTypeTenantQueryService(tenant);
                    GovernmentProcTypeTenantPM customDocumentTypeTenantPm = customDocumentTypeTenantQueryService.GetByTenat(tenant, entityPOCO.Code).FirstOrDefault();
                    if (customDocumentTypeTenantPm != null)
                    {
                        entityPM.IsExport = customDocumentTypeTenantPm.IsExport;
                        entityPM.IsImport = customDocumentTypeTenantPm.IsImport;
                        entityPM.IndexOrder = customDocumentTypeTenantPm.IndexOrder;
                    }
                }
            }
        }
   }


}
   