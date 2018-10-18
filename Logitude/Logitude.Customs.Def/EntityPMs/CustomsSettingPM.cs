using Logitude.Customs.Def.ClosedTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class CustomsSettingPM
    {
        public CustomsDeploymentStage MyCustomsDeploymentStage
        {
            get
            {
                CustomsDeploymentStage customsDeploymentStage=  CustomsDeploymentStage.None;
                Enum.TryParse<CustomsDeploymentStage>(this.CustomsEnvoirmentTypeCode, out customsDeploymentStage);
                return customsDeploymentStage;
            }
        }
        
    }
}
