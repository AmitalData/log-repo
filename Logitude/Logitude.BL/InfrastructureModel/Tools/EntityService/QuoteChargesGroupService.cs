using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class QuoteChargesGroupService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteChargesGroup Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteChargesGroupPM entityPM;
        private IWebFreightContext objectContext;
        private QuoteChargesGroupRepository entityRepository;
        public QuoteChargesGroupService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteChargesGroupRepository(objectContext);
        }

        public void Create(QuoteChargesGroupPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("QuoteChargesGroup", tenant).ToString();
            this.Poco = new QuoteChargesGroup();
            this.Poco.Id = this.entityPM.Id;

            QuoteChargesGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteChargesGroupPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleQuoteChargesGroup(theEntityPm.Id, theEntityPm.Tenant);
            QuoteChargesGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}