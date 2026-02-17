using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ComputingPartnerMapping
    {
        public static void MapEntity(ComputingPartnerPM entityPM, ComputingPartner entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Remarks = entityPM.Remarks;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            if(entityPM.Code==null)
            entityPOCO.Code = entityPM.Name;
            else
                entityPOCO.Code = entityPM.Code;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(ComputingPartnerPM entityPM, ComputingPartner entityPOCO)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            if (!string.IsNullOrEmpty(entityPM.Remarks))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Remarks : mySearchFields + "," + entityPM.Remarks;
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}