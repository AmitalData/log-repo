
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
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class TicketStageDataMapping: IMapping<TicketStagePM, TicketStage>
   {

        public void CustomPMToPOCO(TicketStagePM entityPM, TicketStage entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);

        }

        public void CustomPOCOToPM(TicketStagePM entityPM, TicketStage entityPOCO)
        {
            //throw new NotImplementedException();
        }
        private void BuildSearchFields(TicketStagePM entityPM, TicketStage entityPOCO, bool p)
        {
            string result = entityPM.Name;

            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }
   }


}
   