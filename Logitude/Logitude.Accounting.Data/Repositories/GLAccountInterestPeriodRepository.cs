 
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
   public partial class GLAccountInterestPeriodRepository:IRepository<GLAccountInterestPeriod>
   {

        public List<GLAccountInterestPeriod> GetMulti(EntityKeyFields entityKeys)
        {
            GLAccountKeys myEntityKeys = entityKeys as GLAccountKeys;
            return (from a in context.GLAccountInterestPeriods where a.GLAccountId == myEntityKeys.Id select a).ToList();
        }

        public GLAccountInterestPeriod GetSingleByPeriodStartDateeAndnterestGLAccountId(DateTime PeriodStartDate, string GLAccountId, int Tenant)
        {

            GLAccountInterestPeriod Period = (from a in context.GLAccountInterestPeriods
                                          where a.PeriodStartDate == PeriodStartDate && a.GLAccountId == GLAccountId && a.Tenant == Tenant
                                          select a).FirstOrDefault();

            return Period;
        }

        public GLAccountInterestPeriod GetSingleByGLAccountIdAndTenant( string GLAccountId, int Tenant)
        {

            GLAccountInterestPeriod Period = (from a in context.GLAccountInterestPeriods
                                              where a.GLAccountId == GLAccountId && a.Tenant == Tenant
                                              select a).FirstOrDefault();

            return Period;
        }

        public List<GLAccountInterestPeriod> GetLAccountInterestPeriodPMsByInterestDate(string glAccountId, DateTime InterestCalculationDate,int tenant)
        {
            List<GLAccountInterestPeriod> gLAccountInterestPeriods = (from a in context.GLAccountInterestPeriods
                                                                      where a.Tenant == tenant && a.GLAccountId == glAccountId && a.PeriodStartDate <= InterestCalculationDate
                                                                      select a).ToList();
            return gLAccountInterestPeriods;
        }

    }

}
   