

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
    public class ReportsTemplatesVersionService
    {
        bool isNewEntity;
        private int tenant;
        public ReportsTemplatesVersion Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ReportsTemplatesVersionPM entityPm;
        private ICommonDataContext objectContext;
        private ReportsTemplatesVersionRepository entityRepository;

        private ContactRepository contactRepository;

        private Contact loggedContact;
        public ReportsTemplatesVersionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ReportsTemplatesVersionRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.GetLoggedContact();

        }



        public void Create(ReportsTemplatesVersionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (this.loggedContact != null)
            {
                this.entityPm.UpdatedByUserId = this.loggedContact.Id;
                this.entityPm.CreatedByUserId = this.loggedContact.Id;
            }

            this.Poco = new ReportsTemplatesVersion();

            ReportsTemplatesVersionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ReportsTemplatesVersionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (this.loggedContact != null)
            {
                this.entityPm.UpdatedByUserId = this.loggedContact.Id;
                this.entityPm.CreatedByUserId = this.loggedContact.Id;
            }

            this.Poco = entityRepository.GetSingleReportsTemplatesVersion(entityPM.Id, entityPm.Tenant);
            ReportsTemplatesVersionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


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

    }
}
