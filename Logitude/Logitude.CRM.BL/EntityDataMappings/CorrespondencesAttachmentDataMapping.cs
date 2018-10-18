
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
   
   public partial class CorrespondencesAttachmentDataMapping: IMapping<CorrespondencesAttachmentPM, CorrespondencesAttachment>
   {
        public void CustomPMToPOCO(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   