using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class SharedUserQueryService
    {
        bool isNewEntity;
        private int tenant;
        public SharedUserQuery Poco { get; set; }

        private IWebFreightContext objectContext;
        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private SharedUserQueryPM entityPM;        
        private SharedUserQueryRepository entityRepository;
        public SharedUserQueryService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new SharedUserQueryRepository(objectContext);
        }

        public void Create(SharedUserQueryPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("SharedUserQuery", tenant).ToString();
            this.Poco = new SharedUserQuery();
            this.Poco.Id = this.entityPM.Id;
                        
            SharedUserQueryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(SharedUserQueryPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleSharedUserQuery(theEntityPm.Id, tenant);

            SharedUserQueryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
