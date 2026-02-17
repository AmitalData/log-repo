using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class SchedulerProcedureMapping
    {
        public static void MapEntity(SchedulerProcedurePM schedulerProcedurePM, SchedulerProcedure schedulerProcedure, bool isNewState)
        {
            if (isNewState)
            {
                schedulerProcedurePM.Code = schedulerProcedurePM.Code;
            

            }

        }
    }
}