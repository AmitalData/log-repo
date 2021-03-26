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

    public partial class SecurityClearenceTypeCodeListQueryService
    {
	    private IQueryable<SecurityClearenceTypeCodeList> GetIqueryableList(IQueryable<SecurityClearenceTypeCode> iQueryable)
        {
		IQueryable<SecurityClearenceTypeCodeList> query = (from a in iQueryable
                                            select new SecurityClearenceTypeCodeList()
											{
												Code = a.Code,

												EnglishName = a.EnglishName,

												SearchFields = a.SearchFields,

												LocalName = a.LocalName,

												Inactive = a.Inactive,

											});
            return query;
		}

		private IQueryable<SecurityClearenceTypeCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SecurityClearenceTypeCode> iQueryable)
        {
			return iQueryable;
		}
	}


}
	