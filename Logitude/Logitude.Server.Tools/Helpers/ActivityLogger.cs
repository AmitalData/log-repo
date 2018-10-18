using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.SystemLogs.POCOs;

using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;


using Simplog.Data.CommonDataModel.EntityPOCOs;

using Logitude.Server.Tools.Counters;
using System.Transactions;

namespace Logitude.Server.Tools.Helpers
{
    public class ActivityLogger
    {
        public static void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId)
        {
            try
            {
                EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
                EntityLastActivity activity = new EntityLastActivity()
                {
                    ActivityDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Id = IdCounter.GetNumber("EntityLastActivity", tenant),
                    ActivityTypeCode = activityTypeCode,
                    EntityId = entityId,
                    ObjectTableId = objectTableId,
                    Tenant = tenant,
                    UserId = userId,
                };
                entityLastActivityRepository.Add(activity);
                entityLastActivityRepository.SubmitChanges();
            }
            catch { }
        }

        public static void SendTotangoContactActivity(string email, string module, string activity, int tenant, bool isSharedLogisticsContact, string cardId)
        {

            try
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                UserRepository userRepository = new UserRepository(commonDataContext);
                ContactRepository contactrep = new ContactRepository(commonDataContext);
                Contact loggedContact = contactrep.GetSingleContactByEmail(email, tenant);
                User loggedUser = userRepository.GetSingleUser(loggedContact.Id, loggedContact.Tenant, true);
                
                //TenantRepository rep = new TenantRepository(commonDataContext);
                Tenant currentTenant = TenantRepository.GetSingleTenant(tenant, true);

                string CountryName = currentTenant.Address != null ? (currentTenant.Address.Country != null ? currentTenant.Address.Country.EnglishName : null) : null;

                string orgDisplayName = currentTenant.Company + (CountryName != null ? ("-" + CountryName.Trim()) : "");
                string organizationId = tenant.ToString();
                if (tenant == 65 || tenant == 153)
                {
                    orgDisplayName = loggedUser.Notes;
                    organizationId = loggedUser.Id;
                }

                if (isSharedLogisticsContact)
                {
                    Card card = commonDataContext.Cards.Where(d => d.Id == cardId & d.Tenant == tenant).FirstOrDefault();
                    TotangoActivityLogger.SendUserActivity(organizationId, orgDisplayName, "External Contact", module, activity, loggedContact.Id, tenant, true, cardId, card.PartnerTypeId);
                }
                else
                {
                    TotangoActivityLogger.SendUserActivity(organizationId, orgDisplayName, loggedContact.EnglishName, module, activity, loggedContact.Id, tenant, false, null, null);
                }
            }
            catch { }
        }
    }
    public interface IActivityLogger
    {
        void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId);
    }
    public class ActivityLoggerWrapper :IActivityLogger
    {

        public void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId)
        {
            
            ActivityLogger.AddAcitivityLog(entityId, objectTableId, tenant, activityTypeCode, userId);
        }
    }
}
