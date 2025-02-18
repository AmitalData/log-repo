 
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DefaultValueRepository:IRepository<DefaultValue>
   {
        
		public List<DefaultValue> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        /* If you want to send in the fourth parameter a CODE from the card table,
        you will send to function GetDefaultValue, but if you want to send ID from the card table or from another table,
        you will send to function GetDefaultByCardId */
        public string GetDefaultValue_Cache(string Distr, string DefaultTypeCode, string BranchCode, string CardCode, int Tenant)
        {
            string entityKeyString = $"GetDefaultValue_Cache({Distr},{DefaultTypeCode},{BranchCode},{CardCode},{Tenant})";
            var res = CacheManager
                .GetOrInsertNewObject<string>(entityKeyString,
                () => { return this.GetDefaultValue(Distr, DefaultTypeCode, BranchCode, CardCode, Tenant); });
            return res;

        }

        public string GetDefaultByCardId_Cache(string Distr, string DefaultTypeCode, string BranchCode, string CardId, int Tenant)
        {
            string entityKeyString = $"GetDefaultByCardId_Cache({Distr},{DefaultTypeCode},{BranchCode},{CardId},{Tenant})";
            var res = CacheManager
                .GetOrInsertNewObject<string>(entityKeyString,
                () => { return this.GetDefaultByCardId(Distr, DefaultTypeCode, BranchCode, CardId, Tenant); });
            return res;

        }

        public string GetDefaultValue(string Distr, string DefaultTypeCode, string BranchCode, string CardCode, int Tenant)
        {
            var query =  
                (from a in context.DefaultValues.Include("Branch")
                    join b in context.DefaultTypes
                    on a.DefaultTypeId equals b.Id
                    join Card in context.Cards
                    on a.CardId equals Card.Id into qCards
                    from Card in qCards.DefaultIfEmpty()
                    where a.Distr == Distr && (a.Branch.Code == BranchCode || BranchCode == "NON") && (Card.Code == CardCode || CardCode == "NON")  && a.Tenant == Tenant && b.Code == DefaultTypeCode
                    select a.DefValue).FirstOrDefault();
            return query;
        }

        public string GetDefaultByCardId(string Distr, string DefaultTypeCode, string BranchCode, string CardId, int Tenant)
        {
            var query =
                (from a in context.DefaultValues.Include("Branch")
                 join b in context.DefaultTypes
                 on a.DefaultTypeId equals b.Id
                 where a.Distr == Distr && (a.Branch.Code == BranchCode || BranchCode == "NON") && a.CardId == CardId && a.Tenant == Tenant && b.Code == DefaultTypeCode
                 select a.DefValue).FirstOrDefault();
            return query;
        }
        public DefaultValue GetSingleByDefaultTypeId(string defTypeId, int tenant)
        {
            return (from a in context.DefaultValues
                    where a.DefaultTypeId == defTypeId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public string GetDefaultAccountNumberByDefaultValue(string Distr, string DefaultTypeCode, string BranchCode, string ShortValue, int Tenant)
        {
            string accountNo = "";

            var query =
                (from a in context.DefaultValues.Include("Branch")
                 join b in context.DefaultTypes
                 on a.DefaultTypeId equals b.Id
                 join c in context.Cards
                 on a.CardId equals c.Id
                 where a.Distr == Distr && (a.Branch.Code == BranchCode || BranchCode == "NON") && a.ShortValue == ShortValue
                 && a.Tenant == Tenant && b.Code == DefaultTypeCode
                 select c).ToList();
            if (query != null && query.Count > 0)
            {
                accountNo = query.FirstOrDefault().Code;
            }
            return accountNo;
        }
        public List<string> GetCardIdsByDefaultValue(string Distr, string DefaultTypeCode, List<string> groups, int Tenant)
        {
            List<string> cardIds = new List<string>();

            var query =
                (from a in context.DefaultValues
                 join b in context.DefaultTypes
                                 on a.DefaultTypeId equals b.Id
                 join c in context.Cards
                                 on a.CardId equals c.Id
                 where a.Distr == Distr
                       && (groups.Contains(a.ShortValue) || groups.Contains(a.DefValue))
                       && a.Tenant == Tenant
                       && b.Code == DefaultTypeCode
                 select c).ToList();
            if (query != null && query.Count > 0)
            {
                foreach (var item in query)
                {
                    cardIds.Add(item.Id);
                }
            }
            return cardIds;
        }


    }



}

   


   