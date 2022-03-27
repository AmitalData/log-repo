using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        private static bool isContainerMappingEntityPM;
        private static bool isNew;
        public static void MapContainerConcurrencyFields(ContainerPM entityPM, Container entityPoco, bool isNewEntity, bool isMappingPM = true)
        {
            isNew = isNewEntity;
            isContainerMappingEntityPM = isMappingPM;
            if (isNew)
            {
                MapContainerConcurrencyFields_Client(entityPM, entityPoco);
                return;
            }
            MapContainerConcurrencyFields_OnEdited(entityPM, entityPoco);
            MapContainerConcurrencyFields_Client(entityPM, entityPoco);
        }
        private static void MapContainerConcurrencyFields_Client(ContainerPM entityPM, Container entityPoco)
        {
            if (isContainerMappingEntityPM)
            {
                entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            }
            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
        }
        private static void MapContainerConcurrencyFields_OnEdited(ContainerPM entityPM, Container entityPoco)
        {
           
        }
    }
}
