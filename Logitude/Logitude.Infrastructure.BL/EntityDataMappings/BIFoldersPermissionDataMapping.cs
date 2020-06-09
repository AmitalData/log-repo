
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

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class BIFoldersPermissionDataMapping: IMapping<BIFoldersPermissionPM, BIFoldersPermission>
   {

        public void CustomPMToPOCO(BIFoldersPermissionPM entityPM, BIFoldersPermission entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.FolderId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UserId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.FolderId = entityPM.FolderId;
            entityPOCO.UserId = entityPM.UserId;

        }

        public void CustomPOCOToPM(BIFoldersPermissionPM entityPM, BIFoldersPermission entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   