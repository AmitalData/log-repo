
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClientsTapagDataMapping: IMapping<ClientsTapagPM, ClientsTapag>
   {

        public void CustomPMToPOCO(ClientsTapagPM entityPM, ClientsTapag entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }

            entityPOCO.SearchFields = entityPM.TapagNumber.ToLower();
            
        }

        public void CustomPOCOToPM(ClientsTapagPM entityPM, ClientsTapag entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.SearchFields);

            entityPM.SearchFields = entityPM.TapagNumber?.ToLower();
            //throw new NotImplementedException();
        }
   }


}
   