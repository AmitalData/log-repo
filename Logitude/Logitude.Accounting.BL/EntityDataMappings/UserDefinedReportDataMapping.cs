
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Simplog.Data.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class UserDefinedReportDataMapping: IMapping<UserDefinedReportPM, UserDefinedReport>
   {

        public void CustomPMToPOCO(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.CreateDateTime);
            AddPOCOPropertyName(POCOPropertyNames.UpdatedDateTime);
            entityPM.UpdatedDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.UpdatedDateTime = entityPM.UpdatedDateTime;
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            }
        }

        public void CustomPOCOToPM(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   