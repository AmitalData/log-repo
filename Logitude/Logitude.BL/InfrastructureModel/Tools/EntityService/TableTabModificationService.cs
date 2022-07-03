using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TableTabModificationService
    {
        int tenant;
        IWebFreightContext context;
        public TableTabModificationService(int tenant, IWebFreightContext context)
        {
            this.tenant = tenant;
            this.context = context;
        }
        public void UpdateModification(ObjectTableTabPM tabPM)
        {
            TabModification mod = GetModification(tabPM);

            if (mod == null)
            {
                var newMod = CreateTabModification(tabPM);
                SubmitNewModification(newMod);
            }
            else
            {
                MapModification(tabPM, mod);
                SubmitModification(mod);
            }
        }

        private TabModification MapModification(ObjectTableTabPM tabPM, TabModification mod)
        {
            mod.Name = tabPM.Name;
            mod.Order = tabPM.IndexOrder;
            mod.TabId = tabPM.Id;
            return mod;
        }

        private TabModification GetModification(ObjectTableTabPM tabPM)
        {
            TabModificationRepository modsRepository = new TabModificationRepository(context);
            var mod = modsRepository.GetByTabCode(tabPM.Code, tenant);
            return mod;
        }

        private TabModification CreateTabModification(ObjectTableTabPM tabPM)
        {
            var mod = new TabModification()
            {
                Id = IdCounter.GetNumber("TabModification", tabPM.Tenant),
                Tenant = tenant,
                TabCode = tabPM.Code,
            };

            return MapModification(tabPM, mod);
        }

        private void SubmitModification(TabModification newMod)
        {
            var repository = new TabModificationRepository(context);
            repository.Update(newMod);
            repository.SubmitChanges();
        }

        private void SubmitNewModification(TabModification newMod)
        {
            var repository = new TabModificationRepository(context);
            repository.Add(newMod);
            repository.SubmitChanges();
        }
        
    }
}