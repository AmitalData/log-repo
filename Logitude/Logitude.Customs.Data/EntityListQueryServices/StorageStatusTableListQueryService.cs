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

    public partial class StorageStatusTableListQueryService
    {
	    private IQueryable<StorageStatusTableList> GetIqueryableList(IQueryable<StorageStatusTable> iQueryable)
        {
		IQueryable<StorageStatusTableList> query = (from a in iQueryable
                                            select new StorageStatusTableList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<StorageStatusTable> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<StorageStatusTable> iQueryable)
        {
			return iQueryable;
		}
			}


}
	