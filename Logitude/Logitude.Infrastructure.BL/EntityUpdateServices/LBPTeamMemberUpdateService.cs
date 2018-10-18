using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{
    public partial class LBPTeamMemberUpdateService
    {
        protected override void OnCreating(LBPTeamMemberPM entityPM, TeamPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("LBPTeamMember", entityPM.Tenant);
            }
            entityPM.TeamId = entityParentPM.Id;

            string myLoggedUserId = null;

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.AddedByUserId = myLoggedUserId;

            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.AddDate = myDate;
        }

        protected override void UpdateComposition(LBPTeamMemberPM entityPM)
        {
            TeamMemberBusinessRoleUpdateService rolesUpdateService = new TeamMemberBusinessRoleUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            rolesUpdateService.UpdateMulti(entityPM.BusinessRolesList, entityPM.DeletedBusinessRolesList, entityPM, false);
        }
    }
}
