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

    public partial class QuoteOPCustomerTypeListQueryService
    {
	    private IQueryable<QuoteOPCustomerTypeList> GetIqueryableList(IQueryable<QuoteOPCustomerType> iQueryable)
        {
		IQueryable<QuoteOPCustomerTypeList> query = (from a in iQueryable
                                            select new QuoteOPCustomerTypeList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          ShowInLOV = a.ShowInLOV,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPCustomerType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPCustomerType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPCustomerType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPCustomerType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	