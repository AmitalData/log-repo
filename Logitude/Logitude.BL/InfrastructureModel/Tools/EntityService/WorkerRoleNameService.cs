using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class WorkerRoleNameService
    {
        bool isNewEntity;
        public WorkerRoleName Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private WorkerRoleNamePM entityPm;
        private IWebFreightContext objectContext;
        private WorkerRoleNameRepository entityRepository;
        public WorkerRoleNameService(IWebFreightContext objectContext)
        {
            this.ObjectContext = objectContext;
            this.entityRepository = new WorkerRoleNameRepository(objectContext);
        }

        public void Create(WorkerRoleNamePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new WorkerRoleName();
            this.Poco.Name = this.entityPm.Name;

            WorkerRoleNameMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(WorkerRoleNamePM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleWorkerRoleName(entityPm.Name);

            WorkerRoleNameMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
