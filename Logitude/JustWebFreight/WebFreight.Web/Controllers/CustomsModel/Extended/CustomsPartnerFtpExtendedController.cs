using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsPartnerFtpExtendedController : ApiController
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

                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        var queryService = new CustomsPartnerFtpQueryService(MyContext);
                        var entityPM = queryService.GetSingle(id, false, false);
                        var service = new CustomsPartnerFtpUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
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


        public HttpResponseMessage GetScreenOption(int tenant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        //int tenant = authToken.Tenant;

                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        var queryService = new CustomsPartnerFtpQueryService(MyContext);
                        var details = new CustomsPartnerFtpDetails();
                        var entityPM = new
                        {
                            InterfaceDetailsItems = details.GetAllInterfaceName(),
                            PartnerCodeItems = details.GetAllPartnerCode(),
                            
                            TypeCodeItems= details.GetAllTypeCode()
                        };
                        

                        ///scope.Complete();
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

    }
}