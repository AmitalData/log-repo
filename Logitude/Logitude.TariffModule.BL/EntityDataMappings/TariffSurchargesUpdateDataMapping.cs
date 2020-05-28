
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;
using Simplog.Server.Infrastructure;
using Logitude.TariffModule.Data.Repositories;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffSurchargesUpdateDataMapping: IMapping<TariffSurchargesUpdatePM, TariffSurchargesUpdate>
   {

        public void CustomPMToPOCO(TariffSurchargesUpdatePM entityPM, TariffSurchargesUpdate entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(TariffSurchargesUpdatePM entityPM, TariffSurchargesUpdate entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdateMethodName);

            if (!string.IsNullOrEmpty(entityPOCO.UpdateMethodCode))
            {
                TariffSurchargesUpdateMethodRepository repository = new TariffSurchargesUpdateMethodRepository(entityPOCO.Tenant);
                TariffSurchargesUpdateMethod method = repository.GetSingle(entityPOCO.UpdateMethodCode);
                if (method != null)
                {
                    entityPM.UpdateMethodName = method.Name;
                }
            }
        }
    }


}
   