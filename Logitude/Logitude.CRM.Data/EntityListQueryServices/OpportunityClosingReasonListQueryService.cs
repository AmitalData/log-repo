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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 
    public partial class OpportunityClosingReasonListQueryService
    {
        private IQueryable<OpportunityClosingReasonList> GetIqueryableList(IQueryable<OpportunityClosingReason> iQueryable)
        {
            IQueryable<OpportunityClosingReasonList> query = (from a in iQueryable
                                           select new OpportunityClosingReasonList()
                                           {
                                               Id = a.Id,
                                               Code = a.Code,
                                               Name = a.Name,
                                               LocalName = a.LocalName,
                                               SearchFields = a.SearchFields,
                                               IsClosedLost = a.IsClosedLost,
                                               AddedManually = a.AddedManually,
                                               Tenant = a.Tenant,
                                               InActive = a.InActive,
                                           });
            return query;
        }

        private IQueryable<OpportunityClosingReason> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<OpportunityClosingReason> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<OpportunityClosingReason> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityClosingReason> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}
}
	