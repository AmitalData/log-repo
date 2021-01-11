using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class APPaymentTracing
    {
        public static void Trace(EntityPMs.APPaymentPM entityPM, APPayment payment, bool isNewState)
        {
            string myEntityName = "APPayment";

            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
            bool showLocals = !loggedContact.DontShowLocal;


            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
            }


            if (entityPM.SetApproved)
            {
                if (payment.StatusCode != "AD" && entityPM.StatusCode == "AD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPA",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetVoided)
            {
                if (payment.StatusCode != "VD" && entityPM.StatusCode == "VD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetCancelApproval)
            {
                if (payment.StatusCode != "DR" && entityPM.StatusCode == "DR")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if(entityPM.InternalNotes != payment.InternalNotes)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + payment.InternalNotes + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.InternalNotes
                });
            }


            else if (entityPM.PrintNotes != payment.PrintNotes)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + payment.PrintNotes + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.PrintNotes
                });
            }

            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
            }

            TraceExternalPayment(entityPM, payment, loggedContact.Id);
        }


     

        private static void TraceExternalPayment(APPaymentPM entityPM, APPayment payment, string loggedContactId)
        {
            if (entityPM.ExternalPaymentAmount != null && entityPM.ExternalPaymentDate != null)
            {
                if (entityPM.ExternalPaymentAmount != payment.ExternalPaymentAmount || entityPM.ExternalPaymentDate != payment.ExternalPaymentDate)
                {
                    string notes = "";
                    notes += "Amount: " + String.Format("{0:0,0.00}", entityPM.ExternalPaymentAmount.Value);
                    notes += "\nDate: " + String.Format("{0:dd MMM yyyy}", entityPM.ExternalPaymentDate);
                    notes += "\nNotes : " + entityPM.ExternalPaymentNotes;

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "PXTR",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APPayment",
                        Notes = notes,
                    });
                }
            }
        }
    }
}
