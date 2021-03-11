
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CurrencyTypeDataMapping: IMapping<CurrencyTypePM, CurrencyType>
   {

        public void CustomPMToPOCO(CurrencyTypePM entityPM, CurrencyType entityPOCO)
        {
     
        }

        public void CustomPOCOToPM(CurrencyTypePM entityPM, CurrencyType entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.Code);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EnglishName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.Inactive);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LocalName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SearchFields);


            //CardRepository rep = new CardRepository(entityPM.Tenant);
            int Tenant = 0;
            if (HttpContext.Current != null)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    Tenant = authToken.Tenant;

                }
                catch (Exception)
                {


                }
            }
 
            

            entityPM.MehesInactive= entityPOCO.Inactive;
            var repo = new CurrencyTypeTenantRepository(Tenant);
            var pocoTenant=repo.GetPMByCode(Tenant, entityPOCO.Code);
            if (pocoTenant != null)
            {
                entityPM.TenantInactive = pocoTenant.TenantInactive;
            }
        }
   }


}
   