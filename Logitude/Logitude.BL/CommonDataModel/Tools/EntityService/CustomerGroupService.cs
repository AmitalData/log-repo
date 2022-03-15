using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;


namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerGroupService
    {
        bool isNewEntity;
        private int tenant;
        private Contact loggedContact;
        private ContactRepository contactRepository;
        public CustomerGroup Poco { get; set; }
        private CustomerGroupPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerGroupRepository entityRepository;
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        public CustomerGroupService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.contactRepository = new ContactRepository(objectContext);
            this.entityRepository = new CustomerGroupRepository(objectContext);
            this.GetLoggedContact();
        }

        public void Create(CustomerGroupPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new CustomerGroup();
            entityPM.Id = IdCounter.GetNumber("CustomerGroup", entityPM.Tenant).ToString();
            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            if (loggedContact != null)
            {
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
            }
            CustomerGroupTracing.Trace(entityPM, isNewEntity);
            CustomerGroupMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerGroupPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerGroup(entityPM.Id, entityPm.Tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }
            CustomerGroupTracing.Trace(entityPM, isNewEntity);
            CustomerGroupMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void GetLoggedContact()
        {
            string loggedUseremail = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(loggedUseremail, tenant);
        }
    }
}
