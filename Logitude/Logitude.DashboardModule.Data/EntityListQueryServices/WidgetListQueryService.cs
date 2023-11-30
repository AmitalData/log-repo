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

	public partial class WidgetListQueryService
	{
		private IQueryable<WidgetList> GetIqueryableList(IQueryable<Widget> iQueryable)
		{
			IQueryable<WidgetList> query = (from a in iQueryable
											select new WidgetList()
											{
												Id = a.Id,
												Tenant = a.Tenant,
												Title = a.Title,												
											});
			return query;
		}

		private IQueryable<Widget> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Widget> iQueryable, int tenant)
		{
			return iQueryable;
		}
		private IQueryable<Widget> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Widget> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	