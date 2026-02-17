using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AgentSharedDocumentService
    {
        bool isNewEntity;
        private int tenant;
        public AgentSharedDocument Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AgentSharedDocumentPM entityPm;
        private ICommonDataContext objectContext;
        private AgentSharedDocumentRepository entityRepository;


        private Contact loggedContact;
        public AgentSharedDocumentService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AgentSharedDocumentRepository(objectContext);

        }

     

        public void Create(AgentSharedDocumentPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("AgentSharedDocument", tenant).ToString();
            this.Poco = new AgentSharedDocument();

            AgentSharedDocumentMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AgentSharedDocumentPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            this.Poco = entityRepository.GetSingleAgentSharedDocument(entityPM.Id, entityPm.Tenant);
            AgentSharedDocumentMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }


       
    }
}
