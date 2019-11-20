
using Intuit.Ipp.OAuth2PlatformClient;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class QuickbooksOnlineAuth2 : System.Web.UI.Page
    {

        public String oauth_token;
        public String oauth_tokenCallBack;
        public static String REQUEST_TOKEN_URL = "https://oauth.intuit.com/oauth/v1/get_request_token";
        public static String ACCESS_TOKEN_URL = "https://oauth.intuit.com/oauth/v1//get_access_token";
        public static String AUTHORIZE_URL = "https://appcenter.intuit.com/Connect/Begin";
        public static String OAUTH_URL = "https://oauth.intuit.com/oauth/v1";


        public string oauth_callback_url = LogitudeSettings.LogitudeURL + "/QuickbooksOnlineAuth2.aspx";
        public string GrantUrl = LogitudeSettings.LogitudeURL + "/QuickbooksOnlineAuth2.aspx?connect=true";
        public OAuth2Client oauthClient;

        public List<string> queryKeys;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                if (Request.QueryString.Count > 0)
                {
                    queryKeys = new List<string>(Request.QueryString.AllKeys);
                    if (queryKeys.Contains("connect"))
                    {
                        FireAuth();
                        oauthClient = new OAuth2Client((string)HttpContext.Current.Session["ClientID"], (string)HttpContext.Current.Session["ClientSecret"], oauth_callback_url, "sandbox");


                        List<OidcScopes> scopes = new List<OidcScopes>();
                        scopes.Add(OidcScopes.Accounting);
                        var authorizationRequest = oauthClient.GetAuthorizationURL(scopes);
                        Response.Redirect(authorizationRequest, false);
                    }
                    if (queryKeys.Contains("code"))
                    {
                        ReadToken();
                        DisposeSessions();

                    }
                }


            }

            catch (Exception exepction)
            {
            }
        }

        private async void ReadToken()
        {
            if (HttpContext.Current.Session["ClientID"] != null)
            {
                oauthClient = new OAuth2Client((string)HttpContext.Current.Session["ClientID"], (string)HttpContext.Current.Session["ClientSecret"], oauth_callback_url, "sandbox");
                AsyncMode = true;

                if (Request.QueryString.Count > 0)
                {
                    var response = new AuthorizeResponse(Request.QueryString.ToString());
                    if (response.State != null)
                    {


                        if (response.Code != null)
                        {

                            try
                            {
                                string tenant = HttpContext.Current.Session["tenant"] + "";
                                string token = HttpContext.Current.Request.Headers["Token"] + "";
                                var tokenResp = await oauthClient.GetBearerTokenAsync(response.Code);

                                if (Request.Url.Query == "")
                                {
                                    Response.Redirect(Request.RawUrl);
                                }
                                else
                                {
                                    this.SaveTokens(tokenResp.RefreshToken, response.RealmId, tenant, token);
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                        }


                    }

                    else
                    {
                        List<OidcScopes> scopes = new List<OidcScopes>();
                        scopes.Add(OidcScopes.Accounting);
                        var authorizationRequest = oauthClient.GetAuthorizationURL(scopes);
                        Response.Redirect(authorizationRequest, false);
                    }

                }

                else
                {

                    List<OidcScopes> scopes = new List<OidcScopes>();
                    scopes.Add(OidcScopes.Accounting);
                    var authorizationRequest = oauthClient.GetAuthorizationURL(scopes);
                    Response.Redirect(authorizationRequest, false);
                }


            }

        }


        private void SaveTokens(string RefreshToken, string realMeId, string tenant,string token)
        {

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                AccountingSettingQuery query = new AccountingSettingQuery(int.Parse(tenant));
                AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse(tenant));
                var TempQBORealMeId = entityPM.QBOrealMeID;
                entityPM.RefreshToken = RefreshToken;
                entityPM.QBOrealMeID = realMeId;
                entityPM.QBOOAuth = 2;
                if (!String.IsNullOrEmpty(TempQBORealMeId) && entityPM.QBOrealMeID != TempQBORealMeId)
                {
                    RunStoredProcedureClass.DeleteQBOTranslations(entityPM.Id);
                }
                List<AccountingSettingPM> LoggedEntities = query.GetAccountSettingPMs().Where(r => r.QBOrealMeID == entityPM.QBOrealMeID && r.Id != entityPM.Id).ToList();
                ICommonDataContext MyContext = CommonDataContext.GetContext(int.Parse(tenant));
                AccountingSettingService service = new AccountingSettingService(MyContext, int.Parse(tenant));
                service.UpdateWithToken(entityPM,token);

                foreach (AccountingSettingPM Item in LoggedEntities)
                {
                    Item.RefreshToken = RefreshToken;
                    Item.QBOrealMeID = realMeId;
                    service.UpdateWithToken(Item,token);
                }
                DisposeSessions();
                ClosePage();
                scope.Complete();
            }

        }

        private void DisposeSessions() {
               if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["realMeId"] = null;
                HttpContext.Current.Session["ClientID"] = null;
                HttpContext.Current.Session["tenant"] = null;
                HttpContext.Current.Session["ClientSecret"] = null;
                HttpContext.Current.Session["AuthCode"] = null;
                HttpContext.Current.Session["RefreshToken"] = null;
            }
}

    private void ClosePage()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/JavaScript'>window.close();</script>");

        }



        private void FireAuth()
        {
            string tenant = Request.QueryString["tenant"];
            HttpContext.Current.Session["tenant"] = tenant;
            SettingRepository mySettingRepository = new SettingRepository();
            Setting mySetting = mySettingRepository.GetSingleSetting("1");
            if (mySetting != null)
            {
                HttpContext.Current.Session["ClientID"] = mySetting.QBOClientID;
                HttpContext.Current.Session["ClientSecret"] = mySetting.QBOClientSecret;

            }


        }









    }

}