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

    public partial class QuoteOPCostChargeListQueryService
    {
	    private IQueryable<QuoteOPCostChargeList> GetIqueryableList(IQueryable<QuoteOPCostCharge> iQueryable)
        {
		IQueryable<QuoteOPCostChargeList> query = (from a in iQueryable
                                            select new QuoteOPCostChargeList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPCostCharge> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPCostCharge> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPCostCharge> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPCostCharge> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	