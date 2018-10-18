using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class ProductTypeTracing
    {
        public static void Trace(ProductTypePM entityPM, ProductType poco, bool isNewEntity, ProductTypeModification modification)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !modification.InActive)
                {
                    notes = "Product Type Inactivated";
                }

                else if (!entityPM.InActive && modification.InActive)
                {
                    notes = "Product Type Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPPR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Code,
                    ObjectTableName = "ProductType",
                    Notes = notes,
                });
            }
        }
    }
}
