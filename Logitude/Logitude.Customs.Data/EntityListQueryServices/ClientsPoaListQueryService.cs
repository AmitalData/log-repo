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

    public partial class ClientsPoaListQueryService
    {
	    private IQueryable<ClientsPoaList> GetIqueryableList(IQueryable<ClientsPoa> iQueryable)
        {
		IQueryable<ClientsPoaList> query = (from a in iQueryable
                                            select new ClientsPoaList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ClientsPoa> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientsPoa> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	