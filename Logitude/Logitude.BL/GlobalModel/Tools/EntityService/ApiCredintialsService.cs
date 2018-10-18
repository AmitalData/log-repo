using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class ApiCredintialsService
    {
        int tenant;
        bool isNewEntity;
        private string loggedContactId;
        private string loggedContactName;
        private ApiCredintialsPM entityPM;
        private ApiCredintials entityPoco { get; set; }
        private IGlobalContext objectContext;
        private ApiCredintialsRepository entityRepository;
        public ApiCredintialsService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ApiCredintialsRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                string email = HttpContext.Current.User.Identity.Name;

                ContactRepository contactRepository = new ContactRepository(tenant);

                if (email != null)
                {
                    Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                    this.loggedContactId = loggedContact.Id;
                    this.loggedContactName = loggedContact.EnglishName;
                }

                else
                {
                    ContactPM loggedContact = new ContactQuery(contactRepository).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                    this.loggedContactId = loggedContact.Id;
                    this.loggedContactName = loggedContact.EnglishName;
                }

                scope.Complete();
            }
        }

        public void Create(ApiCredintialsPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.entityPM.Id = IdCounter.GetNumber("ApiCredintials", entityPM.Tenant);
            this.entityPM.CreatedBy = this.entityPM.UpdatedBy = this.loggedContactName;
            this.entityPM.CreateDate = this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.entityPoco = new ApiCredintials();
            ApiCredintialsMapping.MapEntity(entityPM, entityPoco, this.isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(ApiCredintialsPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;

            this.entityPM.UpdatedBy = this.loggedContactName;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.entityPoco = entityRepository.GetSingleApiCredintials(entityPM.Id, tenant);
            ApiCredintialsMapping.MapEntity(entityPM, entityPoco, this.isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }
    }
}
