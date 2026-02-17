using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class TraceEventExtendedController : ApiController
    {

        public HttpResponseMessage PutTraceEventGroup(EventTypeArgs eventTypeArgs)
        {
            try
            {
                EventTypeRepository eventTypesRepository = new EventTypeRepository(eventTypeArgs.Tenant);
                EventTypeQuery eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                List<EventTypeList> eventList = null;


                if (eventTypeArgs.EventTypeList != null && eventTypeArgs.EventTypeList.Count > 0)
                {
                    eventTypeArgs.EventTypeCodeList = new List<string>();
                    foreach (EventTypeClass eventTypeList in eventTypeArgs.EventTypeList)
                    {
                        eventTypeArgs.EventTypeCodeList.Add(eventTypeList.Code);
                    }

                    eventList = eventTypeQuery.GetEventTypeIdsByListEventCode(eventTypeArgs.EventTypeCodeList, eventTypeArgs.Tenant, eventTypeArgs.ObjectTableId);

                    foreach (EventTypeList eventTypeList in eventList)
                    {
                        EventTypeClass eventTypeClass = eventTypeArgs.EventTypeList.Where(d => d.Code == eventTypeList.Code).FirstOrDefault();
                        if (eventTypeClass != null)
                        {
                            if(eventTypeClass.Date!=null) eventTypeList.EventDateTime = eventTypeClass.Date;
                            else eventTypeList.EventDateTime = TenantServerConfigration.GetCurrentDateTime(eventTypeArgs.Tenant);
                        }
                    }

                }
                else
                {
                    eventList = eventTypeQuery.GetEventTypeIdsByListEventCode(eventTypeArgs.EventTypeCodeList, eventTypeArgs.Tenant, eventTypeArgs.ObjectTableId);
                    foreach (EventTypeList eventTypeList in eventList)
                    {
                        eventTypeList.EventDateTime = TenantServerConfigration.GetCurrentDateTime(eventTypeArgs.Tenant);

                    }

                }
               


              TraceEventRepository traceEventRepository = new TraceEventRepository(eventTypeArgs.Tenant);

                if (eventList != null && eventList.Count > 0)
                {
                    foreach (EventTypeList eventTypeList in eventList)
                    {
                        TraceEvent newEvent = new TraceEvent()
                        {
                            Id = IdCounter.GetNumber("EventType", eventTypeArgs.Tenant).ToString(),
                            LogDateTime = TenantServerConfigration.GetCurrentDateTime(eventTypeArgs.Tenant),
                            EventDateTime = (DateTime)eventTypeList.EventDateTime,
                            Tenant = eventTypeArgs.Tenant,
                            UserId = eventTypeArgs.LoggedContactId,
                            ObjectTableId = eventTypeArgs.ObjectTableId,
                            EntityId = eventTypeArgs.EntityId,
                            IsAddedManually = false,
                            EventTypeId = eventTypeList.Id,
                        };

                        traceEventRepository.Add(newEvent);
                    }

                    traceEventRepository.SubmitChanges();

                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }







    }
}