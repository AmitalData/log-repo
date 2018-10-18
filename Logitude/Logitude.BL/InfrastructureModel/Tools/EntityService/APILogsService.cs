using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.EntityPMs;
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
using System.Transactions;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class APILogsService
    {
        bool isNewEntity;
        private int tenant;
        public APILogs Poco { get; set; }
        public APILogsData APILogsDataPoco { get; set; }
        

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private APILogsPM entityPm;
        private IWebFreightContext objectContext;
        private APILogsRepository entityRepository;
        private APILogsDataRepository entityDataRepository;

        public APILogsService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new APILogsRepository(objectContext);
            this.entityDataRepository = new APILogsDataRepository(objectContext);
        }

        public void Create(APILogsPM entityPM)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetNewTransaction())
            {
                this.isNewEntity = true;
                this.entityPm = entityPM;
                this.entityPm.Id = IdCounter.GetNumber("APILogs", tenant).ToString();
                this.Poco = new APILogs();
                this.Poco.Id = this.entityPm.Id;
                APILogsDataPM EntityDataPM = new APILogsDataPM() { Id = entityPM.Id, Tenant = entityPM.Tenant }; 
                APILogsMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                CreateAPILogsData(EntityDataPM);
                scope.Complete();
            }
        }

        private void CreateAPILogsData(APILogsDataPM entityPM)
        {
            APILogsDataPoco = new APILogsData() { Id = entityPM.Id, Tenant = entityPM.Tenant };
            APILogsDataMapping.MapEntity(entityPM, APILogsDataPoco, true);
            entityDataRepository.Add(APILogsDataPoco);
            entityDataRepository.SubmitChanges();
        }

        //private void CreateBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        //{
        //    itemPM.Id = IdCounter.GetNumber("BusinessHoursHoliday", tenant).ToString();
        //    itemPM.Tenant = tenant;
        //    itemPM.BusinessHourId = entityPm.Id;

        //    BusinessHoursHoliday itemPoco = new BusinessHoursHoliday()
        //    {
        //        Id= itemPM.Id,
        //        BusinessHourId = itemPM.BusinessHourId,
        //        Tenant = itemPM.Tenant,
        //    };

        //    string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
        //    UserRepository userRepository = new UserRepository(entityPm.Tenant);
        //    User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPm.Tenant, true);

        //    itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant);
        //    itemPoco.UpdatedByUserId = loggedUser.Id;
        //    itemPoco.CreatedByUserId = loggedUser.Id;
        //    itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant);

        //    BusinessHourMapping.MapBusinessHoursHolidayEntity(itemPM, itemPoco, true);
        //    businessHoursHolidayRepository.Add(itemPoco);
        //}

        public void Update(APILogsPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleAPILogs(entityPM.Id, entityPM.Tenant);
            APILogsDataPM EntityDataPM = new APILogsDataPM() { Id = entityPM.Id, Tenant = entityPM.Tenant, RequestData = entityPM.RequestData, ResponseData = entityPM.ResponseData, DiagnosticLog = entityPM.DiagnosticLog, ExceptionsMessage = entityPM.ExceptionsMessage };
            UpdateAPILogsData(EntityDataPM);
            APILogsMapping.MapEntity(entityPM, Poco, isNewEntity); 
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }

        private void UpdateAPILogsData(APILogsDataPM EntityDataPM)
        {
            this.APILogsDataPoco = entityDataRepository.GetSingleAPILogsData(EntityDataPM.Id, EntityDataPM.Tenant);
            APILogsDataMapping.MapEntity(EntityDataPM, APILogsDataPoco, false);
            entityDataRepository.Update(APILogsDataPoco);
            entityDataRepository.SubmitChanges();
        }

        //private void UpdateBusinessHoursHolidayCollection()
        //{
        //    if (businessHoursHoildayChangeSet != null)
        //    {
        //        foreach (BusinessHoursHolidayPM itemPM in businessHoursHoildayChangeSet)
        //        {
        //            switch (itemPM.ChangeSetOp)
        //            {
        //                case ChangeSetOperation.Insert:
        //                    {
        //                        this.CreateBusinessHoursHoliday(itemPM);
        //                        break;
        //                    }

        //                case ChangeSetOperation.Update:
        //                    {
        //                        this.UpdateBusinessHoursHoliday(itemPM);
        //                        break;
        //                    }

        //                case ChangeSetOperation.Delete:
        //                    {
        //                        this.DeleteBusinessHoursHoliday(itemPM);
        //                        break;
        //                    }

        //                default: { break; }
        //            }
        //        }
        //    }
        //}

        //private void UpdateBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        //{
        //    BusinessHoursHoliday itemPoco = businessHoursHolidayRepository.GetSingleBusinessHoursHolidays(itemPM.Id, tenant);
        //    BusinessHourMapping.MapBusinessHoursHolidayEntity(itemPM, itemPoco, false);
        //    businessHoursHolidayRepository.Update(itemPoco);
        //}

        //private void DeleteBusinessHoursHoliday(BusinessHoursHolidayPM itemPM)
        //{
        //    BusinessHoursHoliday itemPoco = businessHoursHolidayRepository.GetSingleBusinessHoursHolidays(itemPM.Id, tenant);
        //    businessHoursHolidayRepository.Remove(itemPoco);
        //}
    }
}
