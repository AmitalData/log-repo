using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class LBPTeamMemberQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, LBPTeamMemberPM entityPM)
        {
            IInfrastructureContext context = MainContext as InfrastructureContext;
            LBPTeamMemberKeys teamGroupKeys = entityKeys as LBPTeamMemberKeys;
            TeamMemberBusinessRoleQueryService queryService = new TeamMemberBusinessRoleQueryService(context);
            entityPM.BusinessRolesList = queryService.GetMulti(teamGroupKeys, true);
        }
    }
}
