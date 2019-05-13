using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DWObjectFieldMapping
    {
        public static void MapEntity(DWObjectFieldPM entityPM, DWObjectField entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code= entityPM.Code;
            entityPOCO.DataTypeCode = entityPM.DataTypeCode;
            entityPOCO.DimensionTableCode = entityPM.DimensionTableCode;
            entityPOCO.DWObjectTableCode = entityPM.DWObjectTableCode;

            entityPOCO.IsRequired = entityPM.IsRequiered;
            entityPOCO.MaxLength = entityPM.MaxLength;
            entityPOCO.MinLength = entityPM.MinLength;
            entityPOCO.IsMeasurement = entityPM.IsMeasurement;
            entityPOCO.AggregationTypeCode = entityPM.AggregationTypeCode;
            entityPOCO.DisplayInQueryBuilder = entityPM.DisplayInQueryBuilder;
            //entityPOCO.Category1 = entityPM.Category1;
            //entityPOCO.Category2 = entityPM.Category2;
            entityPOCO.LOVAdditionalColumns = entityPM.LOVAdditionalColumns;
            entityPOCO.HideTree = entityPM.HideTree;
            entityPOCO.CannotFilter = entityPM.CannotFilter;
            entityPOCO.HelpText = entityPM.HelpText;
            entityPOCO.IsCustom = entityPM.IsCustom;

        }
    }

 
}
