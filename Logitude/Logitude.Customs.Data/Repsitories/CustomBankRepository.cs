 
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
   public partial class CustomBankRepository:IRepository<CustomBank>
   {
        
		public List<CustomBank> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CustomBank> GetCustomBanksForCard(string cardId, int tenant)
        {
            List<CustomBank> selectedbanks = (from banks in context.CustomBanks
                                              join cards in context.CustomBanksCards on banks.Id equals cards.CustomBankId
                                              where cards.CardId == cardId && !banks.InActive && cards.Tenant == tenant
                                              select banks).ToList();
            return selectedbanks;
        }

        public CustomBank GetCustomBanksByCode(string code, int tenant)
        {
            return (from a in context.CustomBanks
                    where a.BankCode == code && a.Tenant == tenant
                    select a).FirstOrDefault();
           
        }


        public CustomBank GetCustomBanksByInternalCode(string code, int tenant)
        {
            return (from a in context.CustomBanks
                    where a.InternalCode == code && a.Tenant == tenant
                    select a).FirstOrDefault();

        }
   
   }

}
   