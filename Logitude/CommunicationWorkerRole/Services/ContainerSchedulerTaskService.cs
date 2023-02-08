using CommunicationWorkerRole.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
                this.ManageClosedContainer(container);
            }
        }

        private void GetAllContainers()
        {
            shipmentsContext = ShipmentsContext.GetContext(0);
            allContainers = (from d in shipmentsContext.Containers
                             where !d.IsClosed
                             select d);
        }

        private void ManageClosedContainer(Container container)
        {
            var containerSetting = new ContainerSettingRepository(container.Tenant).GetAll(container.Tenant).FirstOrDefault();
            if (containerSetting == null) containerSetting = GetDefaultContainerSetting();
            else if (containerSetting.EmptyReturnClosingDays == null && containerSetting.ShipmentATAClosingDays == null) return;

            bool isClosingByEmptyReturn = containerSetting.EmptyReturnClosingDays != null && container.ActualEmptyReturn != null;
            bool isClosingByShipmentATA = containerSetting.ShipmentATAClosingDays != null && container.ShipmentMainCarriageATA != null;
            bool isUpdatingContainer = false;

            if (isClosingByEmptyReturn)
            {
                double emptyReturnDays = Convert.ToDouble(containerSetting.EmptyReturnClosingDays);
                DateTime? emptyReturnDate = container.ActualEmptyReturn.Value.AddDays(emptyReturnDays);
                if (emptyReturnDate.Value.Date <= todayDate.Date) isUpdatingContainer = true;
            }

            if (isClosingByShipmentATA)
            {
                double shipmentATADays = Convert.ToDouble(containerSetting.ShipmentATAClosingDays);
                DateTime? ShipmentATADate = container.ShipmentMainCarriageATA.Value.AddDays(shipmentATADays);
                if (ShipmentATADate.Value.Date <= todayDate.Date) isUpdatingContainer = true;
            }

            if (isUpdatingContainer)
            {
                this.UpdateClosedContainer(container);
            }
        }

        private ContainerSetting GetDefaultContainerSetting()
        {
            return new ContainerSetting
            {
                EmptyReturnClosingDays = 5,
                ShipmentATAClosingDays = 90,
                ShipmentATADateIndicator = "Vessel"
            };
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

