
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
   
   public partial class OpportunityClosingReasonDataMapping: IMapping<OpportunityClosingReasonPM, OpportunityClosingReason>
   {
        public void CustomPMToPOCO(OpportunityClosingReasonPM entityPM, OpportunityClosingReason entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private void BuildSearchFields(OpportunityClosingReasonPM entityPM, OpportunityClosingReason entityPOCO, bool p)
        {
            string result = entityPM.Name + "," + entityPM.LocalName;

            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }

        public void CustomPOCOToPM(OpportunityClosingReasonPM entityPM, OpportunityClosingReason entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }
}
   