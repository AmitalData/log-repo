using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TermsofUseService
    {
        private int tenant;
        public TermsofUse Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TermsofUsePM entityPm;
        private ICommonDataContext objectContext;
        private TermsofUseRepository entityRepository;
        private TermsofUseQuery entityQuery;
        public TermsofUseService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TermsofUseRepository(objectContext);
            this.entityQuery = new TermsofUseQuery(tenant);
        }

        public void Create(TermsofUsePM entityPM)
        {
            this.entityPm = entityPM;
            this.Poco = new TermsofUse();
            this.entityPm.Date = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPm.Tenant = entityPM.Tenant;
            this.entityPm.VersionNumber = GetLastVersionNumber() + 1; 
            TermsofUseMapping.MapEntity(entityPM, Poco);
            
            entityRepository.Add(Poco); 
            entityPM.Id = this.Poco.Id;
            entityRepository.SubmitChanges(); 
        }



        private int GetLastVersionNumber()
        {
            return entityRepository.GetLastTermsofUseVersionNumber( entityPm.Tenant);
        }
    }
}
