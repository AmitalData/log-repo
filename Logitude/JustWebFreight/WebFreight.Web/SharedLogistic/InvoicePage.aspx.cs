using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.SharedLogistic
{
    public partial class InvoicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string securitykey = Request.QueryString["securitykey"];

            if (string.IsNullOrEmpty(securitykey))
            {
                TokenInput.Value = Request["Token"];
                LoginInput.Value = Request["LoginData"];

                bool isAuthenticated = HttpContext.Current.Request.IsAuthenticated || !string.IsNullOrEmpty(TokenInput.Value) ? true : false;

                if (!isAuthenticated)
                {
                    HttpContext.Current.Response.Redirect("../login.aspx");
                }
            }
        }
    }
}