using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Transactions;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.Helpers;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public partial class AWBSpecialHandlingCodesExtendedController : ApiController
    {
        public HttpResponseMessage Post(AWBSpecialHandlingCodePM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(0);
                    SecurityUtility.CheckContactFeature("AWBSpecialHandlingCode", "NEW", 0);

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                    AWBSpecialHandlingCodeService service = new AWBSpecialHandlingCodeService(objectContext, entityPM);
                    service.Create();

                    TableLastUpdateClass.UpdateTableHistory(0, "AWBSpecialHandlingCode");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(AWBSpecialHandlingCodePM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(0);
                    SecurityUtility.CheckContactFeature("AWBSpecialHandlingCode", "UPDATE", 0);

                    string entityName = "AWBSpecialHandlingCode" + entityPM.Id + 0;
                    string entityPmName = "AWBSpecialHandlingCodePM" + entityPM.Id + 0;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                    AWBSpecialHandlingCodeService service = new AWBSpecialHandlingCodeService(objectContext, entityPM);
                    service.Update();

                    TableLastUpdateClass.UpdateTableHistory(0, "AWBSpecialHandlingCode");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}