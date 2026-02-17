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

    public partial class StorageStatusListQueryService
    {
	    private IQueryable<StorageStatusList> GetIqueryableList(IQueryable<StorageStatus> iQueryable)
        {
		IQueryable<StorageStatusList> query = (from a in iQueryable
                                            select new StorageStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<StorageStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<StorageStatus> iQueryable)
        {
            //throw new NotImplementedException();
            return iQueryable;

        }
			}


}
	