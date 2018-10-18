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
    public class ErrorLogService
    {

        bool isNewEntity;
        private int tenant;
        public ErrorLog Poco { get; set; }

        public ISystemLogContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ErrorLogPM entityPm;
        private ISystemLogContext objectContext;
        private ErrorLogRepository entityRepository;
        public ErrorLogService(ISystemLogContext objectContext)
        {

            this.ObjectContext = objectContext;
            this.entityRepository = new ErrorLogRepository(objectContext);
        }

        public void Create(ErrorLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = Guid.NewGuid().ToString();
            this.Poco = new ErrorLog();
            this.Poco.Id = entityPM.Id;



            ErrorLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }


        public void Update(ErrorLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleErrorLog(entityPM.Id);

            ErrorLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }

    }
}