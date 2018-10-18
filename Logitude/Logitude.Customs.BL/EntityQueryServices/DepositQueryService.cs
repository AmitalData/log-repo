using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class DepositQueryService
    {
       public DepositPM GetDepositByPaymentOrderNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
       {
           DepositPM depositPM = null;
           if (!string.IsNullOrWhiteSpace(paymentNumber) || !string.IsNullOrEmpty(tapagId))
           {

               DepositRepository depositRepository = new DepositRepository(context);
               Deposit deposit = depositRepository.GetDepositByPaymentNumberOrTapagId(paymentNumber, tapagId, tenant);
               if (deposit != null)
               {
                   depositPM = new DepositPM()
                   {
                      Id = deposit.Id,
                      Tenant = deposit.Tenant,
                      EntityTypeCode = deposit.EntityTypeCode,
                      EntityNumber = deposit.EntityNumber,
                      EntityTypeName = deposit.EntityTypeLookup != null ? deposit.EntityTypeLookup.LocalName : null,
                      DepositEssenceTypeCode = deposit.DepositEssenceTypeCode,
                      DepositAmount = deposit.DepositAmount,
                      TradeMarkNumber= deposit.TradeMarkNumber,
                      LawyerNumber = deposit.LawyerNumber,
                      VehicleChassisNumber = deposit.VehicleChassisNumber,
                      EngineNumber = deposit.EngineNumber,
                      BirthDate = deposit.BirthDate,
                      PaymentNumber = deposit.PaymentNumber,
                      TapagID = deposit.TapagID,


                   };
               }


               DepositConditionQueryService conditionQueryService = new DepositConditionQueryService(tenant);
               if (depositPM != null)
               {

                   depositPM.DepositConditions = conditionQueryService.GetMulti(new DepositKeys() { Id = depositPM.Id }, false);
               }

               

           }
           return depositPM;
       }

      

       public override void GetComposition(EntityKeyFields entityKeys, DepositPM entityPM)
    {
           ICustomContext context = MainContext as CustomContext;
           DepositKeys depositKeys = entityKeys as DepositKeys;
           DepositConditionQueryService depositConditionQuery = new DepositConditionQueryService(context);
        
           //******getting all compositionTables for response service purposes only *****///
           entityPM.DepositConditions = depositConditionQuery.GetMulti(depositKeys, true);
          

         
       }

       public string GetDepositIdByTapagNumber(string tapagId, int tenant)
       {
           if (String.IsNullOrWhiteSpace(tapagId)) return "";
           return repository.GetDepositIdByTapagNumber(tapagId, tenant);
       }

        public object GetDepositIdByPaymentNumber(string paymentNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(paymentNumber)) return "";
            return repository.GetDepositIdByPaymentNumber(paymentNumber, tenant);
        }
    }
}
