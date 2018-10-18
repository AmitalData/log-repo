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
    public class RankService
    {
        bool isNewEntity;
        private int tenant;
        public Rank Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RankPM entityPm;
        private ICommonDataContext objectContext;
        private RankRepository entityRepository;
        public RankService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RankRepository(objectContext);
        }

        public void Create(RankPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Rank", tenant).ToString();
            this.Poco = new Rank();
            this.Poco.Id = this.entityPm.Id;

            RankValidating.Validate(entityPM);
            RankTracing.Trace(entityPM, Poco, isNewEntity);
            RankMpapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(RankPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleRank(entityPM.Id , entityPm.Tenant);

            RankValidating.Validate(entityPM);
            RankTracing.Trace(entityPM, Poco, isNewEntity);
            RankMpapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
