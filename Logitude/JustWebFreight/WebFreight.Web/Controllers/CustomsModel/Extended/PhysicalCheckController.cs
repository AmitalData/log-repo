using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.ILOVS;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Reflection;
using Logitude.Customs.Data.EntityLists;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using System.IO;
using System.Net.Http.Headers;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.BL.CommonDataModel.EntityLists;
using WebFreight.Web.Controllers.CommonDataModel.Extended;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class PhysicalCheckController : ApiController
    {


        public HttpResponseMessage GetPhysicalCheckRequest(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter, bool suppressHugeData)
        {
            try
            {
                CommunicationLogStepController nenww = new CommunicationLogStepController();
                List<CommunicationLogStepList> communicationLogStepList = nenww.GetCommunicationLogStepsDocumentDataBystringStepFilter(mainInterfaceCode, communicationLogId, tenant, stringStepFilter, suppressHugeData) ;
                ICustomContext customContext = CustomContext.GetContext(tenant);
                PhysicalCheckPM entitypm = new PhysicalCheckPM
                {
                    Tenant = tenant
                };
                PhysicalCheckDataMapping.PhysicalCheckRequestXmlToPM(communicationLogStepList, entitypm);
                return Request.CreateResponse(HttpStatusCode.OK, entitypm);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        
      
        }
        public HttpResponseMessage GetClosedPhysicalCheck(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter, bool suppressHugeData)
        {
            try
            {
                CommunicationLogStepController nenww = new CommunicationLogStepController();
                List<CommunicationLogStepList> communicationLogStepList = nenww.GetCommunicationLogStepsDocumentDataBystringStepFilter(mainInterfaceCode, communicationLogId, tenant, stringStepFilter, suppressHugeData);
                ICustomContext customContext = CustomContext.GetContext(tenant);
                PhysicalCheckPM entitypm = new PhysicalCheckPM
                {
                    Tenant = tenant
                };
                PhysicalCheckDataMapping.ClosedPhysicalCheckXmlToPM(communicationLogStepList, entitypm);
                return Request.CreateResponse(HttpStatusCode.OK, entitypm);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
    }
}