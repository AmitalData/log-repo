
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

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsSettingExtendedController : ApiController
    {

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

                string insurancePercent = GetDefaultPrivate("ISRAEL", "CIM_INSUR_PERC", "NON", customerCode, tenant);
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

                
                SecurityUtility.AuthenticationOnTenant(tenant);

                //CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                //CustomsSettingPM setting = settingService.GetSettingByTenantN(authToken.Tenant);
                //var resMode = new { DefaultValue = "" };
                //if (setting.IsConnectedToUniFreight)
                //{
                    string DefaultValue = GetDefaultPrivate(DISTRID, DEFID, BRANCHID, CARDID, tenant);
                   var  resMode = new { DefaultValue = DefaultValue };
                //}
                
                //return insurancePercent;
            

                return Request.CreateResponse(HttpStatusCode.OK, resMode);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSkipAutoInsurance(string customerCode, int tenant)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);


                SecurityUtility.AuthenticationOnTenant(tenant);

                var skipautoinsurance = GetSkipAutoInsurancePrivate(customerCode, tenant);

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
            if (string.IsNullOrWhiteSpace(customerCode))
            {
                return true;
            }
            var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            var pm=customsSettingQueryService.GetSettingByTenantN(tenant);
            if (!pm.IsConnectedToUniFreight)
            {
                return true;
            }
            string UNFAutoInsurance_DefaultValue = GetDefaultPrivate("ISRAEL", "CGG_AUTO_INSUR", "NON", "NON", tenant);
            if (String.IsNullOrWhiteSpace(UNFAutoInsurance_DefaultValue))
            {
                return true;
            }
            if (UNFAutoInsurance_DefaultValue == "N")
            {
                return true;
            }

            string UNFCusomer_DefaultValue = GetDefaultPrivate("ISRAEL", "CGG_CARD_INS", "NON", CARDID, tenant);
            if (string.IsNullOrWhiteSpace(UNFCusomer_DefaultValue))
            {
                return true;
            }
            return false;
        }

        private string GetDefaultPrivate(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            var cntxt = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(cntxt);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }



        public HttpResponseMessage GetSincroOption(String SincroScreen, int tenant)
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
                            case "SincroSendDeclaration":
                                {
                                    mySincroTestCaseDetailList=
                                    queryService.GetAllSincroTestCaseDetails()
                                        .Where(r => r.Entity == "Declaration")
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
    }
}