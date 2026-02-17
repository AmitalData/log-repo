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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class GLAccountTypeListQueryService
    {
	    private IQueryable<GLAccountTypeList> GetIqueryableList(IQueryable<GLAccountType> iQueryable)
        {
            IQueryable<GLAccountTypeList> query = (from a in iQueryable
                                                   select new GLAccountTypeList()
                                                         {
                                                             Code = a.Code,
                                                             EnglishName = a.EnglishName,
                                                             LocalName = a.LocalName,
                                                             Inactive = a.Inactive,
                                                            
                                                         });
            return query;
		}

		private IQueryable<GLAccountType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountType> iQueryable)
        {
            return iQueryable;
		}

		private IQueryable<GLAccountType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountType> iQueryable)
        {
			return iQueryable;
		}
	}


}
	