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

	public partial class WidgetMeasureListQueryService
	{
		private IQueryable<WidgetMeasureList> GetIqueryableList(IQueryable<WidgetMeasure> iQueryable)
		{
			IQueryable<WidgetMeasureList> query = (from a in iQueryable
												   select new WidgetMeasureList()
												   {
													   Id = a.Id,
													   Tenant = a.Tenant,
												   });
			return query;
		}

		private IQueryable<WidgetMeasure> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<WidgetMeasure> iQueryable, int tenant)
		{
			return iQueryable;
		}
		private IQueryable<WidgetMeasure> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<WidgetMeasure> iQueryable, int tenant)
		{
			return iQueryable;
		}

	}
}
	