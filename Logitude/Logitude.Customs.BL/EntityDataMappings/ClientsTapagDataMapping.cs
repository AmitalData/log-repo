
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
           

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }
            
        }

        public void CustomPOCOToPM(ClientsTapagPM entityPM, ClientsTapag entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   