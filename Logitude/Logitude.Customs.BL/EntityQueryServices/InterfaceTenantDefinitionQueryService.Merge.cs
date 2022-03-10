using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class InterfaceTenantDefinitionQueryService
    {

        public int? GetTenantPriorityFromCacheByTenatCode(int tenant, string code)
        {

            string entityKeyString = $"GetTenantPriorityFromCacheByTenatCode ({tenant},{code})";

            var dummyInterfaceTenantDefinition = CacheManager.GetOrInsertNewObject<InterfaceTenantDefinitionPM>(entityKeyString,
                () =>
                {
                    
                    var poco = this.repository.GetSingleDefinitionByCode(code, tenant);
                    if (poco?.TenantPriority != null/* && poco?.Active == true*/)
                    {

                        return new InterfaceTenantDefinitionPM() { TenantPriority = poco?.TenantPriority };
                    }
                    var interfaceManagement = this.context.InterfaceManagements.FirstOrDefault(r => r.Code == code);
                    return new InterfaceTenantDefinitionPM() { TenantPriority = interfaceManagement?.DefaultPriority };
                });
            return dummyInterfaceTenantDefinition?.TenantPriority;

        }

        public InterfaceTenantDefinitionPM GetFromCacheByTenatCode(int tenant, string code)
        {

            string entityKeyString = $"InterfaceTenantDefinitionByTenatCode ({tenant},{code})";

            var dummyInterfaceTenantDefinition = CacheManager.GetOrInsertNewObject<InterfaceTenantDefinitionPM>(entityKeyString,
                () =>
                {

                    var poco = this.repository.GetSingleDefinitionByCode(code, tenant);
                    if(poco== null)
                    {
                        return new InterfaceTenantDefinitionPM();
                    }
                    var pm=this.GetEntityPM(poco);
                    if (pm.TenantPriority != null)
                    {

                        var interfaceManagement = this.context.InterfaceManagements.FirstOrDefault(r => r.Code == code);
                        pm.TenantPriority= interfaceManagement?.DefaultPriority;
                    }
                    return pm;
                });
            return dummyInterfaceTenantDefinition;

        }


        //public InterfaceTenantDefinitionPM GetFromCacheByTenatCode(int tenant, string code)
        //{

        //    string entityKeyString = $"GetInterfaceTenantDefinitionByTenatCode ({tenant},{code})";

        //    var pm1 = CacheManager.GetOrInsertNewObject<InterfaceTenantDefinitionPM>(entityKeyString,
        //        () => 
        //        {
        //            var poco = this.repository.GetSingleDefinitionByCode(code, tenant);
        //            if (poco == null) return null;
        //            var pm = this.GetEntityPM(poco);
        //            return pm;
        //        });
        //    return pm1;

        //}
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
