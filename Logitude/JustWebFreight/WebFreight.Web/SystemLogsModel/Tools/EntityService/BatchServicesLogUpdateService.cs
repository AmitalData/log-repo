using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.Tools.Mapping;

namespace WebFreight.Web.SystemLogsModel.Tools.EntityService
{
    public class BatchServicesLogUpdateService
    {

         bool isNewEntity;
        private int tenant;
        public BatchServicesLog Poco { get; set; }

        public ISystemLogContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private BatchServicesLogPM entityPm;
        private ISystemLogContext objectContext;
        private BatchServicesLogRepository entityRepository;
        public BatchServicesLogUpdateService(ISystemLogContext objectContext)
        {
          
            this.ObjectContext = objectContext;
            this.entityRepository = new BatchServicesLogRepository(objectContext);
        }

        public void Create(BatchServicesLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = Guid.NewGuid().ToString(); 
            this.Poco = new BatchServicesLog();
            this.Poco.Id = entityPM.Id;

        

            BatchServicesLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();            
        }


        public void Update(BatchServicesLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBatchServicesLog(entityPM.Id);

            BatchServicesLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
           
        }

    }
}