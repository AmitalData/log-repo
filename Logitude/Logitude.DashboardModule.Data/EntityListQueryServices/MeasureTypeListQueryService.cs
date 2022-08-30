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

	public partial class MeasureTypeListQueryService
	{
		public IQueryable<MeasureTypeList> GetIqueryableList(IQueryable<MeasureType> iQueryable)
		{
			IQueryable<MeasureTypeList> query = (from a in iQueryable
												 select new MeasureTypeList()
												 {
													 Code = a.Code,
													 Name = a.Name,
													 SearchFields = a.SearchFields,
												 });
			return query;
		}

		private IQueryable<MeasureType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<MeasureType> iQueryable)
		{
			return iQueryable;
		}
		private IQueryable<MeasureType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<MeasureType> iQueryable)
		{
			return iQueryable;
		}
	}
}
	