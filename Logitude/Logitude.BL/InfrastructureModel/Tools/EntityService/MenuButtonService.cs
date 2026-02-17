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
    public class MenuButtonService
    {
        bool isNewEntity;
        private int tenant;
        public MenuButton Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MenuButtonPM entityPM;
        private IWebFreightContext objectContext;
        private MenuButtonRepository entityRepository;
        public MenuButtonService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MenuButtonRepository(objectContext);
        }

        public void Create(MenuButtonPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("MenuButton", tenant).ToString();
            this.Poco = new MenuButton();
            this.Poco.Id = this.entityPM.Id;

            MenuButtonValidating.Validate(theEntityPm);
            MenuButtonTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenuButtonMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(MenuButtonPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMenuButton(theEntityPm.Id);

            MenuButtonValidating.Validate(theEntityPm);
            MenuButtonTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenuButtonMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}