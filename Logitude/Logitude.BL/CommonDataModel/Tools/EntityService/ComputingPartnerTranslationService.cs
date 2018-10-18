using System;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ComputingPartnerTranslationService
    {
        bool isNewEntity;
        private int tenant;
        private string loggedContactId;
        public ComputingPartnerTranslation Poco { get; set; }
        private ICommonDataContext objectContext;
        private ComputingPartnerTranslationRepository entityRepository;
        public ComputingPartnerTranslationService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ComputingPartnerTranslationRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;

            if (email != null)
            {
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                this.loggedContactId = loggedContact.Id;
            }
        }

        public void Create(ComputingPartnerTranslationPM entityPM)
        {
            this.isNewEntity = true;

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            entityPM.Id = IdCounter.GetNumber("ComputingPartnerTranslation", tenant).ToString();
            entityPM.CreateDate = entityPM.UpdateDate = todayDateTime;
            entityPM.CreatedByUserId = entityPM.UpdatedByUserId = this.loggedContactId;

            this.Poco = new ComputingPartnerTranslation()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
            };

            ComputingPartnerTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ComputingPartnerTranslationPM entityPM)
        {
            this.isNewEntity = false;

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            entityPM.UpdateDate = todayDateTime;
            entityPM.UpdatedByUserId = this.loggedContactId;

            this.Poco = entityRepository.GetSingleComputingPartnerTranslation(entityPM.Id);

            ComputingPartnerTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
