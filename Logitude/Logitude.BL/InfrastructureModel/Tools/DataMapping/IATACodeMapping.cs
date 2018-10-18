using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class IATACodeMapping
    {
        public static void MapEntity(IATACodePM entityPM, IATACode entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
            }

            entityPOCO.Code = entityPM.Code;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.AirlineId = entityPM.AirlineId;
            entityPOCO.IsIATA = entityPM.IsIATA;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.DueTypeCode = entityPM.DueTypeCode;
            entityPOCO.MeasurementCode = entityPM.MeasurementCode;

            BuildSearchField(entityPM, entityPOCO);
        }

        public static void BuildSearchField(IATACodePM entityPM, IATACode entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.DueTypeCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MeasurementCode);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
