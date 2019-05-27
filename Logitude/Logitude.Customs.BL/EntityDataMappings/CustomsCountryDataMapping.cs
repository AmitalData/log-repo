
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsCountryDataMapping: IMapping<CustomsCountryPM, CustomsCountry>
   {

        public void CustomPMToPOCO(CustomsCountryPM entityPM, CustomsCountry entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(CustomsCountryPM entityPM, CustomsCountry poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code + "," + entityPM.LocalName;

            entityPM.SearchFields = result.ToLower(); ;
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomsCountryPM entityPM, CustomsCountry entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.TarriffName);

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken != null)
                {
                    int tenant = authToken.Tenant;

                    if (entityPOCO.TarriffCode != null)
                    {
                        TradeAgreementQueryService tradeAgreementQueryService = new TradeAgreementQueryService(tenant);
                        TradeAgreementPM tradeAgreementPM = tradeAgreementQueryService.GetSingle(entityPOCO.TarriffCode, false, true);
                        if (tradeAgreementPM != null)
                        {
                            entityPM.TarriffName = tradeAgreementPM.LocalName;
                        }
                    }
                }
            }
        }
   }


}
   