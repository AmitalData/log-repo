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

    public partial class DeclarationPendingListQueryService
    {
	    private IQueryable<DeclarationPendingList> GetIqueryableList(IQueryable<DeclarationPending> iQueryable)
        {
		IQueryable<DeclarationPendingList> query = (from a in iQueryable
                                            select new DeclarationPendingList()
											{
                                                DeclarationID = a.DeclarationID,
                                                Tenant = a.Tenant,
                                                CourierPendingReasonCode = a.CourierPendingReasonCode,
                                                CourierPendingReasonName = a.CourierPendingReason != null ? a.CourierPendingReason.LocalName : null,
                                                PendingRemarks = a.PendingRemarks,
                                                Status = a.Status,
                                            });
            return query;
		}

		private IQueryable<DeclarationPending> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationPending> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	