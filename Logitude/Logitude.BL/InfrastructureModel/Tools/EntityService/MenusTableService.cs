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
    public class MenusTableService
    {
        bool isNewEntity;
        private int tenant;
        public MenusTable Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MenusTablePM entityPM;
        private IWebFreightContext objectContext;
        private MenusTableRepository entityRepository;
        public MenusTableService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MenusTableRepository(objectContext);
        }

        public void Create(MenusTablePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("MenusTable", tenant).ToString();

            this.Poco = new MenusTable();
            this.Poco.Id = this.entityPM.Id;


            MenusTableValidating.Validate(theEntityPm);
            MenusTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenusTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            if (theEntityPm.IndexOfOrder == 0 || theEntityPm.IndexOfOrder == null)
            {
                int index = entityRepository.GetCount(theEntityPm.Tenant, theEntityPm.MenuTypeCode, theEntityPm.CategoryTypeCode);
                theEntityPm.IndexOfOrder = index;
            }
        
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(MenusTablePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMenusTable(theEntityPm.Id);

            MenusTableValidating.Validate(theEntityPm);
            MenusTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenusTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}