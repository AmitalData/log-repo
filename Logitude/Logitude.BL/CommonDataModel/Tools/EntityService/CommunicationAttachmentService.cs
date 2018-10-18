using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CommunicationAttachmentService
    {
        bool isNewEntity;
        private int tenant;
        public CommunicationAttachment Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CommunicationAttachmentPM entityPM;
        private ICommonDataContext objectContext;
        private CommunicationAttachmentRepository entityRepository;
        public CommunicationAttachmentService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CommunicationAttachmentRepository(objectContext);
        }

        public void Create(CommunicationAttachmentPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CommunicationAttachment", tenant).ToString();
            this.Poco = new CommunicationAttachment();
            this.Poco.Id = this.entityPM.Id;

            CommunicationAttachmentValidating.Validate(theEntityPm);
            CommunicationAttachmentTracing.Trace(theEntityPm, Poco, isNewEntity);
            CommunicationAttachmentMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CommunicationAttachmentPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleCommunicationAttachment(theEntityPm.Id, tenant);

            CommunicationAttachmentValidating.Validate(theEntityPm);
            CommunicationAttachmentTracing.Trace(theEntityPm, Poco, isNewEntity);
            CommunicationAttachmentMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
