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

    public partial class LeadDocumentExceptionTypeListQueryService
    {
	    private IQueryable<LeadDocumentExceptionTypeList> GetIqueryableList(IQueryable<LeadDocumentExceptionType> iQueryable)
        {
            IQueryable<LeadDocumentExceptionTypeList> query = (from a in iQueryable
                                                               select new LeadDocumentExceptionTypeList()
                                                                 {
                                                                     Code = a.Code,
                                                                     EnglishName = a.EnglishName,
                                                                     LocalName = a.LocalName,
                                                                     SearchFields = a.SearchFields,
                                                                     Inactive = a.Inactive
                                                                 });
            return query;
		}

		private IQueryable<LeadDocumentExceptionType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LeadDocumentExceptionType> iQueryable)
        {
            return iQueryable;
		}
	}


}
	