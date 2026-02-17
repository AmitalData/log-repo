using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class EntityResourceController : ApiController
    {

        public HttpResponseMessage GetEntityResourceByTableName(string objectTableName, int tenant)
        {
            byte[] zipfilebyte = null;
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, tenant, false);
            if (objectTable != null)
            {
                zipfilebyte = objectTable.EntityResource;
            }
            return Request.CreateResponse(HttpStatusCode.OK, zipfilebyte);
        }


    }
}