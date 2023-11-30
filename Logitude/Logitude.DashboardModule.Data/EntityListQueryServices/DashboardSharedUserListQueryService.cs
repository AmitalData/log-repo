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

	public partial class DashboardSharedUserListQueryService
	{
		private IQueryable<DashboardSharedUserList> GetIqueryableList(IQueryable<DashboardSharedUser> iQueryable)
		{
			IQueryable<DashboardSharedUserList> query = (from a in iQueryable
														 select new DashboardSharedUserList()
														 {
															 Id = a.Id,
															 Tenant = a.Tenant,
														 });
			return query;
		}

		private IQueryable<DashboardSharedUser> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DashboardSharedUser> iQueryable, int tenant)
		{
			return iQueryable;
		}
		private IQueryable<DashboardSharedUser> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<DashboardSharedUser> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	