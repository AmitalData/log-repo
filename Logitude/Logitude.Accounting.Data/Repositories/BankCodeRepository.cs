 
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

namespace Logitude.Accounting.Data.Repositories
{
   public partial class BankCodeRepository:IRepository<BankCode>
   {
        
		public List<BankCode> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public BankCode GetSingleByCode(string code, int tenant)
        {
            return (from a in context.BankCodes
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public BankCode GetSingleByTenant(int tenant)
        {
            return (from a in context.BankCodes
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

   }

}
   