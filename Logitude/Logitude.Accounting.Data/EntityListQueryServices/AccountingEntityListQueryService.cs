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

    public partial class AccountingEntityListQueryService
    {
	    private IQueryable<AccountingEntityList> GetIqueryableList(IQueryable<AccountingEntity> iQueryable)
        {
            IQueryable<AccountingEntityList> query = (from a in iQueryable
                                                      select new AccountingEntityList()
                                                         {
                                                             Code = a.Code,
                                                             EnglishName = a.EnglishName,
                                                             LocalName = a.LocalName,
                                                         });
            return query;
		}

		private IQueryable<AccountingEntity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingEntity> iQueryable)
        {
            return iQueryable;
		}

		private IQueryable<AccountingEntity> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingEntity> iQueryable)
        {
			return iQueryable;
		}
	}


}
	