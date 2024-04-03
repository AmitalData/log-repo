using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public partial class CargoTrackingMilestoneQueryService
    {
        public List<CargoTrackingMilestoneList> GetList(int tenant)
        {
            CargoTrackingMilestoneListQueryService cargoTrackingMilestoneQuery = new CargoTrackingMilestoneListQueryService(context);
            List<CargoTrackingMilestoneList> result = cargoTrackingMilestoneQuery.GetList(tenant);
            result = GetPremetedMilestone(result, tenant);
            return result;
        }

        private List<CargoTrackingMilestoneList> GetPremetedMilestone(List<CargoTrackingMilestoneList> result, int tenant)
        {
            CargoTenantMilestoneDefinitionRepository cargoTenantMilestoneDefinitionRepository = new CargoTenantMilestoneDefinitionRepository(tenant);
            var permission = cargoTenantMilestoneDefinitionRepository.GetAll(tenant).ToDictionary(e=>e.Code,e=>e);
            var PremetedMilestone = result.Where(e => HasPermission(permission, e)).ToList();
            return PremetedMilestone;

        }

        private bool HasPermission(Dictionary<string, CargoTenantMilestoneDefinition> permission, CargoTrackingMilestoneList e)
        {
            if (!permission.ContainsKey(e.Code))
                return true;
            if(!permission[e.Code].IsCustomerView)
                return false;
            return true;
        }
    }
}
