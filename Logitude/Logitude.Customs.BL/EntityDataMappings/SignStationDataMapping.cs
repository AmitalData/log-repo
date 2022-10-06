
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SignStationDataMapping: IMapping<SignStationPM, SignStation>
   {

        public void CustomPMToPOCO(SignStationPM entityPM, SignStation entityPOCO)
        {
            //throw new NotImplementedException();
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.CustomsAgentId = entityPM.CustomsAgentId;
                entityPOCO.PersonId = entityPM.PersonId;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private void BuildSearchFields(SignStationPM entityPM, SignStation entityPOCO, bool isNewEntity)
        {
            string result = "";

            result = entityPM.SignCertificate;

            entityPM.SearchFields = result.ToLower(); ;
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(SignStationPM entityPM, SignStation entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   