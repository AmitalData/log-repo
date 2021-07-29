using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.GlobalModel.Extended
{
    public class TenantManagmentPrivateLabelsController : ApiController
    {

        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                TenantManagmentPrivateLabelsQuery tenantManagmentPrivateLabelsQuery = new TenantManagmentPrivateLabelsQuery();
                TenantManagmentPrivateLabelsPM tenantManagmentPrivateLabelsPM = tenantManagmentPrivateLabelsQuery.GetSinglePM(id);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, tenantManagmentPrivateLabelsPM);
                 

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        // Hybrid Labels   
        
        public HttpResponseMessage PutGetPrivateLabelsBrandingData(PrivateLabelsBrandingDataRequest BrandingDataRequest)
        {

            try
            {
                PrivateLabelsBrandingDataService privateLabelsBrandingDataService = new PrivateLabelsBrandingDataService();
                PrivateLabelsBrandingData brandingData = privateLabelsBrandingDataService.GePrivateLabelsBrandingDataByUrl(BrandingDataRequest);
                ServiceResponse response = new ServiceResponse();
                response.Result = brandingData;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Post(TenantManagmentPrivateLabelsPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        IGlobalContext MyContext = GlobalContext.GetContext();

                        TenantManagmentPrivateLabelsRepository repository = new TenantManagmentPrivateLabelsRepository(MyContext);


                        TenantManagmentPrivateLabels poco = new TenantManagmentPrivateLabels()
                        {
                            ContactUsEmail = entityPM.ContactUsEmail,
                            HybridPartnerId = entityPM.HybridPartnerId,
                            InActive = entityPM.InActive,
                            MainLogo = entityPM.MainLogo,
                            PrivateLabelName = entityPM.PrivateLabelName,
                            SmallLogo = entityPM.SmallLogo,
                            ReceiveAllStatuses = entityPM.ReceiveAllStatuses,
                            PrivateLabelUrl = entityPM.PrivateLabelUrl,
                            PrivateLabelDomain = entityPM.PrivateLabelDomain,
                            PrivateLabelShortName = entityPM.PrivateLabelShortName,
                            MainColor = entityPM.MainColor,
                            BackgroundImageId = entityPM.BackgroundImageId,
                            LoginImageId = entityPM.LoginImageId,
                            LoginProgressImageId = entityPM.LoginProgressImageId,
                            ForgetPasswordImageId = entityPM.ForgetPasswordImageId,
                            SecondaryColor = entityPM.SecondaryColor,
                            DocumentTypeHighlightColor = entityPM.DocumentTypeHighlightColor,
                            MainTabHighlightColor = entityPM.MainTabHighlightColor,
                            HasLogboxAccess = entityPM.HasLogboxAccess,
                            SearchFields = entityPM.PrivateLabelName + "," + entityPM.PrivateLabelShortName + "," + entityPM.PrivateLabelUrl + "," + entityPM.ContactUsEmail + ",",
                            Id = IdCounter.GetNumber("TenantManagmentPrivateLabels", 0).ToString(),
                        };

                        repository.Add(poco);
                        repository.SubmitChanges();
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

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

        public HttpResponseMessage Put(TenantManagmentPrivateLabelsPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        IGlobalContext MyContext = GlobalContext.GetContext();

                        TenantManagmentPrivateLabelsRepository tenantManagmentPrivateLabelsRepository = new TenantManagmentPrivateLabelsRepository(MyContext);
                        TenantManagmentPrivateLabels Poco = tenantManagmentPrivateLabelsRepository.GetSingleTenantManagmentPrivateLabels(entityPM.Id);

                        Poco.ContactUsEmail = entityPM.ContactUsEmail;
                        Poco.HybridPartnerId = entityPM.HybridPartnerId;
                        Poco.InActive = entityPM.InActive;
                        Poco.MainLogo = entityPM.MainLogo;
                        Poco.PrivateLabelName = entityPM.PrivateLabelName;
                        Poco.PrivateLabelShortName = entityPM.PrivateLabelShortName;
                        Poco.PrivateLabelUrl = entityPM.PrivateLabelUrl;
                        Poco.PrivateLabelDomain = entityPM.PrivateLabelDomain;
                        Poco.ReceiveAllStatuses = entityPM.ReceiveAllStatuses;
                        Poco.SmallLogo = entityPM.SmallLogo;
                        Poco.MainColor = entityPM.MainColor;
                        Poco.BackgroundImageId = entityPM.BackgroundImageId;
                        Poco.LoginImageId = entityPM.LoginImageId;
                        Poco.LoginProgressImageId = entityPM.LoginProgressImageId;
                        Poco.ForgetPasswordImageId = entityPM.ForgetPasswordImageId;
                        Poco.SecondaryColor = entityPM.SecondaryColor;
                        Poco.HasLogboxAccess = entityPM.HasLogboxAccess;
                        Poco.MainTabHighlightColor = entityPM.MainTabHighlightColor;
                        Poco.DocumentTypeHighlightColor = entityPM.DocumentTypeHighlightColor;
                        Poco.SearchFields = entityPM.PrivateLabelName + "," + entityPM.PrivateLabelShortName + "," + entityPM.PrivateLabelUrl + "," + entityPM.ContactUsEmail + ",";
                        tenantManagmentPrivateLabelsRepository.Update(Poco);
                        tenantManagmentPrivateLabelsRepository.SubmitChanges();
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);
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
