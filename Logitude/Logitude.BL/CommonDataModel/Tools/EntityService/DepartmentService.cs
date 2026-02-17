using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DepartmentService
    {
          bool isNewEntity;
        private int tenant;
        public Department Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DepartmentPM entityPm;
        private ICommonDataContext objectContext;
        private DepartmentRepository entityRepository;
        public DepartmentService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DepartmentRepository(objectContext);
        }

        public void Create(DepartmentPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Department", tenant).ToString();
            this.Poco = new Department();
            this.Poco.Id = this.entityPm.Id;

            DepartmentValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                DepartmentTracing.Trace(entityPM, Poco, isNewEntity);
            }
            DepartmentMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DepartmentPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleDepartment(entityPM.Id , entityPm.Tenant);

            string entityName = "Department" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "DepartmentPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            DepartmentValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                DepartmentTracing.Trace(entityPM, Poco, isNewEntity);
            }
            DepartmentMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
