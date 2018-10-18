using Dropbox.Api;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web
{
    public partial class DropBoxAuth : System.Web.UI.Page
    {
        private delegate void DoGetAccessToken(string code, string state, string uri, int tenant);
        //string code = "";
        //string state = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //SecurityUtility.RedirectToHttps();
            var state = Request.QueryString["state"];
            var code = Request.QueryString["code"];
            var uri = HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0];
            var tenant = 0;
            //if (Request.Cookies["CurrentTenant"] != null)
            //{
            //    tenant = int.Parse(Request.Cookies["CurrentTenant"].Value);
            //}

            //var Token = Request.QueryString["token"];
            if (!IsPostBack)
            {
                DoGetAccessToken myAction = new DoGetAccessToken((c, s, u, t) => GetToken(code, state, uri, tenant));
                //invoke it asynchrnously, control passes to next statement
                myAction.BeginInvoke(code, state, uri, tenant, null, null);
                //GetToken(code);
            }
            String x = "<script type='text/javascript'>self.close();</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
        }

        private async void GetToken(string code, string state, string uri, int tenant)
        {
            try
            {
                var temp = await DropboxOAuth2Helper.ProcessCodeFlowAsync(code, LogitudeSettings.DropboxAppKey, LogitudeSettings.DropboxAppSecret, uri);
                TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(tenant);
                var currentTenant = Repo.GetSingleTenantAdditionalDataByState(state);
                if (currentTenant != null)
                {
                    currentTenant.DropBoxAccessToken = temp.AccessToken;
                    currentTenant.DropBoxUID = temp.Uid;
                    Repo.Update(currentTenant);
                }
                else
                {
                    currentTenant = new TenantAdditionalData()
                    {
                        Tenant = tenant,
                        DropBoxUID = temp.Uid,
                        DropBoxState = temp.State,
                        DropBoxAccessToken = temp.AccessToken
                    };
                    Repo.Add(currentTenant);
                }
                Repo.SubmitChanges();
                try
                {
                    var path = "/ToLogitude";
                    DropBoxActionsHelper helper = new DropBoxActionsHelper(currentTenant.Tenant);
                    var AccountDetails = await helper.GetCurrentAccount();
                    currentTenant.DropBoxUEmail = AccountDetails.Email;
                    Repo.Update(currentTenant);
                    Repo.SubmitChanges();
                    var list = helper.ListFolder(path);
                    if (list.Result == null)
                    {
                        var folder = helper.CreateFolder(path);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("path/not_found/"))
                    {
                        var path = "/ToLogitude";
                        DropBoxActionsHelper helper = new DropBoxActionsHelper(currentTenant.Tenant);
                        var folder = helper.CreateFolder(path);
                    }
                }
                finally
                {
                   
                }
                
                //var path = "/ToLogitude";
                //DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
                //await helper.Upload(path, "text.txt", "Test To Logitude");
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "DropBoxAuth", "", null);
            }
        }
    }
}