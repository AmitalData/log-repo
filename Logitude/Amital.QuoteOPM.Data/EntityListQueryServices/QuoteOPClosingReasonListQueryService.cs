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

    public partial class QuoteOPClosingReasonListQueryService
    {
	    private IQueryable<QuoteOPClosingReasonList> GetIqueryableList(IQueryable<QuoteOPClosingReason> iQueryable)
        {
		IQueryable<QuoteOPClosingReasonList> query = (from a in iQueryable
                                            select new QuoteOPClosingReasonList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPClosingReason> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPClosingReason> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPClosingReason> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPClosingReason> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	