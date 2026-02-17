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

    public partial class SLALineListQueryService
    {
        private IQueryable<SLALineList> GetIqueryableList(IQueryable<SLALine> iQueryable)
        {
            IQueryable<SLALineList> query = (from a in iQueryable.Include("TicketSeverity")
                                             select new SLALineList()
                                             {
                                                 Id = a.Id,

                                                 Tenant = a.Tenant,

                                                 SLAHeaderId = a.SLAHeaderId,

                                                 SeverityId = a.SeverityId,

                                                 BusinessHoursId = a.BusinessHoursId,

                                                 FirstResponseTime = a.FirstResponseTime,

                                                 FirstResponseTimeUnit = a.FirstResponseTimeUnit,

                                                 FirstResponseTimeInMinute = a.FirstResponseTimeInMinute,

                                                 ResolveWithinTime = a.ResolveWithinTime,

                                                 ResolveWithinTimeUnit = a.ResolveWithinTimeUnit,

                                                 ResolveWithinTimeInMinute = a.ResolveWithinTimeInMinute,

                                                 SeverityName = a.TicketSeverity == null ? "" : a.TicketSeverity.Name,

                                             });
            return query;
        }

        private IQueryable<SLALine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SLALine> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<SLALine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<SLALine> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }

}
	