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

    public partial class DashboardsUserSettingListQueryService
    {
        private IQueryable<DashboardsUserSettingList> GetIqueryableList(IQueryable<DashboardsUserSetting> iQueryable)
        {
            IQueryable<DashboardsUserSettingList> query = (from a in iQueryable
                                                           select new DashboardsUserSettingList()
                                                           {

                                                               Id = a.Id,

                                                               Tenant = a.Tenant,

                                                               UserId = a.UserId,

                                                               PinnedDashboards = a.PinnedDashboards,

                                                           });
            return query;
        }

        private IQueryable<DashboardsUserSetting> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DashboardsUserSetting> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<DashboardsUserSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<DashboardsUserSetting> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
