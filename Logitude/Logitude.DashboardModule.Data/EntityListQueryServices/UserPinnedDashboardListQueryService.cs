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

	public partial class UserPinnedDashboardListQueryService
	{
		private IQueryable<UserPinnedDashboardList> GetIqueryableList(IQueryable<UserPinnedDashboard> iQueryable)
		{
			IQueryable<UserPinnedDashboardList> query = (from a in iQueryable
														 select new UserPinnedDashboardList()
														 {

															 Id = a.Id,
															 Tenant = a.Tenant,
															 UserId = a.UserId,
															 Dashboards = a.Dashboards,
														 });
			return query;
		}

		private IQueryable<UserPinnedDashboard> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<UserPinnedDashboard> iQueryable, int tenant)
		{
			return iQueryable;
		}

		private IQueryable<UserPinnedDashboard> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<UserPinnedDashboard> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	