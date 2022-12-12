using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalFieldSecurityQueryService
    {
        public DigitalFieldSecurityList GetDigitalFieldSecurityQuery(int tenant, string objectTableId, string profileId)
        {
            DigitalFieldSecurityRepository digitalFieldSecurityRepository = new DigitalFieldSecurityRepository(tenant);
            var digitalFieldSecurity = digitalFieldSecurityRepository.GetDigitalFieldSecurity(tenant, objectTableId, profileId)
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
    }
}