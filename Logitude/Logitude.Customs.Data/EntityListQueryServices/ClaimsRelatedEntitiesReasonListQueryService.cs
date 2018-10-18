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

    public partial class ClaimsRelatedEntitiesReasonListQueryService
    {
	    private IQueryable<ClaimsRelatedEntitiesReasonList> GetIqueryableList(IQueryable<ClaimsRelatedEntitiesReason> iQueryable)
        {
		IQueryable<ClaimsRelatedEntitiesReasonList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntitiesReasonList()
											{
                                                ClaimId = a.ClaimId,
                                                CounterKey = a.CounterKey,
                                                Tenant = a.Tenant,
                                                LineNo = a.LineNo,
                                                ReasonListTypeCode = a.ReasonListTypeCode,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntitiesReason> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntitiesReason> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	