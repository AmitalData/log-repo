using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class ARInvoiceTracing
    {
        public static void Trace(ARInvoicePM entityPM, ARInvoice entityPOCO, bool isNewState, string loggedContactId)
        {
            // REMF : Reminder
            // INPD : Invoice Paid
            // UPIN : Invoice Updated
            // CRIN : Invoice Created
            // INAP : Approved
            // INCA : Canceled
            // INVO : Voided
            // INSE : Invoice Sent
            // INNS : Return Invoice to Not Sent
            // INPD : Invoice Paid
            // INNP : Return Invoice to Not Paid
            // COAR : Connected

            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRIN",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoice",
                    Notes = entityPM.EventNote
                });
            }

            else
            {
                // trace the change in the AP invoice
                CreateEventForUpdate(entityPM, entityPOCO, loggedContactId);

                if (entityPM.SalesmanUserId != entityPOCO.SalesmanUserId)
                {
                    string oldSalesman = "empty";
                    string newSalesman = "empty";

                    if (entityPM.SalesmanUserId != null)
                    {
                        Contact myContact = ContactRepository.GetSingleContact(entityPM.SalesmanUserId, entityPM.Tenant, true);
                        if (myContact != null)
                        {
                            newSalesman = myContact.EnglishName;
                        }
                    }

                    if (entityPOCO.SalesmanUserId != null)
                    {
                        Contact myContact = ContactRepository.GetSingleContact(entityPOCO.SalesmanUserId, entityPM.Tenant, true);
                        if (myContact != null)
                        {
                            oldSalesman = myContact.EnglishName;
                        }
                    }

                    string remarks = "Salesman changed from " + oldSalesman + " to " + newSalesman;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ISLC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ARInvoice",
                        Notes = remarks
                    });
                }
            }

            if (entityPM.SetApproved)
            {
                if (entityPM.StatusCode == "AD" && entityPOCO.StatusCode != "AD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "INAP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ARInvoice",
                        Notes = entityPM.EventNote
                    });
                }                
            }

            else if (entityPM.SetVoided)
            {
                if (entityPM.StatusCode == "VD" && entityPOCO.StatusCode != "VD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "INVO",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ARInvoice",
                        Notes = entityPM.EventNote
                    });
                }
            }

            else if (entityPM.StatusCode == "PD" && entityPOCO.StatusCode != "PD")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INPD",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoice",
                    Notes = entityPM.EventNote
                });
            }

            else if (entityPM.StatusCode != "PD" && entityPOCO.StatusCode == "PR")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INNP",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoice",
                    Notes = entityPM.EventNote
                });
            }

            else if (entityPM.Sent && !entityPOCO.Sent)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INSE",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoice",
                    Notes = entityPM.EventNote
                });
            }

            else if (!entityPM.Sent && entityPOCO.Sent)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INNS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoice",
                    Notes = entityPM.EventNote
                });
            }
        }

        private static void CreateEventForUpdate(ARInvoicePM entityPM, ARInvoice invoice, string loggedContactId)
        {
            List<string> notesList = new List<string>();
            if (!string.IsNullOrEmpty(entityPM.EventNote))
            {
                notesList.Add(entityPM.EventNote);
            }

            // if confirmation number has been changed
            if (entityPM.ConfirmationNumber != invoice.ConfirmationNumber)
            {
                string note = TranslateTextsClass.Translate("ARInvoice.F.ConfirmationNumber", entityPM.Tenant) + ":\n" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant) + " " + invoice.ConfirmationNumber?.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant) + entityPM.ConfirmationNumber?.ToString();
                notesList.Add(note);
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = "UPIN",
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = "ARInvoice",
                Notes = (notesList.Count > 0) ? string.Join("\n", notesList) : null
            });
        }
    }
}
