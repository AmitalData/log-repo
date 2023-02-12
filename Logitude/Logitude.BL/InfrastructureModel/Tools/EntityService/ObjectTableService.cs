using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Services;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableService
    {
        bool isNewEntity;
        private int tenant;
        public ObjectTable Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectTablePM entityPM;
        private IWebFreightContext objectContext;
        private ObjectTableRepository entityRepository;
        public ObjectTableService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectTableRepository(objectContext);
        }

        public void Create(ObjectTablePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ObjectTable", tenant).ToString();

            ObjectTableServiceInitializer objectTableServiceInitializer = new ObjectTableServiceInitializer(this.entityPM, this.ObjectContext, this.entityRepository);
            objectTableServiceInitializer.Initialize();

            this.Poco = new ObjectTable();
            this.Poco.Id = this.entityPM.Id;
            this.Poco.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            this.Poco.LastUpdateDate = theEntityPm.LastUpdateDate;

            ObjectTableValidating.Validate(theEntityPm);
            ObjectTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);            
            new CustomObjectDefaultMetaDataService(this.entityPM, this.ObjectContext).Run();
            this.ObjectContext.SaveChanges();

            objectTableServiceInitializer.InitializeTextCode(this.Poco);
            InvalidateCache();
        }

        public void Update(ObjectTablePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTable(theEntityPm.Id , theEntityPm.Tenant , false);

            ObjectTableValidating.Validate(theEntityPm);
            ObjectTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }


        private void InvalidateCache()
        {
            string tenantListName = "tenantobjecttables" + tenant;
            if (CacheManager.CacheWrapper.Get(tenantListName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(tenantListName);
            }
        }

    }
}