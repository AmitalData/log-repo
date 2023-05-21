 
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

        public string GetDefaultValue_Cache(string Distr, string DefaultTypeCode, string BranchCode, string CardCode, int Tenant)
        {
            string entityKeyString = $"GetDefaultValue_Cache({Distr},{DefaultTypeCode},{BranchCode},{CardCode},{Tenant})";
            var res = CacheManager
                .GetOrInsertNewObject<string>(entityKeyString,
                () => { return this.GetDefaultValue(Distr, DefaultTypeCode, BranchCode, CardCode, Tenant); });
            return res;

        }

        public string GetDefaultValue(string Distr, string DefaultTypeCode, string BranchCode, string CardCode, int Tenant)
        {
            var query =  
                (from a in context.DefaultValues.Include("Branch").Include("Card")
                    join b in context.DefaultTypes
                    on a.DefaultTypeId equals b.Id
                    where a.Distr == Distr && (a.Branch.Code == BranchCode || BranchCode == "NON") && (a.Card.Code == CardCode || CardCode == "NON")  && a.Tenant == Tenant && b.Code == DefaultTypeCode
                    select a.Value).FirstOrDefault();
            return query;
        }
        public DefaultValue GetSingleByDefaultTypeId(string defTypeId, int tenant)
        {
            return (from a in context.DefaultValues
                    where a.DefaultTypeId == defTypeId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    }


        public string GetCardIdByDefaultValue(string Distr, string DefaultTypeCode, string BranchCode, string ShortValue, int Tenant)
        {
            string accountNo = "";
            
            var query =
                (from a in context.DefaultValues.Include("Branch")
                 join b in context.DefaultTypes
                 on a.DefaultTypeId equals b.Id
                 where a.Distr == Distr && (a.Branch.Code == BranchCode || BranchCode == "NON") && a.ShortValue == ShortValue 
                 && a.Tenant == Tenant && b.Code == DefaultTypeCode
                 select a).ToList();
            if (query != null && query.Count > 0)
            {
                accountNo = query.FirstOrDefault().Card.Code;
            }
            return accountNo;
        }
    }

   

}
   