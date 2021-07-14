using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.Repsitories;
using System.Linq;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.CRM.BL.EntityDws;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class TicketStageQueryService
    {
        public string GetIdByCode(string code, int tenant)
        {
            string ticketStageId = (from ticketStage in context.TicketStages
                                        where ticketStage.Code == code && ticketStage.Tenant == tenant
                                        select ticketStage.Id).FirstOrDefault();
            return ticketStageId;
        }
    }
}
