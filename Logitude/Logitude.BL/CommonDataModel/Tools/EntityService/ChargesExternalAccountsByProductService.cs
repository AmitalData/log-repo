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
    public class ChargesExternalAccountsByProductService
    {
        private int tenant;
        bool isNewEntity;
        private Contact loggedContact;
        private ICommonDataContext objectContext;
        public ChargesExternalAccountsByProduct Poco { get; set; }
        private ChargesExternalAccountsByProductPM entityPM;
        private ChargesExternalAccountsByProductRepository entityRepository;
        private ContactRepository contactRepository;
        public ChargesExternalAccountsByProductService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.contactRepository = new ContactRepository(objectContext);
            this.entityRepository = new ChargesExternalAccountsByProductRepository(objectContext);
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

        public void Create(ChargesExternalAccountsByProductPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("ChargesExternalAccountsByProduct", tenant).ToString();
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;

            this.Poco = new ChargesExternalAccountsByProduct()
            {
                Id = entityPM.Id,
                Tenant = tenant,
            };

            ChargesExternalAccountsByProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ChargesExternalAccountsByProductPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;

            this.Poco = entityRepository.GetSingleChargesExternalAccountsByProduct(entityPM.Id, tenant);

            ChargesExternalAccountsByProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
