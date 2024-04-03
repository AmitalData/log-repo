using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using System;
using Newtonsoft.Json;
using Logitude.BL.InfrastructureModel.DataContracts;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class MultiEntityUpdateLogService
    {
        bool isNewEntity;
        private int tenant;

        private MultiEntityUpdateLogPM entityPM;
        public MultiEntityUpdateLog Poco { get; set; }

        private MultiEntityUpdateLogRepository entityRepository;

        private IWebFreightContext objectContext;

        private Contact loggedContact;

        private ContactRepository contactRepository;

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        public MultiEntityUpdateLogService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MultiEntityUpdateLogRepository(objectContext);

            this.contactRepository = new ContactRepository(CommonDataContext.GetContext(tenant));
            this.loggedContact = GetLoggedContact();
        }

        public void Create(MultiEntityUpdateLogPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(theEntityPm.MultiEntityUpdateData);// ConvertToXml(theEntityPm.MultiEntityUpdateData);
            if (this.loggedContact != null)
            {
                this.entityPM.CreatedByUserId = this.loggedContact.Id;
            }

            this.Poco = new MultiEntityUpdateLog();
            MultiEntityUpdateLogMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            this.entityPM.Id = Poco.Id;
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            BuildQueue(Poco);

        }

        private void BuildQueue(MultiEntityUpdateLog poco)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("MultiEntityUpdateQueue", poco.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "MultiEntityUpdateId", poco.Id }, { "Tenant", poco.Tenant.ToString() }, }, poco.Tenant);
        }

        public void Update(MultiEntityUpdateLogPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMultiEntityUpdateLog(theEntityPm.Id);
            MultiEntityUpdateLogMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private string ConvertToXml(MultiEntityUpdateData multiEntityUpdateData)
        {

            System.Type type1 = typeof(AutomationSetValue);
            System.Type type2 = typeof(MultiEntityUpdateDataEntity);
            System.Type type3 = "string".GetType();


            System.Type[] types = new System.Type[3];
            types[0] = type1;
            types[1] = type2;
            types[2] = type3;

            return LogitudeXmlSerializer.SerializeObjectToElementString(multiEntityUpdateData, types);
        }

        private Contact GetLoggedContact()
        {
            string email;
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                email = "system@tenant" + tenant.ToString() + ".com";
            }
            return contactRepository.GetSingleContactByEmail(email, tenant);
        }

    }
}
