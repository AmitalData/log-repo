using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalTextCodeQuery
    {
        public DigitalTextCodeList GetDigitalTextCodesQuery(int tenant, string objectTableId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);

            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodes(0, objectTableId)
                                                            .Select(x => new DigitalTextCodeList
                                                            {
                                                                Id = x.Id,
                                                                ObjectTableId = x.ObjectTableId,
                                                                Tenant = x.Tenant,
                                                                Labels = x.Labels,
                                                                CreateDate = x.CreateDate,
                                                                UpdateDate = x.UpdateDate
                                                            }).FirstOrDefault();
            return defaultTextCode;
        }

        public bool CheckTenantTranslation(int tenant, string objectTableId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);
            return digitalTextCodeRepository.CheckTenantTranslation(tenant, objectTableId);
        }

    }

    public class DigitalTextCodeObject
    {
        public string DisplayText { get; set; }

        public string Code { get; set; }
    }
}