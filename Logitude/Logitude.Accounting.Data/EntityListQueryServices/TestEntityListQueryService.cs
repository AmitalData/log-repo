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

    public partial class TestEntityListQueryService
    {
	    private IQueryable<TestEntityList> GetIqueryableList(IQueryable<TestEntity> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<TestEntity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TestEntity> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<TestEntity> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TestEntity> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	