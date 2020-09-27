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

    public partial class GovernmentProcedureTypeListQueryService
    {
	    private IQueryable<GovernmentProcedureTypeList> GetIqueryableList(IQueryable<GovernmentProcedureType> iQueryable)
        {
            IQueryable<GovernmentProcedureTypeList> query = (from a in iQueryable
                                                             select new GovernmentProcedureTypeList()
                                                      {
                                                          Code = a.Code,
                                                          EnglishName = a.EnglishName,
                                                          LocalName = a.LocalName,
                                                          SearchFields = a.SearchFields,
                                                          IsImport = a.IsImport,
                                                          Inactive = a.Inactive,
                                                          IndexOrder = a.IndexOrder,
                                                          IsExport= a.IsExport
                                                      });
            return query;//.Where(d => !d.Code.StartsWith("1") );
		}

        private IQueryable<GovernmentProcedureType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<GovernmentProcedureType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	