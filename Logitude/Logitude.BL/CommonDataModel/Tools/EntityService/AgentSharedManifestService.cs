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
    public class AgentSharedManifestService
    {
        bool isNewEntity;
        private int tenant;
        public AgentSharedManifest Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AgentSharedManifestPM entityPm;
        private ICommonDataContext objectContext;
        private AgentSharedManifestRepository entityRepository;
        private SharedManifestTranslationRepository sharedManifestTranslationRepository;
        private ContactRepository contactRepository;

        private Contact loggedContact;
        public AgentSharedManifestService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AgentSharedManifestRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.sharedManifestTranslationRepository = new SharedManifestTranslationRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
        }

        public void Create(AgentSharedManifestPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.Poco = new AgentSharedManifest();

            AgentSharedManifestMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AgentSharedManifestPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPm.UpdatedByUserId = this.loggedContact.Id;

            /*
             * 
             * AgentId takes the default agent in the destination tenant
             * 
             */


            foreach (SharedManifestTranslationPM trans in entityPM.SharedManifestTranslations)
            {
                trans.AgentId = entityPM.AgentId;
                switch (trans.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        this.CreateSharedManifestTranslation(trans);
                        break;
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        this.UpdateSharedManifestTranslation(trans);
                        break;
                }
            }



            this.Poco = entityRepository.GetSingleAgentSharedManifest(entityPM.Id, entityPm.Tenant);
            AgentSharedManifestMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }


        private void CreateSharedManifestTranslation(SharedManifestTranslationPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("SharedManifestTranslation", itemPM.Tenant).ToString();
            itemPM.CreatedByUserId = loggedContact.Id;
            itemPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);

            itemPM.UpdatedByUserId = loggedContact.Id;
            itemPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);

            SharedManifestTranslation itemPoco = new SharedManifestTranslation()
            {
                Id = itemPM.Id,
                Tenant = tenant,
            };
            SharedManifestTranslationMapping.MapEntity(itemPM, itemPoco, true);
            this.sharedManifestTranslationRepository.Add(itemPoco);
        }

        private void UpdateSharedManifestTranslation(SharedManifestTranslationPM itemPM)
        {
            SharedManifestTranslation itemPoco = sharedManifestTranslationRepository.GetSingleSharedManifestTranslation(itemPM.Id, tenant);
            SharedManifestTranslationMapping.MapEntity(itemPM, itemPoco, false);

            sharedManifestTranslationRepository.Update(itemPoco);

        }

    }
}
