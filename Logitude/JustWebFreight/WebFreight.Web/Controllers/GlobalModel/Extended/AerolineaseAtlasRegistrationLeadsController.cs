
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

namespace WebFreight.Web.Controllers.GlobalModel.Extended
{
    public class AerolineaseAtlasRegistrationLeadsController : ApiController
    {
        public string PostLogitudeLead(LogitudeLeadPM leadPM)
        {
            LogitudeLeadHelper logitudeLeadHelper = new LogitudeLeadHelper();
            if (leadPM != null)
            {
                if (string.IsNullOrEmpty(leadPM.Id)) logitudeLeadHelper.CreateLogitudeLead(leadPM);
            }
            

            return null;

        }



    }
}