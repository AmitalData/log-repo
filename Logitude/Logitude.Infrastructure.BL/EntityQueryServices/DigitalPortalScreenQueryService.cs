using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalPortalScreenQueryService
    {
        public List<DigitalPortalScreenList> GetDigitalPortalScreenQuery(int tenant, string objectTableId, string screenCode = "")
        {
            var digitalPortalScreenRepository = new DigitalPortalScreenRepository(tenant);

            var digitalPortalScreens = digitalPortalScreenRepository.GetDigitalPortalScreens(tenant, objectTableId, screenCode)
                                                                    .Select(x => new DigitalPortalScreenList
                                                                    {
                                                                        Id = x.Id,
                                                                        Tenant = x.Tenant,
                                                                        Name = x.Name,
                                                                        CreateDate = x.CreateDate,
                                                                        UpdateDate = x.UpdateDate,
                                                                        Content = x.Content,
                                                                        DraftContent = x.DraftContent,
                                                                        ScreenCode = x.ScreenCode,
                                                                        ObjectTableId = x.ObjectTableId
                                                                    })
                                                                    .ToList();
            return digitalPortalScreens;
        }
    }
}
