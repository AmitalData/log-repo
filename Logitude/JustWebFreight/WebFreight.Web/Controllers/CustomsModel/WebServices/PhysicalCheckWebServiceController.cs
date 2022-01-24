using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class PhysicalCheckWebServiceController : ApiController
    {
        public HttpResponseMessage GetPhysicalCheckByDeclarationIdLists(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                PhysicalCheckQueryService queryService = new PhysicalCheckQueryService(customContext);
                List<PhysicalCheckList> physicalCheckList = queryService.GetPhysicalChecksByDeclarationId(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, physicalCheckList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostClosePhysicalCheck(string physicalCheckId, int tenant)
        {
            try
            {
                PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(tenant);
                PhysicalCheckPM physicalCheckPM = physicalCheckQueryService.GetSingle(physicalCheckId, false, false);
                if (physicalCheckPM != null)
                {
                    physicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;
                    physicalCheckPM.IsClosed = true;
                    var myEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
                        EventCode = "PCE",
                        EventRemarks = "Limit date: " + physicalCheckPM.LimitDate + ", Destination type: " + physicalCheckPM.StorageSiteName,
                        FUStatusCode = "PCE",
                        FUStatusRemarks = "Limit date: " + physicalCheckPM.LimitDate + ", Destination type: " + physicalCheckPM.StorageSiteName,
                    };

                    ICustomContext dbContext = CustomContext.GetContext(tenant);
                    PhysicalCheckUpdateService updateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    updateService.Update(physicalCheckPM, true);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




 



        public HttpResponseMessage SendSearchResults(GenericRequestParams requestParams)
        {
            try
            {
                //requestParams
                /*GenericRequestParams ContainerizationRequest = new GenericRequestParams()
                {
                    LoggingEnabled = true,
                    Tenant = requestParams.Tenant,
                    RequestName = "המכלה",
                    ResponseName = "המכלה תשובה",
                    LoggingEntityId = requestParams.LoggingEntityId,
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Containerization"),
                    MainInterfaceCode = "2450",
                    InterfaceTypeCode ="2450" , 
                    LoggingEntityReference = requestParams.AppicationId,
                    LoggingUserId = requestParams.LoggingUserId,
                    RequestVIA=requestParams.RequestVIA,
                    ForcePersonalSign=requestParams.ForcePersonalSign,
                };*/
                requestParams.MainInterfaceCode = "195";
                requestParams.InterfaceTypeCode = "195";
                 var service = new SaveCH_MSG_195_SearchResultsMessagingService();
                var responseData = service.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostCloseMarkedPhysicalChecks(string physicalCheckIds, int tenant)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(physicalCheckIds))
                {
                    var ids = physicalCheckIds.Split(',').ToList();
                    foreach (var physicalCheckId in ids)
                    {
                        PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(tenant);
                        PhysicalCheckPM physicalCheckPM = physicalCheckQueryService.GetSingle(physicalCheckId, false, false);
                        if (physicalCheckPM != null)
                        {
                            physicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;
                            physicalCheckPM.IsClosed = true;
                            var myEventContextTagModel = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
                                EventCode = "PCE",
                                EventRemarks = "Limit date: " + physicalCheckPM.LimitDate + ", Destination type: " + physicalCheckPM.StorageSiteName,
                                FUStatusCode = "PCE",
                                FUStatusRemarks = "Limit date: " + physicalCheckPM.LimitDate + ", Destination type: " + physicalCheckPM.StorageSiteName,
                            };
                            physicalCheckPM.CurrentContextTag = myEventContextTagModel;
                            ICustomContext dbContext = CustomContext.GetContext(tenant);
                            PhysicalCheckUpdateService updateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                            updateService.Update(physicalCheckPM, true);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}