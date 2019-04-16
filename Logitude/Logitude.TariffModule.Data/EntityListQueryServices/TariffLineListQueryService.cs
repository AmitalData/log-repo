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

    public partial class TariffLineListQueryService
    {
	    private IQueryable<TariffLineList> GetIqueryableList(IQueryable<TariffLine> iQueryable)
        {
		IQueryable<TariffLineList> query = (from a in iQueryable
                                            select new TariffLineList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          StartDate = a.StartDate,
					
					                          ExpirationDate = a.ExpirationDate,
					
					                          TariffId = a.TariffId,
					
					                          Version = a.Version,
					
					                          MinPrice = a.MinPrice,
					
		                    	            });
            return query;
		}

		private IQueryable<TariffLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TariffLine> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TariffLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TariffLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	