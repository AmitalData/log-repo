using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public class FieldDataTypeController : ApiController
    {
        public HttpResponseMessage GetFieldDataTypes( int tenant)
        {

            try
            {
                DataTypeRepository dataTypeRepository = new DataTypeRepository(tenant);


                List<FieldDataType> result = dataTypeRepository.GetDataTypes().Where(d => d.Code != "Byte[]" && d.Code != "Emails" && d.Code != "Constant" && d.Code != "List" && d.Code != "SigDouble" && d.Code != "UnsDecimal" && d.Code != "UnsInteger" ).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}