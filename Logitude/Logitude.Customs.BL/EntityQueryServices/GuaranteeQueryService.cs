using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class GuaranteeQueryService
    {

       public GuaranteePM GetGuaranteeByTapagId(string tapagId, int tenant)
       {
           Guarantee guarantee = repository.GetGuaranteeByTapagId(tapagId, tenant);
           GuaranteePM guaranteePM  = null;

           if (guarantee != null)
           {
            guaranteePM = new GuaranteePM()
            {
               Id = guarantee.Id,
               Tenant = guarantee.Tenant,
               ClientActivityCode = guarantee.ClientActivityCode,
               BirthDate = guarantee.BirthDate,
               BrandNumber = guarantee.BrandNumber,
               CustomEntityNumber= guarantee.CustomEntityNumber,
               CustomEntityTypeCode = guarantee.CustomEntityTypeCode,
               CustomEntityTypeName = guarantee.EntityTypeLookup != null? guarantee.EntityTypeLookup.LocalName : null,
               EngineNumber = guarantee.EngineNumber,
               GuaranteeExternalNumber =  guarantee.GuaranteeExternalNumber,
               GuaranteeRequestNumber = guarantee.GuaranteeRequestNumber,
               GuaranteeRequestStatusCode = guarantee.GuaranteeRequestStatusCode,
               GuaranteeValidityDate = guarantee.GuaranteeValidityDate,
               LawyerNumber = guarantee.LawyerNumber,
               MsgID = guarantee.MsgID,
               NumeralRequest = guarantee.NumeralRequest,
               RequestValidityDate = guarantee.RequestValidityDate,
               TapagID = guarantee.TapagID,
               UpdateDate = guarantee.UpdateDate,
               VehicleChassisNumber = guarantee.VehicleChassisNumber,
           };
            GuaranteeConditionQueryService conditionQueryService = new GuaranteeConditionQueryService(tenant);
            RequiredGuaranteeTypeQueryService typeQueryService = new RequiredGuaranteeTypeQueryService(tenant);
            if (guaranteePM != null)
            {

                guaranteePM.GuaranteeConditions = conditionQueryService.GetMulti(new GuaranteeKeys() { Id = guaranteePM.Id }, false);
                guaranteePM.RequiredGuaranteeTypes = typeQueryService.GetMulti(new GuaranteeKeys() { Id = guaranteePM.Id }, false);
            }
           }

           return guaranteePM;
       }

       public override void GetComposition(EntityKeyFields entityKeys, GuaranteePM entityPM)
       {
           ICustomContext context = MainContext as CustomContext;
           GuaranteeKeys guaranteeKeys = entityKeys as GuaranteeKeys;
           GuaranteeConditionQueryService guaranteeConditionQueryService = new GuaranteeConditionQueryService(context);
           RequiredGuaranteeTypeQueryService typeQueryService = new RequiredGuaranteeTypeQueryService(context);

           entityPM.GuaranteeConditions = guaranteeConditionQueryService.GetMulti(guaranteeKeys, true);
           entityPM.RequiredGuaranteeTypes = typeQueryService.GetMulti(guaranteeKeys, true);
       }
    }
}


