	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class ClientsTapagListQueryService
    {
	    private IQueryable<ClientsTapagList> GetIqueryableList(IQueryable<ClientsTapag> iQueryable)
        {
		IQueryable<ClientsTapagList> query = (from a in iQueryable
                                            select new ClientsTapagList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          ClientId = a.ClientId,
					
					                          TapagNumber = a.TapagNumber,
					
		                    	            });
            return query;
		}

		private IQueryable<ClientsTapag> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientsTapag> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	