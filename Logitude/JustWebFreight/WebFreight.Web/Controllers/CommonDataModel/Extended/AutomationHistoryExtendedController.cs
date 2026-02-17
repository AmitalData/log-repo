using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class AutomationHistoryExtendedController : ApiController
    {


        public HttpResponseMessage GetAutomationHistoryesByAutomationId(string automationId, int tenant)
        {
            try
            {
                
             AutomationHistoryQuery automationHistoryQuery = new AutomationHistoryQuery(tenant);
             List<AutomationHistoryPM> myResult =  automationHistoryQuery.GetAutomationHistoryPMsByAutomationId(automationId, tenant,true);
      
              return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetAutomationBackupDataByAutomationId(string automationId, int version, int tenant)
        {
            try
            {

                AutomationHistoryQuery automationHistoryQuery = new AutomationHistoryQuery(tenant);
                string automationxmal = automationHistoryQuery.GetAutomationBackupDataByAutomationId(automationId, version, tenant);
                AutomatedBackup automatedDataBackup = new AutomatedBackup();
                if (!string.IsNullOrEmpty(automationxmal))
                {
                    automatedDataBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationxmal);
                }

                return Request.CreateResponse(HttpStatusCode.OK, automatedDataBackup);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    
    }
}