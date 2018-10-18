 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomBanksCardRepository:IRepository<CustomBanksCard>
   {
        
		public List<CustomBanksCard> GetMulti(EntityKeyFields entityKeys)
        {

            CustomBankKeys customBankKeys = entityKeys as CustomBankKeys;

            return (from a in context.CustomBanksCards
                    where a.CustomBankId == customBankKeys.Id
                    select a).ToList();
        }


        public CustomBanksCard GetSingleCustomBanksCard(string bankId, string cardId, int tenant)
        {
            return (from a in context.CustomBanksCards
                    where a.CustomBankId == bankId && a.CardId == cardId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


      

   }

}
   