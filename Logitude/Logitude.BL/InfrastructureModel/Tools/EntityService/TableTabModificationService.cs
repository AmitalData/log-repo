using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TableTabModificationService
    {
        private int tenant;
        private IWebFreightContext webFreightContext;
        private TabModificationRepository tabModificationRepository;
        private TabModification tabModification;
        private int tenantZero = 0;

        public TableTabModificationService(int tenant, IWebFreightContext webFreightContext)
        {
            this.tenant = tenant;
            this.webFreightContext = webFreightContext;
            this.tabModificationRepository = new TabModificationRepository(webFreightContext);


        }

        public void Update(ObjectTableTabPM objectTableTab)
        {
            if (objectTableTab.Tenant != tenantZero) return;
            objectTableTab.HasTabModification = true;

            tabModification = tabModificationRepository.GetByTabCode(objectTableTab.Code, tenant);
            if (tabModification == null)
            {
                 CreateTabModification(objectTableTab);
                return;
            }
            tabModification.Order = objectTableTab.IndexOrder;
            tabModification.Name = objectTableTab.Name;
            tabModificationRepository.Update(tabModification);
        }

        private void CreateTabModification(ObjectTableTabPM objectTableTab)
        {
            TabModification tabModification = new TabModification()
            {
                Id = IdCounter.GetNumber("TabModification", tenant),
                Tenant = tenant,
                TabCode = objectTableTab.Code,
                Order = objectTableTab.IndexOrder,
                Name = objectTableTab.Name,
            };
           tabModificationRepository.Add(tabModification);
        }

    }
}