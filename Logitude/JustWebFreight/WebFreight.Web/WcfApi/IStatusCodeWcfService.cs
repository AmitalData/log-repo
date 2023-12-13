using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IStatusCodeWcfService
    {
        // GET api/<controller>
        [OperationContract]
        Response Upsert(StatusCodePM entityPM, bool batch);
    }
}