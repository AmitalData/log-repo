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

    public partial class ClaimsRelatedEntitiesAmountListQueryService
    {
	    private IQueryable<ClaimsRelatedEntitiesAmountList> GetIqueryableList(IQueryable<ClaimsRelatedEntitiesAmount> iQueryable)
        {
		IQueryable<ClaimsRelatedEntitiesAmountList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntitiesAmountList()
											{
                                                ClaimId = a.ClaimId,
                                                Tenant = a.Tenant,
                                                CounterKey = a.CounterKey,
                                                LineNo = a.LineNo,
                                                PaymentTypeCode = a.PaymentTypeCode,
                                                Amount = a.Amount,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntitiesAmount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntitiesAmount> iQueryable, int tenant)
        {
            return iQueryable;
		}
    }


}
	