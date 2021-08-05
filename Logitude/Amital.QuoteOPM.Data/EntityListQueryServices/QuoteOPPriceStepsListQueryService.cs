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

    public partial class QuoteOPPriceStepsListQueryService
    {
	    private IQueryable<QuoteOPPriceStepsList> GetIqueryableList(IQueryable<QuoteOPPriceSteps> iQueryable)
        {
		IQueryable<QuoteOPPriceStepsList> query = (from a in iQueryable
                                            select new QuoteOPPriceStepsList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          QuoteOPId = a.QuoteOPId,
					
					                          QuoteOPChargeId = a.QuoteOPChargeId,
					
					                          Step = a.Step,
					
					                          CostUnitPrice = a.CostUnitPrice,
					
					                          SaleUnitPrice = a.SaleUnitPrice,
					
					                          MarkupValue = a.MarkupValue,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPPriceSteps> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPPriceSteps> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPPriceSteps> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPPriceSteps> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	