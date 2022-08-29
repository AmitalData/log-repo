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

	public partial class DashboardListQueryService
	{
		private IQueryable<DashboardList> GetIqueryableList(IQueryable<Dashboard> iQueryable)
		{
			IQueryable<DashboardList> query = (from a in iQueryable
											   select new DashboardList()
											   {
												   Id = a.Id,
												   Tenant = a.Tenant,
												   CreateDate = a.CreateDate,
												   CreatedByUserId = a.CreatedByUserId,
												   UpdateDate = a.UpdateDate,
												   UpdatedByUserId = a.UpdatedByUserId,
												   SearchFields = a.SearchFields,
												   Name = a.Name,
												   Description = a.Description,
											   });
			return query;
		}

		private IQueryable<Dashboard> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Dashboard> iQueryable, int tenant)
		{
			return iQueryable;
		}
		private IQueryable<Dashboard> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Dashboard> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	