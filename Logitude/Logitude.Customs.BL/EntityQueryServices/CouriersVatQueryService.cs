using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class CouriersVatQueryService
    {

       public CouriersVatPM GetSingleCouriersVatByCode(string code, int tenant)
       {
           CouriersVat entity = repository.GetByVatNumber(code, null);
          
           CouriersVatPM entityPM = null;

           if (entity != null)
           {
                entityPM = new CouriersVatPM()
                {
                    Id = entity.Id,
                    Tenant = entity.Tenant,
                    VatNumber = entity.VatNumber,
                    LocalName = entity.LocalName,
                    EnglishName = entity.EnglishName,
                    SearchFields = entity.SearchFields,
                    InActive = entity.InActive,
                };

           }
           return entityPM;
       }
    }
}
