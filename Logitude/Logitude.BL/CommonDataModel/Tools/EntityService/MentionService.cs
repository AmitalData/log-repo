using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class MentionService
    {
        private readonly int tenant;
        private readonly ICommonDataContext objectContext;
        private readonly MentionRepository entityRepository;
        private readonly ContactRepository contactRepository;
        private bool isNewEntity;
        private MentionPM entityPM;
        private Mention poco;
        private Contact loggedContact;

        public MentionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new MentionRepository(objectContext);
            this.contactRepository = new ContactRepository(tenant);
            GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null)
            {
                this.loggedContact = contactRepository.GetSingleContactByEmail(HttpContext.Current.User.Identity.Name, tenant);
                return;
            }
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
        }
        public void Create(MentionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;
            this.entityPM.CreatedByUserId = this.loggedContact.Id;
            this.entityPM.Tenant = this.tenant;

            this.poco = new Mention();
            MentionMapping.MapEntity(entityPM, poco, isNewEntity);
            entityRepository.Add(poco);
            entityRepository.SubmitChanges();
        }

        public void Update(MentionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPM.UpdatedByUserId = loggedContact.Id;
            this.entityPM.Tenant = this.tenant;

            this.poco = entityRepository.GetSingleMention(this.entityPM.Id, this.tenant);
            MentionMapping.MapEntity(entityPM, poco, isNewEntity);

            entityRepository.Update(poco);
            entityRepository.SubmitChanges();
        }
    }
}
