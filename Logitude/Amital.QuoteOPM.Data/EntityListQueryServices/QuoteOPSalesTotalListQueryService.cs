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

    public partial class QuoteOPSalesTotalListQueryService
    {
	    private IQueryable<QuoteOPSalesTotalList> GetIqueryableList(IQueryable<QuoteOPSalesTotal> iQueryable)
        {
		IQueryable<QuoteOPSalesTotalList> query = (from a in iQueryable
                                            select new QuoteOPSalesTotalList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPSalesTotal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPSalesTotal> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPSalesTotal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPSalesTotal> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	