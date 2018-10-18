using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddMoveTypes
    {
        public static void AddMoveType(MoveTypeDetails moveTypeDetails, MoveTypeRepository moveTypeRepository, Dictionary<string, MoveType> tenantMoveTypes)
        {
            if (tenantMoveTypes.Keys.Contains(moveTypeDetails.Code))
            {
                MoveType moveType = tenantMoveTypes[moveTypeDetails.Code];

                moveType.Tenant = moveTypeDetails.Tenant;
                moveType.MoveTypeEnglishName = moveTypeDetails.MoveTypeEnglishName;
                moveType.MoveTypeLocalName = moveTypeDetails.MoveTypeLocalName;
                moveType.AddedManually = false;
                moveType.InActive = false;
                moveType.TransportModeId = moveTypeDetails.TransportModeId;
                moveType.Code = moveTypeDetails.Code;                
                moveType.SearchFields = moveTypeDetails.Code + "," + moveTypeDetails.MoveTypeEnglishName + "," + moveTypeDetails.TransportModeId;
                moveTypeRepository.Update(moveType);
            }
            else
            {
                MoveType newMoveType = new MoveType()
                {
                    Tenant = moveTypeDetails.Tenant,
                    MoveTypeEnglishName = moveTypeDetails.MoveTypeEnglishName,
                    MoveTypeLocalName = moveTypeDetails.MoveTypeLocalName,
                    AddedManually = false,
                    InActive = false,
                    TransportModeId = moveTypeDetails.TransportModeId,
                    Code = moveTypeDetails.Code,
                    SearchFields = moveTypeDetails.Code + "," + moveTypeDetails.MoveTypeEnglishName + "," + moveTypeDetails.TransportModeId,
                    Id = IdCounter.GetNumber("MoveType", moveTypeDetails.Tenant).ToString(),

                };
                moveTypeRepository.Add(newMoveType);
            }
        }
    }
}