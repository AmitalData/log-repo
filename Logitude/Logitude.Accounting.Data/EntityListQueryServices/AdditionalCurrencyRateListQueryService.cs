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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class AdditionalCurrencyRateListQueryService
    {
	    private IQueryable<AdditionalCurrencyRateList> GetIqueryableList(IQueryable<AdditionalCurrencyRate> iQueryable)
        {
		IQueryable<AdditionalCurrencyRateList> query = (from a in iQueryable
                                            select new AdditionalCurrencyRateList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          Value = a.Value,
					
		                    	            });
            return query;
		}

		private IQueryable<AdditionalCurrencyRate> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AdditionalCurrencyRate> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<AdditionalCurrencyRate> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AdditionalCurrencyRate> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	