
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
   
   public partial class DeclarationExportRecipientDataMapping: IMapping<DeclarationExportRecipientPM, DeclarationExportRecipient>
   {

        public void CustomPMToPOCO(DeclarationExportRecipientPM entityPM, DeclarationExportRecipient entityPOCO)
        {
            //throw new NotImplementedException();
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DeclarationExportRecipientPM entityPM, DeclarationExportRecipient entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   