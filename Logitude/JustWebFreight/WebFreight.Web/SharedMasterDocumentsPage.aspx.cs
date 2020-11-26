using System;
using System.Web;

namespace WebFreight.Web
{
    public partial class SharedMasterDocumentsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userdata = Request.QueryString["securitykey"];

            if (userdata == null)
            {
                CheckIfUserAuthenticated();
            }
        }

        private static void CheckIfUserAuthenticated()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
        }
    }
}