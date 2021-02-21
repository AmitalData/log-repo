
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
   
   public partial class ExportDeclarationClosingDataDataMapping: IMapping<ExportDeclarationClosingDataPM, ExportDeclarationClosingData>
   {

        public void CustomPMToPOCO(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
            if(entityPOCO.FinalCargoType != null)
            {
                entityPM.FinalCargoTypeName = entityPOCO.FinalCargoType.LocalName;
            }
            if(entityPOCO.FinalLoadingSiteType != null)
            {
                entityPM.FinalLoadingSiteName = entityPOCO.FinalLoadingSiteType.LocalName;
            }
            if(entityPOCO.FinalCustomsShip != null)
            {
                entityPM.FinalShipCodeName = entityPOCO.FinalCustomsShip.LocalName;
            }
        }
   }


}
   