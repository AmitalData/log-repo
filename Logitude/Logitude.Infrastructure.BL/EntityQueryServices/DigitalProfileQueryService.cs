using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
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
                                                                 CreateDate = x.CreateDate,
                                                                 UpdateDate = x.UpdateDate
                                                             })
                                                             .ToList();
            return digitalProfileList;
        }
    }
}