using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
            entityPOCO.UpdateDirection = entityPM.UpdateDirection;
           



        }

    }




}
