using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{

    public partial class DashboardsUserSettingDataMapping : IMapping<DashboardsUserSettingPM, DashboardsUserSetting>
    {

        public void CustomPMToPOCO(DashboardsUserSettingPM entityPM, DashboardsUserSetting entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert && entityPOCO != null)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(DashboardsUserSettingPM entityPM, DashboardsUserSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }
    }


}
