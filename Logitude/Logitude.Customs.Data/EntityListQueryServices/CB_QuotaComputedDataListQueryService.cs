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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CB_QuotaComputedDataListQueryService
    {
	    private IQueryable<CB_QuotaComputedDataList> GetIqueryableList(IQueryable<CB_QuotaComputedData> iQueryable)
        {
		IQueryable<CB_QuotaComputedDataList> query = (from a in iQueryable
                                            select new CB_QuotaComputedDataList()
											{
                     
					                          CB_ID = a.CB_ID,
					
					                          ID = a.ID,
					
					                          QuotaID = a.QuotaID,
					
					                          Title = a.Title,
					
					                          ValidQuotaDetailsHistoryID = a.ValidQuotaDetailsHistoryID,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_QuotaComputedData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_QuotaComputedData> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	