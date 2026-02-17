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

    public partial class ClaimsRelatedEntitiesSeizureListQueryService
    {
	    private IQueryable<ClaimsRelatedEntitiesSeizureList> GetIqueryableList(IQueryable<ClaimsRelatedEntitiesSeizure> iQueryable)
        {
		IQueryable<ClaimsRelatedEntitiesSeizureList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntitiesSeizureList()
											{
					                          ClaimId = a.ClaimId,
					                          CounterKey = a.CounterKey,
					                          Tenant = a.Tenant,
					                          SeizureLinoNo = a.SeizureLinoNo,
                                              SeizureAmount = a.SeizureAmount,
                                              SeizureFactorCode = a.SeizureFactorCode,
                                              SeizureFactorName = a.SeizureFactorType != null ? a.SeizureFactorType.LocalName : null,
                                              SeizureMethodCode = a.SeizureMethodCode,
                                              SeizureMethodName = a.SeizureMethodType != null ? a.SeizureMethodType.LocalName : null,
                                            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntitiesSeizure> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntitiesSeizure> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	