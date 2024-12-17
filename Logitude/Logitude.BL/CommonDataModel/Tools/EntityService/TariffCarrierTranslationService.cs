using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
    public class TariffCarrierTranslationService
    {
        bool isNewEntity;
        private int tenant;
        private string loggedContactId;
        public TariffCarrierTranslation Poco { get; set; }
        private ICommonDataContext objectContext;
        private TariffCarrierTranslationRepository entityRepository;
        public TariffCarrierTranslationService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TariffCarrierTranslationRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            string email = HttpContext.Current.User.Identity.Name;
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

            if (loggedContact == null)
            {
                loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                if (loggedContact.Tenant != tenant)
                {
                    loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                    this.loggedContactId = loggedContact.Id;
                }
                else
                {
                    this.loggedContactId = loggedContact.Id;
                }
            }
        }

        public void Create(TariffCarrierTranslationPM entityPM)
        {
            this.isNewEntity = true;

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            entityPM.Id = IdCounter.GetNumber("TariffCarrierTranslation", tenant).ToString();
            entityPM.CreateDate = entityPM.UpdateDate = todayDateTime;
            entityPM.CreatedByUserId = entityPM.UpdatedByUserId = this.loggedContactId;

            this.Poco = new TariffCarrierTranslation()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
            };

            TariffCarrierTranslationValidating.Validate(entityPM, objectContext, this.isNewEntity);
            TariffCarrierTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TariffCarrierTranslationPM entityPM)
        {
            this.isNewEntity = false;

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            entityPM.UpdateDate = todayDateTime;
            entityPM.UpdatedByUserId = this.loggedContactId;

            this.Poco = entityRepository.GetSingleTariffCarrierTranslation(entityPM.Id, tenant);

            TariffCarrierTranslationValidating.Validate(entityPM, objectContext, this.isNewEntity);
            TariffCarrierTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
