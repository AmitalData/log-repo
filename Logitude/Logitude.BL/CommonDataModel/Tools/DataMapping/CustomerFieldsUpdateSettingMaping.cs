using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerFieldsUpdateSettingMapping
    {

        public static void MapEntity(CustomerFieldsUpdateSettingPM entityPM, CustomerFieldsUpdateSetting entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.ObjectFieldId = entityPM.ObjectFieldId;
            entityPOCO.ObjectFieldCode = entityPM.ObjectFieldCode;
            entityPOCO.UpdateDirection = entityPM.UpdateDirection;
            BuildSearchFields(entityPM, entityPOCO);

        }

        private static void BuildSearchFields(CustomerFieldsUpdateSettingPM entityPM, CustomerFieldsUpdateSetting entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ObjectFieldCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.UpdateDirection);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
