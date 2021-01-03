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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

	public partial class CustomsAutonomyKeywordListQueryService
	{
		private IQueryable<CustomsAutonomyKeywordList> GetIqueryableList(IQueryable<CustomsAutonomyKeyword> iQueryable)
		{

			IQueryable<CustomsAutonomyKeywordList> query = (from a in iQueryable
															select new CustomsAutonomyKeywordList()
															{

																Id = a.Id,

																Tenant = a.Tenant,

																KeywordtypeCode = a.KeywordtypeCode,
																KeywordsList = a.KeywordsList,
																KeywordtypeLocalName = a.KeywordtypeCode == "1" ? "עיר" : (a.KeywordtypeCode == "2" ? "טלפון" : "קידומת ת\"ז/ח\"פ פלסטינאי")

															});
			return query;
		}

		private IQueryable<CustomsAutonomyKeyword> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsAutonomyKeyword> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}


}
	