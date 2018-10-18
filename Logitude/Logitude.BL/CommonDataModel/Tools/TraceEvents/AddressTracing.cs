using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class AddressTracing
    {
        public static void Trace(AddressPM entityPM, Address entityPOCO, bool isNewEntity)
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);

            string myPartnerTypeId = null;
            if (!string.IsNullOrEmpty(entityPM.CardId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CardId, entityPM.Tenant, false);
                if (myCard != null)
                {
                    myPartnerTypeId = myCard.PartnerTypeId;
                }
            }

            if (isNewEntity)
            {
                if (myPartnerTypeId == "CS" || myPartnerTypeId == "PO")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ARUP",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Customer",
                        Notes = "Created: " + entityPM.Description,
                    });
                }
            }

            else
            {
                if (myPartnerTypeId == "CS" || myPartnerTypeId == "PO")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ARUP",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Customer",
                        Notes = "Updated: " + entityPM.Description,
                    });
                }
            }
        }
    }
}
