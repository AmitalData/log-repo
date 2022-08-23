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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{

	public partial class WidgetTypeListQueryService
	{
		private IQueryable<WidgetTypeList> GetIqueryableList(IQueryable<WidgetType> iQueryable)
		{
			IQueryable<WidgetTypeList> query = (from a in iQueryable
												select new WidgetTypeList()
												{
													Code = a.Code,
													Name = a.Name,
													SearchFields = a.SearchFields,
												});
			return query;
		}

		private IQueryable<WidgetType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<WidgetType> iQueryable)
		{
			return iQueryable;
		}
		private IQueryable<WidgetType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<WidgetType> iQueryable)
		{
			return iQueryable;
		}
	}

}
	