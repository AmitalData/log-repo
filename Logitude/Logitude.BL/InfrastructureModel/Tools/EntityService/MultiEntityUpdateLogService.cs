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
            this.GetLoggedContact();
        }

        public void Create(MultiEntityUpdateLogPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.XMLData = BuildXmlData(this.entityPM.XMLData);
            if (this.loggedContact != null)
            {
                this.entityPM.CreatedByUserId = this.loggedContact.Id;
            }

            this.Poco = new MultiEntityUpdateLog();
            MultiEntityUpdateLogMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
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

        private string BuildXmlData(string jsonData)
        {
            var multiEntityUpdateData = JsonConvert.DeserializeObject<MultiEntityUpdateData>(jsonData);

            System.Type type1 = typeof(AutomationSetValue);
            System.Type type2 = typeof(MultiEntityUpdateData);
            System.Type type3 = typeof(MultiEntityUpdateDataEntity);
            System.Type type4 = "string".GetType();


            System.Type[] types = new System.Type[4];
            types[0] = type1;
            types[1] = type2;
            types[2] = type3;
            types[3] = type4;

            return LogitudeXmlSerializer.SerializeObjectToElementString(multiEntityUpdateData, types);
        }

        private void GetLoggedContact()
        {

            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
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
