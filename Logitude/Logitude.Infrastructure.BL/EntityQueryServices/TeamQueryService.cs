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
    public partial class TeamQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TeamPM entityPM)
        {
            IInfrastructureContext context = MainContext as InfrastructureContext;
            TeamKeys teamGroupKeys = entityKeys as TeamKeys;
            LBPTeamMemberQueryService queryService = new LBPTeamMemberQueryService(context);
            entityPM.MemberLines = queryService.GetMulti(teamGroupKeys, true);
        }
    }
}
