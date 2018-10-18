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

    public partial class TicketSeverityListQueryService
    {
	    private IQueryable<TicketSeverityList> GetIqueryableList(IQueryable<TicketSeverity> iQueryable)
        {
            IQueryable<TicketSeverityList> query = (from a in iQueryable
                                                    select new TicketSeverityList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Code = a.Code,
                                                     Name = a.Name,
                                                     Inactive = a.Inactive,
                                                     Severity=a.Severity,
                                                     SearchFields = a.SearchFields,
                                                 }).OrderByDescending(e => e.Severity);
            return query;
		}

		private IQueryable<TicketSeverity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TicketSeverity> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<TicketSeverity> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TicketSeverity> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	