using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class CustomsAirlineQueryService
    {

       public CustomsAirlinePM GetSingleCustomsAirlineByCodeAndPrefix(string code, string prefix, int tenant)
       {
           CustomsAirline entity = repository.GetByAirlineAndPrefix(code, prefix, null);
          
           CustomsAirlinePM entityPM = null;

           if (entity != null)
           {
                entityPM = new CustomsAirlinePM()
                {
                    Id = entity.Id,
                    Tenant = entity.Tenant,
                    AirlineCode = entity.AirlineCode,
                    AirlinePrefix = entity.AirlinePrefix,
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
