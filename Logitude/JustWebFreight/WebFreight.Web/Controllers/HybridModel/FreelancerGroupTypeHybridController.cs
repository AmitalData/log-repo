using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class FreelancerGroupTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert(FreelancerGroupTypePM entityPM)//(UserPM entityPM, bool batch)
        {
            FreelancerGroupTypeWcfService freelancerGroupTypeWcfService = new FreelancerGroupTypeWcfService();
            Response response = freelancerGroupTypeWcfService.Upsert(entityPM, false);
            return response;
        }


    }
}
