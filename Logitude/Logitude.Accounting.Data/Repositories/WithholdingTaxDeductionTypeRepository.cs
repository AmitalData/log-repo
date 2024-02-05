
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
    public partial class WithholdingTaxDeductionTypeRepository : IRepository<WithholdingTaxDeductionType>
    {

        public List<WithholdingTaxDeductionType> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }
        public WithholdingTaxDeductionType GetSingleWithholdingTaxDeductionType(string Code, int tenant, bool getFromCache = false)
        {
            string entityName = "WithholdingTaxDeductionType" + Code + tenant;
            WithholdingTaxDeductionType entity;
            if (getFromCache)
            {
                entity = CacheManager.GetOrInsertNewObject(entityName, () =>
                {
                    return (from a in context.WithholdingTaxDeductionTypes
                            where a.Code == Code && a.Tenant == tenant

                            select a).FirstOrDefault();
                });
            }
            else
            {
                entity = (from a in context.WithholdingTaxDeductionTypes
                          where a.Code == Code && a.Tenant == tenant

                          select a).FirstOrDefault();
            }
            return entity;
        }
        public WithholdingTaxDeductionType GetSingleWithholdingTaxDeductionTypeByCode(string Code, int tenant)
        {
            return (from a in context.WithholdingTaxDeductionTypes
                    where a.Code == Code && a.Tenant == tenant

                          select a).FirstOrDefault();
            }
            return entity;
        }

        public List<WithholdingTaxDeductionType> GetAll()
        {
            return (from a in context.WithholdingTaxDeductionTypes

                    select a).ToList();
        }

    }

}
