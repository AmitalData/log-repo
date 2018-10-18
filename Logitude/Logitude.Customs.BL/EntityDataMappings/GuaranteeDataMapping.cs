
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class GuaranteeDataMapping: IMapping<GuaranteePM, Guarantee>
   {

       public void CustomPMToPOCO(GuaranteePM entityPM, Guarantee entityPOCO)
       {
           AddPOCOPropertyName(POCOPropertyNames.Id);
           AddPOCOPropertyName(POCOPropertyNames.Tenant);
           if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;
           }
       }

       public void CustomPOCOToPM(GuaranteePM entityPM, Guarantee entityPOCO)
       {
           CustomMappedPMProperties.Add(PMPropertyNames.CustomEntityTypeName);
           CustomMappedPMProperties.Add(PMPropertyNames.GuaranteeRequestStatusName);
           CustomMappedPMProperties.Add(PMPropertyNames.ClientActivityTypeName);


           if (entityPOCO.CustomEntityTypeCode != null)
           {
               EntityTypeLookupQueryService entityTypeQuery = new EntityTypeLookupQueryService(entityPM.Tenant);
               EntityTypeLookupPM entityTypeLookup = entityTypeQuery.GetSingle(entityPOCO.CustomEntityTypeCode, false, false);
               if (entityTypeLookup != null)
                   entityPM.CustomEntityTypeName = entityTypeLookup.LocalName;
           }

           if (entityPOCO.GuaranteeRequestStatusCode != null)
           {
               RequestStatusQueryService entityQuery = new RequestStatusQueryService(entityPM.Tenant);
               RequestStatusPM requestStatus = entityQuery.GetSingle(entityPOCO.GuaranteeRequestStatusCode, false, false);
               if (requestStatus != null)
                   entityPM.GuaranteeRequestStatusName = requestStatus.LocalName;
           }

           if (entityPOCO.ClientActivityCode != null)
           {
               CustomerActivityTypeQueryService entityQuery = new CustomerActivityTypeQueryService(entityPM.Tenant);
               CustomerActivityTypePM entity = entityQuery.GetSingle(entityPOCO.ClientActivityCode, false, false);
               if (entity != null)
                   entityPM.ClientActivityTypeName = entity.LocalName;
           }


       }
   }


}
   