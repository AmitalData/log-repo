using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class PortListsController : ApiController
    {

        public HttpResponseMessage GetSingleById(string id, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Port", "READ", tenant);

               

                PortRepository portRepository = new PortRepository(tenant);
                PortQuery portQuery = new PortQuery(portRepository);

                PortList result = null;
                Port port = portRepository.GetSinglePort(tenant, id);

                if (port != null)
                {
                    List<Port> singleEntityList = new List<Port>();
                    singleEntityList.Add(port);
                    
                    IQueryable<Port> iQueryable = singleEntityList.AsQueryable();
                    IQueryable<PortList> iQueryableEntityList = portQuery.GetIQueryableEntityList(iQueryable);
                    result = iQueryableEntityList.FirstOrDefault();

                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}