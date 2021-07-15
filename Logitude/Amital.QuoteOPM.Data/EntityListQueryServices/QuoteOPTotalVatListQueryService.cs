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

    public partial class QuoteOPTotalVATListQueryService
    {
	    private IQueryable<QuoteOPTotalVATList> GetIqueryableList(IQueryable<QuoteOPTotalVAT> iQueryable)
        {
		IQueryable<QuoteOPTotalVATList> query = (from a in iQueryable
                                            select new QuoteOPTotalVATList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTotalVAT> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTotalVAT> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTotalVAT> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTotalVAT> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	