using System;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using Logitude.SystemLogs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System.Net.Http;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Linq;
using System.Net;
using WebFreight.Web.WebServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalAuthenticationController : ApiController
    {
        [ActionName("PostDigitalPortalResetPassword")]
        public UserData PostDigitalPortalResetPassword(ResetPasswordParameters resetPasswordParameters)
        {
            try
            {
                DigitalPortalPasswordHelper resetPasswordHelper = new DigitalPortalPasswordHelper();
                UserData userData = resetPasswordHelper.ForgetPassword(resetPasswordParameters, true);
                return userData;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, resetPasswordParameters.Email, "", "ResetPasswordController : PostResetPassword", null);
                UserData data = new UserData();
                data.HasError = true;
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += Environment.NewLine + e.InnerException.Message;
                }

                data.ExceptionMessage = message;
                return data;
            }
        }

        [ActionName("PostDigitalPortalChangePassword")]
        public HttpResponseMessage PostDigitalPortalChangePassword(ResetPasswordParameters param, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email)) email = email.ToLower();
                string newPassword = param.NewPassword;
                bool isResetRequest = param.IsResetRequest;
                string requestNumber = param.RequestNumber;
                bool succeeded = false;

                if (!string.IsNullOrEmpty(email))
                {
                    PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
                    if (isResetRequest && !string.IsNullOrEmpty(requestNumber))
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        GlobalContact contact = globalContext.GlobalContacts.Where(c => c.Email == email && c.InActive == false).FirstOrDefault();

                        PasswordResetRequest request = globalContext.PasswordResetRequests.Where(r => r.RequestNumber == requestNumber && r.Email.ToLower() == email.ToLower() && r.IsDone == false).FirstOrDefault();
                        if (request != null)
                        {
                            passwordChangeHelper.ValidationPassword(email, newPassword);
                            succeeded = passwordChangeHelper.ChangePassword(email, newPassword);
                            if (succeeded)
                            {
                                request.IsDone = true;
                                globalContext.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        passwordChangeHelper.ValidationPassword(email, newPassword, param.OldPassword, true);
                        succeeded = passwordChangeHelper.ChangePassword(email, newPassword, param.OldPassword);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, succeeded);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, param.Email, "", "PostDigitalPortalChangePassword : PostDigitalPortalChangePassword", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostDigitalPortalCheckPasswordUser")]
        public HttpResponseMessage PostDigitalPortalCheckPasswordUser(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(changePasswordParameter.CurrentPassword, changePasswordParameter.ContactId, 0, changePasswordParameter.Email);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, changePasswordParameter.Email, "", "PostDigitalPortalCheckPasswordUser : PostDigitalPortalCheckPasswordUser", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("GetLoggingData")]
        public HttpResponseMessage GetLoggingData(string email, int tenant)
        {
            try
            {
                SharedLogisticLoggedData myResult = new SharedLogisticLoggedData();

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                var cardRepository = new CardRepository(commonDataContext);
                var contactRepository = new ContactRepository(commonDataContext);
                var tenantRepository = new TenantRepository(commonDataContext);

                var myTenant = tenantRepository.GetSingleTenant(tenant);
                var myContact = contactRepository.GetSingleContactByEmail(email, tenant);

                if (myContact != null)
                {
                    myResult.ContactName = myContact.EnglishName;
                    myResult.ContactId = myContact.Id;
                    myResult.DigitalPortalLanguage = myContact.DigitalPortalLanguage;
                    var imageDetailId = myContact.ImageDetailId;

                    myResult.ImageFileData = GetContactImage(imageDetailId, tenant);
                }

                if (myTenant != null)
                {
                    myResult.TenantCompany = myTenant.Company;
                    myResult.LocalCurrencyCode = myTenant.Currency.Code;
                    myResult.ProfitCurrencyCode = myTenant.ProfitCurrency.Code;
                    myResult.TenantDateTimeFormat = myTenant.DateTimeFormat;
                    myResult.DisplayDocumentsAndEvents = myTenant.DisplayDocumentsAndEvents;
                    myResult.IsQuotesRequestsMenuEnabled = myTenant.IsQuoteRequestActivateInShared;
                    myResult.ShowMultiUnitsOfMeasurements = myTenant.ShowMultiUnitsOfMeasurements;
                }

                var sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
                var sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
                if (sharedLogisticsSetting != null)
                {
                    myResult.IsInvoicesMenuEnabled = sharedLogisticsSetting.IsInvoicesMenuEnabled;
                    myResult.IsAgentShared = sharedLogisticsSetting.IsAgentShared;
                    myResult.IsShipperShared = sharedLogisticsSetting.IsShipperShared;
                    myResult.IsConsigneeShared = sharedLogisticsSetting.IsConsigneeShared;
                }

                if (FeatureToggleHelper.HasFeatureToggle("RDT", tenant))
                {
                    myResult.IsDigitalPortalRequiredDocumentsEnabled = true;
                }

                myResult.IsReportsMenuEnabled = true;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, "", $"GetLoggingData : {tenant}", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string GetContactImage(string imageDetailId, int tenant)
        {
            if (string.IsNullOrEmpty(imageDetailId))
                return null;

            ImageDetailRepository imageDetailRepository = new ImageDetailRepository(tenant);
            var extension = imageDetailRepository.GetImageExtensionbyIdForDigital(imageDetailId);
            var fileLocation = "images";
            Uploader uploaderService = new Uploader();
            var imageFiledata = uploaderService.DownloadFile(imageDetailId, extension, fileLocation, tenant);
            string imageBase64String = null;
            if (imageFiledata != null)
            {
                imageBase64String = "data:image/" + extension + ";base64," + Convert.ToBase64String(imageFiledata);
            }
            return imageBase64String;
        }
    }
}