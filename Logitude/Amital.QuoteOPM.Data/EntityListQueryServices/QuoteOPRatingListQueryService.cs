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

    public partial class QuoteOPRatingListQueryService
    {
	    private IQueryable<QuoteOPRatingList> GetIqueryableList(IQueryable<QuoteOPRating> iQueryable)
        {
		IQueryable<QuoteOPRatingList> query = (from a in iQueryable
                                            select new QuoteOPRatingList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          IndexOrder = a.IndexOrder,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPRating> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPRating> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPRating> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPRating> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	