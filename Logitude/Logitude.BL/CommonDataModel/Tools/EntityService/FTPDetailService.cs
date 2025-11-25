using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class FTPDetailService
    {
        bool isNewEntity;
        private int tenant;
        public FTPDetail Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private FTPDetailPM entityPm;
        private ICommonDataContext objectContext;
        private FTPDetailRepository entityRepository;
        private ContactPM loggedContact;

        public FTPDetailService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FTPDetailRepository(objectContext);

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

        public void Create(FTPDetailPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("FTPDetail", tenant).ToString();
            this.Poco = new FTPDetail();
            this.Poco.Id = this.entityPm.Id;

            FTPDetailMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(FTPDetailPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleFTPDetail(entityPM.Id, entityPm.Tenant);

            FTPDetailMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
