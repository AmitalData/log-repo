using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CargoTenantMilestoneDefinitionQuery
    {
        CargoTenantMilestoneDefinitionRepository repository;

  

        public CargoTenantMilestoneDefinitionQuery(int tenant)
        {
            repository = new CargoTenantMilestoneDefinitionRepository(tenant);
        }

        public CargoTenantMilestoneDefinitionQuery(CargoTenantMilestoneDefinitionRepository repository)
        {
            this.repository = repository;
        }

        public CargoTenantMilestoneDefinitionPM GetSinglePM(string code, int tenant)
        {
            CargoTenantMilestoneDefinitionPM result = null;
            CargoTenantMilestoneDefinition entityPoco = repository.GetSingleCargoTenantMilestoneDefinitionByCodeAndTenant(code, tenant);

            if (entityPoco != null)
            {
                result = new CargoTenantMilestoneDefinitionPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Code = entityPoco.Code,
                    IsCustomerView = entityPoco.IsCustomerView,
                };
            }

            return result;
        }

        public List<CargoTenantMilestoneDefinitionPM> GetAll(int tenant)
        {
            List<CargoTenantMilestoneDefinitionPM> cargoTenantMilestoneDefinitionsPM = new List<CargoTenantMilestoneDefinitionPM>();
            List<CargoTenantMilestoneDefinition> cargoTenantMilestoneDefinitionsPoco = repository.GetAll(tenant);
            MapCargoTenantMilestoneDefinitionsPOCOToCargoTenantMilestoneDefinitionsPM(cargoTenantMilestoneDefinitionsPM, cargoTenantMilestoneDefinitionsPoco);

            return cargoTenantMilestoneDefinitionsPM;
        }

        private void MapCargoTenantMilestoneDefinitionsPOCOToCargoTenantMilestoneDefinitionsPM(List<CargoTenantMilestoneDefinitionPM> cargoTenantMilestoneDefinitionsPM, List<CargoTenantMilestoneDefinition> cargoTenantMilestoneDefinitionsPoco)
        {
            cargoTenantMilestoneDefinitionsPoco.ForEach(cargoTenantMilestone =>
            {
                cargoTenantMilestoneDefinitionsPM.Add(new CargoTenantMilestoneDefinitionPM()
                {
                    Id = cargoTenantMilestone.Id,
                    Tenant = cargoTenantMilestone.Tenant,
                    Code = cargoTenantMilestone.Code,
                    IsCustomerView = cargoTenantMilestone.IsCustomerView,
                });
            });
        }

        public IQueryable<CargoTenantMilestoneDefinitionList> GetIQueryableEntityList(IQueryable<CargoTenantMilestoneDefinition> iQueryable,int tenant)
        {
            IQueryable<CargoTenantMilestoneDefinitionList> result = from entity in iQueryable
                                                                    where entity.Tenant == tenant
                                                                     select new CargoTenantMilestoneDefinitionList()
                                                                     {
                                                                         Id = entity.Id,
                                                                         Code = entity.Code,
                                                                         Tenant = entity.Tenant,
                                                                         IsCustomerView = entity.IsCustomerView,
                                                                     };
            return result;
        }
    }
}
