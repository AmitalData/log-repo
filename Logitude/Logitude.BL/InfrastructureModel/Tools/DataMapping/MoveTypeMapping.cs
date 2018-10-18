using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class MoveTypeMapping
    {
        public static void MapEntity(MoveTypePM entityPM, MoveType entity, bool isNewState)
        {
            entity.Tenant = entityPM.Tenant;
            entity.MoveTypeEnglishName = entityPM.MoveTypeEnglishName;
            entity.MoveTypeLocalName = entityPM.MoveTypeLocalName;
            entity.AddedManually = entityPM.AddedManually;
            entity.InActive = entityPM.InActive;
            entity.TransportModeId = entityPM.TransportModeId;
            entity.Code = entityPM.Code;
            entity.SearchFields = entityPM.Code + "," + entityPM.MoveTypeEnglishName + "," + entityPM.TransportModeId;
        }
    }
}
