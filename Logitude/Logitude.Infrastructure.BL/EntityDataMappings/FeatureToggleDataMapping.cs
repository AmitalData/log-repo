
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{   
   public partial class FeatureToggleDataMapping: IMapping<FeatureTogglePM, FeatureToggle>
   {
        public void CustomPMToPOCO(FeatureTogglePM entityPM, FeatureToggle entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(FeatureTogglePM entityPM, FeatureToggle entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ToggleName);

            ToggleRepository toggleRepository = new ToggleRepository(entityPOCO.Tenant);
            ToggleKeys toggleKeys = new ToggleKeys() { Code = entityPOCO.ToggleCode };
            Toggle toggle = toggleRepository.GetSingle(toggleKeys);
            if (toggle != null)
            {
                entityPM.ToggleName = toggle.Name;
            }
        }

        private void BuildSearchFields(FeatureTogglePM entityPM, FeatureToggle entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ToggleName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.TenantNumber.ToString());
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   