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

    public partial class QuoteOPStageListQueryService
    {
	    private IQueryable<QuoteOPStageList> GetIqueryableList(IQueryable<QuoteOPStage> iQueryable)
        {
		IQueryable<QuoteOPStageList> query = (from a in iQueryable
                                            select new QuoteOPStageList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          MaxDays = a.MaxDays,
					
					                          SearchFields = a.SearchFields,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          Rank = a.Rank,
					
					                          InActive = a.InActive,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPStage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPStage> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPStage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPStage> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	