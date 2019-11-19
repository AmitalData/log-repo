
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

                //if (LogitudeSettings.DeploymentStage != "Dev")
                //{
                //    Security.SecurityUtility.RedirectToHttps(false);
                //}
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
                    }
                }


            }

            catch (Exception exepction)
            {
            }
        }

        private void ReadToken()
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

                        if (response.RealmId != null)
                        {
                            HttpContext.Current.Session["realMeId"] = response.RealmId;
                        }

                        if (response.Code != null)
                        {
                            HttpContext.Current.Session["AuthCode"] = response.Code;
                            PageAsyncTask t = new PageAsyncTask(PerformCodeExchange);
                            Page.RegisterAsyncTask(t);
                            Page.ExecuteRegisteredAsyncTasks();
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

        public async System.Threading.Tasks.Task PerformCodeExchange()
        {
            try
            {
                var tokenResp = await oauthClient.GetBearerTokenAsync((string)HttpContext.Current.Session["AuthCode"]);
                HttpContext.Current.Session["RefreshToken"] = tokenResp.RefreshToken;

                if (Request.Url.Query == "")
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    this.SaveTokens();
                }
            }
            catch (Exception ex)
            {
            }
        }





        private void SaveTokens()
        {

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                AccountingSettingQuery query = new AccountingSettingQuery(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                var TempQBORealMeId = entityPM.QBOrealMeID;
                entityPM.RefreshToken = HttpContext.Current.Session["RefreshToken"].ToString();
                entityPM.QBOrealMeID = HttpContext.Current.Session["realMeId"].ToString();
                entityPM.QBOOAuth = 2;
                if (!String.IsNullOrEmpty(TempQBORealMeId) && entityPM.QBOrealMeID != TempQBORealMeId)
                {
                    RunStoredProcedureClass.DeleteQBOTranslations(entityPM.Id);
                }
                List<AccountingSettingPM> LoggedEntities = query.GetAccountSettingPMs().Where(r => r.QBOrealMeID == entityPM.QBOrealMeID && r.Id != entityPM.Id).ToList();
                ICommonDataContext MyContext = CommonDataContext.GetContext(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                AccountingSettingService service = new AccountingSettingService(MyContext, int.Parse((HttpContext.Current.Session["tenant"] + "")));
                service.Update(entityPM);

                foreach (AccountingSettingPM Item in LoggedEntities)
                {
                    Item.RefreshToken = HttpContext.Current.Session["RefreshToken"].ToString();
                    Item.QBOrealMeID = HttpContext.Current.Session["realMeId"].ToString();
                    service.Update(Item);
                }
                DisposeSessions();
                scope.Complete();
            }

        }

        private void DisposeSessions()
        {
            HttpContext.Current.Session["realMeId"] = null;
            HttpContext.Current.Session["ClientID"] = null;
            HttpContext.Current.Session["tenant"] = null;
            HttpContext.Current.Session["ClientSecret"] = null;
            HttpContext.Current.Session["AuthCode"] = null;
            HttpContext.Current.Session["RefreshToken"] = null;
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