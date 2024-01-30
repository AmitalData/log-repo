 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class AccountingCompanyTypeRepository:IRepository<AccountingCompanyType>
   {
        
		public List<AccountingCompanyType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public AccountingCompanyType GetSingleAccountingCompanyType(string Code, int tenant)
        {
            return (from a in context.AccountingCompanyTypes
                    where a.Code == Code && a.Tenant == tenant

                    select a).FirstOrDefault();
        }
        public AccountingCompanyType GetSingleAccountingCompanyTypeByCode(string Code, int tenant, bool getFromCache)
        {
            string entityName = "AccountingCompanyType" + Code + tenant;
            AccountingCompanyType entity;
            if (getFromCache)
            {
                entity = CacheManager.GetOrInsertNewObject(entityName, () =>
                {
                    return (from a in context.AccountingCompanyTypes
                            where a.Code == Code && a.Tenant == tenant

                            select a).FirstOrDefault();
                });
            }
            else
            {
                entity = (from a in context.AccountingCompanyTypes
                          where a.Code == Code && a.Tenant == tenant

                          select a).FirstOrDefault();

            }
            return entity;
        }

    }

}
   