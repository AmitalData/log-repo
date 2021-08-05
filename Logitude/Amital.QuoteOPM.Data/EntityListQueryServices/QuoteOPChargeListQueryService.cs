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

    public partial class QuoteOPChargeListQueryService
    {
	    private IQueryable<QuoteOPChargeList> GetIqueryableList(IQueryable<QuoteOPCharge> iQueryable)
        {
		IQueryable<QuoteOPChargeList> query = (from a in iQueryable
                                            select new QuoteOPChargeList()
											{
                     
					                          IsCostAllIn = a.IsCostAllIn,
					
					                          TariffId = a.TariffId,
					
					                          TariffNumber = a.TariffNumber,
					
					                          TariffVersion = a.TariffVersion,
					
					                          TariffLineId = a.TariffLineId,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPCharge> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPCharge> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPCharge> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPCharge> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	