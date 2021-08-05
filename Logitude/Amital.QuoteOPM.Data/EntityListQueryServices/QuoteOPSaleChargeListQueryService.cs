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

    public partial class QuoteOPSaleChargeListQueryService
    {
	    private IQueryable<QuoteOPSaleChargeList> GetIqueryableList(IQueryable<QuoteOPSaleCharge> iQueryable)
        {
		IQueryable<QuoteOPSaleChargeList> query = (from a in iQueryable
                                            select new QuoteOPSaleChargeList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPSaleCharge> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPSaleCharge> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPSaleCharge> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPSaleCharge> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	