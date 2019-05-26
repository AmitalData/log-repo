using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.CRMModel.DomainServices;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class LogitudeLeadsController : ApiController
    {
        public string PostLogitudeLead(LogitudeLeadPM leadPM)
        {
            LogitudeLeadHelper logitudeLeadHelper = new LogitudeLeadHelper();
            if (leadPM != null)
            {
                if (string.IsNullOrEmpty(leadPM.Id))
                {
                    var temp = HttpContext.Current.Request.UserHostAddress;
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (currentIP == "160.153.153.150")
                    {
                        logitudeLeadHelper.CreateLogitudeLead(leadPM);
                    }
                }
            }

            return null;

        }



    }
}