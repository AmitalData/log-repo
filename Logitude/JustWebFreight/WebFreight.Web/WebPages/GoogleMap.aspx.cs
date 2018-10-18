using System;

namespace WebFreight.Web.WebPages
{
    public partial class GoogleMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string address = Request.QueryString["address"];
            if (!string.IsNullOrEmpty(address))
            {

                addressinput.Value = address;

                //if (!ClientScript.IsStartupScriptRegistered("alert"))
                //{
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "FindLocaiton", "FindLocaiton();", true);
                //}
            }
            else
            {
                Response.Write("Error in loading address");
            }
        }
    }
}