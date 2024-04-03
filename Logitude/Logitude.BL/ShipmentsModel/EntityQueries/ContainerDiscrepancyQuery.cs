using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerDiscrepancyQuery
    {
        ContainerDiscrepancyRepository repository;
        private bool isMultipleUpdate = false;
        public ContainerDiscrepancyQuery(int tenant)
        {
            repository = new ContainerDiscrepancyRepository(tenant);
        }
        public ContainerDiscrepancyQuery(ContainerDiscrepancyRepository myRepository)
        {
            this.repository = myRepository;
        }
        public ContainerDiscrepancyPM GetSinglePM(string id, int tenant)
        {
            ContainerDiscrepancyPM containerDiscrepancyPM = null;
            ContainerDiscrepancy containerDiscrepancy = repository.GetSingleContainerDiscrepancy(id, tenant);
            if (containerDiscrepancy != null)
            {
                containerDiscrepancyPM = new ContainerDiscrepancyPM()
                {
                    Id = containerDiscrepancy.Id,
                    Tenant = containerDiscrepancy.Tenant,
                    ContainerId = containerDiscrepancy.ContainerId,
                    ShipmentId = containerDiscrepancy.ShipmentId,
                    DiscrepancyDate = containerDiscrepancy.DiscrepancyDate,
                    Discrepancy = containerDiscrepancy.Discrepancy,

                };

            }
            return containerDiscrepancyPM;
        }
    }
}
