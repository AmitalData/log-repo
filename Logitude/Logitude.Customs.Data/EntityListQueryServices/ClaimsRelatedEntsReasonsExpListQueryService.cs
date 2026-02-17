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

    public partial class ClaimsRelatedEntsReasonsExpListQueryService
    {
	    private IQueryable<ClaimsRelatedEntsReasonsExpList> GetIqueryableList(IQueryable<ClaimsRelatedEntsReasonsExp> iQueryable)
        {
		IQueryable<ClaimsRelatedEntsReasonsExpList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntsReasonsExpList()
											{
                                                ClaimId = a.ClaimId,
                                                CounterKey = a.CounterKey,
                                                Tenant = a.Tenant,
                                                ReasonLineNo = a.ReasonLineNo,
                                                LineNo = a.LineNo,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntsReasonsExp> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntsReasonsExp> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	