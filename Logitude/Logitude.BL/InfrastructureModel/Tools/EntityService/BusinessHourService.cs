using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class BusinessHourService
    {
        bool isNewEntity;
        private int tenant;
        public BusinessHour Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private BusinessHourPM entityPm;
        private IWebFreightContext objectContext;
        private BusinessHourRepository entityRepository;
        private BusinessHoursHolidayRepository businessHoursHolidayRepository;
        private List<BusinessHoursHolidayPM> businessHoursHoildayChangeSet;

        public void SetChangeSet(List<BusinessHoursHolidayPM> businessHoursHoildayChangeSet)
        {
            this.businessHoursHoildayChangeSet = businessHoursHoildayChangeSet;
        }

        public BusinessHourService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new BusinessHourRepository(objectContext);
            this.businessHoursHolidayRepository = new BusinessHoursHolidayRepository(objectContext);
        }

        public void Create(BusinessHourPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("BusinessHour", tenant).ToString();
            this.Poco = new BusinessHour();
            this.Poco.Id = this.entityPm.Id;

            foreach (BusinessHoursHolidayPM itemPM in entityPM.BusinessHoursHolidays)
            {
                this.CreateBusinessHoursHoliday(itemPM);
            }

            BusinessHourMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        private void CreateBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("BusinessHoursHoliday", tenant).ToString();
            itemPM.Tenant = tenant;
            itemPM.BusinessHourId = entityPm.Id;

            BusinessHoursHoliday itemPoco = new BusinessHoursHoliday()
            {
                Id= itemPM.Id,
                BusinessHourId = itemPM.BusinessHourId,
                Tenant = itemPM.Tenant,
            };

            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            UserRepository userRepository = new UserRepository(entityPm.Tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPm.Tenant, true);

            itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant);
            itemPoco.UpdatedByUserId = loggedUser.Id;
            itemPoco.CreatedByUserId = loggedUser.Id;
            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant);

            BusinessHourMapping.MapBusinessHoursHolidayEntity(itemPM, itemPoco, true);
            businessHoursHolidayRepository.Add(itemPoco);
        }

        public void Update(BusinessHourPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBusinessHours(entityPM.Id , entityPm.Tenant);

            string entityName = "BusinessHour" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "BusinessHourPM" + entityPM.Id + entityPM.Tenant;

            if (mapComposition)
            {
                this.businessHoursHoildayChangeSet = entityPM.BusinessHoursHolidays;
            }

            this.UpdateBusinessHoursHolidayCollection();
            BusinessHourMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }

        public void Delete(BusinessHourPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBusinessHours(entityPM.Id, entityPm.Tenant);
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateBusinessHoursHolidayCollection()
        {
            if (businessHoursHoildayChangeSet != null)
            {
                foreach (BusinessHoursHolidayPM itemPM in businessHoursHoildayChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateBusinessHoursHoliday(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateBusinessHoursHoliday(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteBusinessHoursHoliday(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        {
            BusinessHoursHoliday itemPoco = businessHoursHolidayRepository.GetSingleBusinessHoursHolidays(itemPM.Id, tenant);
            BusinessHourMapping.MapBusinessHoursHolidayEntity(itemPM, itemPoco, false);
            businessHoursHolidayRepository.Update(itemPoco);
        }

        private void DeleteBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        {
            BusinessHoursHoliday itemPoco = businessHoursHolidayRepository.GetSingleBusinessHoursHolidays(itemPM.Id, tenant);
            businessHoursHolidayRepository.Remove(itemPoco);
        }
    }
}
