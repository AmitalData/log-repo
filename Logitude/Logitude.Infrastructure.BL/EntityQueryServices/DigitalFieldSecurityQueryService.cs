using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalFieldSecurityQueryService
    {
        public DigitalFieldSecurityList GetDigitalFieldSecurityQuery(int tenant, string objectTableId, string profileCode)
        {
            DigitalFieldSecurityRepository digitalFieldSecurityRepository = new DigitalFieldSecurityRepository(tenant);
            var digitalFieldSecurity = digitalFieldSecurityRepository.GetDigitalFieldSecurity(tenant, objectTableId, profileCode)
                                                                     .Select(x => new DigitalFieldSecurityList
                                                                     {
                                                                         Id = x.Id,
                                                                         ObjectTableId = x.ObjectTableId,
                                                                         Tenant = x.Tenant,
                                                                         DefaultSettings = x.DefaultSettings,
                                                                         CreateDate = x.CreateDate,
                                                                         UpdateDate = x.UpdateDate,
                                                                         ProfileId = x.ProfileId
                                                                     })
                                                                     .FirstOrDefault();
            return digitalFieldSecurity;
        }
        
        public List<DigitalFieldSecurityList> GetDigitalFieldSecurityQueryTenant0()
        {
            DigitalFieldSecurityRepository digitalFieldSecurityRepository = new DigitalFieldSecurityRepository(0);
            var digitalFieldSecurity = digitalFieldSecurityRepository.GetDigitalFieldSecurityTenant0()
                                                                     .Select(x => new DigitalFieldSecurityList
                                                                     {
                                                                         Id = x.Id,
                                                                         ObjectTableId = x.ObjectTableId,
                                                                         Tenant = x.Tenant,
                                                                         DefaultSettings = x.DefaultSettings,
                                                                         CreateDate = x.CreateDate,
                                                                         UpdateDate = x.UpdateDate,
                                                                         ProfileId = x.ProfileId
                                                                     })
                                                                     .ToList();
            return digitalFieldSecurity;
        }
        
        public void UpdateDigitalFieldSecurity(DigitalFieldSecurityList digitalFieldSecurityList)
        {
            if (string.IsNullOrEmpty(digitalFieldSecurityList.Id))
            {
                var entityPm = new DigitalFieldSecurityPM
                {
                    ObjectTableId = digitalFieldSecurityList.ObjectTableId,
                    Tenant = digitalFieldSecurityList.Tenant,
                    DefaultSettings = digitalFieldSecurityList.DefaultSettings,
                    CreateDate = digitalFieldSecurityList.CreateDate,
                    UpdateDate = digitalFieldSecurityList.UpdateDate,
                    ProfileId = digitalFieldSecurityList.ProfileId
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                var service = new DigitalFieldSecurityUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalFieldSecurityPM
                {
                    Id = digitalFieldSecurityList.Id,
                    ObjectTableId = digitalFieldSecurityList.ObjectTableId,
                    Tenant = digitalFieldSecurityList.Tenant,
                    DefaultSettings = digitalFieldSecurityList.DefaultSettings,
                    CreateDate = digitalFieldSecurityList.CreateDate,
                    UpdateDate = digitalFieldSecurityList.UpdateDate,
                    ProfileId = digitalFieldSecurityList.ProfileId
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                var service = new DigitalFieldSecurityUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }

        public List<DigitalFieldSecurityList> GetDigitalProfilesObjetTables(int tenant)
        {
            var objetTables = context.DigitalFieldSecurities
                                     .Include("ObjectTable")
                                     .Where(a => a.Tenant == tenant 
                                                 && !a.ObjectTable.Name.Equals("General",StringComparison.InvariantCultureIgnoreCase))
                                     .GroupBy(a => a.ObjectTable)
                                     .Select(a => new DigitalFieldSecurityList
                                     {
                                         ObjectTableId = a.Key.Id,
                                         ObjectTableName = a.Key.Name,
                                     }).ToList();
                                    
            return objetTables;
        }

    }
}