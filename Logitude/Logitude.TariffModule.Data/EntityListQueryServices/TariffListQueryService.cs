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

    public partial class TariffListQueryService
    {
	    private IQueryable<TariffList> GetIqueryableList(IQueryable<Tariff> iQueryable)
        {
		IQueryable<TariffList> query = (from a in iQueryable
                                            select new TariffList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          StartDate = a.StartDate,
					
					                          ExpirationDate = a.ExpirationDate,
					
					                          Name = a.Name,
					
					                          InActive = a.InActive,
					
					                          Description = a.Description,
					
					                          SellerId = a.SellerId,
					
					                          CurrencyId = a.CurrencyId,
					
					                          UpdateDate = a.UpdateDate,
					
		                    	            });
            return query;
		}

		private IQueryable<Tariff> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Tariff> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<Tariff> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Tariff> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	