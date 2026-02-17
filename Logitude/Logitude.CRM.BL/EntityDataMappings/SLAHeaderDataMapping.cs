
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class SLAHeaderDataMapping: IMapping<SLAHeaderPM, SLAHeader>
   {

        public void CustomPMToPOCO(SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   