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

    public partial class SLAEscalationRecepientListQueryService
    {
        private IQueryable<SLAEscalationRecepientList> GetIqueryableList(IQueryable<SLAEscalationRecepient> iQueryable)
        {
            IQueryable<SLAEscalationRecepientList> query = (from a in iQueryable.Include("User.Contact")
                                                            select new SLAEscalationRecepientList()
                                                            {
                                                                Id = a.Id,

                                                                Tenant = a.Tenant,

                                                                SLAEscalationId = a.SLAEscalationId,

                                                                PreDefinitionId = a.PreDefinitionId,

                                                                UserId = a.UserId,

                                                                PreDefinitionName = a.EscalationPreDefinition == null ? "" : a.EscalationPreDefinition.Name,

                                                                UserEmail = a.User != null && a.User.Contact != null ? a.User.Contact.Email : null,
                                                            });

            return query;
        }

        private IQueryable<SLAEscalationRecepient> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SLAEscalationRecepient> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<SLAEscalationRecepient> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<SLAEscalationRecepient> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
}
