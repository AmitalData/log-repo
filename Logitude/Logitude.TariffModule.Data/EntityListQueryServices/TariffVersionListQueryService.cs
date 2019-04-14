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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{ 

    public partial class TariffVersionListQueryService
    {
	    private IQueryable<TariffVersionList> GetIqueryableList(IQueryable<TariffVersion> iQueryable)
        {
		IQueryable<TariffVersionList> query = (from a in iQueryable
                                            select new TariffVersionList()
											{
                     
					                          TariffId = a.TariffId,
					
					                          Tenant = a.Tenant,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          StartDate = a.StartDate,
					
					                          ExpirationDate = a.ExpirationDate,
					
					                          Version = a.Version,
					
		                    	            });
            return query;
		}

		private IQueryable<TariffVersion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TariffVersion> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TariffVersion> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TariffVersion> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	