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
            TokenInput.Value = Request["Token"];
            LoginInput.Value = Request["LoginData"];

            bool isAuthenticated = HttpContext.Current.Request.IsAuthenticated || !string.IsNullOrEmpty(TokenInput.Value) ? true : false;

            if (!isAuthenticated)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }

        }
    }
}