using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class APInvoiceTracing
    {
        public static void Trace(APInvoicePM entityPM, APInvoice invoice, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            bool showLocals = !loggedContact.DontShowLocal;
            // UPPI : Updated
            // CRPI : Created
            // APIA : Approved
            // APIC : Canceled
            // APIV : Voided
            // COIN : Connected
            // CPIN : Copied
            var isCreatedAPInvoiceCopied = isNewState && entityPM.IsNew && entityPM.IsCopied;
            if (isCreatedAPInvoiceCopied)
            {
                CreateEventForCopyInvoice(entityPM, loggedContact, showLocals);
            }
            if (isNewState && !isCreatedAPInvoiceCopied)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRPI",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "APInvoice",
                });
            }

            else if(!isCreatedAPInvoiceCopied)
            {
                if(entityPM.IsEquipment != invoice.IsEquipment)
                {
                    CreateEventForUpdateIsEquipment(entityPM, invoice, loggedContact, showLocals);
                }

                else {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPPI",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APInvoice",
                    });
                }
            }

            if (entityPM.SetApproved)
            {
                if ((entityPM.StatusCode == "AD" && invoice.StatusCode != "AD") || (entityPM.AmountInInvoiceCurrency == 0 && entityPM.StatusCode == "PD"))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APIA",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APInvoice",
                    });
                }
            }

            else if (entityPM.SetVoided)
            {
                if (entityPM.StatusCode == "VD" && invoice.StatusCode != "VD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APIV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APInvoice",
                    });
                }
            }

            else if (entityPM.SetCancelApproval)
            {
                if (entityPM.StatusCode == "WA" && invoice.StatusCode != "WA")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APIC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APInvoice",
                    });
                }
            }
        }

        private static void CreateEventForCopyInvoice(APInvoicePM entityPM, ContactPM loggedContact, bool showLocals)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = "CPIN",
                UserId = loggedContact.Id,
                EntityId = entityPM.Id,
                ObjectTableName = "APInvoice",
                Notes = string.Concat(TranslateTextsClass.Translate("APInvoice.M.CopiedFromAPInvoiceNumber", entityPM.Tenant, showLocals), ' ', entityPM.CopiedFrom)
            });
        }
        private static void CreateEventForUpdateIsEquipment(APInvoicePM entityPM, APInvoice invoice, ContactPM loggedContact, bool showLocals)
        {
            var notes = string.Concat(TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), invoice.IsEquipment.ToString(), TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), entityPM.IsEquipment.ToString());
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = "UPPI",
                UserId = loggedContact.Id,
                EntityId = entityPM.Id,
                ObjectTableName = "APInvoice",
                Notes = notes,
            });
        }
    }
}
