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
    public class CustomFieldsMainObjectService
    {
        bool isNewEntity;
        private int tenant;
        public CustomFieldsMainObject Poco { get; set; }
        private bool isChange = false;
        private IWebFreightContext context;
        public IWebFreightContext Context
        {
            get { return context; }
            set { context = value; }
        }
        private CustomFieldsMainObjectPM entityPM;
        private CustomFieldsMainObjectRepository entityRepository;
        private Contact loggedContact;

        public CustomFieldsMainObjectService(IWebFreightContext context, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new CustomFieldsMainObjectRepository(context);
            this.GetLoggedContact();
        }

        public void Create(CustomFieldsMainObjectPM customFieldsMainObjectPM)
        {
            this.isNewEntity = true;
            this.entityPM = customFieldsMainObjectPM;
            this.entityPM.Id = IdCounter.GetNumber("CustomFieldsMainObject", tenant).ToString();
            this.Poco = new CustomFieldsMainObject();
            CustomFieldsMainObjectMapping.MapEntity(customFieldsMainObjectPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomFieldsMainObjectPM customFieldsMainObjectPM)
        {
            this.isNewEntity = false;
            this.entityPM = customFieldsMainObjectPM;
            this.Poco = entityRepository.GetSingleCustomFieldsMainObject(customFieldsMainObjectPM.Id, customFieldsMainObjectPM.Tenant);
            if (this.Poco == null) return;
            CustomFieldsMainObjectMapping.MapEntity(customFieldsMainObjectPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(CustomFieldsMainObjectPM customFieldsMainObjectPM)
        {
            this.Poco = entityRepository.GetSingleCustomFieldsMainObject(customFieldsMainObjectPM.Id, customFieldsMainObjectPM.Tenant);
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