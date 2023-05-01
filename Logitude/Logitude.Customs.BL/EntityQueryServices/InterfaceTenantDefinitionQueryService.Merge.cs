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

        public InterfaceTenantDefinitionPM GetInterfaceDefWithPriorityFromCacheByTenatCode(int tenant, string code)
        {

            string entityKeyString = $"InterfaceTenantDefinitionByTenatCode ({tenant},{code})";

            var dummyInterfaceTenantDefinition = CacheManager.GetOrInsertNewObject<InterfaceTenantDefinitionPM>(entityKeyString,
                () =>
                {

                    var poco = this.repository.GetSingleDefinitionByCode(code, tenant);
                    if(poco== null)
                    {
                        var NewInterfaceTenantDefinitionPM = new InterfaceTenantDefinitionPM();
                        var interfaceManagement = this.context.InterfaceManagements.FirstOrDefault(r => r.Code == code);
                        NewInterfaceTenantDefinitionPM.TenantPriority = interfaceManagement?.DefaultPriority;
                        return NewInterfaceTenantDefinitionPM;
                    }
                    var pm=this.GetEntityPM(poco);
                    if (pm.TenantPriority == null)
                    {

                        var interfaceManagement = this.context.InterfaceManagements.FirstOrDefault(r => r.Code == code);
                        pm.TenantPriority= interfaceManagement?.DefaultPriority;
                    }
                    return pm;
                });
            return dummyInterfaceTenantDefinition;

        }

        public InterfaceTenantDefinitionPM GetByTenatCode(int tenant, string code)
        {

            

                    var poco = this.repository.GetSingleDefinitionByCode(code, tenant);
                    if (poco == null)
                    {
                        return new InterfaceTenantDefinitionPM();
                    }
                    var pm = this.GetEntityPM(poco);
                    if (pm.TenantPriority != null)
                    {

                        var interfaceManagement = this.context.InterfaceManagements.FirstOrDefault(r => r.Code == code);
                        pm.TenantPriority = interfaceManagement?.DefaultPriority;
                    }
                    return pm;
                

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
            string entityKeyString = $"GetWithInterfaceManagementDefinition ({tenant},{code})";

            var dummyInterfaceManagementDefinition = CacheManager.GetOrInsertNewObject<List<InterfaceTenantDefinitionManagementPM>>(entityKeyString,
                () =>
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
                }, absoluteExpiration: 1440);
            return dummyInterfaceManagementDefinition;
        }


        public  List<InterfaceTenantDefinitionManagementPM> GetInterfaceListDCA(List<InterfaceTenantDefinitionManagementPM> interfaceTenantDefinitionManagementPMs, string CompanyType)
        {
            var interfaceListDCA = //(new IIGMessageQueryService()).GetAll().Where(mess => mess.Interactive.HasFlag(InterfaceType.InteractiveMode.DCA)); ;
                 interfaceTenantDefinitionManagementPMs //_AllInterface
                 .Where(r => r.OverrideActive == true)

                  //להתייחס לשדה Active מרמת ניהול מסרים
                  .Where(r => r.InterfaceManagement.Active == true)



                  .Where(
                     r =>
                  //INTERFACETYPE
                  //ערכים NULL== הכל, C == רק עמילות, B == רק בלדרות
                  string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)//All

                  ||
                  (
                  !string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)
                  &&
                   //COMPANYTYPE שם שדה ערכים C -דיפולטיבי(בסקריפט), או B == בלדרות - אסור ריק יאותחל עם הפצה ראשונה + DEFAULT == C
                   r.InterfaceManagement.InterfaceType == /*customsSettingPM.*/CompanyType
                   )
                   )

                 .Where(rec =>
                     //rec.InterfaceManagement.INOUT ==  Logitude.Customs.BL.ClosedTable.InOutType.In  &&
                     //!string.IsNullOrWhiteSpace(rec.InterfaceManagement.DcaPrefixName) && 
                     //!rec.OverrideInActive &&
                     ///////rec.Interactive == Logitude.Customs.BL.ClosedTable.InteractiveMode.DCABatchIn &&
                     !String.IsNullOrWhiteSpace(
                     rec.InterfaceManagement.DcaPrefixName +
                     rec.InterfaceManagement.DcaPrefixName2 +
                     rec.InterfaceManagement.DcaPrefixName3 +
                     rec.InterfaceManagement.DcaPrefixName4)
                     ).ToList();
            return interfaceListDCA;
        }
    }
}
