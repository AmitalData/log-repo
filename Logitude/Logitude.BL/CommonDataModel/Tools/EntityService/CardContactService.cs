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
    public class CardContactService
    {

        bool isNewEntity;
        private int tenant;
        public CardContact Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CardContactPM entityPm;
        private ICommonDataContext objectContext;
        private CardContactRepository entityRepository;
        public CardContactService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CardContactRepository(objectContext);
        }

        public void Create(CardContactPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CardContact", tenant).ToString();
            this.Poco = new CardContact();
            this.Poco.Id = this.entityPm.Id;

            CardContactValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CardContactTracing.Trace(entityPM, Poco, isNewEntity);
            }
            CardContactMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CardContactPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCardContact(entityPM.Id , entityPm.Tenant);

            CardContactValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CardContactTracing.Trace(entityPM, Poco, isNewEntity);
            }
            CardContactMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
