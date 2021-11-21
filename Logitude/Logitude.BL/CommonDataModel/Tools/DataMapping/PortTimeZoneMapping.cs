using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class PortTimeZoneMapping
    {
        public static void MapEntity(PortTimeZonePM entityPM, PortTimeZone entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Code = entityPM.Code;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.Inactive = entityPM.Inactive;
            entityPOCO.UTCOffset = entityPM.UTCOffset;
            entityPOCO.UTCDSTOffset = entityPM.UTCDSTOffset;            

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(PortTimeZonePM entityPM, PortTimeZone entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.UTCOffset);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
