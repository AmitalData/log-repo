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

    public partial class SLAEscalationListQueryService
    {
        private IQueryable<SLAEscalationList> GetIqueryableList(IQueryable<SLAEscalation> iQueryable)
        {
            IQueryable<SLAEscalationList> query = (from a in iQueryable
                                                   select new SLAEscalationList()
                                                   {
                                                       Id = a.Id,

                                                       Tenant = a.Tenant,

                                                       SLAHeaderId = a.SLAHeaderId,

                                                       LineNumber = a.LineNumber,

                                                       EscalationFor = a.EscalationFor,

                                                       EscalationActionTimeIndicator = a.EscalationActionTimeIndicator,

                                                       EscalationTime = a.EscalationTime,

                                                       EscalationTimeUnit = a.EscalationTimeUnit,

                                                       EscalaitonTimeInMinutes = a.EscalaitonTimeInMinutes,

                                                       TimeIndicator = a.EscalationActionTime == null ? "" : a.EscalationActionTime.Name,

                                                       TimeUnitName = a.TimeUnit == null ? "" : a.TimeUnit.Name,

                                                   });
            return query;
        }

        private IQueryable<SLAEscalation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SLAEscalation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<SLAEscalation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<SLAEscalation> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
}
