
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
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class InternationalSiteDataMapping: IMapping<InternationalSitePM, InternationalSite>
   {

        public void CustomPMToPOCO(InternationalSitePM entityPM, InternationalSite entityPOCO)
        {
            //throw new NotImplementedException();
            //throw new NotImplementedException();
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Code = entityPM.Code;
                
            }
        
        }

        public void CustomPOCOToPM(InternationalSitePM entityPM, InternationalSite entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CountryTypeName);

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (authToken != null)
                {
                    int tenant = authToken.Tenant;
                    if (entityPOCO.CountryTypeCode != null)
                    {
                        CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(tenant);
                        CustomsCountryPM customsCountryPM = customsCountryQueryService.GetSingle(entityPOCO.CountryTypeCode, false, true);
                        if (customsCountryPM != null)
                        {
                            entityPM.CountryTypeName = customsCountryPM.LocalName;
                        }
                    }
                }
            }
        }
   }


}
   