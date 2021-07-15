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

    public partial class QuoteOPVATsTotalListQueryService
    {
	    private IQueryable<QuoteOPVATsTotalList> GetIqueryableList(IQueryable<QuoteOPVATsTotal> iQueryable)
        {
		IQueryable<QuoteOPVATsTotalList> query = (from a in iQueryable
                                            select new QuoteOPVATsTotalList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPVATsTotal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPVATsTotal> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPVATsTotal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPVATsTotal> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	