using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.GlobalModel;

namespace WebFreight.Web
{
    public partial class QuickbooksOnline : System.Web.UI.Page
    {

        public String oauth_token;
        public String oauth_tokenCallBack;
        public static String REQUEST_TOKEN_URL = "https://oauth.intuit.com/oauth/v1/get_request_token";
        public static String ACCESS_TOKEN_URL = "https://oauth.intuit.com/oauth/v1//get_access_token";
        public static String AUTHORIZE_URL = "https://appcenter.intuit.com/Connect/Begin";
        public static String OAUTH_URL = "https://oauth.intuit.com/oauth/v1";
        public String consumerKey = "";
        public String consumerSecret = "";
        public string strrequestToken = string.Empty;
        public string tokenSecret = string.Empty;
        public string oauth_callback_url = LogitudeSettings.LogitudeURL + "/QuickbooksOnline.aspx?";
        public string GrantUrl = LogitudeSettings.LogitudeURL + "/QuickbooksOnline.aspx?connect=true";
        public List<string> queryKeys;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (LogitudeSettings.DeploymentStage != "Dev")
                {
                    Security.SecurityUtility.RedirectToHttps();
                }


                if (Request.QueryString.Count > 0)
                {
                    queryKeys = new List<string>(Request.QueryString.AllKeys);
                    if (queryKeys.Contains("connect"))
                    {
                        FireAuth();
                    }
                    if (queryKeys.Contains("oauth_token"))
                    {
                        ReadToken();
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["accessToken"] == null && HttpContext.Current.Session["accessTokenSecret"] == null)
                    {
                        //
                        //
                    }
                    else
                    {
                        //Disconnect();
                        HttpContext.Current.Session["accessToken"] = null;
                        HttpContext.Current.Session["accessTokenSecret"] = null;
                    }
                }
            }

            catch(Exception exepction)
            {
            }

         

        }


        private void ReadToken()
        {


            HttpContext.Current.Session["oauthToken"] = Request.QueryString["oauth_token"].ToString(); ;
            HttpContext.Current.Session["oauthVerifyer"] = Request.QueryString["oauth_verifier"].ToString();
            HttpContext.Current.Session["realm"] = Request.QueryString["realmId"].ToString();
            HttpContext.Current.Session["dataSource"] = Request.QueryString["dataSource"].ToString();
            //Stored in a session for demo purposes.
            //Production applications should securely store the Access Token
            getAccessToken();
        }


        public string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }


        private void getAccessToken()
        {
            IOAuthSession clientSession = CreateSession();
            if (clientSession != null)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    
                IToken accessToken = clientSession.ExchangeRequestTokenForAccessToken((IToken)HttpContext.Current.Session["requestToken"], HttpContext.Current.Session["oauthVerifyer"].ToString());
                    HttpContext.Current.Session["accessToken"] = accessToken.Token;
                    HttpContext.Current.Session["accessTokenSecret"] = accessToken.TokenSecret;
                    AccountingSettingQuery query = new AccountingSettingQuery(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                    AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                    var TempQBORealMeId = entityPM.QBOrealMeID;
                    entityPM.QBOAccessToken = accessToken.Token;
                    entityPM.QBOAccessTokenSecret = accessToken.TokenSecret;
                    entityPM.QBOrealMeID = HttpContext.Current.Session["realm"].ToString();
                    if (!String.IsNullOrEmpty(TempQBORealMeId) && entityPM.QBOrealMeID != TempQBORealMeId)
                    {
                        RunStoredProcedureClass.DeleteQBOTranslations(entityPM.Id);
                    }
                    List<AccountingSettingPM> LoggedEntities = query.GetAccountSettingPMs().Where(r => r.QBOrealMeID == entityPM.QBOrealMeID && r.Id!=entityPM.Id).ToList();
                    ICommonDataContext MyContext = CommonDataContext.GetContext(int.Parse((HttpContext.Current.Session["tenant"] + "")));
                    AccountingSettingService service = new AccountingSettingService(MyContext, int.Parse((HttpContext.Current.Session["tenant"] + "")));
                    service.Update(entityPM);

                    foreach(AccountingSettingPM Item in LoggedEntities)
                    {
                        Item.QBOAccessToken = accessToken.Token;
                        Item.QBOAccessTokenSecret = accessToken.TokenSecret;
                        Item.QBOrealMeID = HttpContext.Current.Session["realm"].ToString();
                        service.Update(Item);
                    }
                    DisposeSessions();
                    scope.Complete();
                }
            }
        }

        private void DisposeSessions()
        {
            HttpContext.Current.Session["consumerKey"] = null;
            HttpContext.Current.Session["consumerSecret"] = null;
            HttpContext.Current.Session["tenant"] = null;
            HttpContext.Current.Session["requestToken"] = null;
            HttpContext.Current.Session["OAuthAccessToken"] = null;
            HttpContext.Current.Session["oauthLink"] = null;
            HttpContext.Current.Session["oauthVerifyer"] = null;
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/JavaScript'>window.close();</script>");
        }



        private void FireAuth()
        {

            string tenant = Request.QueryString["tenant"];
            SettingRepository mySettingRepository = new SettingRepository();
            Setting mySetting = mySettingRepository.GetSingleSetting("1");

            HttpContext.Current.Session["consumerKey"] = mySetting.QBOConsumerKey;
            HttpContext.Current.Session["consumerSecret"] = mySetting.QBOConsumerSecretKey;
            HttpContext.Current.Session["tenant"] = tenant;
            CreateAuthorization();
            IToken token = (IToken)HttpContext.Current.Session["requestToken"];
            if (token != null)
            {
                tokenSecret = token.TokenSecret;
                strrequestToken = token.Token;
            }
        }


        protected void CreateAuthorization()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


            IOAuthSession session = CreateSession();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            if (session != null)
            {
                IToken requestToken = session.GetRequestToken();
                HttpContext.Current.Session["requestToken"] = requestToken;
                tokenSecret = requestToken.TokenSecret;
                var authUrl = string.Format("{0}?oauth_token={1}&oauth_callback={2}", AUTHORIZE_URL, requestToken.Token, UriUtility.UrlEncode(oauth_callback_url));
                oauth_token = string.Format("{1}", AUTHORIZE_URL, requestToken.Token, UriUtility.UrlEncode(oauth_callback_url));
                oauth_tokenCallBack = string.Format("{2}", AUTHORIZE_URL, requestToken.Token, UriUtility.UrlEncode(oauth_callback_url));
                HttpContext.Current.Session["OAuthAccessToken"] = oauth_token;

                HttpContext.Current.Session["oauthLink"] = authUrl;
                Response.Redirect(authUrl);
            }
        }


        protected IOAuthSession CreateSession()
        {
            try
            {
                string ConsumerKey = (string)HttpContext.Current.Session["consumerKey"];
                string consumerSecret = (string)HttpContext.Current.Session["consumerSecret"];

                if (string.IsNullOrEmpty(consumerSecret) || string.IsNullOrEmpty(ConsumerKey))
                    throw new ApplicationException("No Keys");
                    var consumerContext = new OAuthConsumerContext
                {
                    ConsumerKey = HttpContext.Current.Session["consumerKey"].ToString(),
                    ConsumerSecret = HttpContext.Current.Session["consumerSecret"].ToString(),
                    SignatureMethod = SignatureMethod.HmacSha1
                };
                return new OAuthSession(consumerContext,
                                        REQUEST_TOKEN_URL,
                                       OAUTH_URL,
                                        ACCESS_TOKEN_URL);
            }

            catch (Exception e)
            {

                return null;
               
            }


        }




    }

}