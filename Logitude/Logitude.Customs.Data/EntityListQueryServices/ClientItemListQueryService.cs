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

    public partial class ClientItemListQueryService
    {
	    private IQueryable<ClientItemList> GetIqueryableList(IQueryable<ClientItem> iQueryable)
        {
		IQueryable<ClientItemList> query = (from a in iQueryable
                                            select new ClientItemList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ClientItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientItem> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	