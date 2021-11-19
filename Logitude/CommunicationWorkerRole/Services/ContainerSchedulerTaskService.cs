using CommunicationWorkerRole.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Linq;

namespace CommunicationWorkerRole.Services
{
    public class ContainerSchedulerTaskService
    {
        TaskManagerBase currentTask;
        IShipmentsContext shipmentsContext;
        IShipmentsContext shipmentsContext_Loop;
        DateTime todayDate = DateTime.Now;
        ContainerQuery containerQuery;
        ContainerService containerService;
        ContainerPM containerPM;
        IQueryable<Container> allContainers;

        public ContainerSchedulerTaskService(TaskManagerBase task)
        {
            this.currentTask = task;
        }

        public void ExecuteDailyAutomaticallyClosingContainers()
        {
            this.GetAllContainers();
            foreach (Container container in allContainers)
            {
                this.UpdateClosedContainer(container);
            }
        }

        private void GetAllContainers()
        {
            shipmentsContext = ShipmentsContext.GetContext(0);
            allContainers = (from d in shipmentsContext.Containers
                                                   where !d.IsClosed
                                                   && System.Data.Entity.DbFunctions.TruncateTime(d.ActualEmptyReturn) <= System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                                                   select d);
        }

        private void UpdateClosedContainer(Container container)
        {
            shipmentsContext_Loop = ShipmentsContext.GetContext(container.Tenant);
            containerQuery = new ContainerQuery(container.Tenant);
            containerService = new ContainerService(shipmentsContext_Loop, container.Tenant);
            containerPM = containerQuery.GetSinglePM(container.Id, container.Tenant);
            containerPM.UpdatedByUserId = this.GetSystemUser(container.Tenant);
            containerPM.IsClosed = true;
            containerPM.ClosedDate = TenantServerConfigration.GetCurrentDateTime(container.Tenant);
            containerService.Update(containerPM);
        }

        private string GetSystemUser(int tenant)
        {
            var email = "system@tenant" + tenant + ".com";
            string contactId = "";
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(email, tenant, true);
            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
            }
            contactId = loggedContact?.Id;
            return contactId;
        }
    }
}

