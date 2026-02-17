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
    public class CardExternalAccountsByProductService
    {
        private int tenant;
        bool isNewEntity;
        private Contact loggedContact;
        private ICommonDataContext objectContext;
        public CardExternalAccountsByProduct Poco { get; set; }
        private CardExternalAccountsByProductPM entityPM;
        private CardExternalAccountsByProductRepository entityRepository;
        private ContactRepository contactRepository;
        public CardExternalAccountsByProductService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.contactRepository = new ContactRepository(objectContext);
            this.entityRepository = new CardExternalAccountsByProductRepository(objectContext);
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

        public void Create(CardExternalAccountsByProductPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("CardExternalAccountsByProduct", tenant).ToString();
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;

            this.Poco = new CardExternalAccountsByProduct()
            {
                Id = entityPM.Id,
                Tenant = tenant,
            };

            CardExternalAccountsByProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CardExternalAccountsByProductPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;

            this.Poco = entityRepository.GetSingleCardExternalAccountsByProduct(entityPM.Id, tenant);

            CardExternalAccountsByProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
