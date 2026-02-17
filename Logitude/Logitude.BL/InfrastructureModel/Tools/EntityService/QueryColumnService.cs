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

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class QueryColumnService
    {

        bool isNewEntity;
        private int tenant;
        public QueryColumn Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QueryColumnPM entityPM;
        private IWebFreightContext objectContext;
        private QueryColumnRepository entityRepository;
        public QueryColumnService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QueryColumnRepository(objectContext);
        }

        public void Create(QueryColumnPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("QueryColumn", tenant).ToString();
            this.Poco = new QueryColumn();
            this.Poco.Id = this.entityPM.Id;

            QueryColumnValidating.Validate(theEntityPm);
            QueryColumnTracing.Trace(theEntityPm, Poco, isNewEntity);
            QueryColumnMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QueryColumnPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleQueryColumn(theEntityPm.Id , theEntityPm.Tenant);

           
            if (this.Poco != null)
            {
                QueryColumnValidating.Validate(theEntityPm);
                QueryColumnTracing.Trace(theEntityPm, Poco, isNewEntity);
                QueryColumnMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }

          
        }
    }
}