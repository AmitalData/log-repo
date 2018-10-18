
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
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class BatchTaskExecutionDataMapping: IMapping<BatchTaskExecutionPM, BatchTaskExecution>
   {

        public void CustomPMToPOCO(BatchTaskExecutionPM entityPM, BatchTaskExecution entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;

            BuildSearchFields(entityPM, entityPOCO, false);
        }

        public void CustomPOCOToPM(BatchTaskExecutionPM entityPM, BatchTaskExecution entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.SearchFields);

            if (entityPM.StatusCode != null)
            {
                BatchTaskExecutionStatusQueryService query = new BatchTaskExecutionStatusQueryService(entityPM.Tenant);
                BatchTaskExecutionStatusPM status = query.GetSingle(entityPM.StatusCode, false, false);
                entityPM.StatusName = status.Name;
            }
            if (entityPM.CreatedByUserId != null)
            {
                UserQueryService query1 = new UserQueryService(entityPM.Tenant);
                User user = query1.GetUserById(entityPM.CreatedByUserId, entityPM.Tenant);
                entityPM.CreatedByUserName = user.EnglishName;
            }

            BuildSearchFields(entityPM, entityPOCO,false);
        }

        private void BuildSearchFields(BatchTaskExecutionPM entityPM, BatchTaskExecution entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.StatusName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ClassName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ProgressMessage);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ErrorLog);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   