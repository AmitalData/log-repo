using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
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
        private int tenant;
        private IShipmentsContext shipmentsContext;
        private ContainerDiscrepancyRepository containerDiscrepancyRepository;
        public ContainerDiscrepancyService(IShipmentsContext shipmentsContext, int tenant)
        {
           

            this.tenant = tenant;
            this.shipmentsContext = shipmentsContext;
            this.containerDiscrepancyRepository = new ContainerDiscrepancyRepository(shipmentsContext);
        }

        public void Create(ContainerDiscrepancyPM entityPM)
        {
            
        }
        public void Update(ContainerDiscrepancyPM entityPM)
        {

        }
    }

}
