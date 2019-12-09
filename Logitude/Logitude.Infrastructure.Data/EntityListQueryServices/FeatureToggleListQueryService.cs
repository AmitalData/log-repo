using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System.Linq;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{

    public partial class FeatureToggleListQueryService
    {
        public IQueryable<FeatureToggleList> GetIqueryableList(IQueryable<FeatureToggle> iQueryable)
        {
            IQueryable<FeatureToggleList> query = (from a in iQueryable.Include("Toggle").Include("CreatedByUser")
                                                   where !a.Inactive
                                                   select new FeatureToggleList() 
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       SearchFields = a.SearchFields,
                                                       TenantNumber = a.TenantNumber,
                                                       Inactive = a.Inactive,
                                                       ToggleCode = a.ToggleCode,
                                                       ToggleName = a.Toggle == null ? null : a.Toggle.Name,
                                                       CreatedByUser = a.CreatedByUser == null ? null : a.CreatedByUser.Contact.EnglishName,
                                                   });
            return query;
        }

        private IQueryable<FeatureToggle> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<FeatureToggle> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<FeatureToggle> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<FeatureToggle> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}	