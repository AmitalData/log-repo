using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AgentSharedDocumentMapping
    {
        public static void MapEntity(AgentSharedDocumentPM entityPM, AgentSharedDocument entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
         
            entityPOCO.AgentReference = entityPM.AgentReference;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.DocumentXML = entityPM.DocumentXML;
            entityPOCO.AgentId = entityPM.AgentId;
            entityPOCO.StatusCode = entityPM.StatusCode;
            entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            entityPOCO.AgentSharedManifestRef = entityPM.AgentSharedManifestRef;
      

        }
    }
}
