

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentsExecutionLogService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentsExecutionLog Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentsExecutionLogPM entityPm;
        private ICommonDataContext objectContext;
        private DocumentsExecutionLogRepository entityRepository;


        private Contact loggedContact;
        public DocumentsExecutionLogService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentsExecutionLogRepository(objectContext);

        }



        public void Create(DocumentsExecutionLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("DocumentsExecutionLog", tenant).ToString();
            this.Poco = new DocumentsExecutionLog();

            DocumentsExecutionLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentsExecutionLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleDocumentsExecutionLog(entityPM.Id, entityPm.Tenant);
            DocumentsExecutionLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }



    }
}
