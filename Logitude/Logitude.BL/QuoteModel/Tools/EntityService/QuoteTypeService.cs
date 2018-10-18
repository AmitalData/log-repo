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
    public class QuoteTypeService
    {
          bool isNewEntity;
        private int tenant;
        public QuoteType Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTypePM entityPm;
        private IQuotesContext objectContext;
        private QuoteTypeRepository entityRepository;
        public QuoteTypeService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTypeRepository(objectContext);
        }

        public void Create(QuoteTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM; 
            this.Poco = new QuoteType();
            //this.entityPm.Id = IdCounter.GetNumber("QuoteType", tenant).ToString();
            QuoteTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteType(entityPM.Code);
             
            QuoteTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
