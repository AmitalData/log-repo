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
    public class UserPermittedBranchService
    {
            bool isNewEntity;
        private int tenant;
        public UserPermittedBranch Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UserPermittedBranchPM entityPm;
        private ICommonDataContext objectContext;
        private UserPermittedBranchRepository entityRepository;
        public UserPermittedBranchService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UserPermittedBranchRepository(objectContext);
        }

        public void Create(UserPermittedBranchPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("UserPermittedBranch", tenant).ToString();
            this.Poco = new UserPermittedBranch();
            this.Poco.Id = this.entityPm.Id;

            UserPermittedBranchValidating.Validate(entityPM);
            UserPermittedBranchTracing.Trace(entityPM, Poco, isNewEntity);
            UserPermittedBranchMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(UserPermittedBranchPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleUserPermittedBranch(entityPM.Id, entityPm.Tenant);

            UserPermittedBranchValidating.Validate(entityPM);
            UserPermittedBranchTracing.Trace(entityPM, Poco, isNewEntity);
            UserPermittedBranchMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
