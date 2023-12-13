using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.ServiceModel;
using Logitude.Server.Tools;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IDeclarationStatusWcfService
    {
        // GET api/<controller>
        [OperationContract]
        Response Upsert(List<DeclarationStatusPM> entityPM, bool batch);
        [OperationContract]
        Response BuildDeclarationStatusesList(int tenant, string customFileNo, List<DeclarationStatusPM> DeclarationStatusesList);
    }
}