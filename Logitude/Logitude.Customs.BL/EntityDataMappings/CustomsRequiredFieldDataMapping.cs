
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
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsRequiredFieldDataMapping: IMapping<CustomsRequiredFieldPM, CustomsRequiredField>
   {

        public void CustomPMToPOCO(CustomsRequiredFieldPM entityPM, CustomsRequiredField entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CustomsRequiredFieldPM entityPM, CustomsRequiredField entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ObjectFieldName);
            if (entityPOCO.ObjectfieldCode != null)
            {
                ObjectFieldRepository objectFieldsRep = new ObjectFieldRepository(entityPM.Tenant);
                ObjectField objectField = objectFieldsRep.GetSingleObjectField(entityPOCO.ObjectfieldCode);
                if( objectField != null)
                entityPM.ObjectFieldName = objectField.FieldName;
            }

           
        }
   }


}
   