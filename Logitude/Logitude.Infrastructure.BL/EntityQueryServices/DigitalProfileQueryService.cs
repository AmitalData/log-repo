using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalProfileQueryService
    {
        public List<DigitalProfileList> GetDigitalProfileQuery(int tenant, string profileName = "")
        {
            DigitalProfileRepository digitalProfileRepository = new DigitalProfileRepository(tenant);
            var digitalProfileList = digitalProfileRepository.GetDigitalProfiles(tenant, profileName)
                                                             .Select(x => new DigitalProfileList
                                                             {
                                                                 Id = x.Id,
                                                                 Tenant = x.Tenant,
                                                                 Name = x.Name,
                                                                 Code = x.Code,
                                                                 CreateDate = x.CreateDate,
                                                                 UpdateDate = x.UpdateDate
                                                             })
                                                             .ToList();
            return digitalProfileList;
        }

        public DigitalProfilePM GetDigitalProfileByName(int tenant, string profileName)
        {
            DigitalProfileRepository digitalProfileRepository = new DigitalProfileRepository(tenant);
            var digitalProfile = digitalProfileRepository.GetDigitalProfileByName(tenant, profileName);

            if (digitalProfile != null)
            {
                EntityPM = new DigitalProfilePM();
                mapping.CustomPOCOToPM(EntityPM, digitalProfile);
                mapping.POCOToPM(EntityPM, digitalProfile);
            };

            return EntityPM;
        }

        public void UpdateDigitalProfile(DigitalProfileList digitalProfileList)
        {
            if (string.IsNullOrEmpty(digitalProfileList.Id))
            {
                var entityPm = new DigitalProfilePM
                {
                    Tenant = digitalProfileList.Tenant,
                    Name = digitalProfileList.Name,
                    Code = digitalProfileList.Code,
                    CreateDate = digitalProfileList.CreateDate,
                    UpdateDate = digitalProfileList.UpdateDate
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalProfileUpdateService service = new DigitalProfileUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalProfilePM
                {
                    Id = digitalProfileList.Id,
                    Tenant = digitalProfileList.Tenant,
                    Name = digitalProfileList.Name,
                    Code = digitalProfileList.Code,
                    CreateDate = digitalProfileList.CreateDate,
                    UpdateDate = digitalProfileList.UpdateDate
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalProfileUpdateService service = new DigitalProfileUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}