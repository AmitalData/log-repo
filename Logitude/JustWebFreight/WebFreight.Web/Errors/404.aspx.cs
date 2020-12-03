using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class _404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var IsAngularURL = Request.RawUrl.Contains("/index.html");//.QueryString["Menu"];
                if (IsAngularURL)
                {
                    IGlobalContext objectContext = GlobalContext.GetContext();
                    SettingRepository MySettingRepository = new SettingRepository(objectContext);
                    SettingQuery MySettingQuery = new SettingQuery(MySettingRepository);
                    var MySettings = MySettingQuery.GetSinglePM();
                    if (MySettings.DeploymentStage == "logboxwe1")
                    { 
                        string RedirectUrl = "Angular" + MySettings.HtmlVersion + "/index.html";//?Menu=PREQ&SecurityKey=" + SecurityKey + "&Tenant=" + Tenant;
                        Response.Redirect("~/" + RedirectUrl);
                    }
                    else {
                        Response.Redirect("~/");
                    }

                }
                else
                {
                    HttpContext.Current.Response.StatusCode = 404;
                }
            } 
        }
 

        protected void GoToHomeClick(object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Response.Redirect("~");
            }
            catch (Exception)
            {
                 
            }
           
        }
    }
}