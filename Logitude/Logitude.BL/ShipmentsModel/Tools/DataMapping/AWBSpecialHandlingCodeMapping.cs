using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class AWBSpecialHandlingCodeMapping
    {
        public static void MapEntity(AWBSpecialHandlingCodePM entityPM, AWBSpecialHandlingCode entityPOCO, bool isNewEntity)
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
            BuildSearchField(entityPM, entityPOCO);
        }

        public static void BuildSearchField(AWBSpecialHandlingCodePM entityPM, AWBSpecialHandlingCode entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
