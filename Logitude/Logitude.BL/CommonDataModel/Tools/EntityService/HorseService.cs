using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
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
    public class HorseService
    {
        bool isNewEntity;
        private int tenant;
        public Horse Poco { get; set; }
        private HorsePM entityPM;
        private ICommonDataContext objectContext;
        private HorseRepository entityRepository;
        private Contact loggedContact;
        public HorseService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new HorseRepository(objectContext);

            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(objectContext);

            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }
        
        public void Create(HorsePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.entityPM.Id = IdCounter.GetNumber("Horse", tenant).ToString();
            this.Poco = new Horse();
            this.Poco.Id = this.entityPM.Id;
            
            HorseTracing.Trace(entityPM, Poco, isNewEntity, loggedContact.Id);
            HorseMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(HorsePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleHorse(entityPM.Id, entityPM.Tenant);
            
            HorseTracing.Trace(entityPM, Poco, isNewEntity, loggedContact.Id);
            HorseMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
