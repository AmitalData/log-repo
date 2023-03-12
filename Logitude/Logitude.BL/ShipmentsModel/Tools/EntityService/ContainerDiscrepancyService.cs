using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerDiscrepancyService
    {
        private bool isNewEntity;
        private ContainerDiscrepancyPM containerDiscrepancyPM;
        private ContainerDiscrepancy containerDiscrepancyPoco;
        private int tenant;
        private IShipmentsContext shipmentsContext;
        private ContainerDiscrepancyRepository containerDiscrepancyRepository;
        public ContainerDiscrepancyService(int tenant)
        {
           

            this.tenant = tenant;
            this.containerDiscrepancyRepository = new ContainerDiscrepancyRepository(tenant);
        }

        public void Create(ContainerPM containerPM, ShipmentPM shipmentPM, string reasonOfDiscrepancy)
        {
           
           
            this.containerDiscrepancyPoco = new ContainerDiscrepancy {
                Id = IdCounter.GetNumber("ContainerDiscrepancy", tenant).ToString(),
                ContainerId = containerPM.Id,
                ShipmentId = shipmentPM.Id,
                Discrepancy = reasonOfDiscrepancy,
                DiscrepancyDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date,
                Tenant = tenant,
                
            };

            containerDiscrepancyPoco.SearchFields = containerPM.ShipmentNumber + "," + containerPM.ContainerNumber + "," + containerDiscrepancyPoco.DiscrepancyDate;

            containerDiscrepancyRepository.Add(containerDiscrepancyPoco);
            containerDiscrepancyRepository.SubmitChanges();

        }

        public ContainerDiscrepancy GetContainerDiscrepancyByContainerIdAndDiscrepancyReason(int tenant, string containerId, string shipmentId, string discrepancyReason)
        {
            return containerDiscrepancyRepository.GetContainerDiscrepancyByContainerIdAndDiscrepancyReason(tenant, containerId, shipmentId, discrepancyReason);
            
        }
        public void Update(ContainerDiscrepancyPM entityPM)
        {

        }
    }

}
