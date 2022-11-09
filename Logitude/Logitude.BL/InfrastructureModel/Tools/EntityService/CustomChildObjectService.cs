using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class CustomChildObjectService
    {

        bool isNewEntity;
        private int tenant;
        public CustomChildObject Poco { get; set; }
        private bool isChange = false;
        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomChildObjectPM entityPM;
        private IWebFreightContext objectContext;
        private CustomChildObjectRepository entityRepository;
        private Contact loggedContact;

        public CustomChildObjectService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomChildObjectRepository(objectContext);
            this.GetLoggedContact();

        }

        public void Create(CustomChildObjectPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CustomChildObject", tenant).ToString();
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.entityPM.CreatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.CreatedBy;
            this.Poco = new CustomChildObject();
            CustomChildObjectMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            this.isChange = true;

        }

        public void Update(CustomChildObjectPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;

            this.Poco = entityRepository.GetById(theEntityPm.Id, entityPM.Tenant);
            if (this.Poco == null) return;
            CustomChildObjectMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            this.isChange = true;
        }


        public void Delete(CustomChildObjectPM theEntityPm)
        {
            this.Poco = entityRepository.GetById(theEntityPm.Id, theEntityPm.Tenant);
            if (this.Poco == null) return;
            entityRepository.Remove(Poco);
            this.isChange = true;
        }

        public void Updates(List<CustomChildObjectPM> customChildObjects)
        {

            customChildObjects.ForEach((customChildObject) =>
            {
                switch (customChildObject.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert: { Create(customChildObject); break; }
                    case ChangeSetOperation.Update: { Update(customChildObject); break; }
                    case ChangeSetOperation.Delete: { Delete(customChildObject); break; }
                    default: { break; }
                }
            });

            if (!this.isChange) return;
               
            entityRepository.SubmitChanges();


        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail((HttpContext.Current.User.Identity.Name), tenant , true);
                return;
            }

            this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail(("system@tenant" + tenant.ToString() + ".com"), tenant,true);
        }


    }
}