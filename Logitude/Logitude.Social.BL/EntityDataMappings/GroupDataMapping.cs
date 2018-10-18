
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs; 
using Logitude.Social.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Social.BL.EntityDataMappings
{
   
   public partial class GroupDataMapping: IMapping<GroupPM, Group>
   {

        public void CustomPMToPOCO(GroupPM entityPM, Group entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(GroupPM entityPM, Group entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   