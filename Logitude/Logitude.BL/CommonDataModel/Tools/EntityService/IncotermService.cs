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
    public class IncotermService
    {
        bool isNewEntity;
        private int tenant;
        public Incoterm Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private IncotermPM entityPM;
        private ICommonDataContext objectContext;
        private IncotermRepository entityRepository;
        public IncotermService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new IncotermRepository(objectContext);
        }

        public void Create(IncotermPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Incoterm", tenant).ToString();
            this.Poco = new Incoterm();

            this.Poco.Id = theEntityPm.Id;
            IncotermValidating.Validate(theEntityPm, this.isNewEntity);

            if (!entityPM.IsHybrid)
            {
                IncotermTracing.Trace(theEntityPm, Poco, isNewEntity);
            }

            IncotermMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges(); 
        }

        public void Update(IncotermPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleIncoterm(theEntityPm.Id, theEntityPm.Tenant);

            IncotermValidating.Validate(theEntityPm, false);

            if (!entityPM.IsHybrid)
            {
                IncotermTracing.Trace(theEntityPm, Poco, isNewEntity);
            }

            IncotermMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
