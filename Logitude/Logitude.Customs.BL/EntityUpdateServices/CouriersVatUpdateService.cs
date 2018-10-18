using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure;
using Microsoft.ServiceBus.Messaging;
using System.Transactions;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using System.IO;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Diagnostics;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CouriersVatUpdateService : EntityUpdateService<CouriersVat, CouriersVatPM, EntityPM>
    {

        protected override void OnCreating(CouriersVatPM entityPM, EntityPM entityParentPM)
        {
            ValidateEntity(entityPM);

            entityPM.Id = IdCounter.GetNumber("Customs.CouriersVat", entityPM.Tenant);
        }

        protected override void OnUpdating(CouriersVatPM entityPM, CouriersVat entityPOCO)
        {
            ValidateEntity(entityPM);

            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void Trace(CouriersVatPM entityPM, CouriersVat entityPOCO, string changesXml)
        {
            Contact loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "Customs.CouriersVat",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                //create trace event with updated type.
                string myEventNotes = "";

                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    //myEventNotes = "Previous Local Name: " + (entityPOCO.LocalName == null ? "" : entityPOCO.LocalName);
                    myEventNotes += " Local Name Changed from " + entityPOCO.LocalName + " to " + entityPM.LocalName + ". ";

                }
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    myEventNotes += " English Name Changed from " + entityPOCO.EnglishName + " to " + entityPM.EnglishName + ". ";

                }

                if (entityPM.VatNumber != entityPOCO.VatNumber && (!String.IsNullOrEmpty(entityPM.VatNumber) || !String.IsNullOrEmpty(entityPOCO.VatNumber)))
                {
                    myEventNotes += " Vat Number Changed from " + entityPOCO.VatNumber + " to " + entityPM.VatNumber + ". ";

                }

                if (entityPM.InActive != entityPOCO.InActive )
                {
                    if (entityPM.InActive)
                    {
                        myEventNotes += "Inactivated.";
                    }
                    else
                    {
                        myEventNotes += "Activated.";
                    }

                }

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "Customs.CouriersVat",
                    IsAddedManually = false,
                    EventTypeCode = "UPEV",
                    Notes = myEventNotes,

                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        internal void ValidateEntity(CouriersVatPM entityPM)
        {
            if (IsVatNumberExisit(entityPM.VatNumber, entityPM.Tenant, entityPM.Id))
            {
                throw new Exception("קיים בלדר עם ח.פ זהה"); // There is a couriers vat with the same vat number
            }


        }

        bool IsVatNumberExisit(string vatNumber, int tenant, string entityId = null)
        {
            CouriersVatRepository repo = new CouriersVatRepository(tenant);
            var exist = repo.GetByVatNumber(vatNumber, entityId);

            return ( (exist != null) ? true : false );

        }
        private Contact GetLoggedContact(int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            email = (AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : ("system@tenant" + tenant + ".com"));

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            return contact;
        }

    }
}
