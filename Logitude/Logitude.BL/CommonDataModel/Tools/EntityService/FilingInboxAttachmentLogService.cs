using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;


namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class FilingInboxAttachmentLogService
    {
        bool isNewEntity;
        private int tenant;
        public FilingInboxAttachmentLog Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private FilingInboxAttachmentLogPM entityPm;
        private ICommonDataContext objectContext;
        private FilingInboxAttachmentLogRepository entityRepository;

        public FilingInboxAttachmentLogService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FilingInboxAttachmentLogRepository(objectContext);
        }

        public void Create(FilingInboxAttachmentLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("FilingInboxAttachmentLog", entityPM.Tenant).ToString();
            this.Poco = new FilingInboxAttachmentLog();
            this.Poco.Id = this.entityPm.Id;
            FilingInboxAttachmentLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(FilingInboxAttachmentLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleFilingInboxAttachmentLog(entityPM.Id, tenant);
            FilingInboxAttachmentLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
