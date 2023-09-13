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
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using System.Transactions;
using Logitude.Customs.BL.EntityUpdateServices;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsDocumentsTicketsExtendedController : ApiController
    {
        public HttpResponseMessage Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticationOnTenant(tenant);



                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        CustomsDocumentsTicketQueryService queryService = new CustomsDocumentsTicketQueryService(MyContext);
                        CustomsDocumentsTicketPM entityPM = queryService.GetSingle(id, true, false);
                        CustomsDocumentsTicketUpdateService service = new CustomsDocumentsTicketUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        CustomsDocumentPointerQueryService pointerQueryService = new CustomsDocumentPointerQueryService(MyContext);

                        List<CustomsDocumentPointerPM> pointers = pointerQueryService.GetPointersForTicket(entityPM.Id, entityPM.Tenant);

                        foreach (CustomsDocumentPointerPM pointer in pointers)
                        {
                            entityPM.DeletedCustomsDocumentPointers.Add(pointer);
                            pointer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        }
                        service.Update(entityPM, true);
                        
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetIsConnectDec(string documentsfilingid,string entityId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticationOnTenant(tenant);



                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        CustomsDocumentsTicketQueryService queryService = new CustomsDocumentsTicketQueryService(MyContext);
                    

                        List<string> decConnect= queryService.GetIsConnectDec(documentsfilingid, entityId);
                       var conDec = new ConnectedDeclarations();
                        conDec.decConnect = decConnect;
                      
                        return Request.CreateResponse(HttpStatusCode.OK, conDec);
                   
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetIsConnectTicket(string documentsfilingid, string entityId, int tenant)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);



                    ICustomContext MyContext = CustomContext.GetContext(tenant);
                    CustomsDocumentsTicketQueryService queryService = new CustomsDocumentsTicketQueryService(MyContext);
                    

                    List<string> decConnect = queryService.GetDocConnectTicket(documentsfilingid, entityId, tenant);
                    var conDec = new ConnectedDeclarations();
                    conDec.decConnect = decConnect;

                    return Request.CreateResponse(HttpStatusCode.OK, conDec);

                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetIsSendToCustomsAndNotConnectTicket(string documentsfilingid, int tenant, string entityId)
        {
           
                try
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    ICustomContext MyContext = CustomContext.GetContext(tenant);
                    CustomsDocumentsTicketQueryService queryService = new CustomsDocumentsTicketQueryService(MyContext);

                    bool response = queryService.IsSendToCustomsAndNotConnectTicket(documentsfilingid, entityId, tenant);
                    

                    return Request.CreateResponse(HttpStatusCode.OK, response);

                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
           
        }
        public class ConnectedDeclarations
        {
            public List<string> decConnect { get; set; }
        }
    }
}