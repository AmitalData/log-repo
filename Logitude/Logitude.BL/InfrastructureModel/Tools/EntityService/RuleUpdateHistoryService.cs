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
    public class RuleUpdateHistoryService
    {
        bool isNewEntity;
        private int tenant;
        public RuleUpdateHistory Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RuleUpdateHistoryPM entityPm;
        private IWebFreightContext objectContext;
        private RuleUpdateHistoryRepository entityRepository;
        public RuleUpdateHistoryService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RuleUpdateHistoryRepository(objectContext);
            
        }

        public void Create(RuleUpdateHistoryPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("RuleUpdateHistory", tenant).ToString();
            this.Poco = new RuleUpdateHistory();
            this.Poco.Id = this.entityPm.Id;

            RuleUpdateHistoryMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }
        public void Update(RuleUpdateHistoryPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleRuleUpdateHistory(entityPM.Id, entityPm.Tenant);
            
            RuleUpdateHistoryMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
    }
}
