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

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class QuoteOPPropertiesListQueryService
    {
	    private IQueryable<QuoteOPPropertiesList> GetIqueryableList(IQueryable<QuoteOPProperties> iQueryable)
        {
		IQueryable<QuoteOPPropertiesList> query = (from a in iQueryable
                                            select new QuoteOPPropertiesList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          QuoteID = a.QuoteID,
					
					                          Order = a.Order,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPProperties> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPProperties> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPProperties> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPProperties> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	