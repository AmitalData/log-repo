using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.Helpers;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class TicketClassificationUpdateService
    {
        protected override void OnCreating(TicketClassificationPM entityPM, EntityPM entityParentPM)
        {
            //entityPM.Id = IdCounter.GetNumber("TicketClassification", entityPM.Tenant);
            entityPM.Id = this.GenerateNewId(entityPM);
        }

        protected override void OnUpdating(EntityPMs.TicketClassificationPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
               
            }
        }

        protected override void OnUpdating(EntityPMs.TicketClassificationPM entityPM, TicketClassification entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(TicketClassificationPM entityPM, TicketClassification entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPTC",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketClassification",
                    Notes = changesXml
                });

                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "IATC",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketClassification",
                        Notes = changesXml
                    });
                }

                else if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ACTC",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketClassification",
                        Notes = changesXml
                    });
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRTC",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketClassification",
                    Notes = changesXml
                });
            }

        }


        private string GenerateNewId(TicketClassificationPM entityPM)
        {

            TicketClassificationRepository entityRepository = new TicketClassificationRepository(entityPM.Tenant);
            string myResultId = null;

            if (string.IsNullOrEmpty(entityPM.ParentId))
            {
                myResultId = entityPM.Tenant.ToString();
            }

            else
            {
                int numberOfSplitChar = entityPM.ParentId.Count(d => d == '-');

                List<string> allIds = entityRepository.GetAllIds(entityPM.Tenant);
                List<string> matchedIds = allIds.Where(d => d.Count(c => c == '-') == numberOfSplitChar + 1).ToList();
                List<string> numerics = matchedIds.Select(d => d.Substring(d.LastIndexOf('-') + 1)).ToList();

                int maxIdNumber = 0;

                if (numerics.Count > 0)
                {
                    var x = (from max in numerics
                             select Convert.ToInt32(max)).Max();
                    maxIdNumber = x + 1;
                }

                else
                {
                    Int32.TryParse(numerics.Max(), out maxIdNumber);
                }
                
                myResultId = entityPM.ParentId + '-' + maxIdNumber.ToString();
            }

            return myResultId;
        }
    }
}
