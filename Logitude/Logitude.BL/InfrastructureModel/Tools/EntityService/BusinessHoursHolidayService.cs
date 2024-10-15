using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class BusinessHoursHolidayService
    { 
        bool isNewEntity;
        private int tenant;
        public BusinessHoursHoliday Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private BusinessHoursHolidayPM entityPm;
        private IWebFreightContext objectContext;
        private BusinessHoursHolidayRepository entityRepository;
        public BusinessHoursHolidayService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new BusinessHoursHolidayRepository(objectContext);
        }

        public void Create(BusinessHoursHolidayPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("BusinessHoursHoliday", tenant).ToString();
            this.Poco = new BusinessHoursHoliday();
            this.Poco.Id = this.entityPm.Id;
            if (this.Poco.BusinessHourId!=null) 
                 this.Poco.BusinessHourId = ObjectContext.BusinessHours.Where(b => b.Tenant == entityPM.Tenant && b.Code == "BUS").FirstOrDefault()?.Id;
            BusinessHoursHolidayMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(BusinessHoursHolidayPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBusinessHoursHolidays(entityPM.Id , entityPm.Tenant);
            if (this.Poco.BusinessHourId != null)
                this.Poco.BusinessHourId = ObjectContext.BusinessHours.Where(b => b.Tenant == entityPM.Tenant && b.Code == "BUS").FirstOrDefault()?.Id;
            string entityName = "BusinessHoursHoliday" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "BusinessHoursHolidayPM" + entityPM.Id + entityPM.Tenant;
           
            BusinessHoursHolidayMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }


        public void Delete(BusinessHoursHolidayPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBusinessHoursHolidays(entityPM.Id, entityPm.Tenant);
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }
    }
}