using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class LinksGateway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //http://localhost:9996/LinksGateway.aspx?Menu=PREQ&SecurityKey=cfeb18b0f2044ccab43bb1d2cc67048e&Tenant=203
            var Menu = Request.QueryString["Menu"];
            if (Menu == "PREQ")
            {
                var Tenant = Request.QueryString["Tenant"];
                var SecurityKey = Request.QueryString["SecurityKey"]; 
                IGlobalContext objectContext = GlobalContext.GetContext();
                SettingRepository MySettingRepository = new SettingRepository(objectContext);
                SettingQuery MySettingQuery = new SettingQuery(MySettingRepository);
                var MySettings = MySettingQuery.GetSinglePM();
                string RedirectUrl = "Angular" + MySettings.HtmlVersion + "/index.html?Menu=PREQ&SecurityKey=" + SecurityKey + "&Tenant=" + Tenant;
                Response.Redirect("~/" + RedirectUrl);
            }
           
        }
        
    }
}