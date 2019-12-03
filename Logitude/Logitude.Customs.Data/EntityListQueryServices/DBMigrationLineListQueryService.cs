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

    public partial class DBMigrationLineListQueryService
    {
	    private IQueryable<DBMigrationLineList> GetIqueryableList(IQueryable<DBMigrationLine> iQueryable)
        {
		IQueryable<DBMigrationLineList> query = (from a in iQueryable
                                            select new DBMigrationLineList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<DBMigrationLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DBMigrationLine> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	