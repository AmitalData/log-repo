using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OccasionInviteeUpdateService
    {
        protected override void OnCreating(EntityPMs.OccasionInviteePM entityPM, EntityPMs.OccasionPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("OccasionInvitee", entityPM.Tenant);
            }

            entityPM.OccasionId = entityParentPM.Id;
        }

        public void SaveAllOccasionInvitees(int tenant,string loggedUserEmail, List<OccasionContactSearchresult> temp, ApiQueryFilters filters)
        {

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                string[] ids = { };
                ids = !string.IsNullOrEmpty(filters.Filter8Value) ? filters.Filter8Value.Split(',') : ids;
                List<OccasionInvitee> oldInvitees = this.currentContext.OccasionInvitees.Where(p => p.OccasionId == filters.Filter9Value).ToList();
                temp.RemoveAll(x => ids.Any(y => y == x.ContactId));
                var myCount = 0;
                foreach (OccasionContactSearchresult item in temp)
                {
                    if (oldInvitees.Where(p => p.ContactId == item.ContactId).FirstOrDefault() == null)
                    {
                        OccasionInvitee invitee = new OccasionInvitee();
                        invitee.Id = IdCounter.GetNumber("OccasionInvitee", tenant);
                        invitee.OccasionId = filters.Filter9Value;
                        invitee.Tenant = tenant;
                        invitee.ContactId = item.ContactId;
                        invitee.AddedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        invitee.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        invitee.AddedByUserId = loggedUser.Id;
                        invitee.UpdatedByUserId = loggedUser.Id;
                        oldInvitees.Add(invitee);
                        myCount++;
                        this.currentContext.OccasionInvitees.Add(invitee);
                        if (myCount > 500)
                        {
                            this.currentContext.SaveChanges();
                            myCount = 0;
                        }
                    }
                }
                this.currentContext.SaveChanges();
                scope.Complete();
            }
        }
    }
}
