using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class InterfaceTenantDefinitionQueryService
    {
        public List<InterfaceTenantDefinitionManagementPM> GetWithInterfaceManagementDefinition(int tenant, string code = null)
        {
            var interfaceManagementRepo = new InterfaceManagementRepository(context);
            var interfaceManagementQuery = interfaceManagementRepo.GetAll();
            var interfaceTenantDefinitions = context.InterfaceTenantDefinitions.AsQueryable();
            interfaceTenantDefinitions = interfaceTenantDefinitions.Where(rec => rec.Tenant == tenant);
            if (!string.IsNullOrWhiteSpace(code))
            {
                interfaceManagementQuery = interfaceManagementQuery.Where(rec => rec.Code == code);
                interfaceTenantDefinitions = interfaceTenantDefinitions.Where(rec => rec.Code == code);
            }
            var myJoin = (from interfaceManagementDef in interfaceManagementQuery
                          join tenantDef in interfaceTenantDefinitions
                          on interfaceManagementDef.Code equals tenantDef.Code into xy
                          from defaultTenantDef in xy.DefaultIfEmpty()
                          select new
                          {
                              InterfaceManagementDef = interfaceManagementDef,
                              TenantDef = defaultTenantDef
                          }).ToList();

            var interfaceManagementQueryService = new InterfaceManagementQueryService(this.context);

            var list = new List<InterfaceTenantDefinitionManagementPM>();
            foreach (var item in myJoin)
            {
                if (item.InterfaceManagementDef.Code == "2470")
                {

                }
                var interfaceTenantDef = this.GetEntityPM(item.TenantDef);
                var interfaceManagement = interfaceManagementQueryService.GetEntityPM(item.InterfaceManagementDef);
                var curr = new InterfaceTenantDefinitionManagementPM(tenant, interfaceTenantDef, interfaceManagement);
                list.Add(curr);
            }

            return list;
        }
    }
}
