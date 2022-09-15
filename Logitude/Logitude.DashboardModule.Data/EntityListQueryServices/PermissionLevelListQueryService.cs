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

	public partial class PermissionLevelListQueryService
	{
		private IQueryable<PermissionLevelList> GetIqueryableList(IQueryable<PermissionLevel> iQueryable)
		{
			return (from a in iQueryable
					select new PermissionLevelList()
					{
						Code = a.Code,
						Name = a.Name,
						SearchFields = a.SearchFields,
					});
		}

		private IQueryable<PermissionLevel> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PermissionLevel> iQueryable)
		{
			return iQueryable;
		}
		private IQueryable<PermissionLevel> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<PermissionLevel> iQueryable)
		{
			return iQueryable;
		}
	}
}
	