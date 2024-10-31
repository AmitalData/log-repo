using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IFreelancerGroupTypeWcfService
    {
        // GET api/<controller>
        [OperationContract]
        Response Upsert(FreelancerGroupTypePM entityPM, bool batch);
    }
}