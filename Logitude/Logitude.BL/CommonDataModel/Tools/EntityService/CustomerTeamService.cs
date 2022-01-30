using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerTeamService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerTeam Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerTeamPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerTeamRepository entityRepository;

        public CustomerTeamService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerTeamRepository(objectContext);
        }

        public void Create(CustomerTeamPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new CustomerTeam();
            this.Poco.Id = IdCounter.GetNumber("CustomerTeam", entityPm.Tenant).ToString();
            entityPm.Id = this.Poco.Id;
            CustomerTeamMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerTeamPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerTeam(entityPM.Id, entityPm.Tenant);
            CustomerTeamMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
