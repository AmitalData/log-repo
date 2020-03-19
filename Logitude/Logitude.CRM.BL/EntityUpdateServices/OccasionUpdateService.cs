using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using Simplog.Server.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OccasionUpdateService
    {
        protected override void OnCreating(OccasionPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Occasion", entityPM.Tenant);
            entityPM.OccasionStatusId = "PL";
        }

        protected override void OnUpdating(EntityPMs.OccasionPM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            var isAllAdded = entityPM.IsAllAdded;
            var test = entityPM.RemovedOccasionInvitees;
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entityPM.UpdateDate = myDate;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (loggedContact != null && entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                }
            }       
        }


        protected override void OnUpdating(EntityPMs.OccasionPM entityPM, Occasion entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                this.MapDummyFields(entityPM, entityPOCO);
                this.ComputeOccasionCountsFields(entityPM);
            }
        }

        protected override void UpdateComposition(OccasionPM entityPM)
        {
            OccasionInviteeUpdateService occasionInviteeUpdateService = new OccasionInviteeUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            occasionInviteeUpdateService.UpdateMulti(entityPM.OccasionInvitees, entityPM.DeletedOccasionInvitees, entityPM, false);
        }

        private void MapDummyFields(OccasionPM entityPM, Occasion entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPM.IndustryId) && (entityPM.IndustryId != entityPOCO.IndustryId))
            {
                IndustryRepository iRepository = new IndustryRepository(entityPM.Tenant);
                Industry iEntity = iRepository.GetSingleIndustry(entityPM.IndustryId, entityPM.Tenant);
                if (iEntity != null)
                {
                    entityPM.IndustryName = iEntity.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.OccasionStatusId) && (entityPM.OccasionStatusId != entityPOCO.OccasionStatusId))
            {
                OccasionStatusRepository iRepository = new OccasionStatusRepository(entityPM.Tenant);
                OccasionStatusKeys iKeys = new OccasionStatusKeys() { Code = entityPM.OccasionStatusId };
                OccasionStatus iEntity = iRepository.GetSingle(iKeys);
                if (iEntity != null)
                {
                    entityPM.OccasionStatusName = iEntity.Name;
                }
            }
        }

        protected override void Trace(OccasionPM entityPM, Occasion entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Occasion",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Occasion",
                    Notes = changesXml
                });
            }

        }

        private void ComputeOccasionCountsFields(OccasionPM entityPM)
        {
            if (entityPM.OccasionInvitees != null && entityPM.OccasionInvitees.Count() > 0)
            {
                var invitedList = entityPM.OccasionInvitees.Where(a=>a.ChangeSetOp != ChangeSetOperation.Delete).Where(a => a.Invited);
                var participatedList = entityPM.OccasionInvitees.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete).Where(a => a.Participated);

                entityPM.InvitedContacts = invitedList != null ? invitedList.Count() : 0;
                entityPM.ParticipatedContacts = participatedList != null ? participatedList.Count() : 0;

                List<string> contactIds_Invited = (from a in invitedList
                                                   select a.ContactId).ToList();

                List<string> contactIds_Participated = (from a in participatedList
                                                        select a.ContactId).ToList();

                CardContactRepository cardContactRepository = new CardContactRepository(entityPM.Tenant);
                entityPM.InvitedCustomers = cardContactRepository.GetCardsContactsForContactIds_Count(contactIds_Invited, entityPM.Tenant);
                entityPM.ParticipatedCustomers = cardContactRepository.GetCardsContactsForContactIds_Count(contactIds_Participated, entityPM.Tenant);
            }
        }
    }
}
