 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using System.Web;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class FeatureToggleRepository:IRepository<FeatureToggle>
   {
        public IQueryable<FeatureToggle> GetAllByToggleCodeList(List<string> toggleCodes, int tenant)
        {
            return from a in context.FeatureToggles
                   where a.Tenant == tenant && !a.Inactive && toggleCodes.Contains(a.ToggleCode)
                   select a;
        }

        public List<FeatureToggle> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool HasFeatureToggle(string toggleCode, int tenant)
        {
            bool result = false;
            string featureToggleName = "featuretoggle" + toggleCode + tenant;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(featureToggleName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {

                        result = (from a in context.FeatureToggles
                                  where a.ToggleCode == toggleCode
                                  && (a.TenantNumber == tenant ||  (tenant >= a.FromTenantNumber && tenant <= a.ToTenantNumber)) 
                                  && !a.Inactive
                                  select a).Any();

                        scope.Complete();
                    }

                    CacheManager.CacheWrapper.Insert(featureToggleName, result, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                }
                else
                {
                    result = (bool)CacheManager.CacheWrapper.Get(featureToggleName);
                }

            }
            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {

                    result = (from a in context.FeatureToggles
                              where a.ToggleCode == toggleCode && (a.TenantNumber == tenant || (tenant >= a.FromTenantNumber && tenant <= a.ToTenantNumber)) && !a.Inactive
                              select a).Any();

                    scope.Complete();
                }

            }
            return result;
        }

    }

}
   