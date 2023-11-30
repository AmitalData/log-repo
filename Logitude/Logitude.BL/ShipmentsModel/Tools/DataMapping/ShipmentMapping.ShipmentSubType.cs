using Logitude.BL.ShipmentsModel.EntityPMs;
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
        public static void MapEntity(ShipmentSubTypePM entityPM, ShipmentSubType entityPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;                
                entityPoco.CreateDate = entityPM.CreateDate;
                entityPoco.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPoco.Code = entityPM.Code;
            entityPoco.ShipmentTypeCode = entityPM.ShipmentTypeCode;
            entityPoco.Name = entityPM.Name;
            entityPoco.UpdateDate = entityPM.UpdateDate;
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPoco.Inactive = entityPM.Inactive;
            entityPoco.IsManuallyAdded = entityPM.IsManuallyAdded;
            BuildSearchFields(entityPM, entityPoco);
        }

        private static void BuildSearchFields(ShipmentSubTypePM entityPM, ShipmentSubType entityPoco)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Code : mySearchFields + "," + entityPM.Code;
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
