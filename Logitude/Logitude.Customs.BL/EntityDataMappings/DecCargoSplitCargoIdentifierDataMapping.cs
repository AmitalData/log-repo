
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
   
   public partial class DecCargoSplitCargoIdentifierDataMapping: IMapping<DecCargoSplitCargoIdentifierPM, DecCargoSplitCargoIdentifier>
   {

        public void CustomPMToPOCO(DecCargoSplitCargoIdentifierPM entityPM, DecCargoSplitCargoIdentifier entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationCargoSplitId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationCargoSplitId = entityPM.DeclarationCargoSplitId;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DecCargoSplitCargoIdentifierPM entityPM, DecCargoSplitCargoIdentifier entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   