
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
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SIIRequestDataMapping: IMapping<SIIRequestPM, SIIRequest>
   {
        
        public void CustomPMToPOCO(SIIRequestPM entityPM, SIIRequest entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            entityPOCO.DeclarationId = entityPM.DeclarationId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = !string.IsNullOrEmpty(entityPM.SearchFields) && entityPM.SearchFields.Length >= 1000 ? entityPM.SearchFields.Substring(0, 999) : entityPM.SearchFields;

        }

       

        public void CustomPOCOToPM(SIIRequestPM entityPM, SIIRequest entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VesselName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManifestNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UnloadDate);


            var repo = new SIIRequestRepository(entityPOCO.Tenant);
            var agg = repo.GetAggregateForSii(entityPOCO.Tenant, entityPOCO.DeclarationId);

            if (agg != null)
            {
                entityPM.ImporterId = !string.IsNullOrEmpty(agg.ImporterInternalId)
                                    ? agg.ImporterInternalId
                                    : agg.ImporterCode;

                entityPM.VesselName = agg.VesselLocalName;
                entityPM.ManifestNumber = agg.ManifestNumber;
                if (agg.UnloadDate.HasValue)
                    entityPM.UnloadDate = agg.UnloadDate.Value;
            }

        }

        private void BuildSearchFields(SIIRequestPM entityPM, SIIRequest entityPOCO, bool v)
        {
        }
    }


}
   