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
            this.InitStoredItems();

            bool isAuthenticated = HttpContext.Current.Request.IsAuthenticated || !string.IsNullOrEmpty(TokenInput.Value) ? true : false;

            if (!isAuthenticated)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
        }

        private void InitStoredItems()
        {
            string token = Request["Token"];
            string loginData = Request["LoginData"];

            if (string.IsNullOrEmpty(token))
            {
                token = this.GetSessionValue("Token");
            }

            else
            {
                this.Session.Add("Token", token);
            }

            if (string.IsNullOrEmpty(loginData))
            {
                loginData = this.GetSessionValue("LoginData");
            }

            else
            {
                this.Session.Add("LoginData", loginData);
            }

            TokenInput.Value = token;
            LoginInput.Value = loginData;
        }

        private string GetSessionValue(string itemKey)
        {
            string output = null;

            foreach (string key in Session.Keys)
            {
                if (key == itemKey)
                {
                    if(Session[key] != null)
                    {
                        output = Session[key].ToString();
                        break;
                    }
                }

            }

            return output;
        }
    }
}