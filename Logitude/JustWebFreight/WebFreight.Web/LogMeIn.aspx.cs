using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Security;

namespace WebFreight.Web
{
    public partial class LogMeIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityUtility.RedirectToHttps();
        }
    }
}