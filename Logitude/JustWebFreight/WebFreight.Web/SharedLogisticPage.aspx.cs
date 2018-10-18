using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class SharedLogisticPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string userdata = Request.QueryString["userdata"];

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }

        }
    }
}