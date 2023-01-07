using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalTextCodeQueryService
    {
        public DigitalTextCodeList GetDigitalTextCodesQuery(int tenant, string objectTableId, string profileId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);
            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodes(tenant, objectTableId, profileId)
                                                            .Select(x => new DigitalTextCodeList
                                                            {
                                                                Id = x.Id,
                                                                ObjectTableId = x.ObjectTableId,
                                                                Tenant = x.Tenant,
                                                                Labels = x.Labels,
                                                                ProfileId = x.ProfileId,
                                                                CreateDate = x.CreateDate,
                                                                UpdateDate = x.UpdateDate
                                                            })
                                                            .FirstOrDefault();
            return defaultTextCode;
        }

        public List<DigitalTextCodeList> GetDigitalTextCodesObjetTables(int tenant)
        {
            var objetTables = context.DigitalTextCodes
                                     .Include("ObjectTable")
                                     .Where(a => a.Tenant == tenant)
                                     .Select(a => new DigitalTextCodeList
                                     {
                                         ObjectTableId = a.ObjectTableId,
                                         ObjectTableName = a.ObjectTable.Name,
                                     })
                                     .ToList();
            return objetTables;
        }
                
        public void UpdateDigitalTextCodes(DigitalTextCodeList digitalTextCodeList)
        {
            if (string.IsNullOrEmpty(digitalTextCodeList.Id))
            {
                var entityPm = new DigitalTextCodePM
                {
                    ObjectTableId = digitalTextCodeList.ObjectTableId,
                    Tenant = digitalTextCodeList.Tenant,
                    Labels = digitalTextCodeList.Labels,
                    ProfileId = digitalTextCodeList.ProfileId,
                    CreateDate = digitalTextCodeList.CreateDate,
                    UpdateDate = digitalTextCodeList.UpdateDate
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalTextCodeUpdateService service = new DigitalTextCodeUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalTextCodePM
                {
                    Id = digitalTextCodeList.Id,
                    ObjectTableId = digitalTextCodeList.ObjectTableId,
                    Tenant = digitalTextCodeList.Tenant,
                    Labels = digitalTextCodeList.Labels,
                    ProfileId = digitalTextCodeList.ProfileId,
                    CreateDate = digitalTextCodeList.CreateDate,
                    UpdateDate = digitalTextCodeList.UpdateDate
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalTextCodeUpdateService service = new DigitalTextCodeUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}