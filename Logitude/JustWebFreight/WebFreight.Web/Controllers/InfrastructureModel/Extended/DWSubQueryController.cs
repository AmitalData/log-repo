using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Global
{
    public partial class DWSubQueryController : ApiController
    {
        public HttpResponseMessage GetSingle(string Id)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectTable", "READ", authToken.Tenant);
                DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(authToken.Tenant);
                DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePM(Id, authToken.Tenant);
                DWQueryData QueryData = new DWQueryData();
                if (dWSubQueryPM != null)
                {
                    var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                    var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);

                    QueryData.SubQueryData = dWSubQueryPM;
                    QueryData.Columns = Columns;
                    QueryData.Filters = Filters;
                }
                //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, QueryData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage getByQueryId(string Id)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectTable", "READ", authToken.Tenant);
                DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(authToken.Tenant);
                DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePMByQueryid(Id, authToken.Tenant);
                DWQueryData QueryData = new DWQueryData();
                if (dWSubQueryPM != null)
                {
                    var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                    var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);

                    QueryData.SubQueryData = dWSubQueryPM;
                    QueryData.Columns = Columns;
                    QueryData.Filters = Filters;
                }
                //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, QueryData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleFromTenant(string Id, int copyFromTenant)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(copyFromTenant);

                //SecurityUtility.CheckContactFeature("DWObjectTable", "READ", authToken.Tenant);
                DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(copyFromTenant);
                DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePM(Id, copyFromTenant);
                DWQueryData QueryData = new DWQueryData();
                if (dWSubQueryPM != null)
                {
                    var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                    var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);

                    QueryData.SubQueryData = dWSubQueryPM;
                    QueryData.Columns = Columns;
                    QueryData.Filters = Filters;
                }
                //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, QueryData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage getByQueryIdFromTenant(string Id, int copyFromTenant)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(copyFromTenant);

                //SecurityUtility.CheckContactFeature("DWObjectTable", "READ", authToken.Tenant);
                DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(copyFromTenant);
                DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePMByQueryid(Id, copyFromTenant);
                DWQueryData QueryData = new DWQueryData();
                if (dWSubQueryPM != null)
                {
                    var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                    var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);

                    QueryData.SubQueryData = dWSubQueryPM;
                    QueryData.Columns = Columns;
                    QueryData.Filters = Filters;
                }
                //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, QueryData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Post(DWQueryData QueryData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("DWSubQueryPM", QueryData.SubQueryData.Tenant, authToken.Tenant);

                var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Columns);
                var FiltersXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Filters);
                //var temp = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(XML);
                //var temp1 = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(FilterXML);

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                UserRepository userRepository = new UserRepository(authToken.Tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, authToken.Tenant, true);


                var entityPM = QueryData.SubQueryData;
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                DWQueryService Qservice = new DWQueryService(objectContext, entityPM.Tenant);
                var MyQuery = new DWQueryPM();
                MyQuery.CreatedDate = DateTime.Now;
                MyQuery.UpdatedDate = DateTime.Now;
                MyQuery.CreatedByUserId = loggedUser.Id;
                MyQuery.UpdateByUserId = loggedUser.Id;
                MyQuery.Tenant = authToken.Tenant;
                Qservice.Create(MyQuery);
                entityPM.DWQueryId = MyQuery.Id;
                entityPM.ColumnsXML = ColumnsXML;
                entityPM.FiltersXML = FiltersXML;
                entityPM.Tenant = authToken.Tenant;

                DWSubQueryService service = new DWSubQueryService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(DWQueryData QueryData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("DWSubQueryPM", QueryData.SubQueryData.Tenant, authToken.Tenant);

                var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Columns);
                var FiltersXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Filters);
                var entityPM = QueryData.SubQueryData;
                entityPM.ColumnsXML = ColumnsXML;
                entityPM.FiltersXML = FiltersXML;

                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                DWSubQueryService service = new DWSubQueryService(objectContext, entityPM.Tenant);
                service.Update(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //public HttpResponseMessage Put(DWQueryPM entityPM)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //        IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
        //        DWQueryService service = new DWQueryService(objectContext, entityPM.Tenant);

        //        service.Update(entityPM);
        //        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

    }

}