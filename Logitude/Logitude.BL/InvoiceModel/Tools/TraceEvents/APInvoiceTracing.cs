using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Collections.Generic;

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
                // trace the change in the AP invoice
                CreateEventForUpdate(entityPM, invoice, loggedContact, showLocals);
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
        private static void CreateEventForUpdate(APInvoicePM entityPM, APInvoice invoice, ContactPM loggedContact, bool showLocals)
        {
            List<string> notesList = new List<string>();

            // if is equipment has been changed
            if (entityPM.IsEquipment != invoice.IsEquipment)
            {
                string note = GetTraceEventNotesForUpdateIsEquipment(entityPM, invoice, showLocals);
                notesList.Add(note);
            }

            // if confirmation number has been changed
            if (entityPM.ConfirmationNumber != invoice.ConfirmationNumber)
            {
                string note = TranslateTextsClass.Translate("APInvoice.F.ConfirmationNumber", entityPM.Tenant) + ":\n" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant) + " " + invoice.ConfirmationNumber?.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant) + entityPM.ConfirmationNumber?.ToString();
                notesList.Add(note);
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = "UPPI",
                UserId = loggedContact.Id,
                EntityId = entityPM.Id,
                ObjectTableName = "APInvoice",
                Notes = ( notesList.Count > 0 ) ? string.Join("\n", notesList) : null
            });
        }

        private static string GetTraceEventNotesForUpdateIsEquipment(APInvoicePM entityPM, APInvoice invoice, bool showLocals)
        {
            string oldValue = GetBooleanText(invoice.IsEquipment, showLocals);
            string newValue = GetBooleanText(entityPM.IsEquipment, showLocals);
            return string.Concat(TranslateTextsClass.Translate("APInvoice.F.IsEquipment", entityPM.Tenant, showLocals), " ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), oldValue, TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), newValue);
        }

        private static string GetBooleanText(bool? value, bool showLocals)
        {
            if (value == true)
            {
                return TranslateTextsClass.Translate("Accounting.General.O.True", 0, showLocals);
            }
            else if (value == false || value == null)
            {
                return TranslateTextsClass.Translate("Accounting.General.O.False", 0, showLocals);
            }
            else return TranslateTextsClass.Translate("Accounting.General.O.False", 0, showLocals);
        }

 
    }
}
