using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.Data.Repsitories;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{
    public partial class TeamUpdateService
    {
        protected override void OnCreating(TeamPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Team", entityPM.Tenant);
            }
        }

        protected override void OnUpdating(EntityPMs.TeamPM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

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

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdateDate = myDate;
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(TeamPM entityPM, Team entityPOCO)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

        protected override void UpdateComposition(TeamPM entityPM)
        {
            LBPTeamMemberUpdateService membersUpdateService = new LBPTeamMemberUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            membersUpdateService.UpdateMulti(entityPM.MemberLines, entityPM.DeletedMemberLines, entityPM, false);
        }

        protected override void Trace(TeamPM entityPM, Team entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            string loggedContactId = null;
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            if (contact != null)
            {
                loggedContactId = contact.Id;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Team",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Team",
                    Notes = changesXml
                });

                if (entityPM.InActive && !entityPOCO.InActive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TSAI",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Team",
                        Notes = changesXml
                    });
                }

                if (!entityPM.InActive && entityPOCO.InActive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "TREA",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Team",
                        Notes = changesXml
                    });
                }
            }

            this.TraceMembers(entityPM, loggedContactId);
        }

        private void TraceMembers(TeamPM entityPM, string loggedContactId)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            TeamRepository teamRepository = new TeamRepository(entityPM.Tenant);

            foreach (LBPTeamMemberPM itemPM in entityPM.MemberLines)
            {
                string type = "";
                string name = "";
                if (!string.IsNullOrEmpty(itemPM.MemberTeamId))
                {
                    type = "Team";
                    Team team = teamRepository.GetSingle(itemPM.MemberTeamId, entityPM.Tenant);
                    if (team != null)
                    {
                        name = team.Name;
                    }
                }
                if (!string.IsNullOrEmpty(itemPM.MemberUserId))
                {
                    type = "User";
                    Contact contact = contactRep.GetSingleContact(itemPM.MemberUserId, entityPM.Tenant);
                    if (contact != null)
                    {
                        name = contact.EnglishName;
                    }
                }

                switch (itemPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "TMAD",
                                UserId = loggedContactId,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Team",
                                Notes = "Member(" + type + ") : " + name + " Added ",
                            });
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "TMRD",
                                UserId = loggedContactId,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Team",
                                Notes = "Member(" + type + ") : " + name + " Removed ",
                            });
                            break;
                        }
                }

                TraceBusinessRoles(itemPM, name, entityPM.Id, loggedContactId);
            }
        }
        private void TraceBusinessRoles(LBPTeamMemberPM entityPM, string name,string teamId, string loggedContactId)
        {
            BusinessRoleRepository roleRepository = new BusinessRoleRepository(entityPM.Tenant);

            foreach (TeamMemberBusinessRolePM itemPM in entityPM.BusinessRolesList)
            {
                string role = "";
                switch (itemPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            BusinessRole businessRole = roleRepository.GetSingle(itemPM.BusinessRoleId, itemPM.Tenant);
                            if (businessRole != null)
                            {
                                role = businessRole.Name;
                            }
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "TBRA",
                                UserId = loggedContactId,
                                EntityId = teamId,
                                ObjectTableName = "Team",
                                Notes = "Business Role(" + role + ") : " + "Added to " + name,
                            });
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            BusinessRole businessRole = roleRepository.GetSingle(itemPM.BusinessRoleId, itemPM.Tenant);
                            if (businessRole != null)
                            {
                                role = businessRole.Name;
                            }
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = entityPM.Tenant,
                                EventTypeCode = "TBRR",
                                UserId = loggedContactId,
                                EntityId = entityPM.Id,
                                ObjectTableName = "Team",
                                Notes = "Business Role(" + role + ") : " + "Removed  to " + name,
                            });
                            break;
                        }
                }

            }
        }
    }
}

