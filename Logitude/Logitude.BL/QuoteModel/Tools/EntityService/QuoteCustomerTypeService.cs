using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteCustomerTypeService
    {
          bool isNewEntity;
        private int tenant;
        public QuoteCustomerType Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteCustomerTypePM entityPm;
        private IQuotesContext objectContext;
        private QuoteCustomerTypeRepository entityRepository;
        public QuoteCustomerTypeService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteCustomerTypeRepository(objectContext);
        }

        public void Create(QuoteCustomerTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM; 
            this.Poco = new QuoteCustomerType();
            //this.entityPm.Id = IdCounter.GetNumber("QuoteCustomerType", tenant).ToString();
            QuoteCustomerTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteCustomerTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteCustomerType(entityPM.Code);
             
            QuoteCustomerTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
