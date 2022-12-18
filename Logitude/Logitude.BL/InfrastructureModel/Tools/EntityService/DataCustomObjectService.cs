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
    public class DataCustomObjectService
    {
        bool isNewEntity;
        private int tenant;
        public DataCustomObject Poco { get; set; }
        private bool isChange = false;
        private IWebFreightContext context;
        public IWebFreightContext Context
        {
            get { return context; }
            set { context = value; }
        }
        private DataCustomObjectPM entityPM;
        private DataCustomObjectRepository entityRepository;
        private Contact loggedContact;

        public DataCustomObjectService(IWebFreightContext context, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new DataCustomObjectRepository(context);
            this.GetLoggedContact();
        }

        public void Create(DataCustomObjectPM dataCustomObjectPM)
        {
            this.isNewEntity = true;
            this.entityPM = dataCustomObjectPM;
            this.entityPM.Id = IdCounter.GetNumber("DataCustomObject", tenant).ToString();
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.entityPM.CreatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.CreatedBy;
            this.Poco = new DataCustomObject();
            DataCustomObjectMapping.MapEntity(dataCustomObjectPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DataCustomObjectPM dataCustomObjectPM)
        {
            this.isNewEntity = false;
            this.entityPM = dataCustomObjectPM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.Poco = entityRepository.GetSingleDataCustomObject(dataCustomObjectPM.Id, dataCustomObjectPM.Tenant);
            if (this.Poco == null) return;
            DataCustomObjectMapping.MapEntity(dataCustomObjectPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(DataCustomObjectPM dataCustomObjectPM)
        {
            this.Poco = entityRepository.GetSingleDataCustomObject(dataCustomObjectPM.Id, dataCustomObjectPM.Tenant);
            if (this.Poco == null) return;
            entityRepository.Remove(Poco);
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