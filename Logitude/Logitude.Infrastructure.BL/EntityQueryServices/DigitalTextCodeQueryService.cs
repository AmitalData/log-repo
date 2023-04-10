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
        public DigitalTextCodeList GetDigitalTextCodesQuery(int tenant, string objectTableId, string profileCode, string langCode)
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);
            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodes(tenant, objectTableId, profileCode, langCode)
                                                            .Select(x => new DigitalTextCodeList
                                                            {
                                                                Id = x.Id,
                                                                ObjectTableId = x.ObjectTableId,
                                                                Tenant = x.Tenant,
                                                                Labels = x.Labels,
                                                                ProfileId = x.ProfileId,
                                                                LanguageCode = x.LanguageCode,
                                                                CreateDate = x.CreateDate,
                                                                UpdateDate = x.UpdateDate,
                                                            })
                                                            .FirstOrDefault();
            return defaultTextCode;
        }

        public List<DigitalTextCodeList> GetDigitalTextCodesTenant0()
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(0);
            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodesTenant0()
                                                           .Select(x => new DigitalTextCodeList
                                                           {
                                                               Id = x.Id,
                                                               ObjectTableId = x.ObjectTableId,
                                                               ObjectTableName = x.ObjectTable.Name,
                                                               Tenant = x.Tenant,
                                                               Labels = x.Labels,
                                                               ProfileId = x.ProfileId,
                                                               CreateDate = x.CreateDate,
                                                               UpdateDate = x.UpdateDate,
                                                               ProfileCode = x.DigitalProfile.Code,
                                                               LanguageCode = x.LanguageCode
                                                           })
                                                           .ToList();

            return defaultTextCode;
        }
        
        public List<DigitalTextCodeList> GetDigitalTextCodesObjetTables(int tenant)
        { 
            var objetTables = context.DigitalTextCodes
                                     .Include("ObjectTable")
                                     .Where(a => a.Tenant == tenant)
                                     .GroupBy(a => a.ObjectTable)
                                     .Select(a => new DigitalTextCodeList
                                     {
                                         ObjectTableId = a.Key.Id,
                                         ObjectTableName = a.Key.Name,
                                     }).ToList();

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
                    UpdateDate = digitalTextCodeList.UpdateDate,
                    LanguageCode = digitalTextCodeList.LanguageCode
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
                    UpdateDate = digitalTextCodeList.UpdateDate,
                    LanguageCode = digitalTextCodeList.LanguageCode
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalTextCodeUpdateService service = new DigitalTextCodeUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}