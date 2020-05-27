using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.BL.GlobalModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.BL.CommonDataModel.EntityPMs;

using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TenantAdditionalDataService
    {
        bool isNewEntity;
        int tenant;
        public TenantAdditionalData Poco { get; set; }
        private TenantAdditionalDataRepository entityRepository;
        private ICommonDataContext objectContext;

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TenantAdditionalDataPM entityPm;
        public TenantAdditionalDataService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TenantAdditionalDataRepository(objectContext);
        }





        public void Create(TenantAdditionalDataPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            

            this.entityPm = entityPM;
            this.entityPm.Tenant = authToken.Tenant;
            this.entityPm.Id = authToken.Tenant;
            this.Poco = new TenantAdditionalData();
            this.Poco.Id = this.entityPm.Id;


            TenantAdditionalDataMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TenantAdditionalDataPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.tenant = entityPM.Tenant;
            this.Poco = entityRepository.GetSingleTenantAdditionalDataByTenant(entityPM.Tenant);

            TenantAdditionalDataMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
