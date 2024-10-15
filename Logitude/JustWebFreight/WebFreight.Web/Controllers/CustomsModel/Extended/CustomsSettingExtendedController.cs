
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.CloseTables;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsSettingExtendedController : ApiController
    {


        public HttpResponseMessage GetCustomsClosedTablePMByObjectTableId(string objectTableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                var qs = new CustomsClosedTableQueryService(customContext);
                CustomsClosedTablePM customsClosedTablePM = qs.GetCustomsClosedTableByObjectTableId(objectTableId);


                return Request.CreateResponse(HttpStatusCode.OK, customsClosedTablePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSettingByTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(customContext);
                CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(tenant);
             

                return Request.CreateResponse(HttpStatusCode.OK, CustomsSetting);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        
             public HttpResponseMessage GetAmitalRestrictOwnerModel(bool getFromCache)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                var myCustomsSettingRepository = new CustomsSettingRepository(tenant);
                var res =myCustomsSettingRepository.GetMyAmitalRestrictOwnerModel(getFromCache, tenant);

                var myAmitalRestrictOwnerService = new AmitalRestrictOwnerService();
                var ResctOwnerM =myAmitalRestrictOwnerService.GetAmitalRestrictOwnerModel(getFromCache,tenant);


                return Request.CreateResponse(HttpStatusCode.OK, ResctOwnerM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


     
        public HttpResponseMessage GetInsurancePercentDefault(string customerCode, int tenant)
        {
           


            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (string.IsNullOrWhiteSpace(customerCode) || tenant == null)
                {
                    return null;
                }

                SecurityUtility.AuthenticationOnTenant(tenant);

                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                string insurancePercent = defaultValueQueryService.GetDefault("ISRAEL", "CIM_INSUR_PERC", "NON", customerCode, tenant);
                //return insurancePercent;
                var resMode = new { insurancePercent = insurancePercent };

                return Request.CreateResponse(HttpStatusCode.OK, resMode);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);


                SecurityUtility.AuthenticationOnTenant(tenant);

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                var resMode = new { DefaultValue = "" };
                
                string DefaultValue = defaultValueQueryService.GetDefault(DISTRID, DEFID, BRANCHID, CARDID, tenant);
                resMode = new { DefaultValue = DefaultValue };
                



                return Request.CreateResponse(HttpStatusCode.OK, resMode);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSkipAutoInsurance(string customerCode, int tenant,string direction)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);


                SecurityUtility.AuthenticationOnTenant(tenant);

                var skipautoinsurance = direction == "E" ? false: GetSkipAutoInsurancePrivate(customerCode, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, new { SkipAutoInsurance = skipautoinsurance });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool GetSkipAutoInsurancePrivate(string customerCode, int tenant)
        {
            string CARDID = customerCode;
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);

            if (string.IsNullOrWhiteSpace(customerCode))
            {
                return true;
            }
 
            string UNFAutoInsurance_DefaultValue = defaultValueQueryService.GetDefault("ISRAEL", "CGG_AUTO_INSUR", "NON", "NON", tenant);
            if (String.IsNullOrWhiteSpace(UNFAutoInsurance_DefaultValue))
            {
                return true;
            }
            if (UNFAutoInsurance_DefaultValue == "N")
            {
                return true;
            }

            string UNFCusomer_DefaultValue = defaultValueQueryService.GetDefault("ISRAEL", "CGG_CARD_INS", "NON", CARDID, tenant);
            if (string.IsNullOrWhiteSpace(UNFCusomer_DefaultValue))
            {
                return true;
            }
            return false;
        }




        public HttpResponseMessage GetSincroOption(String SincroScreen, int tenant,string objectTable)
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
                        var queryService = new SincroTestCaseDetails();
                        List<SincroTestCaseDetail> mySincroTestCaseDetailList = null;
                        switch (SincroScreen)
                        {
                            case "SincroSendDeclarationPayment":
                                {
                                    mySincroTestCaseDetailList =
                                    queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "DeclarationPayment")
                                        .Where(r => !r.IsDCA)
                                        .ToList();
                                }
                                break;

                            case "SincroSendDeclaration":
                                {
                                    mySincroTestCaseDetailList=
                                    queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "Declaration")
                                        .Where(r => !r.IsDCA)
                                        .ToList();
                                }
                                break;


                            case "SincroSendManifest":
                                {
                                    mySincroTestCaseDetailList =
                                    queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "Manifest")
                                        .Where(r => !r.IsDCA)
                                        .ToList();
                                }
                                break;

                            case "SincroSendRetrieveDeclaration":
                                {
                                    mySincroTestCaseDetailList =
                                    queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "RetrieveDeclaration")
                                        .Where(r => !r.IsDCA)
                                        .ToList();
                                }
                                break;
                            case "SincroSendDeclarationDCA":
                                {
                                    mySincroTestCaseDetailList
                                        =
                                        queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "Declaration")
                                        .Where(r => r.IsDCA==true)
                                        .ToList();
                                }
                                break;

                            case "SincroSendContainerization":
                                {
                                    mySincroTestCaseDetailList
                                        =
                                        queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "Containerization")
                                        .Where(r => !r.IsDCA == true)
                                        .ToList();
                                }
                                break;

                            default:
                                throw new Exception($"SincroScreen is not valid (SincroScreen)");
                                break;
                        }
                        
                        var details = new CustomsPartnerFtpDetails();
                        var entityPM = new
                        {
                            SincroTestCaseDetailList = mySincroTestCaseDetailList,
                            MoreParams = "",
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



        public HttpResponseMessage PostSincroOption(GenericRequestParams requestParamsData)
        {
            try
            {
                
                var myDCASincroService = new DCASincroService();
                string message =myDCASincroService.BuildDCAMessage(requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = true , Message= message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        public HttpResponseMessage GetTenantDetailsMessagesPMs()
        {

            try
            {
                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);


                //SecurityUtility.AuthenticationOnTenant(tenant);
                ICustomContext MyContext = CustomContext.GetContext(0);

                CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(MyContext);

                Func<int, string> getDcaFilterByEnvironment = new Func<int, string>(tenant =>
                {
                    var dcaFilterByEnvironmentService = new Logitude.CustomsMessaging.Dca.DcaFilterByEnvironmentService();
                    var res = dcaFilterByEnvironmentService.GetDCAEnvPerTenant(tenant);
                    return res.ToString();
                });

                Func<int, bool> IsSuppressDca = new Func<int, bool>(tenant =>
                    {
                        return SecurityUtility.CheckFeature("Customs.Declaration", "DCA", tenant);

                    });
                //Feature DeclarationFeature_DCA = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DCA", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.DCA", NameTextCodeDefaultText = @"SUPPRESSDCA" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes, DeclarationObjectTable);

                var tenantMs = customsSettingQuery.GetTenantDetailsMessagesPMs(getDcaFilterByEnvironment, IsSuppressDca);

                return Request.CreateResponse(HttpStatusCode.OK,   tenantMs  );
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetLastRunningDCAWS()
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                 SecurityUtility.AuthenticationOnTenant(tenant);


                ICustomContext MyContext = CustomContext.GetContext(tenant);

                CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(MyContext);

                var tenantMs = customsSettingQuery.GetLastRunningDCAWS(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, tenantMs);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

         public HttpResponseMessage GetUpdateLastRunningDCA([FromUri]int tenant , int NumOfMessages)
        {

            try
            {
               // int tenant = 1;


                var MyContext = CustomContext.GetContext(tenant);

                CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(MyContext);
                CustomsSettingUpdateService customsSettingUpdateService = new CustomsSettingUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                var settings=   customsSettingQuery.GetSingleByTenant(tenant);

                settings.LastRunningDCAWS = DateTime.Now;
                settings.LastNumOfMessagesDCAWS = NumOfMessages;
                settings.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                customsSettingUpdateService.Update(settings, true);
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAppSettingByCode(string key)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                string val=System.Configuration.ConfigurationManager.AppSettings.Get("AngularKey."+ key);


                return Request.CreateResponse(HttpStatusCode.OK, new { val=val });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}