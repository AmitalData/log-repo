
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{   
   public partial class BIReportFolderDataMapping: IMapping<BIReportFolderPM, BIReportFolder>
   {
        public void CustomPMToPOCO(BIReportFolderPM entityPM, BIReportFolder entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(BIReportFolderPM entityPM, BIReportFolder entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(BIReportFolderPM entityPM, BIReportFolder entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   