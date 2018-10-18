using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomsInterfaceSettingService
    {
        bool isNewEntity;
        private int tenant;
        public CustomsInterfaceSetting Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomsInterfaceSettingPM entityPM;
        private ICommonDataContext objectContext;
        private CustomsInterfaceSettingRepository entityRepository;
        private FTPDetailRepository fTPDetailRepository;
        private ContactPM loggedContact;

        public CustomsInterfaceSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomsInterfaceSettingRepository(objectContext);
            this.fTPDetailRepository = new FTPDetailRepository(objectContext);

            this.GetLoggedData();
        }

        private void GetLoggedData()
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(HttpContext.Current.User.Identity.Name, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(HttpContext.Current.User.Identity.Name, tenant);
            }
        }
        
        public void Create(CustomsInterfaceSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Tenant = this.tenant;
            this.Poco = new CustomsInterfaceSetting();
            this.Poco.Tenant = this.entityPM.Tenant;

            CustomsInterfaceSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomsInterfaceSettingPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCustomsInterfaceSetting(entityPM.Tenant, 0);
            
            CustomsInterfaceSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
