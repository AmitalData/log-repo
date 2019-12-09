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

    public partial class DBMigrationListQueryService
    {
	    private IQueryable<DBMigrationList> GetIqueryableList(IQueryable<DBMigration> iQueryable)
        {
		IQueryable<DBMigrationList> query = (from a in iQueryable
                                            select new DBMigrationList()
											{
                     
					                          Id = a.Id,
					
					                          //CreateDate = a.CreateDate,
					
		                    	            });
            return query;
		}

		private IQueryable<DBMigration> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DBMigration> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	