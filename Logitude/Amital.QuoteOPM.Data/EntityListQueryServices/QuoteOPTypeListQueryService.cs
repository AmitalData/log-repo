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

    public partial class QuoteOPTypeListQueryService
    {
	    private IQueryable<QuoteOPTypeList> GetIqueryableList(IQueryable<QuoteOPType> iQueryable)
        {
		IQueryable<QuoteOPTypeList> query = (from a in iQueryable
                                            select new QuoteOPTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	