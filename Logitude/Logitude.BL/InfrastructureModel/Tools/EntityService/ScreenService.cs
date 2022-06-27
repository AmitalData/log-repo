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
    public class ScreenService
    {

        bool isNewEntity;
        private int tenant;
        public Screen Poco { get; set; }
        private int tenantZero = 0;
        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ScreenPM entityPM;
        private IWebFreightContext objectContext;
        private ScreensRepository entityRepository;
        public ScreenService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ScreensRepository(objectContext);
        }

        public void Create(ScreenPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Screen", tenant).ToString();
            this.Poco = new Screen();
            this.Poco.Id = this.entityPM.Id;

            ScreenModification mod = entityRepository.GetScreenModificationByScreen(theEntityPm.Id, theEntityPm.UserTenant);

            ScreenValidating.Validate(theEntityPm);
            ScreenTracing.Trace(theEntityPm, Poco, isNewEntity);
            ScreenMapping.MapEntity(theEntityPm, Poco, isNewEntity, mod);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ScreenPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleScreen(theEntityPm.Id);

            ScreenModification mod = this.Poco.Tenant == tenantZero ? entityRepository.GetScreenModificationByScreen(theEntityPm.Id, theEntityPm.UserTenant) : null;
            if ((this.Poco.NumberOfColumns != theEntityPm.NumberOfColumns) || (this.Poco.NumberOfRows != theEntityPm.NumberOfRows))
            {
                if (mod == null && this.Poco.Tenant == tenantZero)
                {
                    mod = new ScreenModification() { Tenant = theEntityPm.UserTenant, ScreenCode = theEntityPm.Code, Id = IdCounter.GetNumber("ScreenModification", theEntityPm.Tenant), };
                    this.ObjectContext.ScreenModifications.Add(mod);
                }
            }

            ScreenValidating.Validate(theEntityPm);
            ScreenTracing.Trace(theEntityPm, Poco, isNewEntity);
            ScreenMapping.MapEntity(theEntityPm, Poco, isNewEntity, mod);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}