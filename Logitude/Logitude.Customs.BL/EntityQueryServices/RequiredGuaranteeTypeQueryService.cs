using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class RequiredGuaranteeTypeQueryService
    {

       //public List<RequiredGuaranteeTypeList> GetRequiredGuaranteeTypesForGuarantee(string guaranteeId, int tenant)
       //{
       //    List<RequiredGuaranteeType> types = repository.GetGuaranteeTypesForGuarantee(guaranteeId, tenant);
       //    List<RequiredGuaranteeTypeList> requiredTypes = new List<RequiredGuaranteeTypeList>();

       //    foreach (RequiredGuaranteeType a in types)
       //    {
       //        RequiredGuaranteeTypeList typeList = new RequiredGuaranteeTypeList()
       //        {
       //            Id = a.Id,
       //            Tenant = a.Tenant,
       //            GuaranteeAmount = a.GuaranteeAmount,
       //            GuaranteeId = a.GuaranteeId,
       //            GuaranteeTypeCode = a.GuaranteeTypeCode,
       //            GuaranteeTypeName = a.GuaranteeCertificateType  != null? a.GuaranteeCertificateType.LocalName :null,
       //        };
       //        requiredTypes.Add(typeList);

       //    }

       //    return requiredTypes;
       //}
    }
}
