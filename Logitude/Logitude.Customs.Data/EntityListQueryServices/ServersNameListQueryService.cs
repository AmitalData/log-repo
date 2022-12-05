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

    public partial class ServersNameListQueryService
    {
	    private IQueryable<ServersNameList> GetIqueryableList(IQueryable<ServersName> iQueryable)
        {
		IQueryable<ServersNameList> query = (from a in iQueryable
                                            select new ServersNameList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          ServerName = a.ServerName,
					
					                          ServiceName = a.ServiceName,
					
		                    	            });
            return query;
		}

		private IQueryable<ServersName> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ServersName> iQueryable, int tenant)
        {

			return iQueryable;
		}
			}


}
	