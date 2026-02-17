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

    public partial class ClaimsRelatedEntitiesRefundListQueryService
    {
	    private IQueryable<ClaimsRelatedEntitiesRefundList> GetIqueryableList(IQueryable<ClaimsRelatedEntitiesRefund> iQueryable)
        {
		IQueryable<ClaimsRelatedEntitiesRefundList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntitiesRefundList()
											{
					                          ClaimId = a.ClaimId,
					                          Tenant = a.Tenant,
                                              CounterKey = a.CounterKey,
                                              RefundQuntityLineNo = a.RefundQuntityLineNo,
                                              SequenceNumeric = a.SequenceNumeric,
                                              InvoiceNumber = a.InvoiceNumber,
                                              RefundQuntity = a.RefundQuntity,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntitiesRefund> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntitiesRefund> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	