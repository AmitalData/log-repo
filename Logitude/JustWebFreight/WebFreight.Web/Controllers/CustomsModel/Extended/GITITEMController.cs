using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.LT2UT;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using static Logitude.Customs.BL.Messaging.LT2UT.ItemsTableService;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class GITITEMController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.GITITEM", "READ", authToken.Tenant);

                AmitalContext MyContext = AmitalContext.GetContext(authToken.Tenant);
                GITITEMQueryService GITITEMQuery = new GITITEMQueryService(MyContext);
                GITITEMQuery.InitializeSettings();
                GITITEMPM GITITEMPM = GITITEMQuery.GetSingle(id, false);

                return Request.CreateResponse(HttpStatusCode.OK, GITITEMPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostGITITEMPM(GITITEMDto entityPM)
        {
            GITITEMPM entityGITITEMPM = MapPostGITITEMPMFromDto(entityPM);
            return Post(entityGITITEMPM);
        }
        public HttpResponseMessage PostGITITEMPMList(GITITEMDto[] GITITEMDtoList)
        {

            try
            {
                List<GITITEMPM> list = GITITEMDtoList.ToList().Select(dto => MapPostGITITEMPMFromDto(dto)).ToList();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);



                    ItemsTableService itemsTableService = new ItemsTableService(list, authToken.Tenant);
                    itemsTableService.OpenUnifreighTask("");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "ok" });
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private GITITEMPM MapPostGITITEMPMFromDto(GITITEMDto entityPM)
        {
            GITITEMPM entityGITITEMPM = new GITITEMPM();

            entityGITITEMPM.ITEMNO = entityPM.ITEMNO;
            entityGITITEMPM.PARTNERID = entityPM.PARTNERID;
            entityGITITEMPM.SAPAKID = entityPM.SAPAKID;
            entityGITITEMPM.PRATID = entityPM.PRATID;
            entityGITITEMPM.NAMEENG = entityPM.NAMEENG;
            if (string.IsNullOrWhiteSpace(entityGITITEMPM.NAMEENG))
            {
                entityGITITEMPM.NAMEENG = entityPM.ITEMNO;
            }


            entityGITITEMPM.ORIGINCOUNTRY = entityPM.ORIGINCOUNTRY;
            entityGITITEMPM.UNITID = entityPM.UNITID;
            entityGITITEMPM.TARIFFID = entityPM.TARIFFID;

            return entityGITITEMPM;
        }

        public HttpResponseMessage Post(GITITEMPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        //SecurityUtility.CheckContactFeature("Customs.GITITEM", "NEW", authToken.Tenant);

                        //AmitalContext MyContext = AmitalContext.GetContext(authToken.Tenant);
                        //GITITEMUpdateService service = new GITITEMUpdateService(_AmitalContext);

                        //var myCCUQUELOCKQueryService = new Unifreight.BL.EntityQueryServices.CCUQUELOCKQueryService(_AmitalContext);

                        //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        //service.Update(entityPM, true);



                        //ItemsTableService itemsTableService = new ItemsTableService(entityPM, authToken.Tenant);
                        List<GITITEMPM> list = new List<GITITEMPM>() { entityPM };
                        ItemsTableService itemsTableService = new ItemsTableService(list, authToken.Tenant);
                        itemsTableService.OpenUnifreighTask("");

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

        public HttpResponseMessage Put(GITITEMPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("Customs.GITITEM", "UPDATE", authToken.Tenant);

                        AmitalContext MyContext = AmitalContext.GetContext(authToken.Tenant);
                        GITITEMUpdateService service = new GITITEMUpdateService(MyContext);
                        service.InitializeEntityPM(entityPM);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
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

    }
}