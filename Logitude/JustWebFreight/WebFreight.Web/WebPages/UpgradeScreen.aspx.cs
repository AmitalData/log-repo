using System;
using System.Linq;

using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.WebPages
{
    public partial class UpgradeScreen : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            //IGlobalContext globalcontext = GlobalContext.GetContext();
            //bool isUpgrading = (from a in globalcontext.GlobalDBs
            //                    select a).FirstOrDefault().IsUpgrading;
            //if (!isUpgrading)
            //{
            //    Response.Redirect("Default.aspx");
            //}
        }
    }
}