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

    public partial class QuoteOPListQueryService
    {
	    private IQueryable<QuoteOPList> GetIqueryableList(IQueryable<QuoteOP> iQueryable)
        {
		IQueryable<QuoteOPList> query = (from a in iQueryable
                                            select new QuoteOPList()
											{
                     
					                          Id = a.Id,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOP> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOP> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOP> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOP> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	