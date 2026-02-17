 
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
   public partial class WithholdingTaxDeductionTypeRepository:IRepository<WithholdingTaxDeductionType>
   {
        
		public List<WithholdingTaxDeductionType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public WithholdingTaxDeductionType GetSingleWithholdingTaxDeductionType(string Code, int tenant)
        {
            return (from a in context.WithholdingTaxDeductionTypes
                    where a.Code == Code && a.Tenant == tenant

                    select a).FirstOrDefault();
        }




    }

}
   