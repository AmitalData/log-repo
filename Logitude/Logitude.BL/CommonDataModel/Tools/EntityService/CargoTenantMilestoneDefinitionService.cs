using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CargoTenantMilestoneDefinitionService
    {
        bool isNewEntity;
        private int tenant;
        private CargoTenantMilestoneDefinitionPM entityPM;
        private ICommonDataContext objectContext;
        private CargoTenantMilestoneDefinitionRepository entityRepository;
        public CargoTenantMilestoneDefinition Poco { get; set; }
        public CargoTenantMilestoneDefinitionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CargoTenantMilestoneDefinitionRepository(objectContext);
        }

        public void Create(CargoTenantMilestoneDefinitionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("CargoTenantMilestoneDefinition", tenant).ToString();
            this.Poco = new CargoTenantMilestoneDefinition();
            this.Poco.Id = this.entityPM.Id;

            CargoTenantMilestoneDefinitionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CargoTenantMilestoneDefinitionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCargoTenantMilestoneDefinition(entityPM.Id, tenant);

            CargoTenantMilestoneDefinitionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void UpdateCargoTenantMilestoneDefinitionsPM(List<CargoTenantMilestoneDefinitionPM> cargoTenantZeroMilestoneDefinitionPMs, int tenant)
        {
            CargoTenantMilestoneDefinitionQuery cargoTrackingMilestoneQuery = new CargoTenantMilestoneDefinitionQuery(tenant);
            List<CargoTenantMilestoneDefinitionPM> cargoMyTenantMilestoneDefinitionPMs = cargoTrackingMilestoneQuery.GetAll(tenant);

            cargoTenantZeroMilestoneDefinitionPMs.ForEach(cargoTenantZeroMilestoneDefinition =>
            {
                CargoTenantMilestoneDefinitionPM cargoMyTenantMilestoneDefinition = cargoMyTenantMilestoneDefinitionPMs.FirstOrDefault(f => f.Code == cargoTenantZeroMilestoneDefinition.Code);
                if (cargoMyTenantMilestoneDefinition == null)
                {
                    Create(cargoTenantZeroMilestoneDefinition);
                }
                else
                {
                    cargoMyTenantMilestoneDefinition.IsCustomerView = cargoTenantZeroMilestoneDefinition.IsCustomerView;
                    Update(cargoMyTenantMilestoneDefinition);
                }
            });
        }
    }
}
