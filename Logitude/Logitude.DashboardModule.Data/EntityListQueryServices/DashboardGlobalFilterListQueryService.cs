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

	public partial class DashboardGlobalFilterListQueryService
	{
		private IQueryable<DashboardGlobalFilterList> GetIqueryableList(IQueryable<DashboardGlobalFilter> iQueryable)
		{
			IQueryable<DashboardGlobalFilterList> query = (from a in iQueryable
														   select new DashboardGlobalFilterList()
														   {
															   Id = a.Id,
															   Tenant = a.Tenant,
														   });
			return query;
		}

		private IQueryable<DashboardGlobalFilter> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DashboardGlobalFilter> iQueryable, int tenant)
		{
			return iQueryable;
		}
		private IQueryable<DashboardGlobalFilter> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<DashboardGlobalFilter> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	