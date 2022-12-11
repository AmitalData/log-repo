using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalTextCodeQuery 
    {
        public List<DigitalTextCodeList> GetDigitalTextCodesQuery(int tenant, string objectTableId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);

            return digitalTextCodeRepository.GetDigitalTextCodes(tenant, objectTableId)
                                            .Select(x => new DigitalTextCodeList
                                            {
                                                Id = x.Id,
                                                ObjectTableId = x.ObjectTableId,
                                                Tenant = x.Tenant,
                                                Labels = x.Labels,
                                                CreateDate = x.CreateDate,
                                                UpdateDate = x.UpdateDate
                                            }).ToList();
        }
    }
}
