using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityLists;

namespace Logitude.DashboardModule.Data.EntityListQueryServices
{

    public partial class DashboardGlobalPresetFilterListQueryService
    {
        private IQueryable<DashboardGlobalPresetFilterList> GetIqueryableList(IQueryable<DashboardGlobalPresetFilter> iQueryable)
        {
            IQueryable<DashboardGlobalPresetFilterList> query = (from a in iQueryable
                                                                 select new DashboardGlobalPresetFilterList()
                                                                 {

                                                                     Code = a.Code,

                                                                     DisplayName = a.DisplayName,

                                                                     DataTypeCode = a.DataTypeCode,

                                                                     IsDisabled = a.IsDisabled,

                                                                     IsMultiSelect = a.IsMultiSelect,

                                                                     JoinedTableName = a.JoinedTableName,

                                                                     Sort = a.Sort,

                                                                     JoinedTableDisplayField = a.JoinedTableDisplayField,

                                                                     CanSearch = a.CanSearch,

                                                                 });
            return query;
        }

        private IQueryable<DashboardGlobalPresetFilter> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DashboardGlobalPresetFilter> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<DashboardGlobalPresetFilter> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<DashboardGlobalPresetFilter> iQueryable)
        {
            return iQueryable.Where(x => !x.IsDisabled);
        }

    }


}
