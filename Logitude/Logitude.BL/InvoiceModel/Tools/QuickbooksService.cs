using Intuit.Ipp.Core;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.InvoiceModel.Tools
{
   public static class QuickbooksService
    {

        public static string GetAccessToken(string tenant, AccountingSettingPM entityPM, Setting mySetting)
        {

            var oauth2Client = new OAuth2Client(mySetting.QBOClientID,
                    mySetting.QBOClientSecret,
                    "https://developer.intuit.com/v2/OAuth2Playground/RedirectUrl",
                    "production"); // environment is “sandbox” or “production”

            var previousRefreshToken = entityPM.RefreshToken;
            var tokenResp = oauth2Client.RefreshTokenAsync(previousRefreshToken);
            tokenResp.Wait();
            var data = tokenResp.Result;

            if (!String.IsNullOrEmpty(data.Error) || String.IsNullOrEmpty(data.RefreshToken) ||
                  String.IsNullOrEmpty(data.AccessToken))
            {
                throw new Exception("Refresh token failed - " + data.Error);
            }

            if (previousRefreshToken != data.RefreshToken)
            {
                entityPM.RefreshToken = data.RefreshToken;
                ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Id);
                AccountingSetting accountingSetting = MyContext.AccountingSettings.Where(p => p.Id == entityPM.Id).FirstOrDefault();
                if (accountingSetting != null)
                {
                    accountingSetting.RefreshToken = data.RefreshToken;
                    MyContext.AccountingSettings.Attach(accountingSetting);
                    MyContext.SetAsModified(accountingSetting);
                    MyContext.SaveChanges();
                }
            }

            return data.AccessToken;
        }


        private static ServiceContext GetServiceContextAuth2(String tenant, AccountingSettingPM entityPM, Setting mySetting)
        {
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(QuickbooksService.GetAccessToken(tenant, entityPM, mySetting));
            ServiceContext serviceContext = new ServiceContext(entityPM.QBOrealMeID, IntuitServicesType.QBO, oauthValidator);
            serviceContext.IppConfiguration.BaseUrl.Qbo = "https://quickbooks.api.intuit.com/";

            return serviceContext;

        }


        public static ServiceContext GetServiceContext(String tenant)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Setting mySetting = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository mySettingRepository = new SettingRepository();
                mySetting = mySettingRepository.GetSingleSetting("1");

                scope.Complete();
            }

            AccountingSettingQuery query = new AccountingSettingQuery(int.Parse(tenant));
            AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse(tenant));

            if (entityPM.QBOOAuth == 2 && mySetting.QBOOAuthDefault==2)
            {
                return QuickbooksService.GetServiceContextAuth2(tenant, entityPM, mySetting);
            }

            else
            {
                OAuthRequestValidator oauthValidator = new OAuthRequestValidator(entityPM.QBOAccessToken, entityPM.QBOAccessTokenSecret, mySetting.QBOConsumerKey, mySetting.QBOConsumerSecretKey);
                ServiceContext context = new ServiceContext(mySetting.QBOAppToken, entityPM.QBOrealMeID, IntuitServicesType.QBO, oauthValidator);



                return context;
            }
        }



    }
}
