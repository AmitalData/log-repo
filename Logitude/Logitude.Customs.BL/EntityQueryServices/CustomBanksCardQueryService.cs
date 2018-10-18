using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class CustomBanksCardQueryService
    {

       public CustomBanksCardPM GetSingleCustomBanksCard(string bankId, string cardId, int tenant)
       {
           CustomBanksCard bankCard = repository.GetSingleCustomBanksCard(bankId, cardId, tenant);
          
           CustomBanksCardPM bankCardPM = null;
           if (bankCard != null)
           {
               bankCardPM = new CustomBanksCardPM()
           {
               Id = bankCard.Id,
               Tenant = bankCard.Tenant,
               CardId = bankCard.CardId,
               CustomBankId = bankCard.CustomBankId,

           };

           }
           return bankCardPM;
       }
    }
}
