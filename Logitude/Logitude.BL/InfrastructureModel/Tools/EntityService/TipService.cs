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
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TipService
    {
        bool isNewEntity;
        private int tenant;
        public Tip Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TipPM entityPM;
        private IWebFreightContext objectContext;
        private TipRepository entityRepository;
        public TipService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TipRepository(objectContext);
        }

        public void Create(TipPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;

            TipQuery tipQuery = new TipQuery(entityRepository);
            bool exists = tipQuery.GetSingleTipPM(theEntityPm.Code, theEntityPm.Tenant) != null ? true : false;

            if (!exists)
            {
                TipValidating.Validate(theEntityPm);
                TipTracing.Trace(theEntityPm, Poco, isNewEntity);
                TipMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }

            
        }

        public void Update(TipPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTip(theEntityPm.Code , theEntityPm.Tenant);

            TipValidating.Validate(theEntityPm);
            TipTracing.Trace(theEntityPm, Poco, isNewEntity);
            TipMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}